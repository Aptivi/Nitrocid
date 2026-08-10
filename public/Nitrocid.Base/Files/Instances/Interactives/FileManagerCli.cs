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
using System.Text;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Themes.Colors;
using Textify.General;

namespace Nitrocid.Base.Files.Instances.Interactives
{
    /// <summary>
    /// File manager class relating to the interactive file manager planned back in 2018 (multi-pane like Total Commander)
    /// </summary>
    public class FileManagerCli : BaseInteractiveTui<FileSystemEntry>, IInteractiveTui<FileSystemEntry>
    {
        internal string firstPanePath = PathsManagement.HomePath;
        internal string secondPanePath = PathsManagement.HomePath;
        internal bool refreshFirstPaneListing = true;
        internal bool refreshSecondPaneListing = true;
        private List<FileSystemEntry> firstPaneListing = [];
        private List<FileSystemEntry> secondPaneListing = [];

        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_MISC_INTERACTIVES_FMTUI_DOUBLEPANE_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_FMTUI_DOUBLEPANE_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_DOUBLEPANE_HELP01_BODY") + "\n\n" +
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/fundamentals/simulated-kernel-features/files-and-folders",
            }
        ];

        /// <summary>
        /// Always true in the file manager as we want it to behave like Total Commander
        /// </summary>
        public override bool SecondPaneInteractable =>
            true;

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
        public override IEnumerable<FileSystemEntry> SecondaryDataSource
        {
            get
            {
                try
                {
                    if (refreshSecondPaneListing)
                    {
                        refreshSecondPaneListing = false;
                        secondPaneListing = FilesystemTools.CreateList(secondPanePath, true);
                    }
                    return secondPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get current directory list for the second pane [{0}]: {1}", vars: [secondPanePath, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return [];
                }
            }
        }

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(FileSystemEntry item)
        {
            // Check to see if we're given the file system info
            if (item == null)
                return LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOINFO");

            // Now, populate the info to the status
            try
            {
                return FileManagerTuiCommon.GetStatusStringFrom(item);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(FileSystemEntry item)
        {
            try
            {
                return FileManagerTuiCommon.GetEntryStringFrom(item);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get entry: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        /// <inheritdoc/>
        public override string GetStatusFromItemSecondary(FileSystemEntry item) =>
            GetStatusFromItem(item);

        /// <inheritdoc/>
        public override string GetEntryFromItemSecondary(FileSystemEntry item) =>
            GetEntryFromItem(item);

        internal void Open(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (entry1 is null && entry2 is null)
                    return;

                // Check for existence
                var currentEntry = CurrentPane == 2 ? entry2 : entry1;
                if (currentEntry is null || !currentEntry.Exists)
                    return;

                // Now that the selected file or folder exists, check the type.
                if (currentEntry.Type == FileSystemEntryType.Directory)
                {
                    // We're dealing with a folder. Open it in the selected pane.
                    if (CurrentPane == 2)
                    {
                        secondPanePath = FilesystemTools.NeutralizePath(currentEntry.FilePath + "/");
                        refreshSecondPaneListing = true;
                    }
                    else
                    {
                        firstPanePath = FilesystemTools.NeutralizePath(currentEntry.FilePath + "/");
                        refreshFirstPaneListing = true;
                    }
                    InteractiveTuiTools.SelectionMovement(this, 1);
                }
                else if (currentEntry.Type == FileSystemEntryType.File)
                {
                    // We're dealing with a file. Clear the screen and open the appropriate editor.
                    ThemeColorsTools.LoadBackground();
                    FilesystemTools.OpenDeterministically(currentEntry.FilePath);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTOPENFILEFOLDER") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void GoUp()
        {
            if (CurrentPane == 2)
            {
                secondPanePath = FilesystemTools.NeutralizePath(secondPanePath + "/..");
                refreshSecondPaneListing = true;
            }
            else
            {
                firstPanePath = FilesystemTools.NeutralizePath(firstPanePath + "/..");
                refreshFirstPaneListing = true;
            }
            InteractiveTuiTools.SelectionMovement(this, 1);
        }

        internal void PrintFileSystemEntry(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Get current entry
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;
            FileManagerTuiCommon.PrintFileSystemEntry(currentEntry, Settings.InfoBoxSettings);
        }

        internal void CopyFileOrDir(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Get current entry
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;

            // Get destination and copy
            string dest = (CurrentPane == 2 ? firstPanePath : secondPanePath) + "/";
            FileManagerTuiCommon.CopyFileOrDir(currentEntry, dest, Settings.InfoBoxSettings);
            if (CurrentPane == 2)
                refreshFirstPaneListing = true;
            else
                refreshSecondPaneListing = true;
        }

        internal void MoveFileOrDir(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Get current entry
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;

            // Get destination and copy
            string dest = (CurrentPane == 2 ? firstPanePath : secondPanePath) + "/";
            FileManagerTuiCommon.MoveFileOrDir(currentEntry, dest, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
            refreshSecondPaneListing = true;
        }

        internal void RemoveFileOrDir(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Get current entry
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;

            // Remove target
            FileManagerTuiCommon.RemoveFileOrDir(currentEntry, Settings.InfoBoxSettings);
            if (CurrentPane == 2)
                refreshSecondPaneListing = true;
            else
                refreshFirstPaneListing = true;
        }

        internal void GoTo()
        {
            // Now, render the search box
            string root = CurrentPane == 2 ? secondPanePath : firstPanePath;
            string path = FilesystemTools.SelectFolder(root);
            path = FilesystemTools.NeutralizePath(path, root);
            if (FilesystemTools.FolderExists(path))
            {
                if (CurrentPane == 2)
                {
                    secondPanePath = path;
                    refreshSecondPaneListing = true;
                }
                else
                {
                    firstPanePath = path;
                    refreshFirstPaneListing = true;
                }
                InteractiveTuiTools.SelectionMovement(this, 1);
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FOLDERNOTFOUND"), Settings.InfoBoxSettings);
        }

        internal void CopyTo(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Get current entry
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;

            // Prompt and copy
            string dest = CurrentPane == 2 ? secondPanePath : firstPanePath;
            FileManagerTuiCommon.CopyTo(currentEntry, dest, Settings.InfoBoxSettings);
            if (CurrentPane == 2)
                refreshFirstPaneListing = true;
            else
                refreshSecondPaneListing = true;
        }

        internal void MoveTo(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Get current entry
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;

            // Prompt and copy
            string dest = CurrentPane == 2 ? secondPanePath : firstPanePath;
            FileManagerTuiCommon.MoveTo(currentEntry, dest, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
            refreshSecondPaneListing = true;
        }

        internal void Rename(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Get current entry
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;

            // Prompt and rename
            FileManagerTuiCommon.Rename(currentEntry, Settings.InfoBoxSettings);
            if (CurrentPane == 2)
                refreshSecondPaneListing = true;
            else
                refreshFirstPaneListing = true;
        }

        internal void MakeDir()
        {
            // Make the directory after prompting the user
            string dest = CurrentPane == 2 ? secondPanePath : firstPanePath;
            FileManagerTuiCommon.MakeDir(dest, Settings.InfoBoxSettings);
            if (CurrentPane == 2)
                refreshSecondPaneListing = true;
            else
                refreshFirstPaneListing = true;
        }

        internal void Hash(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Get current entry
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;
            FileManagerTuiCommon.Hash(currentEntry, Settings.InfoBoxSettings);
        }

        internal void Verify(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Get current entry
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;
            FileManagerTuiCommon.Verify(currentEntry, Settings.InfoBoxSettings);
        }

        internal void Preview(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // First, check to see if it's a file
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;
            FileManagerTuiCommon.Preview(currentEntry, Settings.InfoBoxSettings);
        }

        internal void ShowUnixPermissionChangeInfoBoxInstance(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Get the current entry
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;

            // Show this infobox
            FileManagerTuiCommon.ShowUnixPermissionChangeInfoBox(currentEntry, Settings.InfoBoxSettings);
        }
    }
}
