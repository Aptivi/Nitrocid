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

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Nitrocid.Drivers;

namespace Nitrocid.Files.Operations.Querying
{
    /// <summary>
    /// File searching module
    /// </summary>
    public static class Searching
    {

        /// <summary>
        /// Searches a file for string
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="StringLookup">String to find</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static List<string> SearchFileForString(string FilePath, string StringLookup) =>
            DriverHandler.CurrentFilesystemDriverLocal.SearchFileForString(FilePath, StringLookup);

        /// <summary>
        /// Searches a file for string using regexp
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="StringLookup">String to find</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="IOException"></exception>
        public static List<string> SearchFileForStringRegexp(string FilePath, Regex StringLookup) =>
            DriverHandler.CurrentFilesystemDriverLocal.SearchFileForStringRegexp(FilePath, StringLookup);

        /// <summary>
        /// Searches a file for string using regexp
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="StringLookup">String to find</param>
        /// <returns>The list of match collections with their associated line of text</returns>
        /// <exception cref="IOException"></exception>
        public static List<(string, MatchCollection)> SearchFileForStringRegexpMatches(string FilePath, Regex StringLookup) =>
            DriverHandler.CurrentFilesystemDriverLocal.SearchFileForStringRegexpMatches(FilePath, StringLookup);

    }
}
