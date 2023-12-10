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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
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
                return ScreensaverPackInit.SaversConfig.GlitterColorTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.GlitterColorTrueColor = value;
            }
        }
        /// <summary>
        /// [GlitterColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GlitterColorDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                ScreensaverPackInit.SaversConfig.GlitterColorDelay = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum red color level (true color)
        /// </summary>
        public static int GlitterColorMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GlitterColorMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum green color level (true color)
        /// </summary>
        public static int GlitterColorMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GlitterColorMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum blue color level (true color)
        /// </summary>
        public static int GlitterColorMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GlitterColorMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int GlitterColorMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.GlitterColorMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum red color level (true color)
        /// </summary>
        public static int GlitterColorMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GlitterColorMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.GlitterColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GlitterColorMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum green color level (true color)
        /// </summary>
        public static int GlitterColorMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GlitterColorMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.GlitterColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GlitterColorMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum blue color level (true color)
        /// </summary>
        public static int GlitterColorMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.GlitterColorMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.GlitterColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.GlitterColorMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [GlitterColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int GlitterColorMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitterColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.GlitterColorMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.GlitterColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.GlitterColorMaximumColorLevel = value;
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
                    KernelColorTools.SetConsoleColor(ColorStorage, true);
                    ConsoleWrapper.Write(" ");
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(GlitterColorSettings.GlitterColorMinimumColorLevel, GlitterColorSettings.GlitterColorMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    KernelColorTools.SetConsoleColor(new Color(ColorNum), true);
                    ConsoleWrapper.Write(" ");
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(GlitterColorSettings.GlitterColorDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
