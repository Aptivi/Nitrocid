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

using System.IO;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Kernel.Configuration;

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// Size getter module
    /// </summary>
    public static partial class FilesystemTools
    {

        /// <summary>
        /// Gets all file sizes in a folder, depending on the kernel setting <see cref="Config.MainConfig"/>.FullParseMode
        /// </summary>
        /// <param name="DirectoryInfo">Directory information</param>
        /// <returns>Directory Size</returns>
        public static long GetAllSizesInFolder(DirectoryInfo? DirectoryInfo) =>
            DriverHandler.CurrentFilesystemDriverLocal.GetAllSizesInFolder(DirectoryInfo, Config.MainConfig.FullParseMode);

        /// <summary>
        /// Gets all file sizes in a folder, and optionally parses the entire folder
        /// </summary>
        /// <param name="DirectoryInfo">Directory information</param>
        /// <param name="FullParseMode">Whether to parse all the directories</param>
        /// <returns>Directory Size</returns>
        public static long GetAllSizesInFolder(DirectoryInfo? DirectoryInfo, bool FullParseMode) =>
            DriverHandler.CurrentFilesystemDriverLocal.GetAllSizesInFolder(DirectoryInfo, FullParseMode);

    }
}
