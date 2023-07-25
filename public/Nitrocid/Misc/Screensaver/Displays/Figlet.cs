
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
using System.Linq;
using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters.Tools;

namespace KS.Misc.Screensaver.Displays
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
                return Config.SaverConfig.FigletTrueColor;
            }
            set
            {
                Config.SaverConfig.FigletTrueColor = value;
            }
        }
        /// <summary>
        /// [Figlet] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FigletDelay
        {
            get
            {
                return Config.SaverConfig.FigletDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                Config.SaverConfig.FigletDelay = value;
            }
        }
        /// <summary>
        /// [Figlet] Text for Figlet. Shorter is better.
        /// </summary>
        public static string FigletText
        {
            get
            {
                return Config.SaverConfig.FigletText;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                Config.SaverConfig.FigletText = value;
            }
        }
        /// <summary>
        /// [Figlet] Figlet font supported by the figlet library used.
        /// </summary>
        public static string FigletFont
        {
            get
            {
                return Config.SaverConfig.FigletFont;
            }
            set
            {
                Config.SaverConfig.FigletFont = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum red color level (true color)
        /// </summary>
        public static int FigletMinimumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.FigletMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FigletMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum green color level (true color)
        /// </summary>
        public static int FigletMinimumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.FigletMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FigletMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum blue color level (true color)
        /// </summary>
        public static int FigletMinimumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.FigletMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FigletMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FigletMinimumColorLevel
        {
            get
            {
                return Config.SaverConfig.FigletMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                Config.SaverConfig.FigletMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum red color level (true color)
        /// </summary>
        public static int FigletMaximumRedColorLevel
        {
            get
            {
                return Config.SaverConfig.FigletMaximumRedColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FigletMinimumRedColorLevel)
                    value = Config.SaverConfig.FigletMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FigletMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum green color level (true color)
        /// </summary>
        public static int FigletMaximumGreenColorLevel
        {
            get
            {
                return Config.SaverConfig.FigletMaximumGreenColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FigletMinimumGreenColorLevel)
                    value = Config.SaverConfig.FigletMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FigletMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum blue color level (true color)
        /// </summary>
        public static int FigletMaximumBlueColorLevel
        {
            get
            {
                return Config.SaverConfig.FigletMaximumBlueColorLevel;
            }
            set
            {
                if (value <= Config.SaverConfig.FigletMinimumBlueColorLevel)
                    value = Config.SaverConfig.FigletMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                Config.SaverConfig.FigletMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FigletMaximumColorLevel
        {
            get
            {
                return Config.SaverConfig.FigletMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= Config.SaverConfig.FigletMinimumColorLevel)
                    value = Config.SaverConfig.FigletMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                Config.SaverConfig.FigletMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Figlet
    /// </summary>
    public class FigletDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Figlet";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() =>
            KernelColorTools.LoadBack();

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
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(FigletSettings.FigletMinimumColorLevel, FigletSettings.FigletMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                ColorStorage = new Color(ColorNum);
            }

            // Prepare the figlet font for writing
            string FigletWrite = FigletSettings.FigletText.ReplaceAll(new string[] { Convert.ToChar(13).ToString(), Convert.ToChar(10).ToString() }, " - ");
            FigletWrite = FigletFontUsed.Render(FigletWrite);
            var FigletWriteLines = FigletWrite.SplitNewLines().SkipWhile(x => string.IsNullOrEmpty(x)).ToArray();
            int FigletHeight = (int)Math.Round(ConsoleMiddleHeight - FigletWriteLines.Length / 2d);
            int FigletWidth = (int)Math.Round(ConsoleMiddleWidth - FigletWriteLines[0].Length / 2d);

            // Actually write it
            if (!ConsoleResizeListener.WasResized(false))
                TextWriterWhereColor.WriteWhere(FigletWrite, FigletWidth, FigletHeight, true, ColorStorage);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(FigletSettings.FigletDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
