
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
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
using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Files.Operations;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.TimeDate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;

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
        private static List<FileSystemInfo> cachedFileInfosFirstPane = new();
        private static List<FileSystemInfo> cachedFileInfosSecondPane = new();
        private static List<FileManagerBinding> fileManagerBindings = new()
        {
            // Operations
            new FileManagerBinding("Copy",   ConsoleKey.F1, (destinationPath, sourcePath) => Copying.CopyFileOrDir(sourcePath.FullName, destinationPath)),
            new FileManagerBinding("Move",   ConsoleKey.F2, (destinationPath, sourcePath) => Moving.MoveFileOrDir(sourcePath.FullName, destinationPath)),
            new FileManagerBinding("Delete", ConsoleKey.F3, (_,               sourcePath) => Removing.RemoveFileOrDir(sourcePath.FullName)),

            // Misc bindings
            new FileManagerBinding("Exit"  , ConsoleKey.Escape, (_, _) => isExiting = true),
            new FileManagerBinding("Switch", ConsoleKey.Tab   , (_, _) =>
            {
                currentPane++;
                if (currentPane > 2)
                    currentPane = 1;
                redrawRequired = true;
            }),
        };

        /// <summary>
        /// File manager background color
        /// </summary>
        public static Color FileManagerBackgroundColor = new(Convert.ToInt32(ConsoleColors.DarkBlue));
        /// <summary>
        /// File manager foreground color
        /// </summary>
        public static Color FileManagerForegroundColor = new(Convert.ToInt32(ConsoleColors.Yellow));
        /// <summary>
        /// File manager pane background color
        /// </summary>
        public static Color FileManagerPaneBackgroundColor = new(Convert.ToInt32(ConsoleColors.Blue3));
        /// <summary>
        /// File manager pane separator color
        /// </summary>
        public static Color FileManagerPaneSeparatorColor = new(Convert.ToInt32(ConsoleColors.DarkGreen_005f00));
        /// <summary>
        /// File manager selected pane separator color
        /// </summary>
        public static Color FileManagerPaneSelectedSeparatorColor = new(Convert.ToInt32(ConsoleColors.Green3_00d700));
        /// <summary>
        /// File manager pane selected file color (foreground)
        /// </summary>
        public static Color FileManagerPaneSelectedFileForeColor = new(Convert.ToInt32(ConsoleColors.Yellow));
        /// <summary>
        /// File manager pane selected file color (background)
        /// </summary>
        public static Color FileManagerPaneSelectedFileBackColor = new(Convert.ToInt32(ConsoleColors.DarkBlue));
        /// <summary>
        /// File manager pane file color (foreground)
        /// </summary>
        public static Color FileManagerPaneFileForeColor = new(Convert.ToInt32(ConsoleColors.DarkYellow));
        /// <summary>
        /// File manager pane file color (background)
        /// </summary>
        public static Color FileManagerPaneFileBackColor = new(Convert.ToInt32(ConsoleColors.Blue3));
        /// <summary>
        /// File manager option background color
        /// </summary>
        public static Color FileManagerOptionBackgroundColor = new(Convert.ToInt32(ConsoleColors.DarkCyan));
        /// <summary>
        /// File manager key binding in option color
        /// </summary>
        public static Color FileManagerKeyBindingOptionColor = new(Convert.ToInt32(ConsoleColors.Black));
        /// <summary>
        /// File manager option foreground color
        /// </summary>
        public static Color FileManagerOptionForegroundColor = new(Convert.ToInt32(ConsoleColors.Cyan));

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
                            TextWriterWhereColor.WriteWhere($"{binding.BindingName}  ", ConsoleWrapper.CursorLeft + 1, ConsoleWrapper.WindowHeight - 1, FileManagerOptionForegroundColor, FileManagerBackgroundColor);
                        }
                    }

                    // Don't require redraw
                    redrawRequired = false;
                }

                // Render the file lists (first pane)
                var FilesFirstPane = Listing.CreateList(firstPath, true);
                cachedFileInfosFirstPane = FilesFirstPane;
                int pagesFirstPane = FilesFirstPane.Count / SeparatorMaximumHeightInterior;
                int answersPerPageFirstPane = SeparatorMaximumHeightInterior - 1;
                int currentPageFirstPane = (firstPaneCurrentSelection - 1) / answersPerPageFirstPane;
                int startIndexFirstPane = answersPerPageFirstPane * currentPageFirstPane;
                int endIndexFirstPane = answersPerPageFirstPane * (currentPageFirstPane + 1);
                for (int i = startIndexFirstPane; i <= endIndexFirstPane && i <= FilesFirstPane.Count - 1; i++)
                {
                    // Populate the first pane
                    FileSystemInfo file = FilesFirstPane[i];
                    bool isDirectory = Checking.FolderExists(file.FullName);

                    string finalEntry = $" [{(isDirectory ? "/" : "*")}] {file.Name}".Truncate(SeparatorHalfConsoleWidthInterior - 3);
                    var finalForeColor = i == firstPaneCurrentSelection - 1 ? FileManagerPaneSelectedFileForeColor : FileManagerPaneFileForeColor;
                    var finalBackColor = i == firstPaneCurrentSelection - 1 ? FileManagerPaneSelectedFileBackColor : FileManagerPaneFileBackColor;
                    TextWriterWhereColor.WriteWhere(finalEntry + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalEntry.Length), 1, SeparatorMinimumHeightInterior + i - startIndexFirstPane, finalForeColor, finalBackColor);
                }

                // Render the file lists (second pane)
                var FilesSecondPane = Listing.CreateList(secondPath, true);
                cachedFileInfosSecondPane = FilesSecondPane;
                int pagesSecondPane = FilesSecondPane.Count / SeparatorMaximumHeightInterior;
                int answersPerPageSecondPane = SeparatorMaximumHeightInterior - 1;
                int currentPageSecondPane = (secondPaneCurrentSelection - 1) / answersPerPageSecondPane;
                int startIndexSecondPane = answersPerPageSecondPane * currentPageSecondPane;
                int endIndexSecondPane = answersPerPageSecondPane * (currentPageSecondPane + 1);
                for (int i = startIndexSecondPane; i <= endIndexSecondPane && i <= FilesSecondPane.Count - 1; i++)
                {
                    // Populate the second pane
                    FileSystemInfo file = FilesSecondPane[i];
                    bool isDirectory = Checking.FolderExists(file.FullName);

                    string finalEntry = $" [{(isDirectory ? "/" : "*")}] {file.Name}".Truncate(SeparatorHalfConsoleWidthInterior - 3);
                    var finalForeColor = i == secondPaneCurrentSelection - 1 ? FileManagerPaneSelectedFileForeColor : FileManagerPaneFileForeColor;
                    var finalBackColor = i == secondPaneCurrentSelection - 1 ? FileManagerPaneSelectedFileBackColor : FileManagerPaneFileBackColor;
                    TextWriterWhereColor.WriteWhere(finalEntry + " ".Repeat(SeparatorHalfConsoleWidthInterior - finalEntry.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + i - startIndexSecondPane, finalForeColor, finalBackColor);
                }

                // Now, populate the current file/folder info from the current pane
                var FileInfoCurrentPane =    currentPane == 2 ?
                                             cachedFileInfosSecondPane[secondPaneCurrentSelection - 1] :
                                             cachedFileInfosFirstPane[firstPaneCurrentSelection - 1];
                var PathCurrentPane =        currentPane == 2 ?
                                             secondPath :
                                             firstPath;
                var CachedFilesCurrentPane = currentPane == 2 ?
                                             cachedFileInfosSecondPane :
                                             cachedFileInfosFirstPane;

                // Write file info
                string finalInfoRendered = "";
                try
                {
                    bool infoIsDirectory = Checking.FolderExists(FileInfoCurrentPane.FullName);
                    finalInfoRendered = (
                        // Name and directory indicator
                        $" [{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.Name} | " + 

                        // File size or directory size
                        $"{(!infoIsDirectory ? ((FileInfo)FileInfoCurrentPane).Length.FileSizeToString() : SizeGetter.GetAllSizesInFolder((DirectoryInfo)FileInfoCurrentPane))} | " + 

                        // Modified date
                        $"{(!infoIsDirectory ? TimeDateRenderers.Render(((FileInfo)FileInfoCurrentPane).LastWriteTime) : "")}"
                    );
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
                ConsoleKey pressedKey = ConsoleWrapper.ReadKey(true).Key;
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
                    default:
                        var implementedBindings = fileManagerBindings.Where((binding) => binding.BindingKeyName == pressedKey);
                        foreach (var implementedBinding in implementedBindings)
                            implementedBinding.BindingAction.Invoke(PathCurrentPane, FileInfoCurrentPane);
                        break;
                }
            }

            // Clear the console to clean up
            ColorTools.LoadBack();
        }
    }
}
