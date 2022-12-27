
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

using KS.Drivers;

namespace KS.Files.Read
{
    /// <summary>
    /// File reading module
    /// </summary>
    public static class FileRead
    {

        /// <summary>
        /// Reads the contents of a file and writes it to the array. This is blocking and will put a lock on the file until read.
        /// </summary>
        /// <param name="filename">Full path to file</param>
        /// <returns>An array full of file contents</returns>
        public static string[] ReadContents(string filename) =>
            DriverHandler.CurrentFilesystemDriver.ReadContents(filename);

        /// <summary>
        /// Opens a file, reads all lines, and returns the array of lines
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Array of lines</returns>
        public static string[] ReadAllLinesNoBlock(string path) =>
            DriverHandler.CurrentFilesystemDriver.ReadAllLinesNoBlock(path);

        /// <summary>
        /// Reads all the bytes
        /// </summary>
        /// <param name="path">Path to the file</param>
        public static byte[] ReadAllBytes(string path) =>
            DriverHandler.CurrentFilesystemDriver.ReadAllBytes(path);

    }
}
