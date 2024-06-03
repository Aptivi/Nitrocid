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

using Nitrocid.Kernel.Starting.Bootloader.Apps;
using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Kernel.Starting.Bootloader.Style.Styles
{
    internal class BootMgrBootStyle : BaseBootStyle, IBootStyle
    {
        internal List<(int, int)> bootEntryPositions = [];
        public override Dictionary<ConsoleKeyInfo, Action> CustomKeys { get; }

        public override string Render()
        {
            // Render the header and footer
            int marginX = 2;
            int headerY = 0;
            int footerY = ConsoleWrapper.WindowHeight - 1;
            int barLength = ConsoleWrapper.WindowWidth - 4;
            string header = "Windows Boot Manager";
            string footer = "ENTER=Choose";
            int headerTextX = ConsoleWrapper.WindowWidth / 2 - header.Length / 2;
            var builder = new StringBuilder();
            ConsoleColor barColor = ConsoleColor.Gray;
            ConsoleColor barForeground = ConsoleColor.Black;
            builder.Append(
                TextWriterWhereColor.RenderWhereColorBack(new string(' ', barLength), marginX, headerY, new Color(barForeground), new Color(barColor)) +
                TextWriterWhereColor.RenderWhereColorBack(new string(' ', barLength), marginX, footerY, new Color(barForeground), new Color(barColor)) +
                TextWriterWhereColor.RenderWhereColorBack(header, headerTextX, headerY, new Color(barForeground), new Color(barColor)) +
                TextWriterWhereColor.RenderWhereColorBack(footer, 3, footerY, new Color(barForeground), new Color(barColor))
            );

            // Render the hints
            ConsoleColor promptColor = ConsoleColor.White;
            ConsoleColor hintColor = ConsoleColor.Gray;
            int chooseHelpY = 2;
            int optionHelpY = 12;
            builder.Append(
                TextWriterWhereColor.RenderWhereColor("Choose an operating system to start, or press TAB to select a tool:", marginX, chooseHelpY, new Color(promptColor)) +
                TextWriterWhereColor.RenderWhereColor("(Use the arrow keys to highlight your choice, then press ENTER.)", marginX, chooseHelpY + 1, new Color(hintColor)) +
                TextWriterWhereColor.RenderWhereColor("To specify an advanced option for this choice, press F8.", marginX, optionHelpY, new Color(promptColor))
            );

            // Return the result
            return builder.ToString();
        }

        public override string RenderHighlight(int chosenBootEntry)
        {
            // Populate colors
            ConsoleColor normalEntryFg = ConsoleColor.Gray;
            ConsoleColor normalEntryBg = ConsoleColor.Black;
            ConsoleColor selectedEntryFg = normalEntryBg;
            ConsoleColor selectedEntryBg = normalEntryFg;

            // Populate boot entries
            var builder = new StringBuilder();
            var bootApps = BootManager.GetBootApps();
            int maxItemsPerPage = 6;
            int currentPage = (int)Math.Truncate(chosenBootEntry / (double)maxItemsPerPage);
            int startIndex = maxItemsPerPage * currentPage;
            int endIndex = maxItemsPerPage * (currentPage + 1);
            int renderedAnswers = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (i + 1 > bootApps.Count)
                    break;
                string bootApp = BootManager.GetBootAppNameByIndex(i);
                string rendered = $"{bootApp}";
                var finalColorBg = i == chosenBootEntry ? selectedEntryBg : normalEntryBg;
                var finalColorFg = i == chosenBootEntry ? selectedEntryFg : normalEntryFg;
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(rendered + new string(' ', ConsoleWrapper.WindowWidth - 15 - rendered.Length) + (i == chosenBootEntry ? '>' : ' '), 6, 5 + renderedAnswers, false, new Color(finalColorFg), new Color(finalColorBg))
                );
                renderedAnswers++;
            }
            return builder.ToString();
        }

        public override string RenderBootFailedMessage(string content)
        {
            // Render the header and footer
            int marginX = 2;
            int headerY = 0;
            int footerY = ConsoleWrapper.WindowHeight - 1;
            int barLength = ConsoleWrapper.WindowWidth - 4;
            string header = "Windows Boot Manager";
            string footer = "ENTER=Continue";
            int headerTextX = ConsoleWrapper.WindowWidth / 2 - header.Length / 2;
            var builder = new StringBuilder();
            ConsoleColor barColor = ConsoleColor.Gray;
            ConsoleColor barForeground = ConsoleColor.Black;
            builder.Append(
                TextWriterWhereColor.RenderWhereColorBack(new string(' ', barLength), marginX, headerY, new Color(barForeground), new Color(barColor)) +
                TextWriterWhereColor.RenderWhereColorBack(new string(' ', barLength), marginX, footerY, new Color(barForeground), new Color(barColor)) +
                TextWriterWhereColor.RenderWhereColorBack(header, headerTextX, headerY, new Color(barForeground), new Color(barColor)) +
                TextWriterWhereColor.RenderWhereColorBack(footer, 3, footerY, new Color(barForeground), new Color(barColor))
            );

            // Render the hints first
            ConsoleColor promptColor = ConsoleColor.White;
            ConsoleColor hintColor = ConsoleColor.Gray;
            int failedHelpY = 2;
            builder.Append(
                TextWriterWhereColor.RenderWhereColor("Windows failed to start. A recent hardware or software change might be the\ncause. To fix the problem:", marginX, failedHelpY, new Color(hintColor)) +
                TextWriterWhereColor.RenderWhereColor("1. Insert your Windows installation disc and restart your computer.", marginX + 2, failedHelpY + 3, new Color(hintColor)) +
                TextWriterWhereColor.RenderWhereColor("2. Choose your language settings, and then click \"Next.\"", marginX + 2, failedHelpY + 4, new Color(hintColor)) +
                TextWriterWhereColor.RenderWhereColor("3. Click \"Repair your computer.\"", marginX + 2, failedHelpY + 5, new Color(hintColor)) +
                TextWriterWhereColor.RenderWhereColor("If you do not have this disc, contact your system administrator or computer\nmanufacturer for assistance.", marginX, failedHelpY + 7, new Color(hintColor)) +
                TextWriterWhereColor.RenderWhereColor($"File: {ColorTools.RenderSetConsoleColor(new Color(promptColor))}\\Boot\\BCD", marginX + 4, failedHelpY + 10, new Color(hintColor)) +
                TextWriterWhereColor.RenderWhereColor($"Status: {ColorTools.RenderSetConsoleColor(new Color(promptColor))}0xc000000f", marginX + 4, failedHelpY + 12, new Color(hintColor)) +
                TextWriterWhereColor.RenderWhereColor($"Info:", marginX + 4, failedHelpY + 14, new Color(hintColor)) +
                TextWriterWhereColor.RenderWhereColor(content, marginX + 10, failedHelpY + 14, new Color(promptColor))
            );
            return builder.ToString();
        }

        public override string RenderSelectTimeout(int timeout)
        {
            var builder = new StringBuilder();
            ConsoleColor hintColor = ConsoleColor.Gray;
            int marginX = 2;
            int optionHelpY = 12;
            builder.Append(
                TextWriterWhereColor.RenderWhereColor("Seconds until the highlighted choice will be started automatically:", marginX, optionHelpY + 1, true, new Color(hintColor))
            );
            int timeoutX = marginX + "Seconds until the highlighted choice will be started automatically: ".Length;
            int timeoutY = 13;
            builder.Append(
                TextWriterWhereColor.RenderWhereColor($"{timeout} ", timeoutX, timeoutY, true, new Color(hintColor))
            );
            return builder.ToString();
        }

        public override string ClearSelectTimeout()
        {
            var builder = new StringBuilder();
            int marginX = 2;
            int timeoutY = 13;
            ConsoleColor hintColor = ConsoleColor.Gray;
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(new string(' ', ConsoleWrapper.WindowWidth - 2), marginX, timeoutY, true, new Color(hintColor))
            );
            return builder.ToString();
        }
    }
}
