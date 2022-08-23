
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using FluentFTP;
using KS.Files;
using KS.Files.Operations;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Network
{
    public static class NetworkTools
    {

        // Variables
        public static int DownloadRetries = 3;
        public static int UploadRetries = 3;
        public static int PingTimeout = 60000;
        internal static bool TransferFinished;

        public enum SpeedDialType
        {
            /// <summary>
            /// FTP speed dial
            /// </summary>
            FTP,
            /// <summary>
            /// SFTP speed dial
            /// </summary>
            SFTP
        }

        /// <summary>
        /// Checks to see if the network is available
        /// </summary>
        public static bool NetworkAvailable
        {
            get
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
        }

        /// <summary>
        /// Pings an address
        /// </summary>
        /// <param name="Address">Target address</param>
        /// <returns>A ping reply status</returns>
        public static PingReply PingAddress(string Address)
        {
            var Pinger = new Ping();
            var PingerOpts = new PingOptions() { DontFragment = true };
            var PingBuffer = Encoding.ASCII.GetBytes("Kernel Simulator");
            int Timeout = PingTimeout; // 60 seconds = 1 minute. timeout of Pinger.Send() takes milliseconds.
            return PingAddress(Address, Timeout, PingBuffer);
        }

        /// <summary>
        /// Pings an address
        /// </summary>
        /// <param name="Address">Target address</param>
        /// <returns>A ping reply status</returns>
        public static PingReply PingAddress(string Address, int Timeout)
        {
            var Pinger = new Ping();
            var PingerOpts = new PingOptions() { DontFragment = true };
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
            Kernel.Kernel.HostName = NewHost;
            var Token = ConfigTools.GetConfigCategory(Config.ConfigCategory.Login);
            ConfigTools.SetConfigValue(Config.ConfigCategory.Login, Token, "Host Name", Kernel.Kernel.HostName);
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
                DebugWriter.WStkTrc(ex);
                DebugWriter.Wdbg(DebugLevel.E, "Failed to change hostname: {0}", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Adds an entry to speed dial
        /// </summary>
        /// <param name="Address">A speed dial address</param>
        /// <param name="Port">A speed dial port</param>
        /// <param name="User">A speed dial username</param>
        /// <param name="EncryptionMode">A speed dial encryption mode</param>
        /// <param name="SpeedDialType">Speed dial type</param>
        /// <param name="ThrowException">Optionally throw exception</param>
        public static void AddEntryToSpeedDial(string Address, int Port, string User, SpeedDialType SpeedDialType, FtpEncryptionMode EncryptionMode = FtpEncryptionMode.None, bool ThrowException = true)
        {
            string PathName = SpeedDialType == SpeedDialType.SFTP ? "SFTPSpeedDial" : "FTPSpeedDial";
            KernelPathType SpeedDialEnum = (KernelPathType)Convert.ToInt32(Enum.Parse(typeof(KernelPathType), PathName));
            Making.MakeFile(Paths.GetKernelPath(SpeedDialEnum), false);
            string SpeedDialJsonContent = File.ReadAllText(Paths.GetKernelPath(SpeedDialEnum));
            if (SpeedDialJsonContent.StartsWith("["))
            {
                ConvertSpeedDialEntries(SpeedDialType);
                SpeedDialJsonContent = File.ReadAllText(Paths.GetKernelPath(SpeedDialEnum));
            }
            var SpeedDialToken = JObject.Parse(!string.IsNullOrEmpty(SpeedDialJsonContent) ? SpeedDialJsonContent : "{}");
            if (SpeedDialToken[Address] is null)
            {
                var NewSpeedDial = new JObject(new JProperty("Address", Address), new JProperty("Port", Port), new JProperty("User", User), new JProperty("Type", SpeedDialType), new JProperty("FTP Encryption Mode", EncryptionMode));
                SpeedDialToken.Add(Address, NewSpeedDial);
                File.WriteAllText(Paths.GetKernelPath(SpeedDialEnum), JsonConvert.SerializeObject(SpeedDialToken, Formatting.Indented));
            }
            else if (ThrowException)
            {
                if (SpeedDialType == SpeedDialType.FTP)
                {
                    throw new Kernel.Exceptions.FTPNetworkException(Translate.DoTranslation("Entry already exists."));
                }
                else if (SpeedDialType == SpeedDialType.SFTP)
                {
                    throw new Kernel.Exceptions.SFTPNetworkException(Translate.DoTranslation("Entry already exists."));
                }
            }
        }

        /// <summary>
        /// Adds an entry to speed dial
        /// </summary>
        /// <param name="Address">A speed dial address</param>
        /// <param name="Port">A speed dial port</param>
        /// <param name="User">A speed dial username</param>
        /// <param name="EncryptionMode">A speed dial encryption mode</param>
        /// <param name="SpeedDialType">Speed dial type</param>
        /// <param name="ThrowException">Optionally throw exception</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryAddEntryToSpeedDial(string Address, int Port, string User, SpeedDialType SpeedDialType, FtpEncryptionMode EncryptionMode = FtpEncryptionMode.None, bool ThrowException = true)
        {
            try
            {
                AddEntryToSpeedDial(Address, Port, User, SpeedDialType, EncryptionMode, ThrowException);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Lists all speed dial entries
        /// </summary>
        /// <param name="SpeedDialType">Speed dial type</param>
        /// <returns>A list</returns>
        public static Dictionary<string, JToken> ListSpeedDialEntries(SpeedDialType SpeedDialType)
        {
            string PathName = SpeedDialType == SpeedDialType.SFTP ? "SFTPSpeedDial" : "FTPSpeedDial";
            KernelPathType SpeedDialEnum = (KernelPathType)Convert.ToInt32(Enum.Parse(typeof(KernelPathType), PathName));
            Making.MakeFile(Paths.GetKernelPath(SpeedDialEnum), false);
            string SpeedDialJsonContent = File.ReadAllText(Paths.GetKernelPath(SpeedDialEnum));
            if (SpeedDialJsonContent.StartsWith("["))
            {
                ConvertSpeedDialEntries(SpeedDialType);
                SpeedDialJsonContent = File.ReadAllText(Paths.GetKernelPath(SpeedDialEnum));
            }
            var SpeedDialToken = JObject.Parse(!string.IsNullOrEmpty(SpeedDialJsonContent) ? SpeedDialJsonContent : "{}");
            var SpeedDialEntries = new Dictionary<string, JToken>();
            foreach (var SpeedDialAddress in SpeedDialToken.Properties())
                SpeedDialEntries.Add(SpeedDialAddress.Name, SpeedDialAddress.Value);
            return SpeedDialEntries;
        }

        /// <summary>
        /// Convert speed dial entries from the old jsonified version (pre-0.0.16 RC1) to the new jsonified version
        /// </summary>
        /// <param name="SpeedDialType">Speed dial type</param>
        public static void ConvertSpeedDialEntries(SpeedDialType SpeedDialType)
        {
            string PathName = SpeedDialType == SpeedDialType.SFTP ? "SFTPSpeedDial" : "FTPSpeedDial";
            KernelPathType SpeedDialEnum = (KernelPathType)Convert.ToInt32(Enum.Parse(typeof(KernelPathType), PathName));
            string SpeedDialJsonContent = File.ReadAllText(Paths.GetKernelPath(SpeedDialEnum));
            var SpeedDialToken = JArray.Parse(!string.IsNullOrEmpty(SpeedDialJsonContent) ? SpeedDialJsonContent : "[]");
            File.Delete(Paths.GetKernelPath(SpeedDialEnum));
            foreach (string SpeedDialEntry in SpeedDialToken)
            {
                var ChosenLineSeparation = SpeedDialEntry.Split(',');
                string Address = ChosenLineSeparation[0];
                string Port = ChosenLineSeparation[1];
                string Username = ChosenLineSeparation[2];
                FtpEncryptionMode Encryption = (FtpEncryptionMode)Convert.ToInt32(SpeedDialType == SpeedDialType.FTP ? Enum.Parse(typeof(FtpEncryptionMode), ChosenLineSeparation[3]) : FtpEncryptionMode.None);
                AddEntryToSpeedDial(Address, Convert.ToInt32(Port), Username, SpeedDialType, Encryption, false);
            }
        }

        /// <summary>
        /// Gets the filename from the URL
        /// </summary>
        /// <param name="Url">The target URL that contains the filename</param>
        public static string GetFilenameFromUrl(string Url)
        {
            string FileName = Url.Split('/').Last();
            DebugWriter.Wdbg(DebugLevel.I, "Prototype Filename: {0}", FileName);
            if (FileName.Contains(Convert.ToString('?')))
            {
                FileName = FileName.Remove(FileName.IndexOf('?'));
            }
            DebugWriter.Wdbg(DebugLevel.I, "Finished Filename: {0}", FileName);
            return FileName;
        }

    }
}
