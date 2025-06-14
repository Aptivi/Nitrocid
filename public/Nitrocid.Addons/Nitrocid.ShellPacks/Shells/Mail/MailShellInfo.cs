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
            new CommandInfo("cd", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_CD_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "folder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new CdCommand()),

            new CommandInfo("detach", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_DETACH_DESC", "Nitrocid.ShellPacks"), new DetachCommand()),

            new CommandInfo("lsdirs", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_LSDIRS_DESC", "Nitrocid.ShellPacks"), new LsDirsCommand()),

            new CommandInfo("list", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_LIST_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "pageNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_LIST_ARGUMENT_PAGENUM_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new ListCommand()),

            new CommandInfo("mkdir", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_MKDIR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "foldername", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new MkdirCommand()),

            new CommandInfo("mv", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_MV_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "mailId", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_MAILID_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "targetFolder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new MvCommand()),

            new CommandInfo("mvall", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_MVALL_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "senderName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_SENDERNAME_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "targetFolder", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new MvAllCommand()),

            new CommandInfo("read", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_READ_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "mailid", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_MAILID_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new ReadCommand()),

            new CommandInfo("readenc", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_READENC_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "mailid", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_MAILID_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new ReadEncCommand()),

            new CommandInfo("ren", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_REN_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "oldFolderName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_REN_ARGUMENT_OLDFOLDERNAME_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "newFolderName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_REN_ARGUMENT_NEWFOLDERNAME_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new RenCommand()),

            new CommandInfo("rm", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_RM_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "mailid", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_MAILID_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new RmCommand()),

            new CommandInfo("rmall", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_RMALL_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "sendername", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_SENDERNAME_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new RmAllCommand()),

            new CommandInfo("rmdir", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_RMDIR_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "foldername", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_ARGUMENT_FOLDER_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new RmdirCommand()),

            new CommandInfo("send", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_SEND_DESC", "Nitrocid.ShellPacks"), new SendCommand()),

            new CommandInfo("sendenc", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_SENDENC_DESC", "Nitrocid.ShellPacks"), new SendEncCommand()),

            new CommandInfo("tui", LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_COMMAND_TUI_DESC", "Nitrocid.ShellPacks"), new TuiCommand()),
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
