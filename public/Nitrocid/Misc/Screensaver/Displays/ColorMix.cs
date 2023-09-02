
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
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for ColorMix
    /// </summary>
    public static class ColorMixSettings
    {

        /// <summary>
        /// [ColorMix] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool ColorMixTrueColor
        {
            get
            {
                return Config.SaverConfig.ColorMixTrueColor;
            }
            set
            {
                Config.SaverConfig.ColorMixTrueColor = value;
            }
        }
        /// <summary>
        /// [ColorMix] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ColorMixDelay
        {
            get
            {
                return Config.SaverConfig.ColorMixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                Config.SaverConfig.ColorMixDelay = value;
            }
        }
        /// <summary>
        /// [ColorMix] Screensaver background color
        /// </summary>
        public static string ColorMixBackgroundColor
        {
            get
            {
                return Config.SaverConfig.ColorMixBackgroundColor;
            }
            set
            {
                Config.SaverConfig.ColorMixBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum red color level (true color)
        /// </summary>
        public static int ColorMixMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorMixMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorMixMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum green color level (true color)
        /// </summary>
        public static int ColorMixMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorMixMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorMixMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum blue color level (true color)
        /// </summary>
        public static int ColorMixMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorMixMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorMixMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorMixMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorMixMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.ColorMixMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum red color level (true color)
        /// </summary>
        public static int ColorMixMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorMixMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.ColorMixMaximumRedColorLevel)
                    value = Config.SaverConfig.ColorMixMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorMixMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum green color level (true color)
        /// </summary>
        public static int ColorMixMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorMixMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.ColorMixMaximumGreenColorLevel)
                    value = Config.SaverConfig.ColorMixMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorMixMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum blue color level (true color)
        /// </summary>
        public static int ColorMixMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorMixMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.ColorMixMaximumBlueColorLevel)
                    value = Config.SaverConfig.ColorMixMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.ColorMixMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorMixMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.ColorMixMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.ColorMixMaximumColorLevel)
                    value = Config.SaverConfig.ColorMixMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.ColorMixMaximumColorLevel = value;
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
            KernelColorTools.LoadBack(new Color(ColorMixSettings.ColorMixBackgroundColor));
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int EndLeft = ConsoleWrapper.WindowWidth - 1;
            int EndTop = ConsoleWrapper.WindowHeight - 1;
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", EndLeft, EndTop);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", Left, Top);

            // Fill the color if not filled
            if (!(ConsoleWrapper.CursorLeft >= EndLeft & ConsoleWrapper.CursorTop >= EndTop))
            {
                Color colorStorage;
                if (ColorMixSettings.ColorMixTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(ColorMixSettings.ColorMixMinimumRedColorLevel, ColorMixSettings.ColorMixMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(ColorMixSettings.ColorMixMinimumGreenColorLevel, ColorMixSettings.ColorMixMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(ColorMixSettings.ColorMixMinimumBlueColorLevel, ColorMixSettings.ColorMixMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                    colorStorage = new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");
                }
                else
                {
                    int ColorNum = RandomDriver.Random(ColorMixSettings.ColorMixMinimumColorLevel, ColorMixSettings.ColorMixMaximumColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                    colorStorage = new Color(ColorNum);
                }

                if (!ConsoleResizeListener.WasResized(false))
                {
                    KernelColorTools.SetConsoleColor(Color.Empty);
                    KernelColorTools.SetConsoleColor(colorStorage, true);
                    TextWriterColor.WritePlain(" ", false);
                }
                else
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                    KernelColorTools.LoadBack(new Color(ColorMixSettings.ColorMixBackgroundColor));
                }
            }
            else
                ConsoleWrapper.SetCursorPosition(0, 0);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(ColorMixSettings.ColorMixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
