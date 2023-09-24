
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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class GrubError : BaseBSOD
    {
        public override void Simulate()
        {
            KernelColorTools.LoadBack(new Color(ConsoleColors.Black));
            KernelColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate a Haiku bootloader failure
            TextWriterColor.WritePlain("error: unknown filesystem.", true);
            TextWriterColor.WritePlain("Entering rescue mode...", true);
            TextWriterColor.WritePlain("grub rescue> ", false);
            ConsoleWrapper.CursorVisible = true;
        }
    }
}
