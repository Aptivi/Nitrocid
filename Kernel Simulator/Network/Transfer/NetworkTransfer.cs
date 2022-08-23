
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
using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using System.Threading.Tasks;
using FluentFTP.Helpers;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Folders;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Probers;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

namespace KS.Network.Transfer
{
    public static class NetworkTransfer
    {

        public static string DownloadPercentagePrint = "";
        public static string UploadPercentagePrint = "";
        public static bool DownloadNotificationProvoke;
        public static bool UploadNotificationProvoke;
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
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL)
        {
            return DownloadFile(URL, Flags.ShowProgress);
        }

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL, bool ShowProgress)
        {
            string FileName = NetworkTools.GetFilenameFromUrl(URL);
            return DownloadFile(URL, ShowProgress, FileName);
        }

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="FileName">File name to download to</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL, string FileName)
        {
            return DownloadFile(URL, Flags.ShowProgress, FileName);
        }

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <param name="FileName">File name to download to</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL, bool ShowProgress, string FileName)
        {
            // Intialize variables
            var FileUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (DownloadNotificationProvoke)
            {
                DownloadNotif = new Notification(Translate.DoTranslation("Downloading..."), FileUri.AbsoluteUri, Notifications.NotifPriority.Low, Notifications.NotifType.Progress);
                Notifications.NotifySend(DownloadNotif);
            }
            if (ShowProgress)
                WClientProgress.HttpReceiveProgress += HttpReceiveProgressWatch;

            // Send the GET request to the server for the file
            DebugWriter.Wdbg(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            var Response = WClient.GetAsync(FileUri, CancellationToken.Token).Result;
            Response.EnsureSuccessStatusCode();

            // Get the file stream
            string FilePath = Filesystem.NeutralizePath(FileName);
            var FileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);

            // Try to download the file asynchronously
            Task.Run(() => { try { Response.Content.ReadAsStreamAsync().Result.CopyTo(FileStream); FileStream.Flush(); FileStream.Close(); DownloadChecker(null); } catch (Exception ex) { DownloadChecker(ex); } }, CancellationToken.Token);
            while (!NetworkTools.TransferFinished)
            {
                if (Flags.CancelRequested)
                {
                    NetworkTools.TransferFinished = true;
                    CancellationToken.Cancel();
                }
            }

            // We're done downloading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !SuppressDownloadMessage)
                Console.WriteLine();
            SuppressDownloadMessage = false;
            if (IsError)
            {
                if (DownloadNotificationProvoke)
                    DownloadNotif.ProgressFailed = true;
                CancellationToken.Cancel();
                throw ReasonError;
            }
            else
            {
                if (DownloadNotificationProvoke)
                    DownloadNotif.Progress = 100;
                return true;
            }
        }

        /// <summary>
        /// Uploads a file to the current working directory.
        /// </summary>
        /// <param name="FileName">A target file name. Use <see cref="NeutralizePath(String, Boolean)"/> to get full path of source.</param>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadFile(string FileName, string URL)
        {
            return UploadFile(FileName, URL, Flags.ShowProgress);
        }

        /// <summary>
        /// Uploads a file from the current working directory.
        /// </summary>
        /// <param name="FileName">A target file name. Use <see cref="NeutralizePath(String, Boolean)"/> to get full path of source.</param>
        /// <param name="URL">A URL</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadFile(string FileName, string URL, bool ShowProgress)
        {
            // Intialize variables
            var FileUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (UploadNotificationProvoke)
            {
                UploadNotif = new Notification(Translate.DoTranslation("Uploading..."), FileUri.AbsoluteUri, Notifications.NotifPriority.Low, Notifications.NotifType.Progress);
                Notifications.NotifySend(DownloadNotif);
            }
            if (ShowProgress)
                WClientProgress.HttpSendProgress += HttpSendProgressWatch;

            // Send the GET request to the server for the file after getting the stream and target file stream
            DebugWriter.Wdbg(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            string FilePath = Filesystem.NeutralizePath(FileName);
            var FileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            var Content = new StreamContent(FileStream);

            // Upload now
            try
            {
                var Response = WClient.PutAsync(URL, Content, CancellationToken.Token).Result;
                Response.EnsureSuccessStatusCode();
                UploadChecker(null);
            }
            catch (Exception ex)
            {
                UploadChecker(ex);
            }

            // We're done uploading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !SuppressUploadMessage)
                Console.WriteLine();
            SuppressUploadMessage = false;
            if (IsError)
            {
                if (UploadNotificationProvoke)
                    UploadNotif.ProgressFailed = true;
                CancellationToken.Cancel();
                throw ReasonError;
            }
            else
            {
                if (UploadNotificationProvoke)
                    UploadNotif.Progress = 100;
                return true;
            }
        }

        /// <summary>
        /// Downloads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static string DownloadString(string URL)
        {
            return DownloadString(URL, Flags.ShowProgress);
        }

        /// <summary>
        /// Downloads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        public static string DownloadString(string URL, bool ShowProgress)
        {
            // Intialize variables
            var StringUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (DownloadNotificationProvoke)
            {
                DownloadNotif = new Notification(Translate.DoTranslation("Downloading..."), StringUri.AbsoluteUri, Notifications.NotifPriority.Low, Notifications.NotifType.Progress);
                Notifications.NotifySend(DownloadNotif);
            }
            if (ShowProgress)
                WClientProgress.HttpReceiveProgress += HttpReceiveProgressWatch;

            // Send the GET request to the server for the file
            DebugWriter.Wdbg(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            var Response = WClient.GetAsync(StringUri, CancellationToken.Token).Result;
            Response.EnsureSuccessStatusCode();

            // Get the memory stream
            var ContentStream = new MemoryStream();

            // Try to download the string asynchronously
            Task.Run(() => { try { Response.Content.ReadAsStreamAsync().Result.CopyTo(ContentStream); ContentStream.Seek(0L, SeekOrigin.Begin); DownloadChecker(null); } catch (Exception ex) { DownloadChecker(ex); } }, CancellationToken.Token);
            while (!NetworkTools.TransferFinished)
            {
                if (Flags.CancelRequested)
                {
                    NetworkTools.TransferFinished = true;
                    CancellationToken.Cancel();
                }
            }

            // We're done downloading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !SuppressDownloadMessage)
                Console.WriteLine();
            SuppressDownloadMessage = false;
            if (IsError)
            {
                if (DownloadNotificationProvoke)
                    DownloadNotif.ProgressFailed = true;
                CancellationToken.Cancel();
                throw ReasonError;
            }
            else
            {
                if (DownloadNotificationProvoke)
                    DownloadNotif.Progress = 100;
                return new StreamReader(ContentStream).ReadToEnd();
            }
        }

        /// <summary>
        /// Uploads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="Data">Content to upload</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadString(string URL, string Data)
        {
            return UploadString(URL, Data, Flags.ShowProgress);
        }

        /// <summary>
        /// Uploads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL</param>
        /// <param name="Data">Content to upload</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        public static bool UploadString(string URL, string Data, bool ShowProgress)
        {
            // Intialize variables
            var StringUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (UploadNotificationProvoke)
            {
                UploadNotif = new Notification(Translate.DoTranslation("Uploading..."), StringUri.AbsoluteUri, Notifications.NotifPriority.Low, Notifications.NotifType.Progress);
                Notifications.NotifySend(UploadNotif);
            }
            if (ShowProgress)
                WClientProgress.HttpSendProgress += HttpSendProgressWatch;

            // Send the GET request to the server for the file
            DebugWriter.Wdbg(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            var StringContent = new StringContent(Data);

            try
            {
                var Response = WClient.PutAsync(URL, StringContent, CancellationToken.Token).Result;
                Response.EnsureSuccessStatusCode();
                UploadChecker(null);
            }
            catch (Exception ex)
            {
                UploadChecker(ex);
            }

            // We're done uploading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !SuppressUploadMessage)
                Console.WriteLine();
            SuppressUploadMessage = false;
            if (IsError)
            {
                if (UploadNotificationProvoke)
                    UploadNotif.ProgressFailed = true;
                CancellationToken.Cancel();
                throw ReasonError;
            }
            else
            {
                if (UploadNotificationProvoke)
                    UploadNotif.Progress = 100;
                return true;
            }
        }

        /// <summary>
        /// Check for errors on download completion.
        /// </summary>
        private static void DownloadChecker(Exception e)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Download complete. Error: {0}", e?.Message);
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
        private static void UploadChecker(Exception e)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Upload complete. Error: {0}", e?.Message);
            if (e is not null)
            {
                if (UploadNotificationProvoke)
                    UploadNotif.ProgressFailed = true;
                ReasonError = e;
                IsError = true;
            }
            NetworkTools.TransferFinished = true;
        }

        private static void HttpReceiveProgressWatch(object sender, HttpProgressEventArgs e)
        {
            int TotalBytes = (int)(e.TotalBytes ?? -1);
            var TransferInfo = new NetworkTransferInfo(e.BytesTransferred, TotalBytes, NetworkTransferType.Download);
            SuppressDownloadMessage = TotalBytes == -1;
            TransferProgress(TransferInfo);
        }

        private static void HttpSendProgressWatch(object sender, HttpProgressEventArgs e)
        {
            int TotalBytes = (int)(e.TotalBytes ?? -1);
            var TransferInfo = new NetworkTransferInfo(e.BytesTransferred, TotalBytes, NetworkTransferType.Upload);
            SuppressUploadMessage = TotalBytes == -1;
            TransferProgress(TransferInfo);
        }

        /// <summary>
        /// Report the progress to the console.
        /// </summary>
        private static void TransferProgress(NetworkTransferInfo TransferInfo)
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
                                TextWriterWhereColor.WriteWhere(PlaceParse.ProbePlaces(DownloadPercentagePrint), 0, Console.CursorTop, false, ColorTools.ColTypes.Neutral, TransferInfo.DoneSize.FileSizeToString(), TransferInfo.FileSize.FileSizeToString(), Progress);
                            }
                            else
                            {
                                TextWriterWhereColor.WriteWhere(Translate.DoTranslation("{0} of {1} downloaded.") + " | {2}%", 0, Console.CursorTop, false, ColorTools.ColTypes.Neutral, TransferInfo.DoneSize.FileSizeToString(), TransferInfo.FileSize.FileSizeToString(), Progress);
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
                DebugWriter.Wdbg(DebugLevel.E, "Error trying to report transfer progress: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
            }
        }

    }
}