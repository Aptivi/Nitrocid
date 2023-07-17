
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
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.Drivers;
using KS.Files.Print;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;

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
        public static List<FileSystemInfo> CreateList(string folder, bool Sorted = false, bool Recursive = false) =>
            DriverHandler.CurrentFilesystemDriverLocal.CreateList(folder, Sorted, Recursive);

        internal static int GetDigits(this long Number) =>
            Number == 0 ? 1 : (int)Math.Log10(Math.Abs(Number)) + 1;

        /// <summary>
        /// List all files and folders in a specified folder
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        public static void List(string folder) => List(folder, ShowFileDetailsList, Flags.SuppressUnauthorizedMessages, SortList);

        /// <summary>
        /// List all files and folders in a specified folder
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="Sort">Whether to sort the filesystem entries</param>
        public static void List(string folder, bool Sort) => List(folder, ShowFileDetailsList, Flags.SuppressUnauthorizedMessages, Sort);

        /// <summary>
        /// List all files and folders in a specified folder
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="ShowFileDetails">Whether to show the file details</param>
        /// <param name="SuppressUnauthorizedMessage">Whether to silence the access denied messages</param>
        public static void List(string folder, bool ShowFileDetails, bool SuppressUnauthorizedMessage) => List(folder, ShowFileDetails, SuppressUnauthorizedMessage, SortList);

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
            Filesystem.ThrowOnInvalidPath(folder);
            DebugWriter.WriteDebug(DebugLevel.I, "Folder {0} will be listed...", folder);

            // List files and folders
            folder = Filesystem.NeutralizePath(folder);
            if (Checking.FolderExists(folder) | folder.ContainsAnyOf(new[] { "?", "*" }))
            {
                List<FileSystemInfo> enumeration;
                SeparatorWriterColor.WriteSeparator(folder, true);

                // Try to create a list
                try
                {
                    enumeration = CreateList(folder, Sort, Recursive);
                    if (enumeration.Count == 0)
                        TextWriterColor.Write(Translate.DoTranslation("Folder is empty."), true, KernelColorType.Warning);

                    // Enumerate each entry
                    long TotalSize = 0L;
                    foreach (FileSystemInfo Entry in enumeration)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Enumerating {0}...", Entry.FullName);
                        try
                        {
                            if (Checking.FileExists(Entry.FullName))
                            {
                                TotalSize += ((FileInfo)Entry).Length;
                                FileInfoPrinter.PrintFileInfo(Entry);
                            }
                            else if (Checking.FolderExists(Entry.FullName))
                            {
                                DirectoryInfoPrinter.PrintDirectoryInfo(Entry);
                            }
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            if (!SuppressUnauthorizedMessage)
                                TextWriterColor.Write("- " + Translate.DoTranslation("You are not authorized to get info for {0}."), true, KernelColorType.Error, Entry.Name);
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                    }

                    // Show total size in list optionally
                    if (ShowTotalSizeInList)
                        TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Total size in folder:") + " {0}", TotalSize.FileSizeToString());
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(Translate.DoTranslation("Unknown error while listing in directory: {0}"), true, KernelColorType.Error, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else if (Checking.FileExists(folder))
            {
                try
                {
                    FileInfoPrinter.PrintFileInfo(new FileInfo(folder), ShowFileDetails);
                }
                catch (UnauthorizedAccessException ex)
                {
                    if (!SuppressUnauthorizedMessage)
                        TextWriterColor.Write("- " + Translate.DoTranslation("You are not authorized to get info for {0}."), true, KernelColorType.Error, folder);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Directory {0} not found"), true, KernelColorType.Error, folder);
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
