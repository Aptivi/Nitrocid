
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
    /// Settings for FlashColor
    /// </summary>
    public static class FlashColorSettings
    {

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
                int FinalMinimumLevel = 255;
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
                int FinalMaximumLevel = 255;
                if (value <= _flashColorMinimumColorLevel)
                    value = _flashColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _flashColorMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for FlashColor
    /// </summary>
    public class FlashColorDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "FlashColor";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBack(new Color(FlashColorSettings.FlashColorBackgroundColor), true);
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.Random(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleWrapper.SetCursorPosition(Left, Top);

            // Make a flash color
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleExtensions.ClearKeepPosition();
            if (FlashColorSettings.FlashColorTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumRedColorLevel, FlashColorSettings.FlashColorMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumGreenColorLevel, FlashColorSettings.FlashColorMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumBlueColorLevel, FlashColorSettings.FlashColorMaximumBlueColorLevel);
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
                int ColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumColorLevel, FlashColorSettings.FlashColorMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                    ConsoleWrapper.Write(" ");
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(FlashColorSettings.FlashColorDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
