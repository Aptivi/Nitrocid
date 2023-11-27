//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.Drivers;
using KS.Drivers.Encryption;
using KS.Files.Folders;
using KS.Files.Operations;
using KS.Kernel.Debugging;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KS.Files.LineEndings;
using System.Text;
using System.Collections;
using KS.Misc.Text;
using KS.Kernel.Time.Renderers;
using KS.Kernel.Configuration;
using KS.Files;
using KS.Misc.Reflection;
using System.Reflection;
using KS.Files.Instances;
using KS.ConsoleBase.Interactive;
using KS.Files.Extensions;
using KS.Files.Operations.Querying;
using KS.Files.Operations.Printing;
using Terminaux.Sequences.Tools;
using KS.Files.Paths;
using KS.ConsoleBase.Inputs.Styles.Infobox;

namespace KS.Misc.Interactives
{
    /// <summary>
    /// Folder selector class, a descendant of the file manager
    /// </summary>
    public class FolderSelectorCli : BaseInteractiveTui, IInteractiveTui
    {
        internal string selectedFolder = "";
        internal string firstPanePath = PathsManagement.HomePath;
        private static bool refreshFirstPaneListing = true;
        private static List<FileSystemEntry> firstPaneListing = [];

        /// <summary>
        /// Folder Selector bindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } =
        [
            // Operations
            new InteractiveTuiBinding(/* Localizable */ "Open", ConsoleKey.Enter,
                (info, _) => Open((FileSystemEntry)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Select", ConsoleKey.Spacebar,
                (info, _) => Select((FileSystemEntry)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Copy", ConsoleKey.F1,
                (info, _) => CopyTo((FileSystemEntry)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Move", ConsoleKey.F2,
                (info, _) => MoveTo((FileSystemEntry)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Delete", ConsoleKey.F3,
                (info, _) => RemoveFileOrDir((FileSystemEntry)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Up", ConsoleKey.F4,
                (_, _) => GoUp(), true),
            new InteractiveTuiBinding(/* Localizable */ "Info", ConsoleKey.F5,
                (info, _) => PrintFileSystemEntry((FileSystemEntry)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Go To", ConsoleKey.F6,
                (_, _) => GoTo(), true),
            new InteractiveTuiBinding(/* Localizable */ "Rename", ConsoleKey.F7,
                (info, _) => Rename((FileSystemEntry)info), true),
            new InteractiveTuiBinding(/* Localizable */ "New Folder", ConsoleKey.F8,
                (_, _) => MakeDir(), true),
            new InteractiveTuiBinding(/* Localizable */ "Hash...", ConsoleKey.F9,
                (info, _) => Hash((FileSystemEntry)info)),
            new InteractiveTuiBinding(/* Localizable */ "Verify...", ConsoleKey.F10,
                (info, _) => Verify((FileSystemEntry)info)),
            new InteractiveTuiBinding(/* Localizable */ "Preview", ConsoleKey.P,
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
        public override void RenderStatus(object item)
        {
            FileSystemEntry FileInfoCurrentPane = (FileSystemEntry)item;

            // Check to see if we're given the file system info
            if (FileInfoCurrentPane == null)
            {
                Status = Translate.DoTranslation("No info.");
                return;
            }

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = FileInfoCurrentPane.Type == FileSystemEntryType.Directory;
                if (Config.MainConfig.IfmShowFileSize)
                    Status =
                        // Name and directory indicator
                        $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.BaseEntry.Name} | " +

                        // File size or directory size
                        $"{(!infoIsDirectory ? ((FileInfo)FileInfoCurrentPane.BaseEntry).Length.SizeString() : SizeGetter.GetAllSizesInFolder((DirectoryInfo)FileInfoCurrentPane.BaseEntry).SizeString())} | " +

                        // Modified date
                        $"{(!infoIsDirectory ? TimeDateRenderers.Render(((FileInfo)FileInfoCurrentPane.BaseEntry).LastWriteTime) : "")}"
                    ;
                else
                    Status = $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.BaseEntry.Name}";
            }
            catch (Exception ex)
            {
                Status = Translate.DoTranslation(ex.Message);
            }
            Status = $"{Translate.DoTranslation("Selected")} {selectedFolder} - {Status}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            try
            {
                FileSystemEntry file = (FileSystemEntry)item;
                bool isDirectory = file.Type == FileSystemEntryType.Directory;
                return $" [{(isDirectory ? "/" : "*")}] {file.BaseEntry.Name}";
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get entry: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        /// <summary>
        /// Selected folder. If not selected yet and bailed earlier, this string is empty.
        /// </summary>
        public static string SelectedFolder =>
            ((FolderSelectorCli)Instance).selectedFolder;

        private static void Open(FileSystemEntry currentFileSystemEntry)
        {
            try
            {
                if (!currentFileSystemEntry.Exists)
                    return;

                // Now that the selected folder exists, check the type.
                if (currentFileSystemEntry.Type == FileSystemEntryType.Directory)
                {
                    // We're dealing with a folder. Open it in the selected pane.
                    ((FolderSelectorCli)Instance).firstPanePath = FilesystemTools.NeutralizePath(currentFileSystemEntry.FilePath + "/");
                    FirstPaneCurrentSelection = 1;
                    refreshFirstPaneListing = true;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't open folder") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            }
        }

        private static void Select(FileSystemEntry currentFileSystemEntry)
        {
            try
            {
                if (!currentFileSystemEntry.Exists)
                    return;

                // Now that the selected folder exists, check the type.
                if (currentFileSystemEntry.Type == FileSystemEntryType.Directory)
                {
                    ((FolderSelectorCli)Instance).selectedFolder = currentFileSystemEntry.FilePath;
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected '{0}'. Press ESC to exit and confirm selection."), BoxForegroundColor, BoxBackgroundColor, ((FolderSelectorCli)Instance).selectedFolder);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't select folder") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            }
        }

        private static void GoUp()
        {
            ((FolderSelectorCli)Instance).firstPanePath = FilesystemTools.NeutralizePath(((FolderSelectorCli)Instance).firstPanePath + "/..");
            FirstPaneCurrentSelection = 1;
            refreshFirstPaneListing = true;
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
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't get file system info") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
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
                refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't remove file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            }
        }

        private static void GoTo()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a local folder."), BoxForegroundColor, BoxBackgroundColor);
            path = FilesystemTools.NeutralizePath(path, ((FolderSelectorCli)Instance).firstPanePath);
            if (Checking.FolderExists(path))
            {
                FirstPaneCurrentSelection = 1;
                ((FolderSelectorCli)Instance).firstPanePath = path;
                refreshFirstPaneListing = true;
            }
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Folder doesn't exist. Make sure that you've written the correct path."), BoxForegroundColor, BoxBackgroundColor);
        }

        private static void CopyTo(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a destination folder to copy the selected file to."), BoxForegroundColor, BoxBackgroundColor);
                path = FilesystemTools.NeutralizePath(path, ((FolderSelectorCli)Instance).firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                    {
                        Copying.CopyFileOrDir(currentFileSystemEntry.FilePath, path);
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Make sure that you've written the correct path."), BoxForegroundColor, BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), BoxForegroundColor, BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't copy file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            }
        }

        private static void MoveTo(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a path or a full path to a destination folder to move the selected file to."), BoxForegroundColor, BoxBackgroundColor);
                path = FilesystemTools.NeutralizePath(path, ((FolderSelectorCli)Instance).firstPanePath) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (Checking.FolderExists(path))
                {
                    if (Parsing.TryParsePath(path))
                    {
                        Moving.MoveFileOrDir(currentFileSystemEntry.FilePath, path);
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Make sure that you've written the correct path."), BoxForegroundColor, BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("File doesn't exist. Make sure that you've written the correct path."), BoxForegroundColor, BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            }
        }

        private static void Rename(FileSystemEntry currentFileSystemEntry)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemEntry is null)
                return;

            try
            {
                string filename = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a new file name."), BoxForegroundColor, BoxBackgroundColor);
                DebugWriter.WriteDebug(DebugLevel.I, $"New filename is {filename}");
                if (!Checking.FileExists(filename))
                {
                    if (Parsing.TryParseFileName(filename))
                    {
                        Moving.MoveFileOrDir(currentFileSystemEntry.FilePath, Path.GetDirectoryName(currentFileSystemEntry.FilePath) + $"/{filename}");
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Make sure that you've written the correct file name."), BoxForegroundColor, BoxBackgroundColor);
                }
                else
                    InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("File already exists. The name shouldn't be occupied by another file."), BoxForegroundColor, BoxBackgroundColor);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't move file or directory") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            }
        }

        private static void MakeDir()
        {
            // Now, render the search box
            string path = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a new directory name."), BoxForegroundColor, BoxBackgroundColor);
            path = FilesystemTools.NeutralizePath(path, ((FolderSelectorCli)Instance).firstPanePath);
            if (!Checking.FolderExists(path))
            {
                Making.TryMakeDirectory(path);
                refreshFirstPaneListing = true;
            }
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Folder already exists. The name shouldn't be occupied by another folder."), BoxForegroundColor, BoxBackgroundColor);
        }

        private static void Hash(FileSystemEntry currentFileSystemEntry)
        {
            // First, check to see if it's a file
            if (!Checking.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected entry is not a file."), BoxForegroundColor, BoxBackgroundColor);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a hash driver:") + $" {string.Join(", ", hashDrivers)}", BoxForegroundColor, BoxBackgroundColor);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, hashDriver);
            else
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Hash driver not found."), BoxForegroundColor, BoxBackgroundColor);
                return;
            }
            InfoBoxColor.WriteInfoBoxColorBack(hash, BoxForegroundColor, BoxBackgroundColor);
        }

        private static void Verify(FileSystemEntry currentFileSystemEntry)
        {
            // First, check to see if it's a file
            if (!Checking.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected entry is not a file."), BoxForegroundColor, BoxBackgroundColor);
                return;
            }

            // Render the hash box
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            string hashDriver = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter a hash driver:") + $" {string.Join(", ", hashDrivers)}", BoxForegroundColor, BoxBackgroundColor);
            string hash;
            if (string.IsNullOrEmpty(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, DriverHandler.CurrentEncryptionDriver.DriverName);
            else if (hashDrivers.Contains(hashDriver))
                hash = Encryption.GetEncryptedFile(currentFileSystemEntry.FilePath, hashDriver);
            else
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Hash driver not found."), BoxForegroundColor, BoxBackgroundColor);
                return;
            }

            // Now, let the user write the expected hash
            string expectedHash = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Enter your expected hash"), BoxForegroundColor, BoxBackgroundColor);
            if (expectedHash == hash)
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Two hashes match!"), BoxForegroundColor, BoxBackgroundColor);
            else
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Two hashes don't match."), BoxForegroundColor, BoxBackgroundColor);
        }

        private static void Preview(FileSystemEntry currentFileSystemEntry)
        {
            // First, check to see if it's a file
            if (!Checking.FileExists(currentFileSystemEntry.FilePath))
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Selected entry is not a file."), BoxForegroundColor, BoxBackgroundColor);
                return;
            }

            // Render the preview box
            string preview = FileContentPrinter.RenderContents(currentFileSystemEntry.FilePath);
            string filtered = VtSequenceTools.FilterVTSequences(preview);
            InfoBoxColor.WriteInfoBoxColorBack(filtered, BoxForegroundColor, BoxBackgroundColor);
        }
    }
}
