//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using KS.Shell.Shells.UESH.Commands;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Prompts;
using System.Linq;
using KS.Users;
using KS.Languages;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Switches;
using KS.Drivers.Encryption;
using KS.Shell.Shells.UESH.Presets;
using KS.Drivers.Encoding;
using KS.Files.Extensions;
using KS.Kernel.Configuration;

namespace KS.Shell.Shells.UESH
{
    /// <summary>
    /// UESH common shell properties
    /// </summary>
    internal class UESHShellInfo : BaseShellInfo, IShellInfo
    {
        private static readonly string[] hardwareTypes = ["HDD", "CPU", "GPU", "RAM", "all"];

        /// <summary>
        /// List of commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "addgroup",
                new CommandInfo("addgroup", /* Localizable */ "Adds groups",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "groupName")
                        })
                    ], new AddGroupCommand(), CommandFlags.Strict)
            },

            { "adduser",
                new CommandInfo("adduser", /* Localizable */ "Adds users",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "username"),
                            new CommandArgumentPart(false, "password"),
                            new CommandArgumentPart(false, "confirm"),
                        })
                    ], new AddUserCommand(), CommandFlags.Strict)
            },

            { "addusertogroup",
                new CommandInfo("addusertogroup", /* Localizable */ "Adds users to a group",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "username"),
                            new CommandArgumentPart(true, "group"),
                        })
                    ], new AddUserToGroupCommand(), CommandFlags.Strict)
            },

            { "admin",
                new CommandInfo("admin", /* Localizable */ "Administrative shell",
                    [
                        new CommandArgumentInfo()
                    ], new AdminCommand(), CommandFlags.Strict)
            },

            { "alias",
                new CommandInfo("alias", /* Localizable */ "Adds aliases to commands",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "rem/add"),
                            new CommandArgumentPart(true, $"{string.Join("/", Enum.GetNames(typeof(ShellType)))}"),
                            new CommandArgumentPart(true, "alias"),
                            new CommandArgumentPart(false, "cmd"),
                        })
                    ], new AliasCommand(), CommandFlags.Strict)
            },

            { "beep",
                new CommandInfo("beep", /* Localizable */ "Beeps from the console",
                    [
                        new CommandArgumentInfo()
                    ], new BeepCommand())
            },

            { "blockdbgdev",
                new CommandInfo("blockdbgdev", /* Localizable */ "Block a debug device by IP address",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ipaddress"),
                        })
                    ], new BlockDbgDevCommand(), CommandFlags.Strict)
            },

            { "bulkrename",
                new CommandInfo("bulkrename", /* Localizable */ "Renames group of files to selected format",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "targetdir"),
                            new CommandArgumentPart(true, "pattern"),
                            new CommandArgumentPart(false, "newname"),
                        })
                    ], new BulkRenameCommand())
            },

            { "cat",
                new CommandInfo("cat", /* Localizable */ "Prints content of file to console",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "file"),
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
                    ], new CatCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "cdir",
                new CommandInfo("cdir", /* Localizable */ "Gets the current directory",
                    [
                        new CommandArgumentInfo(true)
                    ], new CDirCommand())
            },

            { "chattr",
                new CommandInfo("chattr", /* Localizable */ "Changes attribute of a file",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(true, "add/rem"),
                            new CommandArgumentPart(true, "Normal/ReadOnly/Hidden/Archive"),
                        })
                    ], new ChAttrCommand())
            },

            { "chdir",
                new CommandInfo("chdir", /* Localizable */ "Changes directory",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory/.."),
                        })
                    ], new ChDirCommand())
            },

            { "chhostname",
                new CommandInfo("chhostname", /* Localizable */ "Changes host name",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "hostname"),
                        })
                    ], new ChHostNameCommand(), CommandFlags.Strict)
            },

            { "chklock",
                new CommandInfo("chklock", /* Localizable */ "Checks the file or the folder lock",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "file"),
                        ],
                        [
                            new SwitchInfo("waitforunlock", /* Localizable */ "Waits until the file or the folder is unlocked", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        ])
                    ], new ChkLockCommand())
            },

            { "chlang",
                new CommandInfo("chlang", /* Localizable */ "Changes language",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "language"),
                        ],
                        [
                            new SwitchInfo("usesyslang", /* Localizable */ "Uses the system language settings to try to infer the language from", new SwitchOptions()
                            {
                                OptionalizeLastRequiredArguments = 1,
                                AcceptsValues = false
                            }),
                            new SwitchInfo("user", /* Localizable */ "Changes the user language instead of the system language", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                        ])
                    ], new ChLangCommand(), CommandFlags.Strict)
            },

            { "chmal",
                new CommandInfo("chmal", /* Localizable */ "Changes MAL, the MOTD After Login",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "message"),
                        })
                    ], new ChMalCommand(), CommandFlags.Strict)
            },

            { "chmotd",
                new CommandInfo("chmotd", /* Localizable */ "Changes MOTD, the Message Of The Day",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "message"),
                        })
                    ], new ChMotdCommand(), CommandFlags.Strict)
            },

            { "choice",
                new CommandInfo("choice", /* Localizable */ "Makes user choices",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "answers"),
                            new CommandArgumentPart(true, "input"),
                            new CommandArgumentPart(false, "answertitle1"),
                            new CommandArgumentPart(false, "answertitle2 ..."),
                        ],
                        [
                            new SwitchInfo("o", /* Localizable */ "One line choice style", new SwitchOptions()
                            {
                                ConflictsWith = ["t", "a", "m"],
                                AcceptsValues = false
                            }),
                            new SwitchInfo("t", /* Localizable */ "Two lines choice style", new SwitchOptions()
                            {
                                ConflictsWith = ["a", "o", "m"],
                                AcceptsValues = false
                            }),
                            new SwitchInfo("m", /* Localizable */ "Modern choice style", new SwitchOptions()
                            {
                                ConflictsWith = ["t", "o", "a"],
                                AcceptsValues = false
                            }),
                            new SwitchInfo("a", /* Localizable */ "Table choice style", new SwitchOptions()
                            {
                                ConflictsWith = ["t", "o", "m"],
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
                        ], true)
                    ], new ChoiceCommand())
            },

            { "chpwd",
                new CommandInfo("chpwd", /* Localizable */ "Changes password for current user",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Username"),
                            new CommandArgumentPart(true, "UserPass"),
                            new CommandArgumentPart(true, "newPass"),
                            new CommandArgumentPart(true, "confirm"),
                        })
                    ], new ChPwdCommand(), CommandFlags.Strict)
            },

            { "chusrname",
                new CommandInfo("chusrname", /* Localizable */ "Changes user name",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "oldUserName", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => UserManagement.ListAllUsers().ToArray()
                            }),
                            new CommandArgumentPart(true, "newUserName"),
                        })
                    ], new ChUsrNameCommand(), CommandFlags.Strict)
            },

            { "cls",
                new CommandInfo("cls", /* Localizable */ "Clears the screen",
                    [
                        new CommandArgumentInfo()
                    ], new ClsCommand())
            },

            { "combinestr",
                new CommandInfo("combinestr", /* Localizable */ "Combines the two text files or more into the console.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "input"),
                            new CommandArgumentPart(true, "input2"),
                            new CommandArgumentPart(false, "input3 ..."),
                        ], true)
                    ], new CombineStrCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "combine",
                new CommandInfo("combine", /* Localizable */ "Combines the two text files or more into the output file.",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "output"),
                            new CommandArgumentPart(true, "input"),
                            new CommandArgumentPart(true, "input2"),
                            new CommandArgumentPart(false, "input3 ..."),
                        })
                    ], new CombineCommand())
            },

            { "convertlineendings",
                new CommandInfo("convertlineendings", /* Localizable */ "Converts the line endings to format for the current platform or to specified custom format",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "textfile"),
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
                    ], new ConvertLineEndingsCommand())
            },

            { "copy",
                new CommandInfo("copy", /* Localizable */ "Creates another copy of a file under different directory or name.",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "source"),
                            new CommandArgumentPart(true, "target"),
                        })
                    ], new CopyCommand())
            },

            { "date",
                new CommandInfo("date", /* Localizable */ "Shows date and time",
                    [
                        new CommandArgumentInfo(new[] {
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
                        }, true)
                    ], new DateCommand(), CommandFlags.RedirectionSupported)
            },

            { "debugshell",
                new CommandInfo("debugshell", /* Localizable */ "Starts the debug shell",
                    [
                        new CommandArgumentInfo()
                    ], new DebugShellCommand(), CommandFlags.Strict)
            },

            { "decodefile",
                new CommandInfo("decodefile", /* Localizable */ "Decodes the encoded file",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames()
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
                        ])
                    ], new DecodeFileCommand())
            },

            { "decodetext",
                new CommandInfo("decodetext", /* Localizable */ "Decodes the encoded text",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "encodedString"),
                            new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames()
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
                        ])
                    ], new DecodeTextCommand())
            },

            { "decodebase64",
                new CommandInfo("decodebase64", /* Localizable */ "Decodes the text from the BASE64 representation",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "encoded")
                        ])
                    ], new DecodeBase64Command())
            },

            { "dirinfo",
                new CommandInfo("dirinfo", /* Localizable */ "Provides information about a directory",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory"),
                        })
                    ], new DirInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "disconndbgdev",
                new CommandInfo("disconndbgdev", /* Localizable */ "Disconnect a debug device",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ip"),
                        })
                    ], new DisconnDbgDevCommand(), CommandFlags.Strict)
            },

            { "diskinfo",
                new CommandInfo("diskinfo", /* Localizable */ "Provides information about a disk",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        ], true)
                    ], new DiskInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "dismissnotif",
                new CommandInfo("dismissnotif", /* Localizable */ "Dismisses a notification",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "notificationNumber", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        })
                    ], new DismissNotifCommand())
            },

            { "dismissnotifs",
                new CommandInfo("dismissnotifs", /* Localizable */ "Dismisses all notifications",
                    [
                        new CommandArgumentInfo()
                    ], new DismissNotifsCommand())
            },

            { "echo",
                new CommandInfo("echo", /* Localizable */ "Writes text into the console",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "text"),
                        ],
                        [
                            new SwitchInfo("noparse", /* Localizable */ "Prints the text as it is with no placeholder parsing", false, false, [], 0, false)
                        ], true)
                    ], new EchoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "edit",
                new CommandInfo("edit", /* Localizable */ "Edits a file",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "file"),
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
                    ], new EditCommand())
            },

            { "encodefile",
                new CommandInfo("encodefile", /* Localizable */ "Encodes the file",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames()
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
                        ])
                    ], new EncodeFileCommand())
            },

            { "encodetext",
                new CommandInfo("encodetext", /* Localizable */ "Encodes the text",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "string"),
                            new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames()
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
                        ])
                    ], new EncodeTextCommand())
            },

            { "encodebase64",
                new CommandInfo("encodebase64", /* Localizable */ "Encodes the text to a BASE64 representation",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "string")
                        ])
                    ], new EncodeBase64Command())
            },

            { "fileinfo",
                new CommandInfo("fileinfo", /* Localizable */ "Provides information about a file",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        })
                    ], new FileInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "find",
                new CommandInfo("find", /* Localizable */ "Finds a file in the specified directory or in the current directory",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(true, "directory"),
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
                    ], new FindCommand())
            },

            { "findreg",
                new CommandInfo("findreg", /* Localizable */ "Finds a file in the specified directory or in the current directory using regular expressions",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "fileRegex"),
                            new CommandArgumentPart(true, "directory"),
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
                    ], new FindRegCommand())
            },

            { "fork",
                new CommandInfo("fork", /* Localizable */ "Forks the UESH shell to create another instance",
                    [
                        new CommandArgumentInfo()
                    ], new ForkCommand())
            },

            { "get",
                new CommandInfo("get", /* Localizable */ "Downloads a file to current working directory",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "url")
                        ],
                        [
                            new SwitchInfo("outputpath", /* Localizable */ "Specifies the output path", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        ])
                    ], new GetCommand())
            },

            { "getaddons",
                new CommandInfo("getaddons", /* Localizable */ "Gets all the addons from the official download page and installs them",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new SwitchInfo("reinstall", /* Localizable */ "Reinstalls the addons", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    ], new GetAddonsCommand())
            },

            { "getallexthandlers",
                new CommandInfo("getallexthandlers", /* Localizable */ "Gets all the extension handlers from all the extensions",
                    [
                        new CommandArgumentInfo()
                    ], new GetAllExtHandlersCommand())
            },

            { "getconfigvalue",
                new CommandInfo("getconfigvalue", /* Localizable */ "Gets a configuration variable and its value",
                    [
                        new CommandArgumentInfo(new CommandArgumentPart[]
                        {
                            new(true, "config", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray()
                            }),
                            new(true, "variable", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (arg) => ConfigTools.GetSettingsKeys(arg[0]).Select((sk) => sk.Variable).ToArray()
                            })
                        }, true)
                    ], new GetConfigValueCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "getdefaultexthandler",
                new CommandInfo("getdefaultexthandler", /* Localizable */ "Gets the default extension handler from the specified extension",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray()
                            }),
                        ], true)
                    ], new GetDefaultExtHandlerCommand())
            },

            { "getdefaultexthandlers",
                new CommandInfo("getdefaultexthandlers", /* Localizable */ "Gets the default extension handlers from all the extensions",
                    [
                        new CommandArgumentInfo()
                    ], new GetDefaultExtHandlersCommand())
            },

            { "getexthandlers",
                new CommandInfo("getexthandlers", /* Localizable */ "Gets the extension handlers from the specified extension",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray()
                            }),
                        ], true)
                    ], new GetExtHandlersCommand())
            },

            { "getkeyiv",
                new CommandInfo("getkeyiv", /* Localizable */ "Gets the key and the initialization vector for symmetrical encoding",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames()
                            }),
                        ], true)
                    ], new GetKeyIvCommand())
            },

            { "host",
                new CommandInfo("host", /* Localizable */ "Gets the current host name",
                    [
                        new CommandArgumentInfo(true)
                    ], new HostCommand())
            },

            { "hwinfo",
                new CommandInfo("hwinfo", /* Localizable */ "Prints hardware information",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "HardwareType", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => hardwareTypes
                            })
                        })
                    ], new HwInfoCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },

            { "if",
                new CommandInfo("if", /* Localizable */ "Executes commands once the UESH expressions are satisfied",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ueshExpression"),
                            new CommandArgumentPart(true, "command"),
                        })
                    ], new IfCommand())
            },

            { "ifm",
                new CommandInfo("ifm", /* Localizable */ "Interactive system host file manager",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "firstPanePath"),
                            new CommandArgumentPart(false, "secondPanePath"),
                        })
                    ], new IfmCommand())
            },

            { "input",
                new CommandInfo("input", /* Localizable */ "Allows user to enter input",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "question"),
                        ], true)
                    ], new InputCommand())
            },

            { "inputpass",
                new CommandInfo("inputpass", /* Localizable */ "Allows user to enter input as password",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "question"),
                        ], true)
                    ], new InputPassCommand())
            },

            { "jsonbeautify",
                new CommandInfo("jsonbeautify", /* Localizable */ "Beautifies the JSON file",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "jsonfile"),
                            new CommandArgumentPart(true, "output"),
                        ], true)
                    ], new JsonBeautifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "jsonminify",
                new CommandInfo("jsonminify", /* Localizable */ "Minifies the JSON file",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "jsonfile"),
                            new CommandArgumentPart(true, "output"),
                        ], true)
                    ], new JsonMinifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "langman",
                new CommandInfo("langman", /* Localizable */ "Manage your languages",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "reload/load/unload"),
                            new CommandArgumentPart(true, "customlanguagename", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => LanguageManager.CustomLanguages.Keys.ToArray()
                            }),
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "list/reloadall"),
                        })
                    ], new LangManCommand(), CommandFlags.Strict)
            },

            { "license",
                new CommandInfo("license", /* Localizable */ "Shows license information for the kernel",
                    [
                        new CommandArgumentInfo()
                    ], new LicenseCommand())
            },

            { "lintscript",
                new CommandInfo("lintscript", /* Localizable */ "Checks a UESH script for syntax errors",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "script"),
                        ], true)
                    ], new LintScriptCommand())
            },

            { "list",
                new CommandInfo("list", /* Localizable */ "List file/folder contents in current folder",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "directory"),
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
                                AcceptsValues = false
                            })
                        ])
                    ], new ListCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lockscreen",
                new CommandInfo("lockscreen", /* Localizable */ "Locks your screen with a password",
                    [
                        new CommandArgumentInfo()
                    ], new LockScreenCommand())
            },

            { "logout",
                new CommandInfo("logout", /* Localizable */ "Logs you out",
                    [
                        new CommandArgumentInfo()
                    ], new LogoutCommand(), CommandFlags.NoMaintenance)
            },

            { "lsconfigs",
                new CommandInfo("lsconfigs", /* Localizable */ "Lists all available configurations",
                    [
                        new CommandArgumentInfo(new SwitchInfo[]
                        {
                            new("deep", /* Localizable */ "Deep details about all configurations, including their entries")
                        })
                    ], new LsConfigsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lsconfigvalues",
                new CommandInfo("lsconfigvalues", /* Localizable */ "Lists all configuration variables and their values",
                    [
                        new CommandArgumentInfo(new CommandArgumentPart[]
                        {
                            new(true, "config", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray()
                            })
                        })
                    ], new LsConfigValuesCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lsconnections",
                new CommandInfo("lsconnections", /* Localizable */ "Lists all available connections",
                    [
                        new CommandArgumentInfo()
                    ], new LsConnectionsCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lsdbgdev",
                new CommandInfo("lsdbgdev", /* Localizable */ "Lists debugging devices connected",
                    [
                        new CommandArgumentInfo()
                    ], new LsDbgDevCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lsexthandlers",
                new CommandInfo("lsexthandlers", /* Localizable */ "Lists available extension handlers",
                    [
                        new CommandArgumentInfo()
                    ], new LsExtHandlersCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lsdiskparts",
                new CommandInfo("lsdiskparts", /* Localizable */ "Lists all the disk partitions",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        ], true)
                    ], new LsDiskPartsCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lsdisks",
                new CommandInfo("lsdisks", /* Localizable */ "Lists all the disks",
                    [
                        new CommandArgumentInfo(true)
                    ], new LsDisksCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lsnet",
                new CommandInfo("lsnet", /* Localizable */ "Lists online network devices",
                    [
                        new CommandArgumentInfo()
                    ], new LsNetCommand(), CommandFlags.Strict)
            },

            { "lsvars",
                new CommandInfo("lsvars", /* Localizable */ "Lists available UESH variables",
                    [
                        new CommandArgumentInfo()
                    ], new LsVarsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "md",
                new CommandInfo("md", /* Localizable */ "Creates a directory",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "directory"),
                        ], true)
                    ], new MdCommand())
            },

            { "mkfile",
                new CommandInfo("mkfile", /* Localizable */ "Makes a new file",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "file"),
                        ], true)
                    ], new MkFileCommand())
            },

            { "modman",
                new CommandInfo("modman", /* Localizable */ "Manage your mods",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "start/stop/info/reload/install/uninstall"),
                            new CommandArgumentPart(true, "modfilename"),
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "list/listparts"),
                            new CommandArgumentPart(true, "modname"),
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "reloadall/stopall/startall"),
                        }),
                    ], new ModManCommand(), CommandFlags.Strict)
            },

            { "modmanual",
                new CommandInfo("modmanual", /* Localizable */ "Mod manual",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "modname"),
                        })
                    ], new ModManualCommand())
            },

            { "move",
                new CommandInfo("move", /* Localizable */ "Moves a file to another directory",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "source"),
                            new CommandArgumentPart(true, "target"),
                        })
                    ], new MoveCommand())
            },

            { "partinfo",
                new CommandInfo("partinfo", /* Localizable */ "Provides information about a partition from the specified disk",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(true, "partNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        ], true)
                    ], new PartInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "pathfind",
                new CommandInfo("pathfind", /* Localizable */ "Finds a given file name from path lookup directories",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "fileName"),
                        ], true)
                    ], new PathFindCommand())
            },

            { "perm",
                new CommandInfo("perm", /* Localizable */ "Manage permissions for users",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "userName"),
                            new CommandArgumentPart(true, "allow/revoke"),
                            new CommandArgumentPart(true, "perm"),
                        })
                    ], new PermCommand(), CommandFlags.Strict)
            },

            { "permgroup",
                new CommandInfo("permgroup", /* Localizable */ "Manage permissions for groups",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "groupName"),
                            new CommandArgumentPart(true, "allow/revoke"),
                            new CommandArgumentPart(true, "perm"),
                        })
                    ], new PermGroupCommand(), CommandFlags.Strict)
            },

            { "ping",
                new CommandInfo("ping", /* Localizable */ "Pings an address",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "Address1"),
                            new CommandArgumentPart(false, "Address2 ..."),
                        ],
                        [
                            new SwitchInfo("times", /* Localizable */ "Specifies number of times to ping", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                                IsNumeric = true
                            })
                        ])
                    ], new PingCommand())
            },

            { "platform",
                new CommandInfo("platform", /* Localizable */ "Gets the current platform",
                    [
                        new CommandArgumentInfo(new[]
                        {
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
                        }, true)
                    ], new PlatformCommand())
            },

            { "put",
                new CommandInfo("put", /* Localizable */ "Uploads a file to specified website",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "FileName"),
                            new CommandArgumentPart(true, "URL"),
                        })
                    ], new PutCommand())
            },

            { "rdebug",
                new CommandInfo("rdebug", /* Localizable */ "Enables or disables remote debugging.",
                    [
                        new CommandArgumentInfo()
                    ], new RdebugCommand(), CommandFlags.Strict)
            },

            { "reboot",
                new CommandInfo("reboot", /* Localizable */ "Restarts your computer (WARNING: No syncing, because it is not a final kernel)",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "ip/safe/maintenance/debug"),
                            new CommandArgumentPart(false, "port", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        })
                    ], new RebootCommand())
            },

            { "reloadconfig",
                new CommandInfo("reloadconfig", /* Localizable */ "Reloads configuration file that is edited.",
                    [
                        new CommandArgumentInfo()
                    ], new ReloadConfigCommand(), CommandFlags.Strict)
            },

            { "rexec",
                new CommandInfo("rexec", /* Localizable */ "Remotely executes a command to remote PC",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "address"),
                            new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(false, "command"),
                        })
                    ], new RexecCommand(), CommandFlags.Strict)
            },

            { "rm",
                new CommandInfo("rm", /* Localizable */ "Removes a directory or a file",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "target"),
                        })
                    ], new RmCommand())
            },

            { "rmsec",
                new CommandInfo("rmsec", /* Localizable */ "Removes a file or a directory securely",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "target"),
                        })
                    ], new RmSecCommand())
            },

            { "rmuser",
                new CommandInfo("rmuser", /* Localizable */ "Removes a user from the list",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Username"),
                        })
                    ], new RmUserCommand(), CommandFlags.Strict)
            },

            { "rmgroup",
                new CommandInfo("rmgroup", /* Localizable */ "Removes a group from the list",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "GroupName"),
                        })
                    ], new RmGroupCommand(), CommandFlags.Strict)
            },

            { "rmuserfromgroup",
                new CommandInfo("rmuserfromgroup", /* Localizable */ "Removes a user from the group",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "UserName"),
                            new CommandArgumentPart(true, "GroupName"),
                        })
                    ], new RmUserFromGroupCommand(), CommandFlags.Strict)
            },

            { "saveconfig",
                new CommandInfo("saveconfig", /* Localizable */ "Saves the current kernel configuration to its file",
                    [
                        new CommandArgumentInfo()
                    ], new SaveConfigCommand(), CommandFlags.Strict)
            },

            { "savescreen",
                new CommandInfo("savescreen", /* Localizable */ "Saves your screen from burn outs",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "saver"),
                        ],
                        [
                            new SwitchInfo("select", /* Localizable */ "Gives you an option to select the screensaver to try out", new()
                            {
                                AcceptsValues = false
                            })
                        ])
                    ], new SaveScreenCommand())
            },

            { "search",
                new CommandInfo("search", /* Localizable */ "Searches for specified string in the provided file using regular expressions",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Regexp"),
                            new CommandArgumentPart(true, "File"),
                        })
                    ], new SearchCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "searchword",
                new CommandInfo("searchword", /* Localizable */ "Searches for specified string in the provided file",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "StringEnclosedInDoubleQuotes"),
                            new CommandArgumentPart(true, "File"),
                        })
                    ], new SearchWordCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "select",
                new CommandInfo("select", /* Localizable */ "Provides a selection choice",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "answers"),
                            new CommandArgumentPart(true, "input"),
                            new CommandArgumentPart(false, "answertitle1"),
                            new CommandArgumentPart(false, "answertitle2 ..."),
                        ], true)
                    ], new SelectCommand())
            },

            { "setsaver",
                new CommandInfo("setsaver", /* Localizable */ "Sets up kernel screensavers",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "saver"),
                        })
                    ], new SetSaverCommand(), CommandFlags.Strict)
            },

            { "settings",
                new CommandInfo("settings", /* Localizable */ "Changes kernel configuration",
                    [
                        new CommandArgumentInfo(new[] {
                            new SwitchInfo("saver", /* Localizable */ "Opens the screensaver settings", new SwitchOptions()
                            {
                                ConflictsWith = ["splash", "type", "addonsaver"],
                                AcceptsValues = false
                            }),
                            new SwitchInfo("addonsaver", /* Localizable */ "Opens the addon screensaver settings", new SwitchOptions()
                            {
                                ConflictsWith = ["splash", "type", "saver"],
                                AcceptsValues = false
                            }),
                            new SwitchInfo("splash", /* Localizable */ "Opens the splash settings", new SwitchOptions()
                            {
                                ConflictsWith = ["saver", "type", "addonsaver"],
                                AcceptsValues = false
                            }),
                            new SwitchInfo("type", /* Localizable */ "Opens the custom settings", new SwitchOptions()
                            {
                                ConflictsWith = ["saver", "splash", "addonsaver"],
                                ArgumentsRequired = true
                            })
                        })
                    ], new SettingsCommand(), CommandFlags.Strict)
            },

            { "set",
                new CommandInfo("set", /* Localizable */ "Sets a variable to a value in a script",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "value"),
                        ], true)
                    ], new SetCommand())
            },

            { "setconfigvalue",
                new CommandInfo("setconfigvalue", /* Localizable */ "Sets a configuration variable to a specified value",
                    [
                        new CommandArgumentInfo(new CommandArgumentPart[]
                        {
                            new(true, "config", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => Config.GetKernelConfigs().Select((bkc) => bkc.GetType().Name).ToArray()
                            }),
                            new(true, "variable", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (arg) => ConfigTools.GetSettingsKeys(arg[0]).Select((sk) => sk.Variable).ToArray()
                            }),
                            new(true, "value")
                        })
                    ], new SetConfigValueCommand())
            },

            { "setexthandler",
                new CommandInfo("setexthandler", /* Localizable */ "Sets the default extension handler of the specified extension to the specific implementer",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray()
                            }),
                            new CommandArgumentPart(true, "implementer", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (args) => ExtensionHandlerTools.GetExtensionHandlers(args[0]).Select((h) => h.Implementer).ToArray()
                            }),
                        })
                    ], new SetExtHandlerCommand())
            },

            { "setrange",
                new CommandInfo("setrange", /* Localizable */ "Creates a variable array with the provided values",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "value"),
                            new CommandArgumentPart(false, "value2"),
                            new CommandArgumentPart(false, "value3 ..."),
                        ], true)
                    ], new SetRangeCommand())
            },

            { "shownotifs",
                new CommandInfo("shownotifs", /* Localizable */ "Shows all received notifications",
                    [
                        new CommandArgumentInfo()
                    ], new ShowNotifsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "showtd",
                new CommandInfo("showtd", /* Localizable */ "Shows date and time",
                    [
                        new CommandArgumentInfo()
                    ], new ShowTdCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "showtdzone",
                new CommandInfo("showtdzone", /* Localizable */ "Shows date and time in zones",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "timezone"),
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
                    ], new ShowTdZoneCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "shutdown",
                new CommandInfo("shutdown", /* Localizable */ "The kernel will be shut down",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "ip"),
                            new CommandArgumentPart(false, "port", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        })
                    ], new ShutdownCommand())
            },

            { "sleep",
                new CommandInfo("sleep", /* Localizable */ "Sleeps for specified milliseconds",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ms"),
                        })
                    ], new SleepCommand())
            },

            { "sudo",
                new CommandInfo("sudo", /* Localizable */ "Runs the command as the root user",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "command"),
                        })
                    ], new SudoCommand())
            },

            { "sumfile",
                new CommandInfo("sumfile", /* Localizable */ "Calculates file sums.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames()
                            }),
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(false, "outputFile"),
                        ],
                        [
                            new SwitchInfo("relative", /* Localizable */ "Uses relative path instead of absolute", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        ])
                    ], new SumFileCommand())
            },

            { "sumfiles",
                new CommandInfo("sumfiles", /* Localizable */ "Calculates sums of files in specified directory.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames()
                            }),
                            new CommandArgumentPart(true, "dir"),
                            new CommandArgumentPart(false, "outputFile"),
                        ],
                        [
                            new SwitchInfo("relative", /* Localizable */ "Uses relative path instead of absolute", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        ])
                    ], new SumFilesCommand())
            },

            { "symlink",
                new CommandInfo("symlink", /* Localizable */ "Creates a symbolic link to a file or a folder",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "linkname"),
                            new CommandArgumentPart(true, "target"),
                        })
                    ], new SymlinkCommand())
            },

            { "taskman",
                new CommandInfo("taskman", /* Localizable */ "Task manager",
                    [
                        new CommandArgumentInfo()
                    ], new TaskManCommand())
            },

            { "themeprev",
                new CommandInfo("themeprev", /* Localizable */ "Previews a theme",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "theme"),
                        })
                    ], new ThemePrevCommand())
            },

            { "themeset",
                new CommandInfo("themeset", /* Localizable */ "Selects a theme and sets it",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(false, "theme"),
                        ],
                        [
                            new SwitchInfo("y", /* Localizable */ "Immediately set the theme on selection")
                        ])
                    ], new ThemeSetCommand())
            },

            { "unblockdbgdev",
                new CommandInfo("unblockdbgdev", /* Localizable */ "Unblock a debug device by IP address",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ipaddress"),
                        })
                    ], new UnblockDbgDevCommand(), CommandFlags.Strict)
            },

            { "unset",
                new CommandInfo("unset", /* Localizable */ "Removes a variable from the UESH variable list",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "$variable"),
                        ],
                        [
                            new SwitchInfo("justwipe", /* Localizable */ "Just wipes the variable value without removing it", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        ])
                    ], new UnsetCommand())
            },

            { "unzip",
                new CommandInfo("unzip", /* Localizable */ "Extracts a ZIP archive",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "zipfile"),
                            new CommandArgumentPart(false, "path"),
                        ],
                        [
                            new SwitchInfo("createdir", /* Localizable */ "Creates a directory that contains the contents of the ZIP file", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        ])
                    ], new UnZipCommand())
            },

            { "update",
                new CommandInfo("update", /* Localizable */ "System update",
                    [
                        new CommandArgumentInfo()
                    ], new UpdateCommand(), CommandFlags.Strict)
            },

            { "uptime",
                new CommandInfo("uptime", /* Localizable */ "Shows the kernel uptime",
                    [
                        new CommandArgumentInfo(true)
                    ], new UptimeCommand())
            },

            { "usermanual",
                new CommandInfo("usermanual", /* Localizable */ "Shows the two useful URLs for manual.",
                    [
                        new CommandArgumentInfo()
                    ], new UserManualCommand())
            },

            { "verify",
                new CommandInfo("verify", /* Localizable */ "Verifies sanity of the file",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames()
                            }),
                            new CommandArgumentPart(true, "calculatedhash"),
                            new CommandArgumentPart(true, "hashfile/expectedhash"),
                            new CommandArgumentPart(true, "file"),
                        })
                    ], new VerifyCommand())
            },

            { "version",
                new CommandInfo("version", /* Localizable */ "Gets the current version",
                    [
                        new CommandArgumentInfo(new[]
                        {
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
                        }, true)
                    ], new VersionCommand())
            },

            { "whoami",
                new CommandInfo("whoami", /* Localizable */ "Gets the current user name",
                    [
                        new CommandArgumentInfo(true)
                    ], new WhoamiCommand())
            },

            { "winelevate",
                new CommandInfo("winelevate", /* Localizable */ "Restarts Nitrocid with the elevated permissions (Windows only)",
                    [
                        new CommandArgumentInfo(true)
                    ], new WinElevateCommand())
            },

            { "wraptext",
                new CommandInfo("wraptext", /* Localizable */ "Wraps the text file",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "file"),
                        ],
                        [
                            new SwitchInfo("columns", /* Localizable */ "Specifies the columns per line", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                                IsNumeric = true
                            })
                        ], true)
                    ], new WrapTextCommand())
            },

            { "zip",
                new CommandInfo("zip", /* Localizable */ "Creates a ZIP archive",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "zipfile"),
                            new CommandArgumentPart(true, "path"),
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
                    ], new ZipCommand())
            },
        };

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

        public override BaseShell ShellBase => new UESHShell();

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

    }
}
