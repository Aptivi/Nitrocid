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

using Terminaux.Inputs.Interactive;
using Nitrocid.Languages;
using System.Collections.Generic;
using Textify.General;
using Terminaux.Writer.ConsoleWriters;
using System;
using Terminaux.Inputs.Styles.Infobox;
using System.IO;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Files;
using Nitrocid.Files.Paths;

namespace Nitrocid.Extras.Mods.Modifications.Interactive
{
    /// <summary>
    /// Mod manager TUI class
    /// </summary>
    public class ModManagerTui : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

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
                ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_MODS_TUI_MODNAME", "Nitrocid.Extras.Mods"), selectedMod.ModName) + CharManager.NewLine +
                ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_MODS_TUI_MODFILENAME", "Nitrocid.Extras.Mods"), selectedMod.ModFileName) + CharManager.NewLine +
                ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_MODS_TUI_MODFILEPATH", "Nitrocid.Extras.Mods"), selectedMod.ModFilePath) + CharManager.NewLine +
                ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_MODS_TUI_MODVERSION", "Nitrocid.Extras.Mods"), selectedMod.ModVersion) + CharManager.NewLine + CharManager.NewLine +
                ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_MODS_TUI_MODLANGUAGES", "Nitrocid.Extras.Mods"), $"{selectedMod.ModStrings.Count}") + CharManager.NewLine +
                ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_MODS_TUI_MODENTRYPOINT", "Nitrocid.Extras.Mods"), selectedMod.ModScript.GetType().ToString());
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

        internal void StartModPrompt(bool manual)
        {
            string path = "";
            try
            {
                if (manual)
                {
                    // Give an infobox that lets the user enter the mod path
                    path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MODS_TUI_PATHTOMODPROMPT", "Nitrocid.Extras.Mods"));
                }
                else
                {
                    // Open an interactive TUI that lets the user select one of the mods found in KSMods
                    path = FilesystemTools.SelectFile(PathsManagement.ModsPath);
                }

                // Start the target mod
                ModManager.StartMod(Path.GetFileName(path), ModLoadPriority.Important);
                ModManager.StartMod(Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MODS_TUI_MODSTARTFAILED", "Nitrocid.Extras.Mods") + $" {path}: {ex.Message}");
            }
        }

        internal void StopMod(string? modName)
        {
            if (modName is null)
                return;

            // Get file name from the mod
            var selectedMod = ModManager.GetMod(modName);
            if (selectedMod is null)
                return;

            // Now, stop it
            ModManager.StopMod(selectedMod.ModFileName);
        }

        internal void ReloadMod(string? modName)
        {
            if (modName is null)
                return;

            // Get file name from the mod
            var selectedMod = ModManager.GetMod(modName);
            if (selectedMod is null)
                return;

            // Now, reload it
            ModManager.ReloadMod(selectedMod.ModFileName);
        }

        internal void InstallModPrompt(bool manual)
        {
            string path = "";
            try
            {
                if (manual)
                {
                    // Give an infobox that lets the user enter the mod path
                    path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MODS_TUI_PATHTOMODINSTALLPROMPT", "Nitrocid.Extras.Mods"));
                }
                else
                {
                    // Open an interactive TUI that lets the user select one of the mods
                    path = FilesystemTools.SelectFile();
                }

                // Install the target mod
                ModManager.InstallMod(path);
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MODS_TUI_MODINSTALLFAILED", "Nitrocid.Extras.Mods") + $" {path}: {ex.Message}");
            }
        }

        internal void UninstallMod(string? modName)
        {
            if (modName is null)
                return;

            // Get file name from the mod
            var selectedMod = ModManager.GetMod(modName);
            if (selectedMod is null)
                return;

            // Now, uninstall it
            ModManager.UninstallMod(selectedMod.ModFileName);
        }
    }
}
