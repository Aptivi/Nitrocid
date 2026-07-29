//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using FluentFTP;
using Nitrocid.ShellPacks.Shells.FTP.Tools;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.FTP
{
    /// <summary>
    /// FTP hashing module
    /// </summary>
    public partial class FTPShell : BaseShell, IShell
    {
        /// <summary>
        /// Gets a hash for file
        /// </summary>
        /// <param name="File">A file to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <returns>The <see cref="FtpHash"/> instance containing computed hash of remote file</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public FtpHash FTPGetHash(string File, FtpHashAlgorithm HashAlgorithm) =>
            FTPTools.FTPGetHash(FTPClient, File, HashAlgorithm);

        /// <summary>
        /// Gets a hash for files in a directory
        /// </summary>
        /// <param name="Directory">A directory for its contents to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<string, FtpHash> FTPGetHashes(string Directory, FtpHashAlgorithm HashAlgorithm) =>
            FTPGetHashes(Directory, HashAlgorithm, ShellsInit.ShellsConfig.FtpRecursiveHashing);

        /// <summary>
        /// Gets a hash for files in a directory
        /// </summary>
        /// <param name="Directory">A directory for its contents to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <param name="Recurse">Whether to hash the files within the subdirectories too.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Dictionary<string, FtpHash> FTPGetHashes(string Directory, FtpHashAlgorithm HashAlgorithm, bool Recurse) =>
            FTPTools.FTPGetHashes(FTPClient, Directory, HashAlgorithm, Recurse);
    }
}
