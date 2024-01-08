//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class OS2Warp : BaseBSOD
    {
        public override void Simulate()
        {
            ColorTools.LoadBack(new Color(ConsoleColors.Black));
            ColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate an OS/2 kernel failure
            TextWriterColor.WritePlain("Exception in module: OS2KRNL", true);
            TextWriterColor.WritePlain("TRAP 000d       ERRCD=00c0  ERACC=****  ERLIM=********", true);
            TextWriterColor.WritePlain("EAX=00000008  EBX=0000fdf0  ECX=00001616  EDX=00000000", true);
            TextWriterColor.WritePlain($"ESI={RandomDriver.Random():x8}  EDI={RandomDriver.Random():x8}  EBP={RandomDriver.Random():x8}  FLG={RandomDriver.Random():x8}", true);
            TextWriterColor.WritePlain($"CS:EIP=0140:{RandomDriver.Random():x8}  CSACC=009b  CSLIM={RandomDriver.Random():x8}", true);
            TextWriterColor.WritePlain($"SS:ESP=0030:{RandomDriver.Random():x8}  SSACC=009b  SSLIM={RandomDriver.Random():x8}", true);
            TextWriterColor.WritePlain($"DS=0a00  DSACC=00f3  DSLIM={RandomDriver.Random():x8}  CR0={RandomDriver.Random():x8}", true);
            TextWriterColor.WritePlain($"ES=0a00  ESACC=00f3  ESLIM={RandomDriver.Random():x8}  CR2={RandomDriver.Random():x8}", true);
            TextWriterColor.WritePlain($"FS=0000  FSACC=****  FSLIM=********", true);
            TextWriterColor.WritePlain($"GS=0000  GSACC=****  GSLIM=********\n", true);
            TextWriterColor.WritePlain("The system detected an internal processing error at", true);
            TextWriterColor.WritePlain($"location ##0168:{RandomDriver.Random():x8} - 000e:cb1c", true);
            TextWriterColor.WritePlain("60000, 9084\n", true);
            TextWriterColor.WritePlain($"{RandomDriver.Random():x8}", true);
            TextWriterColor.WritePlain("Internal revision 14.086_W4\n", true);
            TextWriterColor.WritePlain("The system is stopped.  Record all of the above information and", true);
            TextWriterColor.WritePlain("contact your service representative.", true);
        }
    }
}
