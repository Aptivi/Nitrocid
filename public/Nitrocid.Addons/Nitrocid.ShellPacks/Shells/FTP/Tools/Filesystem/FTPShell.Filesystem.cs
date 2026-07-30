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
using System.IO;
using FluentFTP;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Shells.FTP.Tools;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.FTP
{
    /// <summary>
    /// FTP filesystem tools module
    /// </summary>
    public partial class FTPShell : BaseShell, IShell
    {

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="Path">Path to folder</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public List<string> FTPListRemote(string Path) =>
            FTPTools.FTPListRemote(FTPClient, !string.IsNullOrEmpty(Path) ? Path : FtpCurrentRemoteDir ?? "", ShellsInit.ShellsConfig.FtpShowDetailsInList);

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="Path">Path to folder</param>
        /// <param name="ShowDetails">Shows the details of the file</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public List<string> FTPListRemote(string Path, bool ShowDetails) =>
            FTPTools.FTPListRemote(FTPClient, !string.IsNullOrEmpty(Path) ? Path : FtpCurrentRemoteDir ?? "", ShowDetails);

        /// <summary>
        /// Removes remote file or folder
        /// </summary>
        /// <param name="Target">Target folder or file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPDeleteRemote(string Target) =>
            FTPTools.FTPDeleteRemote(FTPClient, Target);

        /// <summary>
        /// Changes FTP remote directory
        /// </summary>
        /// <param name="Directory">Remote directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool FTPChangeRemoteDir(string Directory)
        {
            if (!string.IsNullOrEmpty(Directory))
            {
                var instance = (FTPNetwork?.ConnectionInstance) ??
                    throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));
                if (instance.DirectoryExists(Directory))
                {
                    // Directory exists, go to the new directory
                    instance.SetWorkingDirectory(Directory);
                    FtpCurrentRemoteDir = instance.GetWorkingDirectory();
                    return true;
                }
                else
                {
                    // Directory doesn't exist, go to the old directory
                    throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_REMOTEDIRNOTFOUND"), Directory);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_NEEDSREMOTEDIR"));
            }
        }

        /// <summary>
        /// Change the local directory
        /// </summary>
        /// <param name="Directory">Local directory to change to</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool FTPChangeLocalDir(string Directory)
        {
            if (!string.IsNullOrEmpty(Directory))
            {
                string targetDir;
                targetDir = FilesystemTools.NeutralizePath(Directory, FtpCurrentDirectory);

                // Check if folder exists
                if (FilesystemTools.FolderExists(targetDir))
                {
                    // Parse written directory
                    var parser = new DirectoryInfo(targetDir);
                    FtpCurrentDirectory = parser.FullName;
                    return true;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_LOCALDIRNOTFOUND"), Directory);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_NEEDSLOCALDIR"));
            }
        }

        /// <summary>
        /// Move file or directory to another area, or rename the file
        /// </summary>
        /// <param name="Source">Source file or folder</param>
        /// <param name="Target">Target file or folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool FTPMoveItem(string Source, string Target) =>
            FTPTools.FTPMoveItem(FTPClient, Source, Target);

        /// <summary>
        /// Copy file or directory to another area, or rename the file
        /// </summary>
        /// <param name="Source">Source file or folder</param>
        /// <param name="Target">Target file or folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool FTPCopyItem(string Source, string Target) =>
            FTPTools.FTPCopyItem(FTPClient, Source, Target);

        /// <summary>
        /// Changes the permissions of a remote file
        /// </summary>
        /// <param name="Target">Target file</param>
        /// <param name="Chmod">Permissions in CHMOD format. See https://man7.org/linux/man-pages/man2/chmod.2.html chmod(2) for more info.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPChangePermissions(string Target, int Chmod) =>
            FTPTools.FTPChangePermissions(FTPClient, Target, Chmod);

        /// <summary>
        /// Makes a directory in the remote
        /// </summary>
        /// <param name="name">New directory name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPMakeDirectory(string name) =>
            FTPTools.FTPMakeDirectory(FTPClient, name);

        /// <summary>
        /// Checks to see if an FTP file or directory exists
        /// </summary>
        /// <param name="name">Path to file or directory</param>
        /// <returns>True if found; False otherwise</returns>
        public bool FTPExists(string name) =>
            FTPTools.FTPExists(FTPClient, name);

        /// <summary>
        /// Checks to see if an FTP file exists
        /// </summary>
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public bool FTPFileExists(string name) =>
            FTPTools.FTPFileExists(FTPClient, name);

        /// <summary>
        /// Checks to see if an FTP directory exists
        /// </summary>
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public bool FTPDirectoryExists(string name) =>
            FTPTools.FTPDirectoryExists(FTPClient, name);
    }
}
