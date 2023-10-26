//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class WindowsNt : BaseBSOD
    {
        public override void Simulate()
        {
            KernelColorTools.LoadBack(new Color(ConsoleColors.DarkBlue_000087));
            KernelColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Display technical information
            TextWriterColor.WritePlain($"\n*** STOP: 0x0000007B (0x{RandomDriver.Random():X8},0x{RandomDriver.Random():X8},0x{RandomDriver.Random():X8},0x{RandomDriver.Random():X8})", true);
            TextWriterColor.WritePlain($"PROCESS1_INITIALIZATION_FAILED\n", true);
            TextWriterColor.WritePlain($"CPUID:GenuineIntel 7.2.5 irql:0  SYSVER 0xf000069b\n", true);

            // Display DLL bases
            TextWriterColor.WritePlain($"Dll base DateStmp - Name               Dll base DateStmp - Name", true);
            TextWriterColor.WritePlain($"{RandomDriver.Random():X8} {RandomDriver.Random():X8} - ntoskrnl.exe       {RandomDriver.Random():X8} {RandomDriver.Random():X8} - hal.dll", true);
            TextWriterColor.WritePlain($"{RandomDriver.Random():X8} {RandomDriver.Random():X8} - setupdd.sys        {RandomDriver.Random():X8} {RandomDriver.Random():X8} - pcmcia.sys", true);
            TextWriterColor.WritePlain($"{RandomDriver.Random():X8} {RandomDriver.Random():X8} - SCSIPORT.SYS       {RandomDriver.Random():X8} {RandomDriver.Random():X8} - vga.sys", true);
            TextWriterColor.WritePlain($"{RandomDriver.Random():X8} {RandomDriver.Random():X8} - VIDEOPRT.SYS       {RandomDriver.Random():X8} {RandomDriver.Random():X8} - floppy.sys", true);
            TextWriterColor.WritePlain($"{RandomDriver.Random():X8} {RandomDriver.Random():X8} - i8042prt.sys       {RandomDriver.Random():X8} {RandomDriver.Random():X8} - kbdclass.sys", true);
            TextWriterColor.WritePlain($"{RandomDriver.Random():X8} {RandomDriver.Random():X8} - fastfat.sys\n", true);

            // Display addresses
            TextWriterColor.WritePlain($"Address  dword dump   Build [1057]                           - Name", true);
            for (int i = 0; i < RandomDriver.Random(5, 20); i++)
                TextWriterColor.WritePlain($"{RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} {RandomDriver.Random():X8} - ntoskrnl.exe", true);
            TextWriterColor.WritePlain("", true);

            // Display outro
            TextWriterColor.WritePlain("Restart and set the recovery options in the system control panel", true);
            TextWriterColor.WritePlain("or the /CRASHDEBUG system start option. If this message reappears,", true);
            TextWriterColor.WritePlain("contact your system administrator or technical support group.", true);
        }
    }
}
