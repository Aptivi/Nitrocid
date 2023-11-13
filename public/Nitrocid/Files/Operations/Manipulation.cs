﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.Drivers;

namespace KS.Files.Operations
{
    /// <summary>
    /// Miscellaneous file manipulation routines
    /// </summary>
    public static class Manipulation
    {
        /// <summary>
        /// Clears the contents of a file
        /// </summary>
        /// <param name="path">Path to an existing file</param>
        public static void ClearFile(string path) =>
            DriverHandler.CurrentFilesystemDriverLocal.ClearFile(path);

        /// <summary>
        /// Wraps the text file contents with 78 columns per line (depending on the current driver).
        /// </summary>
        /// <param name="path">Path to an existing text file</param>
        public static void WrapTextFile(string path) =>
            DriverHandler.CurrentFilesystemDriverLocal.WrapTextFile(path);

        /// <summary>
        /// Wraps the text file contents with 78 columns per line.
        /// </summary>
        /// <param name="path">Path to an existing text file</param>
        /// <param name="columns">How many columns until wrapping begins?</param>
        public static void WrapTextFile(string path, int columns) =>
            DriverHandler.CurrentFilesystemDriverLocal.WrapTextFile(path, columns);
    }
}