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
using System.Collections;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Noise
    /// </summary>
    public class NoiseDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Noise";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            double NoiseDense = (ScreensaverPackInit.SaversConfig.NoiseDensity > 100 ? 100 : ScreensaverPackInit.SaversConfig.NoiseDensity) / 100d;
            ColorTools.LoadBackDry(new Color(ConsoleColors.Grey));
            ConsoleWrapper.Clear();
            ColorTools.SetConsoleColorDry(ConsoleColors.Black, true);

            // Select random positions to generate noise
            int AmountOfBlocks = ConsoleWrapper.WindowWidth * ConsoleWrapper.WindowHeight;
            int BlocksToCover = (int)Math.Round(AmountOfBlocks * NoiseDense);
            var CoveredBlocks = new ArrayList();
            while (CoveredBlocks.Count < BlocksToCover)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                int CoverX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                int CoverY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                ConsoleWrapper.SetCursorPosition(CoverX, CoverY);
                ConsoleWrapper.Write(" ");
                if (!CoveredBlocks.Contains(CoverX.ToString() + ", " + CoverY.ToString()))
                    CoveredBlocks.Add(CoverX.ToString() + ", " + CoverY.ToString());
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.NoiseNewScreenDelay);
        }

    }
}
