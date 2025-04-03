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
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files;
using Nitrocid.Drivers;
using Nitrocid.Misc.Reflection;
using Nitrocid.Files.Operations;
using Nitrocid.Files.Folders;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.Files.Operations.Printing;
using Nitrocid.Drivers.Encryption;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Files.Paths;
using Nitrocid.Files.Instances;
using Nitrocid.Files.LineEndings;
using Nitrocid.Files.Extensions;
using Nitrocid.Files.Operations.Querying;
using Textify.General;
using Magico.Files;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Misc.Interactives
{
    /// <summary>
    /// File manager class relating to the interactive file manager planned back in 2018
    /// </summary>
    public class FileManagerCli : BaseInteractiveTui<FileSystemEntry>, IInteractiveTui<FileSystemEntry>
    {
        internal string firstPanePath = PathsManagement.HomePath;
        internal string secondPanePath = PathsManagement.HomePath;
        internal bool refreshFirstPaneListing = true;
        internal bool refreshSecondPaneListing = true;
        private List<FileSystemEntry> firstPaneListing = [];
        private List<FileSystemEntry> secondPaneListing = [];

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
                        firstPaneListing = Listing.CreateList(firstPanePath, true);
                    }
                    return firstPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get current directory list for the first pane [{0}]: {1}", firstPanePath, ex.Message);
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
                        secondPaneListing = Listing.CreateList(secondPanePath, true);
                    }
                    return secondPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get current directory list for the second pane [{0}]: {1}", secondPanePath, ex.Message);
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
                return Translate.DoTranslation("No info.");

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = FileInfoCurrentPane.Type == FileSystemEntryType.Directory;
                return $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.BaseEntry.FullName}";
            }
            catch (Exception ex)
            {
                return Translate.DoTranslation(ex.Message);
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
                        $"{(!isDirectory ? ((FileInfo)file.BaseEntry).Length.SizeString() : SizeGetter.GetAllSizesInFolder((DirectoryInfo)file.BaseEntry).SizeString())}"
                    ;
                else
                    return $"[{(isDirectory ? "/" : "*")}] {file.BaseEntry.Name}";
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get entry: {0}", ex.Message);
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
                    KernelColorTools.LoadBackground();
                    Opening.OpenDeterministically(currentEntry.FilePath);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't open file or folder") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
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
                if (Checking.FolderExists(fullPath))
                {
                    // The file system info instance points to a folder
                    var DirInfo = new DirectoryInfo(fullPath);
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Name: {0}"), DirInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Full name: {0}"), FilesystemTools.NeutralizePath(DirInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Size: {0}"), SizeGetter.GetAllSizesInFolder(DirInfo).SizeString()));
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
                    bool isBinary = Parsing.IsBinaryFile(fileInfo.FullName);
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
                        var Style = LineEndingsTools.GetLineEndingFromFile(fullPath);
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Newline style:") + " {0}", Style.ToString()));
                    }
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Binary file:") + " {0}", isBinary));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("MIME metadata:") + " {0}", MimeTypes.GetMimeType(fileInfo.Extension)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("MIME metadata (extended)") + ": {0}", MagicHandler.GetMagicMimeInfo(fileInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("File type") + ": {0}\n", MagicHandler.GetMagicInfo(fileInfo.FullName)));

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
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't get file system info") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
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
                Copying.CopyFileOrDir(currentEntry.FilePath, dest);
                if (CurrentPane == 2)
                    refreshFirstPaneListing = true;
                else
                    refreshSecondPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't copy file or directory") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
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
                Moving.MoveFileOrDir(currentEntry.FilePath, dest);
                refreshSecondPaneListing = true;
                refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
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
                Removing.RemoveFileOrDir(currentEntry.FilePath);
                if (CurrentPane == 2)
                    refreshSecondPaneListing = true;
                else
                    refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't remove file or directory") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void GoTo()
        {
            // Now, render the search box
            string root = CurrentPane == 2 ? secondPanePath : firstPanePath;
            string path = Selection.SelectFolder(root);
            path = FilesystemTools.NeutralizePath(path, root);
            if (Checking.FolderExists(path))
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
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Folder doesn't exist. Make sure that you've written the correct path."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
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
                string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a destination folder to copy the selected file to."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                path = FilesystemTools.NeutralizePath(path, CurrentPane == 2 ? secondPanePath : firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                    {
                        Copying.CopyFileOrDir(currentEntry.FilePath, path);
                        if (CurrentPane == 2)
                            refreshFirstPaneListing = true;
                        else
                            refreshSecondPaneListing = true;
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Make sure that you've written the correct path."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't copy file or directory") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
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
                string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a destination folder to move the selected file to."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                path = FilesystemTools.NeutralizePath(path, CurrentPane == 2 ? secondPanePath : firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                    {
                        Moving.MoveFileOrDir(entry1.FilePath, path);
                        refreshSecondPaneListing = true;
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Make sure that you've written the correct path."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
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
                string filename = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a new file name."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                DebugWriter.WriteDebug(DebugLevel.I, $"New filename is {filename}");
                if (!Checking.FileExists(filename))
                {
                    if (Parsing.TryParseFileName(filename))
                    {
                        Moving.MoveFileOrDir(currentEntry.FilePath, Path.GetDirectoryName(currentEntry.FilePath) + $"/{filename}");
                        if (CurrentPane == 2)
                            refreshSecondPaneListing = true;
                        else
                            refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Make sure that you've written the correct file name."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("File already exists. The name shouldn't be occupied by another file."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void MakeDir()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a new directory name."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            path = FilesystemTools.NeutralizePath(path, CurrentPane == 2 ? secondPanePath : firstPanePath);
            if (!Checking.FolderExists(path))
            {
                Making.TryMakeDirectory(path);
                if (CurrentPane == 2)
                    refreshSecondPaneListing = true;
                else
                    refreshFirstPaneListing = true;
            }
            else
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Folder already exists. The name shouldn't be occupied by another folder."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
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
            if (!Checking.FileExists(currentEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Selected entry is not a file."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a hash driver:") + $" {string.Join(", ", hashDrivers)}", Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentEntry.FilePath, hashDriver);
            else
            {
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Hash driver not found."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                return;
            }
            InfoBoxModalColor.WriteInfoBoxModalColorBack(hash, Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
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
            if (!Checking.FileExists(currentEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Selected entry is not a file."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a hash driver:") + $" {string.Join(", ", hashDrivers)}", Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentEntry.FilePath, hashDriver);
            else
            {
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Hash driver not found."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                return;
            }

            // Now, let the user write the expected hash
            string expectedHash = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter your expected hash"), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            if (expectedHash == hash)
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Two hashes match!"), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            else
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Two hashes don't match."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
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
            if (!Checking.FileExists(currentEntry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModalColorBack(Translate.DoTranslation("Selected entry is not a file."), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
                return;
            }

            // Render the preview box
            string preview = FileContentPrinter.RenderContents(currentEntry.FilePath);
            string filtered = VtSequenceTools.FilterVTSequences(preview);
            InfoBoxModalColor.WriteInfoBoxModalColorBack(filtered, Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
        }
    }
}
