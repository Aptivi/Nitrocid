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
using System.IO;
using Nitrocid.Drivers;

namespace Nitrocid.Files
{
    /// <summary>
    /// Making file operations module
    /// </summary>
    public static partial class FilesystemTools
    {

        /// <summary>
        /// Makes a directory
        /// </summary>
        /// <param name="NewDirectory">New directory</param>
        /// <param name="ThrowIfDirectoryExists">If directory exists, throw an exception.</param>
        /// <exception cref="IOException"></exception>
        public static void MakeDirectory(string NewDirectory, bool ThrowIfDirectoryExists = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.MakeDirectory(NewDirectory, ThrowIfDirectoryExists);

        /// <summary>
        /// Makes a directory
        /// </summary>
        /// <param name="NewDirectory">New directory</param>
        /// <param name="ThrowIfDirectoryExists">If directory exists, throw an exception.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryMakeDirectory(string NewDirectory, bool ThrowIfDirectoryExists = true)
        {
            try
            {
                MakeDirectory(NewDirectory, ThrowIfDirectoryExists);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Makes a file
        /// </summary>
        /// <param name="NewFile">New file</param>
        /// <param name="ThrowIfFileExists">If file exists, throw an exception.</param>
        /// <exception cref="IOException"></exception>
        public static void MakeFile(string NewFile, bool ThrowIfFileExists = true) =>
            DriverHandler.CurrentFilesystemDriverLocal.MakeFile(NewFile, ThrowIfFileExists);

        /// <summary>
        /// Makes a file
        /// </summary>
        /// <param name="NewFile">New file</param>
        /// <param name="ThrowIfFileExists">If file exists, throw an exception.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryMakeFile(string NewFile, bool ThrowIfFileExists = true)
        {
            try
            {
                MakeFile(NewFile, ThrowIfFileExists);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Makes an empty JSON file
        /// </summary>
        /// <param name="NewFile">New JSON file</param>
        /// <param name="ThrowIfFileExists">If file exists, throw an exception.</param>
        /// <param name="useArray">Use array instead of object</param>
        /// <exception cref="IOException"></exception>
        public static void MakeJsonFile(string NewFile, bool ThrowIfFileExists = true, bool useArray = false) =>
            DriverHandler.CurrentFilesystemDriverLocal.MakeJsonFile(NewFile, ThrowIfFileExists, useArray);

        /// <summary>
        /// Makes an empty JSON file
        /// </summary>
        /// <param name="NewFile">New JSON file</param>
        /// <param name="ThrowIfFileExists">If file exists, throw an exception.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryMakeJsonFile(string NewFile, bool ThrowIfFileExists = true)
        {
            try
            {
                MakeJsonFile(NewFile, ThrowIfFileExists);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Makes a symbolic link
        /// </summary>
        /// <param name="linkName">New symbolic link</param>
        /// <param name="target">Target file to link to</param>
        /// <exception cref="IOException"></exception>
        public static void MakeSymlink(string linkName, string target) =>
            DriverHandler.CurrentFilesystemDriverLocal.MakeSymlink(linkName, target);

        /// <summary>
        /// Makes a symbolic link
        /// </summary>
        /// <param name="linkName">New symbolic link</param>
        /// <param name="target">Target file to link to</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryMakeSymlink(string linkName, string target)
        {
            try
            {
                MakeSymlink(linkName, target);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Makes a randomized file name
        /// </summary>
        public static void MakeRandomFile() =>
            MakeFile(FilesystemTools.GetRandomFileName());

        /// <summary>
        /// Makes a randomized folder name
        /// </summary>
        public static void MakeRandomFolder() =>
            MakeDirectory(FilesystemTools.GetRandomFolderName());

        /// <summary>
        /// Makes a randomized JSON file name
        /// </summary>
        public static void MakeRandomJsonFile() =>
            MakeJsonFile(FilesystemTools.GetRandomFileName());

    }
}
