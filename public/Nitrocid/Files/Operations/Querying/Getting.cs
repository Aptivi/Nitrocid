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
    /// Module related to getting information
    /// </summary>
    public static class Getting
    {
        /// <summary>
        /// Gets the file name with the file number suffix applied
        /// </summary>
        /// <param name="path">Path to the directory that the generated numbered file name will situate</param>
        /// <param name="fileName">The file name with an extension</param>
        /// <returns>Numbered file name with the file number suffix applied in this format: [filename]-[number].[ext]</returns>
        public static string GetNumberedFileName(string? path, string fileName) =>
            DriverHandler.CurrentFilesystemDriverLocal.GetNumberedFileName(path, fileName);

        /// <summary>
        /// Gets the randomized file name
        /// </summary>
        /// <returns>Randomized file name in the temporary directory for your system</returns>
        public static string GetRandomFileName() =>
            DriverHandler.CurrentFilesystemDriverLocal.GetRandomFileName();

        /// <summary>
        /// Gets the randomized folder name
        /// </summary>
        /// <returns>Randomized folder name in the temporary directory for your system</returns>
        public static string GetRandomFolderName() =>
            DriverHandler.CurrentFilesystemDriverLocal.GetRandomFolderName();
    }
}
