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
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class Linux2 : BaseBSOD
    {
        public override void Simulate()
        {
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate a null pointer dereference
            TextWriterRaw.WritePlain(
                 "IP Protocols: IGMP, ICMP, UDP, TCP\n" +
                 "VFS: Diskquotas version dquot_5.6.0 initialized\n" +
                 "Checking 386/387 coupling... Ok, fpu using exception 16 reporting.\n" +
                 "Checking 'hlt' instruction... Ok.\n" +
                 "Linux version 2.0.33 (root@gondor) (gcc version 2.7.2.3) #2 Tue Jun 30 18:40:40 EST 1998\n" +
                 "starting kswapd v 1.4.2.2\n" +
                 "Real Time Clock Driver v1.07\n" +
                 "tpqic02: Runtime config, $Revision: 0.4.1.5 $, $Date: 1994/10/29 02:46:13 $\n" +
                 "tpqic02: DMA buffers: 20 blocks, at address 0x26ce00 (0x26cc58)\n" +
                 "Ramdisk driver initialized : 16 ramdisks of 4096K size\n" +
                 "loop: registered device at major 7\n" +
                 "hdc: WDC WD400BB-00DEA0, 8062MB w/2048kB Cache, CHS=16383/16/63\n" +
                 "ide1 at 0x170-0x177,0x376 on irq 15\n" +
                 "FDC 0 is a post-1991 82077\n" +
                 "md driver 0.35 MAX_MD_DEV=4, MAX_REAL=8\n" +
                 "Failed initialization of WD-7000 SCSI card!\n" +
                 "PPA: unable to initialise controller at 0x378, error 2\n" +
                 "scsi : 0 hosts.\n" +
                 "scsi : detected total.\n" +
                 "Partition check:\n" +
                 " hdc: hdc1 hdc2 < hdc5 hdc6 > hdc3 < hdc7 > hdc4\n" +
                 "VFS: Cannot open root device 03:01\n" +
                 "Kernel panic: VFS: Unable to mount root fs on 03:01\n"
            , false);
        }
    }
}
