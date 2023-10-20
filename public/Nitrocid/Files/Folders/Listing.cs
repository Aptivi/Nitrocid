
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

using System;
using System.Collections.Generic;
using System.IO;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Drivers;
using KS.Files.Instances;
using KS.Files.Operations.Printing;
using KS.Files.Operations.Querying;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Text;

namespace KS.Files.Folders
{
    /// <summary>
    /// File listing module
    /// </summary>
    public static class Listing
    {

        /// <summary>
        /// Whether to sort the list or not
        /// </summary>
        public static bool SortList =>
            Config.MainConfig.SortList;
        /// <summary>
        /// Sort mode
        /// </summary>
        public static FilesystemSortOptions SortMode =>
            (FilesystemSortOptions)Config.MainConfig.SortMode;
        /// <summary>
        /// Sort direction
        /// </summary>
        public static FilesystemSortDirection SortDirection =>
            (FilesystemSortDirection)Config.MainConfig.SortDirection;
        /// <summary>
        /// Show file details when listing
        /// </summary>
        public static bool ShowFileDetailsList =>
            Config.MainConfig.ShowFileDetailsList;
        /// <summary>
        /// Show total size when listing
        /// </summary>
        public static bool ShowTotalSizeInList =>
            Config.MainConfig.ShowTotalSizeInList;

        /// <summary>
        /// Creates a list of files and directories
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="Sorted">Whether the list is sorted or not</param>
        /// <param name="Recursive">Whether the list is recursive or not</param>
        /// <returns>List of filesystem entries if any. Empty list if folder is not found or is empty.</returns>
        public static List<FileSystemEntry> CreateList(string folder, bool Sorted = false, bool Recursive = false) =>
            DriverHandler.CurrentFilesystemDriverLocal.CreateList(folder, Sorted, Recursive);

        /// <summary>
        /// List all files and folders in a specified folder
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        public static void List(string folder) =>
            List(folder, ShowFileDetailsList, FilesystemTools.SuppressUnauthorizedMessages, SortList);

        /// <summary>
        /// List all files and folders in a specified folder
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="Sort">Whether to sort the filesystem entries</param>
        public static void List(string folder, bool Sort) =>
            List(folder, ShowFileDetailsList, FilesystemTools.SuppressUnauthorizedMessages, Sort);

        /// <summary>
        /// List all files and folders in a specified folder
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="ShowFileDetails">Whether to show the file details</param>
        /// <param name="SuppressUnauthorizedMessage">Whether to silence the access denied messages</param>
        public static void List(string folder, bool ShowFileDetails, bool SuppressUnauthorizedMessage) =>
            List(folder, ShowFileDetails, SuppressUnauthorizedMessage, SortList);

        /// <summary>
        /// List all files and folders in a specified folder
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="ShowFileDetails">Whether to show the file details</param>
        /// <param name="SuppressUnauthorizedMessage">Whether to silence the access denied messages</param>
        /// <param name="Sort">Whether to sort the filesystem entries</param>
        /// <param name="Recursive">Whether the list is recursive or not</param>
        public static void List(string folder, bool ShowFileDetails, bool SuppressUnauthorizedMessage, bool Sort, bool Recursive = false)
        {
            FilesystemTools.ThrowOnInvalidPath(folder);
            DebugWriter.WriteDebug(DebugLevel.I, "Folder {0} will be listed...", folder);

            // List files and folders
            folder = FilesystemTools.NeutralizePath(folder);
            if (Checking.FolderExists(folder) | folder.ContainsAnyOf(new[] { "?", "*" }))
            {
                List<FileSystemEntry> enumeration;
                SeparatorWriterColor.WriteSeparator(folder, true);

                // Try to create a list
                try
                {
                    enumeration = CreateList(folder, Sort, Recursive);
                    if (enumeration.Count == 0)
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Folder is empty."), true, KernelColorType.Warning);

                    // Enumerate each entry
                    long TotalSize = 0L;
                    foreach (FileSystemEntry Entry in enumeration)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Enumerating {0}...", Entry.FilePath);
                        try
                        {
                            switch (Entry.Type)
                            {
                                case FileSystemEntryType.File:
                                    TotalSize += ((FileInfo)Entry.BaseEntry).Length;
                                    FileInfoPrinter.PrintFileInfo(Entry);
                                    break;
                                case FileSystemEntryType.Directory:
                                    DirectoryInfoPrinter.PrintDirectoryInfo(Entry);
                                    break;
                            }
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            if (!SuppressUnauthorizedMessage)
                                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("You are not authorized to get info for {0}."), true, KernelColorType.Error, Entry.OriginalFilePath);
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                    }

                    // Show total size in list optionally
                    if (ShowTotalSizeInList)
                        TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Total size in folder:") + " {0}", TotalSize.SizeString());
                }
                catch (Exception ex)
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Unknown error while listing in directory: {0}"), true, KernelColorType.Error, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else if (Checking.FileExists(folder))
            {
                try
                {
                    FileInfoPrinter.PrintFileInfo(new FileSystemEntry(folder), ShowFileDetails);
                }
                catch (UnauthorizedAccessException ex)
                {
                    if (!SuppressUnauthorizedMessage)
                        TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("You are not authorized to get info for {0}."), true, KernelColorType.Error, folder);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Directory {0} not found"), true, KernelColorType.Error, folder);
                DebugWriter.WriteDebug(DebugLevel.I, "IO.FolderExists = {0}", Checking.FolderExists(folder));
            }
        }

        /// <summary>
        /// Gets the filesystem entries of the parent with the specified pattern (wildcards, ...)
        /// </summary>
        /// <param name="Path">The path, including the pattern</param>
        /// <param name="IsFile">Is the entry a file?</param>
        /// <param name="Recursive">Whether the list is recursive or not</param>
        /// <returns>The array of full paths</returns>
        public static string[] GetFilesystemEntries(string Path, bool IsFile = false, bool Recursive = false) =>
            DriverHandler.CurrentFilesystemDriverLocal.GetFilesystemEntries(Path, IsFile, Recursive);

        /// <summary>
        /// Gets the filesystem entries of the parent with the specified pattern (wildcards, ...)
        /// </summary>
        /// <param name="Parent">The parent path. It can be neutralized if necessary</param>
        /// <param name="Pattern">The pattern</param>
        /// <param name="Recursive">Whether the list is recursive or not</param>
        /// <returns>The array of full paths</returns>
        public static string[] GetFilesystemEntries(string Parent, string Pattern, bool Recursive = false) =>
            DriverHandler.CurrentFilesystemDriverLocal.GetFilesystemEntries(Parent, Pattern, Recursive);

        /// <summary>
        /// Gets the filesystem entries of the parent using regular expressions
        /// </summary>
        /// <param name="Parent">The parent path. It can be neutralized if necessary</param>
        /// <param name="Pattern">The regular expression pattern</param>
        /// <param name="Recursive">Whether the list is recursive or not</param>
        /// <returns>The array of full paths</returns>
        public static string[] GetFilesystemEntriesRegex(string Parent, string Pattern, bool Recursive = false) =>
            DriverHandler.CurrentFilesystemDriverLocal.GetFilesystemEntriesRegex(Parent, Pattern, Recursive);

    }
}
