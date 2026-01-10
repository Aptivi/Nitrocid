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
using Nitrocid.Base.Drivers.RNG;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class FreeBsdBoot : BaseBSOD
    {
        public override void Simulate()
        {
            ConsoleColoring.LoadBackDry(new Color(ConsoleColors.Black));
            ConsoleColoring.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate a FreeBSD boot failure
            TextWriterRaw.WritePlain(
                 "FreeBSD/x86 boot\n" +
                 "\n" +
                $"int={RandomDriver.Random():x8}  err={RandomDriver.Random():x8}  efl={RandomDriver.Random():x8}  eip={RandomDriver.Random():x8}\n" +
                $"eax={RandomDriver.Random():x8}  ebx={RandomDriver.Random():x8}  ecx={RandomDriver.Random():x8}  edx={RandomDriver.Random():x8}\n" +
                $"esi={RandomDriver.Random():x8}  edi={RandomDriver.Random():x8}  ebp={RandomDriver.Random():x8}  esp={RandomDriver.Random():x8}\n" +
                $"cs={RandomDriver.Random(0, 0xffff):x4}  ds={RandomDriver.Random(0, 0xffff):x4}  es={RandomDriver.Random(0, 0xffff):x4}    fs={RandomDriver.Random(0, 0xffff):x4}  gs={RandomDriver.Random(0, 0xffff):x4}  ss={RandomDriver.Random(0, 0xffff):x4}\n" +
                $"cs:eip=ff ff ff ff ff ff ff ff-ff ff ff ff ff ff ff ff\n" +
                $"       ff ff ff ff ff ff ff ff-ff ff ff ff ff ff ff ff\n" +
                $"ss:esp={RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2}-{RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2}\n" +
                $"       {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2}-{RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2} {RandomDriver.Random(0, 0xff):x2}\n" +
                $"BTX halted"
            , true);
        }
    }
}
