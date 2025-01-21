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

using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Interactive;
using Nitrocid.Languages;
using System.Collections.Generic;
using System.Text;
using Textify.General;
using Nitrocid.Extras.Mods.Modifications.ManPages;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.Mods.Modifications.Interactive
{
    /// <summary>
    /// Mod manager TUI class
    /// </summary>
    public class ModManagerTui : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            ModManager.ListMods().Keys;

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item)
        {
            // Get some info from the mod
            var selectedMod = ModManager.GetMod(item);
            if (selectedMod is null)
                return "";

            // Render them to the second pane
            return
                ListEntryWriterColor.RenderListEntry(Translate.DoTranslation("Name"), selectedMod.ModName) + CharManager.NewLine +
                ListEntryWriterColor.RenderListEntry(Translate.DoTranslation("File name"), selectedMod.ModFileName) + CharManager.NewLine +
                ListEntryWriterColor.RenderListEntry(Translate.DoTranslation("File path"), selectedMod.ModFilePath) + CharManager.NewLine +
                ListEntryWriterColor.RenderListEntry(Translate.DoTranslation("Version"), selectedMod.ModVersion) + CharManager.NewLine + CharManager.NewLine +
                ListEntryWriterColor.RenderListEntry(Translate.DoTranslation("Languages"), $"{selectedMod.ModStrings.Count}") + CharManager.NewLine +
                ListEntryWriterColor.RenderListEntry(Translate.DoTranslation("Entry point"), selectedMod.ModScript.GetType().ToString());
            ;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item)
        {
            var selectedMod = ModManager.GetMod(item);
            if (selectedMod is null)
                return "";
            return selectedMod.ModName;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item)
        {
            var selectedMod = ModManager.GetMod(item);
            if (selectedMod is null)
                return "";
            return selectedMod.ModName;
        }
    }
}
