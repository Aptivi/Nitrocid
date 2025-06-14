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
using Nitrocid.Languages;

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
            new CommandInfo("cat", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_CAT_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new CatCommand(), CommandFlags.Wrappable),

            new CommandInfo("cdl", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_CDL_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new CdlCommand()),

            new CommandInfo("cdr", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_CDR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new CdrCommand()),

            new CommandInfo("del", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_DEL_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_DEL_ARGUMENT_REMOTEFILE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new DelCommand()),

            new CommandInfo("detach", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_DETACH_DESC", "Nitrocid.ShellPacks"), new DetachCommand()),

            new CommandInfo("get", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_GET_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEFILE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new GetCommand()),

            new CommandInfo("ifm", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_COMMAND_IFM_DESC", "Nitrocid.ShellPacks"), new IfmCommand()),

            new CommandInfo("lsl", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_LSL_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_COMMAND_SHOWDETAILS_DESC", "Nitrocid.ShellPacks"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("suppressmessages", LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_COMMAND_LSL_SWITCH_SUPPRESSMESSAGES_DESC", "Nitrocid.ShellPacks"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LslCommand(), CommandFlags.Wrappable),

            new CommandInfo("lsr", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_LSR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        })
                    ],
                    [
                        new SwitchInfo("showdetails", LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_COMMAND_SHOWDETAILS_DESC", "Nitrocid.ShellPacks"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new LsrCommand(), CommandFlags.Wrappable),

            new CommandInfo("mkldir", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_MKLDIR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_LOCALDIR_DESC", "Nitrocid.ShellPacks")
                        }),
                    ], true)
                ], new MkldirCommand()),

            new CommandInfo("mkrdir", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_MKRDIR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_ARGUMENT_REMOTEDIR_DESC", "Nitrocid.ShellPacks")
                        }),
                    ], true)
                ], new MkrdirCommand()),

            new CommandInfo("put", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_COMMAND_PUT_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_PUT_ARGUMENT_FILE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new PutCommand()),

            new CommandInfo("pwdl", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDL_DESC", "Nitrocid.ShellPacks"), new PwdlCommand()),

            new CommandInfo("pwdr", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_FS_COMMAND_PWDR_DESC", "Nitrocid.ShellPacks"), new PwdrCommand()),
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
