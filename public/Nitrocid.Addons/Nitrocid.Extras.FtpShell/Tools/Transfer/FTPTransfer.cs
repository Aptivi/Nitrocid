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
using System.Text;
using FluentFTP;
using FluentFTP.Helpers;
using Nitrocid.Extras.FtpShell.FTP;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Events;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Extras.FtpShell.Tools.Transfer
{
    /// <summary>
    /// FTP transfer class
    /// </summary>
    public static class FTPTransfer
    {

        // Progress Bar Enabled
        internal static bool progressFlag = true;
        internal static int ConsoleOriginalPositionLeft;
        internal static int ConsoleOriginalPositionTop;

        /// <summary>
        /// Downloads a file from the currently connected FTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPGetFile(string File) => FTPGetFile(File, File);

        /// <summary>
        /// Downloads a file from the currently connected FTP server
        /// </summary>
        /// <param name="File">A remote file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPGetFile(string File, string LocalFile)
        {
            try
            {
                var client = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPNetwork, Translate.DoTranslation("Connection is not established yet."));

                // Show a message to download
                EventsManager.FireEvent(EventType.FTPPreDownload, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading file {0}...", vars: [File]);

                // Try to download 3 times
                string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile, FTPShellCommon.FtpCurrentDirectory);
                var Result = client.DownloadFile(LocalFilePath, File, FtpLocalExists.Resume, (FtpVerify)((int)FtpVerify.Retry + (int)FtpVerify.Throw), FTPTransferProgress.FileProgress);

                // Show a message that it's downloaded
                DebugWriter.WriteDebug(DebugLevel.I, "Downloaded file {0}.", vars: [File]);
                EventsManager.FireEvent(EventType.FTPPostDownload, File, Result.IsSuccess());
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Download failed for file {0}: {1}", vars: [File, ex.Message]);
                EventsManager.FireEvent(EventType.FTPPostDownload, File, false);
            }
            return false;
        }

        /// <summary>
        /// Downloads a folder from the currently connected FTP server
        /// </summary>
        /// <param name="Folder">A remote folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPGetFolder(string Folder) => FTPGetFolder(Folder, "");

        /// <summary>
        /// Downloads a folder from the currently connected FTP server
        /// </summary>
        /// <param name="Folder">A remote folder</param>
        /// <param name="LocalFolder">A local folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPGetFolder(string Folder, string LocalFolder)
        {
            try
            {
                var client = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPNetwork, Translate.DoTranslation("Connection is not established yet."));

                // Show a message to download
                EventsManager.FireEvent(EventType.FTPPreDownload, Folder);
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading folder {0}...", vars: [Folder]);

                // Try to download folder
                string LocalFolderPath = FilesystemTools.NeutralizePath(LocalFolder, FTPShellCommon.FtpCurrentDirectory);
                var Results = client.DownloadDirectory(LocalFolderPath, Folder, FtpFolderSyncMode.Update, FtpLocalExists.Resume, (FtpVerify)((int)FtpVerify.Retry + (int)FtpVerify.Throw), null, FTPTransferProgress.MultipleProgress);

                // Print download results to debugger
                var Failed = false;
                DebugWriter.WriteDebug(DebugLevel.I, "Folder download result:");
                foreach (FtpResult Result in Results)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "-- {0} --", vars: [Result.Name]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Success: {0}", vars: [Result.IsSuccess]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Skipped: {0}", vars: [Result.IsSkipped]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Failure: {0}", vars: [Result.IsFailed]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Size: {0}", vars: [Result.Size]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Type: {0}", vars: [Result.Type]);
                    if (Result.IsFailed)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Download failed for {0}", vars: [Result.Name]);

                        // Download could fail with no exception in very rare cases.
                        if (Result.Exception is not null)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Exception {0}", vars: [Result.Exception.Message]);
                            DebugWriter.WriteDebugStackTrace(Result.Exception);
                        }
                        Failed = true;
                    }
                    EventsManager.FireEvent(EventType.FTPPostDownload, Result.Name, !Failed);
                }

                // Show a message that it's downloaded
                if (!Failed)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Downloaded folder {0}.", vars: [Folder]);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Downloaded folder {0} partially due to failure.", vars: [Folder]);
                }
                EventsManager.FireEvent(EventType.FTPPostDownload, Folder, !Failed);
                return !Failed;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Download failed for folder {0}: {1}", vars: [Folder, ex.Message]);
                EventsManager.FireEvent(EventType.FTPPostDownload, Folder, false);
            }
            return false;
        }

        /// <summary>
        /// Uploads a file to the currently connected FTP server
        /// </summary>
        /// <param name="File">A local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPUploadFile(string File) => FTPUploadFile(File, File);

        /// <summary>
        /// Uploads a file to the currently connected FTP server
        /// </summary>
        /// <param name="File">A local file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPUploadFile(string File, string LocalFile)
        {
            var client = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.FTPNetwork, Translate.DoTranslation("Connection is not established yet."));

            // Show a message to download
            EventsManager.FireEvent(EventType.FTPPreUpload, File);
            DebugWriter.WriteDebug(DebugLevel.I, "Uploading file {0}...", vars: [LocalFile]);
            DebugWriter.WriteDebug(DebugLevel.I, "Where in the remote: {0}", vars: [File]);

            // Try to upload
            string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile, FTPShellCommon.FtpCurrentDirectory);
            bool Success = Convert.ToBoolean(client.UploadFile(LocalFilePath, File, FtpRemoteExists.Resume, true, FtpVerify.Retry, FTPTransferProgress.FileProgress));
            DebugWriter.WriteDebug(DebugLevel.I, "Uploaded file {0} to {1} with status {2}.", vars: [LocalFile, File, Success]);
            EventsManager.FireEvent(EventType.FTPPostUpload, File, Success);
            return Success;
        }

        /// <summary>
        /// Uploads a folder to the currently connected FTP server
        /// </summary>
        /// <param name="Folder">A local folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPUploadFolder(string Folder) => FTPUploadFolder(Folder, Folder);

        /// <summary>
        /// Uploads a folder to the currently connected FTP server
        /// </summary>
        /// <param name="Folder">A remote folder</param>
        /// <param name="LocalFolder">A local folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPUploadFolder(string Folder, string LocalFolder)
        {
            var client = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.FTPNetwork, Translate.DoTranslation("Connection is not established yet."));

            // Show a message to download
            EventsManager.FireEvent(EventType.FTPPreUpload, Folder);
            DebugWriter.WriteDebug(DebugLevel.I, "Uploading folder {0}...", vars: [Folder]);

            // Try to upload
            string LocalFolderPath = FilesystemTools.NeutralizePath(LocalFolder, FTPShellCommon.FtpCurrentDirectory);
            var Results = client.UploadDirectory(LocalFolderPath, Folder, FtpFolderSyncMode.Update, FtpRemoteExists.Resume, FtpVerify.Retry, null, FTPTransferProgress.MultipleProgress);

            // Print upload results to debugger
            var Failed = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Folder upload result:");
            foreach (FtpResult Result in Results)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "-- {0} --", vars: [Result.Name]);
                DebugWriter.WriteDebug(DebugLevel.I, "Success: {0}", vars: [Result.IsSuccess]);
                DebugWriter.WriteDebug(DebugLevel.I, "Skipped: {0}", vars: [Result.IsSkipped]);
                DebugWriter.WriteDebug(DebugLevel.I, "Failure: {0}", vars: [Result.IsFailed]);
                DebugWriter.WriteDebug(DebugLevel.I, "Size: {0}", vars: [Result.Size]);
                DebugWriter.WriteDebug(DebugLevel.I, "Type: {0}", vars: [Result.Type]);
                if (Result.IsFailed)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Upload failed for {0}", vars: [Result.Name]);

                    // Upload could fail with no exception in very rare cases.
                    if (Result.Exception is not null)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Exception {0}", vars: [Result.Exception.Message]);
                        DebugWriter.WriteDebugStackTrace(Result.Exception);
                    }
                    Failed = true;
                }
                EventsManager.FireEvent(EventType.FTPPostUpload, Result.Name, !Failed);
            }

            // Show a message that it's downloaded
            if (!Failed)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Uploaded folder {0}.", vars: [Folder]);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Uploaded folder {0} partially due to failure.", vars: [Folder]);
            }
            EventsManager.FireEvent(EventType.FTPPostUpload, Folder, !Failed);
            return !Failed;
        }

        /// <summary>
        /// Downloads a file to string
        /// </summary>
        /// <param name="File">A text file.</param>
        /// <returns>Contents of the file</returns>
        public static string FTPDownloadToString(string File)
        {
            try
            {
                var client = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPNetwork, Translate.DoTranslation("Connection is not established yet."));

                // Show a message to download
                EventsManager.FireEvent(EventType.FTPPreDownload, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading {0}...", vars: [File]);

                // Try to download 3 times
                var DownloadedBytes = Array.Empty<byte>();
                var DownloadedContent = new StringBuilder();
                bool Downloaded = client.DownloadBytes(out DownloadedBytes, File);
                foreach (byte DownloadedByte in DownloadedBytes)
                    DownloadedContent.Append(Convert.ToChar(DownloadedByte));

                // Show a message that it's downloaded
                DebugWriter.WriteDebug(DebugLevel.I, "Downloaded {0}.", vars: [File]);
                EventsManager.FireEvent(EventType.FTPPostDownload, File, Downloaded);
                return DownloadedContent.ToString();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Download failed for {0}: {1}", vars: [File, ex.Message]);
                EventsManager.FireEvent(EventType.FTPPostDownload, File, false);
                throw new KernelException(KernelExceptionType.FTPFilesystem, Translate.DoTranslation("Download failed for file {0}.") + " {1}", File, ex.Message);
            }
        }

    }
}
