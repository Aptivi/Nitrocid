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

using System.Collections.Generic;
using FluentFTP;
using Nitrocid.Network.Connections;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.ShellPacks.Shells.FTP
{
    /// <summary>
    /// Common FTP shell class
    /// </summary>
    public static class FTPShellCommon
    {

        internal static int verifyRetryAttempts = 3;
        internal static int connectTimeout = 15000;
        internal static int dataConnectTimeout = 15000;
        internal static NetworkConnection? clientConnection;
        internal static string FtpSite = "";
        internal static string FtpPass = "";
        internal static string FtpUser = "";
        internal readonly static List<CommandInfo> FTPModCommands = [];

        /// <summary>
        /// The FTP client used to connect to the FTP server
        /// </summary>
        public static NetworkConnection? ClientFTP =>
            clientConnection;
        /// <summary>
        /// FTP verify retry attempts
        /// </summary>
        public static int FtpVerifyRetryAttempts =>
            ShellsInit.ShellsConfig.FtpVerifyRetryAttempts;
        /// <summary>
        /// FTP connection timeout in milliseconds
        /// </summary>
        public static int FtpConnectTimeout =>
            ShellsInit.ShellsConfig.FtpConnectTimeout;
        /// <summary>
        /// FTP data connection timeout in milliseconds
        /// </summary>
        public static int FtpDataConnectTimeout =>
            ShellsInit.ShellsConfig.FtpDataConnectTimeout;
        /// <summary>
        /// FTP show details in list
        /// </summary>
        public static bool FtpShowDetailsInList =>
            ShellsInit.ShellsConfig.FtpShowDetailsInList;
        /// <summary>
        /// FTP user prompt style
        /// </summary>
        public static string FtpUserPromptStyle =>
            ShellsInit.ShellsConfig.FtpUserPromptStyle;
        /// <summary>
        /// FTP password prompt style
        /// </summary>
        public static string FtpPassPromptStyle =>
            ShellsInit.ShellsConfig.FtpPassPromptStyle;
        /// <summary>
        /// FTP always use first profile
        /// </summary>
        public static bool FtpUseFirstProfile =>
            ShellsInit.ShellsConfig.FtpUseFirstProfile;
        /// <summary>
        /// FTP add new connections to speed dial
        /// </summary>
        public static bool FtpNewConnectionsToSpeedDial =>
            ShellsInit.ShellsConfig.FtpNewConnectionsToSpeedDial;
        /// <summary>
        /// FTP try to validate the certificate
        /// </summary>
        public static bool FtpTryToValidateCertificate =>
            ShellsInit.ShellsConfig.FtpTryToValidateCertificate;
        /// <summary>
        /// FTP recursive hashing
        /// </summary>
        public static bool FtpRecursiveHashing =>
            ShellsInit.ShellsConfig.FtpRecursiveHashing;
        /// <summary>
        /// FTP show MOTD
        /// </summary>
        public static bool FtpShowMotd =>
            ShellsInit.ShellsConfig.FtpShowMotd;
        /// <summary>
        /// FTP always accept invalid certificates. Turning it on is not recommended.
        /// </summary>
        public static bool FtpAlwaysAcceptInvalidCerts =>
            ShellsInit.ShellsConfig.FtpAlwaysAcceptInvalidCerts;
        /// <summary>
        /// FTP protocol versions
        /// </summary>
        public static FtpIpVersion FtpProtocolVersions =>
            (FtpIpVersion)ShellsInit.ShellsConfig.FtpProtocolVersions;
        /// <summary>
        /// FTP current local directory
        /// </summary>
        public static string FtpCurrentDirectory { get; set; } = "";
        /// <summary>
        /// FTP current remote directory
        /// </summary>
        public static string FtpCurrentRemoteDir { get; set; } = "";

    }
}
