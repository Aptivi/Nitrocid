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
using Nitrocid.Base.Files.Unix;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Sequences;
using Terminaux.Themes.Colors;
using Textify.General;

namespace Nitrocid.Base.Files.Instances.Interactives
{
    /// <summary>
    /// File manager class relating to the interactive file manager planned back in 2018 (single-pane like Windows Explorer)
    /// </summary>
    public class FileManagerSingleCli : BaseInteractiveTui<FileSystemEntry>, IInteractiveTui<FileSystemEntry>
    {
        internal string firstPanePath = PathsManagement.HomePath;
        internal bool refreshFirstPaneListing = true;
        private List<FileSystemEntry> firstPaneListing = [];

        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_MISC_INTERACTIVES_FMTUI_SINGLEPANE_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_FMTUI_SINGLEPANE_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SINGLEPANE_HELP01_BODY") + "\n\n" +
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
        public override string GetInfoFromItem(FileSystemEntry? item)
        {
            try
            {
                if (item is null)
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

        internal void Open(FileSystemEntry? entry)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (entry is null)
                    return;

                // Check for existence
                var currentEntry = entry;
                if (currentEntry is null || !currentEntry.Exists)
                    return;

                // Now that the selected file or folder exists, check the type.
                if (currentEntry.Type == FileSystemEntryType.Directory)
                {
                    // We're dealing with a folder. Open it in the selected pane.
                    firstPanePath = FilesystemTools.NeutralizePath(currentEntry.FilePath + "/");
                    refreshFirstPaneListing = true;
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
            firstPanePath = FilesystemTools.NeutralizePath(firstPanePath + "/..");
            refreshFirstPaneListing = true;
            InteractiveTuiTools.SelectionMovement(this, 1);
        }

        internal void PrintFileSystemEntry(FileSystemEntry? entry)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry is null)
                return;
            FileManagerTuiCommon.PrintFileSystemEntry(entry, Settings.InfoBoxSettings);
        }

        internal void GoTo()
        {
            // Now, render the search box
            string root = firstPanePath;
            string path = FilesystemTools.SelectFolder(root);
            path = FilesystemTools.NeutralizePath(path, root);
            if (FilesystemTools.FolderExists(path))
            {
                firstPanePath = path;
                refreshFirstPaneListing = true;
                InteractiveTuiTools.SelectionMovement(this, 1);
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FOLDERNOTFOUND"), Settings.InfoBoxSettings);
        }

        internal void CopyTo(FileSystemEntry? entry)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry is null)
                return;

            // Prompt and copy
            FileManagerTuiCommon.CopyTo(entry, firstPanePath, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
        }

        internal void MoveTo(FileSystemEntry? entry)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry is null)
                return;

            // Prompt and copy
            FileManagerTuiCommon.MoveTo(entry, firstPanePath, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
        }

        internal void RemoveFileOrDir(FileSystemEntry? entry)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry is null)
                return;

            // Remove target
            FileManagerTuiCommon.RemoveFileOrDir(entry, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
        }

        internal void Rename(FileSystemEntry? entry)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry is null)
                return;

            // Prompt and rename
            FileManagerTuiCommon.Rename(entry, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
        }

        internal void MakeDir()
        {
            // Make the directory after prompting the user
            FileManagerTuiCommon.MakeDir(firstPanePath, Settings.InfoBoxSettings);
            refreshFirstPaneListing = true;
        }

        internal void Hash(FileSystemEntry? entry)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry is null)
                return;
            FileManagerTuiCommon.Hash(entry, Settings.InfoBoxSettings);
        }

        internal void Verify(FileSystemEntry? entry)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry is null)
                return;
            FileManagerTuiCommon.Verify(entry, Settings.InfoBoxSettings);
        }

        internal void Preview(FileSystemEntry? entry)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry is null)
                return;
            FileManagerTuiCommon.Preview(entry, Settings.InfoBoxSettings);
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
