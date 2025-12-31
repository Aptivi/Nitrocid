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

using System.Collections.Generic;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD
{
    /// <summary>
    /// BSOD animation module
    /// </summary>
    public static class BSOD
    {

        private static readonly List<BaseBSOD> BSODList =
        [
            new WindowsXP(),
            new Windows2K(),
            new Windows98(),
            new Linux(),
            new VGA(),
            new BiosMbr(),
            new HaikuBootloader(),
            new Linux2(),
            new SamsungS7Bootloader(),
            new OS2Warp(),
            new OS2WarpPreBoot(),
            new YabootLinux(),
            new GrubError(),
            new BootMgrFatalError(),
            new BootMgrError(),
            new MacPpcPanic(),
            new BeOs5Error(),
            new FreeBsdBoot(),
            new FreeBsdPanic(),
            new WindowsNt(),
        ];

        /// <summary>
        /// Simulates the BSOD animation
        /// </summary>
        public static void Simulate(BSODSettings? Settings)
        {
            Settings ??= new();
            int selectedBsodIdx = RandomDriver.RandomIdx(BSODList.Count);
            var selectedBsod = BSODList[selectedBsodIdx];
            ConsoleWrapper.CursorVisible = false;
            selectedBsod.Simulate();

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(Settings.BSODDelay);
        }

    }
}
