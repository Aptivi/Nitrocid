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
using Nitrocid.Drivers.RNG;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class BeOs5Error : BaseBSOD
    {
        public override void Simulate()
        {
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate a BeOS 5.0 load failure
            TextWriterRaw.WritePlain(
                 "PANIC: no shell!\n" +
                 "kernel debugger: Welcome to Kernel Debugging Land...\n" +
                $" eax {RandomDriver.Random():x8}  ebp {RandomDriver.Random():x8}  cs {RandomDriver.Random():x4} | area {RandomDriver.Random():x8}  (kernel_intel_text)\n" +
                $" ebx {RandomDriver.Random():x8}  esp {RandomDriver.Random():x8}  ss {RandomDriver.Random():x4} | addr {RandomDriver.Random():x8}  size {RandomDriver.Random():x8}\n" +
                $" ecx {RandomDriver.Random():x8}  edi {RandomDriver.Random():x8}  ds {RandomDriver.Random():x4} |\n" +
                $" edx {RandomDriver.Random():x8}  esi {RandomDriver.Random():x8}  es {RandomDriver.Random():x4} | Thread: sysinit2\n" +
                $" eip {RandomDriver.Random():x8} flag {RandomDriver.Random():x8}  fs {RandomDriver.Random():x4} | Team:  kernel_team\n" +
                $"trap {RandomDriver.Random():x8}  err {RandomDriver.Random():x8}  gs {RandomDriver.Random():x4} | Stack Trace follows:\n" +
                $"\n" +
                $"{RandomDriver.Random():x8}  {RandomDriver.Random():x8}\n" +
                $"{RandomDriver.Random():x8}  {RandomDriver.Random():x8}\n" +
                 "kdebug>"
            , false);
        }
    }
}
