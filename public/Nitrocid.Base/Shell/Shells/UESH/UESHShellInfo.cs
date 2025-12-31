//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Terminaux.Shell.Prompts;
using Nitrocid.Base.Shell.Shells.UESH.Commands;
using Nitrocid.Base.Shell.Shells.UESH.Presets;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Users;
using Nitrocid.Base.Drivers.Encoding;
using Nitrocid.Base.Files.Extensions;
using Nitrocid.Base.Drivers.Encryption;

namespace Nitrocid.Base.Shell.Shells.UESH
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
            new CommandInfo("2fa", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "add", new()
                        {
                            ExactWording = ["add"],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_ADD_DESC"
                        }),
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_USERNAME_DESC"
                        }),
                    ])
                    {
                        ArgChecker = (cp) => TwoFactorCommand.CheckArgument(cp, "start")
                    },
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "delete", new()
                        {
                            ExactWording = ["delete"],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_DELETE_DESC"
                        }),
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_USERNAME_DESC"
                        }),
                    ])
                    {
                        ArgChecker = (cp) => TwoFactorCommand.CheckArgument(cp, "stop")
                    },
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "check", new()
                        {
                            ExactWording = ["check"],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_CHECK_DESC"
                        }),
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_USERNAME_DESC"
                        }),
                    ])
                    {
                        ArgChecker = (cp) => TwoFactorCommand.CheckArgument(cp, "check")
                    },
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "setupkey", new()
                        {
                            ExactWording = ["setupkey"],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_SETUPKEY_DESC"
                        }),
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_USERNAME_DESC"
                        }),
                    ])
                    {
                        ArgChecker = (cp) => TwoFactorCommand.CheckArgument(cp, "setupkey")
                    },
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "setupqr", new()
                        {
                            ExactWording = ["setupqr"],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_SETUPQR_DESC"
                        }),
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_2FA_ARGUMENT_USERNAME_DESC"
                        }),
                    ])
                    {
                        ArgChecker = (cp) => TwoFactorCommand.CheckArgument(cp, "setupqr")
                    },
                ], new TwoFactorCommand()),

            new CommandInfo("addgroup", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDGROUP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "groupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDGROUP_ARGUMENT_GROUPNAME_DESC"
                        })
                    ])
                ], new AddGroupCommand()),

            new CommandInfo("adduser", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_ARGUMENT_USERNAME_DESC"
                        }),
                        new CommandArgumentPart(false, "password", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_ARGUMENT_PASSWORD_DESC"
                        }),
                        new CommandArgumentPart(false, "confirm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_ARGUMENT_CONFIRM_DESC"
                        }),
                    ])
                ], new AddUserCommand()),

            new CommandInfo("addusertogroup", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSERTOGROUP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSERTOGROUP_ARGUMENT_USERNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "group", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSERTOGROUP_ARGUMENT_GROUP_DESC"
                        }),
                    ])
                ], new AddUserToGroupCommand()),

            new CommandInfo("admin", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADMIN_DESC", new AdminCommand()),

            new CommandInfo("alarm", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "start", new()
                        {
                            ExactWording = ["start"],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_START_DESC"
                        }),
                        new CommandArgumentPart(true, "alarmname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_ALARMTUI_ALARMNAME"
                        }),
                        new CommandArgumentPart(true, "interval", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_START_INTERVAL_DESC"
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
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_STOP_DESC"
                        }),
                        new CommandArgumentPart(true, "alarmname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_STOP_NAME_DESC"
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
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_LIST_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_SWITCH_TUI_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ]),
                ], new AlarmCommand()),

            new CommandInfo("beep", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BEEP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "freq", new()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BEEP_ARGUMENT_FREQ_DESC"
                        }),
                        new CommandArgumentPart(false, "ms", new()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BEEP_ARGUMENT_INTERVAL_DESC"
                        }),
                    ])
                ], new BeepCommand()),

            new CommandInfo("blockdbgdev", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BLOCKDBGDEV_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ipaddress", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BLOCKDBGDEV_ARGUMENT_IPADDRESS_DESC"
                        }),
                    ])
                ], new BlockDbgDevCommand()),

            new CommandInfo("bulkrename", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "targetdir", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_ARGUMENT_TARGETDIR_DESC"
                        }),
                        new CommandArgumentPart(true, "pattern", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_ARGUMENT_PATTERN_DESC"
                        }),
                        new CommandArgumentPart(false, "newname", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_ARGUMENT_NEWNAME_DESC"
                        }),
                    ])
                ], new BulkRenameCommand()),

            new CommandInfo("cat", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CAT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CAT_ARGUMENT_FILE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("lines", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CAT_SWITCH_LINES_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["nolines"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("nolines", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CAT_SWITCH_NOLINES_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["lines"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("plain", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CAT_SWITCH_PLAIN_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new CatCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("cdir", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CDIR_DESC",
                [
                    new CommandArgumentInfo(true)
                ], new CDirCommand()),

            new CommandInfo("changes", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHANGES_DESC", new ChangesCommand()),

            new CommandInfo("chattr", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC"
                        }),
                        new CommandArgumentPart(true, "add/rem", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_ADDREMOVE_DESC"
                        }),
                        new CommandArgumentPart(true, "Normal/ReadOnly/Hidden/Archive", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_NAME_DESC"
                        }),
                    ])
                ], new ChAttrCommand()),

            new CommandInfo("chculture", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHCULTURE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "culture", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHCULTURE_ARGUMENT_CULTUREID_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("user", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHCULTURE_SWITCH_USER_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ChCultureCommand()),

            new CommandInfo("chdir", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHDIR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory/..", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_BULKRENAME_ARGUMENT_TARGETDIR_DESC"
                        }),
                    ])
                ], new ChDirCommand()),

            new CommandInfo("chhostname", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHHOSTNAME_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "hostname", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHHOSTNAME_ARGUMENT_HOSTNAME_DESC"
                        }),
                    ])
                ], new ChHostNameCommand()),

            new CommandInfo("chklock", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHKLOCK_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("waitforunlock", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHKLOCK_SWITCH_WAITFORUNLOCK_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ChkLockCommand()),

            new CommandInfo("chlang", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHLANG_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "language", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHLANG_ARGUMENT_LANGID_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("user", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHLANG_SWITCH_USER_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ChLangCommand()),

            new CommandInfo("chmal", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHMAL_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "message", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHMOTD_ARGUMENT_MESSAGE_DESC"
                        }),
                    ])
                ], new ChMalCommand()),

            new CommandInfo("chmotd", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHMOTD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "message", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHMOTD_ARGUMENT_MESSAGE_DESC"
                        }),
                    ])
                ], new ChMotdCommand()),

            new CommandInfo("chpwd", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHPWD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Username", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHPWD_ARGUMENT_USERNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "UserPass", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHPWD_ARGUMENT_CURRPASSWORD_DESC"
                        }),
                        new CommandArgumentPart(true, "newPass", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_ARGUMENT_PASSWORD_DESC"
                        }),
                        new CommandArgumentPart(true, "confirm", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ADDUSER_ARGUMENT_CONFIRM_DESC"
                        }),
                    ])
                ], new ChPwdCommand()),

            new CommandInfo("chusrname", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHUSRNAME_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "oldUserName", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => [.. UserManagement.ListAllUsers()],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHUSRNAME_ARGUMENT_OLDNAME_DESC",
                        }),
                        new CommandArgumentPart(true, "newUserName", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHUSRNAME_ARGUMENT_NEWNAME_DESC"
                        }),
                    ])
                ], new ChUsrNameCommand()),

            new CommandInfo("combinestr", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_FIRSTINPUT_DESC"
                        }),
                        new CommandArgumentPart(true, "input2", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_SECONDINPUT_DESC"
                        }),
                        new CommandArgumentPart(false, "input3", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_THIRDINPUT_DESC"
                        }),
                    ], true, true)
                ], new CombineStrCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("combine", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "output", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINE_ARGUMENT_OUTPUT_DESC"
                        }),
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_FIRSTINPUT_DESC"
                        }),
                        new CommandArgumentPart(true, "input2", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_SECONDINPUT_DESC"
                        }),
                        new CommandArgumentPart(false, "input3", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMBINESTR_ARGUMENT_THIRDINPUT_DESC"
                        }),
                    ], false, true)
                ], new CombineCommand()),

            new CommandInfo("compare", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMPARE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "source", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMPARE_ARGUMENT_SOURCEINPUT_DESC"
                        }),
                        new CommandArgumentPart(true, "target", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COMPARE_ARGUMENT_TARGETINPUT_DESC"
                        }),
                    ])
                ], new CompareCommand()),

            new CommandInfo("convertlineendings", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "textfile", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_ARGUMENT_TEXTFILE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("w", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_W_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["u", "m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("u", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_U_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["m", "w"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_M_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["u", "w"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("force", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CONVERTLINEENDINGS_SWITCH_FORCE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ConvertLineEndingsCommand()),

            new CommandInfo("copy", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COPY_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "source", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COPY_ARGUMENT_SOURCE_DESC"
                        }),
                        new CommandArgumentPart(true, "target", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_COPY_ARGUMENT_TARGET_DESC"
                        }),
                    ])
                ], new CopyCommand()),

            new CommandInfo("date", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTD_DESC",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("date", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DATE_SWITCH_DATE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["time", "full"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("time", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DATE_SWITCH_TIME_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["date", "full"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("full", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTD_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["date", "time"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("utc", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DATE_SWITCH_UTC_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ], true)
                ], new DateCommand(), CommandFlags.RedirectionSupported),

            new CommandInfo("debugshell", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DEBUGSHELL_DESC", new DebugShellCommand()),

            new CommandInfo("decodefile", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_ARGUMENT_FILE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("key", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_KEY_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new DecodeFileCommand()),

            new CommandInfo("decodetext", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODETEXT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "encodedString", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODETEXT_ARGUMENT_ENCODEDSTRING_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("key", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_KEY_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new DecodeTextCommand()),

            new CommandInfo("dirinfo", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_ARGUMENT_DIRECTORY_DESC"
                        }),
                    ])
                ], new DirInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("disconndbgdev", /* Localizable */ "NKS_SHELL_SHELLS_UESH_DISCONNDBGDEV_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ip", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DISCONNDBGDEV_ARGUMENT_IP_DESC"
                        }),
                    ])
                ], new DisconnDbgDevCommand()),

            new CommandInfo("diskinfo", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DISKINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DISKINFO_ARGUMENT_DISKNUM_DESC"
                        }),
                    ], true)
                ], new DiskInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("dismissnotif", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DISMISSNOTIF_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "notificationNumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DISMISSNOTIF_ARGUMENT_NOTIFICATIONNUM_DESC"
                        }),
                    ])
                ], new DismissNotifCommand()),

            new CommandInfo("dismissnotifs", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DISMISSNOTIFS_DESC", new DismissNotifsCommand()),

            new CommandInfo("driverman", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "change", new()
                        {
                            ExactWording = ["change"],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_CHANGE_DESC"
                        }),
                        new CommandArgumentPart(true, "type", new()
                        {
                            AutoCompleter = (_) => Enum.GetNames<DriverTypes>(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_CHANGE_TYPE_DESC"
                        }),
                        new CommandArgumentPart(true, "driver", new()
                        {
                            AutoCompleter = (args) => DriverHandler.GetDriverNames(DriverHandler.InferDriverTypeFromTypeName(args[1])),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_CHANGE_DRIVER_DESC"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list", new()
                        {
                            ExactWording = ["list"],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_LIST_DESC"
                        }),
                        new CommandArgumentPart(true, "type", new()
                        {
                            AutoCompleter = (_) => Enum.GetNames<DriverTypes>(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_CHANGE_TYPE_DESC"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "types", new()
                        {
                            ExactWording = ["types"],
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DRIVERMAN_ARGUMENT_TYPES_DESC"
                        }),
                    ]),
                ], new DriverManCommand()),

            new CommandInfo("edit", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_EDIT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("text", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_EDIT_SWITCH_TEXT_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["sql", "json", "hex"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("hex", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_EDIT_SWITCH_HEX_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["text", "json", "sql"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("json", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_EDIT_SWITCH_JSON_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["text", "sql", "hex"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("sql", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_EDIT_SWITCH_SQL_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["text", "json", "hex"],
                            AcceptsValues = false
                        }),
                    ])
                ], new EditCommand()),

            new CommandInfo("encodefile", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ENCODEFILE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ENCODEFILE_ARGUMENT_FILE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("key", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_KEY_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new EncodeFileCommand()),

            new CommandInfo("encodetext", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ENCODETEXT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "string", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ENCODETEXT_ARGUMENT_STRING_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("key", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_KEY_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DECODEFILE_SWITCH_IV_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new EncodeTextCommand()),

            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_EXTIP_DESC -> Returns IPv4 public address
            new CommandInfo("extip", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_EXTIP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_SWITCH_QUIET_DESC -> Quiet processing
                        new SwitchInfo("quiet", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SWITCH_QUIET_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false,
                        }),
                    ], true)
                ], new ExtIpCommand()),

            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_EXTIP6_DESC -> Returns IPv6 public address
            new CommandInfo("extip6", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_EXTIP6_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_SWITCH_QUIET_DESC -> Quiet processing
                        new SwitchInfo("quiet", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SWITCH_QUIET_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false,
                        }),
                    ], true)
                ], new ExtIp6Command()),

            new CommandInfo("fileinfo", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FILEINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC"
                        }),
                    ])
                ], new FileInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("find", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FIND_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_FMTUI_FILENAME"
                        }),
                        new CommandArgumentPart(true, "directory", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_ARGUMENT_DIRECTORY_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("recursive", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FIND_SWITCH_RECURSIVE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("exec", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FIND_SWITCH_EXEC_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ], true)
                ], new FindCommand()),

            new CommandInfo("findreg", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FINDREG_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "fileRegex", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FINDREG_ARGUMENT_REGEXP_DESC"
                        }),
                        new CommandArgumentPart(true, "directory", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_ARGUMENT_DIRECTORY_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("recursive", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FIND_SWITCH_RECURSIVE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("exec", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_FIND_SWITCH_EXEC_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ], true)
                ], new FindRegCommand()),

            new CommandInfo("get", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GET_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "url", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GET_ARGUMENT_URL_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("outputpath", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GET_SWITCH_OUTPUTPATH_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ])
                ], new GetCommand()),

#if NKS_EXTENSIONS
            new CommandInfo("getaddons", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETADDONS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("reinstall", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETADDONS_SWITCH_REINSTALL_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new GetAddonsCommand()),
#endif

            new CommandInfo("getallexthandlers", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETALLEXTHANDLERS_DESC", new GetAllExtHandlersCommand()),

            new CommandInfo("getconfigvalue", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_DESC",
                [
                    new CommandArgumentInfo(new CommandArgumentPart[]
                    {
                        new(true, "config", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_ARGUMENT_CONFIG_DESC"
                        }),
                        new(true, "variable", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => ConfigTools.GetSettingsKeys(arg[0]).Select((sk) => sk.Variable).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_ARGUMENT_VARIABLE_DESC"
                        })
                    }, true)
                ], new GetConfigValueCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("getdefaultexthandler", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLER_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLER_ARGUMENT_EXTENSION_DESC"
                        }),
                    ], true)
                ], new GetDefaultExtHandlerCommand()),

            new CommandInfo("getdefaultexthandlers", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLERS_DESC", new GetDefaultExtHandlersCommand()),

            new CommandInfo("getexthandlers", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETEXTHANDLERS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLER_ARGUMENT_EXTENSION_DESC"
                        }),
                    ], true)
                ], new GetExtHandlersCommand()),

            new CommandInfo("getkeyiv", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETKEYIV_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETKEYIV_ARGUMENT_ALGORITHM_DESC"
                        }),
                    ], true)
                ], new GetKeyIvCommand()),

            new CommandInfo("host", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_HOST_DESC",
                [
                    new CommandArgumentInfo(true)
                ], new HostCommand()),

            new CommandInfo("hwinfo", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_HWINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "HardwareType", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => DriverHandler.CurrentHardwareProberDriverLocal.SupportedHardwareTypes.Union(["all"]).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_HWINFO_ARGUMENT_TYPE_DESC"
                        })
                    ])
                ], new HwInfoCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("ifm", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_IFM_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "firstPanePath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_IFM_ARGUMENT_FIRSTPATH_DESC"
                        }),
                        new CommandArgumentPart(false, "secondPanePath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_IFM_ARGUMENT_SECONDPATH_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("single", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_IFM_SWITCH_SINGLE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new IfmCommand()),

            new CommandInfo("ismode", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ISMODE_DESC",
                [
                    new CommandArgumentInfo([],
                    [
                        new SwitchInfo("s", /* Localizable */ "NKS_MISC_SPLASHES_WELCOME_SAFEMODE", new SwitchOptions()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["d", "m"]
                        }),
                        new SwitchInfo("d", /* Localizable */ "NKS_MISC_SPLASHES_WELCOME_DEBUGMODE", new SwitchOptions()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["s", "m"]
                        }),
                        new SwitchInfo("m", /* Localizable */ "NKS_MISC_SPLASHES_WELCOME_MAINTENANCE", new SwitchOptions()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["s", "d"]
                        }),
                        new SwitchInfo("v", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ISMODE_ARGUMENT_VERBOSE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false,
                        }),
                    ], true)
                ], new IsModeCommand()),

            new CommandInfo("license", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LICENSE_DESC", new LicenseCommand()),

            new CommandInfo("list", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_ARGUMENT_DIRECTORY_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("showdetails", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_SHOWDETAILS_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("suppressmessages", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_SUPPRESSMESSAGES_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("recursive", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_RECURSIVE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["tree"]
                        }),
                        new SwitchInfo("tree", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_TREE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["recursive"]
                        })
                    ])
                ], new ListCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lockscreen", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LOCKSCREEN_DESC", new LockScreenCommand()),

            new CommandInfo("logout", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LOGOUT_DESC", new LogoutCommand()),

            new CommandInfo("lsconfigs", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LSCONFIGS_DESC",
                [
                    new CommandArgumentInfo(new SwitchInfo[]
                    {
                        new("deep", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LSCONFIGS_SWITCH_DEEP_DESC")
                    })
                ], new LsConfigsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsconfigvalues", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LSCONFIGVALUES_DESC",
                [
                    new CommandArgumentInfo(new CommandArgumentPart[]
                    {
                        new(true, "config", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_ARGUMENT_CONFIG_DESC"
                        })
                    })
                ], new LsConfigValuesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsconnections", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LSCONNECTIONS_DESC", new LsConnectionsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsdbgdev", /* Localizable */ "NKS_SHELL_SHELLS_UESH_LSDBGDEV_DESC", new LsDbgDevCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsexthandlers", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LSEXTHANDLERS_DESC", new LsExtHandlersCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsdiskparts", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LSDISKPARTS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DISKINFO_ARGUMENT_DISKNUM_DESC"
                        }),
                    ], true)
                ], new LsDiskPartsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsdisks", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LSDISKS_DESC",
                [
                    new CommandArgumentInfo(true)
                ], new LsDisksCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsnet", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LSNET_DESC", new LsNetCommand()),

            new CommandInfo("md", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MD_ARGUMENT_DIRECTORY_DESC"
                        }),
                    ], true)
                ], new MdCommand()),

            new CommandInfo("mkfile", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MKFILE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MKFILE_ARGUMENT_FILE_DESC"
                        }),
                    ], true)
                ], new MkFileCommand()),

            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_DESC -> Generates an XFree86 3.3.6 compatible modeline from display parameters for CRT monitors
            new CommandInfo("modeline", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "horizontalPixels", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALPIXELS_DESC -> How many pixels are there, horizontally (for example, 1280)?
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALPIXELS_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(true, "verticalPixels", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALPIXELS_DESC -> How many pixels are there, vertically (for example, 960)?
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALPIXELS_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(true, "verticalFreq", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALFREQ_DESC -> Vertical refresh frequency in hertz (for example, 75 Hz)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALFREQ_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "verticalSync", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALSYNC_DESC -> Vertical syncing in microseconds (for example, 0 microseconds)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALSYNC_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "verticalBlanking", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALBLANKING_DESC -> Vertical blanking in microseconds (for example, 500 microseconds)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALBLANKING_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "horizontalSync", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALSYNC_DESC -> Horizontal syncing in microseconds (for example, 1 microseconds)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALSYNC_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "horizontalBlanking", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALBLANKING_DESC -> Horizontal blanking in microseconds (for example, 3 microseconds)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALBLANKING_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "verticalRatioFrontPorch", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALRATIOFRONTPORCH_DESC -> Vertical front porch ratio (for example, 1)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALRATIOFRONTPORCH_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "verticalRatioSync", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALRATIOSYNC_DESC -> Vertical sync ratio (for example, 1)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALRATIOSYNC_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "verticalRatioBackPorch", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALRATIOBACKPORCH_DESC -> Vertical back porch ratio (for example, 10)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_VERTICALRATIOBACKPORCH_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "horizontalRatioFrontPorch", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALRATIOFRONTPORCH_DESC -> Horizontal front porch ratio (for example, 1)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALRATIOFRONTPORCH_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "horizontalRatioSync", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALRATIOSYNC_DESC -> Horizontal sync ratio (for example, 4)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALRATIOSYNC_DESC",
                            IsNumeric = true,
                        }),
                        new CommandArgumentPart(false, "horizontalRatioBackPorch", new CommandArgumentPartOptions()
                        {
                            // TODO: NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALRATIOBACKPORCH_DESC -> Horizontal back porch ratio (for example, 7)
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_ARGUMENT_HORIZONTALRATIOBACKPORCH_DESC",
                            IsNumeric = true,
                        }),
                    ],
                    [
                        new SwitchInfo("oneline", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MODELINE_SWITCH_ONELINE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false,
                        })
                    ])
                ], new ModelineCommand()),

            new CommandInfo("move", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MOVE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "source", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MOVE_ARGUMENT_SOURCE_DESC"
                        }),
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_MOVE_ARGUMENT_TARGET_DESC"
                        }),
                    ])
                ], new MoveCommand()),

            new CommandInfo("partinfo", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PARTINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DISKINFO_ARGUMENT_DISKNUM_DESC"
                        }),
                        new CommandArgumentPart(true, "partNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PARTINFO_ARGUMENT_PARTNUM_DESC"
                        }),
                    ], true)
                ], new PartInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("pathfind", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PATHFIND_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "fileName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PATHFIND_ARGUMENT_FILENAME_DESC"
                        }),
                    ], true)
                ], new PathFindCommand()),

            new CommandInfo("perm", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PERM_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "userName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PERM_ARGUMENT_USERNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "allow/revoke", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PERM_ARGUMENT_MODE_DESC"
                        }),
                        new CommandArgumentPart(true, "perm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PERM_ARGUMENT_GRANT_DESC"
                        }),
                    ])
                ], new PermCommand()),

            new CommandInfo("permgroup", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PERMGROUP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "groupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PERMGROUP_ARGUMENT_GROUPNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "allow/revoke", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PERM_ARGUMENT_MODE_DESC"
                        }),
                        new CommandArgumentPart(true, "perm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PERM_ARGUMENT_GRANT_DESC"
                        }),
                    ])
                ], new PermGroupCommand()),

            new CommandInfo("ping", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PING_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Address1", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PING_ARGUMENT_FIRSTHOST_DESC"
                        }),
                        new CommandArgumentPart(false, "Address2", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PING_ARGUMENT_SECONDHOST_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("times", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PING_SWITCH_TIMES_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                            IsNumeric = true
                        })
                    ], false, true)
                ], new PingCommand()),

            new CommandInfo("platform", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("n", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_N_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["r", "v", "b", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("v", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_V_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["n", "r", "b", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("b", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_B_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["n", "v", "r", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("c", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_C_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["n", "v", "b", "r"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("r", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PLATFORM_SWITCH_R_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["n", "v", "b", "c"],
                            AcceptsValues = false
                        })
                    ], true)
                ], new PlatformCommand()),

            new CommandInfo("put", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PUT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "FileName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PUT_ARGUMENT_FILE_DESC"
                        }),
                        new CommandArgumentPart(true, "URL", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_PUT_ARGUMENT_URL_DESC"
                        }),
                    ])
                ], new PutCommand()),

            new CommandInfo("rdebug", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RDEBUG_DESC", new RdebugCommand()),

            new CommandInfo("reboot", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("safe", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_SAFE_DESC", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["maintenance", "debug"]
                        }),
                        new SwitchInfo("maintenance", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_MAINTENANCE_DESC", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "debug"]
                        }),
                        new SwitchInfo("debug", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_DEBUG_DESC", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "maintenance"]
                        }),
                    ])
                ], new RebootCommand()),

            new CommandInfo("reloadconfig", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RELOADCONFIG_DESC", new ReloadConfigCommand()),

            new CommandInfo("rexec", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_HOSTNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_PORT_DESC"
                        }),
                        new CommandArgumentPart(false, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_COMMAND_DESC"
                        }),
                    ])
                ], new RexecCommand()),

            new CommandInfo("rm", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RM_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RM_ARGUMENT_TARGET_DESC"
                        }),
                    ])
                ], new RmCommand()),

            new CommandInfo("rmsec", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RMSEC_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RM_ARGUMENT_TARGET_DESC"
                        }),
                    ])
                ], new RmSecCommand()),

            new CommandInfo("rmuser", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RMUSER_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RMUSER_ARGUMENT_TARGET_DESC"
                        }),
                    ])
                ], new RmUserCommand()),

            new CommandInfo("rmgroup", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RMGROUP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "GroupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RMGROUP_ARGUMENT_TARGET_DESC"
                        }),
                    ])
                ], new RmGroupCommand()),

            new CommandInfo("rmuserfromgroup", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RMUSERFROMGROUP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "UserName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RMUSERFROMGROUP_ARGUMENT_USERNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "GroupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RMUSERFROMGROUP_ARGUMENT_GROUPNAME_DESC"
                        }),
                    ])
                ], new RmUserFromGroupCommand()),

            new CommandInfo("rreboot", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RREBOOT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_HOSTNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_PORT_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("safe", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_SAFE_DESC", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["maintenance", "debug"]
                        }),
                        new SwitchInfo("maintenance", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_MAINTENANCE_DESC", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "debug"]
                        }),
                        new SwitchInfo("debug", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REBOOT_SWITCH_DEBUG_DESC", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "maintenance"]
                        }),
                    ])
                ], new RebootCommand()),

            new CommandInfo("rshutdown", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_RSHUTDOWN_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_HOSTNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_REXEC_ARGUMENT_PORT_DESC"
                        }),
                    ])
                ], new RShutdownCommand()),

            new CommandInfo("saveconfig", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SAVECONFIG_DESC", new SaveConfigCommand()),

            new CommandInfo("savescreen", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SAVESCREEN_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "saver", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SAVESCREEN_ARGUMENT_SAVERNAME_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("select", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SAVESCREEN_SWITCH_SELECT_DESC", new()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("random", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SAVESCREEN_SWITCH_RANDOM_DESC", new()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new SaveScreenCommand()),

            new CommandInfo("search", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Regexp", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_ARGUMENT_REGEXP_DESC"
                        }),
                        new CommandArgumentPart(true, "File", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_ARGUMENT_FILE_DESC"
                        }),
                    ])
                ], new SearchCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("searchword", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SEARCHWORD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "StringEnclosedInDoubleQuotes", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SEARCHWORD_ARGUMENT_STRING_DESC"
                        }),
                        new CommandArgumentPart(true, "File", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SEARCH_ARGUMENT_FILE_DESC"
                        }),
                    ])
                ], new SearchWordCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("setsaver", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETSAVER_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "saver", new()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETSAVER_ARGUMENT_SAVER_DESC"
                        }),
                    ])
                ], new SetSaverCommand()),

            new CommandInfo("settings", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_DESC",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("saver", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_SCREENSAVER_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["splash", "addonsplash", "type", "addonsaver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("addonsaver", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_ADDONSAVER_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["splash", "addonsplash", "type", "saver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("splash", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_SPLASH_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "addonsplash", "type", "addonsaver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("addonsplash", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_ADDONSPLASH_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "splash", "type", "addonsaver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("driver", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_DRIVER_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "addonsplash", "type", "addonsaver", "splash"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("type", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETTINGS_SWITCH_TYPE_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "addonsplash", "splash", "addonsaver", "driver"],
                            ArgumentsRequired = true
                        })
                    ])
                ], new SettingsCommand()),

            new CommandInfo("setconfigvalue", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETCONFIGVALUE_DESC",
                [
                    new CommandArgumentInfo(new CommandArgumentPart[]
                    {
                        new(true, "config", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETCONFIGVALUE_ARGUMENT_CONFIG_DESC"
                        }),
                        new(true, "variable", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => ConfigTools.GetSettingsKeys(arg[0]).Select((sk) => sk.Variable).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETCONFIGVALUE_ARGUMENT_VARIABLE_DESC"
                        }),
                        new(true, "value")
                    })
                ], new SetConfigValueCommand()),

            new CommandInfo("setexthandler", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETEXTHANDLER_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GETDEFAULTEXTHANDLER_ARGUMENT_EXTENSION_DESC"
                        }),
                        new CommandArgumentPart(true, "implementer", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (args) => ExtensionHandlerTools.GetExtensionHandlers(args[0]).Select((h) => h.Implementer).ToArray(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SETEXTHANDLER_ARGUMENT_IMPLEMENTER_DESC"
                        }),
                    ])
                ], new SetExtHandlerCommand()),

            new CommandInfo("shownotifs", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWNOTIFS_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("tui", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWNOTIFS_SWITCH_TUI_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ShowNotifsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("showtd", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTD_DESC", new ShowTdCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("showtdzone", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "timezone", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_ARGUMENT_TIMEZONE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("all", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_SWITCH_ALL_DESC", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1,
                            AcceptsValues = false
                        }),
                        new SwitchInfo("selection", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_SWITCH_SELECTION_DESC", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1,
                            AcceptsValues = false
                        })
                    ])
                ], new ShowTdZoneCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("shutdown", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHUTDOWN_DESC", new ShutdownCommand()),

            new CommandInfo("sudo", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUDO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUDO_ARGUMENT_COMMAND_DESC"
                        }),
                    ])
                ], new SudoCommand()),

            new CommandInfo("sumfile", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_ALGORITHM_DESC"
                        }),
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_FILE_DESC"
                        }),
                        new CommandArgumentPart(false, "outputFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_OUTPUTFILE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("relative", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_SWITCH_RELATIVE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SumFileCommand()),

            new CommandInfo("sumfiles", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILES_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_ALGORITHM_DESC"
                        }),
                        new CommandArgumentPart(true, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DIRINFO_ARGUMENT_DIRECTORY_DESC"
                        }),
                        new CommandArgumentPart(false, "outputFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_OUTPUTFILE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("relative", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_SWITCH_RELATIVE_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SumFilesCommand()),

            new CommandInfo("sumtext", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMTEXT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_ALGORITHM_DESC"
                        }),
                        new CommandArgumentPart(true, "text", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMTEXT_ARGUMENT_TEXT_DESC"
                        }),
                    ])
                ], new SumTextCommand()),

            new CommandInfo("symlink", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYMLINK_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "linkname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYMLINK_ARGUMENT_LINKNAME_DESC"
                        }),
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYMLINK_ARGUMENT_TARGET_DESC"
                        }),
                    ])
                ], new SymlinkCommand()),

            new CommandInfo("sysinfo", /* Localizable */ "NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SYSINFO",
                [
                    new CommandArgumentInfo([],
                    [
                        new SwitchInfo("s", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_S_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("h", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_H_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("u", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_U_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_M_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("l", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_L_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("a", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_A_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new SysInfoCommand()),

            new CommandInfo("taskman", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_TASKMAN_DESC", new TaskManCommand()),

            new CommandInfo("themeprev", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_THEMEPREV_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "theme", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_THEMEPREV_ARGUMENT_THEMENAME_DESC"
                        }),
                    ])
                ], new ThemePrevCommand()),

            new CommandInfo("themeset", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_THEMESET_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "theme", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_THEMEPREV_ARGUMENT_THEMENAME_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("y", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_THEMESET_SWITCH_Y_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ThemeSetCommand()),

            new CommandInfo("tip", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_TIP_DESC", new TipCommand()),

            new CommandInfo("unblockdbgdev", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNBLOCKDBGDEV_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ipaddress", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNBLOCKDBGDEV_ARGUMENT_IP_DESC"
                        }),
                    ])
                ], new UnblockDbgDevCommand()),

            new CommandInfo("unzip", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNZIP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "zipfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNZIP_ARGUMENT_ZIPFILE_DESC"
                        }),
                        new CommandArgumentPart(false, "path", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNZIP_ARGUMENT_PATH_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("createdir", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UNZIP_SWITCH_CREATEDIR_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new UnZipCommand()),

#if SPECIFIERREL
            new CommandInfo("update", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UPDATE_DESC", new UpdateCommand()),
#endif

            new CommandInfo("uptime", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_UPTIME_DESC",
                [
                    new CommandArgumentInfo(true)
                ], new UptimeCommand()),

            new CommandInfo("usermanual", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_USERMANUAL_DESC", new UserManualCommand()),

            new CommandInfo("verify", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_VERIFY_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SUMFILE_ARGUMENT_ALGORITHM_DESC"
                        }),
                        new CommandArgumentPart(true, "calculatedhash", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_VERIFY_ARGUMENT_CALCULATEDHASH_DESC"
                        }),
                        new CommandArgumentPart(true, "hashfile/expectedhash", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_VERIFY_ARGUMENT_HASHFILE_DESC"
                        }),
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_VERIFY_ARGUMENT_FILE_DESC"
                        }),
                    ])
                ], new VerifyCommand()),

            new CommandInfo("version", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_VERSION_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("m", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_VERSION_SWITCH_M_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["k"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("k", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_VERSION_SWITCH_K_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["m"],
                            AcceptsValues = false
                        })
                    ], true)
                ], new VersionCommand()),

            new CommandInfo("whoami", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_WHOAMI_DESC",
                [
                    new CommandArgumentInfo(true)
                ], new WhoamiCommand()),

            new CommandInfo("winelevate", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_WINELEVATE_DESC",
                [
                    new CommandArgumentInfo(true)
                ], new WinElevateCommand()),

            new CommandInfo("wraptext", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_WRAPTEXT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_CHATTR_ARGUMENT_FILE_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("columns", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_WRAPTEXT_SWITCH_COLUMNS_DESC", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                            IsNumeric = true
                        })
                    ], true)
                ], new WrapTextCommand()),

            new CommandInfo("zip", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "zipfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_ARGUMENT_ZIPFILE_DESC"
                        }),
                        new CommandArgumentPart(true, "path", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_ARGUMENT_PATH_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("fast", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_SWITCH_FAST_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["nocomp"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("nocomp", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_SWITCH_NOCOMP_DESC", new SwitchOptions()
                        {
                            ConflictsWith = ["fast"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("nobasedir", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ZIP_SWITCH_NOBASEDIR_DESC", new SwitchOptions()
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
