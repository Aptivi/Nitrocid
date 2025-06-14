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

using System;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Events;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.ShellPacks.Shells.SFTP;
using Renci.SshNet;

namespace Nitrocid.ShellPacks.Tools.Transfer
{
    /// <summary>
    /// SFTP transfer module
    /// </summary>
    public static class SFTPTransfer
    {

        /// <summary>
        /// Downloads a file from the currently connected SFTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPGetFile(string File) => SFTPGetFile(File, File);

        /// <summary>
        /// Downloads a file from the currently connected SFTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPGetFile(string File, string LocalFile)
        {
            try
            {
                var client = (SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.SFTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_2", "Nitrocid.ShellPacks"));

                // Show a message to download
                EventsManager.FireEvent(EventType.SFTPPreDownload, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading file {0}...", vars: [File]);

                // Try to download
                string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile, SFTPShellCommon.SFTPCurrDirect);
                var DownloadFileStream = new System.IO.FileStream(LocalFilePath, System.IO.FileMode.OpenOrCreate);
                client.DownloadFile(File, DownloadFileStream);

                // Show a message that it's downloaded
                DebugWriter.WriteDebug(DebugLevel.I, "Downloaded file {0}.", vars: [File]);
                EventsManager.FireEvent(EventType.SFTPPostDownload, File);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Download failed for file {0}: {1}", vars: [File, ex.Message]);
                EventsManager.FireEvent(EventType.SFTPDownloadError, File, ex);
            }
            return false;
        }

        /// <summary>
        /// Uploads a file to the currently connected SFTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPUploadFile(string File) => SFTPUploadFile(File, File);

        /// <summary>
        /// Uploads a file to the currently connected SFTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPUploadFile(string File, string LocalFile)
        {
            try
            {
                var client = (SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.SFTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_2", "Nitrocid.ShellPacks"));

                // Show a message to download
                EventsManager.FireEvent(EventType.SFTPPreUpload, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Uploading file {0}...", vars: [File]);

                // Try to upload
                string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile, SFTPShellCommon.SFTPCurrDirect);
                var UploadFileStream = new System.IO.FileStream(LocalFilePath, System.IO.FileMode.Open);
                client.UploadFile(UploadFileStream, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Uploaded file {0}", vars: [File]);
                EventsManager.FireEvent(EventType.SFTPPostUpload, File);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Upload failed for file {0}: {1}", vars: [File, ex.Message]);
                EventsManager.FireEvent(EventType.SFTPUploadError, File, ex);
            }
            return false;
        }

        /// <summary>
        /// Downloads a file to string
        /// </summary>
        /// <param name="File">A text file.</param>
        /// <returns>Contents of the file</returns>
        public static string SFTPDownloadToString(string File)
        {
            try
            {
                var client = (SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.SFTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_2", "Nitrocid.ShellPacks"));

                // Show a message to download
                EventsManager.FireEvent(EventType.SFTPPreDownload, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading {0}...", vars: [File]);

                // Try to download 3 times
                var DownloadedBytes = Array.Empty<byte>();
                string DownloadedContent = client.ReadAllText(File);

                // Show a message that it's downloaded
                DebugWriter.WriteDebug(DebugLevel.I, "Downloaded {0}.", vars: [File]);
                EventsManager.FireEvent(EventType.SFTPPostDownload, File, DownloadedContent);
                return DownloadedContent;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Download failed for {0}: {1}", vars: [File, ex.Message]);
                EventsManager.FireEvent(EventType.SFTPPostDownload, File, false);
            }
            return "";
        }

    }
}
