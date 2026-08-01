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
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;
using Nitrocid.Base.Misc.Progress;
using Nitrocid.Base.Misc.Reflection;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Commands;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.Tools.Placeholder;

namespace Nitrocid.Base.Network.Transfer
{
    /// <summary>
    /// Network transfer module
    /// </summary>
    public static class NetworkTransfer
    {
        internal static HttpClient httpClientNormal = new();
        internal static HttpClient httpClientIgnoreCertErrors = new(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
            {
                return true;
            }
        });

        internal static HttpClient HttpClient =>
            Config.MainConfig.IgnoreCertificateErrors ? httpClientIgnoreCertErrors : httpClientNormal;

        /// <summary>
        /// Generates a new HTTP client
        /// </summary>
        public static HttpClient HttpClientNew =>
            Config.MainConfig.IgnoreCertificateErrors ? new(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
                {
                    return true;
                }
            }) : new HttpClient();

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL) =>
            DownloadFile(URL, Config.MainConfig.ShowProgress);

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
        public static bool DownloadFile(string URL, string FileName) =>
            DownloadFile(URL, Config.MainConfig.ShowProgress, FileName);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <param name="FileName">File name to download to</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool DownloadFile(string URL, bool ShowProgress, string FileName)
        {
            // Reset cancellation token
            var cancellationToken = new CancellationTokenSource();

            // Intialize variables
            var FileUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            var downloadNotification = new Notification(LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_DOWNLOADING"), FileUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
            if (Config.MainConfig.DownloadNotificationProvoke)
                NotificationManager.NotifySend(downloadNotification);
            var builtinHandler = new ProgressHandler((_, message) => HttpReceiveProgressWatch(message, downloadNotification), "Download");
            if (ShowProgress)
                ProgressManager.RegisterProgressHandler(builtinHandler);

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", vars: [FilesystemTools.CurrentDir]);
            var Response = HttpClient.GetAsync(FileUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken.Token).Result;
            Response.EnsureSuccessStatusCode();

            // Get the file path
            string FilePath = FilesystemTools.NeutralizePath(FileName);

            // Try to download the file asynchronously
            bool isFailed = false;
            Exception? failureReason = null;
            Task.Run(() =>
            {
                try
                {
                    int size = 16384;
                    var length = Response.Content.Headers.ContentLength;
                    long fileSize = length ?? 0;
                    long totalRead = 0;
                    using var outputFileStream = File.Create(FilePath, size);
                    using var responseStream = Response.Content.ReadAsStream();
                    var buffer = new byte[size];
                    int bytesRead = 0;
                    do
                    {
                        if (CancellationHandlers.CancelRequested)
                            cancellationToken.Cancel();
                        cancellationToken.Token.ThrowIfCancellationRequested();
                        bytesRead = responseStream.Read(buffer, 0, size);
                        outputFileStream.Write(buffer, 0, bytesRead);
                        totalRead += bytesRead;
                        if (ShowProgress)
                        {
                            if (fileSize <= 0)
                                ProgressManager.ReportProgress(0, "Download", $"{totalRead}");
                            else
                            {
                                double prog = 100d * ((double)totalRead / fileSize);
                                ProgressManager.ReportProgress(prog, "Download", $"{totalRead} / {fileSize} | {prog:000.00}%");
                            }
                        }
                    } while (bytesRead > 0);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Download complete. Error: {0}", vars: [ex.Message]);
                    failureReason = ex;
                    isFailed = true;
                }
            }, cancellationToken.Token);

            // Unregister the handler
            if (ShowProgress)
                ProgressManager.UnregisterProgressHandler(builtinHandler);

            // We're done downloading. Check to see if it's actually an error
            if (ShowProgress)
                TextWriterRaw.Write();
            if (isFailed)
            {
                if (Config.MainConfig.DownloadNotificationProvoke)
                    downloadNotification.ProgressState = NotificationProgressState.Failure;
                cancellationToken.Cancel();
                throw failureReason ??
                    new KernelException(KernelExceptionType.Network, LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_EXCEPTION_TRANSFERFAILURE"));
            }
            else
            {
                if (Config.MainConfig.DownloadNotificationProvoke)
                {
                    downloadNotification.Progress = 100;
                    downloadNotification.ProgressState = NotificationProgressState.Success;
                }
                return true;
            }
        }

        /// <summary>
        /// Uploads a file to the current working directory.
        /// </summary>
        /// <param name="FileName">A target file name. Use <see cref="FilesystemTools.NeutralizePath(string, bool)"/> to get full path of source.</param>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadFile(string FileName, string URL) =>
            UploadFile(FileName, URL, Config.MainConfig.ShowProgress);

        /// <summary>
        /// Uploads a file from the current working directory.
        /// </summary>
        /// <param name="FileName">A target file name. Use <see cref="FilesystemTools.NeutralizePath(string, bool)"/> to get full path of source.</param>
        /// <param name="URL">A URL</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadFile(string FileName, string URL, bool ShowProgress)
        {
            // Reset cancellation token
            var cancellationToken = new CancellationTokenSource();

            // Intialize variables
            var FileUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            var uploadNotification = new Notification(LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_UPLOADING"), FileUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
            if (Config.MainConfig.UploadNotificationProvoke)
                NotificationManager.NotifySend(uploadNotification);
            var builtinHandler = new ProgressHandler((_, message) => HttpSendProgressWatch(message, uploadNotification), "Upload");
            if (ShowProgress)
                ProgressManager.RegisterProgressHandler(builtinHandler);

            // Send the GET request to the server for the file after getting the stream and target file stream
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", vars: [FilesystemTools.CurrentDir]);
            string FilePath = FilesystemTools.NeutralizePath(FileName);
            var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            var Content = new StreamContent(stream);

            // Upload now
            bool uploaded = false;
            bool isFailed = false;
            Exception? failureReason = null;
            try
            {
                var progressTask = new Task(() =>
                {
                    double previousPercentage = 0.0;
                    while (!uploaded)
                    {
                        long uploadedBytes = stream.Position;
                        long totalBytes = stream.Length;
                        double percentage = 100 * (uploadedBytes / (double)totalBytes);
                        if (percentage != previousPercentage)
                            ProgressManager.ReportProgress(percentage, "Upload", $"{uploadedBytes} / {totalBytes} | {percentage:000.00}%");
                        previousPercentage = percentage;
                    }
                });
                progressTask.Start();
                var Response = HttpClient.PutAsync(URL, Content, cancellationToken.Token).Result;
                Response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Upload complete. Error: {0}", vars: [ex.Message]);
                failureReason = ex;
                isFailed = true;
            }
            uploaded = true;

            // Unregister the handler
            if (ShowProgress)
                ProgressManager.UnregisterProgressHandler(builtinHandler);

            // We're done uploading. Check to see if it's actually an error
            if (isFailed)
            {
                if (Config.MainConfig.UploadNotificationProvoke)
                    uploadNotification.ProgressState = NotificationProgressState.Failure;
                cancellationToken.Cancel();
                throw failureReason ??
                    new KernelException(KernelExceptionType.Network, LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_EXCEPTION_TRANSFERFAILURE"));
            }
            else
            {
                if (Config.MainConfig.UploadNotificationProvoke)
                {
                    uploadNotification.Progress = 100;
                    uploadNotification.ProgressState = NotificationProgressState.Success;
                }
                return true;
            }
        }

        /// <summary>
        /// Downloads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static string DownloadString(string URL) =>
            DownloadString(URL, Config.MainConfig.ShowProgress);

        /// <summary>
        /// Downloads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        public static string DownloadString(string URL, bool ShowProgress)
        {
            // Reset cancellation token
            var cancellationToken = new CancellationTokenSource();

            // Intialize variables
            var StringUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            var downloadNotification = new Notification(LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_DOWNLOADING"), StringUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
            if (Config.MainConfig.DownloadNotificationProvoke)
                NotificationManager.NotifySend(downloadNotification);
            var builtinHandler = new ProgressHandler((_, message) => HttpReceiveProgressWatch(message, downloadNotification), "Download");
            if (ShowProgress)
                ProgressManager.RegisterProgressHandler(builtinHandler);

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", vars: [FilesystemTools.CurrentDir]);
            var Response = HttpClient.GetAsync(StringUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken.Token).Result;
            Response.EnsureSuccessStatusCode();

            // Try to download the string asynchronously
            string downloaded = "";
            bool isFailed = false;
            Exception? failureReason = null;
            Task.Run(() =>
            {
                try
                {
                    int size = 16384;
                    var length = Response.Content.Headers.ContentLength;
                    long fileSize = length ?? 0;
                    long totalRead = 0;
                    using var ContentStream = new MemoryStream();
                    if (CancellationHandlers.CancelRequested)
                        cancellationToken.Cancel();
                    cancellationToken.Token.ThrowIfCancellationRequested();
                    using var responseStream = Response.Content.ReadAsStream();
                    var buffer = new byte[size];
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = responseStream.Read(buffer, 0, size);
                        ContentStream.Write(buffer, 0, bytesRead);
                        totalRead += bytesRead;
                        double prog = 100d * ((double)totalRead / fileSize);
                        if (ShowProgress)
                            ProgressManager.ReportProgress(prog, "Download", $"{totalRead} / {fileSize} | {prog:000.00}%");
                    } while (bytesRead > 0);
                    ContentStream.Seek(0L, SeekOrigin.Begin);
                    downloaded = new StreamReader(ContentStream).ReadToEnd();
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Download complete. Error: {0}", vars: [ex.Message]);
                    failureReason = ex;
                    isFailed = true;
                }
            }, cancellationToken.Token);

            // Unregister the handler
            if (ShowProgress)
                ProgressManager.UnregisterProgressHandler(builtinHandler);

            // We're done downloading. Check to see if it's actually an error
            if (isFailed)
            {
                if (Config.MainConfig.DownloadNotificationProvoke)
                    downloadNotification.ProgressState = NotificationProgressState.Failure;
                cancellationToken.Cancel();
                throw failureReason ??
                    new KernelException(KernelExceptionType.Network, LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_EXCEPTION_TRANSFERFAILURE"));
            }
            else
            {
                if (Config.MainConfig.DownloadNotificationProvoke)
                {
                    downloadNotification.Progress = 100;
                    downloadNotification.ProgressState = NotificationProgressState.Success;
                }
                return downloaded;
            }
        }

        /// <summary>
        /// Uploads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="Data">Content to upload</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        public static bool UploadString(string URL, string Data) =>
            UploadString(URL, Data, Config.MainConfig.ShowProgress);

        /// <summary>
        /// Uploads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL</param>
        /// <param name="Data">Content to upload</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        public static bool UploadString(string URL, string Data, bool ShowProgress)
        {
            // Reset cancellation token
            var cancellationToken = new CancellationTokenSource();

            // Intialize variables
            var StringUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            var uploadNotification = new Notification(LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_UPLOADING"), StringUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
            if (Config.MainConfig.UploadNotificationProvoke)
                NotificationManager.NotifySend(uploadNotification);
            var builtinHandler = new ProgressHandler((_, message) => HttpSendProgressWatch(message, uploadNotification), "Upload");
            if (ShowProgress)
                ProgressManager.RegisterProgressHandler(builtinHandler);

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", vars: [FilesystemTools.CurrentDir]);
            var StringContent = new StringContent(Data);

            // Upload now
            bool isFailed = false;
            Exception? failureReason = null;
            try
            {
                var Response = HttpClient.PutAsync(URL, StringContent, cancellationToken.Token).Result;
                Response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Upload complete. Error: {0}", vars: [ex.Message]);
                failureReason = ex;
                isFailed = true;
            }

            // Unregister the handler
            if (ShowProgress)
                ProgressManager.UnregisterProgressHandler(builtinHandler);

            // We're done uploading. Check to see if it's actually an error
            if (isFailed)
            {
                if (Config.MainConfig.UploadNotificationProvoke)
                    uploadNotification.ProgressState = NotificationProgressState.Failure;
                cancellationToken.Cancel();
                throw failureReason ??
                    new KernelException(KernelExceptionType.Network, LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_EXCEPTION_TRANSFERFAILURE"));
            }
            else
            {
                if (Config.MainConfig.UploadNotificationProvoke)
                {
                    uploadNotification.Progress = 100;
                    uploadNotification.ProgressState = NotificationProgressState.Success;
                }
                return true;
            }
        }

        internal static void HttpReceiveProgressWatch(string message, Notification downloadNotification)
        {
            long totalRead;
            long fileSize = 0;
            if (message.Contains(" | "))
            {
                string totalReadStr = message[0..message.IndexOf(" / ")];
                string fileSizeStr = message[(message.IndexOf(" / ") + 3)..message.IndexOf(" | ")];
                totalRead = long.Parse(totalReadStr);
                fileSize = long.Parse(fileSizeStr);
            }
            else
                totalRead = long.Parse(message);
            TransferProgress(totalRead, fileSize, downloadNotification, Config.MainConfig.DownloadNotificationProvoke, LanguageTools.GetLocalized("NKS_NETWORK_TRANSFER_DOWNLOADINDICATOR"), Config.MainConfig.DownloadPercentagePrint);
        }

        internal static void HttpSendProgressWatch(string message, Notification uploadNotification)
        {
            long totalRead;
            long fileSize = 0;
            if (message.Contains(" | "))
            {
                string totalReadStr = message[0..message.IndexOf(" / ")];
                string fileSizeStr = message[(message.IndexOf(" / ") + 3)..message.IndexOf(" | ")];
                totalRead = long.Parse(totalReadStr);
                fileSize = long.Parse(fileSizeStr);
            }
            else
                totalRead = long.Parse(message);
            TransferProgress(totalRead, fileSize, uploadNotification, Config.MainConfig.UploadNotificationProvoke, LanguageTools.GetLocalized("NKS_NETWORK_TRANSFER_UPLOADINDICATOR"), Config.MainConfig.UploadPercentagePrint);
        }

        internal static void TransferProgress(long totalRead, long fileSize, Notification notification, bool showNotification, string indicatorBuiltin, string customIndicator)
        {
            try
            {
                if (fileSize >= 0L)
                {
                    // We know the total bytes. Print it out.
                    double Progress = fileSize > 0 ? 100d * (totalRead / (double)fileSize) : 0;
                    if (showNotification && notification is not null)
                        notification.Progress = (int)Math.Round(Progress);
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(customIndicator))
                            TextWriterColor.Write("\r" + PlaceParse.ProbePlaces(customIndicator), false, ThemeColorType.NeutralText, totalRead.SizeString(), fileSize.SizeString(), Progress);
                        else
                        {
                            var progress = new ProgressBar(indicatorBuiltin, (int)((double)totalRead / fileSize * 100), 100)
                            {
                                Accurate = true,
                                Width = ConsoleWrapper.WindowWidth - 1
                            };
                            TextWriterColor.Write($"\r{progress.Render()}", false, ThemeColorType.NeutralText);
                        }
                        ConsoleClearing.ClearLineToRight();
                    }
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
