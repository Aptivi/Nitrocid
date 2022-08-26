
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using ColorSeq;
using Extensification.StringExts;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Misc.Writers.WriterBase;

namespace KS.Misc.Screensaver.Displays
{
    public static class FigletSettings
    {

        private static bool _figlet255Colors;
        private static bool _figletTrueColor = true;
        private static int _figletDelay = 1000;
        private static string _figletText = "Kernel Simulator";
        private static string _figletFont = "Small";
        private static int _figletMinimumRedColorLevel = 0;
        private static int _figletMinimumGreenColorLevel = 0;
        private static int _figletMinimumBlueColorLevel = 0;
        private static int _figletMinimumColorLevel = 0;
        private static int _figletMaximumRedColorLevel = 255;
        private static int _figletMaximumGreenColorLevel = 255;
        private static int _figletMaximumBlueColorLevel = 255;
        private static int _figletMaximumColorLevel = 255;

        /// <summary>
        /// [Figlet] Enable 255 color support. Has a higher priority than 16 color support.
        /// </summary>
        public static bool Figlet255Colors
        {
            get
            {
                return _figlet255Colors;
            }
            set
            {
                _figlet255Colors = value;
            }
        }
        /// <summary>
        /// [Figlet] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool FigletTrueColor
        {
            get
            {
                return _figletTrueColor;
            }
            set
            {
                _figletTrueColor = value;
            }
        }
        /// <summary>
        /// [Figlet] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int FigletDelay
        {
            get
            {
                return _figletDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                _figletDelay = value;
            }
        }
        /// <summary>
        /// [Figlet] Text for Figlet. Shorter is better.
        /// </summary>
        public static string FigletText
        {
            get
            {
                return _figletText;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Kernel Simulator";
                _figletText = value;
            }
        }
        /// <summary>
        /// [Figlet] Figlet font supported by the figlet library used.
        /// </summary>
        public static string FigletFont
        {
            get
            {
                return _figletFont;
            }
            set
            {
                _figletFont = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum red color level (true color)
        /// </summary>
        public static int FigletMinimumRedColorLevel
        {
            get
            {
                return _figletMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _figletMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum green color level (true color)
        /// </summary>
        public static int FigletMinimumGreenColorLevel
        {
            get
            {
                return _figletMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _figletMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum blue color level (true color)
        /// </summary>
        public static int FigletMinimumBlueColorLevel
        {
            get
            {
                return _figletMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _figletMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int FigletMinimumColorLevel
        {
            get
            {
                return _figletMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = _figlet255Colors | _figletTrueColor ? 255 : 15;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _figletMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum red color level (true color)
        /// </summary>
        public static int FigletMaximumRedColorLevel
        {
            get
            {
                return _figletMaximumRedColorLevel;
            }
            set
            {
                if (value <= _figletMinimumRedColorLevel)
                    value = _figletMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _figletMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum green color level (true color)
        /// </summary>
        public static int FigletMaximumGreenColorLevel
        {
            get
            {
                return _figletMaximumGreenColorLevel;
            }
            set
            {
                if (value <= _figletMinimumGreenColorLevel)
                    value = _figletMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _figletMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum blue color level (true color)
        /// </summary>
        public static int FigletMaximumBlueColorLevel
        {
            get
            {
                return _figletMaximumBlueColorLevel;
            }
            set
            {
                if (value <= _figletMinimumBlueColorLevel)
                    value = _figletMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _figletMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Figlet] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int FigletMaximumColorLevel
        {
            get
            {
                return _figletMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = _figlet255Colors | _figletTrueColor ? 255 : 15;
                if (value <= _figletMinimumColorLevel)
                    value = _figletMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _figletMaximumColorLevel = value;
            }
        }

    }
    public class FigletDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "Figlet";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.White;
        }

        public override void ScreensaverLogic()
        {
            int ConsoleMiddleWidth = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowWidth / 2d);
            int ConsoleMiddleHeight = (int)Math.Round(ConsoleBase.ConsoleWrapper.WindowHeight / 2d);
            var FigletFontUsed = FigletTools.GetFigletFont(FigletSettings.FigletFont);
            ConsoleBase.ConsoleWrapper.CursorVisible = false;
            ConsoleBase.ConsoleWrapper.Clear();

            // Set colors
            var ColorStorage = new Color(255, 255, 255);
            if (FigletSettings.FigletTrueColor)
            {
                int RedColorNum = RandomDriver.Next(FigletSettings.FigletMinimumRedColorLevel, FigletSettings.FigletMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Next(FigletSettings.FigletMinimumGreenColorLevel, FigletSettings.FigletMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Next(FigletSettings.FigletMinimumBlueColorLevel, FigletSettings.FigletMaximumBlueColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else if (FigletSettings.Figlet255Colors)
            {
                int ColorNum = RandomDriver.Next(FigletSettings.FigletMinimumColorLevel, FigletSettings.FigletMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                ColorStorage = new Color(ColorNum);
            }
            else
            {
                ConsoleBase.ConsoleWrapper.BackgroundColor = (ConsoleColor)RandomDriver.Next(FigletSettings.FigletMinimumColorLevel, FigletSettings.FigletMaximumColorLevel);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ConsoleBase.ConsoleWrapper.BackgroundColor);
            }

            // Prepare the figlet font for writing
            string FigletWrite = FigletSettings.FigletText.ReplaceAll(new string[] { Convert.ToChar(13).ToString(), Convert.ToChar(10).ToString() }, " - ");
            FigletWrite = FigletFontUsed.Render(FigletWrite);
            var FigletWriteLines = FigletWrite.SplitNewLines().SkipWhile(x => string.IsNullOrEmpty(x)).ToArray();
            int FigletHeight = (int)Math.Round(ConsoleMiddleHeight - FigletWriteLines.Length / 2d);
            int FigletWidth = (int)Math.Round(ConsoleMiddleWidth - FigletWriteLines[0].Length / 2d);

            // Actually write it
            if (CurrentWindowHeight != ConsoleBase.ConsoleWrapper.WindowHeight | CurrentWindowWidth != ConsoleBase.ConsoleWrapper.WindowWidth)
                ResizeSyncing = true;
            if (!ResizeSyncing)
            {
                if (FigletSettings.Figlet255Colors | FigletSettings.FigletTrueColor)
                {
                    TextWriterWhereColor.WriteWhere(FigletWrite, FigletWidth, FigletHeight, true, ColorStorage);
                }
                else
                {
                    WriterPlainManager.currentPlain.WriteWherePlain(FigletWrite, FigletWidth, FigletHeight, true);
                }
            }
            ThreadManager.SleepNoBlock(FigletSettings.FigletDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = ConsoleBase.ConsoleWrapper.WindowWidth;
            CurrentWindowHeight = ConsoleBase.ConsoleWrapper.WindowHeight;
        }

    }
}
