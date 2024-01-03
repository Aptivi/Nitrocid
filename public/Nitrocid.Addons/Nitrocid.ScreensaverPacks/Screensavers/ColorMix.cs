//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
                return ScreensaverPackInit.SaversConfig.ColorMixTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.ColorMixTrueColor = value;
            }
        }
        /// <summary>
        /// [ColorMix] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int ColorMixDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorMixDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                ScreensaverPackInit.SaversConfig.ColorMixDelay = value;
            }
        }
        /// <summary>
        /// [ColorMix] Screensaver background color
        /// </summary>
        public static string ColorMixBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorMixBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.ColorMixBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum red color level (true color)
        /// </summary>
        public static int ColorMixMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorMixMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorMixMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum green color level (true color)
        /// </summary>
        public static int ColorMixMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorMixMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorMixMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum blue color level (true color)
        /// </summary>
        public static int ColorMixMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorMixMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorMixMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorMixMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorMixMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.ColorMixMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum red color level (true color)
        /// </summary>
        public static int ColorMixMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorMixMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ColorMixMaximumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ColorMixMaximumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorMixMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum green color level (true color)
        /// </summary>
        public static int ColorMixMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorMixMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ColorMixMaximumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ColorMixMaximumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorMixMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum blue color level (true color)
        /// </summary>
        public static int ColorMixMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorMixMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.ColorMixMaximumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ColorMixMaximumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.ColorMixMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [ColorMix] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int ColorMixMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.ColorMixMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.ColorMixMaximumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.ColorMixMaximumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.ColorMixMaximumColorLevel = value;
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
            if (ConsoleWrapper.CursorLeft >= EndLeft && ConsoleWrapper.CursorTop >= EndTop)
                ConsoleWrapper.SetCursorPosition(0, 0);
            else
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

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(ColorMixSettings.ColorMixDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
