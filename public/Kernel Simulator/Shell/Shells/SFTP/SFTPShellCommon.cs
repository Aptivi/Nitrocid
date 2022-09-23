
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

using System.Collections.Generic;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.SFTP.Commands;
using Renci.SshNet;

namespace KS.Shell.Shells.SFTP
{
    /// <summary>
    /// SFTP shell common module
    /// </summary>
    public static class SFTPShellCommon
    {

        /// <summary>
        /// SFTP commands
        /// </summary>
        public readonly static Dictionary<string, CommandInfo> SFTPCommands = new()
        {
            { "connect", new CommandInfo("connect", ShellType.SFTPShell, "Connects to an SFTP server (it must start with \"sftp://\")", new CommandArgumentInfo(new[] { "<server>" }, true, 1), new SFTP_ConnectCommand()) },
            { "cdl", new CommandInfo("cdl", ShellType.SFTPShell, "Changes local directory to download to or upload from", new CommandArgumentInfo(new[] { "<directory>" }, true, 1), new SFTP_CdlCommand()) },
            { "cdr", new CommandInfo("cdr", ShellType.SFTPShell, "Changes remote directory to download from or upload to", new CommandArgumentInfo(new[] { "<directory>" }, true, 1), new SFTP_CdrCommand()) },
            { "del", new CommandInfo("del", ShellType.SFTPShell, "Deletes remote file from server", new CommandArgumentInfo(new[] { "<file>" }, true, 1), new SFTP_DelCommand()) },
            { "disconnect", new CommandInfo("disconnect", ShellType.SFTPShell, "Disconnects from server", new CommandArgumentInfo(), new SFTP_DisconnectCommand()) },
            { "get", new CommandInfo("get", ShellType.SFTPShell, "Downloads remote file to local directory using binary or text", new CommandArgumentInfo(new[] { "<file>" }, true, 1), new SFTP_GetCommand()) },
            { "lsl", new CommandInfo("lsl", ShellType.SFTPShell, "Lists local directory", new CommandArgumentInfo(new[] { "[-showdetails|-suppressmessages] [dir]" }, false, 0), new SFTP_LslCommand()) },
            { "lsr", new CommandInfo("lsr", ShellType.SFTPShell, "Lists remote directory", new CommandArgumentInfo(new[] { "[-showdetails] [dir]" }, false, 0), new SFTP_LsrCommand()) },
            { "put", new CommandInfo("put", ShellType.SFTPShell, "Uploads local file to remote directory using binary or text", new CommandArgumentInfo(new[] { "<file>" }, true, 1), new SFTP_PutCommand()) },
            { "pwdl", new CommandInfo("pwdl", ShellType.SFTPShell, "Gets current local directory", new CommandArgumentInfo(), new SFTP_PwdlCommand()) },
            { "pwdr", new CommandInfo("pwdr", ShellType.SFTPShell, "Gets current remote directory", new CommandArgumentInfo(), new SFTP_PwdrCommand()) },
            { "quickconnect", new CommandInfo("quickconnect", ShellType.SFTPShell, "Uses information from Speed Dial to connect to any network quickly", new CommandArgumentInfo(), new SFTP_QuickConnectCommand()) }
        };
        /// <summary>
        /// SFTP current local directory
        /// </summary>
        public static string SFTPCurrDirect;
        /// <summary>
        /// SFTP current remote directory
        /// </summary>
        public static string SFTPCurrentRemoteDir;
        /// <summary>
        /// SFTP show file details in list
        /// </summary>
        public static bool SFTPShowDetailsInList = true;
        /// <summary>
        /// SFTP user prompt style
        /// </summary>
        public static string SFTPUserPromptStyle = "";
        /// <summary>
        /// SFTP add new connections to speed dial
        /// </summary>
        public static bool SFTPNewConnectionsToSpeedDial = true;
        internal static bool SFTPConnected;
        internal static SftpClient _clientSFTP;
        internal static string SFTPSite;
        internal static string SFTPPass;
        internal static string SFTPUser;
        internal readonly static Dictionary<string, CommandInfo> SFTPModCommands = new();

        /// <summary>
        /// The SFTP client used to connect to the SFTP server
        /// </summary>
        public static SftpClient ClientSFTP => _clientSFTP;

    }
}
