//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Terminaux.Writer.ConsoleWriters;
using System.Threading;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class YabootLinux : BaseBSOD
    {
        public override void Simulate()
        {
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate a Yaboot failure
            TextWriterRaw.WritePlain("Welcome to yaboot version 1.3.17", true);
            TextWriterRaw.WritePlain("Enter \"help\" to get some basic usage information", true);
            TextWriterRaw.WritePlain("boot: ", false);
            ConsoleWrapper.CursorVisible = true;
            Thread.Sleep(3000);
            TextWriterSlowColor.WriteSlowlyPlain("Linux", true, 140);
            ConsoleWrapper.CursorVisible = false;
            TextWriterRaw.WritePlain("Please wait, loading kernel...", true);
            Thread.Sleep(60);
            TextWriterRaw.WritePlain("/pci@f2000000/mac-io@17/ata-4@1f000/disk@0:4,/boot/kernel/genkernel-ppc-3.12.21-gentoo-r1: Unknown or corrupt filesystem", true);
            TextWriterRaw.WritePlain("boot: ", false);
            ConsoleWrapper.CursorVisible = true;
        }
    }
}
