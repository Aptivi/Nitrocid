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
            new CommandInfo("addgroup", /* Localizable */ "Adds groups",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "groupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "New group name to add"
                        })
                    ])
                ], new AddGroupCommand(), CommandFlags.Strict),

            new CommandInfo("adduser", /* Localizable */ "Adds users",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "New user name to add"
                        }),
                        new CommandArgumentPart(false, "password", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "New password to use"
                        }),
                        new CommandArgumentPart(false, "confirm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Confirm the new password"
                        }),
                    ])
                ], new AddUserCommand(), CommandFlags.Strict),

            new CommandInfo("addusertogroup", /* Localizable */ "Adds users to a group",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "User name to be added to the group"
                        }),
                        new CommandArgumentPart(true, "group", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Group name to add the user to"
                        }),
                    ])
                ], new AddUserToGroupCommand(), CommandFlags.Strict),

            new CommandInfo("admin", /* Localizable */ "Administrative shell", new AdminCommand(), CommandFlags.Strict),

            new CommandInfo("alarm", /* Localizable */ "Manage your alarms",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "start", new()
                        {
                            ExactWording = ["start"],
                            ArgumentDescription = /* Localizable */ "Starts the alarm"
                        }),
                        new CommandArgumentPart(true, "alarmname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Alarm name"
                        }),
                        new CommandArgumentPart(true, "interval", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Alarm interval in a time span in hours, minutes, and seconds"
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
                            ArgumentDescription = /* Localizable */ "Stops the alarm"
                        }),
                        new CommandArgumentPart(true, "alarmname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Alarm name to stop"
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
                            ArgumentDescription = /* Localizable */ "Lists the alarms"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", /* Localizable */ "Manage your alarms in an interactive TUI", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ]),
                ], new AlarmCommand(), CommandFlags.Strict),

            new CommandInfo("alias", /* Localizable */ "Adds aliases to commands",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "add", new()
                        {
                            ExactWording = ["add"],
                            ArgumentDescription = /* Localizable */ "Adds an alias"
                        }),
                        new CommandArgumentPart(true, "shell", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Shell type to fetch commands from"
                        }),
                        new CommandArgumentPart(true, "alias", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Alias name"
                        }),
                        new CommandArgumentPart(true, "cmd", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Command to be used as a shortcut"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "rem", new()
                        {
                            ExactWording = ["rem"],
                            ArgumentDescription = /* Localizable */ "Removes an alias"
                        }),
                        new CommandArgumentPart(true, "shell", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Shell type to fetch commands from"
                        }),
                        new CommandArgumentPart(true, "alias", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Alias name"
                        }),
                    ]),
                ], new AliasCommand(), CommandFlags.Strict),

            new CommandInfo("beep", /* Localizable */ "Beeps from the console",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "freq", new()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Frequency in Hz"
                        }),
                        new CommandArgumentPart(false, "ms", new()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Time interval for a single beep"
                        }),
                    ])
                ], new BeepCommand()),

            new CommandInfo("blockdbgdev", /* Localizable */ "Block a debug device by IP address",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ipaddress", new()
                        {
                            ArgumentDescription = /* Localizable */ "IP address to block"
                        }),
                    ])
                ], new BlockDbgDevCommand(), CommandFlags.Strict),

            new CommandInfo("bulkrename", /* Localizable */ "Renames group of files to selected format",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "targetdir", new()
                        {
                            ArgumentDescription = /* Localizable */ "Target directory"
                        }),
                        new CommandArgumentPart(true, "pattern", new()
                        {
                            ArgumentDescription = /* Localizable */ "Search pattern"
                        }),
                        new CommandArgumentPart(false, "newname", new()
                        {
                            ArgumentDescription = /* Localizable */ "New name to rename all found files to"
                        }),
                    ])
                ], new BulkRenameCommand()),

            new CommandInfo("cat", /* Localizable */ "Prints content of file to console",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "Path to file to print its contents"
                        }),
                    ],
                    [
                        new SwitchInfo("lines", /* Localizable */ "Prints the line numbers alongside the contents", new SwitchOptions()
                        {
                            ConflictsWith = ["nolines"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("nolines", /* Localizable */ "Prints only the contents", new SwitchOptions()
                        {
                            ConflictsWith = ["lines"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("plain", /* Localizable */ "Force treating binary files as plain text", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new CatCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("cdir", /* Localizable */ "Gets the current directory",
                [
                    new CommandArgumentInfo(true)
                ], new CDirCommand()),

            new CommandInfo("changes", /* Localizable */ "What's new in this version of Nitrocid?",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("online", /* Localizable */ "Fetch the changelogs from the internet instead of locally", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ChangesCommand()),

            new CommandInfo("chattr", /* Localizable */ "Changes attribute of a file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "Path to file"
                        }),
                        new CommandArgumentPart(true, "add/rem", new()
                        {
                            ArgumentDescription = /* Localizable */ "Either addition or removal"
                        }),
                        new CommandArgumentPart(true, "Normal/ReadOnly/Hidden/Archive", new()
                        {
                            ArgumentDescription = /* Localizable */ "Attribute name"
                        }),
                    ])
                ], new ChAttrCommand()),

            new CommandInfo("chculture", /* Localizable */ "Changes culture",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "culture", new()
                        {
                            ArgumentDescription = /* Localizable */ "Culture ID to change to"
                        }),
                    ],
                    [
                        new SwitchInfo("user", /* Localizable */ "Changes the user culture instead of the system culture", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ChCultureCommand(), CommandFlags.Strict),

            new CommandInfo("chdir", /* Localizable */ "Changes directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory/..", new()
                        {
                            ArgumentDescription = /* Localizable */ "Target directory"
                        }),
                    ])
                ], new ChDirCommand()),

            new CommandInfo("chhostname", /* Localizable */ "Changes host name",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "hostname", new()
                        {
                            ArgumentDescription = /* Localizable */ "New host name to change to"
                        }),
                    ])
                ], new ChHostNameCommand(), CommandFlags.Strict),

            new CommandInfo("chklock", /* Localizable */ "Checks the file or the folder lock",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "Path to file"
                        }),
                    ],
                    [
                        new SwitchInfo("waitforunlock", /* Localizable */ "Waits until the file or the folder is unlocked", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ChkLockCommand()),

            new CommandInfo("chlang", /* Localizable */ "Changes language",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "language", new()
                        {
                            ArgumentDescription = /* Localizable */ "Language ID"
                        }),
                    ],
                    [
                        new SwitchInfo("user", /* Localizable */ "Changes the user language instead of the system language", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("country", /* Localizable */ "Changes the language using a country (you might get prompted to choose a language)", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ChLangCommand(), CommandFlags.Strict),

            new CommandInfo("chmal", /* Localizable */ "Changes MAL, the MOTD After Login",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "message", new()
                        {
                            ArgumentDescription = /* Localizable */ "New message to use"
                        }),
                    ])
                ], new ChMalCommand(), CommandFlags.Strict),

            new CommandInfo("chmotd", /* Localizable */ "Changes MOTD, the Message Of The Day",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "message", new()
                        {
                            ArgumentDescription = /* Localizable */ "New message to use"
                        }),
                    ])
                ], new ChMotdCommand(), CommandFlags.Strict),

            new CommandInfo("choice", /* Localizable */ "Makes user choices",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "answers", new()
                        {
                            ArgumentDescription = /* Localizable */ "Answers to split with a slash"
                        }),
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = /* Localizable */ "Question to print to the screen"
                        }),
                        new CommandArgumentPart(false, "answertitle1", new()
                        {
                            ArgumentDescription = /* Localizable */ "First answer title"
                        }),
                        new CommandArgumentPart(false, "answertitle2", new()
                        {
                            ArgumentDescription = /* Localizable */ "Second answer title, and so on..."
                        }),
                    ],
                    [
                        new SwitchInfo("o", /* Localizable */ "One line choice style", new SwitchOptions()
                        {
                            ConflictsWith = ["t", "m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("t", /* Localizable */ "Two lines choice style", new SwitchOptions()
                        {
                            ConflictsWith = ["o", "m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", /* Localizable */ "Modern choice style", new SwitchOptions()
                        {
                            ConflictsWith = ["t", "o"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("single", /* Localizable */ "The output can be only one character", new SwitchOptions()
                        {
                            ConflictsWith = ["multiple"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("multiple", /* Localizable */ "The output can be more than a character", new SwitchOptions()
                        {
                            ConflictsWith = ["single"],
                            AcceptsValues = false
                        })
                    ], true, true)
                ], new ChoiceCommand()),

            new CommandInfo("chpwd", /* Localizable */ "Changes password for current user",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Username", new()
                        {
                            ArgumentDescription = /* Localizable */ "User name to change password"
                        }),
                        new CommandArgumentPart(true, "UserPass", new()
                        {
                            ArgumentDescription = /* Localizable */ "Current user password"
                        }),
                        new CommandArgumentPart(true, "newPass", new()
                        {
                            ArgumentDescription = /* Localizable */ "New password to use"
                        }),
                        new CommandArgumentPart(true, "confirm", new()
                        {
                            ArgumentDescription = /* Localizable */ "Confirm the new password"
                        }),
                    ])
                ], new ChPwdCommand(), CommandFlags.Strict),

            new CommandInfo("chusrname", /* Localizable */ "Changes user name",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "oldUserName", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => [.. UserManagement.ListAllUsers()],
                            ArgumentDescription = /* Localizable */ "Old username to change",
                        }),
                        new CommandArgumentPart(true, "newUserName", new()
                        {
                            ArgumentDescription = /* Localizable */ "New username to use"
                        }),
                    ])
                ], new ChUsrNameCommand(), CommandFlags.Strict),

            new CommandInfo("cls", /* Localizable */ "Clears the screen", new ClsCommand()),

            new CommandInfo("combinestr", /* Localizable */ "Combines the two text files or more into the console.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = /* Localizable */ "First input text file"
                        }),
                        new CommandArgumentPart(true, "input2", new()
                        {
                            ArgumentDescription = /* Localizable */ "Second input text file"
                        }),
                        new CommandArgumentPart(false, "input3", new()
                        {
                            ArgumentDescription = /* Localizable */ "Third input text file, and so on..."
                        }),
                    ], true, true)
                ], new CombineStrCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("combine", /* Localizable */ "Combines the two text files or more into the output file.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "output", new()
                        {
                            ArgumentDescription = /* Localizable */ "Output file"
                        }),
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = /* Localizable */ "First input text file"
                        }),
                        new CommandArgumentPart(true, "input2", new()
                        {
                            ArgumentDescription = /* Localizable */ "Second input text file"
                        }),
                        new CommandArgumentPart(false, "input3", new()
                        {
                            ArgumentDescription = /* Localizable */ "Third input text file, and so on..."
                        }),
                    ], false, true)
                ], new CombineCommand()),

            new CommandInfo("compare", /* Localizable */ "Compares between the two text files.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "source", new()
                        {
                            ArgumentDescription = /* Localizable */ "Source input text file"
                        }),
                        new CommandArgumentPart(true, "target", new()
                        {
                            ArgumentDescription = /* Localizable */ "Target input text file"
                        }),
                    ])
                ], new CompareCommand()),

            new CommandInfo("convertlineendings", /* Localizable */ "Converts the line endings to format for the current platform or to specified custom format",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "textfile", new()
                        {
                            ArgumentDescription = /* Localizable */ "Path to text file"
                        }),
                    ],
                    [
                        new SwitchInfo("w", /* Localizable */ "Converts the line endings to the Windows format", new SwitchOptions()
                        {
                            ConflictsWith = ["u", "m"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("u", /* Localizable */ "Converts the line endings to the Unix format", new SwitchOptions()
                        {
                            ConflictsWith = ["m", "w"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("m", /* Localizable */ "Converts the line endings to the Mac OS 9 format", new SwitchOptions()
                        {
                            ConflictsWith = ["u", "w"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("force", /* Localizable */ "Forces the line ending conversion", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ])
                ], new ConvertLineEndingsCommand()),

            new CommandInfo("copy", /* Localizable */ "Creates another copy of a file under different directory or name.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "source", new()
                        {
                            ArgumentDescription = /* Localizable */ "Source file or directory"
                        }),
                        new CommandArgumentPart(true, "target", new()
                        {
                            ArgumentDescription = /* Localizable */ "Target file or directory"
                        }),
                    ])
                ], new CopyCommand()),

            new CommandInfo("date", /* Localizable */ "Shows date and time",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("date", /* Localizable */ "Shows just the date", new SwitchOptions()
                        {
                            ConflictsWith = ["time", "full"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("time", /* Localizable */ "Shows just the time", new SwitchOptions()
                        {
                            ConflictsWith = ["date", "full"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("full", /* Localizable */ "Shows date and time", new SwitchOptions()
                        {
                            ConflictsWith = ["date", "time"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("utc", /* Localizable */ "Uses UTC instead of local", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ], true)
                ], new DateCommand(), CommandFlags.RedirectionSupported),

            new CommandInfo("debugshell", /* Localizable */ "Starts the debug shell", new DebugShellCommand(), CommandFlags.Strict),

            new CommandInfo("decodefile", /* Localizable */ "Decodes the encoded file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "Path to encoded file"
                        }),
                    ],
                    [
                        new SwitchInfo("key", /* Localizable */ "Specifies the key", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new DecodeFileCommand()),

            new CommandInfo("decodetext", /* Localizable */ "Decodes the encoded text",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "encodedString", new()
                        {
                            ArgumentDescription = /* Localizable */ "Encoded string representation"
                        }),
                    ],
                    [
                        new SwitchInfo("key", /* Localizable */ "Specifies the key", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new DecodeTextCommand()),

            new CommandInfo("decodebase64", /* Localizable */ "Decodes the text from the BASE64 representation",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "encoded", new()
                        {
                            ArgumentDescription = /* Localizable */ "Encoded BASE64 string"
                        })
                    ])
                ], new DecodeBase64Command()),

            new CommandInfo("dirinfo", /* Localizable */ "Provides information about a directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new()
                        {
                            ArgumentDescription = /* Localizable */ "Path to directory"
                        }),
                    ])
                ], new DirInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("disconndbgdev", /* Localizable */ "Disconnect a debug device",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ip", new()
                        {
                            ArgumentDescription = /* Localizable */ "IP address to disconnect"
                        }),
                    ])
                ], new DisconnDbgDevCommand(), CommandFlags.Strict),

            new CommandInfo("diskinfo", /* Localizable */ "Provides information about a disk",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Disk number"
                        }),
                    ], true)
                ], new DiskInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("dismissnotif", /* Localizable */ "Dismisses a notification",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "notificationNumber", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Notification number"
                        }),
                    ])
                ], new DismissNotifCommand()),

            new CommandInfo("dismissnotifs", /* Localizable */ "Dismisses all notifications", new DismissNotifsCommand()),

            new CommandInfo("driverman", /* Localizable */ "Manage your drivers",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "change", new()
                        {
                            ExactWording = ["change"],
                            ArgumentDescription = /* Localizable */ "Change a driver"
                        }),
                        new CommandArgumentPart(true, "type", new()
                        {
                            AutoCompleter = (_) => Enum.GetNames<DriverTypes>(),
                            ArgumentDescription = /* Localizable */ "Target driver type"
                        }),
                        new CommandArgumentPart(true, "driver", new()
                        {
                            AutoCompleter = (args) => DriverHandler.GetDriverNames(DriverHandler.InferDriverTypeFromTypeName(args[1])),
                            ArgumentDescription = /* Localizable */ "Target driver to change to"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list", new()
                        {
                            ExactWording = ["list"],
                            ArgumentDescription = /* Localizable */ "List drivers"
                        }),
                        new CommandArgumentPart(true, "type", new()
                        {
                            AutoCompleter = (_) => Enum.GetNames<DriverTypes>(),
                            ArgumentDescription = /* Localizable */ "Target driver type"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "types", new()
                        {
                            ExactWording = ["types"],
                            ArgumentDescription = /* Localizable */ "List driver types"
                        }),
                    ]),
                ], new DriverManCommand(), CommandFlags.Strict),

            new CommandInfo("echo", /* Localizable */ "Writes text into the console",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "text", new()
                        {
                            ArgumentDescription = /* Localizable */ "String to print"
                        }),
                    ],
                    [
                        new SwitchInfo("noparse", /* Localizable */ "Prints the text as it is with no placeholder parsing", false, false, [], 0, false)
                    ], true)
                ], new EchoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("edit", /* Localizable */ "Edits a file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "Path to file"
                        }),
                    ],
                    [
                        new SwitchInfo("text", /* Localizable */ "Forces text mode", new SwitchOptions()
                        {
                            ConflictsWith = ["sql", "json", "hex"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("hex", /* Localizable */ "Forces hex mode", new SwitchOptions()
                        {
                            ConflictsWith = ["text", "json", "sql"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("json", /* Localizable */ "Forces JSON mode", new SwitchOptions()
                        {
                            ConflictsWith = ["text", "sql", "hex"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("sql", /* Localizable */ "Forces SQL mode", new SwitchOptions()
                        {
                            ConflictsWith = ["text", "json", "hex"],
                            AcceptsValues = false
                        }),
                    ])
                ], new EditCommand()),

            new CommandInfo("encodefile", /* Localizable */ "Encodes the file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "File to encode"
                        }),
                    ],
                    [
                        new SwitchInfo("key", /* Localizable */ "Specifies the key", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new EncodeFileCommand()),

            new CommandInfo("encodetext", /* Localizable */ "Encodes the text",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "string", new()
                        {
                            ArgumentDescription = /* Localizable */ "String to encode"
                        }),
                    ],
                    [
                        new SwitchInfo("key", /* Localizable */ "Specifies the key", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("iv", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("algorithm", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                        }),
                    ])
                ], new EncodeTextCommand()),

            new CommandInfo("encodebase64", /* Localizable */ "Encodes the text to a BASE64 representation",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "string", new()
                        {
                            ArgumentDescription = /* Localizable */ "String to encode"
                        })
                    ])
                ], new EncodeBase64Command()),

            new CommandInfo("fileinfo", /* Localizable */ "Provides information about a file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "Path to file"
                        }),
                    ])
                ], new FileInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("find", /* Localizable */ "Finds a file in the specified directory or in the current directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new()
                        {
                            ArgumentDescription = /* Localizable */ "File name"
                        }),
                        new CommandArgumentPart(true, "directory", new()
                        {
                            ArgumentDescription = /* Localizable */ "Path to directory"
                        }),
                    ],
                    [
                        new SwitchInfo("recursive", /* Localizable */ "Searches for a file recursively", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("exec", /* Localizable */ "Executes a command on a file", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ], true)
                ], new FindCommand()),

            new CommandInfo("findreg", /* Localizable */ "Finds a file in the specified directory or in the current directory using regular expressions",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "fileRegex", new()
                        {
                            ArgumentDescription = /* Localizable */ "Regular expression to find a file"
                        }),
                        new CommandArgumentPart(true, "directory", new()
                        {
                            ArgumentDescription = /* Localizable */ "Path to directory"
                        }),
                    ],
                    [
                        new SwitchInfo("recursive", /* Localizable */ "Searches for a file recursively", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("exec", /* Localizable */ "Executes a command on a file", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ], true)
                ], new FindRegCommand()),

            new CommandInfo("fork", /* Localizable */ "Forks the UESH shell to create another instance", new ForkCommand()),

            new CommandInfo("get", /* Localizable */ "Downloads a file to current working directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "url", new()
                        {
                            ArgumentDescription = /* Localizable */ "Link to a remote resource, such as HTTPS"
                        })
                    ],
                    [
                        new SwitchInfo("outputpath", /* Localizable */ "Specifies the output path", new SwitchOptions()
                        {
                            ArgumentsRequired = true
                        })
                    ])
                ], new GetCommand()),

            new CommandInfo("getaddons", /* Localizable */ "Gets all the addons from the official download page and installs them",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("reinstall", /* Localizable */ "Reinstalls the addons", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new GetAddonsCommand()),

            new CommandInfo("getallexthandlers", /* Localizable */ "Gets all the extension handlers from all the extensions", new GetAllExtHandlersCommand()),

            new CommandInfo("getconfigvalue", /* Localizable */ "Gets a configuration variable and its value",
                [
                    new CommandArgumentInfo(new CommandArgumentPart[]
                    {
                        new(true, "config", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray(),
                            ArgumentDescription = /* Localizable */ "Kernel configuration name"
                        }),
                        new(true, "variable", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => ConfigTools.GetSettingsKeys(arg[0]).Select((sk) => sk.Variable).ToArray(),
                            ArgumentDescription = /* Localizable */ "Variable to get info"
                        })
                    }, true)
                ], new GetConfigValueCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("getdefaultexthandler", /* Localizable */ "Gets the default extension handler from the specified extension",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                            ArgumentDescription = /* Localizable */ "File extension"
                        }),
                    ], true)
                ], new GetDefaultExtHandlerCommand()),

            new CommandInfo("getdefaultexthandlers", /* Localizable */ "Gets the default extension handlers from all the extensions", new GetDefaultExtHandlersCommand()),

            new CommandInfo("getexthandlers", /* Localizable */ "Gets the extension handlers from the specified extension",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                            ArgumentDescription = /* Localizable */ "File extension"
                        }),
                    ], true)
                ], new GetExtHandlersCommand()),

            new CommandInfo("getkeyiv", /* Localizable */ "Gets the key and the initialization vector for symmetrical encoding",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames(),
                            ArgumentDescription = /* Localizable */ "Encoding algorithm"
                        }),
                    ], true)
                ], new GetKeyIvCommand()),

            new CommandInfo("host", /* Localizable */ "Gets the current host name",
                [
                    new CommandArgumentInfo(true)
                ], new HostCommand()),

            new CommandInfo("hwinfo", /* Localizable */ "Prints hardware information",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "HardwareType", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => DriverHandler.CurrentHardwareProberDriverLocal.SupportedHardwareTypes.Union(["all"]).ToArray(),
                            ArgumentDescription = /* Localizable */ "Hardware type"
                        })
                    ])
                ], new HwInfoCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("if", /* Localizable */ "Executes commands once the UESH expressions are satisfied",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ueshExpression", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "UESH expression to test"
                        }),
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Command to execute upon successful evaluation"
                        }),
                    ])
                ], new IfCommand()),

            new CommandInfo("ifm", /* Localizable */ "Interactive system host file manager",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "firstPanePath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to directory for the left pane"
                        }),
                        new CommandArgumentPart(false, "secondPanePath", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to directory for the right pane"
                        }),
                    ])
                ], new IfmCommand()),

            new CommandInfo("input", /* Localizable */ "Allows user to enter input",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "question", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Question to print to the screen"
                        }),
                    ], true)
                ], new InputCommand()),

            new CommandInfo("inputpass", /* Localizable */ "Allows user to enter input as password",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "question", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Question to print to the screen"
                        }),
                    ], true)
                ], new InputPassCommand()),

            new CommandInfo("langman", /* Localizable */ "Manage your languages",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "reload/load/unload", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Reloads, loads, or unloads a custom language"
                        }),
                        new CommandArgumentPart(true, "customlanguagename", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => [.. LanguageManager.CustomLanguages.Keys],
                            ArgumentDescription = /* Localizable */ "Custom language name"
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "list/reloadall", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Lists or reloads all custom languages"
                        }),
                    ])
                ], new LangManCommand(), CommandFlags.Strict),

            new CommandInfo("license", /* Localizable */ "Shows license information for the kernel", new LicenseCommand()),

            new CommandInfo("lintscript", /* Localizable */ "Checks a UESH script for syntax errors",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "script", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to UESH script"
                        }),
                    ], true)
                ], new LintScriptCommand()),

            new CommandInfo("list", /* Localizable */ "List file/folder contents in current folder",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to directory to list"
                        }),
                    ],
                    [
                        new SwitchInfo("showdetails", /* Localizable */ "Shows the file details in the list", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("suppressmessages", /* Localizable */ "Suppresses the annoying \"permission denied\" messages", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("recursive", /* Localizable */ "Lists a folder recursively", new SwitchOptions()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["tree"]
                        }),
                        new SwitchInfo("tree", /* Localizable */ "Lists a folder using the tree form", new SwitchOptions()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["recursive"]
                        })
                    ])
                ], new ListCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lockscreen", /* Localizable */ "Locks your screen with a password", new LockScreenCommand()),

            new CommandInfo("logout", /* Localizable */ "Logs you out", new LogoutCommand(), CommandFlags.NoMaintenance),

            new CommandInfo("lsconfigs", /* Localizable */ "Lists all available configurations",
                [
                    new CommandArgumentInfo(new SwitchInfo[]
                    {
                        new("deep", /* Localizable */ "Deep details about all configurations, including their entries")
                    })
                ], new LsConfigsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsconfigvalues", /* Localizable */ "Lists all configuration variables and their values",
                [
                    new CommandArgumentInfo(new CommandArgumentPart[]
                    {
                        new(true, "config", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray(),
                            ArgumentDescription = /* Localizable */ "Kernel configuration name"
                        })
                    })
                ], new LsConfigValuesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsconnections", /* Localizable */ "Lists all available connections", new LsConnectionsCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsdbgdev", /* Localizable */ "Lists debugging devices connected", new LsDbgDevCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsexthandlers", /* Localizable */ "Lists available extension handlers", new LsExtHandlersCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsdiskparts", /* Localizable */ "Lists all the disk partitions",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Disk number"
                        }),
                    ], true)
                ], new LsDiskPartsCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsdisks", /* Localizable */ "Lists all the disks",
                [
                    new CommandArgumentInfo(true)
                ], new LsDisksCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("lsnet", /* Localizable */ "Lists online network devices", new LsNetCommand(), CommandFlags.Strict),

            new CommandInfo("lsvars", /* Localizable */ "Lists available UESH variables", new LsVarsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("md", /* Localizable */ "Creates a directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "directory", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Directory name to create"
                        }),
                    ], true)
                ], new MdCommand()),

            new CommandInfo("mkfile", /* Localizable */ "Makes a new file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "File name to create"
                        }),
                    ], true)
                ], new MkFileCommand()),

            new CommandInfo("move", /* Localizable */ "Moves a file to another directory",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "source", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Directory or name to move from"
                        }),
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Directory or name to move to"
                        }),
                    ])
                ], new MoveCommand()),

            new CommandInfo("partinfo", /* Localizable */ "Provides information about a partition from the specified disk",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Disk number"
                        }),
                        new CommandArgumentPart(true, "partNum", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Partition number"
                        }),
                    ], true)
                ], new PartInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("pathfind", /* Localizable */ "Finds a given file name from path lookup directories",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "fileName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "File name to lookup"
                        }),
                    ], true)
                ], new PathFindCommand()),

            new CommandInfo("perm", /* Localizable */ "Manage permissions for users",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "userName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "User name"
                        }),
                        new CommandArgumentPart(true, "allow/revoke", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Either allow or revoke"
                        }),
                        new CommandArgumentPart(true, "perm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Permission name to grant or deny"
                        }),
                    ])
                ], new PermCommand(), CommandFlags.Strict),

            new CommandInfo("permgroup", /* Localizable */ "Manage permissions for groups",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "groupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Group name"
                        }),
                        new CommandArgumentPart(true, "allow/revoke", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Either allow or revoke"
                        }),
                        new CommandArgumentPart(true, "perm", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Permission name to grant or deny"
                        }),
                    ])
                ], new PermGroupCommand(), CommandFlags.Strict),

            new CommandInfo("ping", /* Localizable */ "Pings an address",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Address1", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "First IP address or host name"
                        }),
                        new CommandArgumentPart(false, "Address2", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Second IP address or host name, and so on..."
                        }),
                    ],
                    [
                        new SwitchInfo("times", /* Localizable */ "Specifies number of times to ping", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                            IsNumeric = true
                        })
                    ], false, true)
                ], new PingCommand()),

            new CommandInfo("platform", /* Localizable */ "Gets the current platform",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("n", /* Localizable */ "Shows the platform name", new SwitchOptions()
                        {
                            ConflictsWith = ["r", "v", "b", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("v", /* Localizable */ "Shows the platform version", new SwitchOptions()
                        {
                            ConflictsWith = ["n", "r", "b", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("b", /* Localizable */ "Shows the platform bits", new SwitchOptions()
                        {
                            ConflictsWith = ["n", "v", "r", "c"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("c", /* Localizable */ "Shows the .NET platform version", new SwitchOptions()
                        {
                            ConflictsWith = ["n", "v", "b", "r"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("r", /* Localizable */ "Shows the .NET platform runtime identifier", new SwitchOptions()
                        {
                            ConflictsWith = ["n", "v", "b", "c"],
                            AcceptsValues = false
                        })
                    ], true)
                ], new PlatformCommand()),

            new CommandInfo("put", /* Localizable */ "Uploads a file to specified website",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "FileName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to file to upload"
                        }),
                        new CommandArgumentPart(true, "URL", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "URL pointing to the upload endpoint"
                        }),
                    ])
                ], new PutCommand()),

            new CommandInfo("rdebug", /* Localizable */ "Enables or disables remote debugging.", new RdebugCommand(), CommandFlags.Strict),

            new CommandInfo("reboot", /* Localizable */ "Restarts the kernel",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("safe", /* Localizable */ "Restarts the kernel to safe mode.", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["maintenance", "debug"]
                        }),
                        new SwitchInfo("maintenance", /* Localizable */ "Restarts the kernel to maintenance mode.", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "debug"]
                        }),
                        new SwitchInfo("debug", /* Localizable */ "Restarts the kernel to debug mode.", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "maintenance"]
                        }),
                    ])
                ], new RebootCommand()),

            new CommandInfo("reloadconfig", /* Localizable */ "Reloads configuration file that is edited.", new ReloadConfigCommand(), CommandFlags.Strict),

            new CommandInfo("rexec", /* Localizable */ "Remotely executes a command to remote PC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Host name that runs Nitrocid KS"
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Port of Nitrocid KS on the remote target"
                        }),
                        new CommandArgumentPart(false, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Command to execute remotely"
                        }),
                    ])
                ], new RexecCommand(), CommandFlags.Strict),

            new CommandInfo("rm", /* Localizable */ "Removes a directory or a file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Target directory or file"
                        }),
                    ])
                ], new RmCommand()),

            new CommandInfo("rmsec", /* Localizable */ "Removes a file or a directory securely",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Target directory or file"
                        }),
                    ])
                ], new RmSecCommand()),

            new CommandInfo("rmuser", /* Localizable */ "Removes a user from the list",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Username", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "User name to remove"
                        }),
                    ])
                ], new RmUserCommand(), CommandFlags.Strict),

            new CommandInfo("rmgroup", /* Localizable */ "Removes a group from the list",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "GroupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Group name to remove"
                        }),
                    ])
                ], new RmGroupCommand(), CommandFlags.Strict),

            new CommandInfo("rmuserfromgroup", /* Localizable */ "Removes a user from the group",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "UserName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "User name to remove from group"
                        }),
                        new CommandArgumentPart(true, "GroupName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Group name to remove user from"
                        }),
                    ])
                ], new RmUserFromGroupCommand(), CommandFlags.Strict),

            new CommandInfo("rreboot", /* Localizable */ "Restarts a remote kernel instance",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Host name that runs Nitrocid KS"
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Port of Nitrocid KS on the remote target"
                        }),
                    ],
                    [
                        new SwitchInfo("safe", /* Localizable */ "Restarts the kernel to safe mode.", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["maintenance", "debug"]
                        }),
                        new SwitchInfo("maintenance", /* Localizable */ "Restarts the kernel to maintenance mode.", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "debug"]
                        }),
                        new SwitchInfo("debug", /* Localizable */ "Restarts the kernel to debug mode.", new()
                        {
                            AcceptsValues = false,
                            ConflictsWith = ["safe", "maintenance"]
                        }),
                    ])
                ], new RebootCommand()),

            new CommandInfo("rshutdown", /* Localizable */ "The kernel in the remote instance will be shut down",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "address", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Host name that runs Nitrocid KS"
                        }),
                        new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Port of Nitrocid KS on the remote target"
                        }),
                    ])
                ], new RShutdownCommand()),

            new CommandInfo("saveconfig", /* Localizable */ "Saves the current kernel configuration to its file", new SaveConfigCommand(), CommandFlags.Strict),

            new CommandInfo("savescreen", /* Localizable */ "Saves your screen from burn outs",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "saver/random", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Either the screensaver name or 'random'"
                        }),
                    ],
                    [
                        new SwitchInfo("select", /* Localizable */ "Gives you an option to select the screensaver to try out", new()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SaveScreenCommand()),

            new CommandInfo("search", /* Localizable */ "Searches for specified string in the provided file using regular expressions",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "Regexp", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Regular expression to search"
                        }),
                        new CommandArgumentPart(true, "File", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Target file"
                        }),
                    ])
                ], new SearchCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("searchword", /* Localizable */ "Searches for specified string in the provided file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "StringEnclosedInDoubleQuotes", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Word or phrase to search"
                        }),
                        new CommandArgumentPart(true, "File", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Target file"
                        }),
                    ])
                ], new SearchWordCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("select", /* Localizable */ "Provides a selection choice",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "answers", new()
                        {
                            ArgumentDescription = /* Localizable */ "Answers to split with a slash"
                        }),
                        new CommandArgumentPart(true, "input", new()
                        {
                            ArgumentDescription = /* Localizable */ "Question to print to the screen"
                        }),
                        new CommandArgumentPart(false, "answertitle1", new()
                        {
                            ArgumentDescription = /* Localizable */ "First answer title"
                        }),
                        new CommandArgumentPart(false, "answertitle2", new()
                        {
                            ArgumentDescription = /* Localizable */ "Second answer title, and so on..."
                        }),
                    ], true, true)
                ], new SelectCommand()),

            new CommandInfo("setsaver", /* Localizable */ "Sets up kernel screensavers",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "saver", new()
                        {
                            ArgumentDescription = /* Localizable */ "Screensaver name"
                        }),
                    ])
                ], new SetSaverCommand(), CommandFlags.Strict),

            new CommandInfo("settings", /* Localizable */ "Changes kernel configuration",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("saver", /* Localizable */ "Opens the screensaver settings", new SwitchOptions()
                        {
                            ConflictsWith = ["splash", "addonsplash", "type", "addonsaver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("addonsaver", /* Localizable */ "Opens the addon screensaver settings", new SwitchOptions()
                        {
                            ConflictsWith = ["splash", "addonsplash", "type", "saver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("splash", /* Localizable */ "Opens the splash settings", new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "addonsplash", "type", "addonsaver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("addonsplash", /* Localizable */ "Opens the addon splash settings", new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "splash", "type", "addonsaver", "driver"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("driver", /* Localizable */ "Opens the driver settings", new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "addonsplash", "type", "addonsaver", "splash"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("type", /* Localizable */ "Opens the custom settings", new SwitchOptions()
                        {
                            ConflictsWith = ["saver", "addonsplash", "splash", "addonsaver", "driver"],
                            ArgumentsRequired = true
                        }),
                        new SwitchInfo("sel", /* Localizable */ "Uses the legacy settings style", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SettingsCommand(), CommandFlags.Strict),

            new CommandInfo("set", /* Localizable */ "Sets a variable to a value in a script",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "value", new()
                        {
                            ArgumentDescription = /* Localizable */ "Value to set"
                        }),
                    ], true)
                ], new SetCommand()),

            new CommandInfo("setconfigvalue", /* Localizable */ "Sets a configuration variable to a specified value",
                [
                    new CommandArgumentInfo(new CommandArgumentPart[]
                    {
                        new(true, "config", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray(),
                            ArgumentDescription = /* Localizable */ "Kernel configuration name"
                        }),
                        new(true, "variable", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (arg) => ConfigTools.GetSettingsKeys(arg[0]).Select((sk) => sk.Variable).ToArray(),
                            ArgumentDescription = /* Localizable */ "Configuration variable name"
                        }),
                        new(true, "value")
                    })
                ], new SetConfigValueCommand()),

            new CommandInfo("setexthandler", /* Localizable */ "Sets the default extension handler of the specified extension to the specific implementer",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray(),
                            ArgumentDescription = /* Localizable */ "File extension"
                        }),
                        new CommandArgumentPart(true, "implementer", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (args) => ExtensionHandlerTools.GetExtensionHandlers(args[0]).Select((h) => h.Implementer).ToArray(),
                            ArgumentDescription = /* Localizable */ "Implementer name"
                        }),
                    ])
                ], new SetExtHandlerCommand()),

            new CommandInfo("setrange", /* Localizable */ "Creates a variable array with the provided values",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "value", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "First value"
                        }),
                        new CommandArgumentPart(false, "value2", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Second value"
                        }),
                        new CommandArgumentPart(false, "value3", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Third value, and so on..."
                        }),
                    ], true, true)
                ], new SetRangeCommand()),

            new CommandInfo("shownotifs", /* Localizable */ "Shows all received notifications",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("tui", /* Localizable */ "Shows all received notifications in an interactive TUI", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ShowNotifsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("showtd", /* Localizable */ "Shows date and time", new ShowTdCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("showtdzone", /* Localizable */ "Shows date and time in zones",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "timezone", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Time zone name"
                        }),
                    ],
                    [
                        new SwitchInfo("all", /* Localizable */ "Shows all the time zones", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1,
                            AcceptsValues = false
                        }),
                        new SwitchInfo("selection", /* Localizable */ "Opens an interactive TUI in which you'll be able to see the world clock in real time", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1,
                            AcceptsValues = false
                        })
                    ])
                ], new ShowTdZoneCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("shutdown", /* Localizable */ "The kernel will be shut down", new ShutdownCommand()),

            new CommandInfo("sleep", /* Localizable */ "Sleeps for specified milliseconds",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ms", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Interval to sleep"
                        }),
                    ])
                ], new SleepCommand()),
            new CommandInfo("sudo", /* Localizable */ "Runs the command as the root user",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "command", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Command to run"
                        }),
                    ])
                ], new SudoCommand()),

            new CommandInfo("sumfile", /* Localizable */ "Calculates file sums.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = /* Localizable */ "Encryption algorithm"
                        }),
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Input file to calculate the hash sum"
                        }),
                        new CommandArgumentPart(false, "outputFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Output file to save hash sum info"
                        }),
                    ],
                    [
                        new SwitchInfo("relative", /* Localizable */ "Uses relative path instead of absolute", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SumFileCommand()),

            new CommandInfo("sumfiles", /* Localizable */ "Calculates sums of files in specified directory.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = /* Localizable */ "Encryption algorithm"
                        }),
                        new CommandArgumentPart(true, "dir", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to directory"
                        }),
                        new CommandArgumentPart(false, "outputFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Output file to save hash sum info"
                        }),
                    ],
                    [
                        new SwitchInfo("relative", /* Localizable */ "Uses relative path instead of absolute", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new SumFilesCommand()),

            new CommandInfo("sumtext", /* Localizable */ "Calculates text sums.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = /* Localizable */ "Encryption algorithm"
                        }),
                        new CommandArgumentPart(true, "text", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Text to get hash sum"
                        }),
                    ])
                ], new SumTextCommand()),

            new CommandInfo("symlink", /* Localizable */ "Creates a symbolic link to a file or a folder",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "linkname", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Symbolic link name"
                        }),
                        new CommandArgumentPart(true, "target", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Target file to create a shortcut from"
                        }),
                    ])
                ], new SymlinkCommand()),

            new CommandInfo("sysinfo", /* Localizable */ "System information",
                [
                    new CommandArgumentInfo([],
                    [
                        new SwitchInfo("s", /* Localizable */ "Shows the system information"),
                        new SwitchInfo("h", /* Localizable */ "Shows the hardware information"),
                        new SwitchInfo("u", /* Localizable */ "Shows the user information"),
                        new SwitchInfo("m", /* Localizable */ "Shows the message of the day"),
                        new SwitchInfo("l", /* Localizable */ "Shows the message of the day after login"),
                        new SwitchInfo("a", /* Localizable */ "Shows all information"),
                    ])
                ], new SysInfoCommand()),

            new CommandInfo("taskman", /* Localizable */ "Task manager", new TaskManCommand()),

            new CommandInfo("themeprev", /* Localizable */ "Previews a theme",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "theme", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Theme name"
                        }),
                    ])
                ], new ThemePrevCommand()),

            new CommandInfo("themeset", /* Localizable */ "Selects a theme and sets it",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "theme", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Theme name"
                        }),
                    ],
                    [
                        new SwitchInfo("y", /* Localizable */ "Immediately set the theme on selection")
                    ])
                ], new ThemeSetCommand()),

            new CommandInfo("unblockdbgdev", /* Localizable */ "Unblock a debug device by IP address",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "ipaddress", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "IP address to unblock"
                        }),
                    ])
                ], new UnblockDbgDevCommand(), CommandFlags.Strict),

            new CommandInfo("unset", /* Localizable */ "Removes a variable from the UESH variable list",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "$variable", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Variable name"
                        }),
                    ],
                    [
                        new SwitchInfo("justwipe", /* Localizable */ "Just wipes the variable value without removing it", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new UnsetCommand()),

            new CommandInfo("unzip", /* Localizable */ "Extracts a ZIP archive",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "zipfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to ZIP file"
                        }),
                        new CommandArgumentPart(false, "path", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Target path to extract"
                        }),
                    ],
                    [
                        new SwitchInfo("createdir", /* Localizable */ "Creates a directory that contains the contents of the ZIP file", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new UnZipCommand()),

            #if SPECIFIERREL
            new CommandInfo("update", /* Localizable */ "System update", new UpdateCommand(), CommandFlags.Strict),
            #endif

            new CommandInfo("uptime", /* Localizable */ "Shows the kernel uptime",
                [
                    new CommandArgumentInfo(true)
                ], new UptimeCommand()),

            new CommandInfo("usermanual", /* Localizable */ "Shows the two useful URLs for manual.", new UserManualCommand()),

            new CommandInfo("verify", /* Localizable */ "Verifies sanity of the file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "algorithm", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames(),
                            ArgumentDescription = /* Localizable */ "Encryption algorithm"
                        }),
                        new CommandArgumentPart(true, "calculatedhash", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Calculated hash"
                        }),
                        new CommandArgumentPart(true, "hashfile/expectedhash", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Hash file or expected hash"
                        }),
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "File to test"
                        }),
                    ])
                ], new VerifyCommand()),

            new CommandInfo("version", /* Localizable */ "Gets the current version",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("m", /* Localizable */ "Shows the kernel mod API version", new SwitchOptions()
                        {
                            ConflictsWith = ["k"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("k", /* Localizable */ "Shows the kernel version", new SwitchOptions()
                        {
                            ConflictsWith = ["m"],
                            AcceptsValues = false
                        })
                    ], true)
                ], new VersionCommand()),

            new CommandInfo("whoami", /* Localizable */ "Gets the current user name",
                [
                    new CommandArgumentInfo(true)
                ], new WhoamiCommand()),

            new CommandInfo("winelevate", /* Localizable */ "Restarts Nitrocid with the elevated permissions (Windows only)",
                [
                    new CommandArgumentInfo(true)
                ], new WinElevateCommand()),

            new CommandInfo("wraptext", /* Localizable */ "Wraps the text file",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to file"
                        }),
                    ],
                    [
                        new SwitchInfo("columns", /* Localizable */ "Specifies the columns per line", new SwitchOptions()
                        {
                            ArgumentsRequired = true,
                            IsNumeric = true
                        })
                    ], true)
                ], new WrapTextCommand()),

            new CommandInfo("zip", /* Localizable */ "Creates a ZIP archive",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "zipfile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "ZIP file name to create"
                        }),
                        new CommandArgumentPart(true, "path", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to compress"
                        }),
                    ],
                    [
                        new SwitchInfo("fast", /* Localizable */ "Fast compression", new SwitchOptions()
                        {
                            ConflictsWith = ["nocomp"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("nocomp", /* Localizable */ "No compression", new SwitchOptions()
                        {
                            ConflictsWith = ["fast"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("nobasedir", /* Localizable */ "Don't create base directory in archive", new SwitchOptions()
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
