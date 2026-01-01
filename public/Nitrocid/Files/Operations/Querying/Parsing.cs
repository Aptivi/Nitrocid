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

namespace Nitrocid.Files.Operations.Querying
{
    /// <summary>
    /// File parsing module
    /// </summary>
    public static class Parsing
    {

        /// <summary>
        /// Gets all the invalid path characters
        /// </summary>
        public static char[] GetInvalidPathChars() =>
            DriverHandler.CurrentFilesystemDriverLocal.GetInvalidPathChars();

        /// <summary>
        /// Tries to parse the path (For file names and only names, use <see cref="TryParseFileName(string)"/> instead.)
        /// </summary>
        /// <param name="Path">The path to be parsed</param>
        /// <returns>True if successful; false if unsuccessful</returns>
        public static bool TryParsePath(string Path) =>
            DriverHandler.CurrentFilesystemDriverLocal.TryParsePath(Path);

        /// <summary>
        /// Tries to parse the file name (For full paths, use <see cref="TryParsePath(string)"/> instead.)
        /// </summary>
        /// <param name="Name">The file name to be parsed</param>
        /// <returns>True if successful; false if unsuccessful</returns>
        public static bool TryParseFileName(string Name) =>
            DriverHandler.CurrentFilesystemDriverLocal.TryParseFileName(Name);

        /// <summary>
        /// Is the file a binary file?
        /// </summary>
        /// <param name="Path">Path to file</param>
        public static bool IsBinaryFile(string Path) =>
            DriverHandler.CurrentFilesystemDriverLocal.IsBinaryFile(Path);

        /// <summary>
        /// Is the file a JSON file?
        /// </summary>
        /// <param name="Path">Path to file</param>
        public static bool IsJson(string Path) =>
            DriverHandler.CurrentFilesystemDriverLocal.IsJson(Path);

        /// <summary>
        /// Is the file an SQL file?
        /// </summary>
        /// <param name="Path">Path to file</param>
        public static bool IsSql(string Path) =>
            DriverHandler.CurrentFilesystemDriverLocal.IsSql(Path);

    }
}
