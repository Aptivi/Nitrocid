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
using System.Linq;
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;
using Textify.General;
using Terminaux.Writer.CyclicWriters.Graphical;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Starting.Bootloader.Apps;

namespace Nitrocid.Base.Kernel.Starting.Bootloader.Style.Styles
{
    internal class GrubBootStyle : BaseBootStyle, IBootStyle
    {

        public override string Render()
        {
            // Populate colors
            var builder = new StringBuilder();
            ConsoleColor sectionTitle = ConsoleColor.Gray;
            ConsoleColor boxBorderColor = ConsoleColor.DarkGray;

            // Write the section title
            string finalRenderedSection = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_GRUB_TITLE");
            int halfX = ConsoleWrapper.WindowWidth / 2 - finalRenderedSection.Length / 2;
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(finalRenderedSection, halfX, 2, new Color(sectionTitle))
            );

            // Now, render a box
            var border = new Border()
            {
                Left = 2,
                Top = 4,
                Width = ConsoleWrapper.WindowWidth - 6,
                Height = ConsoleWrapper.WindowHeight - 15,
                Color = boxBorderColor,
            };
            builder.Append(border.Render());

            // Offer help for new users
            string help =
                LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_GRUB_KEYBINDING1") + "\n" +
                LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_GRUB_KEYBINDING2") + "\n" +
                LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_GRUB_KEYBINDING3");
            int longest = help.Split(['\n']).Max((text) => text.Length);
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(help, ConsoleWrapper.WindowWidth / 2 - longest / 2 - 2, ConsoleWrapper.WindowHeight - 8, new Color(sectionTitle))
            );
            return builder.ToString();
        }

        public override string RenderHighlight(int chosenBootEntry)
        {
            // Populate colors
            var builder = new StringBuilder();
            ConsoleColor highlightedEntry = ConsoleColor.Gray;
            ConsoleColor normalEntry = ConsoleColor.Black;

            // Populate boot entries inside the box
            var bootApps = BootManager.GetBootApps();
            (int, int) upperLeftCornerInterior = (3, 5);
            (int, int) lowerLeftCornerInterior = (3, ConsoleWrapper.WindowHeight - 9);
            int maxItemsPerPage = lowerLeftCornerInterior.Item2 - upperLeftCornerInterior.Item2 - 1;
            int currentPage = (int)Math.Truncate(chosenBootEntry / (double)maxItemsPerPage);
            int startIndex = maxItemsPerPage * currentPage;
            int endIndex = maxItemsPerPage * (currentPage + 1) - 1;
            int renderedAnswers = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (i + 1 > bootApps.Count)
                    break;
                string bootApp = BootManager.GetBootAppNameByIndex(i);
                string rendered = i == chosenBootEntry ? $"*{bootApp}" : $" {bootApp}";
                var finalColorBg = i == chosenBootEntry ? highlightedEntry : normalEntry;
                var finalColorFg = i == chosenBootEntry ? normalEntry : highlightedEntry;
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(rendered + new string(' ', ConsoleWrapper.WindowWidth - 6 - rendered.Length), upperLeftCornerInterior.Item1, upperLeftCornerInterior.Item2 + renderedAnswers, false, new Color(finalColorFg), new Color(finalColorBg))
                );
                renderedAnswers++;
            }
            return builder.ToString();
        }

        public override string RenderSelectTimeout(int timeout)
        {
            string help = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_GRUB_TIMEOUT").FormatString(timeout) + " ";
            return TextWriterWhereColor.RenderWhereColor(help, 4, ConsoleWrapper.WindowHeight - 5, true, new Color(ConsoleColor.White));
        }

        public override string ClearSelectTimeout()
        {
            string help = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_GRUB_TIMEOUT").FormatString(Config.MainConfig.BootSelectTimeoutSeconds) + " ";
            return TextWriterWhereColor.RenderWhereColor(new string(' ', help.Length), 4, ConsoleWrapper.WindowHeight - 5, true, new Color(ConsoleColor.White));
        }
    }
}
