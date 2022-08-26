
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
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;

namespace KS.Files.Operations
{
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
            DebugWriter.Wdbg(DebugLevel.I, "Source directory: {0}", Source);
            Destination = Filesystem.NeutralizePath(Destination);
            DebugWriter.Wdbg(DebugLevel.I, "Target directory: {0}", Destination);
            string FileName = Path.GetFileName(Source);
            DebugWriter.Wdbg(DebugLevel.I, "Source file name: {0}", FileName);
            if (Checking.FolderExists(Source))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Source and destination are directories");
                Directory.Move(Source, Destination);

                // Raise event
                Kernel.Kernel.KernelEventManager.RaiseDirectoryMoved(Source, Destination);
            }
            else if (Checking.FileExists(Source) & Checking.FolderExists(Destination))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Source is a file and destination is a directory");
                File.Move(Source, Destination + "/" + FileName);

                // Raise event
                Kernel.Kernel.KernelEventManager.RaiseFileMoved(Source, Destination + "/" + FileName);
            }
            else if (Checking.FileExists(Source))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Source is a file and destination is a file");
                File.Move(Source, Destination);

                // Raise event
                Kernel.Kernel.KernelEventManager.RaiseFileMoved(Source, Destination);
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.E, "Source or destination are invalid.");
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
                DebugWriter.Wdbg(DebugLevel.E, "Failed to move {0} to {1}: {2}", Source, Destination, ex.Message);
                DebugWriter.WStkTrc(ex);
            }
            return false;
        }

    }
}