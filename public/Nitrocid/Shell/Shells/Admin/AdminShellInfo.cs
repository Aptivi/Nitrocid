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
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Arguments;
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.Shells.Admin.Commands;
using Nitrocid.Shell.Shells.Admin.Presets;
using Nitrocid.Languages;

namespace Nitrocid.Shell.Shells.Admin
{
    /// <summary>
    /// Common admin shell class
    /// </summary>
    internal class AdminShellInfo : BaseShellInfo<AdminShell>, IShellInfo
    {
        /// <summary>
        /// Admin commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("arghelp", "NKS_SHELL_SHELLS_ADMIN_COMMAND_ARGHELP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "argument", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => [.. ArgumentParse.AvailableCMDLineArgs.Keys],
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_ARGHELP_ARGUMENT_ARGUMENT_DESC"
                        })
                    ])
                ], new ArgHelpCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("bootlog", "NKS_SHELL_SHELLS_ADMIN_COMMAND_BOOTLOG_DESC", new BootLogCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("cdbglog", "NKS_SHELL_SHELLS_ADMIN_CDBGLOG_DESC", new CdbgLogCommand()),

            new CommandInfo("clearfiredevents", "NKS_SHELL_SHELLS_ADMIN_COMMAND_CLEARFIREDEVENTS_DESC", new ClearFiredEventsCommand()),

            new CommandInfo("journal", "NKS_SHELL_SHELLS_ADMIN_COMMAND_JOURNAL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "sessionNum", new()
                        {
                            IsNumeric = true,
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_JOURNAL_ARGUMENT_SESSIONNUM_DESC"
                        }),
                    ])
                ], new JournalCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsevents", "NKS_SHELL_SHELLS_ADMIN_COMMAND_LSEVENTS_DESC", new LsEventsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("lsusers", "NKS_SHELL_SHELLS_ADMIN_COMMAND_LSUSERS_DESC",
                [
                    new CommandArgumentInfo(true)
                ], new LsUsersCommand()),

            new CommandInfo("savenotifs", "NKS_SHELL_SHELLS_ADMIN_COMMAND_SAVENOTIFS_DESC", new SaveNotifsCommand()),

            new CommandInfo("userflag", "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "user", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_USER_DESC"
                        }),
                        new CommandArgumentPart(true, "admin/anonymous/disabled", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_TYPE_DESC"
                        }),
                        new CommandArgumentPart(true, "false/true", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_GRANT_DESC"
                        })
                    ])
                ], new UserFlagCommand()),

            new CommandInfo("userfullname", "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFULLNAME_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "user", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_USER_DESC"
                        }),
                        new CommandArgumentPart(true, "name/clear", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFULLNAME_ARGUMENT_NEWNAME_DESC"
                        })
                    ])
                ], new UserFullNameCommand()),

            new CommandInfo("userinfo", "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "user", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_USER_DESC"
                        })
                    ])
                ], new UserInfoCommand()),

            new CommandInfo("userlang", "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERLANG_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "user", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_USER_DESC"
                        }),
                        new CommandArgumentPart(true, "lang/clear", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERLANG_ARGUMENT_LANGID_DESC"
                        })
                    ])
                ], new UserLangCommand()),

            new CommandInfo("userculture", "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERCULTURE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "user", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERFLAG_ARGUMENT_USER_DESC"
                        }),
                        new CommandArgumentPart(true, "culture/clear", new()
                        {
                            ArgumentDescription = "NKS_SHELL_SHELLS_ADMIN_COMMAND_USERCULTURE_ARGUMENT_CULTUREID_DESC"
                        })
                    ])
                ], new UserCultureCommand()),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new AdminDefaultPreset() },
            { "PowerLine1", new AdminPowerLine1Preset() },
            { "PowerLine2", new AdminPowerLine2Preset() },
            { "PowerLine3", new AdminPowerLine3Preset() },
            { "PowerLineBG1", new AdminPowerLineBG1Preset() },
            { "PowerLineBG2", new AdminPowerLineBG2Preset() },
            { "PowerLineBG3", new AdminPowerLineBG3Preset() }
        };
    }
}
