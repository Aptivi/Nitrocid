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

using System;
using System.Linq;
using Figletize;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Textify.General;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Figlet
    /// </summary>
    public static class FigletSettings
    {

        /// <summary>
        /// [Figlet] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FigletTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FigletTrueColor = value;
            }
        }
        /// <summary>
        /// [Figlet] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FigletDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                ScreensaverPackInit.SaversConfig.FigletDelay = value;
            }
        }
        /// <summary>
        /// [Figlet] Text for Figlet. Shorter is better.
        /// </summary>
        public static string FigletText
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletText;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                ScreensaverPackInit.SaversConfig.FigletText = value;
            }
        }
        /// <summary>
        /// [Figlet] Figlet font supported by the figlet library used.
        /// </summary>
        public static string FigletFont
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletFont;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FigletFont = FigletTools.GetFigletFonts().ContainsKey(value) ? value : "small";
            }
        }
        /// <summary>
        /// [Figlet] Enables the rainbow colors mode
        /// </summary>
        public static bool FigletRainbowMode
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletRainbowMode;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.FigletRainbowMode = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum red color level (true color)
        /// </summary>
        public static int FigletMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FigletMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum green color level (true color)
        /// </summary>
        public static int FigletMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FigletMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum blue color level (true color)
        /// </summary>
        public static int FigletMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FigletMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FigletMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.FigletMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum red color level (true color)
        /// </summary>
        public static int FigletMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FigletMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FigletMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FigletMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum green color level (true color)
        /// </summary>
        public static int FigletMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FigletMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FigletMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FigletMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum blue color level (true color)
        /// </summary>
        public static int FigletMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.FigletMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FigletMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.FigletMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FigletMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.FigletMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.FigletMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.FigletMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.FigletMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Figlet
    /// </summary>
    public class FigletDisplay : BaseScreensaver, IScreensaver
    {

        private int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Figlet";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() =>
            ColorTools.LoadBack();

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int ConsoleMiddleWidth = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d);
            int ConsoleMiddleHeight = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            var FigletFontUsed = FigletTools.GetFigletFont(FigletSettings.FigletFont);
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Set colors
            var ColorStorage = new Color(255, 255, 255);
            if (FigletSettings.FigletTrueColor)
            {
                int RedColorNum = RandomDriver.Random(FigletSettings.FigletMinimumRedColorLevel, FigletSettings.FigletMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(FigletSettings.FigletMinimumGreenColorLevel, FigletSettings.FigletMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(FigletSettings.FigletMinimumBlueColorLevel, FigletSettings.FigletMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(FigletSettings.FigletMinimumColorLevel, FigletSettings.FigletMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                ColorStorage = new Color(ColorNum);
            }
            if (FigletSettings.FigletRainbowMode)
            {
                ColorStorage = new($"hsl:{currentHueAngle};100;50");
                currentHueAngle++;
                if (currentHueAngle > 360)
                    currentHueAngle = 0;
            }

            // Prepare the figlet font for writing
            string FigletWrite = FigletSettings.FigletText.ReplaceAll([Convert.ToChar(13).ToString(), Convert.ToChar(10).ToString()], " - ");
            FigletWrite = FigletFontUsed.Render(FigletWrite);
            var FigletWriteLines = FigletWrite.SplitNewLines().SkipWhile(string.IsNullOrEmpty).ToArray();
            int FigletHeight = (int)Math.Round(ConsoleMiddleHeight - FigletWriteLines.Length / 2d);
            int FigletWidth = (int)Math.Round(ConsoleMiddleWidth - FigletWriteLines[0].Length / 2d);

            // Actually write it
            if (!ConsoleResizeHandler.WasResized(false))
                TextWriterWhereColor.WriteWhereColor(FigletWrite, FigletWidth, FigletHeight, true, ColorStorage);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            int delay = FigletSettings.FigletRainbowMode ? 16 : FigletSettings.FigletDelay;
            ThreadManager.SleepNoBlock(delay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

    }
}
