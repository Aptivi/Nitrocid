//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using KS.Drivers;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Network.Base.Transfer;

namespace KS.Network.Base
{
    /// <summary>
    /// Network tools module
    /// </summary>
    public static class NetworkTools
    {

        internal static int downloadRetries = 3;
        internal static int uploadRetries = 3;
        internal static int pingTimeout = 60000;
        internal static bool TransferFinished;

        /// <summary>
        /// Current kernel host name
        /// </summary>
        public static string HostName =>
            Config.MainConfig.HostName;

        /// <summary>
        /// Download retries before giving up
        /// </summary>
        public static int DownloadRetries =>
            Config.MainConfig.DownloadRetries;

        /// <summary>
        /// Upload retries before giving up
        /// </summary>
        public static int UploadRetries =>
            Config.MainConfig.UploadRetries;

        /// <summary>
        /// Ping timeout in milliseconds
        /// </summary>
        public static int PingTimeout =>
            Config.MainConfig.PingTimeout;

        /// <summary>
        /// Checks to see if the network is available. On Android systems, if there is no Internet connection, the network is considered
        /// unavailable.
        /// </summary>
        public static bool NetworkAvailable =>
            KernelPlatform.IsOnAndroid() ?
            IsInternetAvailableNoNetworkCheck() :
            NetworkInterface.GetIsNetworkAvailable();

        /// <summary>
        /// Checks to see if the Internet connection is available
        /// </summary>
        public static bool InternetAvailable =>
            IsInternetAvailable();

        /// <summary>
        /// Shows the progress bar while downloading using the {Down|Up}load{File|String}() API.
        /// </summary>
        public static bool ShowProgress =>
            Config.MainConfig.ShowProgress;

        /// <summary>
        /// Pings an address
        /// </summary>
        /// <param name="Address">Target address</param>
        /// <param name="options">Ping options</param>
        /// <returns>A ping reply status</returns>
        public static PingReply PingAddress(string Address, PingOptions options = null)
        {
            // 60 seconds = 1 minute. timeout of Pinger.Send() takes milliseconds.
            var PingBuffer = Encoding.ASCII.GetBytes("Nitrocid KS");
            int Timeout = PingTimeout;
            return PingAddress(Address, Timeout, PingBuffer, options);
        }

        /// <summary>
        /// Pings an address
        /// </summary>
        /// <param name="Address">Target address</param>
        /// <param name="Timeout">Ping timeout in milliseconds</param>
        /// <param name="options">Ping options</param>
        /// <returns>A ping reply status</returns>
        public static PingReply PingAddress(string Address, int Timeout, PingOptions options = null)
        {
            var PingBuffer = Encoding.ASCII.GetBytes("Nitrocid KS");
            return PingAddress(Address, Timeout, PingBuffer, options);
        }

        /// <summary>
        /// Pings an address
        /// </summary>
        /// <param name="Address">Target address</param>
        /// <param name="Timeout">Timeout in milliseconds</param>
        /// <param name="text">The text to buffer</param>
        /// <param name="options">Ping options</param>
        /// <returns>A ping reply status</returns>
        public static PingReply PingAddress(string Address, int Timeout, string text, PingOptions options = null)
        {
            var PingBuffer = Encoding.ASCII.GetBytes(text);
            return PingAddress(Address, Timeout, PingBuffer, options);
        }

        /// <summary>
        /// Pings an address
        /// </summary>
        /// <param name="Address">Target address</param>
        /// <param name="Timeout">Timeout in milliseconds</param>
        /// <param name="Buffer">The buffer consisting of array of bytes</param>
        /// <param name="options">Ping options</param>
        /// <returns>A ping reply status</returns>
        public static PingReply PingAddress(string Address, int Timeout, byte[] Buffer, PingOptions options = null)
        {
            var Pinger = new Ping();
            options ??= new PingOptions() { DontFragment = true };
            return Pinger.Send(Address, Timeout, Buffer, options);
        }

        /// <summary>
        /// Changes host name
        /// </summary>
        /// <param name="NewHost">New host name</param>
        public static void ChangeHostname(string NewHost)
        {
            Config.MainConfig.HostName = NewHost;
            Config.CreateConfig();
        }

        /// <summary>
        /// Changes host name
        /// </summary>
        /// <param name="NewHost">New host name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryChangeHostname(string NewHost)
        {
            try
            {
                ChangeHostname(NewHost);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to change hostname: {0}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Gets the filename from the URL
        /// </summary>
        /// <param name="Url">The target URL that contains the filename</param>
        public static string GetFilenameFromUrl(string Url)
        {
            string FileName = Url.Split('/').Last();
            DebugWriter.WriteDebug(DebugLevel.I, "Prototype Filename: {0}", FileName);
            if (FileName.Contains('?'))
            {
                FileName = FileName.Remove(FileName.IndexOf('?'));
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Finished Filename: {0}", FileName);
            return FileName;
        }

        /// <summary>
        /// Gets the online devices in your network, including the router and the broadcast address
        /// </summary>
        public static IPAddress[] GetOnlineDevicesInNetwork() =>
            DriverHandler.CurrentNetworkDriverLocal.GetOnlineDevicesInNetwork();

        private static bool IsInternetAvailable()
        {
            try
            {
                // Return false on systems without network
                if (!NetworkAvailable)
                    return false;

                // Try to ping the connectivity check site
                return IsInternetAvailableNoNetworkCheck();
            }
            catch
            {
                // Network or connection error. Return false.
                return false;
            }
        }

        private static bool IsInternetAvailableNoNetworkCheck()
        {
            try
            {
                // Try to ping the connectivity check site
                var status = NetworkTransfer.WClient.GetAsync("https://connectivitycheck.gstatic.com/generate_204").Result.StatusCode;
                return status == HttpStatusCode.NoContent;
            }
            catch
            {
                // Network or connection error. Return false.
                return false;
            }
        }

    }
}
