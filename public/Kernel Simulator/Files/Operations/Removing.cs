
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
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        public static void RemoveDirectory(string Target, bool ShowProgress, bool secureRemove = false)
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
                RemoveFile(DestinationFilePath, secureRemove);
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
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveDirectory(string Target, bool secureRemove = false)
        {
            try
            {
                RemoveDirectory(Target, secureRemove);
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
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        public static void RemoveFile(string Target, bool secureRemove = false)
        {
            Filesystem.ThrowOnInvalidPath(Target);
            string Dir = Filesystem.NeutralizePath(Target);
            if (secureRemove)
            {
                // Open the file stream and fill it with zeroes
                if (!Checking.FileExists(Dir))
                    throw new FileNotFoundException();
                var target = File.OpenWrite(Dir);
                byte[] zeroes = new byte[target.Length];
                target.Write(zeroes, 0, zeroes.Length);
                target.Close();
            }
            File.Delete(Dir);

            // Raise event
            Kernel.Events.EventsManager.FireEvent("FileRemoved", Target);
        }

        /// <summary>
        /// Removes a file
        /// </summary>
        /// <param name="Target">Target directory</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveFile(string Target, bool secureRemove = false)
        {
            try
            {
                RemoveFile(Target, secureRemove);
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
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        public static void RemoveFileOrDir(string Target, bool secureRemove = false)
        {
            if (Checking.FileExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a file. Removing...", Target);
                RemoveFile(Target, secureRemove);
            }
            else if (Checking.FolderExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a folder. Removing...", Target);
                RemoveDirectory(Target, secureRemove);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to remove {0} which is not found.", Target);
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Can't remove {0} because it doesn't exist."), Target);
            }
        }

        /// <summary>
        /// Removes a file or directory
        /// </summary>
        /// <param name="Target">Target file or directory</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveFileOrDir(string Target, bool secureRemove = false)
        {
            try
            {
                RemoveFileOrDir(Target, secureRemove);
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
