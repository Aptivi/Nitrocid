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
using Terminaux.Shell.Switches;
using Terminaux.Shell.Arguments;
using Nitrocid.ShellPacks.Shells.SFTP.Presets;
using Nitrocid.ShellPacks.Shells.SFTP.Commands;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;

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
            new CommandInfo("cat", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_CAT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC"
                        })
                    ])
                ], new CatCommand(), CommandFlags.Wrappable),

            new CommandInfo("cdl", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_CDL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                        })
                    ])
                ], new CdlCommand()),

            new CommandInfo("cdr", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_CDR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        })
                    ])
                ], new CdrCommand()),

            new CommandInfo("del", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_DEL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_DEL_ARGUMENT_REMOTEFILE_DESC"
                        })
                    ])
                ], new DelCommand()),

            new CommandInfo("detach", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_DETACH_DESC", new DetachCommand()),

            new CommandInfo("get", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_GET_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC"
                        })
                    ])
                ], new GetCommand()),

            new CommandInfo("ifm", /* Localizable */ "NKS_SHELLPACKS_FTP_COMMAND_IFM_DESC", new IfmCommand()),

            new CommandInfo("lsl", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_LSL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", /* Localizable */ "NKS_SHELLPACKS_SFTP_COMMAND_SHOWDETAILS_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("suppressmessages", /* Localizable */ "NKS_SHELLPACKS_SFTP_COMMAND_LSL_SWITCH_SUPPRESSMESSAGES_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LslCommand(), CommandFlags.Wrappable),

            new CommandInfo("lsr", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_LSR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", /* Localizable */ "NKS_SHELLPACKS_SFTP_COMMAND_SHOWDETAILS_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LsrCommand(), CommandFlags.Wrappable),

            new CommandInfo("mkldir", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_MKLDIR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC"
                        }),
                    ], true)
                ], new MkldirCommand()),

            new CommandInfo("mkrdir", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_MKRDIR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC"
                        }),
                    ], true)
                ], new MkrdirCommand()),

            new CommandInfo("put", /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_PUT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_FTPSFTP_COMMAND_PUT_ARGUMENT_FILE_DESC"
                        })
                    ])
                ], new PutCommand()),

            new CommandInfo("pwdl", /* Localizable */ "NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDL_DESC", new PwdlCommand()),

            new CommandInfo("pwdr", /* Localizable */ "NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDR_DESC", new PwdrCommand()),
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
