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
using Nitrocid.ShellPacks.Shells.Mail.Presets;
using Nitrocid.ShellPacks.Shells.Mail.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Languages;

namespace Nitrocid.ShellPacks.Shells.Mail
{
    /// <summary>
    /// Common mail shell class
    /// </summary>
    internal class MailShellInfo : BaseShellInfo<MailShell>, IShellInfo
    {
        /// <summary>
        /// Mail commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("cd", "NKS_SHELLPACKS_MAIL_COMMAND_CD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "folder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC"
                        })
                    ])
                ], new CdCommand()),

            new CommandInfo("detach", "NKS_SHELLPACKS_FTPSFTP_COMMAND_DETACH_DESC", new DetachCommand()),

            new CommandInfo("lsdirs", "NKS_SHELLPACKS_MAIL_COMMAND_LSDIRS_DESC", new LsDirsCommand()),

            new CommandInfo("list", "NKS_SHELLPACKS_MAIL_COMMAND_LIST_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "pageNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_LIST_ARGUMENT_PAGENUM_DESC"
                        })
                    ])
                ], new ListCommand()),

            new CommandInfo("mkdir", "NKS_SHELLPACKS_MAIL_COMMAND_MKDIR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "foldername", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC"
                        })
                    ])
                ], new MkdirCommand()),

            new CommandInfo("mv", "NKS_SHELLPACKS_MAIL_COMMAND_MV_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "mailId", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_MAILID_DESC"
                        }),
                        new CommandArgumentPart(true, "targetFolder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC"
                        })
                    ])
                ], new MvCommand()),

            new CommandInfo("mvall", "NKS_SHELLPACKS_MAIL_COMMAND_MVALL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "senderName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_SENDERNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "targetFolder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC"
                        })
                    ])
                ], new MvAllCommand()),

            new CommandInfo("read", "NKS_SHELLPACKS_MAIL_COMMAND_READ_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "mailid", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_MAILID_DESC"
                        })
                    ])
                ], new ReadCommand()),

            new CommandInfo("readenc", "NKS_SHELLPACKS_MAIL_COMMAND_READENC_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "mailid", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_MAILID_DESC"
                        })
                    ])
                ], new ReadEncCommand()),

            new CommandInfo("ren", "NKS_SHELLPACKS_MAIL_COMMAND_REN_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "oldFolderName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_REN_ARGUMENT_OLDFOLDERNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "newFolderName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_REN_ARGUMENT_NEWFOLDERNAME_DESC"
                        })
                    ])
                ], new RenCommand()),

            new CommandInfo("rm", "NKS_SHELLPACKS_MAIL_COMMAND_RM_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "mailid", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_MAILID_DESC"
                        })
                    ])
                ], new RmCommand()),

            new CommandInfo("rmall", "NKS_SHELLPACKS_MAIL_COMMAND_RMALL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sendername", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_SENDERNAME_DESC"
                        })
                    ])
                ], new RmAllCommand()),

            new CommandInfo("rmdir", "NKS_SHELLPACKS_MAIL_COMMAND_RMDIR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "foldername", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC"
                        })
                    ])
                ], new RmdirCommand()),

            new CommandInfo("send", "NKS_SHELLPACKS_MAIL_COMMAND_SEND_DESC", new SendCommand()),

            new CommandInfo("sendenc", "NKS_SHELLPACKS_MAIL_COMMAND_SENDENC_DESC", new SendEncCommand()),

            new CommandInfo("tui", "NKS_SHELLPACKS_MAIL_COMMAND_TUI_DESC", new TuiCommand()),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new MailDefaultPreset() },
            { "PowerLine1", new MailPowerLine1Preset() },
            { "PowerLine2", new MailPowerLine2Preset() },
            { "PowerLine3", new MailPowerLine3Preset() },
            { "PowerLineBG1", new MailPowerLineBG1Preset() },
            { "PowerLineBG2", new MailPowerLineBG2Preset() },
            { "PowerLineBG3", new MailPowerLineBG3Preset() }
        };

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => "Mail";
    }
}
