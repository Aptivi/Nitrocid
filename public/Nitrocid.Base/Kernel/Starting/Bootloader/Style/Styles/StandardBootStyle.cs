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

extern alias TextifyDep;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Starting.Bootloader.Apps;
using Nitrocid.Base.Kernel.Starting.Bootloader.Style;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using TextifyDep::Textify.General;

namespace Nitrocid.Base.Kernel.Starting.Bootloader.Style.Styles
{
    internal class StandardBootStyle : BaseBootStyle, IBootStyle
    {
        internal List<(int, int)> bootEntryPositions = [];

        public override string Render()
        {
            // Populate colors
            ConsoleColor bootEntry = ConsoleColor.Blue;

            // Prompt the user for selection
            var builder = new StringBuilder();
            var bootApps = BootManager.GetBootApps();
            builder.AppendLine("\n  " + LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_STD_BOOTENTRY") + "\n");
            for (int i = 0; i < bootApps.Count; i++)
            {
                string bootApp = BootManager.GetBootAppNameByIndex(i);
                bootEntryPositions.Add((0, 3 + i));
                builder.AppendLine($"{ColorTools.RenderSetConsoleColor(new Color(bootEntry))} [{i + 1}] {bootApp}");
            }
            return builder.ToString();
        }

        public override string RenderHighlight(int chosenBootEntry)
        {
            // Populate colors
            ConsoleColor highlightedEntry = ConsoleColor.Cyan;

            // Highlight the chosen entry
            string bootApp = BootManager.GetBootAppNameByIndex(chosenBootEntry);
            return TextWriterWhereColor.RenderWhereColor(" [{0}] {1}", bootEntryPositions[chosenBootEntry].Item1, bootEntryPositions[chosenBootEntry].Item2, true, new Color(highlightedEntry), chosenBootEntry + 1, bootApp);
        }

        public override string RenderBootingMessage(string chosenBootName) =>
            LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_BASE_BOOTING").FormatString(chosenBootName);

        public override string RenderSelectTimeout(int timeout) =>
            TextWriterWhereColor.RenderWhereColor($" {timeout}", ConsoleWrapper.WindowWidth - $" {timeout}".Length - 2, ConsoleWrapper.WindowHeight - 2, true, new Color(ConsoleColor.White));

        public override string ClearSelectTimeout()
        {
            string spaces = new(' ', Config.MainConfig.BootSelectTimeoutSeconds.GetDigits());
            return TextWriterWhereColor.RenderWhereColor(spaces, ConsoleWrapper.WindowWidth - spaces.Length - 2, ConsoleWrapper.WindowHeight - 2, true, new Color(ConsoleColor.White));
        }
    }
}
