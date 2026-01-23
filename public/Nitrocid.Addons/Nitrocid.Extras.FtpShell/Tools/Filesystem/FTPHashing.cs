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
using System.Collections.Generic;
using FluentFTP;
using Nitrocid.Extras.FtpShell.FTP;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Extras.FtpShell.Tools.Filesystem
{
    /// <summary>
    /// FTP hashing module
    /// </summary>
    public static class FTPHashing
    {

        /// <summary>
        /// Gets a hash for file
        /// </summary>
        /// <param name="File">A file to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <returns>The <see cref="FtpHash"/> instance containing computed hash of remote file</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static FtpHash FTPGetHash(string File, FtpHashAlgorithm HashAlgorithm)
        {
            if (!string.IsNullOrEmpty(File))
            {
                var client = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPNetwork, Translate.DoTranslation("Connection is not established yet."));
                if (client.FileExists(File))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Hashing {0} using {1}...", vars: [File, HashAlgorithm.ToString()]);
                    return client.GetChecksum(File, HashAlgorithm);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not found.", vars: [File]);
                    throw new KernelException(KernelExceptionType.FTPFilesystem, Translate.DoTranslation("{0} is not found in the server."), File);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.FTPNetwork, Translate.DoTranslation("Enter a remote file to be hashed."));
            }
        }

        /// <summary>
        /// Gets a hash for files in a directory
        /// </summary>
        /// <param name="Directory">A directory for its contents to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static Dictionary<string, FtpHash> FTPGetHashes(string Directory, FtpHashAlgorithm HashAlgorithm) => FTPGetHashes(Directory, HashAlgorithm, FTPShellCommon.FtpRecursiveHashing);

        /// <summary>
        /// Gets a hash for files in a directory
        /// </summary>
        /// <param name="Directory">A directory for its contents to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <param name="Recurse">Whether to hash the files within the subdirectories too.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static Dictionary<string, FtpHash> FTPGetHashes(string Directory, FtpHashAlgorithm HashAlgorithm, bool Recurse)
        {
            if (!string.IsNullOrEmpty(Directory))
            {
                var client = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPNetwork, Translate.DoTranslation("Connection is not established yet."));
                if (client.DirectoryExists(Directory))
                {
                    var Hashes = new Dictionary<string, FtpHash>();
                    FtpListItem[] Items;
                    if (Recurse)
                    {
                        Items = client.GetListing(Directory, FtpListOption.Recursive);
                    }
                    else
                    {
                        Items = client.GetListing(Directory);
                    }
                    foreach (FtpListItem Item in Items)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Hashing {0} using {1}...", vars: [Item.FullName, HashAlgorithm.ToString()]);
                        Hashes.Add(Item.FullName, FTPGetHash(Item.FullName, HashAlgorithm));
                    }
                    return Hashes;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not found.", vars: [Directory]);
                    throw new KernelException(KernelExceptionType.FTPFilesystem, Translate.DoTranslation("{0} is not found in the server."), Directory);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.FTPNetwork, Translate.DoTranslation("Enter a remote directory."));
            }
        }

    }
}
