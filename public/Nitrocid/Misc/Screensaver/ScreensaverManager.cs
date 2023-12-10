//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver.Displays;
using KS.Users.Login;
using KS.Kernel.Events;
using KS.ConsoleBase.Colors;
using KS.Drivers.Console;
using KS.Drivers;
using KS.Users;
using System.Diagnostics;
using KS.ConsoleBase;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using Terminaux.Reader;
using KS.Kernel.Power;
using KS.ConsoleBase.Buffered;
using KS.Misc.Splash;
using KS.ConsoleBase.Inputs;

namespace KS.Misc.Screensaver
{
    /// <summary>
    /// Screensaver management module
    /// </summary>
    public static class ScreensaverManager
    {

        // Private variables
        internal static Dictionary<string, BaseScreensaver> Screensavers = new()
        {
            { "matrix", new MatrixDisplay() },
            { "matrixbleed", new MatrixBleedDisplay() },
            { "plain", new PlainDisplay() }
        };
        internal static Dictionary<string, BaseScreensaver> AddonSavers = [];
        internal static Dictionary<string, BaseScreensaver> CustomSavers = [];
        internal static bool scrnTimeoutEnabled = true;
        internal static int scrnTimeout = 300000;
        internal static string defSaverName = "matrixbleed";
        internal static bool LockMode;
        internal static bool inSaver;
        internal static bool screenRefresh;
        internal static bool ScrnTimeReached;
        internal static bool seizureAcknowledged;
        internal static bool noLock;
        internal static AutoResetEvent SaverAutoReset = new(false);
        internal static KernelThread Timeout = new("Screensaver timeout thread", false, HandleTimeout);

        // Public Variables
        /// <summary>
        /// Screensaver debugging
        /// </summary>
        public static bool ScreensaverDebug =>
            Config.MainConfig.ScreensaverDebug;

        /// <summary>
        /// Password lock enabled
        /// </summary>
        public static bool PasswordLock =>
            Config.MainConfig.PasswordLock;

        /// <summary>
        /// Whether the kernel is on the screensaver mode
        /// </summary>
        public static bool InSaver =>
            inSaver;

        /// <summary>
        /// Screen timeout in milliseconds
        /// </summary>
        public static int ScreenTimeout =>
            Config.MainConfig.ScreenTimeout;

        /// <summary>
        /// Default screensaver name
        /// </summary>
        public static string DefaultSaverName =>
            Config.MainConfig.DefaultSaverName;

        /// <summary>
        /// Whether the screen refresh is required
        /// </summary>
        public static bool ScreenRefreshRequired
        {
            get
            {
                bool refresh = screenRefresh;
                if (refresh)
                {
                    screenRefresh = false;
                    SpinWait.SpinUntil(() => !inSaver);
                }
                return refresh;
            }
        }

        /// <summary>
        /// Gets the name of the screensavers
        /// </summary>
        public static string[] GetScreensaverNames()
        {
            List<string> savers = [.. Screensavers.Keys, .. AddonSavers.Keys, .. CustomSavers.Keys];
            return [.. savers];
        }

        /// <summary>
        /// Shows the screensaver
        /// </summary>
        public static void ShowSavers() =>
            ShowSavers(DefaultSaverName, seizureAcknowledged);

        /// <summary>
        /// Shows the screensaver
        /// </summary>
        /// <param name="saver">A specified screensaver</param>
        public static void ShowSavers(string saver) =>
            ShowSavers(saver, seizureAcknowledged);

        /// <summary>
        /// Shows the screensaver
        /// </summary>
        /// <param name="saver">A specified screensaver</param>
        /// <param name="noSeizureWarning">Whether to prevent the seizure warning from showing up</param>
        public static void ShowSavers(string saver, bool noSeizureWarning)
        {
            try
            {
                // Check to see if the scrensaver exists
                EventsManager.FireEvent(EventType.PreShowScreensaver);
                DebugWriter.WriteDebug(DebugLevel.I, "Requested screensaver: {0}", saver);
                if (!IsScreensaverRegistered(saver))
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("The requested screensaver {0} is not found."), true, KernelColorType.Error, saver);
                    DebugWriter.WriteDebug(DebugLevel.I, "Screensaver {0} not found in the dictionary.", saver);
                    return;
                }

                // Now, judge how to launch the screensaver
                screenRefresh = true;
                string saverName = saver.ToLower();
                if (saverName == "random")
                {
                    // Random screensaver selection function
                    static string SelectRandom()
                    {
                        var savers = GetScreensaverNames();
                        int ScreensaverIndex = RandomDriver.RandomIdx(savers.Length);
                        string ScreensaverName = savers[ScreensaverIndex];
                        return ScreensaverName;
                    }

                    // Get a random screensaver name
                    int ScreensaverIndex = RandomDriver.RandomIdx(Screensavers.Count);
                    string ScreensaverName = SelectRandom();

                    // We don't want another "random" screensaver showing up, so keep selecting until it's no longer "random"
                    while (ScreensaverName == "random")
                        ScreensaverName = SelectRandom();

                    // Run the screensaver
                    ShowSavers(ScreensaverName, true);
                }
                else if (IsScreensaverRegistered(saverName))
                {
                    saver = saverName;
                    var BaseSaver =
                        AddonSavers.TryGetValue(saver, out BaseScreensaver @base) ? @base :
                        CustomSavers.TryGetValue(saver, out BaseScreensaver customBase) ? customBase :
                        Screensavers[saver];
                    if (BaseSaver.ScreensaverContainsFlashingImages && !noSeizureWarning)
                        BaseSaver.ScreensaverSeizureWarning();
                    inSaver = true;
                    ScrnTimeReached = true;
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Start(BaseSaver);
                    DebugWriter.WriteDebug(DebugLevel.I, "{0} started", saver);
                }
            }
            catch (InvalidOperationException ex)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Error when trying to start screensaver, because of an invalid operation."), true, KernelColorType.Error);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            catch (Exception ex)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Error when trying to start screensaver:") + " {0}", true, KernelColorType.Error, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Locks the screen. The password will be required when unlocking, depending on the kernel settings.
        /// </summary>
        public static void LockScreen()
        {
            LockMode = true;
            try
            {
                // Show the screensaver and wait for input
                ShowSavers();
                EventsManager.FireEvent(EventType.PreUnlock, DefaultSaverName);
                SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable);

                // Bail from screensaver and optionally prompt for password
                ScreensaverDisplayer.BailFromScreensaver();

                // When getting out of the lock screen by pressing ENTER when lockscreen is invoked, we need to make sure
                // that we don't write the shell prompt twice.
                if (ConsoleWrapper.KeyAvailable)
                    Input.DetectKeypressUnsafe();

                // Now, show the password prompt
                if (PasswordLock)
                    Login.ShowPasswordPrompt(UserManagement.CurrentUser.Username);

                // Render the current screen
                if (ScreenTools.CurrentScreen is not null)
                    ScreenTools.Render();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to lock screen: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            LockMode = false;
        }

        /// <summary>
        /// Sets the default screensaver
        /// </summary>
        /// <param name="saver">Specified screensaver</param>
        public static void SetDefaultScreensaver(string saver)
        {
            // Check to see if there is a screensaver with this name
            if (!IsScreensaverRegistered(saver))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "{0} is not found.", saver);
                throw new KernelException(KernelExceptionType.NoSuchScreensaver, Translate.DoTranslation("Screensaver {0} not found in database. Check the name and try again."), saver);
            }

            // Now, set the default screensaver.
            saver = saver.ToLower();
            DebugWriter.WriteDebug(DebugLevel.I, "{0} is found. Setting it to default...", saver);
            Config.MainConfig.DefaultSaverName = saver;
            Config.CreateConfig();
        }

        /// <summary>
        /// Gets a screensaver instance from loaded assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        public static BaseScreensaver GetScreensaverInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(IScreensaver).Name) is not null)
                    return (BaseScreensaver)Assembly.CreateInstance(t.FullName);
            }
            return null;
        }

        /// <summary>
        /// Is the screensaver registered?
        /// </summary>
        /// <param name="name">The name of the screensaver to query</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool IsScreensaverRegistered(string name)
        {
            name = name.ToLower();
            return
                Screensavers.ContainsKey(name) ||
                AddonSavers.ContainsKey(name) ||
                CustomSavers.ContainsKey(name) ||
                name == "random";
        }

        /// <summary>
        /// Registers a custom screensaver
        /// </summary>
        /// <param name="name">Screensaver name to register</param>
        /// <param name="screensaver">Base screensaver containing custom screensaver</param>
        public static void RegisterCustomScreensaver(string name, BaseScreensaver screensaver)
        {
            if (IsScreensaverRegistered(name))
                throw new KernelException(KernelExceptionType.ScreensaverManagement, Translate.DoTranslation("Custom screensaver already exists."));

            // Add a custom screensaver to the list of available screensavers.
            CustomSavers.Add(name.ToLower(), screensaver);
        }

        /// <summary>
        /// Unregisters a custom screensaver
        /// </summary>
        /// <param name="name">Screensaver name to unregister</param>
        public static void UnregisterCustomScreensaver(string name)
        {
            if (!IsScreensaverRegistered(name))
                throw new KernelException(KernelExceptionType.ScreensaverManagement, Translate.DoTranslation("Custom screensaver not found."));

            // Remove a custom screensaver from the list of available screensavers.
            CustomSavers.Remove(name.ToLower());
        }

        /// <summary>
        /// Prevents screen lock
        /// </summary>
        public static void PreventLock()
        {
            if (!scrnTimeoutEnabled)
                return;
            if (InSaver)
                ScreensaverDisplayer.BailFromScreensaver();
            DebugWriter.WriteDebug(DebugLevel.I, "Placing screensaver lock...");
            noLock = true;
        }

        /// <summary>
        /// Allows screen lock
        /// </summary>
        public static void AllowLock()
        {
            if (!scrnTimeoutEnabled)
                return;
            DebugWriter.WriteDebug(DebugLevel.I, "Releasing screensaver lock...");
            noLock = false;
        }

        /// <summary>
        /// Screensaver error handler
        /// </summary>
        internal static void HandleSaverError(Exception Exception, bool initialVisible)
        {
            if (Exception is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Screensaver experienced an error: {0}.", Exception.Message);
                DebugWriter.WriteDebugStackTrace(Exception);
                HandleSaverCancel(initialVisible);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), true, KernelColorType.Error, Exception.Message);
            }
        }

        /// <summary>
        /// Screensaver cancellation handler
        /// </summary>
        internal static void HandleSaverCancel(bool initialVisible)
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Cancellation is pending. Cleaning everything up...");
            KernelColorTools.LoadBack();
            ConsoleWrapper.CursorVisible = initialVisible;
            DebugWriter.WriteDebug(DebugLevel.I, "All clean. Screensaver stopped.");
            SaverAutoReset.Set();
        }

        internal static void StartTimeout()
        {
            if (!Timeout.IsAlive && scrnTimeoutEnabled)
                Timeout.Start();
        }

        internal static void StopTimeout()
        {
            if (Timeout.IsAlive)
                Timeout.Stop();
        }

        /// <summary>
        /// Handles the screensaver time so that when it reaches the time threshold, the screensaver launches
        /// </summary>
        private static void HandleTimeout()
        {
            try
            {
                var termDriver = DriverHandler.GetFallbackDriver<IConsoleDriver>();
                SpinWait.SpinUntil(() => SplashReport.KernelBooted);
                while (!PowerManager.KernelShutdown)
                {
                    int OldCursorLeft = termDriver.CursorLeft;
                    SpinWait.SpinUntil(() => !noLock);
                    SpinWait.SpinUntil(() => !ScrnTimeReached || PowerManager.KernelShutdown || noLock);
                    if (!ScrnTimeReached)
                    {
                        // Start the stopwatch for monitoring
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        // Detect movement
                        bool hasMoved = false;
                        SpinWait.SpinUntil(() =>
                        {
                            hasMoved = termDriver.MovementDetected;
                            return hasMoved || PowerManager.KernelShutdown;
                        }, ScreenTimeout);

                        // Check to see if we're locking
                        bool locking = !hasMoved && stopwatch.ElapsedMilliseconds >= ScreenTimeout;
                        stopwatch.Reset();
                        if (PowerManager.KernelShutdown || PowerManager.RebootRequested)
                            break;
                        else if (locking)
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Screen time has reached.");
                            TermReaderTools.Interrupt();
                            LockScreen();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Shutting down screensaver timeout thread: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

    }
}
