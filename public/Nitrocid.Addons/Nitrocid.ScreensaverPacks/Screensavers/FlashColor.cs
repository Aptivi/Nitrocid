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

using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;

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
                return ScreensaverPackInit.SaversConfig.FlashColorTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FlashColorTrueColor = value;
            }
        }
        /// <summary>
        /// [FlashColor] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FlashColorDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashColorDelay;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                ScreensaverPackInit.SaversConfig.FlashColorDelay = value;
            }
        }
        /// <summary>
        /// [FlashColor] Screensaver background color
        /// </summary>
        public static string FlashColorBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashColorBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FlashColorBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum red color level (true color)
        /// </summary>
        public static int FlashColorMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashColorMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashColorMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum green color level (true color)
        /// </summary>
        public static int FlashColorMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashColorMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashColorMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum blue color level (true color)
        /// </summary>
        public static int FlashColorMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashColorMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashColorMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashColorMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashColorMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.FlashColorMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum red color level (true color)
        /// </summary>
        public static int FlashColorMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashColorMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FlashColorMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FlashColorMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashColorMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum green color level (true color)
        /// </summary>
        public static int FlashColorMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashColorMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FlashColorMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FlashColorMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashColorMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum blue color level (true color)
        /// </summary>
        public static int FlashColorMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashColorMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FlashColorMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FlashColorMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FlashColorMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [FlashColor] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FlashColorMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FlashColorMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.FlashColorMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FlashColorMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.FlashColorMaximumColorLevel = value;
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
            ColorTools.LoadBack(new Color(FlashColorSettings.FlashColorBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ColorTools.LoadBack(new Color(ConsoleColors.Black));
            ConsoleWrapper.SetCursorPosition(Left, Top);

            // Make a flash color
            if (FlashColorSettings.FlashColorTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumRedColorLevel, FlashColorSettings.FlashColorMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumGreenColorLevel, FlashColorSettings.FlashColorMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumBlueColorLevel, FlashColorSettings.FlashColorMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ColorTools.SetConsoleColor(ColorStorage, true);
                    ConsoleWrapper.Write(" ");
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(FlashColorSettings.FlashColorMinimumColorLevel, FlashColorSettings.FlashColorMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ColorTools.SetConsoleColor(new Color(ColorNum), true);
                    ConsoleWrapper.Write(" ");
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ThreadManager.SleepNoBlock(FlashColorSettings.FlashColorDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
