
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
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
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
                return Config.SaverConfig.LinesTrueColor;
            }
            set
            {
                Config.SaverConfig.LinesTrueColor = value;
            }
        }
        /// <summary>
        /// [Lines] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LinesDelay
        {
            get
            {
                return Config.SaverConfig.LinesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                Config.SaverConfig.LinesDelay = value;
            }
        }
        /// <summary>
        /// [Lines] Line character
        /// </summary>
        public static string LinesLineChar
        {
            get
            {
                return Config.SaverConfig.LinesLineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                Config.SaverConfig.LinesLineChar = value;
            }
        }
        /// <summary>
        /// [Lines] Screensaver background color
        /// </summary>
        public static string LinesBackgroundColor
        {
            get
            {
                return Config.SaverConfig.LinesBackgroundColor;
            }
            set
            {
                Config.SaverConfig.LinesBackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Lines] The minimum red color level (true color)
        /// </summary>
        public static int LinesMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.LinesMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LinesMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum green color level (true color)
        /// </summary>
        public static int LinesMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.LinesMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LinesMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum blue color level (true color)
        /// </summary>
        public static int LinesMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.LinesMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LinesMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LinesMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.LinesMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.LinesMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum red color level (true color)
        /// </summary>
        public static int LinesMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.LinesMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.LinesMinimumRedColorLevel)
                    value = Config.SaverConfig.LinesMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LinesMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum green color level (true color)
        /// </summary>
        public static int LinesMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.LinesMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.LinesMinimumGreenColorLevel)
                    value = Config.SaverConfig.LinesMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LinesMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum blue color level (true color)
        /// </summary>
        public static int LinesMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.LinesMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.LinesMinimumBlueColorLevel)
                    value = Config.SaverConfig.LinesMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.LinesMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LinesMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.LinesMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.LinesMinimumColorLevel)
                    value = Config.SaverConfig.LinesMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.LinesMaximumColorLevel = value;
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
                KernelColorTools.LoadBack(new Color(LinesSettings.LinesBackgroundColor), true);
                int RedColorNum = RandomDriver.Random(LinesSettings.LinesMinimumRedColorLevel, LinesSettings.LinesMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(LinesSettings.LinesMinimumGreenColorLevel, LinesSettings.LinesMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(LinesSettings.LinesMinimumBlueColorLevel, LinesSettings.LinesMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                KernelColorTools.SetConsoleColor(ColorStorage);
            }
            else
            {
                KernelColorTools.LoadBack(new Color(LinesSettings.LinesBackgroundColor), true);
                int color = RandomDriver.Random(LinesSettings.LinesMinimumColorLevel, LinesSettings.LinesMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                KernelColorTools.SetConsoleColor(new Color(color));
            }

            // Draw a line
            string Line = "";
            int Top = new Random().Next(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got top position ({0})", Top);
            for (int i = 1; i <= ConsoleWrapper.WindowWidth; i++)
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Forming line using {0} or the default \"-\"...", LinesSettings.LinesLineChar);
                Line += !string.IsNullOrWhiteSpace(LinesSettings.LinesLineChar) ? LinesSettings.LinesLineChar : "-";
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Line: {0}", Line);
            }
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
