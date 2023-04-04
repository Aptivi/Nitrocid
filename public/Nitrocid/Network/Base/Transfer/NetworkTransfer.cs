
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using FluentFTP.Helpers;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers;
using KS.Files;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Probers.Placeholder;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Network.Base.Transfer
{
    /// <summary>
    /// Network transfer module
    /// </summary>
    public static class NetworkTransfer
    {

        internal static bool IsError;
        internal static Exception ReasonError;
        internal static CancellationTokenSource CancellationToken = new();
        internal static ProgressMessageHandler WClientProgress = new(new HttpClientHandler());
        internal static HttpClient WClient = new(WClientProgress);
        internal static string DownloadedString;
        internal static Notification DownloadNotif;
        internal static Notification UploadNotif;
        internal static bool SuppressDownloadMessage;
        internal static bool SuppressUploadMessage;

        /// <summary>
        /// Download percentage print style
        /// </summary>
        public static string DownloadPercentagePrint =>
            Config.MainConfig.DownloadPercentagePrint;
        /// <summary>
        /// Upload percentage print style
        /// </summary>
        public static string UploadPercentagePrint =>
            Config.MainConfig.UploadPercentagePrint;
        /// <summary>
        /// Whether to provoke the notification upon download starts
        /// </summary>
        public static bool DownloadNotificationProvoke =>
            Config.MainConfig.DownloadNotificationProvoke;
        /// <summary>
        /// Whether to provoke the notification upon upload starts
        /// </summary>
        public static bool UploadNotificationProvoke =>
            Config.MainConfig.UploadNotificationProvoke;

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL) =>
            DriverHandler.CurrentNetworkDriver.DownloadFile(URL);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL, bool ShowProgress) =>
            DriverHandler.CurrentNetworkDriver.DownloadFile(URL, ShowProgress);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="FileName">File name to download to</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL, string FileName) =>
            DriverHandler.CurrentNetworkDriver.DownloadFile(URL, FileName);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <param name="FileName">File name to download to</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL, bool ShowProgress, string FileName) =>
            DriverHandler.CurrentNetworkDriver.DownloadFile(URL, ShowProgress, FileName);

        /// <summary>
        /// Uploads a file to the current working directory.
        /// </summary>
        /// <param name="FileName">A target file name. Use <see cref="Filesystem.NeutralizePath(string, bool)"/> to get full path of source.</param>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadFile(string FileName, string URL) => 
            DriverHandler.CurrentNetworkDriver.UploadFile(FileName, URL);

        /// <summary>
        /// Uploads a file from the current working directory.
        /// </summary>
        /// <param name="FileName">A target file name. Use <see cref="Filesystem.NeutralizePath(string, bool)"/> to get full path of source.</param>
        /// <param name="URL">A URL</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadFile(string FileName, string URL, bool ShowProgress) =>
            DriverHandler.CurrentNetworkDriver.UploadFile(FileName, URL, ShowProgress);

        /// <summary>
        /// Downloads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static string DownloadString(string URL) => 
            DriverHandler.CurrentNetworkDriver.DownloadString(URL);

        /// <summary>
        /// Downloads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        public static string DownloadString(string URL, bool ShowProgress) =>
            DriverHandler.CurrentNetworkDriver.DownloadString(URL, ShowProgress);

        /// <summary>
        /// Uploads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="Data">Content to upload</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadString(string URL, string Data) =>
            DriverHandler.CurrentNetworkDriver.UploadString(URL, Data);

        /// <summary>
        /// Uploads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL</param>
        /// <param name="Data">Content to upload</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        public static bool UploadString(string URL, string Data, bool ShowProgress) =>
            DriverHandler.CurrentNetworkDriver.UploadString(URL, Data, ShowProgress);

        /// <summary>
        /// Check for errors on download completion.
        /// </summary>
        internal static void DownloadChecker(Exception e)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Download complete. Error: {0}", e?.Message);
            if (e is not null)
            {
                if (DownloadNotificationProvoke)
                    DownloadNotif.ProgressFailed = true;
                ReasonError = e;
                IsError = true;
            }
            NetworkTools.TransferFinished = true;
        }

        /// <summary>
        /// Thread to check for errors on download completion.
        /// </summary>
        internal static void UploadChecker(Exception e)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Upload complete. Error: {0}", e?.Message);
            if (e is not null)
            {
                if (UploadNotificationProvoke)
                    UploadNotif.ProgressFailed = true;
                ReasonError = e;
                IsError = true;
            }
            NetworkTools.TransferFinished = true;
        }

        internal static void HttpReceiveProgressWatch(object sender, HttpProgressEventArgs e)
        {
            int TotalBytes = (int)(e.TotalBytes ?? -1);
            var TransferInfo = new NetworkTransferInfo(e.BytesTransferred, TotalBytes, NetworkTransferType.Download);
            SuppressDownloadMessage = TotalBytes == -1;
            TransferProgress(TransferInfo);
        }

        internal static void HttpSendProgressWatch(object sender, HttpProgressEventArgs e)
        {
            int TotalBytes = (int)(e.TotalBytes ?? -1);
            var TransferInfo = new NetworkTransferInfo(e.BytesTransferred, TotalBytes, NetworkTransferType.Upload);
            SuppressUploadMessage = TotalBytes == -1;
            TransferProgress(TransferInfo);
        }

        /// <summary>
        /// Report the progress to the console.
        /// </summary>
        internal static void TransferProgress(NetworkTransferInfo TransferInfo)
        {
            try
            {
                // Distinguish download from upload
                bool NotificationProvoke = TransferInfo.TransferType == NetworkTransferType.Download ? DownloadNotificationProvoke : UploadNotificationProvoke;
                var NotificationInstance = TransferInfo.TransferType == NetworkTransferType.Download ? DownloadNotif : UploadNotif;

                // Report the progress
                if (!NetworkTools.TransferFinished)
                {
                    if (TransferInfo.FileSize >= 0L & !TransferInfo.MessageSuppressed)
                    {
                        // We know the total bytes. Print it out.
                        double Progress = 100d * (TransferInfo.DoneSize / (double)TransferInfo.FileSize);
                        if (NotificationProvoke)
                        {
                            NotificationInstance.Progress = (int)Math.Round(Progress);
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(DownloadPercentagePrint))
                            {
                                TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(DownloadPercentagePrint), 0, ConsoleWrapper.CursorTop, false, KernelColorType.NeutralText, TransferInfo.DoneSize.FileSizeToString(), TransferInfo.FileSize.FileSizeToString(), Progress);
                            }
                            else
                            {
                                TextWriterWhereColor.WriteWhere(Translate.DoTranslation("{0} of {1} downloaded.") + " | {2}%", 0, ConsoleWrapper.CursorTop, false, KernelColorType.NeutralText, TransferInfo.DoneSize.FileSizeToString(), TransferInfo.FileSize.FileSizeToString(), Progress);
                            }
                            ConsoleExtensions.ClearLineToRight();
                        }
                    }
                    else
                    {
                        TransferInfo.MessageSuppressed = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to report transfer progress: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

    }
}
