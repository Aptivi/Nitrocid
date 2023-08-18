
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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for GlitterColor
    /// </summary>
    public static class GlitterColorSettings
    {

        /// <summary>
        /// [GlitterColor] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool GlitterColorTrueColor
        {
            get
            {
                return Config.SaverConfig.GlitterColorTrueColor;
            }
            set
            {
                Config.SaverConfig.GlitterColorTrueColor = value;
            }
        }
        /// <summary>
        /// [GlitterColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GlitterColorDelay
        {
            get
            {
                return Config.SaverConfig.GlitterColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                Config.SaverConfig.GlitterColorDelay = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum red color level (true color)
        /// </summary>
        public static int GlitterColorMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.GlitterColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GlitterColorMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum green color level (true color)
        /// </summary>
        public static int GlitterColorMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.GlitterColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GlitterColorMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum blue color level (true color)
        /// </summary>
        public static int GlitterColorMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.GlitterColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GlitterColorMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int GlitterColorMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.GlitterColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.GlitterColorMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum red color level (true color)
        /// </summary>
        public static int GlitterColorMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.GlitterColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.GlitterColorMinimumRedColorLevel)
                    value = Config.SaverConfig.GlitterColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GlitterColorMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum green color level (true color)
        /// </summary>
        public static int GlitterColorMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.GlitterColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.GlitterColorMinimumGreenColorLevel)
                    value = Config.SaverConfig.GlitterColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GlitterColorMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum blue color level (true color)
        /// </summary>
        public static int GlitterColorMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.GlitterColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.GlitterColorMinimumBlueColorLevel)
                    value = Config.SaverConfig.GlitterColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.GlitterColorMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int GlitterColorMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.GlitterColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.GlitterColorMinimumColorLevel)
                    value = Config.SaverConfig.GlitterColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.GlitterColorMaximumColorLevel = value;
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
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleWrapper.SetCursorPosition(Left, Top);

            // Make a glitter color
            if (GlitterColorSettings.GlitterColorTrueColor)
            {
                int RedColorNum = RandomDriver.Random(GlitterColorSettings.GlitterColorMinimumRedColorLevel, GlitterColorSettings.GlitterColorMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(GlitterColorSettings.GlitterColorMinimumGreenColorLevel, GlitterColorSettings.GlitterColorMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(GlitterColorSettings.GlitterColorMinimumBlueColorLevel, GlitterColorSettings.GlitterColorMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    KernelColorTools.SetConsoleColor(ColorStorage, true, true);
                    ConsoleWrapper.Write(" ");
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(GlitterColorSettings.GlitterColorMinimumColorLevel, GlitterColorSettings.GlitterColorMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    KernelColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                    ConsoleWrapper.Write(" ");
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(GlitterColorSettings.GlitterColorDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
