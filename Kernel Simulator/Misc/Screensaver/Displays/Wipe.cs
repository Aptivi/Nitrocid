
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
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Misc.Platform;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Screensaver.Displays
{
    public static class WipeSettings
    {

        private static bool _wipe255Colors;
        private static bool _wipeTrueColor = true;
        private static int _wipeDelay = 10;
        private static int _wipeWipesNeededToChangeDirection = 10;
        private static string _wipeBackgroundColor = new Color((int)ConsoleColor.Black).PlainSequence;
        private static int _wipeMinimumRedColorLevel = 0;
        private static int _wipeMinimumGreenColorLevel = 0;
        private static int _wipeMinimumBlueColorLevel = 0;
        private static int _wipeMinimumColorLevel = 0;
        private static int _wipeMaximumRedColorLevel = 255;
        private static int _wipeMaximumGreenColorLevel = 255;
        private static int _wipeMaximumBlueColorLevel = 255;
        private static int _wipeMaximumColorLevel = 255;

        /// <summary>
        /// [Wipe] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool Wipe255Colors
        {
            get
            {
                return _wipe255Colors;
            }
            set
            {
                _wipe255Colors = value;
            }
        }
        /// <summary>
        /// [Wipe] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool WipeTrueColor
        {
            get
            {
                return _wipeTrueColor;
            }
            set
            {
                _wipeTrueColor = value;
            }
        }
        /// <summary>
        /// [Wipe] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int WipeDelay
        {
            get
            {
                return _wipeDelay;
            }
            set
            {
                _wipeDelay = value;
            }
        }
        /// <summary>
        /// [Wipe] How many wipes needed to change direction?
        /// </summary>
        public static int WipeWipesNeededToChangeDirection
        {
            get
            {
                return _wipeWipesNeededToChangeDirection;
            }
            set
            {
                _wipeWipesNeededToChangeDirection = value;
            }
        }
        /// <summary>
        /// [Wipe] Screensaver background color
        /// </summary>
        public static string WipeBackgroundColor
        {
            get
            {
                return _wipeBackgroundColor;
            }
            set
            {
                _wipeBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum red color level (true color)
        /// </summary>
        public static int WipeMinimumRedColorLevel
        {
            get
            {
                return _wipeMinimumRedColorLevel;
            }
            set
            {
                _wipeMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum green color level (true color)
        /// </summary>
        public static int WipeMinimumGreenColorLevel
        {
            get
            {
                return _wipeMinimumGreenColorLevel;
            }
            set
            {
                _wipeMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum blue color level (true color)
        /// </summary>
        public static int WipeMinimumBlueColorLevel
        {
            get
            {
                return _wipeMinimumBlueColorLevel;
            }
            set
            {
                _wipeMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int WipeMinimumColorLevel
        {
            get
            {
                return _wipeMinimumColorLevel;
            }
            set
            {
                _wipeMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum red color level (true color)
        /// </summary>
        public static int WipeMaximumRedColorLevel
        {
            get
            {
                return _wipeMaximumRedColorLevel;
            }
            set
            {
                _wipeMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum green color level (true color)
        /// </summary>
        public static int WipeMaximumGreenColorLevel
        {
            get
            {
                return _wipeMaximumGreenColorLevel;
            }
            set
            {
                _wipeMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum blue color level (true color)
        /// </summary>
        public static int WipeMaximumBlueColorLevel
        {
            get
            {
                return _wipeMaximumBlueColorLevel;
            }
            set
            {
                _wipeMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Wipe] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int WipeMaximumColorLevel
        {
            get
            {
                return _wipeMaximumColorLevel;
            }
            set
            {
                _wipeMaximumColorLevel = value;
            }
        }

    }
    public class WipeDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;
        private WipeDirections ToDirection = WipeDirections.Right;
        private int TimesWiped = 0;

        public override string ScreensaverName { get; set; } = "Wipe";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ColorTools.SetConsoleColor(new Color(WipeSettings.WipeBackgroundColor), true, true);
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ConsoleBase.ConsoleWrapper.Clear();
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
        }

        public override void ScreensaverLogic()
        {
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
            if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                ResizeSyncing = true;

            // Select a color
            if (WipeSettings.WipeTrueColor)
            {
                int RedColorNum = RandomDriver.Next(WipeSettings.WipeMinimumRedColorLevel, WipeSettings.WipeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Next(WipeSettings.WipeMinimumGreenColorLevel, WipeSettings.WipeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Next(WipeSettings.WipeMinimumBlueColorLevel, WipeSettings.WipeMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (!ResizeSyncing)
                    ColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true, true);
            }
            else if (WipeSettings.Wipe255Colors)
            {
                int ColorNum = RandomDriver.Next(WipeSettings.WipeMinimumColorLevel, WipeSettings.WipeMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ResizeSyncing)
                    ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
            }
            else
            {
                if (!ResizeSyncing)
                    ConsoleBase.ConsoleWrapper.BackgroundColor = Screensaver.colors[RandomDriver.Next(WipeSettings.WipeMinimumColorLevel, WipeSettings.WipeMaximumColorLevel)];
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ConsoleBase.ConsoleWrapper.BackgroundColor);
            }

            // Set max height according to platform
            int MaxWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            if (KernelPlatform.IsOnUnix())
                MaxWindowHeight -= 1;
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Max height {0}", MaxWindowHeight);

            // Print a space {Column} times until the entire screen is wiped.
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Wipe direction {0}", ToDirection.ToString());
            switch (ToDirection)
            {
                case WipeDirections.Right:
                    {
                        for (int Column = 0, loopTo = ConsoleBase.ConsoleWrapper.WindowWidth; Column <= loopTo; Column++)
                        {
                            if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                                ResizeSyncing = true;
                            if (ResizeSyncing)
                                break;
                            for (int Row = 0, loopTo1 = MaxWindowHeight; Row <= loopTo1; Row++)
                            {
                                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                                    ResizeSyncing = true;
                                if (ResizeSyncing)
                                    break;

                                // Do the actual writing
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Setting Y position to {0}", Row);
                                ConsoleBase.ConsoleWrapper.SetCursorPosition(0, Row);
                                ConsoleBase.ConsoleWrapper.Write(" ".Repeat(Column));
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Column);
                            }
                            ThreadManager.SleepNoBlock(WipeSettings.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        break;
                    }
                case WipeDirections.Left:
                    {
                        for (int Column = ConsoleBase.ConsoleWrapper.WindowWidth; Column >= 1; Column -= 1)
                        {
                            if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                                ResizeSyncing = true;
                            if (ResizeSyncing)
                                break;
                            for (int Row = 0, loopTo2 = MaxWindowHeight; Row <= loopTo2; Row++)
                            {
                                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                                    ResizeSyncing = true;
                                if (ResizeSyncing)
                                    break;

                                // Do the actual writing
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Setting position to {0}", Column - 1, Row);
                                ConsoleBase.ConsoleWrapper.SetCursorPosition(Column - 1, Row);
                                ConsoleBase.ConsoleWrapper.Write(" ".Repeat(ConsoleBase.ConsoleWrapper.WindowWidth - Column + 1));
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleBase.ConsoleWrapper.WindowWidth - Column + 1);
                            }
                            ThreadManager.SleepNoBlock(WipeSettings.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        break;
                    }
                case WipeDirections.Top:
                    {
                        for (int Row = MaxWindowHeight; Row >= 0; Row -= 1)
                        {
                            if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                                ResizeSyncing = true;
                            if (ResizeSyncing)
                                break;

                            // Do the actual writing
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Setting Y position to {0}", Row);
                            ConsoleBase.ConsoleWrapper.SetCursorPosition(0, Row);
                            ConsoleBase.ConsoleWrapper.Write(" ".Repeat(ConsoleBase.ConsoleWrapper.WindowWidth));
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleBase.ConsoleWrapper.WindowWidth);
                            ThreadManager.SleepNoBlock(WipeSettings.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }

                        break;
                    }
                case WipeDirections.Bottom:
                    {
                        for (int Row = 0, loopTo3 = MaxWindowHeight; Row <= loopTo3; Row++)
                        {
                            if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                                ResizeSyncing = true;
                            if (ResizeSyncing)
                                break;

                            // Do the actual writing
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", ConsoleBase.ConsoleWrapper.WindowWidth);
                            ConsoleBase.ConsoleWrapper.Write(" ".Repeat(ConsoleBase.ConsoleWrapper.WindowWidth));
                            ThreadManager.SleepNoBlock(WipeSettings.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        }
                        ConsoleBase.ConsoleWrapper.SetCursorPosition(0, 0);
                        break;
                    }
            }

            if (!ResizeSyncing)
            {
                TimesWiped += 1;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Wiped {0} times out of {1}", TimesWiped, WipeSettings.WipeWipesNeededToChangeDirection);

                // Check if the number of times wiped is equal to the number of required times to change wiping direction.
                if (TimesWiped == WipeSettings.WipeWipesNeededToChangeDirection)
                {
                    TimesWiped = 0;
                    ToDirection = (WipeDirections)Convert.ToInt32(Enum.Parse(typeof(WipeDirections), RandomDriver.Next(0, 3).ToString()));
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Changed direction to {0}", ToDirection.ToString());
                }
            }
            else
            {
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                ColorTools.SetConsoleColor(new Color(WipeSettings.WipeBackgroundColor), true);
                ConsoleBase.ConsoleWrapper.Clear();
            }

            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(WipeSettings.WipeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <summary>
        /// Wipe directions
        /// </summary>
        private enum WipeDirections
        {
            /// <summary>
            /// Wipe from right to left
            /// </summary>
            Left,
            /// <summary>
            /// Wipe from left to right
            /// </summary>
            Right,
            /// <summary>
            /// Wipe from bottom to top
            /// </summary>
            Top,
            /// <summary>
            /// Wipe from top to bottom
            /// </summary>
            Bottom
        }

    }
}