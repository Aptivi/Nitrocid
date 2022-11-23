
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
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Files.Operations
{
    /// <summary>
    /// Moving file operations module
    /// </summary>
    public static class Moving
    {

        /// <summary>
        /// Moves a file or directory
        /// </summary>
        /// <param name="Source">Source file or directory</param>
        /// <param name="Destination">Target file or directory</param>
        /// <exception cref="IOException"></exception>
        public static void MoveFileOrDir(string Source, string Destination)
        {
            Filesystem.ThrowOnInvalidPath(Source);
            Filesystem.ThrowOnInvalidPath(Destination);
            Source = Filesystem.NeutralizePath(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source directory: {0}", Source);
            Destination = Filesystem.NeutralizePath(Destination);
            DebugWriter.WriteDebug(DebugLevel.I, "Target directory: {0}", Destination);
            string FileName = Path.GetFileName(Source);
            DebugWriter.WriteDebug(DebugLevel.I, "Source file name: {0}", FileName);
            if (Checking.FolderExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source and destination are directories");
                MoveDirectory(Source, Destination);

                // Raise event
                Kernel.Events.EventsManager.FireEvent("DirectoryMoved", Source, Destination);
            }
            else if (Checking.FileExists(Source) & Checking.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a directory");
                File.Move(Source, Destination + "/" + FileName);

                // Raise event
                Kernel.Events.EventsManager.FireEvent("FileMoved", Source, Destination + "/" + FileName);
            }
            else if (Checking.FileExists(Source))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Source is a file and destination is a file");
                File.Move(Source, Destination);

                // Raise event
                Kernel.Events.EventsManager.FireEvent("FileMoved", Source, Destination);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Source or destination are invalid.");
                throw new IOException(Translate.DoTranslation("The path is neither a file nor a directory."));
            }
        }

        /// <summary>
        /// Moves a file or directory
        /// </summary>
        /// <param name="Source">Source file or directory</param>
        /// <param name="Destination">Target file or directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryMoveFileOrDir(string Source, string Destination)
        {
            try
            {
                MoveFileOrDir(Source, Destination);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to move {0} to {1}: {2}", Source, Destination, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Moves the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        public static void MoveDirectory(string Source, string Destination) => MoveDirectory(Source, Destination, Filesystem.ShowFilesystemProgress);

        /// <summary>
        /// Moves the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        /// <param name="ShowProgress">Whether or not to show what files are being moved</param>
        public static void MoveDirectory(string Source, string Destination, bool ShowProgress)
        {
            Filesystem.ThrowOnInvalidPath(Source);
            Filesystem.ThrowOnInvalidPath(Destination);
            if (!Checking.FolderExists(Source))
                throw new IOException(Translate.DoTranslation("Directory {0} not found.").FormatString(Source));

            // Get all source directories and files
            var SourceDirInfo = new DirectoryInfo(Source);
            var SourceDirectories = SourceDirInfo.GetDirectories();
            DebugWriter.WriteDebug(DebugLevel.I, "Source directories: {0}", SourceDirectories.Length);
            var SourceFiles = SourceDirInfo.GetFiles();
            DebugWriter.WriteDebug(DebugLevel.I, "Source files: {0}", SourceFiles.Length);

            // Make a destination directory if it doesn't exist
            if (!Checking.FolderExists(Destination))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Destination directory {0} doesn't exist. Creating...", Destination);
                Directory.CreateDirectory(Destination);
            }

            // Iterate through every file and copy them to destination
            foreach (FileInfo SourceFile in SourceFiles)
            {
                string DestinationFilePath = Path.Combine(Destination, SourceFile.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Moving file {0} to destination...", DestinationFilePath);
                if (ShowProgress)
                    TextWriterColor.Write("-> {0}", DestinationFilePath);
                SourceFile.MoveTo(DestinationFilePath);
            }

            // Iterate through every subdirectory and copy them to destination
            foreach (DirectoryInfo SourceDirectory in SourceDirectories)
            {
                string DestinationDirectoryPath = Path.Combine(Destination, SourceDirectory.Name);
                DebugWriter.WriteDebug(DebugLevel.I, "Calling MoveDirectory() with destination {0}...", DestinationDirectoryPath);
                if (ShowProgress)
                    TextWriterColor.Write("* {0}", DestinationDirectoryPath);
                MoveDirectory(SourceDirectory.FullName, DestinationDirectoryPath);

                // Source subdirectories are removed after moving
                Removing.RemoveDirectory(SourceDirectory.FullName);
            }
        }

    }
}
