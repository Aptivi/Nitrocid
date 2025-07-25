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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using Terminaux.Sequences;
using Nitrocid.Base.Files;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Textify.General;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.Instances;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Files.Extensions;
using Nitrocid.Base.Drivers.Encryption;

namespace Nitrocid.Base.Misc.Interactives
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
            FileSystemEntry? FileInfoCurrentPane = item;

            // Check to see if we're given the file system info
            if (FileInfoCurrentPane == null)
                return LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOINFO");

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = FileInfoCurrentPane.Type == FileSystemEntryType.Directory;
                string status = $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.BaseEntry.FullName}";
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
                FileSystemEntry? file = item;
                if (file is null)
                    return LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOINFO");
                bool isDirectory = file.Type == FileSystemEntryType.Directory;
                bool isSelected = SelectedFolders.Contains(file.FilePath);
                var size = file.FileSize;
                var path = file.FilePath;
                string finalRenderedName = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILENAME") + $": {Path.GetFileName(file.FilePath)}";
                string finalRenderedDir = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ISDIRECTORY") + $": {isDirectory}";
                string finalRenderedSelected = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ISSELECTED") + $": {isSelected}";
                string finalRenderedSize = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILESIZE") + $": {size.SizeString()}";
                string finalRenderedPath = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILEPATH") + $": {path}";
                return
                    finalRenderedName + CharManager.NewLine +
                    finalRenderedDir + CharManager.NewLine +
                    finalRenderedSelected + CharManager.NewLine +
                    finalRenderedSize + CharManager.NewLine +
                    finalRenderedPath
                ;
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
                FileSystemEntry? file = item;
                if (file is null)
                    return LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOINFO");
                bool isDirectory = file.Type == FileSystemEntryType.Directory;
                bool isSelected = SelectedFolders.Contains(file.FilePath);
                if (Config.MainConfig.IfmShowFileSize)
                    return
                        // Name and directory indicator
                        $"[{(isDirectory ? "/" : "*")}] [{(isSelected ? "+" : " ")}] {file.BaseEntry.Name} | " +

                        // File size or directory size
                        $"{(!isDirectory ? ((FileInfo)file.BaseEntry).Length.SizeString() : FilesystemTools.GetAllSizesInFolder((DirectoryInfo)file.BaseEntry).SizeString())}"
                    ;
                else
                    return $"[{(isDirectory ? "/" : "*")}] [{(isSelected ? "+" : " ")}] {file.BaseEntry.Name}";
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

            // Render the final information string
            try
            {
                var finalInfoRendered = new StringBuilder();
                string fullPath = currentFileSystemEntry.FilePath;
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
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_MIMEMETADATA") + " {0}\n", MimeTypes.GetMimeType(fileInfo.Extension)));

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

        internal void RemoveFileOrDir(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                FilesystemTools.RemoveFileOrDir(currentFileSystemEntry.FilePath);
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

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_TARGETPATHCOPY"), Settings.InfoBoxSettings);
                path = FilesystemTools.NeutralizePath(path, firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (FilesystemTools.FolderExists(path))
                {
                    if (FilesystemTools.TryParsePath(path))
                    {
                        FilesystemTools.CopyFileOrDir(currentFileSystemEntry.FilePath, path);
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
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTCOPY") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MoveTo(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_TARGETPATHMOVE"), Settings.InfoBoxSettings);
                path = FilesystemTools.NeutralizePath(path, firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (FilesystemTools.FolderExists(path))
                {
                    if (FilesystemTools.TryParsePath(path))
                    {
                        FilesystemTools.MoveFileOrDir(currentFileSystemEntry.FilePath, path);
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

        internal void Rename(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string filename = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NEWFILENAMEPROMPT"), Settings.InfoBoxSettings);
                DebugWriter.WriteDebug(DebugLevel.I, $"New filename is {filename}");
                if (!FilesystemTools.FileExists(filename))
                {
                    if (FilesystemTools.TryParseFileName(filename))
                    {
                        FilesystemTools.MoveFileOrDir(currentFileSystemEntry.FilePath, Path.GetDirectoryName(currentFileSystemEntry.FilePath) + $"/{filename}");
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
            path = FilesystemTools.NeutralizePath(path, firstPanePath);
            if (!FilesystemTools.FolderExists(path))
            {
                FilesystemTools.TryMakeDirectory(path);
                refreshFirstPaneListing = true;
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FOLDEREXISTS"), Settings.InfoBoxSettings);
        }

        internal void Hash(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!FilesystemTools.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTAFILE"), Settings.InfoBoxSettings);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERPROMPT") + $" {string.Join(", ", hashDrivers)}", Settings.InfoBoxSettings);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, hashDriver);
            else
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERNOTFOUND"), Settings.InfoBoxSettings);
                return;
            }
            InfoBoxModalColor.WriteInfoBoxModal(hash, Settings.InfoBoxSettings);
        }

        internal void Verify(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!FilesystemTools.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTAFILE"), Settings.InfoBoxSettings);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERPROMPT") + $" {string.Join(", ", hashDrivers)}", Settings.InfoBoxSettings);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, hashDriver);
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

        internal void Preview(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!FilesystemTools.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTAFILE"), Settings.InfoBoxSettings);
                return;
            }

            // Render the preview box
            string preview = FilesystemTools.RenderContents(currentFileSystemEntry.FilePath);
            string filtered = VtSequenceTools.FilterVTSequences(preview);
            InfoBoxModalColor.WriteInfoBoxModal(filtered, Settings.InfoBoxSettings);
        }

        internal void PreviewSelected()
        {
            string selected =
                SelectedFolders.Length > 0 ?
                $"  - {string.Join("\n  - ", SelectedFolders)}" :
                LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOSELECTIONSFOLDER");
            InfoBoxModalColor.WriteInfoBoxModal(selected, Settings.InfoBoxSettings);
        }
    }
}
