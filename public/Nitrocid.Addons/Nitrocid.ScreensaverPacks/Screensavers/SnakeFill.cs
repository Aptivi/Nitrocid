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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for SnakeFill
    /// </summary>
    public static class SnakeFillSettings
    {

        /// <summary>
        /// [SnakeFill] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool SnakeFillTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SnakeFillTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SnakeFillTrueColor = value;
            }
        }
        /// <summary>
        /// [SnakeFill] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SnakeFillDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SnakeFillDelay;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SnakeFillDelay = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum red color level (true color)
        /// </summary>
        public static int SnakeFillMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SnakeFillMinimumRedColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SnakeFillMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum green color level (true color)
        /// </summary>
        public static int SnakeFillMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SnakeFillMinimumGreenColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SnakeFillMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum blue color level (true color)
        /// </summary>
        public static int SnakeFillMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SnakeFillMinimumBlueColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SnakeFillMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakeFillMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SnakeFillMinimumColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SnakeFillMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum red color level (true color)
        /// </summary>
        public static int SnakeFillMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SnakeFillMaximumRedColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SnakeFillMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum green color level (true color)
        /// </summary>
        public static int SnakeFillMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SnakeFillMaximumGreenColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SnakeFillMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum blue color level (true color)
        /// </summary>
        public static int SnakeFillMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SnakeFillMaximumBlueColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SnakeFillMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [SnakeFill] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int SnakeFillMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SnakeFillMaximumColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SnakeFillMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for SnakeFill
    /// </summary>
    public class SnakeFillDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "SnakeFill";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a color
            if (SnakeFillSettings.SnakeFillTrueColor)
            {
                int RedColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumRedColorLevel, SnakeFillSettings.SnakeFillMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumGreenColorLevel, SnakeFillSettings.SnakeFillMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumBlueColorLevel, SnakeFillSettings.SnakeFillMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    ColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(SnakeFillSettings.SnakeFillMinimumColorLevel, SnakeFillSettings.SnakeFillMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                    ColorTools.SetConsoleColor(new Color(ColorNum), true);
            }

            // Set max height
            int MaxWindowHeight = ConsoleWrapper.WindowHeight - 1;
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Max height {0}", MaxWindowHeight);

            // Fill the screen!
            bool reverseHeightAxis = false;
            for (int x = 0; x < ConsoleWrapper.WindowWidth; x++)
            {
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Select the height and fill the entire screen
                if (reverseHeightAxis)
                {
                    for (int y = MaxWindowHeight; y >= 0; y--)
                    {
                        if (ConsoleResizeListener.WasResized(false))
                            break;

                        TextWriterWhereColor.WriteWhere(" ", x, y);
                        ThreadManager.SleepNoBlock(SnakeFillSettings.SnakeFillDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        reverseHeightAxis = false;
                    }
                }
                else
                {
                    for (int y = 0; y <= MaxWindowHeight; y++)
                    {
                        if (ConsoleResizeListener.WasResized(false))
                            break;

                        TextWriterWhereColor.WriteWhere(" ", x, y);
                        ThreadManager.SleepNoBlock(SnakeFillSettings.SnakeFillDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        reverseHeightAxis = true;
                    }
                }
            }

            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(SnakeFillSettings.SnakeFillDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
