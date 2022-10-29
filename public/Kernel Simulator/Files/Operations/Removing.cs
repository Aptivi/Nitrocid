
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

using System;
using System.IO;
using Extensification.StringExts;
using KS.Kernel.Exceptions;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using UnitsNet;

namespace KS.Files.Operations
{
    /// <summary>
    /// Removing file operations module
    /// </summary>
    public static class Removing
    {

        /// <summary>
        /// Removes a directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        public static void RemoveDirectory(string Target) =>
            RemoveDirectory(Target, Filesystem.ShowFilesystemProgress);

        /// <summary>
        /// Removes a directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        /// <param name="ShowProgress">Whether or not to show what files are being removed</param>
        public static void RemoveDirectory(string Target, bool ShowProgress)
        {
            Filesystem.ThrowOnInvalidPath(Target);
            if (!Checking.FolderExists(Target))
                throw new IOException(Translate.DoTranslation("Directory {0} not found.").FormatString(Target));

            // Get all source directories and files
            var SourceDirInfo = new DirectoryInfo(Target);
            var SourceDirectories = SourceDirInfo.GetDirectories();
            DebugWriter.WriteDebug(DebugLevel.I, "Source directories: {0}", SourceDirectories.Length);
            var SourceFiles = SourceDirInfo.GetFiles();
            DebugWriter.WriteDebug(DebugLevel.I, "Source files: {0}", SourceFiles.Length);

            // Iterate through every file and delete them
            foreach (FileInfo SourceFile in SourceFiles)
            {
                string DestinationFilePath = Path.Combine(Target, SourceFile.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Removing file {0}...", DestinationFilePath);
                if (ShowProgress)
                    TextWriterColor.Write("-> {0}", DestinationFilePath);
                RemoveFile(DestinationFilePath);
            }

            // Iterate through every subdirectory and delete them
            foreach (DirectoryInfo SourceDirectory in SourceDirectories)
            {
                string DestinationDirectoryPath = Path.Combine(Target, SourceDirectory.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Calling RemoveDirectory() with destination {0}...", DestinationDirectoryPath);
                if (ShowProgress)
                    TextWriterColor.Write("* {0}", DestinationDirectoryPath);
                RemoveDirectory(DestinationDirectoryPath);
            }

            // Raise event
            Directory.Delete(Target, true);
            Kernel.Events.EventsManager.FireEvent("DirectoryRemoved", Target);
        }

        /// <summary>
        /// Removes a directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveDirectory(string Target)
        {
            try
            {
                RemoveDirectory(Target);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Removes a file
        /// </summary>
        /// <param name="Target">Target directory</param>
        public static void RemoveFile(string Target)
        {
            Filesystem.ThrowOnInvalidPath(Target);
            string Dir = Filesystem.NeutralizePath(Target);
            File.Delete(Dir);

            // Raise event
            Kernel.Events.EventsManager.FireEvent("FileRemoved", Target);
        }

        /// <summary>
        /// Removes a file
        /// </summary>
        /// <param name="Target">Target directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveFile(string Target)
        {
            try
            {
                RemoveFile(Target);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Removes file or directory
        /// </summary>
        /// <param name="Target">Path to file or directory</param>
        /// <exception cref="FilesystemException"></exception>
        public static void RemoveFileOrDir(string Target)
        {
            if (Checking.FileExists(Target))
                RemoveFile(Target);
            else if (Checking.FolderExists(Target))
                RemoveDirectory(Target);
            else
                throw new FilesystemException(Translate.DoTranslation("File or directory {0} doesn't exist."), Target);
        }

        /// <summary>
        /// Removes a file or directory
        /// </summary>
        /// <param name="Target">Target file or directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveFileOrDir(string Target)
        {
            try
            {
                RemoveFileOrDir(Target);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

    }
}
