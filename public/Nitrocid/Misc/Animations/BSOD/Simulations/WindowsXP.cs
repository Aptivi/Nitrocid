
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Drivers.RNG;
using System;
using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using Terminaux.Colors;

namespace KS.Misc.Animations.BSOD.Simulations
{
    internal class WindowsXP : BaseBSOD
    {
        // Refer to https://learn.microsoft.com/en-us/windows-hardware/drivers/debugger/bug-check-code-reference2 for more info
        public enum BugCheckCodes
        {
            IRQL_NOT_LESS_OR_EQUAL,
            DRIVER_IRQL_NOT_LESS_OR_EQUAL,
            CRITICAL_PROCESS_DIED
        }

        public class BugCheckParams
        {
            public int WindowsBugCheckCode;
            public bool DisplayMessage;
            public string Message = "";
        }

        private static Dictionary<BugCheckCodes, BugCheckParams> BugChecks = new()
        {
            { BugCheckCodes.IRQL_NOT_LESS_OR_EQUAL,         new BugCheckParams() { WindowsBugCheckCode = 0xA,  DisplayMessage = false } },
            { BugCheckCodes.DRIVER_IRQL_NOT_LESS_OR_EQUAL,  new BugCheckParams() { WindowsBugCheckCode = 0xD1, DisplayMessage = false } },
            { BugCheckCodes.CRITICAL_PROCESS_DIED,          new BugCheckParams() { WindowsBugCheckCode = 0xEF, DisplayMessage = true, Message = "\nA process or thread crucial to system operation has unexpectedly exited or been terminated." } },
        };

        public override void Simulate()
        {
            // Select a random bugcheck
            int bugCheckEnumLength = BugChecks.Count;
            int bugCheckIdx = RandomDriver.RandomIdx(bugCheckEnumLength);
            var bugCheck = (BugCheckCodes)Enum.Parse(typeof(BugCheckCodes), bugCheckIdx.ToString());

            // Now, display that bugcheck
            DisplayBugCheck(bugCheck);
        }

        public void DisplayBugCheck(BugCheckCodes BugCheckCode)
        {
            // Windows 7's BSOD is the same as Windows XP's and Windows Vista's BSOD.
            var bugParams = BugChecks[BugCheckCode];
            KernelColorTools.LoadBack(new Color(ConsoleColors.DarkBlue_000087), true);
            KernelColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // First, write the introduction
            TextWriterColor.WritePlain("A problem has been detected and Windows has been shut down to prevent damage\n" +
                                       "to your computer.\n", true);

            // Then, get the message
            bool displayCodeName = RandomDriver.RandomRussianRoulette();
            string bugCheckMessage = bugParams.DisplayMessage ? bugParams.Message :
                                     // We're not displaying message, but display code if Russian Roulette returned true.
                                     displayCodeName ? BugCheckCode.ToString() : "";
            if (!string.IsNullOrEmpty(bugCheckMessage))
                TextWriterColor.WritePlain($"{bugCheckMessage}\n", true);

            // If this is the first time...
            TextWriterColor.WritePlain("If this is the first time you've seen this Stop error screen,\n" +
                                       "restart your computer. If this screen appears again, follow\n" +
                                       "these steps:\n", true);

            // Display some steps as to how to update your software and hardware drivers through Windows Update
            TextWriterColor.WritePlain("Check to make sure any new hardware or software is properly installed.\n" +
                                       "If this is a new installation, ask your hardware or software manufacturer\n" +
                                       "for any Windows updates you might need.\n", true);

            // Display an unhelpful step that only applies to 2001-era computers or older
            TextWriterColor.WritePlain("If problems continue, disable or remove any newly installed hardware\n" +
                                       "or software. Disable BIOS memory options such as caching or shadowing.", true);

            // Safe mode...
            TextWriterColor.WritePlain("If you need to use Safe Mode to remove or disable components, restart\n" +
                                       "your computer, press F8 to select Advanced Startup Options, and then\n" +
                                       "select Safe Mode.\n", true);

            // Display technical information
            TextWriterColor.WritePlain("Technical information:\n\n" +
                                      $"*** STOP: 0x{bugParams.WindowsBugCheckCode:X8} (0x{RandomDriver.Random():X8}, 0x{RandomDriver.Random():X8}, 0x{RandomDriver.Random():X8}, 0x{RandomDriver.Random():X8})\n\n", true);

            // Display dumping message and stop here
            TextWriterColor.WritePlain("Collecting data for crash dump...\n" +
                                       "Initializing disk for crash dump...\n" +
                                       "Beginning dump of physical memory.", true);
            TextWriterColor.WritePlain("Dumping physical memory to disk:  ", false);
        }
    }
}
