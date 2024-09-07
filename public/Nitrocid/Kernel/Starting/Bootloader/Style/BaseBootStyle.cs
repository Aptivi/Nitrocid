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
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
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
            string finalRenderedSection = "-- " + Translate.DoTranslation("Select boot entry") + " --";
            int halfX = ConsoleWrapper.WindowWidth / 2 - finalRenderedSection.Length / 2;
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(finalRenderedSection, halfX, 1, KernelColorTools.GetColor(KernelColorType.SeparatorText))
            );

            // Now, render a box
            builder.Append(
                BorderColor.RenderBorder(2, 3, ConsoleWrapper.WindowWidth - 6, ConsoleWrapper.WindowHeight - 8, KernelColorTools.GetColor(KernelColorType.Separator))
            );

            // Offer help for new users
            string help = Translate.DoTranslation("SHIFT + H for help. Version") + $" {KernelMain.Version}";
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
            builder.Append(
                SelectionInputTools.RenderSelections(bootApps.Select((kvp, idx) => new InputChoiceInfo($"{idx + 1}", kvp.Key)).ToArray(), upperLeftCornerInterior.Item1, upperLeftCornerInterior.Item2, chosenBootEntry, maxItemsPerPage, ConsoleWrapper.WindowWidth - 6, foregroundColor: KernelColorTools.GetColor(KernelColorType.Option), selectedForegroundColor: KernelColorTools.GetColor(KernelColorType.SelectedOption))
            );

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
            return BorderTextColor.RenderBorder(content, borderX, borderY, maxWidth, maxHeight, KernelColorTools.GetColor(KernelColorType.Separator));
        }

        /// <inheritdoc/>
        public virtual string RenderBootingMessage(string chosenBootName) =>
            Translate.DoTranslation("Booting {0}...").FormatString(chosenBootName);

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
