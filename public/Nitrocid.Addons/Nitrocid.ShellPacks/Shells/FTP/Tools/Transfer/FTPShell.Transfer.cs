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
using System.Text;
using FluentFTP;
using FluentFTP.Helpers;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Shells.FTP.Tools;
using Nitrocid.ShellPacks.Shells.FTP.Tools.Transfer;
using Nitrocid.ShellPacks.Shells.SFTP.Tools;
using Renci.SshNet;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.FTP
{
    /// <summary>
    /// FTP transfer class
    /// </summary>
    public partial class FTPShell : BaseShell, IShell
    {

        /// <summary>
        /// Downloads a file from the currently connected FTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPGetFile(string File) =>
            FTPGetFile(File, File);

        /// <summary>
        /// Downloads a file from the currently connected FTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPGetFile(string File, string LocalFile)
        {
            string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile, FtpCurrentDirectory);
            return FTPTools.FTPGetFile(FTPClient, File, LocalFilePath);
        }

        /// <summary>
        /// Downloads a folder from the currently connected FTP server
        /// </summary>
        /// <param name="Folder">A remote folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPGetFolder(string Folder) =>
            FTPGetFolder(Folder, "");

        /// <summary>
        /// Downloads a folder from the currently connected FTP server
        /// </summary>
        /// <param name="Folder">A remote folder</param>
        /// <param name="LocalFolder">A local folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPGetFolder(string Folder, string LocalFolder)
        {
            string LocalFilePath = FilesystemTools.NeutralizePath(LocalFolder, FtpCurrentDirectory);
            return FTPTools.FTPGetFolder(FTPClient, Folder, LocalFilePath);
        }

        /// <summary>
        /// Uploads a file to the currently connected FTP server
        /// </summary>
        /// <param name="File">A local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPUploadFile(string File) =>
            FTPUploadFile(File, File);

        /// <summary>
        /// Uploads a file to the currently connected FTP server
        /// </summary>
        /// <param name="File">A local file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPUploadFile(string File, string LocalFile)
        {
            string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile, FtpCurrentDirectory);
            return FTPTools.FTPUploadFile(FTPClient, File, LocalFilePath);
        }

        /// <summary>
        /// Uploads a folder to the currently connected FTP server
        /// </summary>
        /// <param name="Folder">A local folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPUploadFolder(string Folder) =>
            FTPUploadFolder(Folder, Folder);

        /// <summary>
        /// Uploads a folder to the currently connected FTP server
        /// </summary>
        /// <param name="Folder">A remote folder</param>
        /// <param name="LocalFolder">A local folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool FTPUploadFolder(string Folder, string LocalFolder)
        {
            string LocalFilePath = FilesystemTools.NeutralizePath(LocalFolder, FtpCurrentDirectory);
            return FTPTools.FTPUploadFolder(FTPClient, Folder, LocalFilePath);
        }

        /// <summary>
        /// Downloads a file to string
        /// </summary>
        /// <param name="File">A text file.</param>
        /// <returns>Contents of the file</returns>
        public string FTPDownloadToString(string File) =>
            FTPTools.FTPDownloadToString(FTPClient, File);
    }
}
