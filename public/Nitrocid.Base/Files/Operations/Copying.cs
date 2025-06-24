//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
    /// Copying file operations module
    /// </summary>
    public static partial class FilesystemTools
    {

        /// <summary>
        /// Copies a file or directory
        /// </summary>
        /// <param name="Source">Source file or directory</param>
        /// <param name="Destination">Target file or directory</param>
        /// <exception cref="IOException"></exception>
        public static void CopyFileOrDir(string Source, string Destination) =>
            DriverHandler.CurrentFilesystemDriverLocal.CopyFileOrDir(Source, Destination);

        /// <summary>
        /// Copies a file or directory
        /// </summary>
        /// <param name="Source">Source file or directory</param>
        /// <param name="Destination">Target file or directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryCopyFileOrDir(string Source, string Destination)
        {
            try
            {
                CopyFileOrDir(Source, Destination);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to copy {0} to {1}: {2}", vars: [Source, Destination, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Copies the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        public static void CopyDirectory(string Source, string Destination) =>
            DriverHandler.CurrentFilesystemDriverLocal.CopyDirectory(Source, Destination);

        /// <summary>
        /// Copies the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryCopyDirectory(string Source, string Destination)
        {
            try
            {
                CopyDirectory(Source, Destination);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to copy {0} to {1}: {2}", vars: [Source, Destination, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Copies the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        /// <param name="ShowProgress">Whether or not to show what files are being copied</param>
        public static void CopyDirectory(string Source, string Destination, bool ShowProgress) =>
            DriverHandler.CurrentFilesystemDriverLocal.CopyDirectory(Source, Destination, ShowProgress);

        /// <summary>
        /// Copies the directory from source to destination
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Target directory</param>
        /// <param name="ShowProgress">Whether or not to show what files are being copied</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryCopyDirectory(string Source, string Destination, bool ShowProgress)
        {
            try
            {
                CopyDirectory(Source, Destination, ShowProgress);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to copy {0} to {1}: {2}", vars: [Source, Destination, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Copies the file from source to destination
        /// </summary>
        /// <param name="Source">Source file</param>
        /// <param name="Destination">Target directory</param>
        public static void CopyFile(string Source, string Destination) =>
            DriverHandler.CurrentFilesystemDriverLocal.CopyFile(Source, Destination);

        /// <summary>
        /// Copies the file from source to destination
        /// </summary>
        /// <param name="Source">Source file</param>
        /// <param name="Destination">Target directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static bool TryCopyFile(string Source, string Destination)
        {
            try
            {
                CopyFile(Source, Destination);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to copy {0} to {1}: {2}", vars: [Source, Destination, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

    }
}
