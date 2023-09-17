
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using KS.Shell.Shells.UESH.Commands;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Prompts;
using KS.Shell.Prompts.Presets.UESH;
using KS.Shell.ShellBase.Commands.UnifiedCommands;
using System.Linq;
using KS.Users;
using UnitsNet;
using KS.ConsoleBase.Themes;
using KS.Misc.Screensaver;
using KS.Misc.Splash;
using KS.Users.Groups;
using KS.Modifications;
using KS.Languages;

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
                new CommandInfo("addgroup", ShellType, /* Localizable */ "Adds groups",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "groupName")
                        }, Array.Empty<SwitchInfo>())
                    }, new AddGroupCommand(), CommandFlags.Strict)
            },
            
            { "adduser",
                new CommandInfo("adduser", ShellType, /* Localizable */ "Adds users",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "username"),
                            new CommandArgumentPart(false, "password"),
                            new CommandArgumentPart(false, "confirm"),
                        }, Array.Empty<SwitchInfo>())
                    }, new AddUserCommand(), CommandFlags.Strict)
            },
            
            { "addusertogroup",
                new CommandInfo("addusertogroup", ShellType, /* Localizable */ "Adds users to a group",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "username"),
                            new CommandArgumentPart(true, "group"),
                        }, Array.Empty<SwitchInfo>())
                    }, new AddUserToGroupCommand(), CommandFlags.Strict)
            },
            
            { "admin",
                new CommandInfo("admin", ShellType, /* Localizable */ "Administrative shell",
                    new[] {
                        new CommandArgumentInfo()
                    }, new AdminCommand(), CommandFlags.Strict)
            },
            
            { "alias",
                new CommandInfo("alias", ShellType, /* Localizable */ "Adds aliases to commands",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "rem/add"),
                            new CommandArgumentPart(true, $"{string.Join("/", Enum.GetNames(typeof(ShellType)))}"),
                            new CommandArgumentPart(true, $"alias"),
                            new CommandArgumentPart(false, $"cmd", (startFrom, _, _) => HelpUnifiedCommand.ListCmds(startFrom)),
                        }, Array.Empty<SwitchInfo>())
                    }, new AliasCommand(), CommandFlags.Strict)
            },
            
            { "backrace",
                new CommandInfo("backrace", ShellType, /* Localizable */ "Do you back the wrong horse?",
                    new[] {
                        new CommandArgumentInfo()
                    }, new BackRaceCommand())
            },
            
            { "beep",
                new CommandInfo("beep", ShellType, /* Localizable */ "Beeps from the console",
                    new[] {
                        new CommandArgumentInfo()
                    }, new BeepCommand())
            },
            
            { "blockdbgdev",
                new CommandInfo("blockdbgdev", ShellType, /* Localizable */ "Block a debug device by IP address",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ipaddress"),
                        }, Array.Empty<SwitchInfo>())
                    }, new BlockDbgDevCommand(), CommandFlags.Strict)
            },
            
            { "bulkrename",
                new CommandInfo("bulkrename", ShellType, /* Localizable */ "Renames group of files to selected format",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "targetdir"),
                            new CommandArgumentPart(true, "pattern"),
                            new CommandArgumentPart(false, "newname"),
                        }, Array.Empty<SwitchInfo>())
                    }, new BulkRenameCommand())
            },
            
            { "calc",
                new CommandInfo("calc", ShellType, /* Localizable */ "Calculator to calculate expressions.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "expression"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new CalcCommand())
            },
            
            { "cat",
                new CommandInfo("cat", ShellType, /* Localizable */ "Prints content of file to console",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        }, new[] {
                            new SwitchInfo("lines", /* Localizable */ "Prints the line numbers alongside the contents", false, false, new string[] { "nolines" }, 0, false),
                            new SwitchInfo("nolines", /* Localizable */ "Prints only the contents", false, false, new string[] { "lines" }, 0, false),
                            new SwitchInfo("plain", /* Localizable */ "Force treating binary files as plain text", false, false, Array.Empty<string>(), 0, false)
                        })
                    }, new CatCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "cdir",
                new CommandInfo("cdir", ShellType, /* Localizable */ "Gets the current directory",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), Array.Empty<SwitchInfo>(), true)
                    }, new CDirCommand())
            },

            { "chattr",
                new CommandInfo("chattr", ShellType, /* Localizable */ "Changes attribute of a file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(true, "+/-attributes"),
                        }, Array.Empty<SwitchInfo>())
                    }, new ChAttrCommand())
            },
            
            { "chdir",
                new CommandInfo("chdir", ShellType, /* Localizable */ "Changes directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory/.."),
                        }, Array.Empty<SwitchInfo>())
                    }, new ChDirCommand())
            },
            
            { "chhostname",
                new CommandInfo("chhostname", ShellType, /* Localizable */ "Changes host name",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "hostname"),
                        }, Array.Empty<SwitchInfo>())
                    }, new ChHostNameCommand(), CommandFlags.Strict)
            },
            
            { "chklock",
                new CommandInfo("chklock", ShellType, /* Localizable */ "Checks the file or the folder lock",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        }, new[] {
                            new SwitchInfo("waitforunlock", /* Localizable */ "Waits until the file or the folder is unlocked", false, true, Array.Empty<string>(), 0, false)
                        })
                    }, new ChkLockCommand())
            },
            
            { "chmal",
                new CommandInfo("chmal", ShellType, /* Localizable */ "Changes MAL, the MOTD After Login",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "message"),
                        }, Array.Empty<SwitchInfo>())
                    }, new ChMalCommand(), CommandFlags.Strict)
            },
            
            { "chmotd",
                new CommandInfo("chmotd", ShellType, /* Localizable */ "Changes MOTD, the Message Of The Day",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "message"),
                        }, Array.Empty<SwitchInfo>())
                    }, new ChMotdCommand(), CommandFlags.Strict)
            },
            
            { "choice",
                new CommandInfo("choice", ShellType, /* Localizable */ "Makes user choices",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "answers"),
                            new CommandArgumentPart(true, "input"),
                            new CommandArgumentPart(false, "answertitle1"),
                            new CommandArgumentPart(false, "answertitle2 ..."),
                        }, new[] {
                            new SwitchInfo("o", /* Localizable */ "One line choice style", false, false, new string[] { "t", "m", "a" }, 0, false),
                            new SwitchInfo("t", /* Localizable */ "Two lines choice style", false, false, new string[] { "o", "m", "a" }, 0, false),
                            new SwitchInfo("m", /* Localizable */ "Modern choice style", false, false, new string[] { "t", "o", "a" }, 0, false),
                            new SwitchInfo("a", /* Localizable */ "Table choice style", false, false, new string[] { "t", "o", "m" }, 0, false),
                            new SwitchInfo("single", /* Localizable */ "The output can be only one character", false, false, new string[] { "multiple" }, 0, false),
                            new SwitchInfo("multiple", /* Localizable */ "The output can be more than a character", false, false, new string[] { "single" }, 0, false)
                        }, true)
                    }, new ChoiceCommand())
            },
            
            { "chpwd",
                new CommandInfo("chpwd", ShellType, /* Localizable */ "Changes password for current user",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Username", (startFrom, _, _) => UserManagement.ListAllUsers().Where((src) => src.StartsWith(startFrom)).ToArray()),
                            new CommandArgumentPart(true, "UserPass"),
                            new CommandArgumentPart(true, "newPass"),
                            new CommandArgumentPart(true, "confirm"),
                        }, Array.Empty<SwitchInfo>())
                    }, new ChPwdCommand(), CommandFlags.Strict)
            },
            
            { "chusrname",
                new CommandInfo("chusrname", ShellType, /* Localizable */ "Changes user name",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "oldUserName", (startFrom, _, _) => UserManagement.ListAllUsers().Where((src) => src.StartsWith(startFrom)).ToArray()),
                            new CommandArgumentPart(true, "newUserName"),
                        }, Array.Empty<SwitchInfo>())
                    }, new ChUsrNameCommand(), CommandFlags.Strict)
            },
            
            { "cls",
                new CommandInfo("cls", ShellType, /* Localizable */ "Clears the screen",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ClsCommand())
            },
            
            { "colorhextorgb",
                new CommandInfo("colorhextorgb", ShellType, /* Localizable */ "Converts the hexadecimal representation of the color to RGB numbers.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToRgbCommand())
            },
            
            { "colorhextorgbks",
                new CommandInfo("colorhextorgbks", ShellType, /* Localizable */ "Converts the hexadecimal representation of the color to RGB numbers in KS format.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "#RRGGBB"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorHexToRgbKSCommand())
            },
            
            { "colorrgbtohex",
                new CommandInfo("colorrgbtohex", ShellType, /* Localizable */ "Converts the color RGB numbers to hex.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "R"),
                            new CommandArgumentPart(true, "G"),
                            new CommandArgumentPart(true, "B"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new ColorRgbToHexCommand())
            },
            
            { "combinestr",
                new CommandInfo("combinestr", ShellType, /* Localizable */ "Combines the two text files or more into the console.",
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
                new CommandInfo("combine", ShellType, /* Localizable */ "Combines the two text files or more into the output file.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "output"),
                            new CommandArgumentPart(true, "input"),
                            new CommandArgumentPart(true, "input2"),
                            new CommandArgumentPart(false, "input3 ..."),
                        }, Array.Empty<SwitchInfo>())
                    }, new CombineCommand())
            },
            
            { "convertlineendings",
                new CommandInfo("convertlineendings", ShellType, /* Localizable */ "Converts the line endings to format for the current platform or to specified custom format",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "textfile"),
                        }, new[] {
                            new SwitchInfo("w", /* Localizable */ "Converts the line endings to the Windows format", false, false, new string[] { "u", "m" }, 0, false),
                            new SwitchInfo("u", /* Localizable */ "Converts the line endings to the Unix format", false, false, new string[] { "w", "m" }, 0, false),
                            new SwitchInfo("m", /* Localizable */ "Converts the line endings to the Mac OS 9 format", false, false, new string[] { "u", "w" }, 0, false),
                            new SwitchInfo("force", /* Localizable */ "Forces the line ending conversion", false, false, Array.Empty<string>(), 0, false),
                        })
                    }, new ConvertLineEndingsCommand())
            },
            
            { "copy",
                new CommandInfo("copy", ShellType, /* Localizable */ "Creates another copy of a file under different directory or name.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "source"),
                            new CommandArgumentPart(true, "target"),
                        }, Array.Empty<SwitchInfo>())
                    }, new CopyCommand())
            },
            
            { "date",
                new CommandInfo("date", ShellType, /* Localizable */ "Shows date and time",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), new[] {
                            new SwitchInfo("date", /* Localizable */ "Shows just the date", false, false, new string[] { "time", "full" }, 0, false),
                            new SwitchInfo("time", /* Localizable */ "Shows just the time", false, false, new string[] { "date", "full" }, 0, false),
                            new SwitchInfo("full", /* Localizable */ "Shows date and time", false, false, new string[] { "date", "time" }, 0, false),
                            new SwitchInfo("utc", /* Localizable */ "Uses UTC instead of local", false, false, Array.Empty<string>(), 0, false)
                        }, true)
                    }, new DateCommand(), CommandFlags.RedirectionSupported)
            },
            
            { "debugshell",
                new CommandInfo("debugshell", ShellType, /* Localizable */ "Starts the debug shell",
                    new[] {
                        new CommandArgumentInfo()
                    }, new DebugShellCommand(), CommandFlags.Strict)
            },
            
            { "dict",
                new CommandInfo("dict", ShellType, /* Localizable */ "The English Dictionary",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "word"),
                        }, Array.Empty<SwitchInfo>())
                    }, new DictCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "dirinfo",
                new CommandInfo("dirinfo", ShellType, /* Localizable */ "Provides information about a directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory"),
                        }, Array.Empty<SwitchInfo>())
                    }, new DirInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "disconndbgdev",
                new CommandInfo("disconndbgdev", ShellType, /* Localizable */ "Disconnect a debug device",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ip"),
                        }, Array.Empty<SwitchInfo>())
                    }, new DisconnDbgDevCommand(), CommandFlags.Strict)
            },

            { "diskinfo",
                new CommandInfo("diskinfo", ShellType, /* Localizable */ "Provides information about a disk",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "diskNum"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new DiskInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "dismissnotif",
                new CommandInfo("dismissnotif", ShellType, /* Localizable */ "Dismisses a notification",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "notificationNumber"),
                        }, Array.Empty<SwitchInfo>())
                    }, new DismissNotifCommand())
            },
            
            { "dismissnotifs",
                new CommandInfo("dismissnotifs", ShellType, /* Localizable */ "Dismisses all notifications",
                    new[] {
                        new CommandArgumentInfo()
                    }, new DismissNotifsCommand())
            },
            
            { "echo",
                new CommandInfo("echo", ShellType, /* Localizable */ "Writes text into the console",
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
                new CommandInfo("edit", ShellType, /* Localizable */ "Edits a file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        }, new[] {
                            new SwitchInfo("text", /* Localizable */ "Forces text mode", false, false, new string[] { "hex", "json", "sql" }, 0, false),
                            new SwitchInfo("hex", /* Localizable */ "Forces hex mode", false, false, new string[] { "text", "json", "sql" }, 0, false),
                            new SwitchInfo("json", /* Localizable */ "Forces JSON mode", false, false, new string[] { "text", "hex", "sql" }, 0, false),
                            new SwitchInfo("sql", /* Localizable */ "Forces SQL mode", false, false, new string[] { "text", "hex", "json" }, 0, false),
                        })
                    }, new EditCommand())
            },
            
            { "fileinfo",
                new CommandInfo("fileinfo", ShellType, /* Localizable */ "Provides information about a file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        }, Array.Empty<SwitchInfo>())
                    }, new FileInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "find",
                new CommandInfo("find", ShellType, /* Localizable */ "Finds a file in the specified directory or in the current directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(true, "directory"),
                        }, new[] {
                            new SwitchInfo("recursive", /* Localizable */ "Searches for a file recursively", false, false, Array.Empty<string>(), 0, false),
                            new SwitchInfo("exec", /* Localizable */ "Executes a command on a file", false, true)
                        }, true)
                    }, new FindCommand())
            },
            
            { "findreg",
                new CommandInfo("findreg", ShellType, /* Localizable */ "Finds a file in the specified directory or in the current directory using regular expressions",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "fileRegex"),
                            new CommandArgumentPart(true, "directory"),
                        }, new[] {
                            new SwitchInfo("recursive", /* Localizable */ "Searches for a file recursively", false, false, Array.Empty<string>(), 0, false),
                            new SwitchInfo("exec", /* Localizable */ "Executes a command on a file", false, true)
                        }, true)
                    }, new FindRegCommand())
            },
            
            { "fork",
                new CommandInfo("fork", ShellType, /* Localizable */ "Forks the UESH shell to create another instance",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ForkCommand())
            },
            
            { "ftp",
                new CommandInfo("ftp", ShellType, /* Localizable */ "Use an FTP shell to interact with servers",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "server"),
                        }, Array.Empty<SwitchInfo>())
                    }, new FtpCommand())
            },
            
            { "genname",
                new CommandInfo("genname", ShellType, /* Localizable */ "Name and surname generator",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "namescount"),
                            new CommandArgumentPart(false, "nameprefix"),
                            new CommandArgumentPart(false, "namesuffix"),
                            new CommandArgumentPart(false, "surnameprefix"),
                            new CommandArgumentPart(false, "surnamesuffix"),
                        }, new[] {
                            new SwitchInfo("t", /* Localizable */ "Generate nametags (umlauts are currently not supported)", false, false, Array.Empty<string>(), 0, false)
                        }, true)
                    }, new GenNameCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "gettimeinfo",
                new CommandInfo("gettimeinfo", ShellType, /* Localizable */ "Gets the date and time information",
                    new[] { 
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "date")
                        }, Array.Empty<SwitchInfo>())
                    }, new GetTimeInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "get",
                new CommandInfo("get", ShellType, /* Localizable */ "Downloads a file to current working directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "url")
                        }, new[]
                        {
                            new SwitchInfo("outputpath", /* Localizable */ "Specifies the output path", false, true)
                        })
                    }, new Get_Command())
            },
            
            { "hangman",
                new CommandInfo("hangman", ShellType, /* Localizable */ "Starts the Hangman game",
                    new[] {
                        new CommandArgumentInfo()
                    }, new HangmanCommand())
            },

            { "host",
                new CommandInfo("host", ShellType, /* Localizable */ "Gets the current host name",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), Array.Empty<SwitchInfo>(), true)
                    }, new HostCommand())
            },
            
            { "http",
                new CommandInfo("http", ShellType, /* Localizable */ "Starts the HTTP shell",
                    new[] {
                        new CommandArgumentInfo()
                    }, new HttpCommand())
            },
            
            { "hwinfo",
                new CommandInfo("hwinfo", ShellType, /* Localizable */ "Prints hardware information",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "HardwareType", (_, _, _) => new[] { "HDD", "LogicalParts", "CPU", "GPU", "Sound", "Network", "System", "Machine", "BIOS", "RAM", "all" })
                        }, Array.Empty<SwitchInfo>())
                    }, new HwInfoCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
            },
            
            { "if",
                new CommandInfo("if", ShellType, /* Localizable */ "Executes commands once the UESH expressions are satisfied",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ueshExpression"),
                            new CommandArgumentPart(true, "command"),
                        }, Array.Empty<SwitchInfo>())
                    }, new IfCommand())
            },
            
            { "ifm",
                new CommandInfo("ifm", ShellType, /* Localizable */ "Interactive system host file manager",
                    new[] {
                        new CommandArgumentInfo()
                    }, new IfmCommand())
            },
            
            { "imaginary",
                new CommandInfo("imaginary", ShellType, /* Localizable */ "Show information about the imaginary number formula specified by a specified real and imaginary number",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "real"),
                            new CommandArgumentPart(true, "imaginary"),
                        }, Array.Empty<SwitchInfo>())
                    }, new ImaginaryCommand())
            },
            
            { "input",
                new CommandInfo("input", ShellType, /* Localizable */ "Allows user to enter input",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "question"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new InputCommand())
            },
            
            { "jsonbeautify",
                new CommandInfo("jsonbeautify", ShellType, /* Localizable */ "Beautifies the JSON file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "jsonfile"),
                            new CommandArgumentPart(true, "output"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new JsonBeautifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "jsonminify",
                new CommandInfo("jsonminify", ShellType, /* Localizable */ "Minifies the JSON file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "jsonfile"),
                            new CommandArgumentPart(true, "output"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new JsonMinifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "langman",
                new CommandInfo("langman", ShellType, /* Localizable */ "Manage your languages",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "reload/load/unload"),
                            new CommandArgumentPart(true, "customlanguagename", (startFrom, _, _) => LanguageManager.CustomLanguages.Keys.Where((src) => src.StartsWith(startFrom)).ToArray()),
                        }, Array.Empty<SwitchInfo>()),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "list/reloadall"),
                        }, Array.Empty<SwitchInfo>())
                    }, new LangManCommand(), CommandFlags.Strict)
            },
            
            { "license",
                new CommandInfo("license", ShellType, /* Localizable */ "Shows license information for the kernel",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LicenseCommand())
            },
            
            { "lintscript",
                new CommandInfo("lintscript", ShellType, /* Localizable */ "Checks a UESH script for syntax errors",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "script"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new LintScriptCommand())
            },
            
            { "list",
                new CommandInfo("list", ShellType, /* Localizable */ "List file/folder contents in current folder",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "directory"),
                        }, new[] {
                            new SwitchInfo("showdetails", /* Localizable */ "Shows the file details in the list", false, false, Array.Empty<string>(), 0, false),
                            new SwitchInfo("suppressmessages", /* Localizable */ "Suppresses the annoying \"permission denied\" messages", false, false, Array.Empty<string>(), 0, false),
                            new SwitchInfo("recursive", /* Localizable */ "Lists a folder recursively", false, false, Array.Empty<string>(), 0, false)
                        })
                    }, new ListCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "listunits",
                new CommandInfo("listunits", ShellType, /* Localizable */ "Lists all available units",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "type", (startFrom, _, _) => Quantity.Infos.Select((src) => src.Name).Where((src) => src.StartsWith(startFrom)).ToArray()),
                        }, Array.Empty<SwitchInfo>())
                    }, new ListUnitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "lockscreen",
                new CommandInfo("lockscreen", ShellType, /* Localizable */ "Locks your screen with a password",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LockScreenCommand())
            },
            
            { "logout",
                new CommandInfo("logout", ShellType, /* Localizable */ "Logs you out",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LogoutCommand(), CommandFlags.NoMaintenance)
            },

            { "lsconnections",
                new CommandInfo("lsconnections", ShellType, /* Localizable */ "Lists all available connections",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsConnectionsCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "lsdbgdev",
                new CommandInfo("lsdbgdev", ShellType, /* Localizable */ "Lists debugging devices connected",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsDbgDevCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
           
            { "lsdiskparts",
                new CommandInfo("lsdiskparts", ShellType, /* Localizable */ "Lists all the disk partitions",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "diskNum"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new LsDiskPartsCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
           
            { "lsdisks",
                new CommandInfo("lsdisks", ShellType, /* Localizable */ "Lists all the disks",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), Array.Empty<SwitchInfo>(), true)
                    }, new LsDisksCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "lsnet",
                new CommandInfo("lsnet", ShellType, /* Localizable */ "Lists online network devices",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsNetCommand(), CommandFlags.Strict)
            },
            
            { "lsusers",
                new CommandInfo("lsusers", ShellType, /* Localizable */ "Lists the users",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), Array.Empty<SwitchInfo>(), true)
                    }, new LsUsersCommand())
            },
            
            { "lsvars",
                new CommandInfo("lsvars", ShellType, /* Localizable */ "Lists available UESH variables",
                    new[] {
                        new CommandArgumentInfo()
                    }, new LsVarsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "mail",
                new CommandInfo("mail", ShellType, /* Localizable */ "Opens the mail client",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "emailAddress"),
                        }, Array.Empty<SwitchInfo>())
                    }, new MailCommand()) },
            
            { "md",
                new CommandInfo("md", ShellType, /* Localizable */ "Creates a directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new MdCommand()) },
            
            { "meteor",
                new CommandInfo("meteor", ShellType, /* Localizable */ "You are a spaceship and the meteors are coming to destroy you. Can you save it?",
                    new[] {
                        new CommandArgumentInfo()
                    }, new MeteorCommand())
            },
            
            { "mkfile",
                new CommandInfo("mkfile", ShellType, /* Localizable */ "Makes a new file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "file"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new MkFileCommand())
            },
            
            { "mklang",
                new CommandInfo("mklang", ShellType, /* Localizable */ "Makes a new language",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "pathToTranslations"),
                        }, Array.Empty<SwitchInfo>())
                    }, new MkLangCommand())
            },
            
            { "mktheme",
                new CommandInfo("mktheme", ShellType, /* Localizable */ "Makes a new theme",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "themeName"),
                        }, Array.Empty<SwitchInfo>())
                    }, new MkThemeCommand())
            },
            
            { "modman",
                new CommandInfo("modman", ShellType, /* Localizable */ "Manage your mods",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "start/stop/info/reload/install/uninstall"),
                            new CommandArgumentPart(true, "modfilename"),
                        }, Array.Empty<SwitchInfo>()),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "list/listparts"),
                            new CommandArgumentPart(true, "modname", (startFrom, _, _) => ModManager.ListMods(startFrom).Keys.ToArray()),
                        }, Array.Empty<SwitchInfo>()),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "reloadall/stopall/startall"),
                        }, Array.Empty<SwitchInfo>()),
                    }, new ModManCommand(), CommandFlags.Strict)
            },
            
            { "modmanual",
                new CommandInfo("modmanual", ShellType, /* Localizable */ "Mod manual",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "modname", (startFrom, _, _) => ModManager.ListMods(startFrom).Keys.ToArray()),
                        }, Array.Empty<SwitchInfo>())
                    }, new ModManualCommand())
            },
            
            { "move",
                new CommandInfo("move", ShellType, /* Localizable */ "Moves a file to another directory",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "source"),
                            new CommandArgumentPart(true, "target"),
                        }, Array.Empty<SwitchInfo>())
                    }, new MoveCommand())
            },

            { "partinfo",
                new CommandInfo("partinfo", ShellType, /* Localizable */ "Provides information about a partition from the specified disk",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "diskNum"),
                            new CommandArgumentPart(true, "partNum"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new PartInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "pathfind",
                new CommandInfo("pathfind", ShellType, /* Localizable */ "Finds a given file name from path lookup directories",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "fileName"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new PathFindCommand())
            },
            
            { "perm",
                new CommandInfo("perm", ShellType, /* Localizable */ "Manage permissions for users",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "userName", (startFrom, _, _) => UserManagement.ListAllUsers().Where((src) => src.StartsWith(startFrom)).ToArray()),
                            new CommandArgumentPart(true, "allow/revoke"),
                            new CommandArgumentPart(true, "perm"),
                        }, Array.Empty<SwitchInfo>())
                    }, new PermCommand(), CommandFlags.Strict)
            },
            
            { "permgroup",
                new CommandInfo("permgroup", ShellType, /* Localizable */ "Manage permissions for groups",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "groupName", (startFrom, _, _) => GroupManagement.AvailableGroups.Select((src) => src.GroupName).Where((src) => src.StartsWith(startFrom)).ToArray()),
                            new CommandArgumentPart(true, "allow/revoke"),
                            new CommandArgumentPart(true, "perm"),
                        }, Array.Empty<SwitchInfo>())
                    }, new PermGroupCommand(), CommandFlags.Strict)
            },
            
            { "ping",
                new CommandInfo("ping", ShellType, /* Localizable */ "Pings an address",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Address1"),
                            new CommandArgumentPart(false, "Address2 ..."),
                        }, new[] {
                            new SwitchInfo("times", /* Localizable */ "Specifies number of times to ping", false, true)
                        })
                    }, new PingCommand())
            },

            { "platform",
                new CommandInfo("platform", ShellType, /* Localizable */ "Gets the current platform",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), new[]
                        {
                            new SwitchInfo("n", /* Localizable */ "Shows the platform name", false, false, new string[]{ "v", "b", "c", "r" }, 0, false),
                            new SwitchInfo("v", /* Localizable */ "Shows the platform version", false, false, new string[]{ "n", "b", "c", "r" }, 0, false),
                            new SwitchInfo("b", /* Localizable */ "Shows the platform bits", false, false, new string[]{ "n", "v", "c", "r" }, 0, false),
                            new SwitchInfo("c", /* Localizable */ "Shows the .NET platform version", false, false, new string[]{ "n", "v", "b", "r" }, 0, false),
                            new SwitchInfo("r", /* Localizable */ "Shows the .NET platform runtime identifier", false, false, new string[]{ "n", "v", "b", "c" }, 0, false)
                        }, true)
                    }, new PlatformCommand())
            },

            { "playlyric",
                new CommandInfo("playlyric", ShellType, /* Localizable */ "Plays a lyric file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "lyric.lrc"),
                        }, Array.Empty<SwitchInfo>())
                    }, new PlayLyricCommand())
            },
            
            { "previewsplash",
                new CommandInfo("previewsplash", ShellType, /* Localizable */ "Previews the splash",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "splashName", (startFrom, _, _) => SplashManager.Splashes.Keys.Where((src) => src.StartsWith(startFrom)).ToArray()),
                        }, new[]
                        {
                            new SwitchInfo("splashout", /* Localizable */ "Specifies whether to test out the important messages feature on splash", false, false, Array.Empty<string>(), 0, false)
                        })
                    }, new PreviewSplashCommand())
            },
            
            { "put",
                new CommandInfo("put", ShellType, /* Localizable */ "Uploads a file to specified website",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "FileName"),
                            new CommandArgumentPart(true, "URL"),
                        }, Array.Empty<SwitchInfo>())
                    }, new PutCommand())
            },
            
            { "quote",
                new CommandInfo("quote", ShellType, /* Localizable */ "Gets a random quote",
                    new[] {
                        new CommandArgumentInfo()
                    }, new QuoteCommand())
            },
            
            { "reboot",
                new CommandInfo("reboot", ShellType, /* Localizable */ "Restarts your computer (WARNING: No syncing, because it is not a final kernel)",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "ip"),
                            new CommandArgumentPart(false, "port"),
                        }, Array.Empty<SwitchInfo>())
                    }, new RebootCommand())
            },
            
            { "reloadconfig",
                new CommandInfo("reloadconfig", ShellType, /* Localizable */ "Reloads configuration file that is edited.",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ReloadConfigCommand(), CommandFlags.Strict)
            },
            
            { "retroks",
                new CommandInfo("retroks", ShellType, /* Localizable */ "Retro Nitrocid KS based on 0.0.4.1",
                    new[] {
                        new CommandArgumentInfo()
                    }, new RetroKSCommand())
            },
            
            { "rexec",
                new CommandInfo("rexec", ShellType, /* Localizable */ "Remotely executes a command to remote PC",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "address"),
                            new CommandArgumentPart(true, "port"),
                            new CommandArgumentPart(false, "command"),
                        }, Array.Empty<SwitchInfo>())
                    }, new RexecCommand(), CommandFlags.Strict)
            },
            
            { "rm",
                new CommandInfo("rm", ShellType, /* Localizable */ "Removes a directory or a file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "directory/file"),
                        }, Array.Empty<SwitchInfo>())
                    }, new RmCommand())
            },
            
            { "rdebug",
                new CommandInfo("rdebug", ShellType, /* Localizable */ "Enables or disables remote debugging.",
                    new[] {
                        new CommandArgumentInfo()
                    }, new RdebugCommand(), CommandFlags.Strict)
            },
            
            { "rmuser",
                new CommandInfo("rmuser", ShellType, /* Localizable */ "Removes a user from the list",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Username", (startFrom, _, _) => UserManagement.ListAllUsers().Where((src) => src.StartsWith(startFrom)).ToArray()),
                        }, Array.Empty<SwitchInfo>())
                    }, new RmUserCommand(), CommandFlags.Strict)
            },
            
            { "rmgroup",
                new CommandInfo("rmgroup", ShellType, /* Localizable */ "Removes a group from the list",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "GroupName", (startFrom, _, _) => GroupManagement.AvailableGroups.Select((src) => src.GroupName).Where((src) => src.StartsWith(startFrom)).ToArray()),
                        }, Array.Empty<SwitchInfo>())
                    }, new RmGroupCommand(), CommandFlags.Strict)
            },
            
            { "rmuserfromgroup",
                new CommandInfo("rmuserfromgroup", ShellType, /* Localizable */ "Removes a user from the group",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "UserName", (startFrom, _, _) => UserManagement.ListAllUsers().Where((src) => src.StartsWith(startFrom)).ToArray()),
                            new CommandArgumentPart(true, "GroupName", (startFrom, _, _) => GroupManagement.AvailableGroups.Select((src) => src.GroupName).Where((src) => src.StartsWith(startFrom)).ToArray()),
                        }, Array.Empty<SwitchInfo>())
                    }, new RmUserFromGroupCommand(), CommandFlags.Strict)
            },
            
            { "roulette",
                new CommandInfo("roulette", ShellType, /* Localizable */ "Russian Roulette",
                    new[] {
                        new CommandArgumentInfo()
                    }, new RouletteCommand())
            },
            
            { "rss",
                new CommandInfo("rss", ShellType, /* Localizable */ "Opens an RSS shell to read the feeds",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "feedlink"),
                        }, Array.Empty<SwitchInfo>())
                    }, new RssCommand())
            },
            
            { "saveconfig",
                new CommandInfo("saveconfig", ShellType, /* Localizable */ "Saves the current kernel configuration to its file",
                    new[] {
                        new CommandArgumentInfo()
                    }, new SaveConfigCommand(), CommandFlags.Strict)
            },
            
            { "savescreen",
                new CommandInfo("savescreen", ShellType, /* Localizable */ "Saves your screen from burn outs",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "saver", (startFrom, _, _) => ScreensaverManager.Screensavers.Keys.Where((src) => src.StartsWith(startFrom)).ToArray()),
                        }, Array.Empty<SwitchInfo>())
                    }, new SaveScreenCommand())
            },
            
            { "search",
                new CommandInfo("search", ShellType, /* Localizable */ "Searches for specified string in the provided file using regular expressions",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "Regexp"),
                            new CommandArgumentPart(true, "File"),
                        }, Array.Empty<SwitchInfo>())
                    }, new SearchCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "searchword",
                new CommandInfo("searchword", ShellType, /* Localizable */ "Searches for specified string in the provided file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "StringEnclosedInDoubleQuotes"),
                            new CommandArgumentPart(true, "File"),
                        }, Array.Empty<SwitchInfo>())
                    }, new SearchWordCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "select",
                new CommandInfo("select", ShellType, /* Localizable */ "Provides a selection choice",
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
                new CommandInfo("setsaver", ShellType, /* Localizable */ "Sets up kernel screensavers",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "customsaver/builtinsaver"),
                        }, Array.Empty<SwitchInfo>())
                    }, new SetSaverCommand(), CommandFlags.Strict)
            },
            
            { "settings",
                new CommandInfo("settings", ShellType, /* Localizable */ "Changes kernel configuration",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), new[] {
                            new SwitchInfo("saver", /* Localizable */ "Opens the screensaver settings", false, false, new string[] { "splash" }, 0, false),
                            new SwitchInfo("splash", /* Localizable */ "Opens the splash settings", false, false, new string[] { "saver" }, 0, false)
                        })
                    }, new SettingsCommand(), CommandFlags.Strict)
            },
            
            { "set",
                new CommandInfo("set", ShellType, /* Localizable */ "Sets a variable to a value in a script",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "value"),
                        }, Array.Empty<SwitchInfo>(), true)
                    }, new SetCommand())
            },
            
            { "setrange",
                new CommandInfo("setrange", ShellType, /* Localizable */ "Creates a variable array with the provided values",
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
                new CommandInfo("sftp", ShellType, /* Localizable */ "Lets you use an SSH FTP server",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "server"),
                        }, Array.Empty<SwitchInfo>())
                    }, new SftpCommand())
            },
            
            { "shipduet",
                new CommandInfo("shipduet", ShellType, /* Localizable */ "Two spaceships are on a fight with each other. One shot and the spaceship will blow. This is a local two-player game.",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ShipDuetCommand())
            },
            
            { "shownotifs",
                new CommandInfo("shownotifs", ShellType, /* Localizable */ "Shows all received notifications",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ShowNotifsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "showtd",
                new CommandInfo("showtd", ShellType, /* Localizable */ "Shows date and time",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ShowTdCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "showtdzone",
                new CommandInfo("showtdzone", ShellType, /* Localizable */ "Shows date and time in zones",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "timezone"),
                        }, new[] {
                            new SwitchInfo("all", /* Localizable */ "Shows all the time zones", false, false, null, 1)
                        })
                    }, new ShowTdZoneCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
            
            { "shutdown",
                new CommandInfo("shutdown", ShellType, /* Localizable */ "The kernel will be shut down",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "ip"),
                            new CommandArgumentPart(false, "port"),
                        }, Array.Empty<SwitchInfo>())
                    }, new ShutdownCommand())
            },
            
            { "sleep",
                new CommandInfo("sleep", ShellType, /* Localizable */ "Sleeps for specified milliseconds",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ms"),
                        }, Array.Empty<SwitchInfo>())
                    }, new SleepCommand())
            },
            
            { "snaker",
                new CommandInfo("snaker", ShellType, /* Localizable */ "The snake game!",
                    new[] {
                        new CommandArgumentInfo()
                    }, new SnakerCommand())
            },
            
            { "solver",
                new CommandInfo("solver", ShellType, /* Localizable */ "See if you can solve mathematical equations on time",
                    new[] {
                        new CommandArgumentInfo()
                    }, new SolverCommand())
            },
            
            { "speedpress",
                new CommandInfo("speedpress", ShellType, /* Localizable */ "See if you can press a key on time",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), new[] {
                            new SwitchInfo("e", /* Localizable */ "Starts the game in easy difficulty", false, false, new string[] { "m", "h", "v", "c" }, 0, false),
                            new SwitchInfo("m", /* Localizable */ "Starts the game in medium difficulty", false, false, new string[] { "e", "h", "v", "c" }, 0, false),
                            new SwitchInfo("h", /* Localizable */ "Starts the game in hard difficulty", false, false, new string[] { "m", "e", "v", "c" }, 0, false),
                            new SwitchInfo("v", /* Localizable */ "Starts the game in very hard difficulty", false, false, new string[] { "m", "h", "e", "c" }, 0, false),
                            new SwitchInfo("c", /* Localizable */ "Starts the game in custom difficulty. Please note that the custom timeout in milliseconds should be written as argument.", false, true, new string[] { "m", "h", "v", "e" }) })
                    }, new SpeedPressCommand())
            },
            
            { "sshell",
                new CommandInfo("sshell", ShellType, /* Localizable */ "Connects to an SSH server.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "address:port"),
                            new CommandArgumentPart(true, "username"),
                        }, Array.Empty<SwitchInfo>())
                    }, new SshellCommand())
            },
            
            { "sshcmd",
                new CommandInfo("sshcmd", ShellType, /* Localizable */ "Connects to an SSH server to execute a command.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "address:port"),
                            new CommandArgumentPart(true, "username"),
                            new CommandArgumentPart(true, "command"),
                        }, Array.Empty<SwitchInfo>())
                    }, new SshcmdCommand())
            },
            
            { "stopwatch",
                new CommandInfo("stopwatch", ShellType, /* Localizable */ "A simple stopwatch",
                    new[] {
                        new CommandArgumentInfo()
                    }, new StopwatchCommand())
            },

            { "sudo",
                new CommandInfo("sudo", ShellType, /* Localizable */ "Runs the command as the root user",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "command"),
                        }, Array.Empty<SwitchInfo>())
                    }, new SudoCommand())
            },

            { "sumfile",
                new CommandInfo("sumfile", ShellType, /* Localizable */ "Calculates file sums.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "algorithm/all"),
                            new CommandArgumentPart(true, "file"),
                            new CommandArgumentPart(false, "outputFile"),
                        }, new[] {
                            new SwitchInfo("relative", /* Localizable */ "Uses relative path instead of absolute", false, false, Array.Empty<string>(), 0, false)
                        })
                    }, new SumFileCommand())
            },
            
            { "sumfiles",
                new CommandInfo("sumfiles", ShellType, /* Localizable */ "Calculates sums of files in specified directory.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "algorithm/all"),
                            new CommandArgumentPart(true, "dir"),
                            new CommandArgumentPart(false, "outputFile"),
                        }, new[] {
                            new SwitchInfo("relative", /* Localizable */ "Uses relative path instead of absolute", false, false, Array.Empty<string>(), 0, false)
                        })
                    }, new SumFilesCommand())
            },
            
            { "taskman",
                new CommandInfo("taskman", ShellType, /* Localizable */ "Task manager",
                    new[] {
                        new CommandArgumentInfo()
                    }, new TaskManCommand())
            },
            
            { "themesel",
                new CommandInfo("themesel", ShellType, /* Localizable */ "Selects a theme and sets it",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(false, "theme", (startFrom, _, _) => ThemeTools.GetInstalledThemes().Keys.Where((src) => src.StartsWith(startFrom)).ToArray()),
                        }, Array.Empty<SwitchInfo>())
                    }, new ThemeSelCommand())
            },
            
            { "timer",
                new CommandInfo("timer", ShellType, /* Localizable */ "A simple timer",
                    new[] {
                        new CommandArgumentInfo()
                    }, new TimerCommand())
            },

            { "unblockdbgdev",
                new CommandInfo("unblockdbgdev", ShellType, /* Localizable */ "Unblock a debug device by IP address",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "ipaddress"),
                        }, Array.Empty<SwitchInfo>())
                    }, new UnblockDbgDevCommand(), CommandFlags.Strict)
            },
            
            { "unitconv",
                new CommandInfo("unitconv", ShellType, /* Localizable */ "Unit converter",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "unittype"),
                            new CommandArgumentPart(true, "quantity"),
                            new CommandArgumentPart(true, "sourceunit"),
                            new CommandArgumentPart(true, "targetunit"),
                        }, new[]
                        {
                            new SwitchInfo("tui", /* Localizable */ "Use the TUI version of the unit converter", false, false, Array.Empty<string>(), 4, false)
                        })
                    }, new UnitConvCommand())
            },
            
            { "unset",
                new CommandInfo("unset", ShellType, /* Localizable */ "Removes a variable from the UESH variable list",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "$variable"),
                        }, new[]
                        {
                            new SwitchInfo("justwipe", /* Localizable */ "Just wipes the variable value without removing it", false, false, Array.Empty<string>(), 0, false)
                        })
                    }, new UnsetCommand())
            },
            
            { "unzip",
                new CommandInfo("unzip", ShellType, /* Localizable */ "Extracts a ZIP archive",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "zipfile"),
                            new CommandArgumentPart(false, "path"),
                        }, new[] {
                            new SwitchInfo("createdir", /* Localizable */ "Creates a directory that contains the contents of the ZIP file", false, false, Array.Empty<string>(), 0, false)
                        })
                    }, new UnZipCommand())
            },
            
            { "update",
                new CommandInfo("update", ShellType, /* Localizable */ "System update",
                    new[] {
                        new CommandArgumentInfo()
                    }, new UpdateCommand(), CommandFlags.Strict)
            },
            
            { "uptime",
                new CommandInfo("uptime", ShellType, /* Localizable */ "Shows the kernel uptime",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), Array.Empty<SwitchInfo>(), true)
                    }, new UptimeCommand())
            },
            
            { "usermanual",
                new CommandInfo("usermanual", ShellType, /* Localizable */ "Shows the two useful URLs for manual.",
                    new[] {
                        new CommandArgumentInfo()
                    }, new UserManualCommand())
            },
            
            { "verify",
                new CommandInfo("verify", ShellType, /* Localizable */ "Verifies sanity of the file",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "algorithm"),
                            new CommandArgumentPart(true, "calculatedhash"),
                            new CommandArgumentPart(true, "hashfile/expectedhash"),
                            new CommandArgumentPart(true, "file"),
                        }, Array.Empty<SwitchInfo>())
                    }, new VerifyCommand())
            },

            { "version",
                new CommandInfo("version", ShellType, /* Localizable */ "Gets the current version",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), new[]
                        {
                            new SwitchInfo("m", /* Localizable */ "Shows the kernel mod API version", false, false, new string[]{ "k" }, 0, false),
                            new SwitchInfo("k", /* Localizable */ "Shows the kernel version", false, false, new string[]{ "m" }, 0, false)
                        }, true)
                    }, new VersionCommand())
            },

            { "whoami",
                new CommandInfo("whoami", ShellType, /* Localizable */ "Gets the current user name",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), Array.Empty<SwitchInfo>(), true)
                    }, new WhoamiCommand())
            },
            
            { "wordle",
                new CommandInfo("wordle", ShellType, /* Localizable */ "The Wordle game simulator",
                    new[] {
                        new CommandArgumentInfo(Array.Empty<CommandArgumentPart>(), new[] {
                            new SwitchInfo("orig", /* Localizable */ "Play the Wordle game originally", false, false, Array.Empty<string>(), 0, false)
                        })
                    }, new WordleCommand())
            },

            { "zip",
                new CommandInfo("zip", ShellType, /* Localizable */ "Creates a ZIP archive",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "zipfile"),
                            new CommandArgumentPart(true, "path"),
                        }, new[] {
                            new SwitchInfo("fast", /* Localizable */ "Fast compression", false, false, new string[] { "nocomp" }, 0, false),
                            new SwitchInfo("nocomp", /* Localizable */ "No compression", false, false, new string[] { "fast" }, 0, false),
                            new SwitchInfo("nobasedir", /* Localizable */ "Don't create base directory in archive", false, false, Array.Empty<string>(), 0, false)
                        })
                    }, new ZipCommand())
            },

            // Hidden
            { "2015",
                new CommandInfo("2015", ShellType, /* Localizable */ "Starts the joke program, HDD Uncleaner 2015.",
                    new[] {
                        new CommandArgumentInfo()
                    }, new HddUncleanerCommand(), CommandFlags.Hidden)
            },
            
            { "2018",
                new CommandInfo("2018", ShellType, /* Localizable */ "Commemorates the 5-year anniversary of the kernel release",
                    new[] {
                        new CommandArgumentInfo()
                    }, new AnniversaryCommand(), CommandFlags.Hidden)
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

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["Shell"];
    }
}
