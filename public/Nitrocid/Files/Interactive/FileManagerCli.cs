
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using ColorSeq;
using Extensification.StringExts;
using FluentFTP.Helpers;
using KS.Files.Folders;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.FancyWriters;
using KS.TimeDate;
using System;
using System.Collections.Generic;
using System.IO;
using MimeKit;
using KS.Files.LineEndings;
using System.Text;
using KS.Misc.Interactive;
using System.Collections;

namespace KS.Files.Interactive
{
    /// <summary>
    /// File manager class relating to the interactive file manager planned back in 2018
    /// </summary>
    public class FileManagerCli : BaseInteractiveTui, IInteractiveTui
    {
        private static string firstPanePath = Paths.HomePath;
        private static string secondPanePath = Paths.HomePath;

        /// <summary>
        /// File manager bindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } = new()
        {
            // Operations
            new InteractiveTuiBinding(/* Localizable */ "Open",   ConsoleKey.Enter, (info, _) => Open((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Copy",   ConsoleKey.F1,    (info, _) => CopyFileOrDir((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Move",   ConsoleKey.F2,    (info, _) => MoveFileOrDir((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Delete", ConsoleKey.F3,    (info, _) => RemoveFileOrDir((FileSystemInfo)info), true),
            new InteractiveTuiBinding(/* Localizable */ "Up",     ConsoleKey.F4,    (_, _)    => GoUp(), true),
            new InteractiveTuiBinding(/* Localizable */ "Info",   ConsoleKey.F5,    (info, _) => PrintFileSystemInfo((FileSystemInfo)info), true),

            // Misc bindings
            new InteractiveTuiBinding(/* Localizable */ "Switch", ConsoleKey.Tab,   (_, _)    => Switch(), true),
        };

        /// <summary>
        /// File manager background color
        /// </summary>
        public static new Color BackgroundColor => FileManagerCliColors.FileManagerBackgroundColor;
        /// <summary>
        /// File manager foreground color
        /// </summary>
        public static new Color ForegroundColor => FileManagerCliColors.FileManagerForegroundColor;
        /// <summary>
        /// File manager pane background color
        /// </summary>
        public static new Color PaneBackgroundColor => FileManagerCliColors.FileManagerPaneBackgroundColor;
        /// <summary>
        /// File manager pane separator color
        /// </summary>
        public static new Color PaneSeparatorColor => FileManagerCliColors.FileManagerPaneSeparatorColor;
        /// <summary>
        /// File manager pane selected File color (foreground)
        /// </summary>
        public static new Color PaneSelectedItemForeColor => FileManagerCliColors.FileManagerPaneSelectedFileForeColor;
        /// <summary>
        /// File manager pane selected File color (background)
        /// </summary>
        public static new Color PaneSelectedItemBackColor => FileManagerCliColors.FileManagerPaneSelectedFileBackColor;
        /// <summary>
        /// File manager pane File color (foreground)
        /// </summary>
        public static new Color PaneItemForeColor => FileManagerCliColors.FileManagerPaneFileForeColor;
        /// <summary>
        /// File manager pane File color (background)
        /// </summary>
        public static new Color PaneItemBackColor => FileManagerCliColors.FileManagerPaneFileBackColor;
        /// <summary>
        /// File manager option background color
        /// </summary>
        public static new Color OptionBackgroundColor => FileManagerCliColors.FileManagerOptionBackgroundColor;
        /// <summary>
        /// File manager key binding in option color
        /// </summary>
        public static new Color KeyBindingOptionColor => FileManagerCliColors.FileManagerKeyBindingOptionColor;
        /// <summary>
        /// File manager option foreground color
        /// </summary>
        public static new Color OptionForegroundColor => FileManagerCliColors.FileManagerOptionForegroundColor;
        /// <summary>
        /// File manager box background color
        /// </summary>
        public static new Color BoxBackgroundColor => FileManagerCliColors.FileManagerBoxBackgroundColor;
        /// <summary>
        /// File manager box foreground color
        /// </summary>
        public static new Color BoxForegroundColor => FileManagerCliColors.FileManagerBoxForegroundColor;

        /// <summary>
        /// Always true in the file manager as we want it to behave like Total Commander
        /// </summary>
        public override bool SecondPaneInteractable =>
            true;
        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            Listing.CreateList(firstPanePath, true);
        /// <inheritdoc/>
        public override IEnumerable SecondaryDataSource =>
            Listing.CreateList(secondPanePath, true);

        /// <inheritdoc/>
        public override void RenderStatus(object item)
        {
            FileSystemInfo FileInfoCurrentPane = (FileSystemInfo)item;

            // Check to see if we're given the file system info
            if (FileInfoCurrentPane == null)
            {
                Status = Translate.DoTranslation("No info.");
                return;
            }

            // Now, populate the info to the status
            bool infoIsDirectory = Checking.FolderExists(FileInfoCurrentPane.FullName);
            Status =
                // Name and directory indicator
                $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.Name} | " +

                // File size or directory size
                $"{(!infoIsDirectory ? ((FileInfo)FileInfoCurrentPane).Length.FileSizeToString() : SizeGetter.GetAllSizesInFolder((DirectoryInfo)FileInfoCurrentPane).FileSizeToString())} | " +

                // Modified date
                $"{(!infoIsDirectory ? TimeDateRenderers.Render(((FileInfo)FileInfoCurrentPane).LastWriteTime) : "")}"
            ;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            FileSystemInfo file = (FileSystemInfo)item;
            bool isDirectory = Checking.FolderExists(file.FullName);
            return $" [{(isDirectory ? "/" : "*")}] {file.Name}";
        }

        private static void Open(FileSystemInfo currentFileSystemInfo)
        {
            if (!Checking.FolderExists(currentFileSystemInfo.FullName))
                return;
            if (CurrentPane == 2)
            {
                secondPanePath = Filesystem.NeutralizePath(currentFileSystemInfo.FullName);
                SecondPaneCurrentSelection = 1;
            }
            else
            {
                firstPanePath = Filesystem.NeutralizePath(currentFileSystemInfo.FullName);
                FirstPaneCurrentSelection = 1;
            }
        }

        private static void GoUp()
        {
            if (CurrentPane == 2)
            {
                secondPanePath = Filesystem.NeutralizePath(secondPanePath + "/..");
                SecondPaneCurrentSelection = 1;
            }
            else
            {
                firstPanePath = Filesystem.NeutralizePath(firstPanePath + "/..");
                FirstPaneCurrentSelection = 1;
            }
        }

        private static void Switch()
        {
            CurrentPane++;
            if (CurrentPane > 2)
                CurrentPane = 1;
            RedrawRequired = true;
        }

        private static void PrintFileSystemInfo(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            string fullPath = currentFileSystemInfo.FullName;
            if (Checking.FolderExists(fullPath))
            {
                // The file system info instance points to a folder
                var DirInfo = new DirectoryInfo(fullPath);
                finalInfoRendered.AppendLine(Translate.DoTranslation("Name: {0}").FormatString(DirInfo.Name));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Full name: {0}").FormatString(Filesystem.NeutralizePath(DirInfo.FullName)));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Size: {0}").FormatString(SizeGetter.GetAllSizesInFolder(DirInfo).FileSizeToString()));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Creation time: {0}").FormatString(TimeDateRenderers.Render(DirInfo.CreationTime)));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Last access time: {0}").FormatString(TimeDateRenderers.Render(DirInfo.LastAccessTime)));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Last write time: {0}").FormatString(TimeDateRenderers.Render(DirInfo.LastWriteTime)));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Attributes: {0}").FormatString(DirInfo.Attributes));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Parent directory: {0}").FormatString(Filesystem.NeutralizePath(DirInfo.Parent.FullName)));
            }
            else
            {
                // The file system info instance points to a file
                FileInfo fileInfo = new(fullPath);
                var Style = LineEndingsTools.GetLineEndingFromFile(fullPath);
                finalInfoRendered.AppendLine(Translate.DoTranslation("Name: {0}").FormatString(fileInfo.Name));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Full name: {0}").FormatString(Filesystem.NeutralizePath(fileInfo.FullName)));
                finalInfoRendered.AppendLine(Translate.DoTranslation("File size: {0}").FormatString(fileInfo.Length.FileSizeToString()));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Creation time: {0}").FormatString(TimeDateRenderers.Render(fileInfo.CreationTime)));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Last access time: {0}").FormatString(TimeDateRenderers.Render(fileInfo.LastAccessTime)));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Last write time: {0}").FormatString(TimeDateRenderers.Render(fileInfo.LastWriteTime)));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Attributes: {0}").FormatString(fileInfo.Attributes));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Where to find: {0}").FormatString(Filesystem.NeutralizePath(fileInfo.DirectoryName)));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Newline style:") + " {0}".FormatString(Style.ToString()));
                finalInfoRendered.AppendLine(Translate.DoTranslation("Binary file:") + " {0}".FormatString(Parsing.IsBinaryFile(fileInfo.FullName)));
                finalInfoRendered.AppendLine(Translate.DoTranslation("MIME metadata:") + " {0}".FormatString(MimeTypes.GetMimeType(Filesystem.NeutralizePath(fileInfo.FullName))));
            }
            finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));

            // Now, render the info box
            InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), BoxForegroundColor, BoxBackgroundColor);
            RedrawRequired = true;
        }

        private static void CopyFileOrDir(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            string dest = (CurrentPane == 2 ? secondPanePath : firstPanePath) + "/";
            DebugCheck.AssertNull(dest, "destination is null!");
            DebugCheck.Assert(string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
            Copying.CopyFileOrDir(currentFileSystemInfo.FullName, dest);
        }

        private static void MoveFileOrDir(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            string dest = (CurrentPane == 2 ? secondPanePath : firstPanePath) + "/";
            DebugCheck.AssertNull(dest, "destination is null!");
            DebugCheck.Assert(string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
            Moving.MoveFileOrDir(currentFileSystemInfo.FullName, dest);
        }

        private static void RemoveFileOrDir(FileSystemInfo currentFileSystemInfo)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            Removing.RemoveFileOrDir(currentFileSystemInfo.FullName);
        }
    }
}
