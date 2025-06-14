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

using System;
using System.Collections.Generic;
using System.Linq;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Users;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Drivers.Encoding;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Languages;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Shell.Prompts;
using Nitrocid.Files.Extensions;
using Nitrocid.Shell.Shells.UESH.Commands;
using Nitrocid.Shell.Shells.UESH.Presets;
using Nitrocid.Drivers;

namespace Nitrocid.Shell.Shells.UESH
{
    /// <summary>
    /// UESH common shell properties
    /// </summary>
    internal class UESHShellInfo : BaseShellInfo<UESHShell>, IShellInfo
    {
        /// <summary>
        /// List of commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("addgroup", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDGROUP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "groupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDGROUP_ARGUMENT_GROUPNAME_DESC")
                        })
                    ])
                ], new AddGroupCommand(), CommandFlags.Strict),

            new CommandInfo("adduser", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_ARGUMENT_USERNAME_DESC")
                        }),
                        new CommandArgumentPart(false, "password", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_ARGUMENT_PASSWORD_DESC")
                        }),
                        new CommandArgumentPart(false, "confirm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_ARGUMENT_CONFIRM_DESC")
                        }),
                    ])
                ], new AddUserCommand(), CommandFlags.Strict),

            new CommandInfo("addusertogroup", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSERTOGROUP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSERTOGROUP_ARGUMENT_USERNAME_DESC")
                        }),
                        new CommandArgumentPart(true, "group", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSERTOGROUP_ARGUMENT_GROUP_DESC")
                        }),
                    ])
                ], new AddUserToGroupCommand(), CommandFlags.Strict),

            new CommandInfo("admin", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADMIN_DESC"), new AdminCommand(), CommandFlags.Strict),

            new CommandInfo("alarm", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "start", new()
                        {
                            ExactWording = ["start"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_START_DESC")
                        }),
                        new CommandArgumentPart(true, "alarmname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_ALARMNAME")
                        }),
                        new CommandArgumentPart(true, "interval", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_START_INTERVAL_DESC")
                        }),
                    ])
                    {
                        ArgChecker = (cp) => AlarmCommand.CheckArgument(cp, "start")
                    },
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "stop", new()
                        {
                            ExactWording = ["stop"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_STOP_DESC")
                        }),
                        new CommandArgumentPart(true, "alarmname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_STOP_NAME_DESC")
                        }),
                    ])
                    {
                        ArgChecker = (cp) => AlarmCommand.CheckArgument(cp, "stop")
                    },
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list", new()
                        {
                            ExactWording = ["list"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_LIST_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("tui", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_SWITCH_TUI_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ]),
                ], new AlarmCommand(), CommandFlags.Strict),

            new CommandInfo("alias", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALIAS_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "add", new()
                        {
                            ExactWording = ["add"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALIAS_ARGUMENT_ADD_DESC")
                        }),
                        new CommandArgumentPart(true, "shell", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALIAS_ARGUMENT_ADD_TYPE_DESC")
                        }),
                        new CommandArgumentPart(true, "alias", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALIAS_ARGUMENT_ADD_ALIAS_DESC")
                        }),
                        new CommandArgumentPart(true, "cmd", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALIAS_ARGUMENT_ADD_CMD_DESC")
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "rem", new()
                        {
                            ExactWording = ["rem"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALIAS_ARGUMENT_REM_DESC")
                        }),
                        new CommandArgumentPart(true, "shell", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALIAS_ARGUMENT_ADD_TYPE_DESC")
                        }),
                        new CommandArgumentPart(true, "alias", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALIAS_ARGUMENT_ADD_ALIAS_DESC")
                        }),
                    ]),
                ], new AliasCommand(), CommandFlags.Strict),

            new CommandInfo("beep", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_BEEP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "freq", new()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_BEEP_ARGUMENT_FREQ_DESC")
                        }),
                        new CommandArgumentPart(false, "ms", new()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_BEEP_ARGUMENT_INTERVAL_DESC")
                        }),
                    ])
                ], new BeepCommand()),

            new CommandInfo("blockdbgdev", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_BLOCKDBGDEV_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ipaddress", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_BLOCKDBGDEV_ARGUMENT_IPADDRESS_DESC")
                        }),
                    ])
                ], new BlockDbgDevCommand(), CommandFlags.Strict),

            new CommandInfo("bulkrename", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "targetdir", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_ARGUMENT_TARGETDIR_DESC")
                        }),
                        new CommandArgumentPart(true, "pattern", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_ARGUMENT_PATTERN_DESC")
                        }),
                        new CommandArgumentPart(false, "newname", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_ARGUMENT_NEWNAME_DESC")
                        }),
                    ])
                ], new BulkRenameCommand()),

            new CommandInfo("cat", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CAT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CAT_ARGUMENT_FILE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("lines", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CAT_SWITCH_LINES_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["nolines"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("nolines", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CAT_SWITCH_NOLINES_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["lines"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("plain", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CAT_SWITCH_PLAIN_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new CatCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("cdir", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CDIR_DESC"),
                [
                    new CommandArgumentInfo(true)
                ], new CDirCommand()),

            new CommandInfo("changes", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHANGES_DESC"), new ChangesCommand()),

            new CommandInfo("chattr", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC")
                        }),
                        new CommandArgumentPart(true, "add/rem", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_ADDREMOVE_DESC")
                        }),
                        new CommandArgumentPart(true, "Normal/ReadOnly/Hidden/Archive", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_NAME_DESC")
                        }),
                    ])
                ], new ChAttrCommand()),

            new CommandInfo("chculture", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHCULTURE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "culture", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHCULTURE_ARGUMENT_CULTUREID_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("user", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHCULTURE_SWITCH_USER_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ChCultureCommand(), CommandFlags.Strict),

            new CommandInfo("chdir", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHDIR_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory/..", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_ARGUMENT_TARGETDIR_DESC")
                        }),
                    ])
                ], new ChDirCommand()),

            new CommandInfo("chhostname", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHHOSTNAME_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "hostname", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHHOSTNAME_ARGUMENT_HOSTNAME_DESC")
                        }),
                    ])
                ], new ChHostNameCommand(), CommandFlags.Strict),

            new CommandInfo("chklock", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHKLOCK_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("waitforunlock", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHKLOCK_SWITCH_WAITFORUNLOCK_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ChkLockCommand()),

            new CommandInfo("chlang", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHLANG_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "language", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHLANG_ARGUMENT_LANGID_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("user", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHLANG_SWITCH_USER_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ChLangCommand(), CommandFlags.Strict),

            new CommandInfo("chmal", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHMAL_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "message", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHMOTD_ARGUMENT_MESSAGE_DESC")
                        }),
                    ])
                ], new ChMalCommand(), CommandFlags.Strict),

            new CommandInfo("chmotd", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHMOTD_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "message", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHMOTD_ARGUMENT_MESSAGE_DESC")
                        }),
                    ])
                ], new ChMotdCommand(), CommandFlags.Strict),

            new CommandInfo("choice", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "answers", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_ARGUMENT_ANSWERS_DESC")
                        }),
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_ARGUMENT_INPUT_DESC")
                        }),
                        new CommandArgumentPart(false, "answertitle1", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_ARGUMENT_TITLE1_DESC")
                        }),
                        new CommandArgumentPart(false, "answertitle2", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_ARGUMENT_TITLE2_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("o", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_SWITCH_O_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["t", "m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("t", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_SWITCH_T_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["o", "m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_SWITCH_M_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["t", "o"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("single", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_SWITCH_SINGLE_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["multiple"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("multiple", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_SWITCH_MULTIPLE_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["single"],
                            AcceptsValues = false
                        })
                    ], true, true)
                ], new ChoiceCommand()),

            new CommandInfo("chpwd", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHPWD_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Username", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHPWD_ARGUMENT_USERNAME_DESC")
                        }),
                        new CommandArgumentPart(true, "UserPass", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHPWD_ARGUMENT_CURRPASSWORD_DESC")
                        }),
                        new CommandArgumentPart(true, "newPass", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_ARGUMENT_PASSWORD_DESC")
                        }),
                        new CommandArgumentPart(true, "confirm", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_ARGUMENT_CONFIRM_DESC")
                        }),
                    ])
                ], new ChPwdCommand(), CommandFlags.Strict),

            new CommandInfo("chusrname", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHUSRNAME_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "oldUserName", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => [.. UserManagement.ListAllUsers()],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHUSRNAME_ARGUMENT_OLDNAME_DESC"),
                        }),
                        new CommandArgumentPart(true, "newUserName", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHUSRNAME_ARGUMENT_NEWNAME_DESC")
                        }),
                    ])
                ], new ChUsrNameCommand(), CommandFlags.Strict),

            new CommandInfo("cls", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CLS_DESC"), new ClsCommand()),

            new CommandInfo("combinestr", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_FIRSTINPUT_DESC")
                        }),
                        new CommandArgumentPart(true, "input2", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_SECONDINPUT_DESC")
                        }),
                        new CommandArgumentPart(false, "input3", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_THIRDINPUT_DESC")
                        }),
                    ], true, true)
                ], new CombineStrCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("combine", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "output", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINE_ARGUMENT_OUTPUT_DESC")
                        }),
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_FIRSTINPUT_DESC")
                        }),
                        new CommandArgumentPart(true, "input2", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_SECONDINPUT_DESC")
                        }),
                        new CommandArgumentPart(false, "input3", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_THIRDINPUT_DESC")
                        }),
                    ], false, true)
                ], new CombineCommand()),

            new CommandInfo("compare", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMPARE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "source", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMPARE_ARGUMENT_SOURCEINPUT_DESC")
                        }),
                        new CommandArgumentPart(true, "target", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COMPARE_ARGUMENT_TARGETINPUT_DESC")
                        }),
                    ])
                ], new CompareCommand()),

            new CommandInfo("convertlineendings", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "textfile", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_ARGUMENT_TEXTFILE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("w", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_W_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["u", "m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("u", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_U_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["m", "w"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_M_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["u", "w"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("force", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_FORCE_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ConvertLineEndingsCommand()),

            new CommandInfo("copy", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COPY_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "source", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COPY_ARGUMENT_SOURCE_DESC")
                        }),
                        new CommandArgumentPart(true, "target", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_COPY_ARGUMENT_TARGET_DESC")
                        }),
                    ])
                ], new CopyCommand()),

            new CommandInfo("date", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTD_DESC"),
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("date", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DATE_SWITCH_DATE_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["time", "full"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("time", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DATE_SWITCH_TIME_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["date", "full"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("full", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTD_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["date", "time"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("utc", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DATE_SWITCH_UTC_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ], true)
                ], new DateCommand(), CommandFlags.RedirectionSupported),

            new CommandInfo("debugshell", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DEBUGSHELL_DESC"), new DebugShellCommand(), CommandFlags.Strict),

            new CommandInfo("decodefile", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_ARGUMENT_FILE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("key", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_KEY_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new DecodeFileCommand()),

            new CommandInfo("decodetext", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODETEXT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "encodedString", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODETEXT_ARGUMENT_ENCODEDSTRING_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("key", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_KEY_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new DecodeTextCommand()),

            new CommandInfo("decodebase64", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEBASE64_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "encoded", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEBASE64_ARGUMENT_ENCODED_DESC")
                        })
                    ])
                ], new DecodeBase64Command()),

            new CommandInfo("dirinfo", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_ARGUMENT_DIRECTORY_DESC")
                        }),
                    ])
                ], new DirInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("disconndbgdev", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_DISCONNDBGDEV_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ip", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DISCONNDBGDEV_ARGUMENT_IP_DESC")
                        }),
                    ])
                ], new DisconnDbgDevCommand(), CommandFlags.Strict),

            new CommandInfo("diskinfo", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DISKINFO_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DISKINFO_ARGUMENT_DISKNUM_DESC")
                        }),
                    ], true)
                ], new DiskInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("dismissnotif", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DISMISSNOTIF_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "notificationNumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DISMISSNOTIF_ARGUMENT_NOTIFICATIONNUM_DESC")
                        }),
                    ])
                ], new DismissNotifCommand()),

            new CommandInfo("dismissnotifs", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DISMISSNOTIFS_DESC"), new DismissNotifsCommand()),

            new CommandInfo("driverman", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "change", new()
                        {
                            ExactWording = ["change"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_CHANGE_DESC")
                        }),
                        new CommandArgumentPart(true, "type", new()
                        {
                            AutoCompleter = (_) => Enum.GetNames<DriverTypes>(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_CHANGE_TYPE_DESC")
                        }),
                        new CommandArgumentPart(true, "driver", new()
                        {
                            AutoCompleter = (args) => DriverHandler.GetDriverNames(DriverHandler.InferDriverTypeFromTypeName(args[1])),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_CHANGE_DRIVER_DESC")
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list", new()
                        {
                            ExactWording = ["list"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_LIST_DESC")
                        }),
                        new CommandArgumentPart(true, "type", new()
                        {
                            AutoCompleter = (_) => Enum.GetNames<DriverTypes>(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_CHANGE_TYPE_DESC")
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "types", new()
                        {
                            ExactWording = ["types"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_TYPES_DESC")
                        }),
                    ]),
                ], new DriverManCommand(), CommandFlags.Strict),

            new CommandInfo("echo", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ECHO_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ECHO_ARGUMENT_TEXT_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("noparse", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ECHO_SWITCH_NOPARSE_DESC"), false, false, [], 0, false)
                    ], true)
                ], new EchoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("edit", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_EDIT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("text", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_EDIT_SWITCH_TEXT_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["sql", "json", "hex"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("hex", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_EDIT_SWITCH_HEX_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["text", "json", "sql"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("json", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_EDIT_SWITCH_JSON_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["text", "sql", "hex"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("sql", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_EDIT_SWITCH_SQL_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["text", "json", "hex"],
                            AcceptsValues = false
                        }),
                    ])
                ], new EditCommand()),

            new CommandInfo("encodefile", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ENCODEFILE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ENCODEFILE_ARGUMENT_FILE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("key", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_KEY_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new EncodeFileCommand()),

            new CommandInfo("encodetext", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ENCODETEXT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "string", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ENCODETEXT_ARGUMENT_STRING_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("key", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_KEY_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new EncodeTextCommand()),

            new CommandInfo("encodebase64", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ENCODEBASE64_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "string", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ENCODETEXT_ARGUMENT_STRING_DESC")
                        })
                    ])
                ], new EncodeBase64Command()),

            new CommandInfo("fileinfo", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FILEINFO_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC")
                        }),
                    ])
                ], new FileInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("find", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FIND_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILENAME")
                        }),
                        new CommandArgumentPart(true, "directory", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_ARGUMENT_DIRECTORY_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("recursive", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FIND_SWITCH_RECURSIVE_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("exec", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FIND_SWITCH_EXEC_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ], true)
                ], new FindCommand()),

            new CommandInfo("findreg", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FINDREG_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "fileRegex", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FINDREG_ARGUMENT_REGEXP_DESC")
                        }),
                        new CommandArgumentPart(true, "directory", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_ARGUMENT_DIRECTORY_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("recursive", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FIND_SWITCH_RECURSIVE_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("exec", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FIND_SWITCH_EXEC_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ], true)
                ], new FindRegCommand()),

            new CommandInfo("fork", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_FORK_DESC"), new ForkCommand()),

            new CommandInfo("get", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GET_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "url", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GET_ARGUMENT_URL_DESC")
                        })
                    ],
                    [
                        new SwitchInfo("outputpath", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GET_SWITCH_OUTPUTPATH_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ])
                ], new GetCommand()),

            new CommandInfo("getaddons", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETADDONS_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("reinstall", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETADDONS_SWITCH_REINSTALL_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new GetAddonsCommand()),

            new CommandInfo("getallexthandlers", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETALLEXTHANDLERS_DESC"), new GetAllExtHandlersCommand()),

            new CommandInfo("getconfigvalue", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_DESC"),
                [
                    new CommandArgumentInfo(new CommandArgumentPart[]
                    {
                        new(true, "config", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_ARGUMENT_CONFIG_DESC")
                        }),
                        new(true, "variable", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => ConfigTools.GetSettingsKeys(arg[0]).Select((sk) => sk.Variable).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_ARGUMENT_VARIABLE_DESC")
                        })
                    }, true)
                ], new GetConfigValueCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("getdefaultexthandler", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLER_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLER_ARGUMENT_EXTENSION_DESC")
                        }),
                    ], true)
                ], new GetDefaultExtHandlerCommand()),

            new CommandInfo("getdefaultexthandlers", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLERS_DESC"), new GetDefaultExtHandlersCommand()),

            new CommandInfo("getexthandlers", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETEXTHANDLERS_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLER_ARGUMENT_EXTENSION_DESC")
                        }),
                    ], true)
                ], new GetExtHandlersCommand()),

            new CommandInfo("getkeyiv", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETKEYIV_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETKEYIV_ARGUMENT_ALGORITHM_DESC")
                        }),
                    ], true)
                ], new GetKeyIvCommand()),

            new CommandInfo("host", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_HOST_DESC"),
                [
                    new CommandArgumentInfo(true)
                ], new HostCommand()),

            new CommandInfo("hwinfo", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_HWINFO_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "HardwareType", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => DriverHandler.CurrentHardwareProberDriverLocal.SupportedHardwareTypes.Union(["all"]).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_HWINFO_ARGUMENT_TYPE_DESC")
                        })
                    ])
                ], new HwInfoCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("if", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_IF_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ueshExpression", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_IF_ARGUMENT_UESHEXPRESSION_DESC")
                        }),
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_IF_ARGUMENT_COMMAND_DESC")
                        }),
                    ])
                ], new IfCommand()),

            new CommandInfo("ifm", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_IFM_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "firstPanePath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_IFM_ARGUMENT_FIRSTPATH_DESC")
                        }),
                        new CommandArgumentPart(false, "secondPanePath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_IFM_ARGUMENT_SECONDPATH_DESC")
                        }),
                    ])
                ], new IfmCommand()),

            new CommandInfo("input", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_INPUT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "question", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_ARGUMENT_INPUT_DESC")
                        }),
                    ], true)
                ], new InputCommand()),

            new CommandInfo("inputpass", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_INPUTPASS_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "question", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_ARGUMENT_INPUT_DESC")
                        }),
                    ], true)
                ], new InputPassCommand()),

            new CommandInfo("license", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LICENSE_DESC"), new LicenseCommand()),

            new CommandInfo("lintscript", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LINTSCRIPT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "script", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LINTSCRIPT_ARGUMENT_SCRIPT_DESC")
                        }),
                    ], true)
                ], new LintScriptCommand()),

            new CommandInfo("list", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LIST_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LIST_ARGUMENT_DIRECTORY_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("showdetails", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_SHOWDETAILS_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("suppressmessages", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_SUPPRESSMESSAGES_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("recursive", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_RECURSIVE_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["tree"]
                        }),
                        new SwitchInfo("tree", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_TREE_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["recursive"]
                        })
                    ])
                ], new ListCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lockscreen", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LOCKSCREEN_DESC"), new LockScreenCommand()),

            new CommandInfo("logout", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LOGOUT_DESC"), new LogoutCommand(), CommandFlags.NoMaintenance),

            new CommandInfo("lsconfigs", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSCONFIGS_DESC"),
                [
                    new CommandArgumentInfo(new SwitchInfo[]
                    {
                        new("deep", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSCONFIGS_SWITCH_DEEP_DESC"))
                    })
                ], new LsConfigsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsconfigvalues", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSCONFIGVALUES_DESC"),
                [
                    new CommandArgumentInfo(new CommandArgumentPart[]
                    {
                        new(true, "config", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_ARGUMENT_CONFIG_DESC")
                        })
                    })
                ], new LsConfigValuesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsconnections", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSCONNECTIONS_DESC"), new LsConnectionsCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsdbgdev", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSDBGDEV_DESC"), new LsDbgDevCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsexthandlers", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSEXTHANDLERS_DESC"), new LsExtHandlersCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsdiskparts", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSDISKPARTS_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DISKINFO_ARGUMENT_DISKNUM_DESC")
                        }),
                    ], true)
                ], new LsDiskPartsCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsdisks", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSDISKS_DESC"),
                [
                    new CommandArgumentInfo(true)
                ], new LsDisksCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsnet", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSNET_DESC"), new LsNetCommand(), CommandFlags.Strict),

            new CommandInfo("lsvars", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LSVARS_DESC"), new LsVarsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("md", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_MD_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_MD_ARGUMENT_DIRECTORY_DESC")
                        }),
                    ], true)
                ], new MdCommand()),

            new CommandInfo("mkfile", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_MKFILE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_MKFILE_ARGUMENT_FILE_DESC")
                        }),
                    ], true)
                ], new MkFileCommand()),

            new CommandInfo("move", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_MOVE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "source", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_MOVE_ARGUMENT_SOURCE_DESC")
                        }),
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_MOVE_ARGUMENT_TARGET_DESC")
                        }),
                    ])
                ], new MoveCommand()),

            new CommandInfo("partinfo", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PARTINFO_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DISKINFO_ARGUMENT_DISKNUM_DESC")
                        }),
                        new CommandArgumentPart(true, "partNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PARTINFO_ARGUMENT_PARTNUM_DESC")
                        }),
                    ], true)
                ], new PartInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("pathfind", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PATHFIND_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "fileName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PATHFIND_ARGUMENT_FILENAME_DESC")
                        }),
                    ], true)
                ], new PathFindCommand()),

            new CommandInfo("perm", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PERM_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "userName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PERM_ARGUMENT_USERNAME_DESC")
                        }),
                        new CommandArgumentPart(true, "allow/revoke", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PERM_ARGUMENT_MODE_DESC")
                        }),
                        new CommandArgumentPart(true, "perm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PERM_ARGUMENT_GRANT_DESC")
                        }),
                    ])
                ], new PermCommand(), CommandFlags.Strict),

            new CommandInfo("permgroup", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PERMGROUP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "groupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PERMGROUP_ARGUMENT_GROUPNAME_DESC")
                        }),
                        new CommandArgumentPart(true, "allow/revoke", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PERM_ARGUMENT_MODE_DESC")
                        }),
                        new CommandArgumentPart(true, "perm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PERM_ARGUMENT_GRANT_DESC")
                        }),
                    ])
                ], new PermGroupCommand(), CommandFlags.Strict),

            new CommandInfo("ping", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PING_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Address1", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PING_ARGUMENT_FIRSTHOST_DESC")
                        }),
                        new CommandArgumentPart(false, "Address2", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PING_ARGUMENT_SECONDHOST_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("times", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PING_SWITCH_TIMES_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                            IsNumeric = true
                        })
                    ], false, true)
                ], new PingCommand()),

            new CommandInfo("platform", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("n", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_N_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["r", "v", "b", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("v", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_V_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["n", "r", "b", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("b", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_B_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["n", "v", "r", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("c", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_C_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["n", "v", "b", "r"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("r", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_R_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["n", "v", "b", "c"],
                            AcceptsValues = false
                        })
                    ], true)
                ], new PlatformCommand()),

            new CommandInfo("put", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PUT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "FileName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PUT_ARGUMENT_FILE_DESC")
                        }),
                        new CommandArgumentPart(true, "URL", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_PUT_ARGUMENT_URL_DESC")
                        }),
                    ])
                ], new PutCommand()),

            new CommandInfo("rdebug", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RDEBUG_DESC"), new RdebugCommand(), CommandFlags.Strict),

            new CommandInfo("reboot", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("safe", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_SAFE_DESC"), new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["maintenance", "debug"]
                        }),
                        new SwitchInfo("maintenance", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_MAINTENANCE_DESC"), new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "debug"]
                        }),
                        new SwitchInfo("debug", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_DEBUG_DESC"), new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "maintenance"]
                        }),
                    ])
                ], new RebootCommand()),

            new CommandInfo("reloadconfig", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RELOADCONFIG_DESC"), new ReloadConfigCommand(), CommandFlags.Strict),

            new CommandInfo("rexec", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_HOSTNAME_DESC")
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_PORT_DESC")
                        }),
                        new CommandArgumentPart(false, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_COMMAND_DESC")
                        }),
                    ])
                ], new RexecCommand(), CommandFlags.Strict),

            new CommandInfo("rm", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RM_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RM_ARGUMENT_TARGET_DESC")
                        }),
                    ])
                ], new RmCommand()),

            new CommandInfo("rmsec", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RMSEC_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RM_ARGUMENT_TARGET_DESC")
                        }),
                    ])
                ], new RmSecCommand()),

            new CommandInfo("rmuser", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RMUSER_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RMUSER_ARGUMENT_TARGET_DESC")
                        }),
                    ])
                ], new RmUserCommand(), CommandFlags.Strict),

            new CommandInfo("rmgroup", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RMGROUP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "GroupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RMGROUP_ARGUMENT_TARGET_DESC")
                        }),
                    ])
                ], new RmGroupCommand(), CommandFlags.Strict),

            new CommandInfo("rmuserfromgroup", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RMUSERFROMGROUP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "UserName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RMUSERFROMGROUP_ARGUMENT_USERNAME_DESC")
                        }),
                        new CommandArgumentPart(true, "GroupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RMUSERFROMGROUP_ARGUMENT_GROUPNAME_DESC")
                        }),
                    ])
                ], new RmUserFromGroupCommand(), CommandFlags.Strict),

            new CommandInfo("rreboot", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RREBOOT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_HOSTNAME_DESC")
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_PORT_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("safe", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_SAFE_DESC"), new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["maintenance", "debug"]
                        }),
                        new SwitchInfo("maintenance", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_MAINTENANCE_DESC"), new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "debug"]
                        }),
                        new SwitchInfo("debug", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_DEBUG_DESC"), new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "maintenance"]
                        }),
                    ])
                ], new RebootCommand()),

            new CommandInfo("rshutdown", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_RSHUTDOWN_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_HOSTNAME_DESC")
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_PORT_DESC")
                        }),
                    ])
                ], new RShutdownCommand()),

            new CommandInfo("saveconfig", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SAVECONFIG_DESC"), new SaveConfigCommand(), CommandFlags.Strict),

            new CommandInfo("savescreen", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SAVESCREEN_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "saver/random", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SAVESCREEN_ARGUMENT_SAVERNAME_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("select", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SAVESCREEN_SWITCH_SELECT_DESC"), new()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SaveScreenCommand()),

            new CommandInfo("search", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Regexp", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_ARGUMENT_REGEXP_DESC")
                        }),
                        new CommandArgumentPart(true, "File", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_ARGUMENT_FILE_DESC")
                        }),
                    ])
                ], new SearchCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("searchword", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SEARCHWORD_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "StringEnclosedInDoubleQuotes", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SEARCHWORD_ARGUMENT_STRING_DESC")
                        }),
                        new CommandArgumentPart(true, "File", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_ARGUMENT_FILE_DESC")
                        }),
                    ])
                ], new SearchWordCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("select", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SELECT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "answers", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_ARGUMENT_ANSWERS_DESC")
                        }),
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_ARGUMENT_INPUT_DESC")
                        }),
                        new CommandArgumentPart(false, "answertitle1", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_ARGUMENT_TITLE1_DESC")
                        }),
                        new CommandArgumentPart(false, "answertitle2", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHOICE_ARGUMENT_TITLE2_DESC")
                        }),
                    ], true, true)
                ], new SelectCommand()),

            new CommandInfo("setsaver", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETSAVER_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "saver", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETSAVER_ARGUMENT_SAVER_DESC")
                        }),
                    ])
                ], new SetSaverCommand(), CommandFlags.Strict),

            new CommandInfo("settings", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_DESC"),
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("saver", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_SCREENSAVER_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["splash", "addonsplash", "type", "addonsaver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("addonsaver", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_ADDONSAVER_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["splash", "addonsplash", "type", "saver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("splash", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_SPLASH_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "addonsplash", "type", "addonsaver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("addonsplash", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_ADDONSPLASH_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "splash", "type", "addonsaver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("driver", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_DRIVER_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "addonsplash", "type", "addonsaver", "splash"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("type", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_TYPE_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "addonsplash", "splash", "addonsaver", "driver"],
                            ArgumentsRequired = true
                        }),
                        new SwitchInfo("sel", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_SEL_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SettingsCommand(), CommandFlags.Strict),

            new CommandInfo("set", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SET_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "value", new()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SET_ARGUMENT_VALUE_DESC")
                        }),
                    ], true)
                ], new SetCommand()),

            new CommandInfo("setconfigvalue", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETCONFIGVALUE_DESC"),
                [
                    new CommandArgumentInfo(new CommandArgumentPart[]
                    {
                        new(true, "config", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_ARGUMENT_CONFIG_DESC")
                        }),
                        new(true, "variable", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => ConfigTools.GetSettingsKeys(arg[0]).Select((sk) => sk.Variable).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETCONFIGVALUE_ARGUMENT_VARIABLE_DESC")
                        }),
                        new(true, "value")
                    })
                ], new SetConfigValueCommand()),

            new CommandInfo("setexthandler", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETEXTHANDLER_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLER_ARGUMENT_EXTENSION_DESC")
                        }),
                        new CommandArgumentPart(true, "implementer", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (args) => ExtensionHandlerTools.GetExtensionHandlers(args[0]).Select((h) => h.Implementer).ToArray(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETEXTHANDLER_ARGUMENT_IMPLEMENTER_DESC")
                        }),
                    ])
                ], new SetExtHandlerCommand()),

            new CommandInfo("setrange", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETRANGE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "value", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETRANGE_ARGUMENT_VALUE1_DESC")
                        }),
                        new CommandArgumentPart(false, "value2", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETRANGE_ARGUMENT_VALUE2_DESC")
                        }),
                        new CommandArgumentPart(false, "value3", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SETRANGE_ARGUMENT_VALUE3_DESC")
                        }),
                    ], true, true)
                ], new SetRangeCommand()),

            new CommandInfo("shownotifs", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWNOTIFS_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("tui", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWNOTIFS_SWITCH_TUI_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ShowNotifsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("showtd", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTD_DESC"), new ShowTdCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("showtdzone", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "timezone", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_ARGUMENT_TIMEZONE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("all", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_SWITCH_ALL_DESC"), new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1,
                            AcceptsValues = false
                        }),
                        new SwitchInfo("selection", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_SWITCH_SELECTION_DESC"), new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1,
                            AcceptsValues = false
                        })
                    ])
                ], new ShowTdZoneCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("shutdown", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHUTDOWN_DESC"), new ShutdownCommand()),

            new CommandInfo("sleep", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SLEEP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ms", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SLEEP_ARGUMENT_MS_DESC")
                        }),
                    ])
                ], new SleepCommand()),
            new CommandInfo("sudo", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUDO_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUDO_ARGUMENT_COMMAND_DESC")
                        }),
                    ])
                ], new SudoCommand()),

            new CommandInfo("sumfile", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_ALGORITHM_DESC")
                        }),
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_FILE_DESC")
                        }),
                        new CommandArgumentPart(false, "outputFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_OUTPUTFILE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("relative", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_SWITCH_RELATIVE_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SumFileCommand()),

            new CommandInfo("sumfiles", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILES_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_ALGORITHM_DESC")
                        }),
                        new CommandArgumentPart(true, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_ARGUMENT_DIRECTORY_DESC")
                        }),
                        new CommandArgumentPart(false, "outputFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_OUTPUTFILE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("relative", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_SWITCH_RELATIVE_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SumFilesCommand()),

            new CommandInfo("sumtext", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMTEXT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_ALGORITHM_DESC")
                        }),
                        new CommandArgumentPart(true, "text", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMTEXT_ARGUMENT_TEXT_DESC")
                        }),
                    ])
                ], new SumTextCommand()),

            new CommandInfo("symlink", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SYMLINK_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "linkname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SYMLINK_ARGUMENT_LINKNAME_DESC")
                        }),
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SYMLINK_ARGUMENT_TARGET_DESC")
                        }),
                    ])
                ], new SymlinkCommand()),

            new CommandInfo("sysinfo", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SYSINFO"),
                [
                    new CommandArgumentInfo([],
                    [
                        new SwitchInfo("s", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_S_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("h", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_H_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("u", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_U_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_M_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("l", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_L_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("a", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_A_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new SysInfoCommand()),

            new CommandInfo("taskman", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_TASKMAN_DESC"), new TaskManCommand()),

            new CommandInfo("themeprev", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_THEMEPREV_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "theme", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_THEMEPREV_ARGUMENT_THEMENAME_DESC")
                        }),
                    ])
                ], new ThemePrevCommand()),

            new CommandInfo("themeset", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_THEMESET_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "theme", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_THEMEPREV_ARGUMENT_THEMENAME_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("y", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_THEMESET_SWITCH_Y_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ThemeSetCommand()),

            new CommandInfo("unblockdbgdev", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UNBLOCKDBGDEV_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ipaddress", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UNBLOCKDBGDEV_ARGUMENT_IP_DESC")
                        }),
                    ])
                ], new UnblockDbgDevCommand(), CommandFlags.Strict),

            new CommandInfo("unset", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UNSET_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "$variable", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UNSET_ARGUMENT_VARIABLE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("justwipe", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UNSET_SWITCH_JUSTWIPE_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new UnsetCommand()),

            new CommandInfo("unzip", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UNZIP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "zipfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UNZIP_ARGUMENT_ZIPFILE_DESC")
                        }),
                        new CommandArgumentPart(false, "path", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UNZIP_ARGUMENT_PATH_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("createdir", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UNZIP_SWITCH_CREATEDIR_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new UnZipCommand()),

            #if SPECIFIERREL
            new CommandInfo("update", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UPDATE_DESC"), new UpdateCommand(), CommandFlags.Strict),
            #endif

            new CommandInfo("uptime", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_UPTIME_DESC"),
                [
                    new CommandArgumentInfo(true)
                ], new UptimeCommand()),

            new CommandInfo("usermanual", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_USERMANUAL_DESC"), new UserManualCommand()),

            new CommandInfo("verify", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_VERIFY_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_ALGORITHM_DESC")
                        }),
                        new CommandArgumentPart(true, "calculatedhash", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_VERIFY_ARGUMENT_CALCULATEDHASH_DESC")
                        }),
                        new CommandArgumentPart(true, "hashfile/expectedhash", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_VERIFY_ARGUMENT_HASHFILE_DESC")
                        }),
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_VERIFY_ARGUMENT_FILE_DESC")
                        }),
                    ])
                ], new VerifyCommand()),

            new CommandInfo("version", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_VERSION_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("m", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_VERSION_SWITCH_M_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["k"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("k", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_VERSION_SWITCH_K_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["m"],
                            AcceptsValues = false
                        })
                    ], true)
                ], new VersionCommand()),

            new CommandInfo("whoami", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_WHOAMI_DESC"),
                [
                    new CommandArgumentInfo(true)
                ], new WhoamiCommand()),

            new CommandInfo("winelevate", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_WINELEVATE_DESC"),
                [
                    new CommandArgumentInfo(true)
                ], new WinElevateCommand()),

            new CommandInfo("wraptext", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_WRAPTEXT_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("columns", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_WRAPTEXT_SWITCH_COLUMNS_DESC"), new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                            IsNumeric = true
                        })
                    ], true)
                ], new WrapTextCommand()),

            new CommandInfo("zip", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "zipfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_ARGUMENT_ZIPFILE_DESC")
                        }),
                        new CommandArgumentPart(true, "path", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_ARGUMENT_PATH_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("fast", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_SWITCH_FAST_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["nocomp"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("nocomp", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_SWITCH_NOCOMP_DESC"), new SwitchOptions()
                        {
                            ConflictsWith = ["fast"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("nobasedir", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_SWITCH_NOBASEDIR_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ZipCommand()),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new DefaultPreset() },
            { "PowerLine1", new PowerLine1Preset() },
            { "PowerLine2", new PowerLine2Preset() },
            { "PowerLine3", new PowerLine3Preset() },
            { "PowerLineBG1", new PowerLineBG1Preset() },
            { "PowerLineBG2", new PowerLineBG2Preset() },
            { "PowerLineBG3", new PowerLineBG3Preset() }
        };
    }
}
