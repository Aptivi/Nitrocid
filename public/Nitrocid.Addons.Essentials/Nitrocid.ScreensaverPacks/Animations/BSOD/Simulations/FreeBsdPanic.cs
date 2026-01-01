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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class FreeBsdPanic : BaseBSOD
    {
        public override void Simulate()
        {
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate a FreeBSD kernel panic
            TextWriterRaw.WritePlain(
                 "Fatal trap 9: general protection fault while in kernel mode\n" +
                 "cpuid = 4; apic id = 14\n" +
                $"instruction pointer     = 0x{RandomDriver.Random():x2}:0x{RandomDriver.Random():x16}\n" +
                $"stack pointer           = 0x{RandomDriver.Random():x2}:0x{RandomDriver.Random():x16}\n" +
                $"frame pointer           = 0x{RandomDriver.Random():x2}:0x{RandomDriver.Random():x16}\n" +
                 "code segment            = base 0x0, limit 0xfffff, type 0x1b\n" +
                 "                        = DPL 0, pres 1, long 1, def32 0, gran 1\n" +
                 "processor eflags        = interrupt enabled, resule, IOPL = 0\n" +
                 "current process         = 599 (ppp)\n" +
                 "trap number             = 9\n" +
                 "panic: general protection fault\n" +
                 "cpuid = 4\n" +
                 "Uptime: 2m15s\n" +
                 "Automatic reboot in 15 seconds - press a key on the console to abort\n" +
                 "-> Press a key on the console to reboot,\n" +
                 "-> or switch off the system now."
            , true);
        }
    }
}
