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

using Nitrocid.Base.Network.Connections;

namespace Nitrocid.ShellPacks.Shells.SFTP
{
    /// <summary>
    /// SFTP shell common module
    /// </summary>
    public static class SFTPShellCommon
    {

        internal static NetworkConnection? clientConnection;
        internal static string SFTPSite = "";
        internal static string SFTPPass = "";
        internal static string SFTPUser = "";

        /// <summary>
        /// The SFTP client used to connect to the SFTP server
        /// </summary>
        public static NetworkConnection? ClientSFTP =>
            clientConnection;
        /// <summary>
        /// SFTP current local directory
        /// </summary>
        public static string SFTPCurrDirect { get; set; } = "";
        /// <summary>
        /// SFTP current remote directory
        /// </summary>
        public static string SFTPCurrentRemoteDir { get; set; } = "";

    }
}
