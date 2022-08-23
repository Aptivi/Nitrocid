
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
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
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Screensaver.Displays;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Screensaver
{
    public static class Screensaver
    {

        // Public Variables
        public static bool LockMode;
        public static bool InSaver;
        public static bool ScreensaverDebug;
        public static string DefSaverName = "matrix";
        public static int ScrnTimeout = 300000;
        public static bool PasswordLock = true;
        public readonly static ConsoleColor[] colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));        // 15 Console Colors
        public readonly static ConsoleColors[] colors255 = (ConsoleColors[])Enum.GetValues(typeof(ConsoleColors));  // 255 Console Colors

        // Private variables
        internal static Dictionary<string, BaseScreensaver> Screensavers = new Dictionary<string, BaseScreensaver>()
        {
            { "barrot", new BarRotDisplay() },
            { "beatfader", new BeatFaderDisplay() },
            { "beatpulse", new BeatPulseDisplay() },
            { "beatedgepulse", new BeatEdgePulseDisplay() },
            { "bouncingblock", new BouncingBlockDisplay() },
            { "bouncingtext", new BouncingTextDisplay() },
            { "colormix", new ColorMixDisplay() },
            { "dateandtime", new DateAndTimeDisplay() },
            { "disco", new DiscoDisplay() },
            { "dissolve", new DissolveDisplay() },
            { "edgepulse", new EdgePulseDisplay() },
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
            { "lighter", new LighterDisplay() },
            { "lines", new LinesDisplay() },
            { "linotypo", new LinotypoDisplay() },
            { "marquee", new MarqueeDisplay() },
            { "matrix", new MatrixDisplay() },
            { "noise", new NoiseDisplay() },
            { "personlookup", new PersonLookupDisplay() },
            { "plain", new PlainDisplay() },
            { "progressclock", new ProgressClockDisplay() },
            { "pulse", new PulseDisplay() },
            { "ramp", new RampDisplay() },
            { "random", new RandomSaverDisplay() },
            { "snaker", new SnakerDisplay() },
            { "spotwrite", new SpotWriteDisplay() },
            { "stackbox", new StackBoxDisplay() },
            { "starfield", new StarfieldDisplay() },
            { "typewriter", new TypewriterDisplay() },
            { "typo", new TypoDisplay() },
            { "windowslogo", new WindowsLogoDisplay() },
            { "wipe", new WipeDisplay() }
        };
        internal static AutoResetEvent SaverAutoReset = new AutoResetEvent(false);
        internal static KernelThread Timeout = new KernelThread("Screensaver timeout thread", false, HandleTimeout);

        /// <summary>
        /// Handles the screensaver time so that when it reaches the time threshold, the screensaver launches
        /// </summary>
        public static void HandleTimeout()
        {
            try
            {
                int CountedTime;
                int OldCursorLeft = Console.CursorLeft;
                while (!Flags.KernelShutdown)
                {
                    if (!Flags.ScrnTimeReached)
                    {
                        bool exitWhile = false;
                        var loopTo = ScrnTimeout;
                        for (CountedTime = 0; CountedTime <= loopTo; CountedTime++)
                        {
                            Thread.Sleep(1);
                            if (Console.KeyAvailable | OldCursorLeft != Console.CursorLeft)
                            {
                                CountedTime = 0;
                            }
                            OldCursorLeft = Console.CursorLeft;
                            if (CountedTime > ScrnTimeout)
                            {
                                // This shouldn't happen, but the counted time is bigger than the screen timeout. Just bail.
                                break;
                            }

                            // Poll to see if there is a kernel shutdown signal
                            if (Flags.KernelShutdown)
                            {
                                exitWhile = true;
                                break;
                            }
                        }

                        if (exitWhile)
                        {
                            break;
                        }
                        if (!Flags.RebootRequested)
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Screen time has reached.");
                            LockScreen();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Shutting down screensaver timeout thread: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
            }
        }

        /// <summary>
        /// Shows the screensaver
        /// </summary>
        /// <param name="saver">A specified screensaver</param>
        public static void ShowSavers(string saver)
        {
            try
            {
                InSaver = true;
                Flags.ScrnTimeReached = true;
                Kernel.Kernel.KernelEventManager.RaisePreShowScreensaver(saver);
                DebugWriter.Wdbg(DebugLevel.I, "Requested screensaver: {0}", saver);
                if (Screensavers.ContainsKey(saver.ToLower()))
                {
                    saver = saver.ToLower();
                    var BaseSaver = Screensavers[saver];
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Start(BaseSaver);
                    DebugWriter.Wdbg(DebugLevel.I, "{0} started", saver);
                    Input.DetectKeypress();
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Stop();
                    SaverAutoReset.WaitOne();
                }
                else if (CustomSaverTools.CustomSavers.ContainsKey(saver))
                {
                    // Only one custom screensaver can be used.
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Start(new CustomDisplay(CustomSaverTools.CustomSavers[saver].ScreensaverBase));
                    DebugWriter.Wdbg(DebugLevel.I, "Custom screensaver {0} started", saver);
                    Input.DetectKeypress();
                    ScreensaverDisplayer.ScreensaverDisplayerThread.Stop();
                    SaverAutoReset.WaitOne();
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("The requested screensaver {0} is not found."), true, ColorTools.ColTypes.Error, saver);
                    DebugWriter.Wdbg(DebugLevel.I, "Screensaver {0} not found in the dictionary.", saver);
                }

                // Raise event
                DebugWriter.Wdbg(DebugLevel.I, "Screensaver really stopped.");
                Kernel.Kernel.KernelEventManager.RaisePostShowScreensaver(saver);
            }
            catch (InvalidOperationException ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Error when trying to start screensaver, because of an invalid operation."), true, ColorTools.ColTypes.Error);
                DebugWriter.WStkTrc(ex);
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("Error when trying to start screensaver:") + " {0}", true, ColorTools.ColTypes.Error, ex.Message);
                DebugWriter.WStkTrc(ex);
            }
            finally
            {
                InSaver = false;
                Flags.ScrnTimeReached = false;
            }
        }

        /// <summary>
        /// Locks the screen. The password will be required when unlocking, depending on the kernel settings.
        /// </summary>
        public static void LockScreen()
        {
            LockMode = true;
            ShowSavers(DefSaverName);
            Kernel.Kernel.KernelEventManager.RaisePreUnlock(DefSaverName);
            if (PasswordLock)
            {
                Login.Login.ShowPasswordPrompt(Login.Login.CurrentUser.Username);
            }
            else
            {
                LockMode = false;
            }
        }

        /// <summary>
        /// Sets the default screensaver
        /// </summary>
        /// <param name="saver">Specified screensaver</param>
        public static void SetDefaultScreensaver(string saver)
        {
            saver = saver.ToLower();
            if (Screensavers.ContainsKey(saver) | CustomSaverTools.CustomSavers.ContainsKey(saver))
            {
                DebugWriter.Wdbg(DebugLevel.I, "{0} is found. Setting it to default...", saver);
                DefSaverName = saver;
                var Token = ConfigTools.GetConfigCategory(Config.ConfigCategory.Screensaver);
                ConfigTools.SetConfigValue(Config.ConfigCategory.Screensaver, Token, "Screensaver", saver);
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.W, "{0} is not found.", saver);
                throw new Kernel.Exceptions.NoSuchScreensaverException(Translate.DoTranslation("Screensaver {0} not found in database. Check the name and try again."), saver);
            }
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
        /// Screensaver error handler
        /// </summary>
        internal static void HandleSaverError(Exception Exception)
        {
            if (Exception is not null)
            {
                DebugWriter.Wdbg(DebugLevel.W, "Screensaver experienced an error: {0}.", Exception.Message);
                DebugWriter.WStkTrc(Exception);
                HandleSaverCancel();
                TextWriterColor.Write(Translate.DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), true, ColorTools.ColTypes.Error, Exception.Message);
            }
        }

        /// <summary>
        /// Screensaver cancellation handler
        /// </summary>
        internal static void HandleSaverCancel()
        {
            DebugWriter.Wdbg(DebugLevel.W, "Cancellation is pending. Cleaning everything up...");
            ColorTools.SetInputColor();
            ColorTools.LoadBack();
            Console.CursorVisible = true;
            DebugWriter.Wdbg(DebugLevel.I, "All clean. Screensaver stopped.");
            SaverAutoReset.Set();
        }

    }
}
