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
    internal class OS2Warp : BaseBSOD
    {
        public override void Simulate()
        {
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate an OS/2 kernel failure
            TextWriterRaw.WritePlain("Exception in module: OS2KRNL", true);
            TextWriterRaw.WritePlain("TRAP 000d       ERRCD=00c0  ERACC=****  ERLIM=********", true);
            TextWriterRaw.WritePlain("EAX=00000008  EBX=0000fdf0  ECX=00001616  EDX=00000000", true);
            TextWriterRaw.WritePlain($"ESI={RandomDriver.Random():x8}  EDI={RandomDriver.Random():x8}  EBP={RandomDriver.Random():x8}  FLG={RandomDriver.Random():x8}", true);
            TextWriterRaw.WritePlain($"CS:EIP=0140:{RandomDriver.Random():x8}  CSACC=009b  CSLIM={RandomDriver.Random():x8}", true);
            TextWriterRaw.WritePlain($"SS:ESP=0030:{RandomDriver.Random():x8}  SSACC=009b  SSLIM={RandomDriver.Random():x8}", true);
            TextWriterRaw.WritePlain($"DS=0a00  DSACC=00f3  DSLIM={RandomDriver.Random():x8}  CR0={RandomDriver.Random():x8}", true);
            TextWriterRaw.WritePlain($"ES=0a00  ESACC=00f3  ESLIM={RandomDriver.Random():x8}  CR2={RandomDriver.Random():x8}", true);
            TextWriterRaw.WritePlain($"FS=0000  FSACC=****  FSLIM=********", true);
            TextWriterRaw.WritePlain($"GS=0000  GSACC=****  GSLIM=********\n", true);
            TextWriterRaw.WritePlain("The system detected an internal processing error at", true);
            TextWriterRaw.WritePlain($"location ##0168:{RandomDriver.Random():x8} - 000e:cb1c", true);
            TextWriterRaw.WritePlain("60000, 9084\n", true);
            TextWriterRaw.WritePlain($"{RandomDriver.Random():x8}", true);
            TextWriterRaw.WritePlain("Internal revision 14.086_W4\n", true);
            TextWriterRaw.WritePlain("The system is stopped.  Record all of the above information and", true);
            TextWriterRaw.WritePlain("contact your service representative.", true);
        }
    }
}
