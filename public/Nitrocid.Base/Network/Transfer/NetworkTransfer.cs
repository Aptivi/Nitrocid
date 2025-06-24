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
using System.Net.Http;
using System.Threading;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Files;
using Textify.Tools.Placeholder;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Network;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;

namespace Nitrocid.Base.Network.Transfer
{
    /// <summary>
    /// Network transfer module
    /// </summary>
    public static class NetworkTransfer
    {

        internal static bool IsError;
        internal static Exception? ReasonError;
        internal static CancellationTokenSource CancellationToken = new();
        internal static HttpClient WClient = new();
        internal static string DownloadedString = "";
        internal static Notification? DownloadNotif;
        internal static Notification? UploadNotif;
        internal static bool SuppressDownloadMessage;
        internal static bool SuppressUploadMessage;

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL) =>
            DriverHandler.CurrentNetworkDriverLocal.DownloadFile(URL);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL, bool ShowProgress) =>
            DriverHandler.CurrentNetworkDriverLocal.DownloadFile(URL, ShowProgress);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="FileName">File name to download to</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL, string FileName) =>
            DriverHandler.CurrentNetworkDriverLocal.DownloadFile(URL, FileName);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <param name="FileName">File name to download to</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL, bool ShowProgress, string FileName) =>
            DriverHandler.CurrentNetworkDriverLocal.DownloadFile(URL, ShowProgress, FileName);

        /// <summary>
        /// Uploads a file to the current working directory.
        /// </summary>
        /// <param name="FileName">A target file name. Use <see cref="FilesystemTools.NeutralizePath(string, bool)"/> to get full path of source.</param>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadFile(string FileName, string URL) =>
            DriverHandler.CurrentNetworkDriverLocal.UploadFile(FileName, URL);

        /// <summary>
        /// Uploads a file from the current working directory.
        /// </summary>
        /// <param name="FileName">A target file name. Use <see cref="FilesystemTools.NeutralizePath(string, bool)"/> to get full path of source.</param>
        /// <param name="URL">A URL</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadFile(string FileName, string URL, bool ShowProgress) =>
            DriverHandler.CurrentNetworkDriverLocal.UploadFile(FileName, URL, ShowProgress);

        /// <summary>
        /// Downloads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static string DownloadString(string URL) =>
            DriverHandler.CurrentNetworkDriverLocal.DownloadString(URL);

        /// <summary>
        /// Downloads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        public static string DownloadString(string URL, bool ShowProgress) =>
            DriverHandler.CurrentNetworkDriverLocal.DownloadString(URL, ShowProgress);

        /// <summary>
        /// Uploads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="Data">Content to upload</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadString(string URL, string Data) =>
            DriverHandler.CurrentNetworkDriverLocal.UploadString(URL, Data);

        /// <summary>
        /// Uploads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL</param>
        /// <param name="Data">Content to upload</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        public static bool UploadString(string URL, string Data, bool ShowProgress) =>
            DriverHandler.CurrentNetworkDriverLocal.UploadString(URL, Data, ShowProgress);

        /// <summary>
        /// Check for errors on download completion.
        /// </summary>
        internal static void DownloadChecker(Exception? e)
        {
            if (e is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Download complete. Error: {0}", vars: [e.Message]);
                if (Config.MainConfig.DownloadNotificationProvoke && DownloadNotif is not null)
                    DownloadNotif.ProgressState = NotificationProgressState.Failure;
                ReasonError = e;
                IsError = true;
            }
            else if (Config.MainConfig.DownloadNotificationProvoke && DownloadNotif is not null)
                DownloadNotif.ProgressState = NotificationProgressState.Success;
            DebugWriter.WriteDebug(DebugLevel.I, "Download complete.");
            NetworkTools.TransferFinished = true;
        }

        /// <summary>
        /// Thread to check for errors on download completion.
        /// </summary>
        internal static void UploadChecker(Exception? e)
        {
            if (e is not null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Upload complete. Error: {0}", vars: [e.Message]);
                if (Config.MainConfig.UploadNotificationProvoke && UploadNotif is not null)
                    UploadNotif.ProgressState = NotificationProgressState.Failure;
                ReasonError = e;
                IsError = true;
            }
            else if (Config.MainConfig.UploadNotificationProvoke && UploadNotif is not null)
                UploadNotif.ProgressState = NotificationProgressState.Success;
            DebugWriter.WriteDebug(DebugLevel.I, "Upload complete.");
            NetworkTools.TransferFinished = true;
        }

        internal static void HttpReceiveProgressWatch(string message)
        {
            string totalReadStr = message[0..message.IndexOf(" / ")];
            string fileSizeStr = message[(message.IndexOf(" / ") + 3)..message.IndexOf(" | ")];
            long totalRead = long.Parse(totalReadStr);
            long fileSize = long.Parse(fileSizeStr);
            var TransferInfo = new NetworkTransferInfo(totalRead, fileSize, NetworkTransferType.Download);
            SuppressDownloadMessage = fileSize == 0;
            TransferProgress(TransferInfo);
        }

        internal static void HttpSendProgressWatch(string message)
        {
            string totalReadStr = message[0..message.IndexOf(" / ")];
            string fileSizeStr = message[(message.IndexOf(" / ") + 3)..message.IndexOf(" | ")];
            long totalRead = long.Parse(totalReadStr);
            long fileSize = long.Parse(fileSizeStr);
            var TransferInfo = new NetworkTransferInfo(totalRead, fileSize, NetworkTransferType.Upload);
            SuppressUploadMessage = fileSize == 0;
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
                bool NotificationProvoke = TransferInfo.TransferType == NetworkTransferType.Download ? Config.MainConfig.DownloadNotificationProvoke : Config.MainConfig.UploadNotificationProvoke;
                var NotificationInstance = TransferInfo.TransferType == NetworkTransferType.Download ? DownloadNotif : UploadNotif;
                string indicator = TransferInfo.TransferType == NetworkTransferType.Download ? LanguageTools.GetLocalized("NKS_NETWORK_TRANSFER_DOWNLOADINDICATOR") : LanguageTools.GetLocalized("NKS_NETWORK_TRANSFER_UPLOADINDICATOR");

                // Report the progress
                if (!NetworkTools.TransferFinished)
                {
                    if (TransferInfo.FileSize >= 0L & !TransferInfo.MessageSuppressed)
                    {
                        // We know the total bytes. Print it out.
                        double Progress = 100d * (TransferInfo.DoneSize / (double)TransferInfo.FileSize);
                        if (NotificationProvoke && NotificationInstance is not null)
                            NotificationInstance.Progress = (int)Math.Round(Progress);
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(Config.MainConfig.DownloadPercentagePrint))
                                TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(Config.MainConfig.DownloadPercentagePrint), 0, ConsoleWrapper.CursorTop, false, ThemeColorType.NeutralText, TransferInfo.DoneSize.SizeString(), TransferInfo.FileSize.SizeString(), Progress);
                            else
                                TextWriterWhereColor.WriteWhere(" {2:000.00}% | " + indicator, 0, ConsoleWrapper.CursorTop, false, ThemeColorType.NeutralText, TransferInfo.DoneSize.SizeString(), TransferInfo.FileSize.SizeString(), Progress);
                            ConsoleClearing.ClearLineToRight();
                        }
                    }
                    else
                        TransferInfo.MessageSuppressed = true;
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to report transfer progress: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

    }
}
