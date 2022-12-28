
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using System.Collections.Generic;
using KS.Drivers;

namespace KS.Files.PathLookup
{
    /// <summary>
    /// Path lookup tools module
    /// </summary>
    public static class PathLookupTools
    {

        /// <summary>
        /// Gets the lookup path list
        /// </summary>
        public static List<string> GetPathList() =>
            DriverHandler.CurrentFilesystemDriver.GetPathList();

        /// <summary>
        /// Adds a (non-)neutralized path to lookup
        /// </summary>
        public static void AddToPathLookup(string Path) =>
            DriverHandler.CurrentFilesystemDriver.AddToPathLookup(Path);

        /// <summary>
        /// Adds a (non-)neutralized path to lookup
        /// </summary>
        public static void AddToPathLookup(string Path, string RootPath) =>
            DriverHandler.CurrentFilesystemDriver.AddToPathLookup(Path, RootPath);

        /// <summary>
        /// Adds a (non-)neutralized path to lookup
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryAddToPathLookup(string Path)
        {
            try
            {
                AddToPathLookup(Path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a (non-)neutralized path to lookup
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryAddToPathLookup(string Path, string RootPath)
        {
            try
            {
                AddToPathLookup(Path, RootPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an existing (non-)neutralized path from lookup
        /// </summary>
        public static void RemoveFromPathLookup(string Path) =>
            DriverHandler.CurrentFilesystemDriver.RemoveFromPathLookup(Path);

        /// <summary>
        /// Removes an existing (non-)neutralized path from lookup
        /// </summary>
        public static void RemoveFromPathLookup(string Path, string RootPath) =>
            DriverHandler.CurrentFilesystemDriver.RemoveFromPathLookup(Path, RootPath);

        /// <summary>
        /// Removes an existing (non-)neutralized path from lookup
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveFromPathLookup(string Path)
        {
            try
            {
                RemoveFromPathLookup(Path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an existing (non-)neutralized path from lookup
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveFromPathLookup(string Path, string RootPath)
        {
            try
            {
                RemoveFromPathLookup(Path, RootPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the file exists in PATH and writes the result (path to file) to a string variable, if any.
        /// </summary>
        /// <param name="FilePath">A full path to file or just a file name</param>
        /// <param name="Result">The neutralized path</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FileExistsInPath(string FilePath, ref string Result) =>
            DriverHandler.CurrentFilesystemDriver.FileExistsInPath(FilePath, ref Result);

    }
}
