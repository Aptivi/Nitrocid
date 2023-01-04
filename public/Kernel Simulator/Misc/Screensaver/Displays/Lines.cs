
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Lines
    /// </summary>
    public static class LinesSettings
    {

        private static bool _TrueColor = true;
        private static int _Delay = 500;
        private static string _LineChar = "-";
        private static string _BackgroundColor = new Color((int)ConsoleColor.Black).PlainSequence;
        private static int _MinimumRedColorLevel = 0;
        private static int _MinimumGreenColorLevel = 0;
        private static int _MinimumBlueColorLevel = 0;
        private static int _MinimumColorLevel = 0;
        private static int _MaximumRedColorLevel = 255;
        private static int _MaximumGreenColorLevel = 255;
        private static int _MaximumBlueColorLevel = 255;
        private static int _MaximumColorLevel = 255;

        /// <summary>
        /// [Lines] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool LinesTrueColor
        {
            get
            {
                return _TrueColor;
            }
            set
            {
                _TrueColor = value;
            }
        }
        /// <summary>
        /// [Lines] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LinesDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                _Delay = value;
            }
        }
        /// <summary>
        /// [Lines] Line character
        /// </summary>
        public static string LinesLineChar
        {
            get
            {
                return _LineChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "-";
                _LineChar = value;
            }
        }
        /// <summary>
        /// [Lines] Screensaver background color
        /// </summary>
        public static string LinesBackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = new Color(value).PlainSequence;
            }
        }
        /// <summary>
        /// [Lines] The minimum red color level (true color)
        /// </summary>
        public static int LinesMinimumRedColorLevel
        {
            get
            {
                return _MinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum green color level (true color)
        /// </summary>
        public static int LinesMinimumGreenColorLevel
        {
            get
            {
                return _MinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum blue color level (true color)
        /// </summary>
        public static int LinesMinimumBlueColorLevel
        {
            get
            {
                return _MinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _MinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LinesMinimumColorLevel
        {
            get
            {
                return _MinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                _MinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum red color level (true color)
        /// </summary>
        public static int LinesMaximumRedColorLevel
        {
            get
            {
                return _MaximumRedColorLevel;
            }
            set
            {
                if (value <= _MinimumRedColorLevel)
                    value = _MinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum green color level (true color)
        /// </summary>
        public static int LinesMaximumGreenColorLevel
        {
            get
            {
                return _MaximumGreenColorLevel;
            }
            set
            {
                if (value <= _MinimumGreenColorLevel)
                    value = _MinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum blue color level (true color)
        /// </summary>
        public static int LinesMaximumBlueColorLevel
        {
            get
            {
                return _MaximumBlueColorLevel;
            }
            set
            {
                if (value <= _MinimumBlueColorLevel)
                    value = _MinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                _MaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lines] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LinesMaximumColorLevel
        {
            get
            {
                return _MaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= _MinimumColorLevel)
                    value = _MinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                _MaximumColorLevel = value;
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
                ColorTools.LoadBack(new Color(LinesSettings.LinesBackgroundColor), true);
                int RedColorNum = RandomDriver.Random(LinesSettings.LinesMinimumRedColorLevel, LinesSettings.LinesMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(LinesSettings.LinesMinimumGreenColorLevel, LinesSettings.LinesMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(LinesSettings.LinesMinimumBlueColorLevel, LinesSettings.LinesMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                ColorTools.SetConsoleColor(ColorStorage);
            }
            else
            {
                ColorTools.LoadBack(new Color(LinesSettings.LinesBackgroundColor), true);
                int color = RandomDriver.Random(LinesSettings.LinesMinimumColorLevel, LinesSettings.LinesMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", color);
                ColorTools.SetConsoleColor(new Color(color));
            }

            // Draw a line
            string Line = "";
            int Top = new Random().Next(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Got top position ({0})", Top);
            for (int i = 1; i <= ConsoleWrapper.WindowWidth; i++)
            {
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Forming line using {0} or the default \"-\"...", LinesSettings.LinesLineChar);
                Line += !string.IsNullOrWhiteSpace(LinesSettings.LinesLineChar) ? LinesSettings.LinesLineChar : "-";
                DebugWriter.WriteDebugConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Line: {0}", Line);
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
