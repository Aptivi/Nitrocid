//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
                return ScreensaverPackInit.SaversConfig.FlashTextTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FlashTextTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashText] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FlashTextDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                ScreensaverPackInit.SaversConfig.FlashTextDelay = value;
            }
        }
        /// <summary>
        /// [FlashText] Text for FlashText. Shorter is better.
        /// </summary>
        public static string FlashTextWrite
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextWrite;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FlashTextWrite = value;
            }
        }
        /// <summary>
        /// [FlashText] Screensaver background color
        /// </summary>
        public static string FlashTextBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FlashTextBackgroundColor = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum red color level (true color)
        /// </summary>
        public static int FlashTextMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashTextMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum green color level (true color)
        /// </summary>
        public static int FlashTextMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashTextMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum blue color level (true color)
        /// </summary>
        public static int FlashTextMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashTextMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashTextMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.FlashTextMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum red color level (true color)
        /// </summary>
        public static int FlashTextMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FlashTextMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FlashTextMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashTextMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum green color level (true color)
        /// </summary>
        public static int FlashTextMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FlashTextMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FlashTextMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashTextMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum blue color level (true color)
        /// </summary>
        public static int FlashTextMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FlashTextMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FlashTextMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashTextMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashText] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashTextMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashTextMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.FlashTextMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FlashTextMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.FlashTextMaximumColorLevel = value;
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
            KernelColorTools.LoadBack(new Color(FlashTextSettings.FlashTextBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);

            // Select position
            Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth - FlashTextSettings.FlashTextWrite.Length);
            Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Make two delay halves to make up one half for screen with text and one half for screen with no text to make a flashing effect
            int HalfDelay = (int)Math.Round(FlashTextSettings.FlashTextDelay / 2d);

            // Make a flashing text
            if (FlashTextSettings.FlashTextTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumRedColorLevel, FlashTextSettings.FlashTextMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumGreenColorLevel, FlashTextSettings.FlashTextMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumBlueColorLevel, FlashTextSettings.FlashTextMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    TextWriterWhereColor.WriteWhereColorBack(FlashTextSettings.FlashTextWrite, Left, Top, true, ColorStorage, FlashTextSettings.FlashTextBackgroundColor);
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(FlashTextSettings.FlashTextMinimumColorLevel, FlashTextSettings.FlashTextMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    TextWriterWhereColor.WriteWhereColorBack(FlashTextSettings.FlashTextWrite, Left, Top, true, new Color(ColorNum), FlashTextSettings.FlashTextBackgroundColor);
                }
            }
            ThreadManager.SleepNoBlock(HalfDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            KernelColorTools.LoadBack(new Color(ConsoleColors.Black));
            ThreadManager.SleepNoBlock(HalfDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

    }
}
