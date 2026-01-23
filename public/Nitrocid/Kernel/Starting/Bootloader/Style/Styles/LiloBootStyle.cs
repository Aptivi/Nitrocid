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
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;
using Nitrocid.Kernel.Starting.Bootloader.Apps;
using Nitrocid.Kernel.Configuration;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Nitrocid.Languages;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.Kernel.Starting.Bootloader.Style.Styles
{
    internal class LiloBootStyle : BaseBootStyle, IBootStyle
    {

        public override string Render()
        {
            // Populate colors
            ConsoleColor sectionTitle = ConsoleColor.Yellow;
            ConsoleColor boxBorderColor = ConsoleColor.Gray;
            ConsoleColor boxBorderBackgroundColor = ConsoleColor.DarkRed;
            ConsoleColor promptColor = ConsoleColor.Gray;

            // Write the box
            var builder = new StringBuilder();
            int interiorWidth = 41;
            int interiorHeight = 12;
            int bootPrompt = 16;
            int halfX = ConsoleWrapper.WindowWidth / 2 - (interiorWidth + 2) / 2;
            int extraStartX = ConsoleWrapper.WindowWidth / 2 - (interiorWidth + 4) / 2;
            int extraEndX = ConsoleWrapper.WindowWidth / 2 + (interiorWidth + 4) / 2;
            int startY = 1;
            int endY = bootPrompt + startY - 2;
            var borderSettings = new BorderSettings()
            {
                BorderUpperLeftCornerChar = '╓',
                BorderLowerLeftCornerChar = '╙',
                BorderUpperRightCornerChar = '╖',
                BorderLowerRightCornerChar = '╜',
                BorderUpperFrameChar = '─',
                BorderLowerFrameChar = '─',
                BorderLeftFrameChar = '║',
                BorderRightFrameChar = '║',
            };
            var border = new Border()
            {
                Left = halfX,
                Top = startY,
                Width = interiorWidth,
                Height = interiorHeight,
                Color = boxBorderColor,
                BackgroundColor = boxBorderBackgroundColor,
                Settings = borderSettings
            };
            builder.Append(border.Render());
            for (int y = startY; y < endY; y++)
            {
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(" ", extraStartX, y, new Color(boxBorderColor), new Color(boxBorderBackgroundColor)) +
                    TextWriterWhereColor.RenderWhereColorBack(" ", extraEndX, y, new Color(boxBorderColor), new Color(boxBorderBackgroundColor))
                );
            }

            // Offer the boot prompt
            builder.Append(
                TextWriterWhereColor.RenderWhereColor("boot: ", 0, bootPrompt, new Color(promptColor))
            );

            // Now, fill the box with usual things, starting from the title
            string title = "LILO 22.7  Boot Menu";
            int titleX = ConsoleWrapper.WindowWidth / 2 - title.Length / 2;
            int titleY = 3;
            builder.Append(
                TextWriterWhereColor.RenderWhereColorBack(title, titleX, titleY, new Color(sectionTitle), new Color(boxBorderBackgroundColor))
            );

            // The two separators
            int separator1Y = 5;
            int separator2Y = 10;
            string separator1 = "╟──┬─────────────────╥──┬─────────────────╢";
            string separator2 = "╟──┴─────────────────╨──┴─────────────────╢";
            builder.Append(
                TextWriterWhereColor.RenderWhereColorBack(separator1, halfX, separator1Y, new Color(boxBorderColor), new Color(boxBorderBackgroundColor)) +
                TextWriterWhereColor.RenderWhereColorBack(separator2, halfX, separator2Y, new Color(boxBorderColor), new Color(boxBorderBackgroundColor))
            );

            // Connecting the separators
            int startSepY = 6;
            int endSepY = 9;
            int connectX = halfX + 1;
            string separator = "  │                 ║  │                 ";
            for (int y = startSepY; y <= endSepY; y++)
            {
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(separator, connectX, y, new Color(boxBorderColor), new Color(boxBorderBackgroundColor))
                );
            }

            // Write the help text
            int textY = 11;
            int textX = halfX + 1;
            string help =
                Translate.DoTranslation("Hit any key to cancel timeout") + "\n" +
                Translate.DoTranslation("Use arrow keys to make selection") + "\n" +
                Translate.DoTranslation("Enter choice & options, hit CR to boot");
            builder.Append(
                TextWriterWhereColor.RenderWhereColorBack(help, textX, textY, new Color(boxBorderColor), new Color(boxBorderBackgroundColor))
            );
            return builder.ToString();
        }

        public override string RenderHighlight(int chosenBootEntry)
        {
            // Populate colors
            ConsoleColor normalEntryFg = ConsoleColor.Gray;
            ConsoleColor normalEntryBg = ConsoleColor.DarkRed;
            ConsoleColor selectedEntryFg = ConsoleColor.DarkBlue;
            ConsoleColor selectedEntryBg = normalEntryFg;

            // Populate boot entries inside the box
            var builder = new StringBuilder();
            var bootApps = BootManager.GetBootApps();
            int interiorWidth = 41;
            int halfX = ConsoleWrapper.WindowWidth / 2 - (interiorWidth + 2) / 2 + 4;
            (int, int) upperLeftCornerInterior = (halfX, 6);
            int maxItemsPerPage = 4;
            int currentPage = (int)Math.Truncate(chosenBootEntry / (double)maxItemsPerPage);
            int startIndex = maxItemsPerPage * currentPage;
            int endIndex = maxItemsPerPage * (currentPage + 1);
            int renderedAnswers = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (i + 1 > bootApps.Count)
                    break;
                string bootApp = BootManager.GetBootAppNameByIndex(i);
                string rendered = $" {bootApp} ";
                rendered = rendered.Length > 16 ? $" {rendered.Substring(1, 15)} " : rendered;
                var finalColorBg = i == chosenBootEntry ? selectedEntryBg : normalEntryBg;
                var finalColorFg = i == chosenBootEntry ? selectedEntryFg : normalEntryFg;
                builder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(rendered, upperLeftCornerInterior.Item1, upperLeftCornerInterior.Item2 + renderedAnswers, false, new Color(finalColorFg), new Color(finalColorBg))
                );
                renderedAnswers++;
            }
            return builder.ToString();
        }

        public override string RenderSelectTimeout(int timeout)
        {
            string help = $"{TimeSpan.FromSeconds(timeout):mm}:{TimeSpan.FromSeconds(timeout):ss}";
            int textY = 11;
            int interiorWidth = 41;
            int extraEndX = ConsoleWrapper.WindowWidth / 2 + interiorWidth / 2 - help.Length;
            return TextWriterWhereColor.RenderWhereColorBack(help, extraEndX, textY, true, new Color(ConsoleColor.Gray), new Color(ConsoleColor.DarkRed));
        }

        public override string ClearSelectTimeout()
        {
            string help = $"{TimeSpan.FromSeconds(Config.MainConfig.BootSelectTimeoutSeconds):mm}:{TimeSpan.FromSeconds(Config.MainConfig.BootSelectTimeoutSeconds):ss}";
            int textY = 11;
            int interiorWidth = 41;
            int extraEndX = ConsoleWrapper.WindowWidth / 2 + interiorWidth / 2 - help.Length;
            return TextWriterWhereColor.RenderWhereColorBack(new string(' ', help.Length), extraEndX, textY, true, new Color(ConsoleColor.Gray), new Color(ConsoleColor.DarkRed));
        }
    }
}
