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
    /// Settings for Lines
    /// </summary>
    public static class LinesSettings
    {

        /// <summary>
        /// [Lines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool LinesTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LinesTrueColor = value;
            }
        }
        /// <summary>
        /// [Lines] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LinesDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                ScreensaverPackInit.SaversConfig.LinesDelay = value;
            }
        }
        /// <summary>
        /// [Lines] Line character
        /// </summary>
        public static string LinesLineChar
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                ScreensaverPackInit.SaversConfig.LinesLineChar = value;
            }
        }
        /// <summary>
        /// [Lines] Screensaver background color
        /// </summary>
        public static string LinesBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LinesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Lines] The minimum red color level (true color)
        /// </summary>
        public static int LinesMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LinesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum green color level (true color)
        /// </summary>
        public static int LinesMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LinesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum blue color level (true color)
        /// </summary>
        public static int LinesMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LinesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LinesMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.LinesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum red color level (true color)
        /// </summary>
        public static int LinesMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.LinesMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LinesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LinesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum green color level (true color)
        /// </summary>
        public static int LinesMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.LinesMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LinesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LinesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum blue color level (true color)
        /// </summary>
        public static int LinesMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.LinesMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LinesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.LinesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LinesMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LinesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.LinesMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.LinesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.LinesMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Lines
    /// </summary>
    public class LinesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Lines";

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

            // Select a color
            if (LinesSettings.LinesTrueColor)
            {
                KernelColorTools.LoadBack(new Color(LinesSettings.LinesBackgroundColor));
                int RedColorNum = RandomDriver.Random(LinesSettings.LinesMinimumRedColorLevel, LinesSettings.LinesMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(LinesSettings.LinesMinimumGreenColorLevel, LinesSettings.LinesMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(LinesSettings.LinesMinimumBlueColorLevel, LinesSettings.LinesMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                KernelColorTools.SetConsoleColor(ColorStorage);
            }
            else
            {
                KernelColorTools.LoadBack(new Color(LinesSettings.LinesBackgroundColor));
                int color = RandomDriver.Random(LinesSettings.LinesMinimumColorLevel, LinesSettings.LinesMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                KernelColorTools.SetConsoleColor(new Color(color));
            }

            // Draw a line
            string lineString = !string.IsNullOrWhiteSpace(LinesSettings.LinesLineChar) ? LinesSettings.LinesLineChar : "-";
            string Line = new(lineString[0], ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got top position ({0})", Top);
            if (!ConsoleResizeListener.WasResized(false))
            {
                ConsoleWrapper.SetCursorPosition(0, Top);
                ConsoleWrapper.WriteLine(Line);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(LinesSettings.LinesDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
