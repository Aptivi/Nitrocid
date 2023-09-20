
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

using KS.Files.Folders;
using FS = KS.Files.Filesystem;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Network.Base;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using KS.Network.Base.Transfer;
using System.Net.Http;
using KS.Kernel.Exceptions;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Configuration;

namespace KS.Drivers.Network
{
    /// <summary>
    /// Base network driver
    /// </summary>
    public abstract class BaseNetworkDriver : INetworkDriver
    {
        /// <inheritdoc/>
        public virtual string DriverName => "Default";

        /// <inheritdoc/>
        public virtual DriverTypes DriverType => DriverTypes.Network;

        /// <inheritdoc/>
        public virtual bool DriverInternal => false;

        /// <inheritdoc/>
        public virtual bool NetworkAvailable => NetworkInterface.GetIsNetworkAvailable();

        /// <inheritdoc/>
        public virtual bool DownloadFile(string URL) =>
            DownloadFile(URL, KernelFlags.ShowProgress);

        /// <inheritdoc/>
        public virtual bool DownloadFile(string URL, bool ShowProgress)
        {
            string FileName = NetworkTools.GetFilenameFromUrl(URL);
            return DownloadFile(URL, ShowProgress, FileName);
        }

        /// <inheritdoc/>
        public virtual bool DownloadFile(string URL, string FileName) =>
            DownloadFile(URL, KernelFlags.ShowProgress, FileName);

        /// <inheritdoc/>
        public virtual bool DownloadFile(string URL, bool ShowProgress, string FileName)
        {
            // Intialize variables
            var FileUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (NetworkTransfer.DownloadNotificationProvoke)
            {
                NetworkTransfer.DownloadNotif = new Notification(Translate.DoTranslation("Downloading..."), FileUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
                NotificationManager.NotifySend(NetworkTransfer.DownloadNotif);
            }
            if (ShowProgress)
                NetworkTransfer.WClientProgress.HttpReceiveProgress += NetworkTransfer.HttpReceiveProgressWatch;

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            var Response = NetworkTransfer.WClient.GetAsync(FileUri, HttpCompletionOption.ResponseHeadersRead, NetworkTransfer.CancellationToken.Token).Result;
            Response.EnsureSuccessStatusCode();

            // Get the file stream
            string FilePath = FS.NeutralizePath(FileName);
            var FileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);

            // Try to download the file asynchronously
            Task.Run(() => 
            { 
                try 
                { 
                    Response.Content.ReadAsStreamAsync().Result.CopyTo(FileStream);
                    FileStream.Flush();
                    FileStream.Close();
                    NetworkTransfer.DownloadChecker(null);
                }
                catch (Exception ex)
                { 
                    NetworkTransfer.DownloadChecker(ex);
                } 
            }, NetworkTransfer.CancellationToken.Token);
            while (!NetworkTools.TransferFinished)
            {
                if (KernelFlags.CancelRequested)
                {
                    NetworkTools.TransferFinished = true;
                    NetworkTransfer.CancellationToken.Cancel();
                }
            }

            // We're done downloading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !NetworkTransfer.SuppressDownloadMessage)
                TextWriterColor.Write();
            NetworkTransfer.SuppressDownloadMessage = false;
            if (NetworkTransfer.IsError)
            {
                if (NetworkTransfer.DownloadNotificationProvoke)
                    NetworkTransfer.DownloadNotif.ProgressFailed = true;
                NetworkTransfer.CancellationToken.Cancel();
                throw NetworkTransfer.ReasonError;
            }
            else
            {
                if (NetworkTransfer.DownloadNotificationProvoke)
                    NetworkTransfer.DownloadNotif.Progress = 100;
                return true;
            }
        }

        /// <inheritdoc/>
        public virtual string DownloadString(string URL) =>
            DownloadString(URL, KernelFlags.ShowProgress);

        /// <inheritdoc/>
        public virtual string DownloadString(string URL, bool ShowProgress)
        {
            // Intialize variables
            var StringUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (NetworkTransfer.DownloadNotificationProvoke)
            {
                NetworkTransfer.DownloadNotif = new Notification(Translate.DoTranslation("Downloading..."), StringUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
                NotificationManager.NotifySend(NetworkTransfer.DownloadNotif);
            }
            if (ShowProgress)
                NetworkTransfer.WClientProgress.HttpReceiveProgress += NetworkTransfer.HttpReceiveProgressWatch;

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            var Response = NetworkTransfer.WClient.GetAsync(StringUri, HttpCompletionOption.ResponseHeadersRead, NetworkTransfer.CancellationToken.Token).Result;
            Response.EnsureSuccessStatusCode();

            // Get the memory stream
            var ContentStream = new MemoryStream();

            // Try to download the string asynchronously
            Task.Run(() => 
            { 
                try 
                { 
                    Response.Content.ReadAsStreamAsync().Result.CopyTo(ContentStream); 
                    ContentStream.Seek(0L, SeekOrigin.Begin);
                    NetworkTransfer.DownloadChecker(null); 
                } 
                catch (Exception ex) 
                {
                    NetworkTransfer.DownloadChecker(ex); 
                } 
            }, NetworkTransfer.CancellationToken.Token);
            while (!NetworkTools.TransferFinished)
            {
                if (KernelFlags.CancelRequested)
                {
                    NetworkTools.TransferFinished = true;
                    NetworkTransfer.CancellationToken.Cancel();
                }
            }

            // We're done downloading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !NetworkTransfer.SuppressDownloadMessage)
                TextWriterColor.Write();
            NetworkTransfer.SuppressDownloadMessage = false;
            if (NetworkTransfer.IsError)
            {
                if (NetworkTransfer.DownloadNotificationProvoke)
                    NetworkTransfer.DownloadNotif.ProgressFailed = true;
                NetworkTransfer.CancellationToken.Cancel();
                throw NetworkTransfer.ReasonError;
            }
            else
            {
                if (NetworkTransfer.DownloadNotificationProvoke)
                    NetworkTransfer.DownloadNotif.Progress = 100;
                return new StreamReader(ContentStream).ReadToEnd();
            }
        }

        /// <inheritdoc/>
        public virtual string GetFilenameFromUrl(string Url)
        {
            string FileName = Url.Split('/').Last();
            DebugWriter.WriteDebug(DebugLevel.I, "Prototype Filename: {0}", FileName);
            if (FileName.Contains(Convert.ToString('?')))
            {
                FileName = FileName.Remove(FileName.IndexOf('?'));
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Finished Filename: {0}", FileName);
            return FileName;
        }

        /// <inheritdoc/>
        public virtual PingReply PingAddress(string Address, int Timeout, byte[] Buffer)
        {
            var Pinger = new Ping();
            var PingerOpts = new PingOptions() { DontFragment = true };
            return Pinger.Send(Address, Timeout, Buffer, PingerOpts);
        }

        /// <inheritdoc/>
        public virtual bool UploadFile(string FileName, string URL) =>
            UploadFile(FileName, URL, KernelFlags.ShowProgress);

        /// <inheritdoc/>
        public virtual bool UploadFile(string FileName, string URL, bool ShowProgress)
        {
            // Intialize variables
            var FileUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (NetworkTransfer.UploadNotificationProvoke)
            {
                NetworkTransfer.UploadNotif = new Notification(Translate.DoTranslation("Uploading..."), FileUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
                NotificationManager.NotifySend(NetworkTransfer.DownloadNotif);
            }
            if (ShowProgress)
                NetworkTransfer.WClientProgress.HttpSendProgress += NetworkTransfer.HttpSendProgressWatch;

            // Send the GET request to the server for the file after getting the stream and target file stream
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            string FilePath = FS.NeutralizePath(FileName);
            var FileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            var Content = new StreamContent(FileStream);

            // Upload now
            try
            {
                var Response = NetworkTransfer.WClient.PutAsync(URL, Content, NetworkTransfer.CancellationToken.Token).Result;
                Response.EnsureSuccessStatusCode();
                NetworkTransfer.UploadChecker(null);
            }
            catch (Exception ex)
            {
                NetworkTransfer.UploadChecker(ex);
            }

            // We're done uploading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !NetworkTransfer.SuppressUploadMessage)
                TextWriterColor.Write();
            NetworkTransfer.SuppressUploadMessage = false;
            if (NetworkTransfer.IsError)
            {
                if (NetworkTransfer.UploadNotificationProvoke)
                    NetworkTransfer.UploadNotif.ProgressFailed = true;
                NetworkTransfer.CancellationToken.Cancel();
                throw NetworkTransfer.ReasonError;
            }
            else
            {
                if (NetworkTransfer.UploadNotificationProvoke)
                    NetworkTransfer.UploadNotif.Progress = 100;
                return true;
            }
        }

        /// <inheritdoc/>
        public virtual bool UploadString(string URL, string Data) => 
            UploadString(URL, Data, KernelFlags.ShowProgress);

        /// <inheritdoc/>
        public virtual bool UploadString(string URL, string Data, bool ShowProgress)
        {
            // Intialize variables
            var StringUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (NetworkTransfer.UploadNotificationProvoke)
            {
                NetworkTransfer.UploadNotif = new Notification(Translate.DoTranslation("Uploading..."), StringUri.AbsoluteUri, NotificationPriority.Low, NotificationType.Progress);
                NotificationManager.NotifySend(NetworkTransfer.UploadNotif);
            }
            if (ShowProgress)
                NetworkTransfer.WClientProgress.HttpSendProgress += NetworkTransfer.HttpSendProgressWatch;

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            var StringContent = new StringContent(Data);

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

            // We're done uploading. Check to see if it's actually an error
            NetworkTools.TransferFinished = false;
            if (ShowProgress & !NetworkTransfer.SuppressUploadMessage)
                TextWriterColor.Write();
            NetworkTransfer.SuppressUploadMessage = false;
            if (NetworkTransfer.IsError)
            {
                if (NetworkTransfer.UploadNotificationProvoke)
                    NetworkTransfer.UploadNotif.ProgressFailed = true;
                NetworkTransfer.CancellationToken.Cancel();
                throw NetworkTransfer.ReasonError;
            }
            else
            {
                if (NetworkTransfer.UploadNotificationProvoke)
                    NetworkTransfer.UploadNotif.Progress = 100;
                return true;
            }
        }

        /// <summary>
        /// Gets the online devices in your network, including the router and the broadcast address
        /// </summary>
        public IPAddress[] GetOnlineDevicesInNetwork()
        {
            if (!NetworkTools.NetworkAvailable)
                throw new KernelException(KernelExceptionType.NetworkOffline);

            // Get the local hostname and get its IP address information from the list
            List<IPAddress> onlineAddresses = new();
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
                        byte[] bytes = { addressBytes[0], addressBytes[1], addressBytes[2], fourthOctet };
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

            return onlineAddresses.ToArray();
        }
    }
}
