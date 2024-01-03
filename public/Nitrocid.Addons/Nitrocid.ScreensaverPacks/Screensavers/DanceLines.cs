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
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for DanceLines
    /// </summary>
    public static class DanceLinesSettings
    {

        /// <summary>
        /// [DanceLines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool DanceLinesTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DanceLinesTrueColor = value;
            }
        }
        /// <summary>
        /// [DanceLines] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int DanceLinesDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.DanceLinesDelay = value;
            }
        }
        /// <summary>
        /// [DanceLines] Line character
        /// </summary>
        public static string DanceLinesLineChar
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                ScreensaverPackInit.SaversConfig.DanceLinesLineChar = value;
            }
        }
        /// <summary>
        /// [DanceLines] Screensaver background color
        /// </summary>
        public static string DanceLinesBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.DanceLinesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum red color level (true color)
        /// </summary>
        public static int DanceLinesMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DanceLinesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum green color level (true color)
        /// </summary>
        public static int DanceLinesMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DanceLinesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum blue color level (true color)
        /// </summary>
        public static int DanceLinesMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DanceLinesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int DanceLinesMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.DanceLinesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum red color level (true color)
        /// </summary>
        public static int DanceLinesMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.DanceLinesMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DanceLinesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DanceLinesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum green color level (true color)
        /// </summary>
        public static int DanceLinesMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.DanceLinesMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DanceLinesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DanceLinesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum blue color level (true color)
        /// </summary>
        public static int DanceLinesMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.DanceLinesMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DanceLinesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.DanceLinesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [DanceLines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int DanceLinesMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.DanceLinesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.DanceLinesMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.DanceLinesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.DanceLinesMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for DanceLines
    /// </summary>
    public class DanceLinesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "DanceLines";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            KernelColorTools.LoadBack(new Color(DanceLinesSettings.DanceLinesBackgroundColor));

            // Draw few lines
            string lineString = !string.IsNullOrWhiteSpace(DanceLinesSettings.DanceLinesLineChar) ? DanceLinesSettings.DanceLinesLineChar : "-";
            for (int i = 0; i < ConsoleWrapper.WindowHeight; i++)
            {
                // Draw a randomly-sized line
                string line = new(lineString[0], RandomDriver.Random(ConsoleWrapper.WindowWidth));

                // Select a color
                if (DanceLinesSettings.DanceLinesTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(DanceLinesSettings.DanceLinesMinimumRedColorLevel, DanceLinesSettings.DanceLinesMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(DanceLinesSettings.DanceLinesMinimumGreenColorLevel, DanceLinesSettings.DanceLinesMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(DanceLinesSettings.DanceLinesMinimumBlueColorLevel, DanceLinesSettings.DanceLinesMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                    var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    KernelColorTools.SetConsoleColor(ColorStorage);
                }
                else
                {
                    int color = RandomDriver.Random(DanceLinesSettings.DanceLinesMinimumColorLevel, DanceLinesSettings.DanceLinesMaximumColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                    KernelColorTools.SetConsoleColor(new Color(color));
                }

                // Now, draw a line
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got top position ({0})", i);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(0, i);
                    ConsoleWrapper.Write(line);
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(DanceLinesSettings.DanceLinesDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
