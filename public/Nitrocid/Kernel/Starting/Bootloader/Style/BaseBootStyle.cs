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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Starting.Bootloader.Apps;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Inputs.Styles;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Textify.General;

namespace Nitrocid.Kernel.Starting.Bootloader.Style
{
    /// <summary>
    /// Base boot style
    /// </summary>
    public abstract class BaseBootStyle : IBootStyle
    {
        /// <inheritdoc/>
        public virtual Dictionary<ConsoleKeyInfo, Action>? CustomKeys { get; }

        /// <inheritdoc/>
        public virtual string Render()
        {
            // Write the section title
            var builder = new StringBuilder();
            string finalRenderedSection = "-- " + LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_BASE_BOOTENTRY") + " --";
            int halfX = ConsoleWrapper.WindowWidth / 2 - finalRenderedSection.Length / 2;
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(finalRenderedSection, halfX, 1, KernelColorTools.GetColor(KernelColorType.SeparatorText))
            );

            // Now, render a box
            var border = new Border()
            {
                Left = 2,
                Top = 3,
                Width = ConsoleWrapper.WindowWidth - 6,
                Height = ConsoleWrapper.WindowHeight - 8,
                Color = KernelColorTools.GetColor(KernelColorType.Separator)
            };
            builder.Append(border.Render());

            // Offer help for new users
            string help = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_BASE_HELPTIP") + $" {KernelMain.Version}";
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(help, ConsoleWrapper.WindowWidth - help.Length - 2, ConsoleWrapper.WindowHeight - 2, KernelColorTools.GetColor(KernelColorType.NeutralText))
            );
            return builder.ToString();
        }

        /// <inheritdoc/>
        public virtual string RenderHighlight(int chosenBootEntry)
        {
            // Populate boot entries inside the box
            var builder = new StringBuilder();
            var bootApps = BootManager.GetBootApps();
            (int, int) upperLeftCornerInterior = (3, 4);
            (int, int) lowerLeftCornerInterior = (3, ConsoleWrapper.WindowHeight - upperLeftCornerInterior.Item2);
            int maxItemsPerPage = lowerLeftCornerInterior.Item2 - upperLeftCornerInterior.Item2;
            int pages = (int)Math.Truncate(bootApps.Count / (double)maxItemsPerPage);
            int currentPage = (int)Math.Truncate(chosenBootEntry / (double)maxItemsPerPage);
            var bootChoices = bootApps.Select((kvp, idx) => new InputChoiceInfo($"{idx + 1}", kvp.Key)).ToArray();
            var selections = new Selection(bootChoices)
            {
                Left = upperLeftCornerInterior.Item1,
                Top = upperLeftCornerInterior.Item2,
                CurrentSelection = chosenBootEntry,
                Height = maxItemsPerPage,
                Width = ConsoleWrapper.WindowWidth - 6,
                Settings = new()
                {
                    OptionColor = KernelColorTools.GetColor(KernelColorType.Option),
                    SelectedOptionColor = KernelColorTools.GetColor(KernelColorType.SelectedOption),
                }
            };
            builder.Append(selections.Render());

            // Populate page number
            string renderedNumber = $"[{chosenBootEntry + 1}/{bootApps.Count}]═[{currentPage + 1}/{pages}]";
            (int, int) lowerRightCornerToWrite = (ConsoleWrapper.WindowWidth - renderedNumber.Length - 3, ConsoleWrapper.WindowHeight - 4);
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(renderedNumber, lowerRightCornerToWrite.Item1, lowerRightCornerToWrite.Item2, KernelColorTools.GetColor(KernelColorType.Separator))
            );
            return builder.ToString();
        }

        /// <inheritdoc/>
        public virtual string RenderModalDialog(string content)
        {
            // Populate colors
            var splitLines = content.SplitNewLines();
            int maxWidth = splitLines.Max((str) => str.Length);
            int maxHeight = splitLines.Length;
            if (maxWidth >= ConsoleWrapper.WindowWidth)
                maxWidth = ConsoleWrapper.WindowWidth - 4;
            if (maxHeight >= ConsoleWrapper.WindowHeight)
                maxHeight = ConsoleWrapper.WindowHeight - 4;
            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
            var border = new Border()
            {
                Text = content,
                Left = borderX,
                Top = borderY,
                Width = maxWidth,
                Height = maxHeight,
                Color = KernelColorTools.GetColor(KernelColorType.Separator)
            };
            return border.Render();
        }

        /// <inheritdoc/>
        public virtual string RenderBootingMessage(string chosenBootName) =>
            LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_BASE_BOOTING").FormatString(chosenBootName);

        /// <inheritdoc/>
        public virtual string RenderSelectTimeout(int timeout) =>
            TextWriterWhereColor.RenderWhereColor($"{timeout} ", 2, ConsoleWrapper.WindowHeight - 2, true,KernelColorTools.GetColor(KernelColorType.NeutralText));

        /// <inheritdoc/>
        public virtual string ClearSelectTimeout()
        {
            string spaces = new(' ', Config.MainConfig.BootSelectTimeoutSeconds.GetDigits());
            return TextWriterWhereColor.RenderWhereColor(spaces, 2, ConsoleWrapper.WindowHeight - 2, true, KernelColorTools.GetColor(KernelColorType.NeutralText));
        }
    }
}
