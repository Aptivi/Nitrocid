
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
using KS.Misc.Writers.DebugWriters;

namespace KS.Files.Operations
{
    public static class Removing
    {

        /// <summary>
        /// Removes a directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        public static void RemoveDirectory(string Target)
        {
            Filesystem.ThrowOnInvalidPath(Target);
            string Dir = Filesystem.NeutralizePath(Target);
            Directory.Delete(Dir, true);

            // Raise event
            Kernel.Kernel.KernelEventManager.RaiseDirectoryRemoved(Target);
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
                DebugWriter.WStkTrc(ex);
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
            Kernel.Kernel.KernelEventManager.RaiseFileRemoved(Target);
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
                DebugWriter.WStkTrc(ex);
            }
            return false;
        }

    }
}