
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using KS.Drivers;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;

namespace KS.Network.Base
{
    /// <summary>
    /// Network tools module
    /// </summary>
    public static class NetworkTools
    {

        /// <summary>
        /// Current kernel host name
        /// </summary>
        public static string HostName = "kernel";
        /// <summary>
        /// Download retries before giving up
        /// </summary>
        public static int DownloadRetries = 3;
        /// <summary>
        /// Upload retries before giving up
        /// </summary>
        public static int UploadRetries = 3;
        /// <summary>
        /// Ping timeout in milliseconds
        /// </summary>
        public static int PingTimeout = 60000;
        internal static bool TransferFinished;

        /// <summary>
        /// Checks to see if the network is available
        /// </summary>
        public static bool NetworkAvailable => NetworkInterface.GetIsNetworkAvailable();

        /// <summary>
        /// Pings an address
        /// </summary>
        /// <param name="Address">Target address</param>
        /// <returns>A ping reply status</returns>
        public static PingReply PingAddress(string Address)
        {
            var PingBuffer = Encoding.ASCII.GetBytes("Kernel Simulator");
            int Timeout = PingTimeout; // 60 seconds = 1 minute. timeout of Pinger.Send() takes milliseconds.
            return PingAddress(Address, Timeout, PingBuffer);
        }

        /// <summary>
        /// Pings an address
        /// </summary>
        /// <param name="Address">Target address</param>
        /// <param name="Timeout">Ping timeout in milliseconds</param>
        /// <returns>A ping reply status</returns>
        public static PingReply PingAddress(string Address, int Timeout)
        {
            var PingBuffer = Encoding.ASCII.GetBytes("Kernel Simulator");
            return PingAddress(Address, Timeout, PingBuffer);
        }

        /// <summary>
        /// Pings an address
        /// </summary>
        /// <param name="Address">Target address</param>
        /// <param name="Timeout">Timeout in milliseconds</param>
        /// <param name="Buffer">The buffer consisting of array of bytes</param>
        /// <returns>A ping reply status</returns>
        public static PingReply PingAddress(string Address, int Timeout, byte[] Buffer)
        {
            var Pinger = new Ping();
            var PingerOpts = new PingOptions() { DontFragment = true };
            return Pinger.Send(Address, Timeout, Buffer, PingerOpts);
        }

        /// <summary>
        /// Changes host name
        /// </summary>
        /// <param name="NewHost">New host name</param>
        public static void ChangeHostname(string NewHost)
        {
            HostName = NewHost;
            var Token = ConfigTools.GetConfigCategory(ConfigCategory.Login);
            ConfigTools.SetConfigValue(ConfigCategory.Login, Token, "Host Name", HostName);
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
            if (FileName.Contains(Convert.ToString('?')))
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
            DriverHandler.CurrentNetworkDriver.GetOnlineDevicesInNetwork();

    }
}
