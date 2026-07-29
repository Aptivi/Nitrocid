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

using Nitrocid.Base.Files;
using Nitrocid.ShellPacks.Shells.SFTP.Tools;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.SFTP
{
    /// <summary>
    /// SFTP transfer module
    /// </summary>
    public partial class SFTPShell : BaseShell, IShell
    {
        /// <summary>
        /// Downloads a file from the currently connected SFTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool SFTPGetFile(string File) =>
            SFTPGetFile(File, File);

        /// <summary>
        /// Downloads a file from the currently connected SFTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool SFTPGetFile(string File, string LocalFile)
        {
            string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile, SFTPCurrDirect);
            return SFTPTools.SFTPGetFile(SFTPClient, File, LocalFilePath);
        }

        /// <summary>
        /// Uploads a file to the currently connected SFTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool SFTPUploadFile(string File) =>
            SFTPUploadFile(File, File);

        /// <summary>
        /// Uploads a file to the currently connected SFTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool SFTPUploadFile(string File, string LocalFile)
        {
            string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile, SFTPCurrDirect);
            return SFTPTools.SFTPUploadFile(SFTPClient, File, LocalFilePath);
        }

        /// <summary>
        /// Downloads a file to string
        /// </summary>
        /// <param name="File">A text file.</param>
        /// <returns>Contents of the file</returns>
        public string SFTPDownloadToString(string File) =>
            SFTPTools.SFTPDownloadToString(SFTPClient, File);
    }
}
