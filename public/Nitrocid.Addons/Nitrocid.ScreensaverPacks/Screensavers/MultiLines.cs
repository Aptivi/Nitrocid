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
    /// Settings for MultiLines
    /// </summary>
    public static class MultiLinesSettings
    {

        /// <summary>
        /// [MultiLines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool MultiLinesTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MultiLinesTrueColor = value;
            }
        }
        /// <summary>
        /// [MultiLines] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int MultiLinesDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                ScreensaverPackInit.SaversConfig.MultiLinesDelay = value;
            }
        }
        /// <summary>
        /// [MultiLines] Line character
        /// </summary>
        public static string MultiLinesLineChar
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                ScreensaverPackInit.SaversConfig.MultiLinesLineChar = value;
            }
        }
        /// <summary>
        /// [MultiLines] Screensaver background color
        /// </summary>
        public static string MultiLinesBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.MultiLinesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum red color level (true color)
        /// </summary>
        public static int MultiLinesMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.MultiLinesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum green color level (true color)
        /// </summary>
        public static int MultiLinesMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.MultiLinesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum blue color level (true color)
        /// </summary>
        public static int MultiLinesMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.MultiLinesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int MultiLinesMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.MultiLinesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum red color level (true color)
        /// </summary>
        public static int MultiLinesMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.MultiLinesMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.MultiLinesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.MultiLinesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum green color level (true color)
        /// </summary>
        public static int MultiLinesMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.MultiLinesMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.MultiLinesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.MultiLinesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum blue color level (true color)
        /// </summary>
        public static int MultiLinesMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.MultiLinesMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.MultiLinesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.MultiLinesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [MultiLines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int MultiLinesMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.MultiLinesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.MultiLinesMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.MultiLinesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.MultiLinesMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for MultiLines
    /// </summary>
    public class MultiLinesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "MultiLines";

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
            KernelColorTools.LoadBack(new Color(MultiLinesSettings.MultiLinesBackgroundColor));

            // Draw few lines
            string lineString = !string.IsNullOrWhiteSpace(MultiLinesSettings.MultiLinesLineChar) ? MultiLinesSettings.MultiLinesLineChar : "-";
            string Line = new(lineString[0], ConsoleWrapper.WindowWidth);
            int linesCount = RandomDriver.Random(1, 10);
            for (int i = 0; i < linesCount; i++)
            {
                // Select a color
                if (MultiLinesSettings.MultiLinesTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(MultiLinesSettings.MultiLinesMinimumRedColorLevel, MultiLinesSettings.MultiLinesMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(MultiLinesSettings.MultiLinesMinimumGreenColorLevel, MultiLinesSettings.MultiLinesMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(MultiLinesSettings.MultiLinesMinimumBlueColorLevel, MultiLinesSettings.MultiLinesMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                    var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    KernelColorTools.SetConsoleColor(ColorStorage);
                }
                else
                {
                    int color = RandomDriver.Random(MultiLinesSettings.MultiLinesMinimumColorLevel, MultiLinesSettings.MultiLinesMaximumColorLevel);
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                    KernelColorTools.SetConsoleColor(new Color(color));
                }

                // Now, draw a line
                int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got top position ({0})", Top);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(0, Top);
                    ConsoleWrapper.WriteLine(Line);
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(MultiLinesSettings.MultiLinesDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
