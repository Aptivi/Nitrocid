//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using System.Diagnostics;
using Terminaux.Reader;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Users;
using Nitrocid.Users.Login;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Drivers;
using Nitrocid.Kernel.Threading;
using Nitrocid.Drivers.RNG;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Misc.Splash;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Events;
using Terminaux.Base.Buffered;
using Nitrocid.Kernel.Power;
using Nitrocid.Misc.Screensaver.Displays;
using Terminaux.Base;
using Nitrocid.Kernel;
using System.Linq;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs;

namespace Nitrocid.Misc.Screensaver
{
    /// <summary>
    /// Screensaver management module
    /// </summary>
    public static class ScreensaverManager
    {

        // Private variables
        internal static Dictionary<string, BaseScreensaver> Screensavers = new()
        {
            { "matrixbleed", new MatrixBleedDisplay() },
            { "plain", new PlainDisplay() }
        };
        internal static Dictionary<string, BaseScreensaver> AddonSavers = [];
        internal static Dictionary<string, BaseScreensaver> CustomSavers = [];
        internal static bool scrnTimeoutEnabled = true;
        internal static TimeSpan scrnTimeout = new(0, 5, 0);
        internal static bool LockMode;
        internal static bool inSaver;
        internal static bool screenRefresh;
        internal static bool ScrnTimeReached;
        internal static bool seizureAcknowledged;
        internal static bool noLock;
        internal static bool bailing;
        internal static AutoResetEvent SaverAutoReset = new(false);
        internal static KernelThread Timeout = new("Screensaver timeout thread", false, HandleTimeout) { isCritical = true };

        /// <summary>
        /// Whether the kernel is on the screensaver mode
        /// </summary>
        public static bool InSaver =>
            inSaver;

        /// <summary>
        /// Whether the screensaver is stopping or not
        /// </summary>
        public static bool Bailing =>
            bailing;

        /// <summary>
        /// Screen timeout interval
        /// </summary>
        public static TimeSpan ScreenTimeout
        {
            get
            {
                if (!TimeSpan.TryParse(Config.MainConfig.ScreenTimeout, out TimeSpan timeout))
                    return new(0, 5, 0);
                return timeout;
            }
        }

        /// <summary>
        /// Default screensaver name
        /// </summary>
        public static string DefaultSaverName =>
            GetScreensaverNames().Contains(Config.MainConfig.DefaultSaverName) ? Config.MainConfig.DefaultSaverName : "matrixbleed";

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
        /// Gets the screensaver instance
        /// </summary>
        /// <param name="saver">Saver name</param>
        /// <returns>An instance of <see cref="BaseScreensaver"/> that represents the selected screensaver</returns>
        public static BaseScreensaver GetScreensaver(string saver)
        {
            // Check to see if there is a screensaver with this name
            if (!IsScreensaverRegistered(saver))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "{0} is not found.", vars: [saver]);
                throw new KernelException(KernelExceptionType.NoSuchScreensaver, Translate.DoTranslation("Screensaver {0} not found in database. Check the name and try again."), saver);
            }

            // Now, get the screensaver instance
            var BaseSaver =
                AddonSavers.TryGetValue(saver, out BaseScreensaver? @base) ? @base :
                CustomSavers.TryGetValue(saver, out BaseScreensaver? customBase) ? customBase :
                Screensavers[saver];
            return BaseSaver;
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
                // Select a random screensaver if asked
                bool randomProvided = saver.Equals("random", StringComparison.OrdinalIgnoreCase);
                saver = randomProvided ? SelectRandomScreensaver() : saver.ToLower();

                // Check to see if the scrensaver exists
                EventsManager.FireEvent(EventType.PreShowScreensaver);
                DebugWriter.WriteDebug(DebugLevel.I, "Requested screensaver: {0}", vars: [saver]);
                if (!IsScreensaverRegistered(saver))
                {
                    TextWriters.Write(Translate.DoTranslation("The requested screensaver {0} is not found."), true, KernelColorType.Error, saver);
                    DebugWriter.WriteDebug(DebugLevel.I, "Screensaver {0} not found in the dictionary.", vars: [saver]);
                    return;
                }

                // Now, launch the screensaver
                screenRefresh = true;
                if (IsScreensaverRegistered(saver))
                {
                    // Get the base screensaver
                    var BaseSaver =
                        AddonSavers.TryGetValue(saver, out BaseScreensaver? @base) ? @base :
                        CustomSavers.TryGetValue(saver, out BaseScreensaver? customBase) ? customBase :
                        Screensavers[saver];

                    // Show the seizure warning if required
                    if (BaseSaver.ScreensaverContainsFlashingImages && !noSeizureWarning && !randomProvided)
                        BaseSaver.ScreensaverSeizureWarning();

                    // Start the screensaver thread
                    inSaver = true;
                    ScrnTimeReached = true;
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Start(BaseSaver);

                    // Play the ambience sound if needed
                    if (Config.MainConfig.EnableAmbientSoundFx)
                        ScreensaverDisplayer.ScreensaverAmbienceThread.Start();
                    DebugWriter.WriteDebug(DebugLevel.I, "{0} started", vars: [saver]);
                }
            }
            catch (Exception ex)
            {
                TextWriters.Write(Translate.DoTranslation("Error when trying to start screensaver:") + " {0}", true, KernelColorType.Error, ex.Message);
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
                InputEventInfo eventInfo = Input.ReadPointerOrKey();
                EventsManager.FireEvent(EventType.PreUnlock, DefaultSaverName);

                // Bail from screensaver and optionally prompt for password
                ScreensaverDisplayer.BailFromScreensaver();

                // When getting out of the lock screen by pressing ENTER when lockscreen is invoked, we need to make sure
                // that we don't write the shell prompt twice. Furthermore, when we use the mouse to generate click events,
                // we need to make sure that we ignore the Clicked event and listen to the Released event to ensure that
                // there are no more mouse events left, or the screensaver would exit instantly, causing info to be displayed
                // longer than the set duration.
                if (eventInfo.PointerEventContext is not null && eventInfo.PointerEventContext.ButtonPress == PointerButtonPress.Clicked)
                {
                    while (eventInfo.PointerEventContext is not null && eventInfo.PointerEventContext.ButtonPress != PointerButtonPress.Released)
                        eventInfo = Input.ReadPointerOrKey(InputEventType.Mouse);
                }

                // Now, show the password prompt
                if (Config.MainConfig.PasswordLock)
                    Login.ShowPasswordPrompt(UserManagement.CurrentUser.Username);

                // Render the current screen
                if (ScreenTools.CurrentScreen is not null)
                    ScreenTools.Render();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to lock screen: {0}", vars: [ex.Message]);
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
                DebugWriter.WriteDebug(DebugLevel.W, "{0} is not found.", vars: [saver]);
                throw new KernelException(KernelExceptionType.NoSuchScreensaver, Translate.DoTranslation("Screensaver {0} not found in database. Check the name and try again."), saver);
            }

            // Now, set the default screensaver.
            saver = saver.ToLower();
            DebugWriter.WriteDebug(DebugLevel.I, "{0} is found. Setting it to default...", vars: [saver]);
            Config.MainConfig.DefaultSaverName = saver;
            Config.CreateConfig();
        }

        /// <summary>
        /// Gets a screensaver instance from loaded assembly
        /// </summary>
        /// <param name="Assembly">An assembly</param>
        public static BaseScreensaver? GetScreensaverInstance(Assembly Assembly)
        {
            foreach (Type t in Assembly.GetTypes())
            {
                if (t.GetInterface(typeof(IScreensaver).Name) is not null)
                    return (BaseScreensaver?)Assembly.CreateInstance(t.FullName ?? "");
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
                CustomSavers.ContainsKey(name);
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
        /// Delays the screensaver thread until either the unified screensaver delay period or the specified delay period.
        /// </summary>
        /// <param name="delay">Delay period (usually a configured value from individual screensaver settings)</param>
        /// <param name="force">Whether to force use your specified period delay period or not</param>
        /// <remarks>
        /// It also returns when either the screensaver displayer thread is told to stop or the console is resized.
        /// </remarks>
        public static void Delay(int delay, bool force = false)
        {
            int finalDelay = Config.MainConfig.ScreensaverUnifiedDelay && !force ? Config.MainConfig.ScreensaverDelay : delay;
            SpinWait.SpinUntil(() =>
                ScreensaverDisplayer.ScreensaverDisplayerThread.IsStopping ||
                ConsoleResizeHandler.WasResized(false)
            , finalDelay);
        }

        /// <summary>
        /// Screensaver error handler
        /// </summary>
        internal static void HandleSaverError(Exception Exception, bool initialVisible)
        {
            if (Exception is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Screensaver experienced an error: {0}.", vars: [Exception.Message]);
                DebugWriter.WriteDebugStackTrace(Exception);
                HandleSaverCancel(initialVisible);
                TextWriters.Write(Translate.DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), true, KernelColorType.Error, Exception.Message);
            }
        }

        /// <summary>
        /// Screensaver cancellation handler
        /// </summary>
        internal static void HandleSaverCancel(bool initialVisible)
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Cancellation is pending. Cleaning everything up...");
            KernelColorTools.LoadBackground();
            ConsoleWrapper.CursorVisible = initialVisible;
            DebugWriter.WriteDebug(DebugLevel.I, "All clean. Screensaver stopped.");
            SaverAutoReset.Set();
        }

        internal static void StartTimeout()
        {
            if (!Timeout.IsAlive && scrnTimeoutEnabled && KernelPlatform.IsOnUsualEnvironment())
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
                SpinWait.SpinUntil(() => SplashReport.KernelBooted);
                while (!PowerManager.KernelShutdown)
                {
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
                            hasMoved = ConsoleWrapper.MovementDetected;
                            return hasMoved || PowerManager.KernelShutdown;
                        }, ScreenTimeout);

                        // Check to see if we're locking
                        bool locking = !hasMoved && stopwatch.Elapsed >= ScreenTimeout;
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
                DebugWriter.WriteDebug(DebugLevel.E, "Shutting down screensaver timeout thread: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        private static string SelectRandomScreensaver()
        {
            static string SelectRandom()
            {
                var savers = GetScreensaverNames();
                int ScreensaverIndex = RandomDriver.RandomIdx(savers.Length);
                string ScreensaverName = savers[ScreensaverIndex];
                return ScreensaverName;
            }

            // Get a random screensaver name
            string ScreensaverName = SelectRandom();
            return ScreensaverName.ToLower();
        }

    }
}
