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
using Nitrocid.Base.Drivers.RNG;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class MacPpcPanic : BaseBSOD
    {
        public override void Simulate()
        {
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate a Mac OS X 10.0 PowerPC kernel panic on boot
            TextWriterRaw.WritePlain(
                 "panic (cpu 0): Couldn't register lo modules\n\n" +
                $"backtrace: 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8}\n\n" +
                 "No debugger configured - dumping debug information\n\n" +
                 "version string : Darwin Kernel Version 1.2:\n" +
                 "Fri Nov  3 13:34:08 PST 2008; root:xnu/xnu-109.5.obj~3/RELEASE_PPC\n\n\n" +
                 "DBAT0: 00000000 00000000\n" +
                 "DBAT1: 00000000 00000000\n" +
                $"DBAT2: {RandomDriver.Random():X8} {RandomDriver.Random():X8}\n" +
                $"DBAT3: {RandomDriver.Random():X8} {RandomDriver.Random():X8}\n" +
                $"MSR={RandomDriver.Random():X8}\n" +
                $"backtrace: 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8} 0x{RandomDriver.Random():x8}\n" +
                 "panic: We are hanging here..."
            , true);
        }
    }
}
