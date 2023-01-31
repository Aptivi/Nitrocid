
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
using System.Collections.Generic;
using System.Linq;
using ColorSeq;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Debugging;
using KS.Misc.Threading;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Lighter
    /// </summary>
    public static class LighterSettings
    {

        private static bool _TrueColor = true;
        private static int _Delay = 100;
        private static int _MaxPositions = 10;
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
        /// [Lighter] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool LighterTrueColor
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
        /// [Lighter] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LighterDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                _Delay = value;
            }
        }
        /// <summary>
        /// [Lighter] How many positions to write before starting to blacken them?
        /// </summary>
        public static int LighterMaxPositions
        {
            get
            {
                return _MaxPositions;
            }
            set
            {
                _MaxPositions = value;
            }
        }
        /// <summary>
        /// [Lighter] Screensaver background color
        /// </summary>
        public static string LighterBackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum red color level (true color)
        /// </summary>
        public static int LighterMinimumRedColorLevel
        {
            get
            {
                return _MinimumRedColorLevel;
            }
            set
            {
                _MinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum green color level (true color)
        /// </summary>
        public static int LighterMinimumGreenColorLevel
        {
            get
            {
                return _MinimumGreenColorLevel;
            }
            set
            {
                _MinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum blue color level (true color)
        /// </summary>
        public static int LighterMinimumBlueColorLevel
        {
            get
            {
                return _MinimumBlueColorLevel;
            }
            set
            {
                _MinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LighterMinimumColorLevel
        {
            get
            {
                return _MinimumColorLevel;
            }
            set
            {
                _MinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum red color level (true color)
        /// </summary>
        public static int LighterMaximumRedColorLevel
        {
            get
            {
                return _MaximumRedColorLevel;
            }
            set
            {
                _MaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum green color level (true color)
        /// </summary>
        public static int LighterMaximumGreenColorLevel
        {
            get
            {
                return _MaximumGreenColorLevel;
            }
            set
            {
                _MaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum blue color level (true color)
        /// </summary>
        public static int LighterMaximumBlueColorLevel
        {
            get
            {
                return _MaximumBlueColorLevel;
            }
            set
            {
                _MaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LighterMaximumColorLevel
        {
            get
            {
                return _MaximumColorLevel;
            }
            set
            {
                _MaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for Lighter
    /// </summary>
    public class LighterDisplay : BaseScreensaver, IScreensaver
    {

        private readonly List<Tuple<int, int>> CoveredPositions = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Lighter";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBack(new Color(LighterSettings.LighterBackgroundColor), true);
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleWrapper.SetCursorPosition(Left, Top);
            if (!CoveredPositions.Any(t => t.Item1 == Left & t.Item2 == Top))
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Covering position...");
                CoveredPositions.Add(new Tuple<int, int>(Left, Top));
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Position covered. Covered positions: {0}", CoveredPositions.Count);
            }

            // Select a color and write the space
            if (LighterSettings.LighterTrueColor)
            {
                int RedColorNum = RandomDriver.Random(LighterSettings.LighterMinimumRedColorLevel, LighterSettings.LighterMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(LighterSettings.LighterMinimumGreenColorLevel, LighterSettings.LighterMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(LighterSettings.LighterMinimumBlueColorLevel, LighterSettings.LighterMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ColorTools.SetConsoleColor(ColorStorage, true, true);
                    ConsoleWrapper.Write(" ");
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(LighterSettings.LighterMinimumColorLevel, LighterSettings.LighterMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ColorTools.SetConsoleColor(new Color(ColorNum), true, true);
                    ConsoleWrapper.Write(" ");
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }

            // Simulate a trail effect
            if (CoveredPositions.Count == LighterSettings.LighterMaxPositions)
            {
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Covered positions exceeded max positions of {0}", LighterSettings.LighterMaxPositions);
                int WipeLeft = Convert.ToInt32(CoveredPositions[0].ToString().Substring(0, CoveredPositions[0].ToString().IndexOf(";")));
                int WipeTop = Convert.ToInt32(CoveredPositions[0].ToString().Substring(CoveredPositions[0].ToString().IndexOf(";") + 1));
                DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.I, "Wiping in {0}, {1}...", WipeLeft, WipeTop);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(WipeLeft, WipeTop);
                    ColorTools.SetConsoleColor(new Color(LighterSettings.LighterBackgroundColor), true, true);
                    ConsoleWrapper.Write(" ");
                    CoveredPositions.RemoveAt(0);
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Screensaver.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(LighterSettings.LighterDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
