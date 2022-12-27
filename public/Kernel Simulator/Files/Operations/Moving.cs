
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
using KS.Drivers;
using KS.Kernel.Debugging;

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
        public static void MoveFileOrDir(string Source, string Destination) =>
            DriverHandler.CurrentFilesystemDriver.MoveFileOrDir(Source, Destination);

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
        public static void MoveDirectory(string Source, string Destination) =>
            DriverHandler.CurrentFilesystemDriver.MoveDirectory(Source, Destination);

        /// <summary>
        /// Moves the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        /// <param name="ShowProgress">Whether or not to show what files are being moved</param>
        public static void MoveDirectory(string Source, string Destination, bool ShowProgress) =>
            DriverHandler.CurrentFilesystemDriver.MoveDirectory(Source, Destination, ShowProgress);

    }
}
