
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
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Misc.Threading;

namespace KS.Misc.Screensaver.Displays
{
    public static class GlitterColorSettings
    {

        private static bool _glitterColor255Colors;
        private static bool _glitterColorTrueColor = true;
        private static int _glitterColorDelay = 1;
        private static int _glitterColorMinimumRedColorLevel = 0;
        private static int _glitterColorMinimumGreenColorLevel = 0;
        private static int _glitterColorMinimumBlueColorLevel = 0;
        private static int _glitterColorMinimumColorLevel = 0;
        private static int _glitterColorMaximumRedColorLevel = 255;
        private static int _glitterColorMaximumGreenColorLevel = 255;
        private static int _glitterColorMaximumBlueColorLevel = 255;
        private static int _glitterColorMaximumColorLevel = 255;

        /// <summary>
        /// [GlitterColor] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool GlitterColor255Colors
        {
            get
            {
                return _glitterColor255Colors;
            }
            set
            {
                _glitterColor255Colors = value;
            }
        }
        /// <summary>
        /// [GlitterColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool GlitterColorTrueColor
        {
            get
            {
                return _glitterColorTrueColor;
            }
            set
            {
                _glitterColorTrueColor = value;
            }
        }
        /// <summary>
        /// [GlitterColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GlitterColorDelay
        {
            get
            {
                return _glitterColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                _glitterColorDelay = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum red color level (true color)
        /// </summary>
        public static int GlitterColorMinimumRedColorLevel
        {
            get
            {
                return _glitterColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _glitterColorMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum green color level (true color)
        /// </summary>
        public static int GlitterColorMinimumGreenColorLevel
        {
            get
            {
                return _glitterColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _glitterColorMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum blue color level (true color)
        /// </summary>
        public static int GlitterColorMinimumBlueColorLevel
        {
            get
            {
                return _glitterColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _glitterColorMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int GlitterColorMinimumColorLevel
        {
            get
            {
                return _glitterColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _glitterColor255Colors | _glitterColorTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _glitterColorMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum red color level (true color)
        /// </summary>
        public static int GlitterColorMaximumRedColorLevel
        {
            get
            {
                return _glitterColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= _glitterColorMinimumRedColorLevel)
                    value = _glitterColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _glitterColorMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum green color level (true color)
        /// </summary>
        public static int GlitterColorMaximumGreenColorLevel
        {
            get
            {
                return _glitterColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _glitterColorMinimumGreenColorLevel)
                    value = _glitterColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _glitterColorMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum blue color level (true color)
        /// </summary>
        public static int GlitterColorMaximumBlueColorLevel
        {
            get
            {
                return _glitterColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _glitterColorMinimumBlueColorLevel)
                    value = _glitterColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _glitterColorMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int GlitterColorMaximumColorLevel
        {
            get
            {
                return _glitterColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _glitterColor255Colors | _glitterColorTrueColor ? 255 : 15;
                if (value <= _glitterColorMinimumColorLevel)
                    value = _glitterColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _glitterColorMaximumColorLevel = value;
            }
        }

    }

    public class GlitterColorDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "GlitterColor";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleBase.ConsoleWrapper.WindowWidth, ConsoleBase.ConsoleWrapper.WindowHeight);
        }

        public override void ScreensaverLogic()
        {
            ConsoleBase.ConsoleWrapper.CursorVisible = false;

            // Select position
            int Left = RandomDriver.Next(ConsoleBase.ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.Next(ConsoleBase.ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleBase.ConsoleWrapper.SetCursorPosition(Left, Top);

            // Make a glitter color
            if (GlitterColorSettings.GlitterColorTrueColor)
            {
                int RedColorNum = RandomDriver.Next(GlitterColorSettings.GlitterColorMinimumRedColorLevel, GlitterColorSettings.GlitterColorMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Next(GlitterColorSettings.GlitterColorMinimumGreenColorLevel, GlitterColorSettings.GlitterColorMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Next(GlitterColorSettings.GlitterColorMinimumBlueColorLevel, GlitterColorSettings.GlitterColorMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ColorTools.SetConsoleColor(ColorStorage, true, true);
                    ConsoleBase.ConsoleWrapper.Write(" ");
                }
            }
            else if (GlitterColorSettings.GlitterColor255Colors)
            {
                int ColorNum = RandomDriver.Next(GlitterColorSettings.GlitterColorMinimumColorLevel, GlitterColorSettings.GlitterColorMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                    ConsoleBase.ConsoleWrapper.Write(" ");
                }
            }
            else
            {
                if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                    ResizeSyncing = true;
                if (!ResizeSyncing)
                {
                    ConsoleBase.ConsoleWrapper.BackgroundColor = Screensaver.colors[RandomDriver.Next(GlitterColorSettings.GlitterColorMinimumColorLevel, GlitterColorSettings.GlitterColorMaximumColorLevel)];
                    DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ConsoleBase.ConsoleWrapper.BackgroundColor);
                    ConsoleBase.ConsoleWrapper.Write(" ");
                }
            }

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ThreadManager.SleepNoBlock(GlitterColorSettings.GlitterColorDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}