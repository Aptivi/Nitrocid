
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Screensaver.Displays;
using KS.Users.Login;
using KS.Kernel.Events;
using KS.ConsoleBase.Colors;
using KS.Drivers.Console;
using KS.Drivers;
using System.Linq;
using KS.Users;
using System.Diagnostics;
using KS.ConsoleBase;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using Terminaux.Reader;

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
            { "aurora", new AuroraDisplay() },
            { "barrot", new BarRotDisplay() },
            { "barwave", new BarWaveDisplay() },
            { "beatfader", new BeatFaderDisplay() },
            { "beatpulse", new BeatPulseDisplay() },
            { "beatedgepulse", new BeatEdgePulseDisplay() },
            { "bloom", new BloomDisplay() },
            { "bouncingblock", new BouncingBlockDisplay() },
            { "bouncingtext", new BouncingTextDisplay() },
            { "boxgrid", new BoxGridDisplay() },
            { "bsod", new BSODDisplay() },
            { "colorbleed", new ColorBleedDisplay() },
            { "colormix", new ColorMixDisplay() },
            { "dateandtime", new DateAndTimeDisplay() },
            { "disco", new DiscoDisplay() },
            { "dissolve", new DissolveDisplay() },
            { "edgepulse", new EdgePulseDisplay() },
            { "equalizer", new EqualizerDisplay() },
            { "excalibeats", new ExcaliBeatsDisplay() },
            { "fader", new FaderDisplay() },
            { "faderback", new FaderBackDisplay() },
            { "fallingline", new FallingLineDisplay() },
            { "figlet", new FigletDisplay() },
            { "fireworks", new FireworksDisplay() },
            { "flashcolor", new FlashColorDisplay() },
            { "flashtext", new FlashTextDisplay() },
            { "glitch", new GlitchDisplay() },
            { "glittercolor", new GlitterColorDisplay() },
            { "glittermatrix", new GlitterMatrixDisplay() },
            { "gradient", new GradientDisplay() },
            { "gradientrot", new GradientRotDisplay() },
            { "indeterminate", new IndeterminateDisplay() },
            { "ksx", new KSXDisplay() },
            { "ksx2", new KSX2Display() },
            { "lighter", new LighterDisplay() },
            { "lightning", new LightningDisplay() },
            { "lightspeed", new LightspeedDisplay() },
            { "lines", new LinesDisplay() },
            { "linotypo", new LinotypoDisplay() },
            { "marquee", new MarqueeDisplay() },
            { "matrixbleed", new MatrixBleedDisplay() },
            { "matrix", new MatrixDisplay() },
            { "memdump", new MemdumpDisplay() },
            { "mesmerize", new MesmerizeDisplay() },
            { "newyear", new NewYearDisplay() },
            { "noise", new NoiseDisplay() },
            { "numberscatter", new NumberScatterDisplay() },
            { "plain", new PlainDisplay() },
            { "progressclock", new ProgressClockDisplay() },
            { "pulse", new PulseDisplay() },
            { "ramp", new RampDisplay() },
            { "simplematrix", new SimpleMatrixDisplay() },
            { "siren", new SirenDisplay() },
            { "snakefill", new SnakeFillDisplay() },
            { "spin", new SpinDisplay() },
            { "spotwrite", new SpotWriteDisplay() },
            { "squarecorner", new SquareCornerDisplay() },
            { "stackbox", new StackBoxDisplay() },
            { "starfield", new StarfieldDisplay() },
            { "text", new TextDisplay() },
            { "textbox", new TextBoxDisplay() },
            { "typewriter", new TypewriterDisplay() },
            { "typo", new TypoDisplay() },
            { "wave", new WaveDisplay() },
            { "windowslogo", new WindowsLogoDisplay() },
            { "wipe", new WipeDisplay() },
            { "wordhasher", new WordHasherDisplay() }
        };
        internal static int scrnTimeout = 300000;
        internal static string defSaverName = "matrix";
        internal static bool LockMode;
        internal static bool ShellSuppressLockMode;
        internal static bool inSaver;
        internal static AutoResetEvent SaverAutoReset = new(false);
        internal static KernelThread Timeout = new("Screensaver timeout thread", false, HandleTimeout) { isCritical = true };

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
        /// Gets the name of the screensavers
        /// </summary>
        public static string[] GetScreensaverNames() =>
            Screensavers.Keys.ToArray();

        /// <summary>
        /// Handles the screensaver time so that when it reaches the time threshold, the screensaver launches
        /// </summary>
        public static void HandleTimeout()
        {
            try
            {
                var termDriver = DriverHandler.GetDriver<IConsoleDriver>("Default");
                while (!Flags.KernelShutdown)
                {
                    int OldCursorLeft = termDriver.CursorLeft;
                    SpinWait.SpinUntil(() => !Flags.ScrnTimeReached || Flags.KernelShutdown);
                    if (!Flags.ScrnTimeReached)
                    {
                        // Start the stopwatch for monitoring
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        // Detect movement
                        bool hasMoved = false;
                        SpinWait.SpinUntil(() =>
                        {
                            hasMoved = termDriver.MovementDetected;
                            return hasMoved || Flags.KernelShutdown;
                        }, ScreenTimeout);

                        // Check to see if we're locking
                        bool locking = !hasMoved && stopwatch.ElapsedMilliseconds >= ScreenTimeout;
                        stopwatch.Reset();
                        if (Flags.ScrnTimeReached || Flags.KernelShutdown || Flags.RebootRequested)
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

        /// <summary>
        /// Shows the screensaver
        /// </summary>
        public static void ShowSavers() =>
            ShowSavers(DefaultSaverName);

        /// <summary>
        /// Shows the screensaver
        /// </summary>
        /// <param name="saver">A specified screensaver</param>
        public static void ShowSavers(string saver)
        {
            try
            {
                // Check to see if the scrensaver exists
                EventsManager.FireEvent(EventType.PreShowScreensaver);
                DebugWriter.WriteDebug(DebugLevel.I, "Requested screensaver: {0}", saver);
                if (!IsScreensaverRegistered(saver))
                {
                    TextWriterColor.Write(Translate.DoTranslation("The requested screensaver {0} is not found."), true, KernelColorType.Error, saver);
                    DebugWriter.WriteDebug(DebugLevel.I, "Screensaver {0} not found in the dictionary.", saver);
                    return;
                }

                // Now, judge how to launch the screensaver
                if (saver.ToLower() == "random")
                {
                    // Random screensaver selection function
                    static string SelectRandom()
                    {
                        int ScreensaverIndex = RandomDriver.RandomIdx(Screensavers.Count);
                        string ScreensaverName = Screensavers.Keys.ElementAtOrDefault(ScreensaverIndex);
                        return ScreensaverName;
                    }

                    // Get a random screensaver name
                    int ScreensaverIndex = RandomDriver.RandomIdx(Screensavers.Count);
                    string ScreensaverName = SelectRandom();

                    // We don't want another "random" screensaver showing up, so keep selecting until it's no longer "random"
                    while (ScreensaverName == "random")
                        ScreensaverName = SelectRandom();

                    // Run the screensaver
                    ShowSavers(ScreensaverName);
                }
                else if (Screensavers.ContainsKey(saver.ToLower()))
                {
                    saver = saver.ToLower();
                    var BaseSaver = Screensavers[saver];
                    if (BaseSaver.ScreensaverContainsFlashingImages)
                        BaseSaver.ScreensaverSeizureWarning();
                    inSaver = true;
                    Flags.ScrnTimeReached = true;
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Start(BaseSaver);
                    DebugWriter.WriteDebug(DebugLevel.I, "{0} started", saver);
                }
                else
                {
                    // Only one custom screensaver can be used.
                    var BaseSaver = CustomSaverTools.CustomSavers[saver];
                    if (BaseSaver.ScreensaverContainsFlashingImages)
                        BaseSaver.ScreensaverSeizureWarning();
                    inSaver = true;
                    Flags.ScrnTimeReached = true;
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Start(new CustomDisplay(BaseSaver));
                    DebugWriter.WriteDebug(DebugLevel.I, "Custom screensaver {0} started", saver);
                }
            }
            catch (InvalidOperationException ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Error when trying to start screensaver, because of an invalid operation."), true, KernelColorType.Error);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Error when trying to start screensaver:") + " {0}", true, KernelColorType.Error, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Locks the screen. The password will be required when unlocking, depending on the kernel settings.
        /// </summary>
        public static void LockScreen()
        {
            ShellSuppressLockMode = LockMode = true;
            try
            {
                // Show the screensaver and wait for input
                ShowSavers();
                EventsManager.FireEvent(EventType.PreUnlock, DefaultSaverName);
                SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable);

                // Bail from screensaver and optionally prompt for password
                ScreensaverDisplayer.BailFromScreensaver();
                if (PasswordLock)
                    Login.ShowPasswordPrompt(UserManagement.CurrentUser.Username);
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
            return Screensavers.ContainsKey(name) || CustomSaverTools.CustomSavers.ContainsKey(name) || name == "random";
        }

        /// <summary>
        /// Screensaver error handler
        /// </summary>
        internal static void HandleSaverError(Exception Exception)
        {
            if (Exception is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Screensaver experienced an error: {0}.", Exception.Message);
                DebugWriter.WriteDebugStackTrace(Exception);
                HandleSaverCancel();
                TextWriterColor.Write(Translate.DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), true, KernelColorType.Error, Exception.Message);
            }
        }

        /// <summary>
        /// Screensaver cancellation handler
        /// </summary>
        internal static void HandleSaverCancel()
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Cancellation is pending. Cleaning everything up...");
            KernelColorTools.LoadBack();
            ConsoleWrapper.CursorVisible = true;
            DebugWriter.WriteDebug(DebugLevel.I, "All clean. Screensaver stopped.");
            SaverAutoReset.Set();
        }

    }
}
