
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
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for ColorMix
    /// </summary>
    public static class ColorMixSettings
    {

        private static bool _TrueColor = true;
        private static int _Delay = 1;
        private static string _BackgroundColor = new Color((int)ConsoleColor.Red).PlainSequence;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MinimumColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;
        private static int _MaximumColorLevel = 255;

        /// <summary>
        /// [ColorMix] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ColorMixTrueColor
        {
            get
            {
                return _TrueColor;
            }
            set
            {
                _TrueColor = value;
            }
        }
        /// <summary>
        /// [ColorMix] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ColorMixDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                _Delay = value;
            }
        }
        /// <summary>
        /// [ColorMix] Screensaver background color
        /// </summary>
        public static string ColorMixBackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum red color level (true color)
        /// </summary>
        public static int ColorMixMinimumRedColorLevel
        {
            get
            {
                return _MinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum green color level (true color)
        /// </summary>
        public static int ColorMixMinimumGreenColorLevel
        {
            get
            {
                return _MinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum blue color level (true color)
        /// </summary>
        public static int ColorMixMinimumBlueColorLevel
        {
            get
            {
                return _MinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorMixMinimumColorLevel
        {
            get
            {
                return _MinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum red color level (true color)
        /// </summary>
        public static int ColorMixMaximumRedColorLevel
        {
            get
            {
                return _MaximumRedColorLevel;
            }
            set
            {
                if (value <= _MinimumRedColorLevel)
                    value = _MinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum green color level (true color)
        /// </summary>
        public static int ColorMixMaximumGreenColorLevel
        {
            get
            {
                return _MaximumGreenColorLevel;
            }
            set
            {
                if (value <= _MinimumGreenColorLevel)
                    value = _MinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum blue color level (true color)
        /// </summary>
        public static int ColorMixMaximumBlueColorLevel
        {
            get
            {
                return _MaximumBlueColorLevel;
            }
            set
            {
                if (value <= _MinimumBlueColorLevel)
                    value = _MinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorMixMaximumColorLevel
        {
            get
            {
                return _MaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevel)
                    value = _MinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for ColorMix
    /// </summary>
    public class ColorMixDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "ColorMix";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleWrapper.ForegroundColor = ConsoleColor.White;
            ColorTools.LoadBack(new Color(ColorMixSettings.ColorMixBackgroundColor), true);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Set colors
            if (ColorMixSettings.ColorMixTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ColorMixSettings.ColorMixMinimumRedColorLevel, ColorMixSettings.ColorMixMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ColorMixSettings.ColorMixMinimumGreenColorLevel, ColorMixSettings.ColorMixMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ColorMixSettings.ColorMixMinimumBlueColorLevel, ColorMixSettings.ColorMixMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ColorTools.SetConsoleColor(ColorStorage, true, true);
                    ConsoleWrapper.Write(" ");
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(ColorMixSettings.ColorMixMinimumColorLevel, ColorMixSettings.ColorMixMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                    ConsoleWrapper.Write(" ");
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(ColorMixSettings.ColorMixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
