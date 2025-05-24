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
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.ShellPacks.Shells.SFTP.Presets;
using Nitrocid.ShellPacks.Shells.SFTP.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;

namespace Nitrocid.ShellPacks.Shells.SFTP
{
    /// <summary>
    /// Common SFTP shell class
    /// </summary>
    internal class SFTPShellInfo : BaseShellInfo<SFTPShell>, IShellInfo
    {
        /// <summary>
        /// SFTP commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("cat", /* Localizable */ "Reads the content of a remote file to the console",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote file"
                        })
                    ])
                ], new CatCommand(), CommandFlags.Wrappable),

            new CommandInfo("cdl", /* Localizable */ "Changes local directory to download to or upload from",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local directory"
                        })
                    ])
                ], new CdlCommand()),

            new CommandInfo("cdr", /* Localizable */ "Changes remote directory to download from or upload to",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        })
                    ])
                ], new CdrCommand()),

            new CommandInfo("del", /* Localizable */ "Deletes remote file from server",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote file to delete"
                        })
                    ])
                ], new DelCommand()),

            new CommandInfo("detach", /* Localizable */ "Exits the shell without disconnecting", new DetachCommand()),

            new CommandInfo("get", /* Localizable */ "Downloads remote file to local directory using binary or text",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote file"
                        })
                    ])
                ], new GetCommand()),

            new CommandInfo("ifm", /* Localizable */ "Interactive system host file manager", new IfmCommand()),

            new CommandInfo("lsl", /* Localizable */ "Lists local directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local directory"
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", /* Localizable */ "Shows the details of the files and folders", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("suppressmessages", /* Localizable */ "Suppresses the \"unauthorized\" messages", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LslCommand(), CommandFlags.Wrappable),

            new CommandInfo("lsr", /* Localizable */ "Lists remote directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", /* Localizable */ "Shows the details of the files and folders", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LsrCommand(), CommandFlags.Wrappable),

            new CommandInfo("mkldir", /* Localizable */ "Creates a local directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local directory"
                        }),
                    ], true)
                ], new MkldirCommand()),

            new CommandInfo("mkrdir", /* Localizable */ "Creates a remote directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to remote directory"
                        }),
                    ], true)
                ], new MkrdirCommand()),

            new CommandInfo("put", /* Localizable */ "Uploads local file to remote directory using binary or text",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to local file"
                        })
                    ])
                ], new PutCommand()),

            new CommandInfo("pwdl", /* Localizable */ "Gets current local directory", new PwdlCommand()),

            new CommandInfo("pwdr", /* Localizable */ "Gets current remote directory", new PwdrCommand()),
        ];

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

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => "SFTP";
    }
}
