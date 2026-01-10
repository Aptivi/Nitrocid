//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Themes.Colors;
using Textify.General;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files.Instances;

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// File listing module
    /// </summary>
    public static partial class FilesystemTools
    {
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
            List(folder, Config.MainConfig.ShowFileDetailsList, Config.MainConfig.SuppressUnauthorizedMessages, Config.MainConfig.SortList);

        /// <summary>
        /// List all files and folders in a specified folder
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="Sort">Whether to sort the filesystem entries</param>
        public static void List(string folder, bool Sort) =>
            List(folder, Config.MainConfig.ShowFileDetailsList, Config.MainConfig.SuppressUnauthorizedMessages, Sort);

        /// <summary>
        /// List all files and folders in a specified folder
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="ShowFileDetails">Whether to show the file details</param>
        /// <param name="SuppressUnauthorizedMessage">Whether to silence the access denied messages</param>
        public static void List(string folder, bool ShowFileDetails, bool SuppressUnauthorizedMessage) =>
            List(folder, ShowFileDetails, SuppressUnauthorizedMessage, Config.MainConfig.SortList);

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
            DebugWriter.WriteDebug(DebugLevel.I, "Folder {0} will be listed...", vars: [folder]);

            // List files and folders
            folder = FilesystemTools.NeutralizePath(folder);
            if (FilesystemTools.FolderExists(folder) | folder.ContainsAnyOf(["?", "*"]))
            {
                List<FileSystemEntry> enumeration;
                SeparatorWriterColor.WriteSeparatorColor(folder, ThemeColorsTools.GetColor(ThemeColorType.ListTitle));

                // Try to create a list
                try
                {
                    enumeration = CreateList(folder, Sort, Recursive);
                    if (enumeration.Count == 0)
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_LISTFOLDEREMPTY"), true, ThemeColorType.Warning);

                    // Enumerate each entry
                    long TotalSize = 0L;
                    foreach (FileSystemEntry Entry in enumeration)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Enumerating {0}...", vars: [Entry.FilePath]);
                        try
                        {
                            switch (Entry.Type)
                            {
                                case FileSystemEntryType.File:
                                    TotalSize += ((FileInfo)Entry.BaseEntry).Length;
                                    FilesystemTools.PrintFileInfo(Entry);
                                    break;
                                case FileSystemEntryType.Directory:
                                    FilesystemTools.PrintDirectoryInfo(Entry);
                                    break;
                            }
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            if (!SuppressUnauthorizedMessage)
                                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_FILES_UNAUTHORIZED"), true, ThemeColorType.Error, Entry.OriginalFilePath);
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                        catch (Exception ex)
                        {
                            if (!SuppressUnauthorizedMessage)
                                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_FILES_INFOFAILED"), true, ThemeColorType.Error, Entry.OriginalFilePath);
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                    }

                    // Show total size in list optionally
                    if (Config.MainConfig.ShowTotalSizeInList)
                        TextWriterColor.Write(CharManager.NewLine + LanguageTools.GetLocalized("NKS_FILES_LISTTOTALSIZE") + " {0}", TotalSize.SizeString());
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_LISTDIRUNKNOWNERROR"), true, ThemeColorType.Error, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else if (FilesystemTools.FileExists(folder))
            {
                try
                {
                    FilesystemTools.PrintFileInfo(new FileSystemEntry(folder), ShowFileDetails);
                }
                catch (UnauthorizedAccessException ex)
                {
                    if (!SuppressUnauthorizedMessage)
                        TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_FILES_UNAUTHORIZED"), true, ThemeColorType.Error, folder);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
                catch (Exception ex)
                {
                    if (!SuppressUnauthorizedMessage)
                        TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_FILES_INFOFAILED"), true, ThemeColorType.Error, folder);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_DIRECTORYNOTFOUND2"), true, ThemeColorType.Error, folder);
                DebugWriter.WriteDebug(DebugLevel.I, "IO.FolderExists = {0}", vars: [FilesystemTools.FolderExists(folder)]);
            }
        }

        /// <summary>
        /// List all files and folders in a specified folder (tree form)
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        public static void ListTree(string folder) =>
            ListTree(folder, Config.MainConfig.SuppressUnauthorizedMessages, Config.MainConfig.SortList, 0);

        /// <summary>
        /// List all files and folders in a specified folder (tree form)
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="Sort">Whether to sort the filesystem entries</param>
        public static void ListTree(string folder, bool Sort) =>
            ListTree(folder, Config.MainConfig.SuppressUnauthorizedMessages, Sort, 0);

        /// <summary>
        /// List all files and folders in a specified folder (tree form)
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="SuppressUnauthorizedMessage">Whether to silence the access denied messages</param>
        /// <param name="Sort">Whether to sort the filesystem entries</param>
        public static void ListTree(string folder, bool SuppressUnauthorizedMessage, bool Sort) =>
            ListTree(folder, SuppressUnauthorizedMessage, Sort, 0);

        /// <summary>
        /// List all files and folders in a specified folder (tree form)
        /// </summary>
        /// <param name="folder">Full path to folder</param>
        /// <param name="SuppressUnauthorizedMessage">Whether to silence the access denied messages</param>
        /// <param name="Sort">Whether to sort the filesystem entries</param>
        /// <param name="level">Indentation level</param>
        internal static void ListTree(string folder, bool SuppressUnauthorizedMessage, bool Sort, int level = 0)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Folder {0} will be listed...", vars: [folder]);

            // List files and folders
            folder = FilesystemTools.NeutralizePath(folder);
            if (FilesystemTools.FolderExists(folder) | folder.ContainsAnyOf(["?", "*"]))
            {
                List<FileSystemEntry> enumeration;
                if (level == 0)
                    SeparatorWriterColor.WriteSeparatorColor(folder, ThemeColorsTools.GetColor(ThemeColorType.ListTitle));

                // Try to create a list
                try
                {
                    enumeration = CreateList(folder, Sort);
                    if (enumeration.Count == 0)
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_LISTFOLDEREMPTY"), true, ThemeColorType.Warning);

                    // Enumerate each entry
                    foreach (FileSystemEntry Entry in enumeration)
                    {
                        string name = Path.GetFileName(Entry.FilePath);
                        DebugWriter.WriteDebug(DebugLevel.I, "Enumerating {0}...", Entry.FilePath);
                        try
                        {
                            switch (Entry.Type)
                            {
                                case FileSystemEntryType.File:
                                    ListEntryWriterColor.WriteListEntry(name, Entry.FileSize.SizeString(), indent: level);
                                    break;
                                case FileSystemEntryType.Directory:
                                    ListEntryWriterColor.WriteListEntry(name, "[/]", indent: level);
                                    ListTree(Entry.FilePath, SuppressUnauthorizedMessage, Sort, level + 1);
                                    break;
                            }
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            if (!SuppressUnauthorizedMessage)
                                ListEntryWriterColor.WriteListEntry(name, LanguageTools.GetLocalized("NKS_FILES_UNAUTHORIZED2"), indent: level);
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                        catch (Exception ex)
                        {
                            if (!SuppressUnauthorizedMessage)
                                ListEntryWriterColor.WriteListEntry(name, LanguageTools.GetLocalized("NKS_FILES_ERROR"), indent: level);
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_LISTDIRUNKNOWNERROR"), true, ThemeColorType.Error, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else if (FilesystemTools.FileExists(folder))
            {
                var entry = new FileSystemEntry(folder);
                string name = Path.GetFileName(entry.FilePath);
                try
                {
                    ListEntryWriterColor.WriteListEntry(name, entry.FileSize.SizeString());
                }
                catch (UnauthorizedAccessException ex)
                {
                    if (!SuppressUnauthorizedMessage)
                        ListEntryWriterColor.WriteListEntry(name, LanguageTools.GetLocalized("NKS_FILES_UNAUTHORIZED2"));
                    DebugWriter.WriteDebugStackTrace(ex);
                }
                catch (Exception ex)
                {
                    if (!SuppressUnauthorizedMessage)
                        ListEntryWriterColor.WriteListEntry(name, LanguageTools.GetLocalized("NKS_FILES_ERROR"));
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_DIRECTORYNOTFOUND2"), true, ThemeColorType.Error, folder);
                DebugWriter.WriteDebug(DebugLevel.I, "IO.FolderExists = {0}", vars: [FilesystemTools.FolderExists(folder)]);
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
