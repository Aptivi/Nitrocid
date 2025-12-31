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
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Kernel.Debugging;

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// Attribute management module
    /// </summary>
    public static partial class FilesystemTools
    {

        /// <summary>
        /// Adds attribute to file
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="Attributes">Attributes</param>
        public static void AddAttributeToFile(string FilePath, FileAttributes Attributes) =>
            DriverHandler.CurrentFilesystemDriverLocal.AddAttributeToFile(FilePath, Attributes);

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
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to add attribute {0} for file {1}: {2}", vars: [Attributes, Path.GetFileName(FilePath), ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Removes attribute
        /// </summary>
        /// <param name="attributes">All attributes</param>
        /// <param name="attributesToRemove">Attributes to remove</param>
        /// <returns>Attributes without target attribute</returns>
        public static FileAttributes RemoveAttribute(this FileAttributes attributes, FileAttributes attributesToRemove) =>
            attributes & ~attributesToRemove;

        /// <summary>
        /// Removes attribute from file
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="Attributes">Attributes</param>
        public static void RemoveAttributeFromFile(string FilePath, FileAttributes Attributes) =>
            DriverHandler.CurrentFilesystemDriverLocal.RemoveAttributeFromFile(FilePath, Attributes);

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
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to remove attribute {0} for file {1}: {2}", vars: [Attributes, Path.GetFileName(FilePath), ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

    }
}
