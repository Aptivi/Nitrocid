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

using FS = Nitrocid.Base.Files.FilesystemTools;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Network;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;
using Nitrocid.Base.Misc.Progress;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Network.Transfer;

namespace Nitrocid.Base.Drivers.Network
{
    /// <summary>
    /// Base network driver
    /// </summary>
    public abstract class BaseNetworkDriver : INetworkDriver
    {
        /// <inheritdoc/>
        public virtual string DriverName =>
            "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType =>
            DriverTypes.Network;

        /// <inheritdoc/>
        public virtual bool DriverInternal =>
            false;

        /// <inheritdoc/>
        public virtual bool NetworkAvailable =>
            NetworkInterface.GetIsNetworkAvailable();

        /// <inheritdoc/>
        public virtual bool DownloadFile(string URL) =>
            DownloadFile(URL, Config.MainConfig.ShowProgress);

        /// <inheritdoc/>
        public virtual bool DownloadFile(string URL, bool ShowProgress)
        {
            string FileName = NetworkTools.GetFilenameFromUrl(URL);
            return DownloadFile(URL, ShowProgress, FileName);
        }

        /// <inheritdoc/>
        public virtual bool DownloadFile(string URL, string FileName) =>
            DownloadFile(URL, Config.MainConfig.ShowProgress, FileName);

        /// <inheritdoc/>
        public virtual bool DownloadFile(string URL, bool ShowProgress, string FileName)
        {
            // Reset cancellation token
            if (NetworkTransfer.CancellationToken.IsCancellationRequested)
                NetworkTransfer.CancellationToken = new();

            // Intialize variables
            var FileUri = new Uri(URL);
            var builtinHandler = new ProgressHandler((_, message) => NetworkTransfer.HttpReceiveProgressWatch(message), "Download");

            // Initialize the progress bar indicator and the file completed event handler
            if (Config.MainConfig.DownloadNotificationProvoke)
            {
                NetworkTransfer.DownloadNotif = new Notification(LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_DOWNLOADING"), FileUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
                NotificationManager.NotifySend(NetworkTransfer.DownloadNotif);
            }
            if (ShowProgress)
                ProgressManager.RegisterProgressHandler(builtinHandler);

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", vars: [FS.CurrentDir]);
            var Response = NetworkTransfer.WClient.GetAsync(FileUri, HttpCompletionOption.ResponseHeadersRead, NetworkTransfer.CancellationToken.Token).Result;
            Response.EnsureSuccessStatusCode();

            // Get the file path
            string FilePath = FS.NeutralizePath(FileName);

            // Try to download the file asynchronously
            Task.Run(() =>
            {
                try
                {
                    int size = 16384;
                    var length = Response.Content.Headers.ContentLength;
                    long fileSize = length ?? 0;
                    long totalRead = 0;
                    using (var outputFileStream = File.Create(FilePath, size))
                    {
                        using var responseStream = Response.Content.ReadAsStream();
                        var buffer = new byte[size];
                        int bytesRead = 0;
                        do
                        {
                            bytesRead = responseStream.Read(buffer, 0, size);
                            outputFileStream.Write(buffer, 0, bytesRead);
                            totalRead += bytesRead;
                            double prog = 100d * ((double)totalRead / fileSize);
                            if (ShowProgress)
                                ProgressManager.ReportProgress(prog, "Download", $"{totalRead} / {fileSize} | {prog:000.00}%");
                        } while (bytesRead > 0);
                    }
                    NetworkTransfer.DownloadChecker(null);
                }
                catch (Exception ex)
                {
                    NetworkTransfer.DownloadChecker(ex);
                }
            }, NetworkTransfer.CancellationToken.Token);
            while (!NetworkTools.TransferFinished)
            {
                if (CancellationHandlers.CancelRequested)
                {
                    NetworkTools.TransferFinished = true;
                    NetworkTransfer.CancellationToken.Cancel();
                }
            }

            // Unregister the handler
            if (ShowProgress)
                ProgressManager.UnregisterProgressHandler(builtinHandler);

            // We're done downloading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !NetworkTransfer.SuppressDownloadMessage)
                TextWriterRaw.Write();
            NetworkTransfer.SuppressDownloadMessage = false;
            if (NetworkTransfer.IsError)
            {
                if (Config.MainConfig.DownloadNotificationProvoke && NetworkTransfer.DownloadNotif is not null)
                    NetworkTransfer.DownloadNotif.ProgressState = NotificationProgressState.Failure;
                NetworkTransfer.CancellationToken.Cancel();
                throw NetworkTransfer.ReasonError ??
                    new KernelException(KernelExceptionType.Network, LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_EXCEPTION_TRANSFERFAILURE"));
            }
            else
            {
                if (Config.MainConfig.DownloadNotificationProvoke && NetworkTransfer.DownloadNotif is not null)
                {
                    NetworkTransfer.DownloadNotif.Progress = 100;
                    NetworkTransfer.DownloadNotif.ProgressState = NotificationProgressState.Success;
                }
                return true;
            }
        }

        /// <inheritdoc/>
        public virtual string DownloadString(string URL) =>
            DownloadString(URL, Config.MainConfig.ShowProgress);

        /// <inheritdoc/>
        public virtual string DownloadString(string URL, bool ShowProgress)
        {
            // Reset cancellation token
            if (NetworkTransfer.CancellationToken.IsCancellationRequested)
                NetworkTransfer.CancellationToken = new();

            // Intialize variables
            var StringUri = new Uri(URL);
            var builtinHandler = new ProgressHandler((_, message) => NetworkTransfer.HttpReceiveProgressWatch(message), "Download");

            // Initialize the progress bar indicator and the file completed event handler
            if (Config.MainConfig.DownloadNotificationProvoke)
            {
                NetworkTransfer.DownloadNotif = new Notification(LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_DOWNLOADING"), StringUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
                NotificationManager.NotifySend(NetworkTransfer.DownloadNotif);
            }
            if (ShowProgress)
                ProgressManager.RegisterProgressHandler(builtinHandler);

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", vars: [FS.CurrentDir]);
            var Response = NetworkTransfer.WClient.GetAsync(StringUri, HttpCompletionOption.ResponseHeadersRead, NetworkTransfer.CancellationToken.Token).Result;
            Response.EnsureSuccessStatusCode();

            // Try to download the string asynchronously
            string downloaded = "";
            Task.Run(() =>
            {
                try
                {
                    int size = 16384;
                    var length = Response.Content.Headers.ContentLength;
                    long fileSize = length ?? 0;
                    long totalRead = 0;
                    using (var ContentStream = new MemoryStream())
                    {
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
                    NetworkTransfer.DownloadChecker(null);
                }
                catch (Exception ex)
                {
                    NetworkTransfer.DownloadChecker(ex);
                }
            }, NetworkTransfer.CancellationToken.Token);
            while (!NetworkTools.TransferFinished)
            {
                if (CancellationHandlers.CancelRequested)
                {
                    NetworkTools.TransferFinished = true;
                    NetworkTransfer.CancellationToken.Cancel();
                }
            }

            // Unregister the handler
            if (ShowProgress)
                ProgressManager.UnregisterProgressHandler(builtinHandler);

            // We're done downloading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !NetworkTransfer.SuppressDownloadMessage)
                TextWriterRaw.Write();
            NetworkTransfer.SuppressDownloadMessage = false;
            if (NetworkTransfer.IsError)
            {
                if (Config.MainConfig.DownloadNotificationProvoke && NetworkTransfer.DownloadNotif is not null)
                    NetworkTransfer.DownloadNotif.ProgressState = NotificationProgressState.Failure;
                NetworkTransfer.CancellationToken.Cancel();
                throw NetworkTransfer.ReasonError ??
                    new KernelException(KernelExceptionType.Network, LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_EXCEPTION_TRANSFERFAILURE"));
            }
            else
            {
                if (Config.MainConfig.DownloadNotificationProvoke && NetworkTransfer.DownloadNotif is not null)
                {
                    NetworkTransfer.DownloadNotif.Progress = 100;
                    NetworkTransfer.DownloadNotif.ProgressState = NotificationProgressState.Success;
                }
                return downloaded;
            }
        }

        /// <inheritdoc/>
        public virtual bool UploadFile(string FileName, string URL) =>
            UploadFile(FileName, URL, Config.MainConfig.ShowProgress);

        /// <inheritdoc/>
        public virtual bool UploadFile(string FileName, string URL, bool ShowProgress)
        {
            // Reset cancellation token
            if (NetworkTransfer.CancellationToken.IsCancellationRequested)
                NetworkTransfer.CancellationToken = new();

            // Intialize variables
            var FileUri = new Uri(URL);
            var builtinHandler = new ProgressHandler((_, message) => NetworkTransfer.HttpSendProgressWatch(message), "Upload");

            // Initialize the progress bar indicator and the file completed event handler
            if (Config.MainConfig.UploadNotificationProvoke && NetworkTransfer.DownloadNotif is not null)
            {
                NetworkTransfer.UploadNotif = new Notification(LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_UPLOADING"), FileUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
                NotificationManager.NotifySend(NetworkTransfer.DownloadNotif);
            }
            if (ShowProgress)
                ProgressManager.RegisterProgressHandler(builtinHandler);

            // Send the GET request to the server for the file after getting the stream and target file stream
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", vars: [FS.CurrentDir]);
            string FilePath = FS.NeutralizePath(FileName);
            var FileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            var Content = new StreamContent(FileStream);

            // Upload now
            bool uploaded = false;
            try
            {
                var progressTask = new Task(() => { UploadProgress(FileStream, ref uploaded); });
                progressTask.Start();
                var Response = NetworkTransfer.WClient.PutAsync(URL, Content, NetworkTransfer.CancellationToken.Token).Result;
                Response.EnsureSuccessStatusCode();
                NetworkTransfer.UploadChecker(null);
            }
            catch (Exception ex)
            {
                NetworkTransfer.UploadChecker(ex);
            }
            uploaded = true;

            // Unregister the handler
            if (ShowProgress)
                ProgressManager.UnregisterProgressHandler(builtinHandler);

            // We're done uploading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !NetworkTransfer.SuppressUploadMessage)
                TextWriterRaw.Write();
            NetworkTransfer.SuppressUploadMessage = false;
            if (NetworkTransfer.IsError)
            {
                if (Config.MainConfig.UploadNotificationProvoke && NetworkTransfer.UploadNotif is not null)
                    NetworkTransfer.UploadNotif.ProgressState = NotificationProgressState.Failure;
                NetworkTransfer.CancellationToken.Cancel();
                throw NetworkTransfer.ReasonError ??
                    new KernelException(KernelExceptionType.Network, LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_EXCEPTION_TRANSFERFAILURE"));
            }
            else
            {
                if (Config.MainConfig.UploadNotificationProvoke && NetworkTransfer.UploadNotif is not null)
                {
                    NetworkTransfer.UploadNotif.Progress = 100;
                    NetworkTransfer.UploadNotif.ProgressState = NotificationProgressState.Success;
                }
                return true;
            }
        }

        /// <inheritdoc/>
        public virtual bool UploadString(string URL, string Data) =>
            UploadString(URL, Data, Config.MainConfig.ShowProgress);

        /// <inheritdoc/>
        public virtual bool UploadString(string URL, string Data, bool ShowProgress)
        {
            // Reset cancellation token
            if (NetworkTransfer.CancellationToken.IsCancellationRequested)
                NetworkTransfer.CancellationToken = new();

            // Intialize variables
            var StringUri = new Uri(URL);
            var builtinHandler = new ProgressHandler((_, message) => NetworkTransfer.HttpSendProgressWatch(message), "Upload");

            // Initialize the progress bar indicator and the file completed event handler
            if (Config.MainConfig.UploadNotificationProvoke)
            {
                NetworkTransfer.UploadNotif = new Notification(LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_UPLOADING"), StringUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
                NotificationManager.NotifySend(NetworkTransfer.UploadNotif);
            }
            if (ShowProgress)
                ProgressManager.RegisterProgressHandler(builtinHandler);

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", vars: [FS.CurrentDir]);
            var StringContent = new StringContent(Data);

            // Upload now
            try
            {
                var Response = NetworkTransfer.WClient.PutAsync(URL, StringContent, NetworkTransfer.CancellationToken.Token).Result;
                Response.EnsureSuccessStatusCode();
                NetworkTransfer.UploadChecker(null);
            }
            catch (Exception ex)
            {
                NetworkTransfer.UploadChecker(ex);
            }

            // Unregister the handler
            if (ShowProgress)
                ProgressManager.UnregisterProgressHandler(builtinHandler);

            // We're done uploading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !NetworkTransfer.SuppressUploadMessage)
                TextWriterRaw.Write();
            NetworkTransfer.SuppressUploadMessage = false;
            if (NetworkTransfer.IsError)
            {
                if (Config.MainConfig.UploadNotificationProvoke && NetworkTransfer.UploadNotif is not null)
                    NetworkTransfer.UploadNotif.ProgressState = NotificationProgressState.Failure;
                NetworkTransfer.CancellationToken.Cancel();
                throw NetworkTransfer.ReasonError ??
                    new KernelException(KernelExceptionType.Network, LanguageTools.GetLocalized("NKS_DRIVERS_NETWORK_BASE_EXCEPTION_TRANSFERFAILURE"));
            }
            else
            {
                if (Config.MainConfig.UploadNotificationProvoke && NetworkTransfer.UploadNotif is not null)
                {
                    NetworkTransfer.UploadNotif.Progress = 100;
                    NetworkTransfer.UploadNotif.ProgressState = NotificationProgressState.Success;
                }
                return true;
            }
        }

        /// <inheritdoc/>
        public virtual string GetFilenameFromUrl(string Url)
        {
            string FileName = Url.Split('/').Last();
            DebugWriter.WriteDebug(DebugLevel.I, "Prototype Filename: {0}", vars: [FileName]);
            if (FileName.Contains('?'))
            {
                FileName = FileName.Remove(FileName.IndexOf('?'));
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Finished Filename: {0}", vars: [FileName]);
            return FileName;
        }

        /// <inheritdoc/>
        public virtual PingReply PingAddress(string Address, int Timeout, byte[] Buffer)
        {
            var Pinger = new Ping();
            var PingerOpts = new PingOptions() { DontFragment = true };
            return Pinger.Send(Address, Timeout, Buffer, PingerOpts);
        }

        /// <summary>
        /// Gets the online devices in your network, including the router and the broadcast address
        /// </summary>
        public IPAddress[] GetOnlineDevicesInNetwork()
        {
            if (!NetworkTools.NetworkAvailable)
                throw new KernelException(KernelExceptionType.NetworkOffline);

            // Get the local hostname and get its IP address information from the list
            List<IPAddress> onlineAddresses = [];
            string hostname = Dns.GetHostName();
            var hostnameEntry = Dns.GetHostEntry(hostname);

            // Enumerate through different addresses
            for (int i = 0; i < hostnameEntry.AddressList.Length; i++)
            {
                var address = hostnameEntry.AddressList[i];

                // If IPv4 or IPv6 (not implemented), go ahead
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    // Get the octets
                    byte[] addressBytes = address.GetAddressBytes();

                    // Now, scan the entire network
                    for (byte fourthOctet = 0; fourthOctet <= 255; fourthOctet++)
                    {
                        // Get the address to be scanned
                        byte[] bytes = [addressBytes[0], addressBytes[1], addressBytes[2], fourthOctet];
                        string addressString = string.Join(".", bytes);

                        // Ping it and time it out to 10 milliseconds
                        var reply = NetworkTools.PingAddress(addressString, 10);

                        // Check the reply
                        if (reply.Status == IPStatus.Success)
                            onlineAddresses.Add(new IPAddress(bytes));
                    }
                }
                else if (address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    var reply = NetworkTools.PingAddress("ff02::1", 10);

                    // Check the reply
                    if (reply.Status == IPStatus.Success)
                        onlineAddresses.Add(reply.Address);
                }
            }

            return [.. onlineAddresses];
        }

        private static void UploadProgress(FileStream stream, ref bool uploaded)
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
        }
    }
}
