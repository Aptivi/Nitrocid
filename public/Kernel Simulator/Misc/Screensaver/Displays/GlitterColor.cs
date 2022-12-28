
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for GlitterColor
    /// </summary>
    public static class GlitterColorSettings
    {

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
                int FinalMinimumLevel = 255;
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
                int FinalMaximumLevel = 255;
                if (value <= _glitterColorMinimumColorLevel)
                    value = _glitterColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _glitterColorMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for GlitterColor
    /// </summary>
    public class GlitterColorDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "GlitterColor";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleWrapper.SetCursorPosition(Left, Top);

            // Make a glitter color
            if (GlitterColorSettings.GlitterColorTrueColor)
            {
                int RedColorNum = RandomDriver.Random(GlitterColorSettings.GlitterColorMinimumRedColorLevel, GlitterColorSettings.GlitterColorMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(GlitterColorSettings.GlitterColorMinimumGreenColorLevel, GlitterColorSettings.GlitterColorMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(GlitterColorSettings.GlitterColorMinimumBlueColorLevel, GlitterColorSettings.GlitterColorMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ColorTools.SetConsoleColor(ColorStorage, true, true);
                    ConsoleWrapper.Write(" ");
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(GlitterColorSettings.GlitterColorMinimumColorLevel, GlitterColorSettings.GlitterColorMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                    ConsoleWrapper.Write(" ");
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(GlitterColorSettings.GlitterColorDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
