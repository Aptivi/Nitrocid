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

using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;
using Nitrocid.Kernel.Starting.Bootloader.Apps;
using Nitrocid.Languages;
using Terminaux.Base.Extensions;

namespace Nitrocid.Kernel.Starting.Bootloader.Style.Styles
{
    internal class NtldrBootStyle : BaseBootStyle, IBootStyle
    {
        internal List<(int, int)> bootEntryPositions = [];

        public override string Render()
        {
            // Prompt the user for selection
            var bootApps = BootManager.GetBootApps();
            var builder = new StringBuilder();
            builder.AppendLine("\n\n" + LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_NTLDR_PROMPT") + ":\n\n");
            for (int i = 0; i < bootApps.Count; i++)
            {
                string bootApp = BootManager.GetBootAppNameByIndex(i);
                bootEntryPositions.Add((0, 5 + i));
                builder.AppendLine($"    {bootApp}");
            }
            builder.AppendLine("\n" + LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_NTLDR_KEYBINDING1"));
            builder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_NTLDR_KEYBINDING2") + "\n\n\n");
            builder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_NTLDR_ADVOPTIONS"));
            return builder.ToString();
        }

        public override string RenderHighlight(int chosenBootEntry)
        {
            // Populate colors
            ConsoleColor highlightedEntryForeground = ConsoleColor.Black;
            ConsoleColor highlightedEntryBackground = ConsoleColor.Gray;

            // Highlight the chosen entry
            string bootApp = BootManager.GetBootAppNameByIndex(chosenBootEntry);
            return TextWriterWhereColor.RenderWhereColorBack("    {0}", bootEntryPositions[chosenBootEntry].Item1, bootEntryPositions[chosenBootEntry].Item2, true, new Color(highlightedEntryForeground), new Color(highlightedEntryBackground), bootApp);
        }

        public override string RenderModalDialog(string content)
        {
            // Populate colors
            ConsoleColor highlightedEntryForeground = ConsoleColor.Black;
            ConsoleColor highlightedEntryBackground = ConsoleColor.Gray;
            var builder = new StringBuilder();

            builder.AppendLine(
               $"""
                
                {content}
                
                """
            );
            builder.AppendLine(
                $"{ColorTools.RenderSetConsoleColor(new Color(highlightedEntryForeground))}" +
                $"{ColorTools.RenderSetConsoleColor(new Color(highlightedEntryBackground), true)}" +
                 "    " + LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_NTLDR_CONTINUE") +
                $"{ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor)}" +
                $"{ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)}"
            );
            builder.AppendLine("\n" + LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_NTLDR_KEYBINDING1"));
            return builder.ToString();
        }

        public override string RenderSelectTimeout(int timeout)
        {
            ConsoleColor hintColor = ConsoleColor.Gray;
            var builder = new StringBuilder();
            int marginX = 2;
            int optionHelpY =
                bootEntryPositions.Count > 0 ?
                bootEntryPositions[bootEntryPositions.Count - 1].Item2 + 9 :
                17;
            string secs = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_BOOTMGR_TIMEOUT");
            int timeoutX = marginX + ConsoleChar.EstimateCellWidth(secs) + 1;
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(secs, marginX, optionHelpY, true, new Color(hintColor)) +
                TextWriterWhereColor.RenderWhereColor($"{timeout} ", timeoutX, optionHelpY, true, new Color(hintColor))
            );
            return builder.ToString();
        }

        public override string ClearSelectTimeout()
        {
            int marginX = 2;
            int timeoutY =
                bootEntryPositions.Count > 0 ?
                bootEntryPositions[bootEntryPositions.Count - 1].Item2 + 9 :
                17;
            ConsoleColor hintColor = ConsoleColor.Gray;
            return TextWriterWhereColor.RenderWhereColor(new string(' ', ConsoleWrapper.WindowWidth - 2), marginX, timeoutY, true, new Color(hintColor));
        }
    }
}
