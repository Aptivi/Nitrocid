
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

using KS.Files.Folders;
using KS.Files;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Base;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading;
using KS.Network.Base.Transfer;
using System.Net.Http;

namespace KS.Drivers.Network
{
    /// <summary>
    /// Base network driver
    /// </summary>
    public abstract class BaseNetworkDriver : INetworkDriver
    {
        /// <inheritdoc/>
        public string DriverName => "Default";

        /// <inheritdoc/>
        public DriverTypes DriverType => DriverTypes.Network;

        /// <inheritdoc/>
        public bool NetworkAvailable => NetworkInterface.GetIsNetworkAvailable();

        /// <inheritdoc/>
        public bool DownloadFile(string URL) =>
            DownloadFile(URL, Flags.ShowProgress);

        /// <inheritdoc/>
        public bool DownloadFile(string URL, bool ShowProgress)
        {
            string FileName = NetworkTools.GetFilenameFromUrl(URL);
            return DownloadFile(URL, ShowProgress, FileName);
        }

        /// <inheritdoc/>
        public bool DownloadFile(string URL, string FileName) =>
            DownloadFile(URL, Flags.ShowProgress, FileName);

        /// <inheritdoc/>
        public bool DownloadFile(string URL, bool ShowProgress, string FileName)
        {
            // Intialize variables
            var FileUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (NetworkTransfer.DownloadNotificationProvoke)
            {
                NetworkTransfer.DownloadNotif = new Notification(Translate.DoTranslation("Downloading..."), FileUri.AbsoluteUri, Notifications.NotifPriority.Low, Notifications.NotifType.Progress);
                Notifications.NotifySend(NetworkTransfer.DownloadNotif);
            }
            if (ShowProgress)
                NetworkTransfer.WClientProgress.HttpReceiveProgress += NetworkTransfer.HttpReceiveProgressWatch;

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            var Response = NetworkTransfer.WClient.GetAsync(FileUri, NetworkTransfer.CancellationToken.Token).Result;
            Response.EnsureSuccessStatusCode();

            // Get the file stream
            string FilePath = Filesystem.NeutralizePath(FileName);
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
                if (Flags.CancelRequested)
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
        public string DownloadString(string URL) =>
            DownloadString(URL, Flags.ShowProgress);

        /// <inheritdoc/>
        public string DownloadString(string URL, bool ShowProgress)
        {
            // Intialize variables
            var StringUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (NetworkTransfer.DownloadNotificationProvoke)
            {
                NetworkTransfer.DownloadNotif = new Notification(Translate.DoTranslation("Downloading..."), StringUri.AbsoluteUri, Notifications.NotifPriority.Low, Notifications.NotifType.Progress);
                Notifications.NotifySend(NetworkTransfer.DownloadNotif);
            }
            if (ShowProgress)
                NetworkTransfer.WClientProgress.HttpReceiveProgress += NetworkTransfer.HttpReceiveProgressWatch;

            // Send the GET request to the server for the file
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            var Response = NetworkTransfer.WClient.GetAsync(StringUri, NetworkTransfer.CancellationToken.Token).Result;
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
                if (Flags.CancelRequested)
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
        public string GetFilenameFromUrl(string Url)
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
        public PingReply PingAddress(string Address, int Timeout, byte[] Buffer)
        {
            var Pinger = new Ping();
            var PingerOpts = new PingOptions() { DontFragment = true };
            return Pinger.Send(Address, Timeout, Buffer, PingerOpts);
        }

        /// <inheritdoc/>
        public bool UploadFile(string FileName, string URL) =>
            UploadFile(FileName, URL, Flags.ShowProgress);

        /// <inheritdoc/>
        public bool UploadFile(string FileName, string URL, bool ShowProgress)
        {
            // Intialize variables
            var FileUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (NetworkTransfer.UploadNotificationProvoke)
            {
                NetworkTransfer.UploadNotif = new Notification(Translate.DoTranslation("Uploading..."), FileUri.AbsoluteUri, Notifications.NotifPriority.Low, Notifications.NotifType.Progress);
                Notifications.NotifySend(NetworkTransfer.DownloadNotif);
            }
            if (ShowProgress)
                NetworkTransfer.WClientProgress.HttpSendProgress += NetworkTransfer.HttpSendProgressWatch;

            // Send the GET request to the server for the file after getting the stream and target file stream
            DebugWriter.WriteDebug(DebugLevel.I, "Directory location: {0}", CurrentDirectory.CurrentDir);
            string FilePath = Filesystem.NeutralizePath(FileName);
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
        public bool UploadString(string URL, string Data) => 
            UploadString(URL, Data, Flags.ShowProgress);

        /// <inheritdoc/>
        public bool UploadString(string URL, string Data, bool ShowProgress)
        {
            // Intialize variables
            var StringUri = new Uri(URL);

            // Initialize the progress bar indicator and the file completed event handler
            if (NetworkTransfer.UploadNotificationProvoke)
            {
                NetworkTransfer.UploadNotif = new Notification(Translate.DoTranslation("Uploading..."), StringUri.AbsoluteUri, Notifications.NotifPriority.Low, Notifications.NotifType.Progress);
                Notifications.NotifySend(NetworkTransfer.UploadNotif);
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
    }
}
