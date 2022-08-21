
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
using System.Collections.Generic;
using System.Linq;
using KS.Files.Querying;

namespace KS.Files.PathLookup
{
    public static class PathLookupTools
    {

        /// <summary>
        /// Gets the lookup path list
        /// </summary>
        public static List<string> GetPathList()
        {
            return Shell.Shell.PathsToLookup.Split(Convert.ToChar(Shell.Shell.PathLookupDelimiter)).ToList();
        }

        /// <summary>
        /// Adds a (non-)neutralized path to lookup
        /// </summary>
        public static void AddToPathLookup(string Path)
        {
            Filesystem.ThrowOnInvalidPath(Path);
            var LookupPaths = GetPathList();
            Path = Filesystem.NeutralizePath(Path);
            LookupPaths.Add(Path);
            Shell.Shell.PathsToLookup = string.Join(Shell.Shell.PathLookupDelimiter, LookupPaths);
        }

        /// <summary>
        /// Adds a (non-)neutralized path to lookup
        /// </summary>
        public static void AddToPathLookup(string Path, string RootPath)
        {
            Filesystem.ThrowOnInvalidPath(Path);
            Filesystem.ThrowOnInvalidPath(RootPath);
            var LookupPaths = GetPathList();
            Path = Filesystem.NeutralizePath(Path, RootPath);
            LookupPaths.Add(Path);
            Shell.Shell.PathsToLookup = string.Join(Shell.Shell.PathLookupDelimiter, LookupPaths);
        }

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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an existing (non-)neutralized path from lookup
        /// </summary>
        public static void RemoveFromPathLookup(string Path)
        {
            Filesystem.ThrowOnInvalidPath(Path);
            var LookupPaths = GetPathList();
            Path = Filesystem.NeutralizePath(Path);
            LookupPaths.Remove(Path);
            Shell.Shell.PathsToLookup = string.Join(Shell.Shell.PathLookupDelimiter, LookupPaths);
        }

        /// <summary>
        /// Removes an existing (non-)neutralized path from lookup
        /// </summary>
        public static void RemoveFromPathLookup(string Path, string RootPath)
        {
            Filesystem.ThrowOnInvalidPath(Path);
            Filesystem.ThrowOnInvalidPath(RootPath);
            var LookupPaths = GetPathList();
            Path = Filesystem.NeutralizePath(Path, RootPath);
            LookupPaths.Remove(Path);
            Shell.Shell.PathsToLookup = string.Join(Shell.Shell.PathLookupDelimiter, LookupPaths);
        }

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
            catch (Exception ex)
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
            catch (Exception ex)
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
        public static bool FileExistsInPath(string FilePath, ref string Result)
        {
            Filesystem.ThrowOnInvalidPath(FilePath);
            var LookupPaths = GetPathList();
            string ResultingPath;
            foreach (string LookupPath in LookupPaths)
            {
                ResultingPath = Filesystem.NeutralizePath(FilePath, LookupPath);
                if (Checking.FileExists(ResultingPath))
                {
                    Result = ResultingPath;
                    return true;
                }
            }
            return false;
        }

    }
}