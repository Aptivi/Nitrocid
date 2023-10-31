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

namespace KS.Shell.Shells.UESH
{
    /// <summary>
    /// UESH common shell properties
    /// </summary>
    internal class UESHShellInfo : BaseShellInfo, IShellInfo
    {
        /// <summary>
        /// List of commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "addgroup",
                new CommandInfo("addgroup", /* Localizable */ "Adds groups",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "groupName")
                        })
                    }, new AddGroupCommand(), CommandFlags.Strict)
            },
            
            { "adduser",
                new CommandInfo("adduser", /* Localizable */ "Adds users",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "username"),
                            new CommandArgumentPart(false, "password"),
                            new CommandArgumentPart(false, "confirm"),
                        })
                    }, new AddUserCommand(), CommandFlags.Strict)
            },
            
            { "addusertogroup",
                new CommandInfo("addusertogroup", /* Localizable */ "Adds users to a group",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "username"),
                            new CommandArgumentPart(true, "group"),
                        })
                    }, new AddUserToGroupCommand(), CommandFlags.Strict)
            },
            
            { "admin",
                new CommandInfo("admin", /* Localizable */ "Administrative shell",
                    new[] {
                        new CommandArgumentInfo()
                    }, new AdminCommand(), CommandFlags.Strict)
            },
            
            { "alias",
                new CommandInfo("alias", /* Localizable */ "Adds aliases to commands",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "rem/add"),
                            new CommandArgumentPart(true, $"{string.Join("/", Enum.GetNames(typeof(ShellType)))}"),
                            new CommandArgumentPart(true, "alias"),
                            new CommandArgumentPart(false, "cmd"),
                        })
                    }, new AliasCommand(), CommandFlags.Strict)
            },
            
            { "beep",
                new CommandInfo("beep", /* Localizable */ "Beeps from the console",
                    new[] {
                        new CommandArgumentInfo()
                    }, new BeepCommand())
            },
            
            { "blockdbgdev",
                new CommandInfo("blockdbgdev", /* Localizable */ "Block a debug device by IP address",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ipaddress"),
                        })
                    }, new BlockDbgDevCommand(), CommandFlags.Strict)
            },
            
            { "bulkrename",
                new CommandInfo("bulkrename", /* Localizable */ "Renames group of files to selected format",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "targetdir"),
                            new CommandArgumentPart(true, "pattern"),
                            new CommandArgumentPart(false, "newname"),
                        })
                    }, new BulkRenameCommand())
            },
            
            { "cat",
                new CommandInfo("cat", /* Localizable */ "Prints content of file to console",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        }, new[] {
                            new SwitchInfo("lines", /* Localizable */ "Prints the line numbers alongside the contents", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "nolines" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("nolines", /* Localizable */ "Prints only the contents", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "lines" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("plain", /* Localizable */ "Force treating binary files as plain text", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    }, new CatCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "cdir",
                new CommandInfo("cdir", /* Localizable */ "Gets the current directory",
                    new[] {
                        new CommandArgumentInfo(true)
                    }, new CDirCommand())
            },

            { "chattr",
                new CommandInfo("chattr", /* Localizable */ "Changes attribute of a file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(true, "add/rem"),
                            new CommandArgumentPart(true, "Normal/ReadOnly/Hidden/Archive"),
                        })
                    }, new ChAttrCommand())
            },
            
            { "chdir",
                new CommandInfo("chdir", /* Localizable */ "Changes directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory/.."),
                        })
                    }, new ChDirCommand())
            },
            
            { "chhostname",
                new CommandInfo("chhostname", /* Localizable */ "Changes host name",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "hostname"),
                        })
                    }, new ChHostNameCommand(), CommandFlags.Strict)
            },
            
            { "chklock",
                new CommandInfo("chklock", /* Localizable */ "Checks the file or the folder lock",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        }, new[] {
                            new SwitchInfo("waitforunlock", /* Localizable */ "Waits until the file or the folder is unlocked", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    }, new ChkLockCommand())
            },
            
            { "chlang",
                new CommandInfo("chlang", /* Localizable */ "Changes language",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "language"),
                        }, new[]
                        {
                            new SwitchInfo("usesyslang", /* Localizable */ "Uses the system language settings to try to infer the language from", new SwitchOptions()
                            {
                                OptionalizeLastRequiredArguments = 1,
                                AcceptsValues = false
                            }),
                            new SwitchInfo("user", /* Localizable */ "Changes the user language instead of the system language", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                        })
                    }, new ChLangCommand(), CommandFlags.Strict)
            },
            
            { "chmal",
                new CommandInfo("chmal", /* Localizable */ "Changes MAL, the MOTD After Login",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "message"),
                        })
                    }, new ChMalCommand(), CommandFlags.Strict)
            },
            
            { "chmotd",
                new CommandInfo("chmotd", /* Localizable */ "Changes MOTD, the Message Of The Day",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "message"),
                        })
                    }, new ChMotdCommand(), CommandFlags.Strict)
            },
            
            { "choice",
                new CommandInfo("choice", /* Localizable */ "Makes user choices",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "answers"),
                            new CommandArgumentPart(true, "input"),
                            new CommandArgumentPart(false, "answertitle1"),
                            new CommandArgumentPart(false, "answertitle2 ..."),
                        }, new[] {
                            new SwitchInfo("o", /* Localizable */ "One line choice style", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "t", "a", "m" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("t", /* Localizable */ "Two lines choice style", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "a", "o", "m" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("m", /* Localizable */ "Modern choice style", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "t", "o", "a" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("a", /* Localizable */ "Table choice style", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "t", "o", "m" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("single", /* Localizable */ "The output can be only one character", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "multiple" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("multiple", /* Localizable */ "The output can be more than a character", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "single" },
                                AcceptsValues = false
                            })
                        }, true)
                    }, new ChoiceCommand())
            },
            
            { "chpwd",
                new CommandInfo("chpwd", /* Localizable */ "Changes password for current user",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Username"),
                            new CommandArgumentPart(true, "UserPass"),
                            new CommandArgumentPart(true, "newPass"),
                            new CommandArgumentPart(true, "confirm"),
                        })
                    }, new ChPwdCommand(), CommandFlags.Strict)
            },
            
            { "chusrname",
                new CommandInfo("chusrname", /* Localizable */ "Changes user name",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "oldUserName", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => UserManagement.ListAllUsers().ToArray()
                            }),
                            new CommandArgumentPart(true, "newUserName"),
                        })
                    }, new ChUsrNameCommand(), CommandFlags.Strict)
            },
            
            { "cls",
                new CommandInfo("cls", /* Localizable */ "Clears the screen",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ClsCommand())
            },
            
            { "combinestr",
                new CommandInfo("combinestr", /* Localizable */ "Combines the two text files or more into the console.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "input"),
                            new CommandArgumentPart(true, "input2"),
                            new CommandArgumentPart(false, "input3 ..."),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new CombineStrCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "combine",
                new CommandInfo("combine", /* Localizable */ "Combines the two text files or more into the output file.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "output"),
                            new CommandArgumentPart(true, "input"),
                            new CommandArgumentPart(true, "input2"),
                            new CommandArgumentPart(false, "input3 ..."),
                        })
                    }, new CombineCommand())
            },
            
            { "convertlineendings",
                new CommandInfo("convertlineendings", /* Localizable */ "Converts the line endings to format for the current platform or to specified custom format",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "textfile"),
                        }, new[] {
                            new SwitchInfo("w", /* Localizable */ "Converts the line endings to the Windows format", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "u", "m" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("u", /* Localizable */ "Converts the line endings to the Unix format", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "m", "w" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("m", /* Localizable */ "Converts the line endings to the Mac OS 9 format", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "u", "w" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("force", /* Localizable */ "Forces the line ending conversion", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                        })
                    }, new ConvertLineEndingsCommand())
            },
            
            { "copy",
                new CommandInfo("copy", /* Localizable */ "Creates another copy of a file under different directory or name.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "source"),
                            new CommandArgumentPart(true, "target"),
                        })
                    }, new CopyCommand())
            },
            
            { "date",
                new CommandInfo("date", /* Localizable */ "Shows date and time",
                    new[] {
                        new CommandArgumentInfo(new[] {
                            new SwitchInfo("date", /* Localizable */ "Shows just the date", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "time", "full" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("time", /* Localizable */ "Shows just the time", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "date", "full" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("full", /* Localizable */ "Shows date and time", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "date", "time" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("utc", /* Localizable */ "Uses UTC instead of local", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        }, true)
                    }, new DateCommand(), CommandFlags.RedirectionSupported)
            },
            
            { "debugshell",
                new CommandInfo("debugshell", /* Localizable */ "Starts the debug shell",
                    new[] {
                        new CommandArgumentInfo()
                    }, new DebugShellCommand(), CommandFlags.Strict)
            },
            
            { "decodefile",
                new CommandInfo("decodefile", /* Localizable */ "Decodes the encoded file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames()
                            }),
                        }, new[]
                        {
                            new SwitchInfo("key", /* Localizable */ "Specifies the key", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                            }),
                            new SwitchInfo("iv", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                            }),
                        })
                    }, new DecodeFileCommand())
            },
            
            { "decodetext",
                new CommandInfo("decodetext", /* Localizable */ "Decodes the encoded text",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "encodedString"),
                            new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames()
                            }),
                        }, new[]
                        {
                            new SwitchInfo("key", /* Localizable */ "Specifies the key", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                            }),
                            new SwitchInfo("iv", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                            }),
                        })
                    }, new DecodeTextCommand())
            },
            
            { "dirinfo",
                new CommandInfo("dirinfo", /* Localizable */ "Provides information about a directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory"),
                        })
                    }, new DirInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "disconndbgdev",
                new CommandInfo("disconndbgdev", /* Localizable */ "Disconnect a debug device",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ip"),
                        })
                    }, new DisconnDbgDevCommand(), CommandFlags.Strict)
            },

            { "diskinfo",
                new CommandInfo("diskinfo", /* Localizable */ "Provides information about a disk",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new DiskInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "dismissnotif",
                new CommandInfo("dismissnotif", /* Localizable */ "Dismisses a notification",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "notificationNumber", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        })
                    }, new DismissNotifCommand())
            },
            
            { "dismissnotifs",
                new CommandInfo("dismissnotifs", /* Localizable */ "Dismisses all notifications",
                    new[] {
                        new CommandArgumentInfo()
                    }, new DismissNotifsCommand())
            },
            
            { "echo",
                new CommandInfo("echo", /* Localizable */ "Writes text into the console",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "text"),
                        }, new[]
                        {
                            new SwitchInfo("noparse", /* Localizable */ "Prints the text as it is with no placeholder parsing", false, false, Array.Empty<string>(), 0, false)
                        }, true)
                    }, new EchoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "edit",
                new CommandInfo("edit", /* Localizable */ "Edits a file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        }, new[] {
                            new SwitchInfo("text", /* Localizable */ "Forces text mode", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "sql", "json", "hex" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("hex", /* Localizable */ "Forces hex mode", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "text", "json", "sql" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("json", /* Localizable */ "Forces JSON mode", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "text", "sql", "hex" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("sql", /* Localizable */ "Forces SQL mode", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "text", "json", "hex" },
                                AcceptsValues = false
                            }),
                        })
                    }, new EditCommand())
            },

            { "encodefile",
                new CommandInfo("encodefile", /* Localizable */ "Encodes the file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames()
                            }),
                        }, new[]
                        {
                            new SwitchInfo("key", /* Localizable */ "Specifies the key", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                            }),
                            new SwitchInfo("iv", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                            }),
                        })
                    }, new EncodeFileCommand())
            },

            { "encodetext",
                new CommandInfo("encodetext", /* Localizable */ "Encodes the text",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "string"),
                            new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames()
                            }),
                        }, new[]
                        {
                            new SwitchInfo("key", /* Localizable */ "Specifies the key", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                            }),
                            new SwitchInfo("iv", /* Localizable */ "Specifies the initialization vector", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                            }),
                        })
                    }, new EncodeTextCommand())
            },

            { "fileinfo",
                new CommandInfo("fileinfo", /* Localizable */ "Provides information about a file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        })
                    }, new FileInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "find",
                new CommandInfo("find", /* Localizable */ "Finds a file in the specified directory or in the current directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(true, "directory"),
                        }, new[] {
                            new SwitchInfo("recursive", /* Localizable */ "Searches for a file recursively", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                            new SwitchInfo("exec", /* Localizable */ "Executes a command on a file", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        }, true)
                    }, new FindCommand())
            },
            
            { "findreg",
                new CommandInfo("findreg", /* Localizable */ "Finds a file in the specified directory or in the current directory using regular expressions",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "fileRegex"),
                            new CommandArgumentPart(true, "directory"),
                        }, new[] {
                            new SwitchInfo("recursive", /* Localizable */ "Searches for a file recursively", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                            new SwitchInfo("exec", /* Localizable */ "Executes a command on a file", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        }, true)
                    }, new FindRegCommand())
            },
            
            { "fork",
                new CommandInfo("fork", /* Localizable */ "Forks the UESH shell to create another instance",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ForkCommand())
            },
            
            { "ftp",
                new CommandInfo("ftp", /* Localizable */ "Use an FTP shell to interact with servers",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "server"),
                        })
                    }, new FtpCommand())
            },
            
            { "get",
                new CommandInfo("get", /* Localizable */ "Downloads a file to current working directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "url")
                        }, new[]
                        {
                            new SwitchInfo("outputpath", /* Localizable */ "Specifies the output path", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            })
                        })
                    }, new GetCommand())
            },

            { "getallexthandlers",
                new CommandInfo("getallexthandlers", /* Localizable */ "Gets all the extension handlers from all the extensions",
                    new[] {
                        new CommandArgumentInfo()
                    }, new GetAllExtHandlersCommand())
            },

            { "getdefaultexthandler",
                new CommandInfo("getdefaultexthandler", /* Localizable */ "Gets the default extension handler from the specified extension",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray()
                            }),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new GetDefaultExtHandlerCommand())
            },

            { "getdefaultexthandlers",
                new CommandInfo("getdefaultexthandlers", /* Localizable */ "Gets the default extension handlers from all the extensions",
                    new[] {
                        new CommandArgumentInfo()
                    }, new GetDefaultExtHandlersCommand())
            },

            { "getexthandlers",
                new CommandInfo("getexthandlers", /* Localizable */ "Gets the extension handlers from the specified extension",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "extension", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => ExtensionHandlerTools.GetExtensionHandlers().Select((h) => h.Extension).ToArray()
                            }),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new GetExtHandlersCommand())
            },

            { "getkeyiv",
                new CommandInfo("getkeyiv", /* Localizable */ "Gets the key and the initialization vector for symmetrical encoding",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "algorithm", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncodingDriverTools.GetEncodingDriverNames()
                            }),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new GetKeyIvCommand())
            },

            { "host",
                new CommandInfo("host", /* Localizable */ "Gets the current host name",
                    new[] {
                        new CommandArgumentInfo(true)
                    }, new HostCommand())
            },
            
            { "http",
                new CommandInfo("http", /* Localizable */ "Starts the HTTP shell",
                    new[] {
                        new CommandArgumentInfo()
                    }, new HttpCommand())
            },
            
            { "hwinfo",
                new CommandInfo("hwinfo", /* Localizable */ "Prints hardware information",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "HardwareType", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => new[] { "HDD", "CPU", "GPU", "RAM", "all" }
                            })
                        })
                    }, new HwInfoCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },
            
            { "if",
                new CommandInfo("if", /* Localizable */ "Executes commands once the UESH expressions are satisfied",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ueshExpression"),
                            new CommandArgumentPart(true, "command"),
                        })
                    }, new IfCommand())
            },
            
            { "ifm",
                new CommandInfo("ifm", /* Localizable */ "Interactive system host file manager",
                    new[] {
                        new CommandArgumentInfo()
                    }, new IfmCommand())
            },
            
            { "input",
                new CommandInfo("input", /* Localizable */ "Allows user to enter input",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "question"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new InputCommand())
            },
            
            { "jsonbeautify",
                new CommandInfo("jsonbeautify", /* Localizable */ "Beautifies the JSON file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "jsonfile"),
                            new CommandArgumentPart(true, "output"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new JsonBeautifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "jsonminify",
                new CommandInfo("jsonminify", /* Localizable */ "Minifies the JSON file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "jsonfile"),
                            new CommandArgumentPart(true, "output"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new JsonMinifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "langman",
                new CommandInfo("langman", /* Localizable */ "Manage your languages",
                    new[] {
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
                    }, new LangManCommand(), CommandFlags.Strict)
            },
            
            { "license",
                new CommandInfo("license", /* Localizable */ "Shows license information for the kernel",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LicenseCommand())
            },
            
            { "lintscript",
                new CommandInfo("lintscript", /* Localizable */ "Checks a UESH script for syntax errors",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "script"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new LintScriptCommand())
            },
            
            { "list",
                new CommandInfo("list", /* Localizable */ "List file/folder contents in current folder",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "directory"),
                        }, new[] {
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
                        })
                    }, new ListCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "lockscreen",
                new CommandInfo("lockscreen", /* Localizable */ "Locks your screen with a password",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LockScreenCommand())
            },
            
            { "logout",
                new CommandInfo("logout", /* Localizable */ "Logs you out",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LogoutCommand(), CommandFlags.NoMaintenance)
            },

            { "lsconnections",
                new CommandInfo("lsconnections", /* Localizable */ "Lists all available connections",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsConnectionsCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lsdbgdev",
                new CommandInfo("lsdbgdev", /* Localizable */ "Lists debugging devices connected",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsDbgDevCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lsexthandlers",
                new CommandInfo("lsexthandlers", /* Localizable */ "Lists available extension handlers",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsExtHandlersCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
           
            { "lsdiskparts",
                new CommandInfo("lsdiskparts", /* Localizable */ "Lists all the disk partitions",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new LsDiskPartsCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
           
            { "lsdisks",
                new CommandInfo("lsdisks", /* Localizable */ "Lists all the disks",
                    new[] {
                        new CommandArgumentInfo(true)
                    }, new LsDisksCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "lsnet",
                new CommandInfo("lsnet", /* Localizable */ "Lists online network devices",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsNetCommand(), CommandFlags.Strict)
            },
            
            { "lsvars",
                new CommandInfo("lsvars", /* Localizable */ "Lists available UESH variables",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsVarsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "mail",
                new CommandInfo("mail", /* Localizable */ "Opens the mail client",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "emailAddress"),
                        })
                    }, new MailCommand())
            },
            
            { "md",
                new CommandInfo("md", /* Localizable */ "Creates a directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new MdCommand())
            },
            
            { "mkfile",
                new CommandInfo("mkfile", /* Localizable */ "Makes a new file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new MkFileCommand())
            },
            
            { "modman",
                new CommandInfo("modman", /* Localizable */ "Manage your mods",
                    new[] {
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
                    }, new ModManCommand(), CommandFlags.Strict)
            },
            
            { "modmanual",
                new CommandInfo("modmanual", /* Localizable */ "Mod manual",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "modname"),
                        })
                    }, new ModManualCommand())
            },
            
            { "move",
                new CommandInfo("move", /* Localizable */ "Moves a file to another directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "source"),
                            new CommandArgumentPart(true, "target"),
                        })
                    }, new MoveCommand())
            },

            { "partinfo",
                new CommandInfo("partinfo", /* Localizable */ "Provides information about a partition from the specified disk",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "diskNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(true, "partNum", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new PartInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "pathfind",
                new CommandInfo("pathfind", /* Localizable */ "Finds a given file name from path lookup directories",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "fileName"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new PathFindCommand())
            },
            
            { "perm",
                new CommandInfo("perm", /* Localizable */ "Manage permissions for users",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "userName"),
                            new CommandArgumentPart(true, "allow/revoke"),
                            new CommandArgumentPart(true, "perm"),
                        })
                    }, new PermCommand(), CommandFlags.Strict)
            },
            
            { "permgroup",
                new CommandInfo("permgroup", /* Localizable */ "Manage permissions for groups",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "groupName"),
                            new CommandArgumentPart(true, "allow/revoke"),
                            new CommandArgumentPart(true, "perm"),
                        })
                    }, new PermGroupCommand(), CommandFlags.Strict)
            },
            
            { "ping",
                new CommandInfo("ping", /* Localizable */ "Pings an address",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Address1"),
                            new CommandArgumentPart(false, "Address2 ..."),
                        }, new[] {
                            new SwitchInfo("times", /* Localizable */ "Specifies number of times to ping", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                                IsNumeric = true
                            })
                        })
                    }, new PingCommand())
            },

            { "platform",
                new CommandInfo("platform", /* Localizable */ "Gets the current platform",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new SwitchInfo("n", /* Localizable */ "Shows the platform name", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "r", "v", "b", "c" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("v", /* Localizable */ "Shows the platform version", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "n", "r", "b", "c" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("b", /* Localizable */ "Shows the platform bits", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "n", "v", "r", "c" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("c", /* Localizable */ "Shows the .NET platform version", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "n", "v", "b", "r" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("r", /* Localizable */ "Shows the .NET platform runtime identifier", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "n", "v", "b", "c" },
                                AcceptsValues = false
                            })
                        }, true)
                    }, new PlatformCommand())
            },
            
            { "previewsplash",
                new CommandInfo("previewsplash", /* Localizable */ "Previews the splash",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "splashName"),
                        }, new[]
                        {
                            new SwitchInfo("splashout", /* Localizable */ "Specifies whether to test out the important messages feature on splash", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                            new SwitchInfo("context", /* Localizable */ "Specifies the splash screen context", new SwitchOptions()
                            {
                                ArgumentsRequired = true
                            }),
                        })
                    }, new PreviewSplashCommand())
            },
            
            { "put",
                new CommandInfo("put", /* Localizable */ "Uploads a file to specified website",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "FileName"),
                            new CommandArgumentPart(true, "URL"),
                        })
                    }, new PutCommand())
            },

            { "rdebug",
                new CommandInfo("rdebug", /* Localizable */ "Enables or disables remote debugging.",
                    new[] {
                        new CommandArgumentInfo()
                    }, new RdebugCommand(), CommandFlags.Strict)
            },

            { "reboot",
                new CommandInfo("reboot", /* Localizable */ "Restarts your computer (WARNING: No syncing, because it is not a final kernel)",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "ip"),
                            new CommandArgumentPart(false, "port", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        })
                    }, new RebootCommand())
            },
            
            { "reloadconfig",
                new CommandInfo("reloadconfig", /* Localizable */ "Reloads configuration file that is edited.",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ReloadConfigCommand(), CommandFlags.Strict)
            },
            
            { "rexec",
                new CommandInfo("rexec", /* Localizable */ "Remotely executes a command to remote PC",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "address"),
                            new CommandArgumentPart(true, "port", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(false, "command"),
                        })
                    }, new RexecCommand(), CommandFlags.Strict)
            },
            
            { "rm",
                new CommandInfo("rm", /* Localizable */ "Removes a directory or a file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "target"),
                        })
                    }, new RmCommand())
            },
            
            { "rmsec",
                new CommandInfo("rmsec", /* Localizable */ "Removes a file or a directory securely",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "target"),
                        })
                    }, new RmSecCommand())
            },
            
            { "rmuser",
                new CommandInfo("rmuser", /* Localizable */ "Removes a user from the list",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Username"),
                        })
                    }, new RmUserCommand(), CommandFlags.Strict)
            },
            
            { "rmgroup",
                new CommandInfo("rmgroup", /* Localizable */ "Removes a group from the list",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "GroupName"),
                        })
                    }, new RmGroupCommand(), CommandFlags.Strict)
            },
            
            { "rmuserfromgroup",
                new CommandInfo("rmuserfromgroup", /* Localizable */ "Removes a user from the group",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "UserName"),
                            new CommandArgumentPart(true, "GroupName"),
                        })
                    }, new RmUserFromGroupCommand(), CommandFlags.Strict)
            },
            
            { "rss",
                new CommandInfo("rss", /* Localizable */ "Opens an RSS shell to read the feeds",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "feedlink"),
                        })
                    }, new RssCommand())
            },
            
            { "saveconfig",
                new CommandInfo("saveconfig", /* Localizable */ "Saves the current kernel configuration to its file",
                    new[] {
                        new CommandArgumentInfo()
                    }, new SaveConfigCommand(), CommandFlags.Strict)
            },
            
            { "savescreen",
                new CommandInfo("savescreen", /* Localizable */ "Saves your screen from burn outs",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "saver"),
                        })
                    }, new SaveScreenCommand())
            },
            
            { "search",
                new CommandInfo("search", /* Localizable */ "Searches for specified string in the provided file using regular expressions",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Regexp"),
                            new CommandArgumentPart(true, "File"),
                        })
                    }, new SearchCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "searchword",
                new CommandInfo("searchword", /* Localizable */ "Searches for specified string in the provided file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "StringEnclosedInDoubleQuotes"),
                            new CommandArgumentPart(true, "File"),
                        })
                    }, new SearchWordCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "select",
                new CommandInfo("select", /* Localizable */ "Provides a selection choice",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "answers"),
                            new CommandArgumentPart(true, "input"),
                            new CommandArgumentPart(false, "answertitle1"),
                            new CommandArgumentPart(false, "answertitle2 ..."),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new SelectCommand())
            },
            
            { "setsaver",
                new CommandInfo("setsaver", /* Localizable */ "Sets up kernel screensavers",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "saver"),
                        })
                    }, new SetSaverCommand(), CommandFlags.Strict)
            },
            
            { "settings",
                new CommandInfo("settings", /* Localizable */ "Changes kernel configuration",
                    new[] {
                        new CommandArgumentInfo(new[] {
                            new SwitchInfo("saver", /* Localizable */ "Opens the screensaver settings", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "splash", "type", "addonsaver" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("addonsaver", /* Localizable */ "Opens the addon screensaver settings", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "splash", "type", "saver" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("splash", /* Localizable */ "Opens the splash settings", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "saver", "type", "addonsaver" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("type", /* Localizable */ "Opens the custom settings", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "saver", "splash", "addonsaver" },
                                ArgumentsRequired = true
                            })
                        })
                    }, new SettingsCommand(), CommandFlags.Strict)
            },
            
            { "set",
                new CommandInfo("set", /* Localizable */ "Sets a variable to a value in a script",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "value"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new SetCommand())
            },

            { "setexthandler",
                new CommandInfo("setexthandler", /* Localizable */ "Sets the default extension handler of the specified extension to the specific implementer",
                    new[] {
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
                    }, new SetExtHandlerCommand())
            },

            { "setrange",
                new CommandInfo("setrange", /* Localizable */ "Creates a variable array with the provided values",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "value"),
                            new CommandArgumentPart(false, "value2"),
                            new CommandArgumentPart(false, "value3 ..."),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new SetRangeCommand())
            },
            
            { "sftp",
                new CommandInfo("sftp", /* Localizable */ "Lets you use an SSH FTP server",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "server"),
                        })
                    }, new SftpCommand())
            },
            
            { "shownotifs",
                new CommandInfo("shownotifs", /* Localizable */ "Shows all received notifications",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ShowNotifsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "showtd",
                new CommandInfo("showtd", /* Localizable */ "Shows date and time",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ShowTdCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "showtdzone",
                new CommandInfo("showtdzone", /* Localizable */ "Shows date and time in zones",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "timezone"),
                        }, new[] {
                            new SwitchInfo("all", /* Localizable */ "Shows all the time zones", new SwitchOptions()
                            {
                                OptionalizeLastRequiredArguments = 1,
                                AcceptsValues = false
                            })
                        })
                    }, new ShowTdZoneCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "shutdown",
                new CommandInfo("shutdown", /* Localizable */ "The kernel will be shut down",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "ip"),
                            new CommandArgumentPart(false, "port", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        })
                    }, new ShutdownCommand())
            },
            
            { "sleep",
                new CommandInfo("sleep", /* Localizable */ "Sleeps for specified milliseconds",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ms"),
                        })
                    }, new SleepCommand())
            },
            
            { "sshell",
                new CommandInfo("sshell", /* Localizable */ "Connects to an SSH server.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "address:port"),
                            new CommandArgumentPart(true, "username"),
                        })
                    }, new SshellCommand())
            },
            
            { "sshcmd",
                new CommandInfo("sshcmd", /* Localizable */ "Connects to an SSH server to execute a command.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "address:port"),
                            new CommandArgumentPart(true, "username"),
                            new CommandArgumentPart(true, "command"),
                        })
                    }, new SshcmdCommand())
            },

            { "sudo",
                new CommandInfo("sudo", /* Localizable */ "Runs the command as the root user",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "command"),
                        })
                    }, new SudoCommand())
            },

            { "sumfile",
                new CommandInfo("sumfile", /* Localizable */ "Calculates file sums.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames()
                            }),
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(false, "outputFile"),
                        }, new[] {
                            new SwitchInfo("relative", /* Localizable */ "Uses relative path instead of absolute", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    }, new SumFileCommand())
            },
            
            { "sumfiles",
                new CommandInfo("sumfiles", /* Localizable */ "Calculates sums of files in specified directory.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "algorithm/all", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => EncryptionDriverTools.GetEncryptionDriverNames()
                            }),
                            new CommandArgumentPart(true, "dir"),
                            new CommandArgumentPart(false, "outputFile"),
                        }, new[] {
                            new SwitchInfo("relative", /* Localizable */ "Uses relative path instead of absolute", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    }, new SumFilesCommand())
            },
            
            { "taskman",
                new CommandInfo("taskman", /* Localizable */ "Task manager",
                    new[] {
                        new CommandArgumentInfo()
                    }, new TaskManCommand())
            },
            
            { "themeprev",
                new CommandInfo("themeprev", /* Localizable */ "Previews a theme",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "theme"),
                        })
                    }, new ThemePrevCommand())
            },
            
            { "themeset",
                new CommandInfo("themeset", /* Localizable */ "Selects a theme and sets it",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "theme"),
                        })
                    }, new ThemeSetCommand())
            },

            { "unblockdbgdev",
                new CommandInfo("unblockdbgdev", /* Localizable */ "Unblock a debug device by IP address",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ipaddress"),
                        })
                    }, new UnblockDbgDevCommand(), CommandFlags.Strict)
            },
            
            { "unset",
                new CommandInfo("unset", /* Localizable */ "Removes a variable from the UESH variable list",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "$variable"),
                        }, new[]
                        {
                            new SwitchInfo("justwipe", /* Localizable */ "Just wipes the variable value without removing it", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    }, new UnsetCommand())
            },
            
            { "unzip",
                new CommandInfo("unzip", /* Localizable */ "Extracts a ZIP archive",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "zipfile"),
                            new CommandArgumentPart(false, "path"),
                        }, new[] {
                            new SwitchInfo("createdir", /* Localizable */ "Creates a directory that contains the contents of the ZIP file", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    }, new UnZipCommand())
            },
            
            { "update",
                new CommandInfo("update", /* Localizable */ "System update",
                    new[] {
                        new CommandArgumentInfo()
                    }, new UpdateCommand(), CommandFlags.Strict)
            },
            
            { "uptime",
                new CommandInfo("uptime", /* Localizable */ "Shows the kernel uptime",
                    new[] {
                        new CommandArgumentInfo(true)
                    }, new UptimeCommand())
            },
            
            { "usermanual",
                new CommandInfo("usermanual", /* Localizable */ "Shows the two useful URLs for manual.",
                    new[] {
                        new CommandArgumentInfo()
                    }, new UserManualCommand())
            },
            
            { "verify",
                new CommandInfo("verify", /* Localizable */ "Verifies sanity of the file",
                    new[] {
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
                    }, new VerifyCommand())
            },

            { "version",
                new CommandInfo("version", /* Localizable */ "Gets the current version",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new SwitchInfo("m", /* Localizable */ "Shows the kernel mod API version", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "k" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("k", /* Localizable */ "Shows the kernel version", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "m" },
                                AcceptsValues = false
                            })
                        }, true)
                    }, new VersionCommand())
            },

            { "whoami",
                new CommandInfo("whoami", /* Localizable */ "Gets the current user name",
                    new[] {
                        new CommandArgumentInfo(true)
                    }, new WhoamiCommand())
            },

            { "winelevate",
                new CommandInfo("winelevate", /* Localizable */ "Restarts Nitrocid with the elevated permissions (Windows only)",
                    new[] {
                        new CommandArgumentInfo(true)
                    }, new WinElevateCommand())
            },

            { "wraptext",
                new CommandInfo("wraptext", /* Localizable */ "Wraps the text file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        }, new[]
                        {
                            new SwitchInfo("columns", /* Localizable */ "Specifies the columns per line", new SwitchOptions()
                            {
                                ArgumentsRequired = true,
                                IsNumeric = true
                            })
                        }, true)
                    }, new WrapTextCommand())
            },

            { "zip",
                new CommandInfo("zip", /* Localizable */ "Creates a ZIP archive",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "zipfile"),
                            new CommandArgumentPart(true, "path"),
                        }, new[] {
                            new SwitchInfo("fast", /* Localizable */ "Fast compression", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "nocomp" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("nocomp", /* Localizable */ "No compression", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "fast" },
                                AcceptsValues = false
                            }),
                            new SwitchInfo("nobasedir", /* Localizable */ "Don't create base directory in archive", new SwitchOptions()
                            {
                                AcceptsValues = false
                            })
                        })
                    }, new ZipCommand())
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
