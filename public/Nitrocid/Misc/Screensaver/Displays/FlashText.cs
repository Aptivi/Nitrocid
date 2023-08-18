
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
    /// Settings for FlashText
    /// </summary>
    public static class FlashTextSettings
    {

        /// <summary>
        /// [FlashText] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FlashTextTrueColor
        {
            get
            {
                return Config.SaverConfig.FlashTextTrueColor;
            }
            set
            {
                Config.SaverConfig.FlashTextTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashText] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FlashTextDelay
        {
            get
            {
                return Config.SaverConfig.FlashTextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                Config.SaverConfig.FlashTextDelay = value;
            }
        }
        /// <summary>
        /// [FlashText] Text for FlashText. Shorter is better.
        /// </summary>
        public static string FlashTextWrite
        {
            get
            {
                return Config.SaverConfig.FlashTextWrite;
            }
            set
            {
                Config.SaverConfig.FlashTextWrite = value;
            }
        }
        /// <summary>
        /// [FlashText] Screensaver background color
        /// </summary>
        public static string FlashTextBackgroundColor
        {
            get
            {
                return Config.SaverConfig.FlashTextBackgroundColor;
            }
            set
            {
                Config.SaverConfig.FlashTextBackgroundColor = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum red color level (true color)
        /// </summary>
        public static int FlashTextMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashTextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashTextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum green color level (true color)
        /// </summary>
        public static int FlashTextMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashTextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashTextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum blue color level (true color)
        /// </summary>
        public static int FlashTextMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashTextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashTextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashTextMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashTextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.FlashTextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum red color level (true color)
        /// </summary>
        public static int FlashTextMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashTextMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FlashTextMinimumRedColorLevel)
                    value = Config.SaverConfig.FlashTextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashTextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum green color level (true color)
        /// </summary>
        public static int FlashTextMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashTextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FlashTextMinimumGreenColorLevel)
                    value = Config.SaverConfig.FlashTextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashTextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum blue color level (true color)
        /// </summary>
        public static int FlashTextMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashTextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FlashTextMinimumBlueColorLevel)
                    value = Config.SaverConfig.FlashTextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FlashTextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashTextMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.FlashTextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.FlashTextMinimumColorLevel)
                    value = Config.SaverConfig.FlashTextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.FlashTextMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for FlashText
    /// </summary>
    public class FlashTextDisplay : BaseScreensaver, IScreensaver
    {

        private int Left, Top;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "FlashText";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            KernelColorTools.LoadBack(new Color(FlashTextSettings.FlashTextBackgroundColor), true);
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);

            // Select position
            Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            Top = RandomDriver.Random(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Make two delay halves to make up one half for screen with text and one half for screen with no text to make a flashing effect
            int HalfDelay = (int)Math.Round(FlashTextSettings.FlashTextDelay / 2d);

            // Make a flashing text
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();
            if (FlashTextSettings.FlashTextTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumRedColorLevel, FlashTextSettings.FlashTextMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumGreenColorLevel, FlashTextSettings.FlashTextMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumBlueColorLevel, FlashTextSettings.FlashTextMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    TextWriterWhereColor.WriteWhere(FlashTextSettings.FlashTextWrite, Left, Top, true, ColorStorage, FlashTextSettings.FlashTextBackgroundColor);
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumColorLevel, FlashTextSettings.FlashTextMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    TextWriterWhereColor.WriteWhere(FlashTextSettings.FlashTextWrite, Left, Top, true, new Color(ColorNum), FlashTextSettings.FlashTextBackgroundColor);
                }
            }
            ThreadManager.SleepNoBlock(HalfDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleWrapper.Clear();
            ThreadManager.SleepNoBlock(HalfDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
