//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using System.Collections;
using System.Reflection;
using Textify.Sequences.Tools;
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

namespace Nitrocid.Misc.Interactives
{
    /// <summary>
    /// Folders selector class, a descendant of the file manager
    /// </summary>
    public class FoldersSelectorCli : BaseInteractiveTui, IInteractiveTui
    {
        internal List<string> selectedFolders = [];
        internal string firstPanePath = PathsManagement.HomePath;
        internal bool refreshFirstPaneListing = true;
        private List<FileSystemEntry> firstPaneListing = [];

        /// <summary>
        /// File Selector bindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } =
        [
            // Operations
            new InteractiveTuiBinding("Open", ConsoleKey.Enter,
                (info, _) => Open((FileSystemEntry)info)),
            new InteractiveTuiBinding("Select", ConsoleKey.Spacebar,
                (info, _) => Select((FileSystemEntry)info)),
            new InteractiveTuiBinding("Copy", ConsoleKey.F1,
                (info, _) => CopyTo((FileSystemEntry)info)),
            new InteractiveTuiBinding("Move", ConsoleKey.F2,
                (info, _) => MoveTo((FileSystemEntry)info)),
            new InteractiveTuiBinding("Delete", ConsoleKey.F3,
                (info, _) => RemoveFileOrDir((FileSystemEntry)info)),
            new InteractiveTuiBinding("Up", ConsoleKey.F4,
                (_, _) => GoUp()),
            new InteractiveTuiBinding("Info", ConsoleKey.F5,
                (info, _) => PrintFileSystemEntry((FileSystemEntry)info)),
            new InteractiveTuiBinding("Go To", ConsoleKey.F6,
                (_, _) => GoTo()),
            new InteractiveTuiBinding("Rename", ConsoleKey.F7,
                (info, _) => Rename((FileSystemEntry)info)),
            new InteractiveTuiBinding("New Folder", ConsoleKey.F8,
                (_, _) => MakeDir()),
            new InteractiveTuiBinding("Hash...", ConsoleKey.F9,
                (info, _) => Hash((FileSystemEntry)info)),
            new InteractiveTuiBinding("Verify...", ConsoleKey.F10,
                (info, _) => Verify((FileSystemEntry)info)),
            new InteractiveTuiBinding("Preview Selected", ConsoleKey.S,
                (_, _) => PreviewSelected()),
            new InteractiveTuiBinding("Preview", ConsoleKey.P,
                (info, _) => Preview((FileSystemEntry)info)),
        ];

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource
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
                    return new List<FileSystemEntry>();
                }
            }
        }

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override void RenderStatus(object item)
        {
            FileSystemEntry FileInfoCurrentPane = (FileSystemEntry)item;

            // Check to see if we're given the file system info
            if (FileInfoCurrentPane == null)
            {
                InteractiveTuiStatus.Status = Translate.DoTranslation("No info.");
                return;
            }

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = FileInfoCurrentPane.Type == FileSystemEntryType.Directory;
                if (Config.MainConfig.IfmShowFileSize)
                    InteractiveTuiStatus.Status =
                        // Name and directory indicator
                        $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.BaseEntry.Name} | " +

                        // File size or directory size
                        $"{(!infoIsDirectory ? ((FileInfo)FileInfoCurrentPane.BaseEntry).Length.SizeString() : SizeGetter.GetAllSizesInFolder((DirectoryInfo)FileInfoCurrentPane.BaseEntry).SizeString())} | " +

                        // Modified date
                        $"{(!infoIsDirectory ? TimeDateRenderers.Render(((FileInfo)FileInfoCurrentPane.BaseEntry).LastWriteTime) : "")}"
                    ;
                else
                    InteractiveTuiStatus.Status = $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.BaseEntry.Name}";
            }
            catch (Exception ex)
            {
                InteractiveTuiStatus.Status = Translate.DoTranslation(ex.Message);
            }
            if (selectedFolders.Count > 0)
                InteractiveTuiStatus.Status = $"{Translate.DoTranslation("Selected")}: {selectedFolders.Count} - {Translate.DoTranslation("Press SPACE for more info")} - {InteractiveTuiStatus.Status}";
        }

        /// <inheritdoc/>
        public override string GetInfoFromItem(object item)
        {
            try
            {
                FileSystemEntry file = (FileSystemEntry)item;
                bool isDirectory = file.Type == FileSystemEntryType.Directory;
                bool isSelected = SelectedFolders.Contains(file.FilePath);
                var size = file.FileSize;
                var path = file.FilePath;
                string finalRenderedName = Translate.DoTranslation("File name") + $": {Path.GetFileName(file.FilePath)}";
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
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get file entry: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            try
            {
                FileSystemEntry file = (FileSystemEntry)item;
                bool isDirectory = file.Type == FileSystemEntryType.Directory;
                bool isSelected = SelectedFolders.Contains(file.FilePath);
                return $" [{(isDirectory ? "/" : "*")}] [{(isSelected ? "+" : " ")}] {file.BaseEntry.Name}";
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get entry: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        /// <summary>
        /// Selected files. If not selected yet and bailed earlier, this list is empty.
        /// </summary>
        public string[] SelectedFolders =>
            [.. selectedFolders];

        private static void Select(FileSystemEntry currentFileSystemEntry)
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
                    if (!((FoldersSelectorCli)Instance).selectedFolders.Remove(currentFileSystemEntry.FilePath))
                    {
                        ((FoldersSelectorCli)Instance).selectedFolders.Add(currentFileSystemEntry.FilePath);
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected '{0}'. Press ESC to exit and confirm selections."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor, currentFileSystemEntry.FilePath);
                    }
                    else
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Unselected '{0}'. Press ESC to exit and confirm selections."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor, currentFileSystemEntry.FilePath);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't select folder") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
        }

        private static void Open(FileSystemEntry currentFileSystemEntry)
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
                    ((FoldersSelectorCli)Instance).firstPanePath = FilesystemTools.NeutralizePath(currentFileSystemEntry.FilePath + "/");
                    InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
                    ((FoldersSelectorCli)Instance).refreshFirstPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't open folder") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
        }

        private static void GoUp()
        {
            ((FoldersSelectorCli)Instance).firstPanePath = FilesystemTools.NeutralizePath(((FoldersSelectorCli)Instance).firstPanePath + "/..");
            InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
            ((FoldersSelectorCli)Instance).refreshFirstPaneListing = true;
        }

        private static void PrintFileSystemEntry(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // Render the final information string
            try
            {
                var finalInfoRendered = new StringBuilder();
                string fullPath = currentFileSystemEntry.FilePath;
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
                    finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("MIME metadata:") + " {0}\n", MimeTypes.GetMimeType(fileInfo.Extension)));

                    // .NET managed info
                    if (ReflectionCommon.IsDotnetAssemblyFile(fullPath, out AssemblyName asmName))
                    {
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Name: {0}"), asmName.Name));
                        finalInfoRendered.AppendLine(TextTools.FormatString(Translate.DoTranslation("Full name") + ": {0}", asmName.FullName));
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
                        var handler = ExtensionHandlerTools.GetExtensionHandler(fileInfo.Extension);
                        finalInfoRendered.AppendLine(handler.InfoHandler(fullPath));
                    }
                }
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));

                // Now, render the info box
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't get file system info") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
        }

        private static void RemoveFileOrDir(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                Removing.RemoveFileOrDir(currentFileSystemEntry.FilePath);
                ((FoldersSelectorCli)Instance).refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't remove file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
        }

        private static void GoTo()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a local folder."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            path = FilesystemTools.NeutralizePath(path, ((FoldersSelectorCli)Instance).firstPanePath);
            if (Checking.FolderExists(path))
            {
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
                ((FoldersSelectorCli)Instance).firstPanePath = path;
                ((FoldersSelectorCli)Instance).refreshFirstPaneListing = true;
            }
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Folder doesn't exist. Make sure that you've written the correct path."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
        }

        private static void CopyTo(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a destination folder to copy the selected file to."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                path = FilesystemTools.NeutralizePath(path, ((FoldersSelectorCli)Instance).firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                    {
                        Copying.CopyFileOrDir(currentFileSystemEntry.FilePath, path);
                        ((FoldersSelectorCli)Instance).refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Make sure that you've written the correct path."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't copy file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
        }

        private static void MoveTo(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a destination folder to move the selected file to."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                path = FilesystemTools.NeutralizePath(path, ((FoldersSelectorCli)Instance).firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                    {
                        Moving.MoveFileOrDir(currentFileSystemEntry.FilePath, path);
                        ((FoldersSelectorCli)Instance).refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Make sure that you've written the correct path."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
        }

        private static void Rename(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string filename = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a new file name."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                DebugWriter.WriteDebug(DebugLevel.I, $"New filename is {filename}");
                if (!Checking.FileExists(filename))
                {
                    if (Parsing.TryParseFileName(filename))
                    {
                        Moving.MoveFileOrDir(currentFileSystemEntry.FilePath, Path.GetDirectoryName(currentFileSystemEntry.FilePath) + $"/{filename}");
                        ((FoldersSelectorCli)Instance).refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Make sure that you've written the correct file name."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("File already exists. The name shouldn't be occupied by another file."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
        }

        private static void MakeDir()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a new directory name."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            path = FilesystemTools.NeutralizePath(path, ((FoldersSelectorCli)Instance).firstPanePath);
            if (!Checking.FolderExists(path))
            {
                Making.TryMakeDirectory(path);
                ((FoldersSelectorCli)Instance).refreshFirstPaneListing = true;
            }
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Folder already exists. The name shouldn't be occupied by another folder."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
        }

        private static void Hash(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!Checking.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected entry is not a file."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a hash driver:") + $" {string.Join(", ", hashDrivers)}", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, hashDriver);
            else
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Hash driver not found."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                return;
            }
            InfoBoxColor.WriteInfoBoxColorBack(hash, InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
        }

        private static void Verify(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!Checking.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected entry is not a file."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a hash driver:") + $" {string.Join(", ", hashDrivers)}", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, hashDriver);
            else
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Hash driver not found."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                return;
            }

            // Now, let the user write the expected hash
            string expectedHash = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter your expected hash"), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            if (expectedHash == hash)
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Two hashes match!"), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Two hashes don't match."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
        }

        private static void Preview(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            // First, check to see if it's a file
            if (!Checking.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected entry is not a file."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                return;
            }

            // Render the preview box
            string preview = FileContentPrinter.RenderContents(currentFileSystemEntry.FilePath);
            string filtered = VtSequenceTools.FilterVTSequences(preview);
            InfoBoxColor.WriteInfoBoxColorBack(filtered, InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
        }

        private static void PreviewSelected()
        {
            string selected =
                ((FoldersSelectorCli)Instance).SelectedFolders.Length > 0 ?
                $"  - {string.Join("\n  - ", ((FoldersSelectorCli)Instance).SelectedFolders)}" :
                Translate.DoTranslation("No selected folders.");
            InfoBoxColor.WriteInfoBoxColorBack(selected, InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
        }
    }
}
