
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
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Screensaver.Displays
{
    public static class FlashColorSettings
    {

        private static bool _flashColor255Colors;
        private static bool _flashColorTrueColor = true;
        private static int _flashColorDelay = 20;
        private static string _flashColorBackgroundColor = new Color((int)ConsoleColor.Black).PlainSequence;
        private static int _flashColorMinimumRedColorLevel = 0;
        private static int _flashColorMinimumGreenColorLevel = 0;
        private static int _flashColorMinimumBlueColorLevel = 0;
        private static int _flashColorMinimumColorLevel = 0;
        private static int _flashColorMaximumRedColorLevel = 255;
        private static int _flashColorMaximumGreenColorLevel = 255;
        private static int _flashColorMaximumBlueColorLevel = 255;
        private static int _flashColorMaximumColorLevel = 0;

        /// <summary>
        /// [FlashColor] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool FlashColor255Colors
        {
            get
            {
                return _flashColor255Colors;
            }
            set
            {
                _flashColor255Colors = value;
            }
        }
        /// <summary>
        /// [FlashColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FlashColorTrueColor
        {
            get
            {
                return _flashColorTrueColor;
            }
            set
            {
                _flashColorTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FlashColorDelay
        {
            get
            {
                return _flashColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                _flashColorDelay = value;
            }
        }
        /// <summary>
        /// [FlashColor] Screensaver background color
        /// </summary>
        public static string FlashColorBackgroundColor
        {
            get
            {
                return _flashColorBackgroundColor;
            }
            set
            {
                _flashColorBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum red color level (true color)
        /// </summary>
        public static int FlashColorMinimumRedColorLevel
        {
            get
            {
                return _flashColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _flashColorMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum green color level (true color)
        /// </summary>
        public static int FlashColorMinimumGreenColorLevel
        {
            get
            {
                return _flashColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _flashColorMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum blue color level (true color)
        /// </summary>
        public static int FlashColorMinimumBlueColorLevel
        {
            get
            {
                return _flashColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _flashColorMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashColorMinimumColorLevel
        {
            get
            {
                return _flashColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _flashColor255Colors | _flashColorTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _flashColorMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum red color level (true color)
        /// </summary>
        public static int FlashColorMaximumRedColorLevel
        {
            get
            {
                return _flashColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= _flashColorMinimumRedColorLevel)
                    value = _flashColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _flashColorMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum green color level (true color)
        /// </summary>
        public static int FlashColorMaximumGreenColorLevel
        {
            get
            {
                return _flashColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _flashColorMinimumGreenColorLevel)
                    value = _flashColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _flashColorMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum blue color level (true color)
        /// </summary>
        public static int FlashColorMaximumBlueColorLevel
        {
            get
            {
                return _flashColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _flashColorMinimumBlueColorLevel)
                    value = _flashColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _flashColorMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashColorMaximumColorLevel
        {
            get
            {
                return _flashColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _flashColor255Colors | _flashColorTrueColor ? 255 : 15;
                if (value <= _flashColorMinimumColorLevel)
                    value = _flashColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _flashColorMaximumColorLevel = value;
            }
        }

    }

    public class FlashColorDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "FlashColor";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            ColorTools.SetConsoleColor(new Color(FlashColorSettings.FlashColorBackgroundColor), true, true);
            ConsoleWrapper.Clear();
            DebugWriter.Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select position
            int Left = RandomDriver.Next(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.Next(ConsoleWrapper.WindowHeight);
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleWrapper.SetCursorPosition(Left, Top);

            // Make a flash color
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleExtensions.ClearKeepPosition();
            if (FlashColorSettings.FlashColorTrueColor)
            {
                int RedColorNum = RandomDriver.Next(FlashColorSettings.FlashColorMinimumRedColorLevel, FlashColorSettings.FlashColorMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Next(FlashColorSettings.FlashColorMinimumGreenColorLevel, FlashColorSettings.FlashColorMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Next(FlashColorSettings.FlashColorMinimumBlueColorLevel, FlashColorSettings.FlashColorMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ColorTools.SetConsoleColor(ColorStorage, true, true);
                    ConsoleWrapper.Write(" ");
                }
            }
            else if (FlashColorSettings.FlashColor255Colors)
            {
                int ColorNum = RandomDriver.Next(FlashColorSettings.FlashColorMinimumColorLevel, FlashColorSettings.FlashColorMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                    ConsoleWrapper.Write(" ");
                }
            }
            else
            {
                if (CurrentWindowHeight != ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ConsoleWrapper.BackgroundColor = Screensaver.colors[RandomDriver.Next(FlashColorSettings.FlashColorMinimumColorLevel, FlashColorSettings.FlashColorMaximumColorLevel)];
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ConsoleWrapper.BackgroundColor);
                    ConsoleWrapper.Write(" ");
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(FlashColorSettings.FlashColorDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}