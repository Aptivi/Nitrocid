using System;
using System.Collections.Generic;
using ColorSeq;

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

using Extensification.IntegerExts;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Screensaver.Displays
{
    public static class StackBoxSettings
    {

        private static bool _stackBox255Colors;
        private static bool _stackBoxTrueColor = true;
        private static int _stackBoxDelay = 10;
        private static bool _stackBoxFill = true;
        private static int _stackBoxMinimumRedColorLevel = 0;
        private static int _stackBoxMinimumGreenColorLevel = 0;
        private static int _stackBoxMinimumBlueColorLevel = 0;
        private static int _stackBoxMinimumColorLevel = 0;
        private static int _stackBoxMaximumRedColorLevel = 255;
        private static int _stackBoxMaximumGreenColorLevel = 255;
        private static int _stackBoxMaximumBlueColorLevel = 255;
        private static int _stackBoxMaximumColorLevel = 255;

        /// <summary>
        /// [StackBox] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool StackBox255Colors
        {
            get
            {
                return _stackBox255Colors;
            }
            set
            {
                _stackBox255Colors = value;
            }
        }
        /// <summary>
        /// [StackBox] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool StackBoxTrueColor
        {
            get
            {
                return _stackBoxTrueColor;
            }
            set
            {
                _stackBoxTrueColor = value;
            }
        }
        /// <summary>
        /// [StackBox] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int StackBoxDelay
        {
            get
            {
                return _stackBoxDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _stackBoxDelay = value;
            }
        }
        /// <summary>
        /// [StackBox] Whether to fill in the boxes drawn, or only draw the outline
        /// </summary>
        public static bool StackBoxFill
        {
            get
            {
                return _stackBoxFill;
            }
            set
            {
                _stackBoxFill = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum red color level (true color)
        /// </summary>
        public static int StackBoxMinimumRedColorLevel
        {
            get
            {
                return _stackBoxMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _stackBoxMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum green color level (true color)
        /// </summary>
        public static int StackBoxMinimumGreenColorLevel
        {
            get
            {
                return _stackBoxMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _stackBoxMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum blue color level (true color)
        /// </summary>
        public static int StackBoxMinimumBlueColorLevel
        {
            get
            {
                return _stackBoxMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _stackBoxMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int StackBoxMinimumColorLevel
        {
            get
            {
                return _stackBoxMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _stackBox255Colors | _stackBoxTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _stackBoxMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum red color level (true color)
        /// </summary>
        public static int StackBoxMaximumRedColorLevel
        {
            get
            {
                return _stackBoxMaximumRedColorLevel;
            }
            set
            {
                if (value <= _stackBoxMinimumRedColorLevel)
                    value = _stackBoxMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _stackBoxMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum green color level (true color)
        /// </summary>
        public static int StackBoxMaximumGreenColorLevel
        {
            get
            {
                return _stackBoxMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _stackBoxMinimumGreenColorLevel)
                    value = _stackBoxMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _stackBoxMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum blue color level (true color)
        /// </summary>
        public static int StackBoxMaximumBlueColorLevel
        {
            get
            {
                return _stackBoxMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _stackBoxMinimumBlueColorLevel)
                    value = _stackBoxMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _stackBoxMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [StackBox] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int StackBoxMaximumColorLevel
        {
            get
            {
                return _stackBoxMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _stackBox255Colors | _stackBoxTrueColor ? 255 : 15;
                if (value <= _stackBoxMinimumColorLevel)
                    value = _stackBoxMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _stackBoxMaximumColorLevel = value;
            }
        }

    }

    public class StackBoxDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "StackBox";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            Console.CursorVisible = false;
            if (ResizeSyncing)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();

                // Reset resize sync
                ResizeSyncing = false;
                CurrentWindowWidth = Console.WindowWidth;
                CurrentWindowHeight = Console.WindowHeight;
            }
            else
            {
                bool Drawable = true;
                if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                    ResizeSyncing = true;

                // Get the required positions for the box
                int BoxStartX = RandomDriver.Next(Console.WindowWidth);
                int BoxEndX = RandomDriver.Next(Console.WindowWidth);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Box X position {0} -> {1}", BoxStartX, BoxEndX);
                int BoxStartY = RandomDriver.Next(Console.WindowHeight);
                int BoxEndY = RandomDriver.Next(Console.WindowHeight);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Box Y position {0} -> {1}", BoxStartY, BoxEndY);

                // Check to see if start is less than or equal to end
                BoxStartX.SwapIfSourceLarger(ref BoxEndX);
                BoxStartY.SwapIfSourceLarger(ref BoxEndY);
                if (BoxStartX == BoxEndX | BoxStartY == BoxEndY)
                {
                    // Don't draw; it won't be shown anyways
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Asking StackBox not to draw. Consult above two lines.");
                    Drawable = false;
                }

                if (Drawable)
                {
                    // Select color
                    if (StackBoxSettings.StackBoxTrueColor)
                    {
                        int RedColorNum = RandomDriver.Next(StackBoxSettings.StackBoxMinimumRedColorLevel, StackBoxSettings.StackBoxMaximumRedColorLevel);
                        int GreenColorNum = RandomDriver.Next(StackBoxSettings.StackBoxMinimumGreenColorLevel, StackBoxSettings.StackBoxMaximumGreenColorLevel);
                        int BlueColorNum = RandomDriver.Next(StackBoxSettings.StackBoxMinimumBlueColorLevel, StackBoxSettings.StackBoxMaximumBlueColorLevel);
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                        ColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true, true);
                    }
                    else if (StackBoxSettings.StackBox255Colors)
                    {
                        int ColorNum = RandomDriver.Next(StackBoxSettings.StackBoxMinimumColorLevel, StackBoxSettings.StackBoxMaximumColorLevel);
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                        ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                    }
                    else
                    {
                        Console.BackgroundColor = Screensaver.colors[RandomDriver.Next(StackBoxSettings.StackBoxMinimumColorLevel, StackBoxSettings.StackBoxMaximumColorLevel)];
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor);
                    }

                    // Draw the box
                    if (StackBoxSettings.StackBoxFill)
                    {
                        // Cover all the positions
                        for (int X = BoxStartX, loopTo = BoxEndX; X <= loopTo; X++)
                        {
                            for (int Y = BoxStartY, loopTo1 = BoxEndY; Y <= loopTo1; Y++)
                            {
                                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Filling {0},{1}...", X, Y);
                                Console.SetCursorPosition(X, Y);
                                Console.Write(" ");
                            }
                        }
                    }
                    else
                    {
                        // Draw the upper and lower borders
                        for (int X = BoxStartX, loopTo2 = BoxEndX; X <= loopTo2; X++)
                        {
                            Console.SetCursorPosition(X, BoxStartY);
                            Console.Write(" ");
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn upper border at {0}", X);
                            Console.SetCursorPosition(X, BoxEndY);
                            Console.Write(" ");
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn lower border at {0}", X);
                        }

                        // Draw the left and right borders
                        for (int Y = BoxStartY, loopTo3 = BoxEndY; Y <= loopTo3; Y++)
                        {
                            Console.SetCursorPosition(BoxStartX, Y);
                            Console.Write(" ");
                            if (!(BoxStartX >= Console.WindowWidth - 1))
                                Console.Write(" ");
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn left border at {0}", Y);
                            Console.SetCursorPosition(BoxEndX, Y);
                            Console.Write(" ");
                            if (!(BoxEndX >= Console.WindowWidth - 1))
                                Console.Write(" ");
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Drawn right border at {0}", Y);
                        }
                    }
                }
            }
            ThreadManager.SleepNoBlock(StackBoxSettings.StackBoxDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}