﻿//
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
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files;
using Nitrocid.Drivers;
using Nitrocid.Misc.Reflection;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.Drivers.Encryption;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Files.Paths;
using Nitrocid.Files.Instances;
using Nitrocid.Files.Extensions;
using Textify.General;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Misc.Interactives
{
    /// <summary>
    /// File selector class, a descendant of the file manager
    /// </summary>
    public class FileSelectorCli : BaseInteractiveTui<FileSystemEntry>, IInteractiveTui<FileSystemEntry>
    {
        internal string selectedFile = "";
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
            // Check to see if we're given the file system info
            if (item == null)
                return Translate.DoTranslation("No info.");

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = item.Type == FileSystemEntryType.Directory;
                string status = $"[{(infoIsDirectory ? "/" : "*")}] {item.BaseEntry.FullName}";
                if (!string.IsNullOrEmpty(selectedFile))
                    status = $"{Translate.DoTranslation("Selected")} {selectedFile} - {status}";
                return status;
            }
            catch (Exception ex)
            {
                return Translate.DoTranslation(ex.Message);
            }
        }

        /// <inheritdoc/>
        public override string GetInfoFromItem(FileSystemEntry? item)
        {
            try
            {
                if (item is null)
                    return "";
                bool isDirectory = item.Type == FileSystemEntryType.Directory;
                bool isSelected = SelectedFile == item.FilePath;
                var size = item.FileSize;
                var path = item.FilePath;
                string finalRenderedName = Translate.DoTranslation("File name") + $": {Path.GetFileName(item.FilePath)}";
                string finalRenderedDir = Translate.DoTranslation("Is a directory") + $": {isDirectory}";
                string finalRenderedSelected = Translate.DoTranslation("Is selected") + $": {isSelected}";
                string finalRenderedSize = Translate.DoTranslation("File size") + $": {size.SizeString()}";
                string finalRenderedPath = Translate.DoTranslation("File path") + $": {path}";
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
                if (item is null)
                    return "";
                bool isDirectory = item.Type == FileSystemEntryType.Directory;
                bool isSelected = SelectedFile == item.FilePath;
                if (Config.MainConfig.IfmShowFileSize)
                    return
                        // Name and directory indicator
                        $"[{(isDirectory ? "/" : "*")}] [{(isSelected ? "+" : " ")}] {item.BaseEntry.Name} | " +

                        // File size or directory size
                        $"{(!isDirectory ? ((FileInfo)item.BaseEntry).Length.SizeString() : FilesystemTools.GetAllSizesInFolder((DirectoryInfo)item.BaseEntry).SizeString())}"
                    ;
                else
                    return $"[{(isDirectory ? "/" : "*")}] [{(isSelected ? "+" : " ")}] {item.BaseEntry.Name}";
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get entry: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        /// <summary>
        /// Selected file. If not selected yet and bailed earlier, this string is empty.
        /// </summary>
        public string SelectedFile =>
            selectedFile;

        internal void SelectOrGoTo(FileSystemEntry? currentFileSystemEntry)
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
                else if (currentFileSystemEntry.Type == FileSystemEntryType.File)
                {
                    // We're dealing with a file. Clear the screen and open the appropriate editor.
                    selectedFile = currentFileSystemEntry.FilePath;
                    InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Selected '{0}'. Press ESC to exit and confirm your selection."), Settings.InfoBoxSettings, selectedFile);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't open folder or select file") + TextTools.FormatString(": {0}", ex.Message));
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
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Name: {0}"), DirInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Full name: {0}"), FilesystemTools.NeutralizePath(DirInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Size: {0}"), FilesystemTools.GetAllSizesInFolder(DirInfo).SizeString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Creation time: {0}"), TimeDateRenderers.Render(DirInfo.CreationTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last access time: {0}"), TimeDateRenderers.Render(DirInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last write time: {0}"), TimeDateRenderers.Render(DirInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Attributes: {0}"), DirInfo.Attributes));
                    if (DirInfo.Parent is not null)
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Parent directory: {0}"), FilesystemTools.NeutralizePath(DirInfo.Parent.FullName)));
                }
                else
                {
                    // The file system info instance points to a file
                    FileInfo fileInfo = new(fullPath);
                    bool isBinary = FilesystemTools.IsBinaryFile(fileInfo.FullName);
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Name: {0}"), fileInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Full name: {0}"), FilesystemTools.NeutralizePath(fileInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("File size: {0}"), fileInfo.Length.SizeString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Creation time: {0}"), TimeDateRenderers.Render(fileInfo.CreationTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last access time: {0}"), TimeDateRenderers.Render(fileInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Last write time: {0}"), TimeDateRenderers.Render(fileInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Attributes: {0}"), fileInfo.Attributes));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Where to find: {0}"), FilesystemTools.NeutralizePath(fileInfo.DirectoryName)));
                    if (!isBinary)
                    {
                        var Style = FilesystemTools.GetLineEndingFromFile(fullPath);
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Newline style:") + " {0}", Style.ToString()));
                    }
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Binary file:") + " {0}", isBinary));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("MIME metadata:") + " {0}\n", MimeTypes.GetMimeType(fileInfo.Extension)));

                    // .NET managed info
                    if (ReflectionCommon.IsDotnetAssemblyFile(fullPath, out AssemblyName? asmName) && asmName is not null)
                    {
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Name: {0}"), asmName.Name));
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Full name") + ": {0}", asmName.FullName));
                        if (asmName.Version is not null)
                            finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Version") + ": {0}", asmName.Version.ToString()));
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Culture name") + ": {0}", asmName.CultureName));
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Content type") + ": {0}\n", asmName.ContentType.ToString()));
                    }
                    else
                    {
                        finalInfoRendered.AppendLine(Translate.DoTranslation("File is not a valid .NET assembly.\n"));
                    }

                    // Other info handled by the extension handler
                    if (ExtensionHandlerTools.IsHandlerRegistered(fileInfo.Extension))
                    {
                        var handler = ExtensionHandlerTools.GetExtensionHandler(fileInfo.Extension) ??
                            throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Handler is registered but somehow failed to get an extension handler for") + $" {fileInfo.Extension}");
                        finalInfoRendered.AppendLine(handler.InfoHandler(fullPath));
                    }
                }

                // Now, render the info box
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't get file system info") + TextTools.FormatString(": {0}", ex.Message));
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
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't remove file or directory") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void GoTo()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path or a full path to a local folder."), Settings.InfoBoxSettings);
            path = FilesystemTools.NeutralizePath(path, firstPanePath);
            if (FilesystemTools.FolderExists(path))
            {
                InteractiveTuiTools.SelectionMovement(this, 1);
                firstPanePath = path;
                refreshFirstPaneListing = true;
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Folder doesn't exist. Make sure that you've written the correct path."), Settings.InfoBoxSettings);
        }

        internal void CopyTo(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path or a full path to a destination folder to copy the selected file to."), Settings.InfoBoxSettings);
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
                        InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Make sure that you've written the correct path."), Settings.InfoBoxSettings);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), Settings.InfoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't copy file or directory") + TextTools.FormatString(": {0}", ex.Message));
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
                string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a path or a full path to a destination folder to move the selected file to."), Settings.InfoBoxSettings);
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
                        InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Make sure that you've written the correct path."), Settings.InfoBoxSettings);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), Settings.InfoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
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
                string filename = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a new file name."), Settings.InfoBoxSettings);
                DebugWriter.WriteDebug(DebugLevel.I, $"New filename is {filename}");
                if (!FilesystemTools.FileExists(filename))
                {
                    if (FilesystemTools.TryParseFileName(filename))
                    {
                        FilesystemTools.MoveFileOrDir(currentFileSystemEntry.FilePath, Path.GetDirectoryName(currentFileSystemEntry.FilePath) + $"/{filename}");
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Make sure that you've written the correct file name."), Settings.InfoBoxSettings);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("File already exists. The name shouldn't be occupied by another file."), Settings.InfoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MakeDir()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a new directory name."), Settings.InfoBoxSettings);
            path = FilesystemTools.NeutralizePath(path, firstPanePath);
            if (!FilesystemTools.FolderExists(path))
            {
                FilesystemTools.TryMakeDirectory(path);
                refreshFirstPaneListing = true;
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Folder already exists. The name shouldn't be occupied by another folder."), Settings.InfoBoxSettings);
        }

        internal void Hash(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!FilesystemTools.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Selected entry is not a file."), Settings.InfoBoxSettings);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a hash driver:") + $" {string.Join(", ", hashDrivers)}", Settings.InfoBoxSettings);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, hashDriver);
            else
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Hash driver not found."), Settings.InfoBoxSettings);
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
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Selected entry is not a file."), Settings.InfoBoxSettings);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter a hash driver:") + $" {string.Join(", ", hashDrivers)}", Settings.InfoBoxSettings);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, hashDriver);
            else
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Hash driver not found."), Settings.InfoBoxSettings);
                return;
            }

            // Now, let the user write the expected hash
            string expectedHash = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Enter your expected hash"), Settings.InfoBoxSettings);
            if (expectedHash == hash)
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Two hashes match!"), Settings.InfoBoxSettings);
            else
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Two hashes don't match."), Settings.InfoBoxSettings);
        }

        internal void Preview(FileSystemEntry? currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!FilesystemTools.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Selected entry is not a file."), Settings.InfoBoxSettings);
                return;
            }

            // Render the preview box
            string preview = FilesystemTools.RenderContents(currentFileSystemEntry.FilePath);
            string filtered = VtSequenceTools.FilterVTSequences(preview);
            InfoBoxModalColor.WriteInfoBoxModal(filtered, Settings.InfoBoxSettings);
        }
    }
}
