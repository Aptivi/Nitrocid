
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Shells.SFTP;

namespace KS.Network.SFTP.Transfer
{
    public static class SFTPTransfer
    {

        /// <summary>
        /// Downloads a file from the currently connected SFTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPGetFile(string File)
        {
            if (SFTPShellCommon.SFTPConnected)
            {
                try
                {
                    // Show a message to download
                    Kernel.Kernel.KernelEventManager.RaiseSFTPPreDownload(File);
                    DebugWriter.Wdbg(DebugLevel.I, "Downloading file {0}...", File);

                    // Try to download
                    var DownloadFileStream = new System.IO.FileStream($"{SFTPShellCommon.SFTPCurrDirect}/{File}", System.IO.FileMode.OpenOrCreate);
                    SFTPShellCommon.ClientSFTP.DownloadFile($"{SFTPShellCommon.SFTPCurrentRemoteDir}/{File}", DownloadFileStream);

                    // Show a message that it's downloaded
                    DebugWriter.Wdbg(DebugLevel.I, "Downloaded file {0}.", File);
                    Kernel.Kernel.KernelEventManager.RaiseSFTPPostDownload(File);
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Download failed for file {0}: {1}", File, ex.Message);
                    Kernel.Kernel.KernelEventManager.RaiseSFTPDownloadError(File, ex);
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("You must connect to server before performing transmission."));
            }
            return false;
        }

        /// <summary>
        /// Uploads a file to the currently connected SFTP server
        /// </summary>
        /// <param name="File">A local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPUploadFile(string File)
        {
            if (SFTPShellCommon.SFTPConnected)
            {
                try
                {
                    // Show a message to download
                    Kernel.Kernel.KernelEventManager.RaiseSFTPPreUpload(File);
                    DebugWriter.Wdbg(DebugLevel.I, "Uploading file {0}...", File);

                    // Try to upload
                    var UploadFileStream = new System.IO.FileStream($"{SFTPShellCommon.SFTPCurrDirect}/{File}", System.IO.FileMode.Open);
                    SFTPShellCommon.ClientSFTP.UploadFile(UploadFileStream, $"{SFTPShellCommon.SFTPCurrentRemoteDir}/{File}");
                    DebugWriter.Wdbg(DebugLevel.I, "Uploaded file {0}", File);
                    Kernel.Kernel.KernelEventManager.RaiseSFTPPostUpload(File);
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Upload failed for file {0}: {1}", File, ex.Message);
                    Kernel.Kernel.KernelEventManager.RaiseSFTPUploadError(File, ex);
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("You must connect to server before performing transmission."));
            }
            return false;
        }

    }
}