
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

using KS.Misc.Threading;
using KS.ConsoleBase;
using System.Collections.Generic;
using KS.Misc.Animations.BSOD.Simulations;
using KS.Drivers.RNG;

namespace KS.Misc.Animations.BSOD
{
    /// <summary>
    /// BSOD animation module
    /// </summary>
    public static class BSOD
    {

        private static readonly List<BaseBSOD> BSODList = new()
        {
            new WindowsXP(),
            new Windows2K(),
            new Windows98(),
            new Linux(),
            new VGA(),
        };

        /// <summary>
        /// Simulates the BSOD animation
        /// </summary>
        public static void Simulate(BSODSettings Settings)
        {
            int selectedBsodIdx = RandomDriver.RandomIdx(BSODList.Count);
            var selectedBsod = BSODList[selectedBsodIdx];
            selectedBsod.Simulate();

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(Settings.BSODDelay, Screensaver.ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
