
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
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for FlashColor
    /// </summary>
    public static class FlashColorSettings
    {

        /// <summary>
        /// [FlashColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FlashColorTrueColor
        {
            get
            {
                return Config.SaverConfig.FlashColorTrueColor;
            }
            set
            {
                Config.SaverConfig.FlashColorTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FlashColorDelay
        {
            get
            {
                return Config.SaverConfig.FlashColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                Config.SaverConfig.FlashColorDelay = value;
            }
        }
        /// <summary>
        /// [FlashColor] Screensaver background color
        /// </summary>
        public static string FlashColorBackgroundColor
        {
            get
            {
                return Config.SaverConfig.FlashColorBackgroundColor;
            }
            set
            {
                Config.SaverConfig.FlashColorBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum red color level (true color)
        /// </summary>
        public static int FlashColorMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashColorMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum green color level (true color)
        /// </summary>
        public static int FlashColorMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashColorMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum blue color level (true color)
        /// </summary>
        public static int FlashColorMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashColorMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashColorMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.FlashColorMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum red color level (true color)
        /// </summary>
        public static int FlashColorMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FlashColorMinimumRedColorLevel)
                    value = Config.SaverConfig.FlashColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashColorMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum green color level (true color)
        /// </summary>
        public static int FlashColorMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FlashColorMinimumGreenColorLevel)
                    value = Config.SaverConfig.FlashColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashColorMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum blue color level (true color)
        /// </summary>
        public static int FlashColorMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FlashColorMinimumBlueColorLevel)
                    value = Config.SaverConfig.FlashColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashColorMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashColorMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.FlashColorMinimumColorLevel)
                    value = Config.SaverConfig.FlashColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.FlashColorMaximumColorLevel = value;
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
            KernelColorTools.LoadBack(new Color(FlashColorSettings.FlashColorBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.Random(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleWrapper.SetCursorPosition(Left, Top);

            // Make a flash color
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleExtensions.ClearKeepPosition();
            if (FlashColorSettings.FlashColorTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumRedColorLevel, FlashColorSettings.FlashColorMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumGreenColorLevel, FlashColorSettings.FlashColorMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumBlueColorLevel, FlashColorSettings.FlashColorMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    KernelColorTools.SetConsoleColor(ColorStorage, true);
                    ConsoleWrapper.Write(" ");
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumColorLevel, FlashColorSettings.FlashColorMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    KernelColorTools.SetConsoleColor(new Color(ColorNum), true);
                    ConsoleWrapper.Write(" ");
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(FlashColorSettings.FlashColorDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
