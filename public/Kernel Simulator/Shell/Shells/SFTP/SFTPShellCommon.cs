
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

using Renci.SshNet;

namespace KS.Shell.Shells.SFTP
{
    /// <summary>
    /// SFTP shell common module
    /// </summary>
    public static class SFTPShellCommon
    {

        internal static bool SFTPConnected;
        internal static SftpClient _clientSFTP;
        internal static string SFTPSite;
        internal static string SFTPPass;
        internal static string SFTPUser;

        /// <summary>
        /// The SFTP client used to connect to the SFTP server
        /// </summary>
        public static SftpClient ClientSFTP => _clientSFTP;
        /// <summary>
        /// SFTP current local directory
        /// </summary>
        public static string SFTPCurrDirect { get; set; }
        /// <summary>
        /// SFTP current remote directory
        /// </summary>
        public static string SFTPCurrentRemoteDir { get; set; }
        /// <summary>
        /// SFTP show file details in list
        /// </summary>
        public static bool SFTPShowDetailsInList { get; set; } = true;
        /// <summary>
        /// SFTP user prompt style
        /// </summary>
        public static string SFTPUserPromptStyle { get; set; } = "";
        /// <summary>
        /// SFTP add new connections to speed dial
        /// </summary>
        public static bool SFTPNewConnectionsToSpeedDial { get; set; } = true;

    }
}
