using System;

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

using System.IO;
using KS.Misc.Writers.DebugWriters;

namespace KS.Files
{
    public static class AttributeManager
    {

        /// <summary>
        /// Adds attribute to file
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="Attributes">Attributes</param>
        public static void AddAttributeToFile(string FilePath, FileAttributes Attributes)
        {
            Filesystem.ThrowOnInvalidPath(FilePath);
            FilePath = Filesystem.NeutralizePath(FilePath);
            DebugWriter.Wdbg(DebugLevel.I, "Setting file attribute to {0}...", Attributes);
            File.SetAttributes(FilePath, Attributes);

            // Raise event
            Kernel.Kernel.KernelEventManager.RaiseFileAttributeAdded(FilePath, Attributes);
        }

        /// <summary>
        /// Adds attribute to file
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="Attributes">Attributes</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryAddAttributeToFile(string FilePath, FileAttributes Attributes)
        {
            try
            {
                AddAttributeToFile(FilePath, Attributes);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to add attribute {0} for file {1}: {2}", Attributes, Path.GetFileName(FilePath), ex.Message);
                DebugWriter.WStkTrc(ex);
            }
            return false;
        }

        /// <summary>
        /// Removes attribute
        /// </summary>
        /// <param name="attributes">All attributes</param>
        /// <param name="attributesToRemove">Attributes to remove</param>
        /// <returns>Attributes without target attribute</returns>
        public static FileAttributes RemoveAttribute(this FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }

        /// <summary>
        /// Removes attribute from file
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="Attributes">Attributes</param>
        public static void RemoveAttributeFromFile(string FilePath, FileAttributes Attributes)
        {
            Filesystem.ThrowOnInvalidPath(FilePath);
            FilePath = Filesystem.NeutralizePath(FilePath);
            var Attrib = File.GetAttributes(FilePath);
            DebugWriter.Wdbg(DebugLevel.I, "File attributes: {0}", Attrib);
            Attrib = Attrib.RemoveAttribute(Attributes);
            DebugWriter.Wdbg(DebugLevel.I, "Setting file attribute to {0}...", Attrib);
            File.SetAttributes(FilePath, Attrib);

            // Raise event
            Kernel.Kernel.KernelEventManager.RaiseFileAttributeRemoved(FilePath, Attributes);
        }

        /// <summary>
        /// Removes attribute from file
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="Attributes">Attributes</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveAttributeFromFile(string FilePath, FileAttributes Attributes)
        {
            try
            {
                RemoveAttributeFromFile(FilePath, Attributes);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to remove attribute {0} for file {1}: {2}", Attributes, Path.GetFileName(FilePath), ex.Message);
                DebugWriter.WStkTrc(ex);
            }
            return false;
        }

    }
}