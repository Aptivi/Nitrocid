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
using Terminaux.Inputs;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Modules;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Sequences;
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
            FileSystemEntry FileInfoCurrentPane = item;

            // Check to see if we're given the file system info
            if (FileInfoCurrentPane == null)
                return LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOINFO");

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = FileInfoCurrentPane.Type == FileSystemEntryType.Directory;
                UnixPermissionType permissionTypeUser = item.UnixPermissions[0].Types;
                UnixPermissionType permissionTypeGroup = item.UnixPermissions[1].Types;
                UnixPermissionType permissionTypeOther = item.UnixPermissions[2].Types;
                string finalRenderedPermissions =
                    $"[{UnixPermissionManager.BuildPermissionRepresentation(permissionTypeUser)}" +
                    $" {UnixPermissionManager.BuildPermissionRepresentation(permissionTypeGroup)}" +
                    $" {UnixPermissionManager.BuildPermissionRepresentation(permissionTypeOther)}]";
                string finalRenderedSpecialPermissions = $"[{UnixPermissionManager.BuildSpecialPermissionRepresentation(item.UnixSpecial)}]";
                return
                    $"[{(infoIsDirectory ? "/" : "*")}] {finalRenderedPermissions} {finalRenderedSpecialPermissions} " +
                    FileInfoCurrentPane.BaseEntry.FullName;
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
                FileSystemEntry file = item;
                bool isDirectory = file.Type == FileSystemEntryType.Directory;
                if (Config.MainConfig.IfmShowFileSize)
                    return
                        // Name and directory indicator
                        $"[{(isDirectory ? "/" : "*")}] {file.BaseEntry.Name} | " +

                        // File size or directory size
                        $"{(!isDirectory ? ((FileInfo)file.BaseEntry).Length.SizeString() : FilesystemTools.GetAllSizesInFolder((DirectoryInfo)file.BaseEntry).SizeString())}"
                    ;
                else
                    return $"[{(isDirectory ? "/" : "*")}] {file.BaseEntry.Name}";
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

            // Render the final information string
            try
            {
                var currentEntry = CurrentPane == 2 ? entry2 : entry1;
                if (currentEntry is null || !currentEntry.Exists)
                    return;
                var finalInfoRendered = new StringBuilder();
                string fullPath = currentEntry.FilePath;
                if (FilesystemTools.FolderExists(fullPath))
                {
                    // The file system info instance points to a folder
                    var DirInfo = new DirectoryInfo(fullPath);
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME"), DirInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_FULLNAME"), FilesystemTools.NeutralizePath(DirInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYSIZE"), FilesystemTools.GetAllSizesInFolder(DirInfo).SizeString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_CREATIONTIME"), TimeDateRenderers.Render(DirInfo.CreationTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTACCESSTIME"), TimeDateRenderers.Render(DirInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTWRITETIME"), TimeDateRenderers.Render(DirInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_ATTRIBUTES"), DirInfo.Attributes));
                    if (DirInfo.Parent is not null)
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_PARENTDIRECTORY"), FilesystemTools.NeutralizePath(DirInfo.Parent.FullName)));
                }
                else
                {
                    // The file system info instance points to a file
                    FileInfo fileInfo = new(fullPath);
                    bool isBinary = FilesystemTools.IsBinaryFile(fileInfo.FullName);
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME"), fileInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_FULLNAME"), FilesystemTools.NeutralizePath(fileInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_FILESIZE"), fileInfo.Length.SizeString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_CREATIONTIME"), TimeDateRenderers.Render(fileInfo.CreationTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTACCESSTIME"), TimeDateRenderers.Render(fileInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTWRITETIME"), TimeDateRenderers.Render(fileInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_ATTRIBUTES"), fileInfo.Attributes));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_WHERETOFIND"), FilesystemTools.NeutralizePath(fileInfo.DirectoryName)));
                    if (!isBinary)
                    {
                        var Style = FilesystemTools.GetLineEndingFromFile(fullPath);
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NEWLINESTYLE") + " {0}", Style.ToString()));
                    }
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_BINARYFILE") + " {0}", isBinary));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_MIMEMETADATA") + " {0}", MimeTypes.GetMimeType(fileInfo.Extension)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_MIMEMETADATAEXT") + ": {0}", MimeTypes.GetExtendedMimeType(fileInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILETYPE") + ": {0}\n", MimeTypes.GetMagicInfo(fileInfo.FullName)));

                    // .NET managed info
                    if (ReflectionCommon.IsDotnetAssemblyFile(fullPath, out AssemblyName? asmName) && asmName is not null)
                    {
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME"), asmName.Name));
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FULLNAME") + ": {0}", asmName.FullName));
                        if (asmName.Version is not null)
                            finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_VERSION") + ": {0}", asmName.Version.ToString()));
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CULTURENAME") + ": {0}", asmName.CultureName));
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CONTENTTYPE") + ": {0}\n", asmName.ContentType.ToString()));
                    }
                    else
                    {
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTDOTNETASM"));
                    }

                    // Other info handled by the extension handler
                    if (ExtensionHandlerTools.IsHandlerRegistered(fileInfo.Extension))
                    {
                        var handler = ExtensionHandlerTools.GetExtensionHandler(fileInfo.Extension) ??
                            throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_EXCEPTION_HANDLERFAILED") + $" {fileInfo.Extension}");
                        finalInfoRendered.AppendLine(handler.InfoHandler(fullPath));
                    }
                }

                // Now, render the info box
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTGETFSINFO") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void CopyFileOrDir(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                var currentEntry = CurrentPane == 2 ? entry2 : entry1;
                if (currentEntry is null || !currentEntry.Exists)
                    return;
                string dest = (CurrentPane == 2 ? firstPanePath : secondPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {dest}");
                DebugCheck.AssertNull(dest, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
                FilesystemTools.CopyFileOrDir(currentEntry.FilePath, dest);
                if (CurrentPane == 2)
                    refreshFirstPaneListing = true;
                else
                    refreshSecondPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTCOPY") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MoveFileOrDir(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                var currentEntry = CurrentPane == 2 ? entry2 : entry1;
                if (currentEntry is null || !currentEntry.Exists)
                    return;
                string dest = (CurrentPane == 2 ? firstPanePath : secondPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {dest}");
                DebugCheck.AssertNull(dest, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
                FilesystemTools.MoveFileOrDir(currentEntry.FilePath, dest);
                refreshSecondPaneListing = true;
                refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTMOVE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void RemoveFileOrDir(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                var currentEntry = CurrentPane == 2 ? entry2 : entry1;
                if (currentEntry is null || !currentEntry.Exists)
                    return;
                FilesystemTools.RemoveFileOrDir(currentEntry.FilePath);
                if (CurrentPane == 2)
                    refreshSecondPaneListing = true;
                else
                    refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTREMOVE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
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

            try
            {
                var currentEntry = CurrentPane == 2 ? entry2 : entry1;
                if (currentEntry is null || !currentEntry.Exists)
                    return;
                string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_TARGETPATHCOPY"), Settings.InfoBoxSettings);
                path = FilesystemTools.NeutralizePath(path, CurrentPane == 2 ? secondPanePath : firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (FilesystemTools.FolderExists(path))
                {
                    if (FilesystemTools.TryParsePath(path))
                    {
                        FilesystemTools.CopyFileOrDir(currentEntry.FilePath, path);
                        if (CurrentPane == 2)
                            refreshFirstPaneListing = true;
                        else
                            refreshSecondPaneListing = true;
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INVALIDPATH"), Settings.InfoBoxSettings);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILENOTFOUND"), Settings.InfoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTCOPY") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MoveTo(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                if (entry1 is null || !entry1.Exists)
                    return;
                string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_TARGETPATHMOVE"), Settings.InfoBoxSettings);
                path = FilesystemTools.NeutralizePath(path, CurrentPane == 2 ? secondPanePath : firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (FilesystemTools.FolderExists(path))
                {
                    if (FilesystemTools.TryParsePath(path))
                    {
                        FilesystemTools.MoveFileOrDir(entry1.FilePath, path);
                        refreshSecondPaneListing = true;
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INVALIDPATH"), Settings.InfoBoxSettings);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILENOTFOUND"), Settings.InfoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTMOVE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void Rename(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                var currentEntry = CurrentPane == 2 ? entry2 : entry1;
                if (currentEntry is null || !currentEntry.Exists)
                    return;
                string filename = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NEWFILENAMEPROMPT"), Settings.InfoBoxSettings);
                DebugWriter.WriteDebug(DebugLevel.I, $"New filename is {filename}");
                if (!FilesystemTools.FileExists(filename))
                {
                    if (FilesystemTools.TryParseFileName(filename))
                    {
                        FilesystemTools.MoveFileOrDir(currentEntry.FilePath, Path.GetDirectoryName(currentEntry.FilePath) + $"/{filename}");
                        if (CurrentPane == 2)
                            refreshSecondPaneListing = true;
                        else
                            refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INVALIDFILENAME"), Settings.InfoBoxSettings);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILEEXISTS"), Settings.InfoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTMOVE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MakeDir()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NEWFOLDERNAMEPROMPT"), Settings.InfoBoxSettings);
            path = FilesystemTools.NeutralizePath(path, CurrentPane == 2 ? secondPanePath : firstPanePath);
            if (!FilesystemTools.FolderExists(path))
            {
                FilesystemTools.TryMakeDirectory(path);
                if (CurrentPane == 2)
                    refreshSecondPaneListing = true;
                else
                    refreshFirstPaneListing = true;
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FOLDEREXISTS"), Settings.InfoBoxSettings);
        }

        internal void Hash(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // First, check to see if it's a file
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;
            if (!FilesystemTools.FileExists(currentEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTAFILE"), Settings.InfoBoxSettings);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERPROMPT") + $" {string.Join(", ", hashDrivers)}", Settings.InfoBoxSettings);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentEntry.FilePath, hashDriver);
            else
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERNOTFOUND"), Settings.InfoBoxSettings);
                return;
            }
            InfoBoxModalColor.WriteInfoBoxModal(hash, Settings.InfoBoxSettings);
        }

        internal void Verify(FileSystemEntry? entry1, FileSystemEntry? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // First, check to see if it's a file
            var currentEntry = CurrentPane == 2 ? entry2 : entry1;
            if (currentEntry is null || !currentEntry.Exists)
                return;
            if (!FilesystemTools.FileExists(currentEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTAFILE"), Settings.InfoBoxSettings);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERPROMPT") + $" {string.Join(", ", hashDrivers)}", Settings.InfoBoxSettings);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentEntry.FilePath, hashDriver);
            else
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERNOTFOUND"), Settings.InfoBoxSettings);
                return;
            }

            // Now, let the user write the expected hash
            string expectedHash = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_EXPECTEDHASHPROMPT"), Settings.InfoBoxSettings);
            if (expectedHash == hash)
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHESMATCH"), Settings.InfoBoxSettings);
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHESNOMATCH"), Settings.InfoBoxSettings);
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
            if (!FilesystemTools.FileExists(currentEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTAFILE"), Settings.InfoBoxSettings);
                return;
            }

            // Render the preview box
            string preview = FilesystemTools.RenderContents(currentEntry.FilePath, false);
            string filtered = FilesystemTools.IsBinaryFile(currentEntry.FilePath) ? preview : VtSequenceTools.FilterVTSequences(preview);
            InfoBoxModalColor.WriteInfoBoxModal(filtered, Settings.InfoBoxSettings);
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
            ShowUnixPermissionChangeInfoBox(currentEntry, Settings.InfoBoxSettings);
        }

        internal static void ShowUnixPermissionChangeInfoBox(FileSystemEntry fileSystemEntry, InfoBoxSettings infoBoxSettings)
        {
            try
            {
                // Input choices for permissions
                InputChoiceCategoryInfo[] permissionsInputs = [new("", [new("", [
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_READ_NAME -> Read
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_READ_TITLE -> File can be read or directory can be queried
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_READ_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_READ_TITLE")),

                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_WRITE_NAME -> Write
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_WRITE_TITLE -> File can be write or directory can be modified
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_WRITE_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_WRITE_TITLE")),

                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_EXECUTE_NAME -> Execute
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_EXECUTE_TITLE -> File can be execute or directory can be traversed
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_EXECUTE_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_EXECUTE_TITLE")),
                ])])];
                
                // Input choices for special permissions
                InputChoiceCategoryInfo[] specialPermissionsInputs = [new("", [new("", [
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETUID_NAME -> Setuid
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETUID_TITLE -> File can be executed with the privileges of the file owner, usually a root user
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETUID_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETUID_TITLE")),

                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETGID_NAME -> Setgid
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETGID_TITLE -> File can be executed with the privileges of the file group, usually an administrative group
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETGID_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETGID_TITLE")),

                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_STICKY_NAME -> Sticky
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_STICKY_TITLE -> Files inside a directory with this bit set can only be manipulated with by file owner, directory owner, and a root user
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_STICKY_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_STICKY_TITLE")),
                ])])];

                // Get current permissions
                var permissionDescriptors = fileSystemEntry.UnixPermissions;
                var specialPermissions = fileSystemEntry.UnixSpecial;

                // Get current user permissions
                List<int> userPerms = [];
                if (permissionDescriptors[0].Types.HasFlag(UnixPermissionType.Read))
                    userPerms.Add(0);
                if (permissionDescriptors[0].Types.HasFlag(UnixPermissionType.Write))
                    userPerms.Add(1);
                if (permissionDescriptors[0].Types.HasFlag(UnixPermissionType.Execute))
                    userPerms.Add(2);

                // Get current group permissions
                List<int> groupPerms = [];
                if (permissionDescriptors[1].Types.HasFlag(UnixPermissionType.Read))
                    groupPerms.Add(0);
                if (permissionDescriptors[1].Types.HasFlag(UnixPermissionType.Write))
                    groupPerms.Add(1);
                if (permissionDescriptors[1].Types.HasFlag(UnixPermissionType.Execute))
                    groupPerms.Add(2);

                // Get current other permissions
                List<int> otherPerms = [];
                if (permissionDescriptors[2].Types.HasFlag(UnixPermissionType.Read))
                    otherPerms.Add(0);
                if (permissionDescriptors[2].Types.HasFlag(UnixPermissionType.Write))
                    otherPerms.Add(1);
                if (permissionDescriptors[2].Types.HasFlag(UnixPermissionType.Execute))
                    otherPerms.Add(2);

                // Get current special permissions
                List<int> specialPerms = [];
                if (specialPermissions.HasFlag(UnixPermissionSpecial.SetUid))
                    specialPerms.Add(0);
                if (specialPermissions.HasFlag(UnixPermissionSpecial.SetGid))
                    specialPerms.Add(1);
                if (specialPermissions.HasFlag(UnixPermissionSpecial.Sticky))
                    specialPerms.Add(2);

                // Permission change input modules
                InputModule[] modules =
                [
                    new MultiComboBoxModule()
                    {
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_USERPERMS_NAME -> User permissions
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_USERPERMS_DESC -> You can set file permissions for the owner here
                        Name = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_USERPERMS_NAME"),
                        Description = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_USERPERMS_DESC"),
                        Choices = permissionsInputs,
                        Value = userPerms.ToArray(),
                    },
                    new MultiComboBoxModule()
                    {
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_GROUPPERMS_NAME -> Group permissions
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_GROUPPERMS_DESC -> You can set file permissions for the group here
                        Name = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_GROUPPERMS_NAME"),
                        Description = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_GROUPPERMS_DESC"),
                        Choices = permissionsInputs,
                        Value = groupPerms.ToArray(),
                    },
                    new MultiComboBoxModule()
                    {
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_OTHERPERMS_NAME -> Other permissions
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_OTHERPERMS_DESC -> You can set file permissions for others here
                        Name = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_OTHERPERMS_NAME"),
                        Description = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_OTHERPERMS_DESC"),
                        Choices = permissionsInputs,
                        Value = otherPerms.ToArray(),
                    },
                    new MultiComboBoxModule()
                    {
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIALPERMS_NAME -> Special permissions
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIALPERMS_DESC -> You can set special permissions for files or directories
                        Name = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIALPERMS_NAME"),
                        Description = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIALPERMS_DESC"),
                        Choices = specialPermissionsInputs,
                        Value = specialPerms.ToArray(),
                    },
                ];

                // Open an infobox
                // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERMSINFOBOX_NAME -> You can change the permissions for this file or directory here.
                bool done = InfoBoxMultiInputColor.WriteInfoBoxMultiInput(modules, LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERMSINFOBOX_NAME") + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", infoBoxSettings);
                if (!done)
                    return;

                // Grab the changed permissions
                var finalUserPerms = modules[0].GetValue<int[]>() ?? [];
                var finalGroupPerms = modules[1].GetValue<int[]>() ?? [];
                var finalOtherPerms = modules[2].GetValue<int[]>() ?? [];
                var finalSpecialPerms = modules[3].GetValue<int[]>() ?? [];

                // Convert them to actual enum values
                var typeUser = GetUnixPermissionTypeFromArray(finalUserPerms);
                var typeGroup = GetUnixPermissionTypeFromArray(finalGroupPerms);
                var typeOther = GetUnixPermissionTypeFromArray(finalOtherPerms);
                var typeSpecial = GetUnixPermissionSpecialFromArray(finalSpecialPerms);

                // Update descriptors
                permissionDescriptors[0].Types = typeUser;
                permissionDescriptors[1].Types = typeGroup;
                permissionDescriptors[2].Types = typeOther;

                // Set permissions
                FilesystemTools.SetUnixFileMode(fileSystemEntry.FilePath, permissionDescriptors, typeSpecial);
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERMSSETFAILURE") + $": {ex.Message}", infoBoxSettings);
            }
        }

        private static UnixPermissionType GetUnixPermissionTypeFromArray(int[] indexes)
        {
            var permissionsEnum = Enum.GetValues<UnixPermissionType>();
            var type = UnixPermissionType.None;
            foreach (var i in indexes)
                type |= permissionsEnum[i + 1];
            return type;
        }

        private static UnixPermissionSpecial GetUnixPermissionSpecialFromArray(int[] indexes)
        {
            var permissionsEnum = Enum.GetValues<UnixPermissionSpecial>();
            var type = UnixPermissionSpecial.None;
            foreach (var i in indexes)
                type |= permissionsEnum[i + 1];
            return type;
        }
    }
}
