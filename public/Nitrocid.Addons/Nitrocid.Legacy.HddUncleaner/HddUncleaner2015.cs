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

using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Languages;

namespace Nitrocid.Legacy.HddUncleaner
{
    internal class HddUncleaner2015
    {
        // This "mock app" mocks "HDD Cleaner 2.1," which was released on November 24th, 2015. We'll call it
        // "HDD Uncleaner 2015." It'll basically do nothing except display messages to the console using the
        // Nitrocid KS API. It'll try to simulate how this archived program looks like. Please don't take
        // this too literally as it's just a joke program intended to amuse you. It also doesn't simulate
        // extraneous features, like the "AutoRecovery" routine.
        //
        // Please note that we don't intend to localize this.

        private static bool exiting = false;

        internal static void EntryPoint()
        {
            // Disclaimer
            TextWriters.Write(Translate.DoTranslation("This \"mock app\" mocks \"HDD Cleaner 2.1,\" which was released on November 24th, 2015. We'll call it \"HDD Uncleaner 2015.\" It'll basically do nothing except display messages to the console using the Nitrocid KS API. It'll try to simulate how this archived program looks like. Please don't take this too literally as it's just a joke program intended to amuse you. It also doesn't simulate extraneous features, like the \"AutoRecovery\" routine. Please note that we don't intend to localize this. Press any key to start."), true, KernelColorType.Warning);
            Input.DetectKeypress();

            while (!exiting)
                TagMain();
            exiting = false;

            // Clean up after ourselves
            ConsoleWrapper.Clear();
        }

        private static void TagMain()
        {
            // :main (line 138)
            ConsoleExtensions.SetTitle("-----==++==----- Hard Disk Cleaner -----==++==-----");
            ConsoleWrapper.Clear();
            TextWriterColor.Write();
            TextWriterColor.Write("+---------------------------------------------------------+");
            TextWriterColor.Write(": Hard Disk Cleaner   -----==+==-----   Choice 8 for exit :");
            TextWriterColor.Write("+---------------------------------------------------------+");
            TextWriterColor.Write();
            TextWriterColor.Write("              +-----------------------------+");
            TextWriterColor.Write("        -----=:=-----  Select Options -----=:=-----");
            TextWriterColor.Write("              +-----------------------------+");
            TextWriterColor.Write("              : +--- Options -+- Saves ---+ :");
            TextWriterColor.Write("              : :1. Defrag    : saves more: :");
            TextWriterColor.Write("              : :2. Cleanup   : saves some: :");
            TextWriterColor.Write("              : :3. Manual    : saves less: :");
            TextWriterColor.Write("              : :4. Reinstall : saves all : :");
            TextWriterColor.Write("              : :5. Programs  : saves more: :");
            TextWriterColor.Write("              : :6. SysRestore: saves more: :");
            TextWriterColor.Write("              : :7. Hibernate : saves more: :");
            TextWriterColor.Write("              : +-------------------------+ :");
            TextWriterColor.Write("              +---End support of HC 1.0.0---+");
            TextWriterColor.Write();
            string option = Input.ReadLine("    Your option:", "");

            // :main - Checking for option (line 165)
            ConsoleWrapper.Clear();
            switch (option)
            {
                case "1":
                    // User selected the Defrag option. Translate this to the :defrag tag
                    // :defrag (line 182)
                    ConsoleExtensions.SetTitle("-----==++==----- Defrag -----==++==-----");
                    OsSelect();
                    break;
                case "2":
                    // User selected the Cleanup option. Translate this to the :cleans tag
                    // :cleans (line 205)
                    ConsoleExtensions.SetTitle("-----==++==----- Clean Disk -----==++==-----");
                    OsSelect();
                    break;
                case "3":
                    // User selected the Manual option. Translate this to the :explor tag
                    // :explor (line 228)
                    ConsoleExtensions.SetTitle("-----==++==----- Explorer -----==++==-----");
                    Input.DetectKeypress();
                    break;
                case "4":
                    // User selected the Reinstall option. Translate this to the :format tag
                    // :format (line 236)
                    ConsoleExtensions.SetTitle("-----==++==----- WARNING!! -----==++==-----");
                    TextWriterColor.Write();
                    TextWriterColor.Write("IMPORTANT, YOU MUST BACKUP FILES TO FLOPPY, USB OR EXTERNAL HDD OTHERWISE YOU'LL LOSE YOUR IMPORTANT FILES, APPS AND GAMES!");
                    Input.DetectKeypress();
                    ConsoleWrapper.Clear();
                    OsSelect();
                    break;
                case "5":
                    // User selected the Programs option. Translate this to the :progra tag
                    // :progra (line 333)
                    ConsoleExtensions.SetTitle("-----==++==----- Add or remove programs -----==++==-----");
                    Input.DetectKeypress();
                    break;
                case "6":
                    // User selected the SysRestore option. Translate this to the :sysres tag
                    // :sysres (line 341)
                    ConsoleExtensions.SetTitle("-----==++==----- System Restore Turnoff -----==++==-----");
                    OsSelect();
                    break;
                case "7":
                    // User selected the Hibernate option. Translate this to the :hibern tag
                    // :hibern (line 384)
                    ConsoleExtensions.SetTitle("-----==++==----- Hibernation system off -----==++==-----");
                    TextWriterColor.Write();
                    TextWriterColor.Write("1. Click on \"Hibernate\" tab");
                    TextWriterColor.Write("2. Clear the check box \"Enable Hibernation\"");
                    TextWriterColor.Write("3. Click on OK or Apply button");
                    Input.ReadLine("  Your OS:", "");
                    break;
                case "8":
                    // User selected the exit option. Translate this to the :exihdc tag
                    // :exihdc (line 607)
                    ConsoleExtensions.SetTitle("-----==++==----- Deleting cache... -----==++==-----");
                    exiting = true;
                    break;
                default:
                    // User selected the invalid option.
                    // :main (line 175)
                    ConsoleExtensions.SetTitle("-----==++==----- Wrong Parameter! -----==++==-----");
                    TextWriterColor.Write();
                    TextWriterColor.Write($"The option {option} is not found, We will add soon.");
                    Input.DetectKeypress();
                    break;
            }
        }

        private static void OsSelect()
        {
            // Of course, this is not VMware Workstation.
            TextWriterColor.Write();
            TextWriterColor.Write("Which OS do you run?");
            TextWriterColor.Write();
            TextWriterColor.Write("1. Windows XP");
            TextWriterColor.Write("2. Windows 7");
            TextWriterColor.Write();

            // Prompt for option. "HDD Cleaner" didn't check for option validity.
            Input.ReadLine("    Your option:", "");
        }
    }
}
