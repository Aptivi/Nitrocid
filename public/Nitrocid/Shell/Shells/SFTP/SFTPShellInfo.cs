
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
using KS.Shell.Prompts.Presets.SFTP;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.SFTP.Commands;
using System;

namespace KS.Shell.Shells.SFTP
{
    /// <summary>
    /// Common SFTP shell class
    /// </summary>
    internal class SFTPShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// SFTP commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "cat", new CommandInfo("cat", ShellType, /* Localizable */ "Reads the content of a remote file to the console",
                new CommandArgumentInfo(new[] { "file" }, Array.Empty<SwitchInfo>(), true, 1), new SFTP_CatCommand(), CommandFlags.Wrappable) },
            { "cdl", new CommandInfo("cdl", ShellType, /* Localizable */ "Changes local directory to download to or upload from",
                new CommandArgumentInfo(new[] { "directory" }, Array.Empty<SwitchInfo>(), true, 1), new SFTP_CdlCommand()) },
            { "cdr", new CommandInfo("cdr", ShellType, /* Localizable */ "Changes remote directory to download from or upload to",
                new CommandArgumentInfo(new[] { "directory" }, Array.Empty<SwitchInfo>(), true, 1), new SFTP_CdrCommand()) },
            { "del", new CommandInfo("del", ShellType, /* Localizable */ "Deletes remote file from server",
                new CommandArgumentInfo(new[] { "file" }, Array.Empty<SwitchInfo>(), true, 1), new SFTP_DelCommand()) },
            { "detach", new CommandInfo("detach", ShellType, /* Localizable */ "Exits the shell without disconnecting",
                new CommandArgumentInfo(), new SFTP_DetachCommand()) },
            { "get", new CommandInfo("get", ShellType, /* Localizable */ "Downloads remote file to local directory using binary or text",
                new CommandArgumentInfo(new[] { "file" }, Array.Empty<SwitchInfo>(), true, 1), new SFTP_GetCommand()) },
            { "lsl", new CommandInfo("lsl", ShellType, /* Localizable */ "Lists local directory",
                new CommandArgumentInfo(new[] { "dir" }, new[] { new SwitchInfo("showdetails", /* Localizable */ "Shows the details of the files and folders"), new SwitchInfo("suppressmessages", /* Localizable */ "Suppresses the \"unauthorized\" messages") }, false, 0), new SFTP_LslCommand()) },
            { "lsr", new CommandInfo("lsr", ShellType, /* Localizable */ "Lists remote directory",
                new CommandArgumentInfo(new[] { "dir" }, new[] { new SwitchInfo("showdetails", /* Localizable */ "Shows the details of the files and folders") }, false, 0), new SFTP_LsrCommand()) },
            { "put", new CommandInfo("put", ShellType, /* Localizable */ "Uploads local file to remote directory using binary or text",
                new CommandArgumentInfo(new[] { "file" }, Array.Empty<SwitchInfo>(), true, 1), new SFTP_PutCommand()) },
            { "pwdl", new CommandInfo("pwdl", ShellType, /* Localizable */ "Gets current local directory",
                new CommandArgumentInfo(), new SFTP_PwdlCommand()) },
            { "pwdr", new CommandInfo("pwdr", ShellType, /* Localizable */ "Gets current remote directory",
                new CommandArgumentInfo(), new SFTP_PwdrCommand()) },
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new SFTPDefaultPreset() },
            { "PowerLine1", new SftpPowerLine1Preset() },
            { "PowerLine2", new SftpPowerLine2Preset() },
            { "PowerLine3", new SftpPowerLine3Preset() },
            { "PowerLineBG1", new SftpPowerLineBG1Preset() },
            { "PowerLineBG2", new SftpPowerLineBG2Preset() },
            { "PowerLineBG3", new SftpPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new SFTPShell();

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["SFTPShell"];

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => Network.Base.Connections.NetworkConnectionType.SFTP.ToString();

    }
}
