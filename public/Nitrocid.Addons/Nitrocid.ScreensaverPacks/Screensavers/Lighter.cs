
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
    /// Settings for Lighter
    /// </summary>
    public static class LighterSettings
    {

        /// <summary>
        /// [Lighter] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool LighterTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterTrueColor = value;
            }
        }
        /// <summary>
        /// [Lighter] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LighterDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterDelay;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterDelay = value;
            }
        }
        /// <summary>
        /// [Lighter] How many positions to write before starting to blacken them?
        /// </summary>
        public static int LighterMaxPositions
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterMaxPositions;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterMaxPositions = value;
            }
        }
        /// <summary>
        /// [Lighter] Screensaver background color
        /// </summary>
        public static string LighterBackgroundColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterBackgroundColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterBackgroundColor = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum red color level (true color)
        /// </summary>
        public static int LighterMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterMinimumRedColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum green color level (true color)
        /// </summary>
        public static int LighterMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterMinimumGreenColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum blue color level (true color)
        /// </summary>
        public static int LighterMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterMinimumBlueColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int LighterMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterMinimumColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum red color level (true color)
        /// </summary>
        public static int LighterMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterMaximumRedColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum green color level (true color)
        /// </summary>
        public static int LighterMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterMaximumGreenColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum blue color level (true color)
        /// </summary>
        public static int LighterMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterMaximumBlueColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [Lighter] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int LighterMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.LighterMaximumColorLevel;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.LighterMaximumColorLevel = value;
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
            KernelColorTools.LoadBack(new Color(LighterSettings.LighterBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top);
            ConsoleWrapper.SetCursorPosition(Left, Top);
            if (!CoveredPositions.Any(t => t.Item1 == Left & t.Item2 == Top))
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Covering position...");
                CoveredPositions.Add(new Tuple<int, int>(Left, Top));
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Position covered. Covered positions: {0}", CoveredPositions.Count);
            }

            // Select a color and write the space
            if (LighterSettings.LighterTrueColor)
            {
                int RedColorNum = RandomDriver.Random(LighterSettings.LighterMinimumRedColorLevel, LighterSettings.LighterMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(LighterSettings.LighterMinimumGreenColorLevel, LighterSettings.LighterMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(LighterSettings.LighterMinimumBlueColorLevel, LighterSettings.LighterMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    KernelColorTools.SetConsoleColor(ColorStorage, true);
                    ConsoleWrapper.Write(" ");
                }
                else
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(LighterSettings.LighterMinimumColorLevel, LighterSettings.LighterMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    KernelColorTools.SetConsoleColor(new Color(ColorNum), true);
                    ConsoleWrapper.Write(" ");
                }
                else
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }

            // Simulate a trail effect
            if (CoveredPositions.Count == LighterSettings.LighterMaxPositions)
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Covered positions exceeded max positions of {0}", LighterSettings.LighterMaxPositions);
                int WipeLeft = CoveredPositions[0].Item1;
                int WipeTop = CoveredPositions[0].Item2;
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Wiping in {0}, {1}...", WipeLeft, WipeTop);
                if (!ConsoleResizeListener.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(WipeLeft, WipeTop);
                    KernelColorTools.SetConsoleColor(new Color(LighterSettings.LighterBackgroundColor), true);
                    ConsoleWrapper.Write(" ");
                    CoveredPositions.RemoveAt(0);
                }
                else
                {
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(LighterSettings.LighterDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
