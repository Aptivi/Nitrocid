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
using System;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class BootMgrError : BaseBSOD
    {
        public override void Simulate()
        {
            // Clear the screen
            ColorTools.LoadBackDry(new Color(0, 0, 0));

            // Render the header and footer
            int marginX = 2;
            int headerY = 0;
            int footerY = ConsoleWrapper.WindowHeight - 1;
            int barLength = ConsoleWrapper.WindowWidth - 4;
            string header = "Windows Boot Manager";
            string footer = "ENTER=Continue";
            int headerTextX = (ConsoleWrapper.WindowWidth / 2) - (header.Length / 2);
            ConsoleColor barColor = ConsoleColor.Gray;
            ConsoleColor barForeground = ConsoleColor.Black;
            TextWriterWhereColor.WriteWhereColorBack(new string(' ', barLength), marginX, headerY, new Color(barForeground), new Color(barColor));
            TextWriterWhereColor.WriteWhereColorBack(new string(' ', barLength), marginX, footerY, new Color(barForeground), new Color(barColor));
            TextWriterWhereColor.WriteWhereColorBack(header, headerTextX, headerY, new Color(barForeground), new Color(barColor));
            TextWriterWhereColor.WriteWhereColorBack(footer, 3, footerY, new Color(barForeground), new Color(barColor));

            // Render the message
            ConsoleColor promptColor = ConsoleColor.White;
            ConsoleColor hintColor = ConsoleColor.Gray;
            int failedHelpY = 2;
            TextWriterWhereColor.WriteWhereColor("Windows failed to start. A recent hardware or software change might be the\ncause. To fix the problem:", marginX, failedHelpY, new Color(hintColor));
            TextWriterWhereColor.WriteWhereColor("1. Insert your Windows installation disc and restart your computer.", marginX + 2, failedHelpY + 3, new Color(hintColor));
            TextWriterWhereColor.WriteWhereColor("2. Choose your language settings, and then click \"Next.\"", marginX + 2, failedHelpY + 4, new Color(hintColor));
            TextWriterWhereColor.WriteWhereColor("3. Click \"Repair your computer.\"", marginX + 2, failedHelpY + 5, new Color(hintColor));
            TextWriterWhereColor.WriteWhereColor("If you do not have this disc, contact your system administrator or computer\nmanufacturer for assistance.", marginX, failedHelpY + 7, new Color(hintColor));
            TextWriterWhereColor.WriteWhereColor($"File: {new Color(promptColor).VTSequenceForeground}\\Boot\\BCD", marginX + 4, failedHelpY + 10, new Color(hintColor));
            TextWriterWhereColor.WriteWhereColor($"Status: {new Color(promptColor).VTSequenceForeground}0xc000000f", marginX + 4, failedHelpY + 12, new Color(hintColor));
            TextWriterWhereColor.WriteWhereColor($"Info: ", marginX + 4, failedHelpY + 14, new Color(hintColor));
            TextWriterWhereColor.WriteWhereColor($"{new Color(promptColor).VTSequenceForeground}An error occurred while attempting to read the boot configuration\ndata.", marginX + 10, failedHelpY + 14, new Color(hintColor));
        }
    }
}
