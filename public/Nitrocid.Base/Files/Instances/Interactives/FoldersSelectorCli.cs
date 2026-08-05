//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Files.Extensions;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Sequences;
using Textify.General;

namespace Nitrocid.Base.Files.Instances.Interactives
{
    /// <summary>
    /// Folders selector class, a descendant of the file manager
    /// </summary>
    public class FoldersSelectorCli : BaseInteractiveTui<FileSystemEntry>, IInteractiveTui<FileSystemEntry>
    {
        internal List<string> selectedFolders = [];
        internal string firstPanePath = PathsManagement.HomePath;
        internal bool refreshFirstPaneListing = true;
        private List<FileSystemEntry> firstPaneListing = [];

        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_MISC_INTERACTIVES_FMTUI_FOLDERS_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_FMTUI_FOLDERS_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FOLDERS_HELP01_BODY") + "\n\n" +
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/fundamentals/simulated-kernel-features/files-and-folders",
            }
        ];

        /// <inheritdoc/>
        public override IEnumerable<FileSystemEntry> PrimaryDataSource
        {
            get
            {
                try
                {
                    if (refreshFirstPaneListing)
                    {
                        refreshFirstPaneListing = false;
                        firstPaneListing = FilesystemTools.CreateList(firstPanePath, true);
                    }
                    return firstPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get current directory list for the first pane [{0}]: {1}", vars: [firstPanePath, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return [];
                }
            }
        }

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(FileSystemEntry? item)
        {
            // Check to see if we're given the file system info
            if (item == null)
                return LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOINFO");

            // Now, populate the info to the status
            try
            {
                string status = FileManagerTuiCommon.GetStatusStringFrom(item);
                if (selectedFolders.Count > 0)
                    status = $"{LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SELECTED")}: {selectedFolders.Count} - {LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPACEMORESELECTIONINFO")} - {status}";
                return status;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <inheritdoc/>
        public override string GetInfoFromItem(FileSystemEntry? item)
        {
            try
            {
                if (item == null)
                    return LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOINFO");
                return FileManagerTuiCommon.GetInfoStringFrom(item);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get file entry: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(FileSystemEntry? item)
        {
            try
            {
                if (item is null)
                    return LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOINFO");
                bool isSelected = SelectedFolders.Contains(item.FilePath);
                return $"[{(isSelected ? "+" : " ")}] " + FileManagerTuiCommon.GetEntryStringFrom(item);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get entry: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        /// <summary>
        /// Selected files. If not selected yet and bailed earlier, this list is empty.
        /// </summary>
        public string[] SelectedFolders =>
            [.. selectedFolders];

        internal void Select(FileSystemEntry? currentFileSystemEntry)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (currentFileSystemEntry is null)
                    return;

                // Check for existence
                if (!currentFileSystemEntry.Exists)
                    return;

                // Now that the selected file or folder exists, check the type.
                if (currentFileSystemEntry.Type == FileSystemEntryType.Directory)
                {
                    // We're dealing with a folder. Clear the screen and open the appropriate editor.
                    if (!selectedFolders.Remove(currentFileSystemEntry.FilePath))
                    {
                        selectedFolders.Add(currentFileSystemEntry.FilePath);
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SELECTSUCCESS_MULTI"), Settings.InfoBoxSettings, currentFileSystemEntry.FilePath);
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_UNSELECTSUCCESS_MULTI"), Settings.InfoBoxSettings, currentFileSystemEntry.FilePath);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANNOTSELECTFOLDER") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void Open(FileSystemEntry? currentFileSystemEntry)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (currentFileSystemEntry is null)
                    return;

                // Check for existence
                if (!currentFileSystemEntry.Exists)
                    return;

                // Now that the selected file or folder exists, check the type.
                if (currentFileSystemEntry.Type == FileSystemEntryType.Directory)
                {
                    // We're dealing with a folder. Open it in the selected pane.
                    firstPanePath = FilesystemTools.NeutralizePath(currentFileSystemEntry.FilePath + "/");
                    InteractiveTuiTools.SelectionMovement(this, 1);
                    refreshFirstPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANNOTOPENFOLDER") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void GoUp()
        {
            firstPanePath = FilesystemTools.NeutralizePath(firstPanePath + "/..");
            InteractiveTuiTools.SelectionMovement(this, 1);
            refreshFirstPaneListing = true;
        }

        internal void PrintFileSystemEntry(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;
            FileManagerTuiCommon.PrintFileSystemEntry(currentFileSystemEntry, Settings.InfoBoxSettings);
        }

        internal void RemoveFileOrDir(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // Remove target
            FileManagerTuiCommon.RemoveFileOrDir(currentFileSystemEntry, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
        }

        internal void GoTo()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FOLDERPROMPT"), Settings.InfoBoxSettings);
            path = FilesystemTools.NeutralizePath(path, firstPanePath);
            if (FilesystemTools.FolderExists(path))
            {
                InteractiveTuiTools.SelectionMovement(this, 1);
                firstPanePath = path;
                refreshFirstPaneListing = true;
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FOLDERNOTFOUND"), Settings.InfoBoxSettings);
        }

        internal void CopyTo(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // Prompt and copy
            FileManagerTuiCommon.CopyTo(currentFileSystemEntry, firstPanePath, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
        }

        internal void MoveTo(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // Prompt and copy
            FileManagerTuiCommon.MoveTo(currentFileSystemEntry, firstPanePath, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
        }

        internal void Rename(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // Prompt and rename
            FileManagerTuiCommon.Rename(currentFileSystemEntry, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
        }

        internal void MakeDir()
        {
            // Make the directory after prompting the user
            FileManagerTuiCommon.MakeDir(firstPanePath, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
        }

        internal void Hash(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;
            FileManagerTuiCommon.Hash(currentFileSystemEntry, Settings.InfoBoxSettings);
        }

        internal void Verify(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;
            FileManagerTuiCommon.Verify(currentFileSystemEntry, Settings.InfoBoxSettings);
        }

        internal void Preview(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;
            FileManagerTuiCommon.Preview(currentFileSystemEntry, Settings.InfoBoxSettings);
        }

        internal void PreviewSelected()
        {
            string selected =
                SelectedFolders.Length > 0 ?
                $"  - {string.Join("\n  - ", SelectedFolders)}" :
                LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOSELECTIONSFOLDER");
            InfoBoxModalColor.WriteInfoBoxModal(selected, Settings.InfoBoxSettings);
        }

        internal void ShowUnixPermissionChangeInfoBoxInstance(FileSystemEntry? entry)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry is null)
                return;

            // Show this infobox
            FileManagerTuiCommon.ShowUnixPermissionChangeInfoBox(entry, Settings.InfoBoxSettings);
        }
    }
}
