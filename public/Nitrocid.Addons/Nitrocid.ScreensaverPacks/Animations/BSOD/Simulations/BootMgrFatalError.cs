//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using System;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class BootMgrFatalError : BaseBSOD
    {
        internal enum Variant
        {
            Missing,
            Compressed,
            Corrupted,
        }

        public override void Simulate()
        {
            KernelColorTools.LoadBack(new Color(ConsoleColors.Black));
            KernelColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Select a variant
            int excVariantInt = RandomDriver.RandomIdx(Enum.GetNames<Variant>().Length);
            var excVariant = Enum.Parse<Variant>($"{excVariantInt}");
            switch (excVariant)
            {
                case Variant.Missing:
                    TextWriterColor.WritePlain("BOOTMGR is missing.", true);
                    TextWriterColor.WritePlain("Press Ctrl+Alt+Del to restart...", true);
                    break;
                case Variant.Compressed:
                    TextWriterColor.WritePlain("BOOTMGR is compressed.", true);
                    TextWriterColor.WritePlain("Press Ctrl+Alt+Del to restart...", true);
                    break;
                case Variant.Corrupted:
                    TextWriterColor.WritePlain("BOOTMGR image is corrupt.  The system cannot boot.", true);
                    break;
            }
        }
    }
}
