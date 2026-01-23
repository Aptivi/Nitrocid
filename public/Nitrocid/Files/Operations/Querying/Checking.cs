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

using Nitrocid.Drivers;

namespace Nitrocid.Files
{
    /// <summary>
    /// File checking module
    /// </summary>
    public static partial class FilesystemTools
    {

        /// <summary>
        /// Checks to see if the file exists. Windows 10/11 bug aware.
        /// </summary>
        /// <param name="File">Target file</param>
        /// <param name="Neutralize">Whether to neutralize the path</param>
        /// <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        public static bool FileExists(string File, bool Neutralize = false) =>
            DriverHandler.CurrentFilesystemDriverLocal.FileExists(File, Neutralize);

        /// <summary>
        /// Checks to see if the folder exists. Windows 10/11 bug aware.
        /// </summary>
        /// <param name="Folder">Target folder</param>
        /// <param name="Neutralize">Whether to neutralize the path</param>
        /// <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        public static bool FolderExists(string Folder, bool Neutralize = false) =>
            DriverHandler.CurrentFilesystemDriverLocal.FolderExists(Folder, Neutralize);

        /// <summary>
        /// Checks to see if the file or the folder exists. Windows 10/11 bug aware.
        /// </summary>
        /// <param name="Path">Target path</param>
        /// <param name="Neutralize">Whether to neutralize the path</param>
        /// <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        public static bool Exists(string Path, bool Neutralize = false) =>
            DriverHandler.CurrentFilesystemDriverLocal.Exists(Path, Neutralize);

        /// <summary>
        /// Checks to see if the file or the folder contains the root path or not. Windows 10/11 bug aware.
        /// </summary>
        /// <param name="Path">Target path</param>
        /// <returns>True if rooted; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        public static bool Rooted(string Path) =>
            DriverHandler.CurrentFilesystemDriverLocal.Rooted(Path);

    }
}
