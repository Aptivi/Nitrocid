//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using System.Collections.Generic;
using System.Linq;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Nitrocid.Kernel.Configuration;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Lighter
    /// </summary>
    public class LighterDisplay : BaseScreensaver, IScreensaver
    {

        private readonly List<Tuple<int, int>> CoveredPositions = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Lighter";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            CoveredPositions.Clear();
            ConsoleColoring.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.LighterBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Select a position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", vars: [Left, Top]);
            ConsoleWrapper.SetCursorPosition(Left, Top);
            if (!CoveredPositions.Any(t => t.Item1 == Left & t.Item2 == Top))
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Covering position...");
                CoveredPositions.Add(new Tuple<int, int>(Left, Top));
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Position covered. Covered positions: {0}", vars: [CoveredPositions.Count]);
            }

            // Select a color and write the space
            if (ScreensaverPackInit.SaversConfig.LighterTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LighterMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.LighterMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LighterMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.LighterMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LighterMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.LighterMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleColoring.SetConsoleColorDry(ColorStorage, true);
                    ConsoleWrapper.Write(" ");
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.LighterMinimumColorLevel, ScreensaverPackInit.SaversConfig.LighterMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleColoring.SetConsoleColorDry(new Color(ColorNum), true);
                    ConsoleWrapper.Write(" ");
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }

            // Simulate a trail effect
            if (CoveredPositions.Count == ScreensaverPackInit.SaversConfig.LighterMaxPositions)
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Covered positions exceeded max positions of {0}", vars: [ScreensaverPackInit.SaversConfig.LighterMaxPositions]);
                int WipeLeft = CoveredPositions[0].Item1;
                int WipeTop = CoveredPositions[0].Item2;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Wiping in {0}, {1}...", vars: [WipeLeft, WipeTop]);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(WipeLeft, WipeTop);
                    ConsoleColoring.SetConsoleColorDry(new Color(ScreensaverPackInit.SaversConfig.LighterBackgroundColor), true);
                    ConsoleWrapper.Write(" ");
                    CoveredPositions.RemoveAt(0);
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing covered positions...");
                    CoveredPositions.Clear();
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.LighterDelay);
        }

    }
}
