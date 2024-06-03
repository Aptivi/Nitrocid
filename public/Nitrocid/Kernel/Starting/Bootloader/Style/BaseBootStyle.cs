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

using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Starting.Bootloader.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Colors;
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
        public virtual Dictionary<ConsoleKeyInfo, Action> CustomKeys { get; }

        /// <inheritdoc/>
        public virtual string Render()
        {
            // Populate colors
            ConsoleColor sectionTitle = ConsoleColor.Green;
            ConsoleColor boxBorderColor = ConsoleColor.Gray;

            // Write the section title
            var builder = new StringBuilder();
            string finalRenderedSection = "-- Select boot entry --";
            int halfX = ConsoleWrapper.WindowWidth / 2 - finalRenderedSection.Length / 2;
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(finalRenderedSection, halfX, 2, new Color(sectionTitle))
            );

            // Now, render a box
            builder.Append(
                BorderColor.RenderBorder(2, 4, ConsoleWrapper.WindowWidth - 6, ConsoleWrapper.WindowHeight - 9, new Color(boxBorderColor))
            );

            // Offer help for new users
            string help = $"SHIFT + H for help. Version {KernelMain.Version}";
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(help, ConsoleWrapper.WindowWidth - help.Length - 2, ConsoleWrapper.WindowHeight - 2, new Color(ConsoleColor.White))
            );
            return builder.ToString();
        }

        /// <inheritdoc/>
        public virtual string RenderHighlight(int chosenBootEntry)
        {
            // Populate colors
            ConsoleColor highlightedEntry = ConsoleColor.DarkGreen;
            ConsoleColor normalEntry = ConsoleColor.Gray;
            ConsoleColor pageNumberColor = ConsoleColor.Gray;

            // Populate boot entries inside the box
            var builder = new StringBuilder();
            var bootApps = BootManager.GetBootApps();
            (int, int) upperLeftCornerInterior = (4, 6);
            (int, int) lowerLeftCornerInterior = (4, ConsoleWrapper.WindowHeight - 6);
            int maxItemsPerPage = lowerLeftCornerInterior.Item2 - upperLeftCornerInterior.Item2;
            int pages = (int)Math.Truncate(bootApps.Count / (double)maxItemsPerPage);
            int currentPage = (int)Math.Truncate(chosenBootEntry / (double)maxItemsPerPage);
            int startIndex = maxItemsPerPage * currentPage;
            int endIndex = maxItemsPerPage * (currentPage + 1) - 1;
            int renderedAnswers = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (i + 1 > bootApps.Count)
                    break;
                string bootApp = BootManager.GetBootAppNameByIndex(i);
                string rendered = $" >> {bootApp}";
                var finalColor = i == chosenBootEntry ? highlightedEntry : normalEntry;
                builder.Append(
                    TextWriterWhereColor.RenderWhereColor(rendered, upperLeftCornerInterior.Item1, upperLeftCornerInterior.Item2 + renderedAnswers, new Color(finalColor))
                );
                renderedAnswers++;
            }

            // Populate page number
            string renderedNumber = $"[{chosenBootEntry + 1}/{bootApps.Count}]═[{currentPage + 1}/{pages}]";
            (int, int) lowerRightCornerToWrite = (ConsoleWrapper.WindowWidth - renderedNumber.Length - 3, ConsoleWrapper.WindowHeight - 4);
            builder.Append(
                TextWriterWhereColor.RenderWhereColor(renderedNumber, lowerRightCornerToWrite.Item1, lowerRightCornerToWrite.Item2, new Color(pageNumberColor))
            );
            return builder.ToString();
        }

        /// <inheritdoc/>
        public virtual string RenderModalDialog(string content)
        {
            // Populate colors
            ConsoleColor dialogBG = ConsoleColor.Black;
            ConsoleColor dialogFG = ConsoleColor.Gray;
            ColorTools.LoadBack();

            var splitLines = content.SplitNewLines();
            int maxWidth = splitLines.Max((str) => str.Length);
            int maxHeight = splitLines.Length;
            if (maxWidth >= ConsoleWrapper.WindowWidth)
                maxWidth = ConsoleWrapper.WindowWidth - 4;
            if (maxHeight >= ConsoleWrapper.WindowHeight)
                maxHeight = ConsoleWrapper.WindowHeight - 4;
            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
            return BorderTextColor.RenderBorder(content, borderX, borderY, maxWidth, maxHeight, new Color(dialogFG), new Color(dialogBG));
        }

        /// <inheritdoc/>
        public virtual string RenderBootingMessage(string chosenBootName) =>
            $"Booting {chosenBootName}...";

        /// <inheritdoc/>
        public virtual string RenderBootFailedMessage(string content) =>
            content;

        /// <inheritdoc/>
        public virtual string RenderSelectTimeout(int timeout) =>
            TextWriterWhereColor.RenderWhereColor($"{timeout} ", 2, ConsoleWrapper.WindowHeight - 2, true, new Color(ConsoleColor.White));

        /// <inheritdoc/>
        public virtual string ClearSelectTimeout()
        {
            string spaces = new(' ', GetDigits(Config.MainConfig.BootSelectTimeoutSeconds));
            return TextWriterWhereColor.RenderWhereColor(spaces, 2, ConsoleWrapper.WindowHeight - 2, true, new Color(ConsoleColor.White));
        }

        internal static int GetDigits(int Number) =>
            Number == 0 ? 1 : (int)Math.Log10(Math.Abs(Number)) + 1;
    }
}
