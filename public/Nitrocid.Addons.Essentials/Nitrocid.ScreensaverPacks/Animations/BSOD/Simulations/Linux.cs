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
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class Linux : BaseBSOD
    {
        public override void Simulate()
        {
            ConsoleColoring.LoadBackDry(new Color(ConsoleColors.Black));
            ConsoleColoring.SetConsoleColor(new Color(ConsoleColors.White));

            // Simulate a null pointer dereference
            TextWriterRaw.WritePlain(
                $"Unable to handle kernel NULL pointer dereference at virtual address {RandomDriver.Random():X8}\n" +
                 " printing eip:\n" +
                 "*pde = 00000000\n" +
                 "Oops: 0000 [#1]\n" +
                 "PREEMPT\n" +
                 "Modules linked in: xfs dm_mod sd_mod data_sil libata scsi_mod uhci_hcd usbcore\n" +
                 "CPU:    0\n" +
                $"EIP:    0060:[<c02568e6>]    Not tainted VLI\n" +
                $"EFLAGS: {RandomDriver.Random():X8}   (2.6.9)\n" +
                 "EIP is at ide_dma_timeout_retry+0x56/0xe0\n" +
                $"eax: {RandomDriver.Random():X8}   ebx: 00000000   ecx: {RandomDriver.Random():X8}    edx: {RandomDriver.Random():X8}\n" +
                $"esi: {RandomDriver.Random():X8}   edi: 00000000   ebp: {RandomDriver.Random():X8}    esp: {RandomDriver.Random():X8}\n" +
                $"ds: {RandomDriver.Random():X4}   es: {RandomDriver.Random():X4}   ss: {RandomDriver.Random():X4}\n" +
                $"Process swapper (pid: 0, threadinfo={RandomDriver.Random():X8} task={RandomDriver.Random():X8})\n" +
                $"Stack: {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8}\n" +
                $"       {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8}\n" +
                $"       {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8}\n" +
                 "Call Trace:\n" +
                 " [<c0256ad9>] ide_timer_expiry+0x169/0x230\n" +
                 " [<c0256970>] ide_timer_expiry+0x0/0x230\n" +
                 " [<c01256ec>] run_timer_softirq+0xcc/0x1b0\n" +
                 " [<c0114a17>] smp_local_timer_interrupt+0x17/0x70\n" +
                 " [<c0121585>] __do_softirq+0x85/0x90\n" +
                 " [<c01215b7>] do_softirq+0x27/0x140\n" +
                 " [<c0108066>] do_IRQ+0x106/0x140\n" +
                 " [<c0103030>] default_idle+0x0/0x40\n" +
                 " [<c0105c38>] common_interrupt+0x18/0x20\n" +
                 " [<c0103030>] default_idle+0x0/0x40\n" +
                 " [<c020e2aa>] acpi_processor_idle+0x13a/0x1c7\n" +
                 " [<c0103030>] default_idle+0x0/0x40\n" +
                 " [<c0103030>] default_idle+0x0/0x40\n" +
                 " [<c01030f3>] cpu_idle+0x43/0x60\n" +
                 " [<c03d67e4>] start_kernel+0x154/0x170\n" +
                 " [<c03d63b0>] unknown_bootoption+0x0/0x1a0\n" +
                 "Code: 96 ac 04 00 00 fe 83 8f 00 00 00 c6 83 90 00 00 00 01 89 1c 24 ff 96 94 04 00 00 8b 43 70 8b 40 08 8b 58 20 c7 40 20 00 00 00 00 <8b> 4b 38 c7 43 4c 00 00 00 00 8b 01 8b 51 04 89 43 0c 89 53 10\n" +
                 "<0>Kernel panic - not syncing: Fatal exception in interrupt"
            , false);
        }
    }
}
