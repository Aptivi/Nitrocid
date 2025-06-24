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

namespace Nitrocid.Base.Files.Folders
{
    /// <summary>
    /// How are the file system entries sorted in list?
    /// </summary>
    public enum FilesystemSortOptions
    {
        /// <summary>
        /// Sort by full name
        /// </summary>
        FullName = 1,
        /// <summary>
        /// Sort by length
        /// </summary>
        Length,
        /// <summary>
        /// Sort by creation time
        /// </summary>
        CreationTime,
        /// <summary>
        /// Sort by last write time
        /// </summary>
        LastWriteTime,
        /// <summary>
        /// Sort by last access time
        /// </summary>
        LastAccessTime,
        /// <summary>
        /// Sort by file extension
        /// </summary>
        Extension,
        /// <summary>
        /// Sort by the UTC creation time
        /// </summary>
        CreationTimeUtc,
        /// <summary>
        /// Sort by the UTC last write time
        /// </summary>
        LastWriteTimeUtc,
        /// <summary>
        /// Sort by the UTC last access time
        /// </summary>
        LastAccessTimeUtc,
    }
}
