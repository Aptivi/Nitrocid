
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

using System.Collections.Generic;
using FluentFTP;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP
{
    /// <summary>
    /// Common FTP shell class
    /// </summary>
    public static class FTPShellCommon
    {

        internal static int verifyRetryAttempts = 3;
        internal static int connectTimeout = 15000;
        internal static int dataConnectTimeout = 15000;
        internal static FtpClient _clientFTP;
        internal static bool FtpConnected;
        internal static string FtpSite;
        internal static string FtpPass;
        internal static string FtpUser;
        internal readonly static Dictionary<string, CommandInfo> FTPModCommands = new();

        /// <summary>
        /// The FTP client used to connect to the FTP server
        /// </summary>
        public static FtpClient ClientFTP => _clientFTP;
        /// <summary>
        /// FTP verify retry attempts
        /// </summary>
        public static int FtpVerifyRetryAttempts
        {
            get => verifyRetryAttempts;
            set => verifyRetryAttempts = value < 0 ? 3 : value;
        }
        /// <summary>
        /// FTP connection timeout in milliseconds
        /// </summary>
        public static int FtpConnectTimeout
        {
            get => connectTimeout;
            set => connectTimeout = value < 0 ? 15000 : value;
        }
        /// <summary>
        /// FTP data connection timeout in milliseconds
        /// </summary>
        public static int FtpDataConnectTimeout
        {
            get => dataConnectTimeout;
            set => dataConnectTimeout = value < 0 ? 15000 : value;
        }
        /// <summary>
        /// FTP current local directory
        /// </summary>
        public static string FtpCurrentDirectory { get; set; }
        /// <summary>
        /// FTP current remote directory
        /// </summary>
        public static string FtpCurrentRemoteDir { get; set; }
        /// <summary>
        /// FTP show details in list
        /// </summary>
        public static bool FtpShowDetailsInList { get; set; } = true;
        /// <summary>
        /// FTP user prompt style
        /// </summary>
        public static string FtpUserPromptStyle { get; set; } = "";
        /// <summary>
        /// FTP password prompt style
        /// </summary>
        public static string FtpPassPromptStyle { get; set; } = "";
        /// <summary>
        /// FTP always use first profile
        /// </summary>
        public static bool FtpUseFirstProfile { get; set; }
        /// <summary>
        /// FTP add new connections to speed dial
        /// </summary>
        public static bool FtpNewConnectionsToSpeedDial { get; set; } = true;
        /// <summary>
        /// FTP try to validate the certificate
        /// </summary>
        public static bool FtpTryToValidateCertificate { get; set; } = true;
        /// <summary>
        /// FTP recursive hashing
        /// </summary>
        public static bool FtpRecursiveHashing { get; set; }
        /// <summary>
        /// FTP show MOTD
        /// </summary>
        public static bool FtpShowMotd { get; set; } = true;
        /// <summary>
        /// FTP always accept invalid certificates. Turning it on is not recommended.
        /// </summary>
        public static bool FtpAlwaysAcceptInvalidCerts { get; set; }
        /// <summary>
        /// FTP protocol versions
        /// </summary>
        public static FtpIpVersion FtpProtocolVersions { get; set; } = FtpIpVersion.ANY;

    }
}
