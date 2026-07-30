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
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Shells.SFTP.Tools;
using Renci.SshNet;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.SFTP
{
    public partial class SFTPShell : BaseShell, IShell
    {

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="Path">Path to folder</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public List<string> SFTPListRemote(string Path) =>
            SFTPTools.SFTPListRemote(SFTPClient, !string.IsNullOrEmpty(Path) ? Path : SFTPCurrentRemoteDir ?? "");

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="Path">Path to folder</param>
        /// <param name="ShowDetails">Shows the details of the file</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public List<string> SFTPListRemote(string Path, bool ShowDetails) =>
            SFTPTools.SFTPListRemote(SFTPClient, !string.IsNullOrEmpty(Path) ? Path : SFTPCurrentRemoteDir ?? "", ShowDetails);

        /// <summary>
        /// Removes remote file or folder
        /// </summary>
        /// <param name="Target">Target folder or file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool SFTPDeleteRemote(string Target) =>
            SFTPTools.SFTPDeleteRemote(SFTPClient, Target);

        /// <summary>
        /// Changes FTP remote directory
        /// </summary>
        /// <param name="Directory">Remote directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool SFTPChangeRemoteDir(string Directory)
        {
            var client = (SFTPNetwork?.ConnectionInstance) ??
                throw new KernelException(KernelExceptionType.SFTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_2"));
            if (!string.IsNullOrEmpty(Directory))
            {
                if (client.Exists(Directory))
                {
                    // Directory exists, go to the new directory
                    client.ChangeDirectory(Directory);
                    SFTPCurrentRemoteDir = client.WorkingDirectory;
                    return true;
                }
                else
                {
                    // Directory doesn't exist, go to the old directory
                    throw new KernelException(KernelExceptionType.SFTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_REMOTEDIRNOTFOUND"), Directory);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.SFTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_NEEDSREMOTEDIR"));
            }
        }

        /// <summary>
        /// Changes FTP local directory
        /// </summary>
        /// <param name="Directory">Local directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool SFTPChangeLocalDir(string Directory)
        {
            string targetDir;
            targetDir = FilesystemTools.NeutralizePath(Directory, SFTPCurrDirect);

            // Check if folder exists
            if (FilesystemTools.FolderExists(targetDir))
            {
                // Parse written directory
                var parser = new System.IO.DirectoryInfo(targetDir);
                SFTPCurrDirect = parser.FullName;
                return true;
            }
            else
            {
                throw new KernelException(KernelExceptionType.SFTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_LOCALDIRNOTFOUND"), Directory);
            }
        }

        /// <summary>
        /// Gets the absolute path for the given path
        /// </summary>
        /// <param name="Path">The remote path</param>
        /// <returns>Absolute path for a remote path</returns>
        public string SFTPGetCanonicalPath(string Path) =>
            SFTPTools.SFTPGetCanonicalPath(SFTPClient, Path);

        /// <summary>
        /// Makes a directory in the remote
        /// </summary>
        /// <param name="name">New directory name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool SFTPMakeDirectory(string name) =>
            SFTPTools.SFTPMakeDirectory(SFTPClient, name);

        /// <summary>
        /// Checks to see if an SFTP file or directory exists
        /// </summary>
        /// <param name="name">Path to file or directory</param>
        /// <returns>True if found; False otherwise</returns>
        public bool SFTPExists(string name) =>
            SFTPTools.SFTPExists(SFTPClient, name);

        /// <summary>
        /// Checks to see if an SFTP file exists
        /// </summary>
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public bool SFTPFileExists(string name) =>
            SFTPTools.SFTPFileExists(SFTPClient, name);

        /// <summary>
        /// Checks to see if an SFTP directory exists
        /// </summary>
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public bool SFTPDirectoryExists(string name) =>
            SFTPTools.SFTPDirectoryExists(SFTPClient, name);
    }
}
