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
            builder.AppendLine("\n\n" + Translate.DoTranslation("Please select the operating system to start") + ":\n\n");
            for (int i = 0; i < bootApps.Count; i++)
            {
                string bootApp = BootManager.GetBootAppNameByIndex(i);
                bootEntryPositions.Add((0, 5 + i));
                builder.AppendLine($"    {bootApp}");
            }
            builder.AppendLine("\n" + Translate.DoTranslation("Use the up and down arrow keys to move the highlight to your choice."));
            builder.AppendLine(Translate.DoTranslation("Press ENTER to choose.") + "\n\n\n");
            builder.AppendLine(Translate.DoTranslation("For troubleshooting and advanced startup options for Windows, press F8."));
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
                $"{ConsoleColoring.RenderSetConsoleColor(new Color(highlightedEntryForeground))}" +
                $"{ConsoleColoring.RenderSetConsoleColor(new Color(highlightedEntryBackground), true)}" +
                 "    " + Translate.DoTranslation("Continue") +
                $"{ConsoleColoring.RenderSetConsoleColor(ConsoleColoring.CurrentForegroundColor)}" +
                $"{ConsoleColoring.RenderSetConsoleColor(ConsoleColoring.CurrentBackgroundColor, true)}"
            );
            builder.AppendLine("\n" + Translate.DoTranslation("Use the up and down arrow keys to move the highlight to your choice."));
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
            string secs = Translate.DoTranslation("Seconds until the highlighted choice will be started automatically:");
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
