
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
using KS.ConsoleBase;
using KS.Files.Folders;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.TimeDate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KS.ConsoleBase.Inputs;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;
using MimeKit;
using KS.Files.LineEndings;
using System.Text;
using KS.Kernel.Configuration;

namespace KS.Files.Interactive
{
    /// <summary>
    /// File manager class relating to the interactive file manager planned back in 2018
    /// </summary>
    public static class FileManagerCli
    {
        private static bool redrawRequired = true;
        private static bool isExiting = false;
        private static int firstPaneCurrentSelection = 1;
        private static int secondPaneCurrentSelection = 1;
        private static int currentPane = 1;
        private static string firstPanePath = "";
        private static string secondPanePath = "";
        private static List<FileSystemInfo> cachedFileInfosFirstPane = new();
        private static List<FileSystemInfo> cachedFileInfosSecondPane = new();
        private static readonly List<FileManagerBinding> fileManagerBindings = new()
        {
            // Operations
            new FileManagerBinding(/* Localizable */ "Copy",   ConsoleKey.F1, (destinationPath, sourcePath) => CopyFileOrDir(sourcePath, destinationPath), true),
            new FileManagerBinding(/* Localizable */ "Move",   ConsoleKey.F2, (destinationPath, sourcePath) => MoveFileOrDir(sourcePath, destinationPath), true),
            new FileManagerBinding(/* Localizable */ "Delete", ConsoleKey.F3, (_,               sourcePath) => RemoveFileOrDir(sourcePath), true),
            new FileManagerBinding(/* Localizable */ "Up",     ConsoleKey.F4, (_,               _         ) => GoUp(), true),
            new FileManagerBinding(/* Localizable */ "Info",   ConsoleKey.F5, (_,               sourcePath) => PrintFileSystemInfo(sourcePath), true),

            // Misc bindings
            new FileManagerBinding(/* Localizable */ "Exit"  , ConsoleKey.Escape, (_, _) => isExiting = true, true),
            new FileManagerBinding(/* Localizable */ "Switch", ConsoleKey.Tab   , (_, _) => Switch(), true),
        };

        /// <summary>
        /// File manager background color
        /// </summary>
        public static Color FileManagerBackgroundColor =>
            new(Config.MainConfig.FileManagerBackgroundColor);
        /// <summary>
        /// File manager foreground color
        /// </summary>
        public static Color FileManagerForegroundColor =>
            new(Config.MainConfig.FileManagerForegroundColor);
        /// <summary>
        /// File manager pane background color
        /// </summary>
        public static Color FileManagerPaneBackgroundColor =>
            new(Config.MainConfig.FileManagerPaneBackgroundColor);
        /// <summary>
        /// File manager pane separator color
        /// </summary>
        public static Color FileManagerPaneSeparatorColor =>
            new(Config.MainConfig.FileManagerPaneSeparatorColor);
        /// <summary>
        /// File manager selected pane separator color
        /// </summary>
        public static Color FileManagerPaneSelectedSeparatorColor =>
            new(Config.MainConfig.FileManagerPaneSelectedSeparatorColor);
        /// <summary>
        /// File manager pane selected file color (foreground)
        /// </summary>
        public static Color FileManagerPaneSelectedFileForeColor =>
            new(Config.MainConfig.FileManagerPaneSelectedFileForeColor);
        /// <summary>
        /// File manager pane selected file color (background)
        /// </summary>
        public static Color FileManagerPaneSelectedFileBackColor =>
            new(Config.MainConfig.FileManagerPaneSelectedFileBackColor);
        /// <summary>
        /// File manager pane file color (foreground)
        /// </summary>
        public static Color FileManagerPaneFileForeColor =>
            new(Config.MainConfig.FileManagerPaneFileForeColor);
        /// <summary>
        /// File manager pane file color (background)
        /// </summary>
        public static Color FileManagerPaneFileBackColor =>
            new(Config.MainConfig.FileManagerPaneFileBackColor);
        /// <summary>
        /// File manager option background color
        /// </summary>
        public static Color FileManagerOptionBackgroundColor =>
            new(Config.MainConfig.FileManagerOptionBackgroundColor);
        /// <summary>
        /// File manager key binding in option color
        /// </summary>
        public static Color FileManagerKeyBindingOptionColor =>
            new(Config.MainConfig.FileManagerKeyBindingOptionColor);
        /// <summary>
        /// File manager option foreground color
        /// </summary>
        public static Color FileManagerOptionForegroundColor =>
            new(Config.MainConfig.FileManagerOptionForegroundColor);
        /// <summary>
        /// File manager box background color
        /// </summary>
        public static Color FileManagerBoxBackgroundColor =>
            new(Config.MainConfig.FileManagerBoxBackgroundColor);
        /// <summary>
        /// File manager box foreground color
        /// </summary>
        public static Color FileManagerBoxForegroundColor =>
            new(Config.MainConfig.FileManagerBoxForegroundColor);

        /// <summary>
        /// Opens the file manager to the current path
        /// </summary>
        public static void OpenMain() =>
            OpenMain(CurrentDirectory.CurrentDir, CurrentDirectory.CurrentDir);

        /// <summary>
        /// Opens the file manager to the current path
        /// </summary>
        /// <param name="firstPath">(Non)neutralized path to the folder for the first pane</param>
        public static void OpenMain(string firstPath) =>
            OpenMain(firstPath, CurrentDirectory.CurrentDir);

        /// <summary>
        /// Opens the file manager to the specified path
        /// </summary>
        /// <param name="firstPath">(Non)neutralized path to the folder for the first pane</param>
        /// <param name="secondPath">(Non)neutralized path to the folder for the second pane</param>
        public static void OpenMain(string firstPath, string secondPath)
        {
            isExiting = false;
            redrawRequired = true;
            firstPanePath = firstPath;
            secondPanePath = secondPath;
            string lastFirstPanePath = "";
            string lastSecondPanePath = "";

            while (!isExiting)
            {
                // Prepare the console
                ConsoleWrapper.CursorVisible = false;
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = (ConsoleWrapper.WindowWidth / 2) - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMinimumHeightInterior = 2;
                int SeparatorMaximumHeight = ConsoleWrapper.WindowHeight - 2;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

                // Redraw the entire file manager screen
                if (redrawRequired)
                {
                    ColorTools.LoadBack(FileManagerBackgroundColor, true);

                    // Make a separator that separates the two panes to make it look like Total Commander or Midnight Commander. We need information in the upper and the
                    // lower part of the console, so we need to render the entire program to look like this: (just a concept mockup)
                    //
                    //       | vvvvvvvvvvvvvvvvvvvvvv (SeparatorHalfConsoleWidth)
                    //       |  vvvvvvvvvvvvvvvvvvvv  (SeparatorHalfConsoleWidthInterior)
                    // H: 0  |
                    // H: 1  | a--------------------|c---------------------| < ----> (SeparatorMinimumHeight)
                    // H: 2  | |b                   ||d                    | << ----> (SeparatorMinimumHeightInterior)
                    // H: 3  | |                    ||                     | <<
                    // H: 4  | |                    ||                     | <<
                    // H: 5  | |                    ||                     | <<
                    // H: 6  | |                    ||                     | <<
                    // H: 7  | |                    ||                     | <<
                    // H: 8  | |                    ||                     | << ----> (SeparatorMaximumHeightInterior)
                    // H: 9  | |--------------------||---------------------| < ----> (SeparatorMaximumHeight)
                    // H: 10 |
                    //       | where a is the dimension for the first pane upper left corner           (0, SeparatorMinimumHeight                                     (usually 1))
                    //       |   and b is the dimension for the first pane interior upper left corner  (1, SeparatorMinimumHeightInterior                             (usually 2))
                    //       |   and c is the dimension for the second pane upper left corner          (SeparatorHalfConsoleWidth, SeparatorMinimumHeight             (usually 1))
                    //       |   and d is the dimension for the second pane interior upper left corner (SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior (usually 2))

                    // First, the horizontal and vertical separators
                    var finalFirstPaneSeparatorColor = currentPane == 1 ? FileManagerPaneSelectedSeparatorColor : FileManagerPaneSeparatorColor;
                    var finalSecondPaneSeparatorColor = currentPane == 2 ? FileManagerPaneSelectedSeparatorColor : FileManagerPaneSeparatorColor;
                    BorderColor.WriteBorder(0, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalFirstPaneSeparatorColor, FileManagerPaneBackgroundColor);
                    BorderColor.WriteBorder(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalSecondPaneSeparatorColor, FileManagerPaneBackgroundColor);

                    // Render the key bindings
                    ConsoleWrapper.CursorLeft = 0;
                    foreach (FileManagerBinding binding in fileManagerBindings)
                    {
                        // First, check to see if the rendered binding info is going to exceed the console window width
                        if (!($" {binding.BindingKeyName} {binding.BindingName}  ".Length + ConsoleWrapper.CursorLeft >= ConsoleWrapper.WindowWidth))
                        {
                            TextWriterWhereColor.WriteWhere($" {binding.BindingKeyName} ", ConsoleWrapper.CursorLeft + 0, ConsoleWrapper.WindowHeight - 1, FileManagerKeyBindingOptionColor, FileManagerOptionBackgroundColor);
                            TextWriterWhereColor.WriteWhere($"{(binding._localizable ? Translate.DoTranslation(binding.BindingName) : binding.BindingName)}  ", ConsoleWrapper.CursorLeft + 1, ConsoleWrapper.WindowHeight - 1, FileManagerOptionForegroundColor, FileManagerBackgroundColor);
                        }
                    }

                    // Don't require redraw
                    redrawRequired = false;
                }

                // Render the file lists (first pane)
                if (lastFirstPanePath != firstPanePath)
                {
                    var FilesFirstPane = Listing.CreateList(firstPanePath, true);
                    cachedFileInfosFirstPane = FilesFirstPane;
                    lastFirstPanePath = firstPanePath;
                }
                int pagesFirstPane = cachedFileInfosFirstPane.Count / SeparatorMaximumHeightInterior;
                int answersPerPageFirstPane = SeparatorMaximumHeightInterior - 1;
                int currentPageFirstPane = (firstPaneCurrentSelection - 1) / answersPerPageFirstPane;
                int startIndexFirstPane = answersPerPageFirstPane * currentPageFirstPane;
                int endIndexFirstPane = answersPerPageFirstPane * (currentPageFirstPane + 1);
                for (int i = 0; i <= answersPerPageFirstPane; i++)
                {
                    // Populate the first pane
                    string finalEntry = "";
                    int finalIndex = i + startIndexFirstPane;
                    if (finalIndex <= cachedFileInfosFirstPane.Count - 1)
                    {
                        FileSystemInfo file = cachedFileInfosFirstPane[finalIndex];
                        bool isDirectory = Checking.FolderExists(file.FullName);
                        finalEntry = $" [{(isDirectory ? "/" : "*")}] {file.Name}".Truncate(SeparatorHalfConsoleWidthInterior - 4);
                    }

                    var finalForeColor = finalIndex == firstPaneCurrentSelection - 1 ? FileManagerPaneSelectedFileForeColor : FileManagerPaneFileForeColor;
                    var finalBackColor = finalIndex == firstPaneCurrentSelection - 1 ? FileManagerPaneSelectedFileBackColor : FileManagerPaneFileBackColor;
                    TextWriterWhereColor.WriteWhere(finalEntry + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalEntry.Length), 1, SeparatorMinimumHeightInterior + finalIndex - startIndexFirstPane, finalForeColor, finalBackColor);
                }
                ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)firstPaneCurrentSelection / cachedFileInfosFirstPane.Count), SeparatorHalfConsoleWidthInterior - 1, 1, 2, 2, false);

                // Render the file lists (second pane)
                if (lastSecondPanePath != secondPanePath)
                {
                    var FilesSecondPane = Listing.CreateList(secondPanePath, true);
                    cachedFileInfosSecondPane = FilesSecondPane;
                    lastSecondPanePath = secondPanePath;
                }
                int pagesSecondPane = cachedFileInfosSecondPane.Count / SeparatorMaximumHeightInterior;
                int answersPerPageSecondPane = SeparatorMaximumHeightInterior - 1;
                int currentPageSecondPane = (secondPaneCurrentSelection - 1) / answersPerPageSecondPane;
                int startIndexSecondPane = answersPerPageSecondPane * currentPageSecondPane;
                int endIndexSecondPane = answersPerPageSecondPane * (currentPageSecondPane + 1);
                for (int i = 0; i <= answersPerPageSecondPane; i++)
                {
                    // Populate the second pane
                    string finalEntry = "";
                    int finalIndex = i + startIndexSecondPane;
                    if (finalIndex <= cachedFileInfosSecondPane.Count - 1)
                    {
                        FileSystemInfo file = cachedFileInfosSecondPane[finalIndex];
                        bool isDirectory = Checking.FolderExists(file.FullName);
                        finalEntry = $" [{(isDirectory ? "/" : "*")}] {file.Name}".Truncate(SeparatorHalfConsoleWidthInterior - 4);
                    }

                    var finalForeColor = finalIndex == secondPaneCurrentSelection - 1 ? FileManagerPaneSelectedFileForeColor : FileManagerPaneFileForeColor;
                    var finalBackColor = finalIndex == secondPaneCurrentSelection - 1 ? FileManagerPaneSelectedFileBackColor : FileManagerPaneFileBackColor;
                    TextWriterWhereColor.WriteWhere(finalEntry + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalEntry.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + finalIndex - startIndexSecondPane, finalForeColor, finalBackColor);
                }
                ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)secondPaneCurrentSelection / cachedFileInfosSecondPane.Count), ConsoleWrapper.WindowWidth - 3, 1, 2, 2, false);

                // Now, populate the current file/folder info from the current pane
                var PathCurrentPane =        currentPane == 2 ?
                                             secondPanePath :
                                             firstPanePath;
                var CachedFilesCurrentPane = currentPane == 2 ?
                                             cachedFileInfosSecondPane :
                                             cachedFileInfosFirstPane;

                // Write file info
                string finalInfoRendered = "";
                try
                {
                    if (CachedFilesCurrentPane.Count > 0)
                    {
                        var FileInfoCurrentPane = currentPane == 2 ?
                                                  cachedFileInfosSecondPane[secondPaneCurrentSelection - 1] :
                                                  cachedFileInfosFirstPane[firstPaneCurrentSelection - 1];
                        bool infoIsDirectory = Checking.FolderExists(FileInfoCurrentPane.FullName);
                        finalInfoRendered = 
                            // Name and directory indicator
                            $" [{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.Name} | " + 

                            // File size or directory size
                            $"{(!infoIsDirectory ? ((FileInfo)FileInfoCurrentPane).Length.FileSizeToString() : SizeGetter.GetAllSizesInFolder((DirectoryInfo)FileInfoCurrentPane).FileSizeToString())} | " + 

                            // Modified date
                            $"{(!infoIsDirectory ? TimeDateRenderers.Render(((FileInfo)FileInfoCurrentPane).LastWriteTime) : "")}"
                        ;
                    }
                    else
                        finalInfoRendered = Translate.DoTranslation("No files.");
                }
                catch (Exception ex)
                {
                    finalInfoRendered = Translate.DoTranslation("Failed to get file or folder information.");
                    DebugWriter.WriteDebug(DebugLevel.E, "Error trying to get file or folder information in ifm: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
                TextWriterWhereColor.WriteWhere(finalInfoRendered.Truncate(ConsoleWrapper.WindowWidth - 3), 0, 0, FileManagerForegroundColor, FileManagerBackgroundColor);
                ConsoleExtensions.ClearLineToRight();

                // Wait for key
                ConsoleKey pressedKey = Input.DetectKeypress().Key;
                switch (pressedKey)
                {
                    case ConsoleKey.UpArrow:
                        if (currentPane == 2)
                        {
                            secondPaneCurrentSelection--;
                            if (secondPaneCurrentSelection < 1)
                                secondPaneCurrentSelection = CachedFilesCurrentPane.Count;
                        }
                        else
                        {
                            firstPaneCurrentSelection--;
                            if (firstPaneCurrentSelection < 1)
                                firstPaneCurrentSelection = CachedFilesCurrentPane.Count;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentPane == 2)
                        {
                            secondPaneCurrentSelection++;
                            if (secondPaneCurrentSelection > CachedFilesCurrentPane.Count)
                                secondPaneCurrentSelection = 1;
                        }
                        else
                        {
                            firstPaneCurrentSelection++;
                            if (firstPaneCurrentSelection > CachedFilesCurrentPane.Count)
                                firstPaneCurrentSelection = 1;
                        }
                        break;
                    case ConsoleKey.PageUp:
                        if (currentPane == 2)
                            secondPaneCurrentSelection = 1;
                        else
                            firstPaneCurrentSelection = 1;
                        break;
                    case ConsoleKey.PageDown:
                        if (currentPane == 2)
                            secondPaneCurrentSelection = CachedFilesCurrentPane.Count;
                        else
                            firstPaneCurrentSelection = CachedFilesCurrentPane.Count;
                        break;
                    case ConsoleKey.Enter:
                        {
                            if (CachedFilesCurrentPane.Count <= 0)
                                break;
                            var FileInfoCurrentPane = currentPane == 2 ?
                                                      cachedFileInfosSecondPane[secondPaneCurrentSelection - 1] :
                                                      cachedFileInfosFirstPane[firstPaneCurrentSelection - 1];
                            if (!Checking.FolderExists(FileInfoCurrentPane.FullName))
                                break;
                            if (currentPane == 2)
                            {
                                secondPanePath = Filesystem.NeutralizePath(FileInfoCurrentPane.FullName);
                                secondPaneCurrentSelection = 1;
                            }
                            else
                            {
                                firstPanePath = Filesystem.NeutralizePath(FileInfoCurrentPane.FullName);
                                firstPaneCurrentSelection = 1;
                            }
                            break;
                        }
                    default:
                        {
                            var implementedBindings = fileManagerBindings.Where((binding) => binding.BindingKeyName == pressedKey);
                            var FileInfoCurrentPane =
                                CachedFilesCurrentPane.Count > 0 ?
                                    currentPane == 2 ?
                                    cachedFileInfosSecondPane[secondPaneCurrentSelection - 1] :
                                    cachedFileInfosFirstPane[firstPaneCurrentSelection - 1]
                                :
                                    null
                                ;
                            foreach (var implementedBinding in implementedBindings)
                                implementedBinding.BindingAction.Invoke(PathCurrentPane, FileInfoCurrentPane);
                            break;
                        }
                }
            }

            // Clear the console to clean up
            ColorTools.LoadBack();
        }

        private static void GoUp()
        {
            if (currentPane == 2)
            {
                secondPanePath = Filesystem.NeutralizePath(secondPanePath + "/..");
                secondPaneCurrentSelection = 1;
            }
            else
            {
                firstPanePath = Filesystem.NeutralizePath(firstPanePath + "/..");
                firstPaneCurrentSelection = 1;
            }
        }

        private static void Switch()
        {
            currentPane++;
            if (currentPane > 2)
                currentPane = 1;
            redrawRequired = true;
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
            InfoBoxColor.WriteInfoBox(finalInfoRendered.ToString(), FileManagerBoxForegroundColor, FileManagerBoxBackgroundColor);
            redrawRequired = true;
        }

        private static void CopyFileOrDir(FileSystemInfo currentFileSystemInfo, string dest)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            DebugCheck.AssertNull(dest);
            DebugCheck.Assert(string.IsNullOrWhiteSpace(dest));
            Copying.CopyFileOrDir(currentFileSystemInfo.FullName, dest);
        }

        private static void MoveFileOrDir(FileSystemInfo currentFileSystemInfo, string dest)
        {
            // Don't do anything if we haven't been provided anything.
            if (currentFileSystemInfo is null)
                return;

            DebugCheck.AssertNull(dest);
            DebugCheck.Assert(string.IsNullOrWhiteSpace(dest));
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
