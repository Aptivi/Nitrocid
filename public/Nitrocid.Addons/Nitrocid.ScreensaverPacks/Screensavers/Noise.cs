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
using System.Collections;
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Noise
    /// </summary>
    public static class NoiseSettings
    {

        /// <summary>
        /// [Noise] How many milliseconds to wait before making the new screen?
        /// </summary>
        public static int NoiseNewScreenDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.NoiseNewScreenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 5000;
                ScreensaverPackInit.SaversConfig.NoiseNewScreenDelay = value;
            }
        }
        /// <summary>
        /// [Noise] The noise density in percent
        /// </summary>
        public static int NoiseDensity
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.NoiseDensity;
            }
            set
            {
                if (value < 0)
                    value = 40;
                if (value > 100)
                    value = 40;
                ScreensaverPackInit.SaversConfig.NoiseDensity = value;
            }
        }

    }

    /// <summary>
    /// Display code for Noise
    /// </summary>
    public class NoiseDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Noise";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            double NoiseDense = (NoiseSettings.NoiseDensity > 100 ? 100 : NoiseSettings.NoiseDensity) / 100d;

            ConsoleWrapper.CursorVisible = false;
            KernelColorTools.LoadBack(new Color(ConsoleColors.DarkGray));
            ConsoleWrapper.Clear();
            KernelColorTools.SetConsoleColor(ConsoleColors.Black, true);

            // Select random positions to generate noise
            int AmountOfBlocks = ConsoleWrapper.WindowWidth * ConsoleWrapper.WindowHeight;
            int BlocksToCover = (int)Math.Round(AmountOfBlocks * NoiseDense);
            var CoveredBlocks = new ArrayList();
            while (CoveredBlocks.Count < BlocksToCover)
            {
                if (!ConsoleResizeListener.WasResized(false))
                {
                    int CoverX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                    int CoverY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                    ConsoleWrapper.SetCursorPosition(CoverX, CoverY);
                    ConsoleWrapper.Write(" ");
                    if (!CoveredBlocks.Contains(CoverX.ToString() + ", " + CoverY.ToString()))
                        CoveredBlocks.Add(CoverX.ToString() + ", " + CoverY.ToString());
                }
                else
                {
                    // We're resizing.
                    break;
                }
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(NoiseSettings.NoiseNewScreenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
