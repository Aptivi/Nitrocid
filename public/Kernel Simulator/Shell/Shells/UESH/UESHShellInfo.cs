
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
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
            { "adduser", new CommandInfo("adduser", ShellType.Shell, "Adds users", new CommandArgumentInfo(new[] { "<userName> [password] [confirm]" }, true, 1), new AddUserCommand(), CommandFlags.Strict) },
            { "admin", new CommandInfo("admin", ShellType.Shell, "Administrative shell", new CommandArgumentInfo(), new AdminCommand(), CommandFlags.Strict) },
            { "alias", new CommandInfo("alias", ShellType.Shell, "Adds aliases to commands", new CommandArgumentInfo( new[] { $"<rem/add> <{string.Join("/", Enum.GetNames(typeof(ShellType)))}> <alias> <cmd>" }, true, 3, (_) => HelpUnifiedCommand.ListCmds(_)), new AliasCommand(), CommandFlags.Strict) },
            { "archive", new CommandInfo("archive", ShellType.Shell, "Opens the archive file to the archive shell", new CommandArgumentInfo(new[] { "<archivefile>" }, true, 1), new ArchiveCommand()) },
            { "beep", new CommandInfo("beep", ShellType.Shell, "Beeps from the console", new CommandArgumentInfo(), new BeepCommand()) },
            { "blockdbgdev", new CommandInfo("blockdbgdev", ShellType.Shell, "Block a debug device by IP address", new CommandArgumentInfo(new[] { "<ipaddress>" }, true, 1), new BlockDbgDevCommand(), CommandFlags.Strict) },
            { "calc", new CommandInfo("calc", ShellType.Shell, "Calculator to calculate expressions.", new CommandArgumentInfo(new[] { "<expression>" }, true, 1), new CalcCommand()) },
            { "calendar", new CommandInfo("calendar", ShellType.Shell, "Calendar, event, and reminder manager", new CommandArgumentInfo(new[] { "<show> [year] [month]", "<event> <add> <date> <title>", "<event> <remove> <eventid>", "<event> <list>", "<event> <saveall>", "<reminder> <add> <dateandtime> <title>", "<reminder> <remove> <reminderid>", "<reminder> <list>", "<reminder> <saveall>" }, true, 1), new CalendarCommand()) },
            { "cat", new CommandInfo("cat", ShellType.Shell, "Prints content of file to console", new CommandArgumentInfo(new[] { "[-lines|-nolines|-plain] <file>" }, true, 1), new CatCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "chattr", new CommandInfo("chattr", ShellType.Shell, "Changes attribute of a file", new CommandArgumentInfo(new[] { "<file> +/-<attributes>" }, true, 2), new ChAttrCommand()) },
            { "chdir", new CommandInfo("chdir", ShellType.Shell, "Changes directory", new CommandArgumentInfo(new[] { "<directory/..>" }, true, 1), new ChDirCommand()) },
            { "chhostname", new CommandInfo("chhostname", ShellType.Shell, "Changes host name", new CommandArgumentInfo(new[] { "<HostName>" }, true, 1), new ChHostNameCommand(), CommandFlags.Strict) },
            { "chmal", new CommandInfo("chmal", ShellType.Shell, "Changes MAL, the MOTD After Login", new CommandArgumentInfo(new[] { "[Message]" }, false, 0), new ChMalCommand(), CommandFlags.Strict) },
            { "chmotd", new CommandInfo("chmotd", ShellType.Shell, "Changes MOTD, the Message Of The Day", new CommandArgumentInfo(new[] { "[Message]" }, false, 0), new ChMotdCommand(), CommandFlags.Strict) },
            { "choice", new CommandInfo("choice", ShellType.Shell, "Makes user choices", new CommandArgumentInfo(new[] { "[-o|-t|-m|-a] [-multiple|-single] <$variable> <answers> <input> [answertitle1] [answertitle2] ..." }, true, 3), new ChoiceCommand(), CommandFlags.SettingVariable) },
            { "chpwd", new CommandInfo("chpwd", ShellType.Shell, "Changes password for current user", new CommandArgumentInfo(new[] { "<Username> <UserPass> <newPass> <confirm>" }, true, 4, (_) => UserManagement.ListAllUsers().Where((src) => src.StartsWith(_)).ToArray()), new ChPwdCommand(), CommandFlags.Strict) },
            { "chusrname", new CommandInfo("chusrname", ShellType.Shell, "Changes user name", new CommandArgumentInfo(new[] { "<oldUserName> <newUserName>" }, true, 2, (_) => UserManagement.ListAllUsers().Where((src) => src.StartsWith(_)).ToArray()), new ChUsrNameCommand(), CommandFlags.Strict) },
            { "clearfiredevents", new CommandInfo("clearfiredevents", ShellType.Shell, "Clears all fired events", new CommandArgumentInfo(), new ClearFiredEventsCommand()) },
            { "cls", new CommandInfo("cls", ShellType.Shell, "Clears the screen", new CommandArgumentInfo(), new ClsCommand()) },
            { "colorhextorgb", new CommandInfo("colorhextorgb", ShellType.Shell, "Converts the hexadecimal representation of the color to RGB numbers.", new CommandArgumentInfo(new[] { "<#RRGGBB>" }, true, 1), new ColorHexToRgbCommand()) },
            { "colorhextorgbks", new CommandInfo("colorhextorgbks", ShellType.Shell, "Converts the hexadecimal representation of the color to RGB numbers in KS format.", new CommandArgumentInfo(new[] { "<#RRGGBB>" }, true, 1), new ColorHexToRgbKSCommand()) },
            { "colorrgbtohex", new CommandInfo("colorrgbtohex", ShellType.Shell, "Converts the color RGB numbers to hex.", new CommandArgumentInfo(new[] { "<R> <G> <B>" }, true, 3), new ColorRgbToHexCommand()) },
            { "combinestr", new CommandInfo("combinestr", ShellType.Shell, "Combines the two text files or more into the console.", new CommandArgumentInfo(new[] { "<input1> <input2> [input3] ..." }, true, 2), new CombineStrCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "combine", new CommandInfo("combine", ShellType.Shell, "Combines the two text files or more into the output file.", new CommandArgumentInfo(new[] { "<output> <input1> <input2> [input3] ..." }, true, 3), new CombineCommand()) },
            { "convertlineendings", new CommandInfo("convertlineendings", ShellType.Shell, "Converts the line endings to format for the current platform or to specified custom format", new CommandArgumentInfo(new[] { "<textfile> [-w|-u|-m]" }, true, 1), new ConvertLineEndingsCommand()) },
            { "copy", new CommandInfo("copy", ShellType.Shell, "Creates another copy of a file under different directory or name.", new CommandArgumentInfo(new[] { "<source> <target>" }, true, 2), new CopyCommand()) },
            { "dict", new CommandInfo("dict", ShellType.Shell, "The English Dictionary", new CommandArgumentInfo(new[] { "<word>" }, true, 1), new DictCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "dirinfo", new CommandInfo("dirinfo", ShellType.Shell, "Provides information about a directory", new CommandArgumentInfo(new[] { "<directory>" }, true, 1), new DirInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "disconndbgdev", new CommandInfo("disconndbgdev", ShellType.Shell, "Disconnect a debug device", new CommandArgumentInfo(new[] { "<ip>" }, true, 1), new DisconnDbgDevCommand(), CommandFlags.Strict) },
            { "dismissnotif", new CommandInfo("dismissnotif", ShellType.Shell, "Dismisses a notification", new CommandArgumentInfo(new[] { "<notificationNumber>" }, true, 1), new DismissNotifCommand()) },
            { "dismissnotifs", new CommandInfo("dismissnotifs", ShellType.Shell, "Dismisses all notifications", new CommandArgumentInfo(), new DismissNotifsCommand()) },
            { "echo", new CommandInfo("echo", ShellType.Shell, "Writes text into the console", new CommandArgumentInfo(new[] { "[text]" }, false, 0), new EchoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "edit", new CommandInfo("edit", ShellType.Shell, "Edits a file", new CommandArgumentInfo(new[] { "<file>" }, true, 1), new EditCommand()) },
            { "fileinfo", new CommandInfo("fileinfo", ShellType.Shell, "Provides information about a file", new CommandArgumentInfo(new[] { "<file>" }, true, 1), new FileInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "find", new CommandInfo("find", ShellType.Shell, "Finds a file in the specified directory or in the current directory", new CommandArgumentInfo(new[] { "<file> [directory]" }, true, 1), new FindCommand()) },
            { "ftp", new CommandInfo("ftp", ShellType.Shell, "Use an FTP shell to interact with servers", new CommandArgumentInfo(new[] { "<server>" }, false, 0), new FtpCommand()) },
            { "genname", new CommandInfo("genname", ShellType.Shell, "Name and surname generator", new CommandArgumentInfo(new[] { "[-t] [namescount] [nameprefix] [namesuffix] [surnameprefix] [surnamesuffix]" }, false, 0), new GenNameCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "gettimeinfo", new CommandInfo("gettimeinfo", ShellType.Shell, "Gets the date and time information", new CommandArgumentInfo(new[] { "<date>" }, true, 1), new GetTimeInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "get", new CommandInfo("get", ShellType.Shell, "Downloads a file to current working directory", new CommandArgumentInfo(new[] { "<URL>" }, true, 1), new Get_Command()) },
            { "http", new CommandInfo("http", ShellType.Shell, "Starts the HTTP shell", new CommandArgumentInfo(), new HttpCommand()) },
            { "hwinfo", new CommandInfo("hwinfo", ShellType.Shell, "Prints hardware information", new CommandArgumentInfo(new[] { "<HardwareType>" }, true, 1, (_) => new[] { "HDD", "LogicalParts", "CPU", "GPU", "Sound", "Network", "System", "Machine", "BIOS", "RAM", "all" }), new HwInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "if", new CommandInfo("if", ShellType.Shell, "Executes commands once the UESH expressions are satisfied", new CommandArgumentInfo(new[] { "<uesh-expression> <command>" }, true, 2), new IfCommand()) },
            { "ifm", new CommandInfo("ifm", ShellType.Shell, "Interactive system host file manager", new CommandArgumentInfo(), new IfmCommand()) },
            { "input", new CommandInfo("input", ShellType.Shell, "Allows user to enter input", new CommandArgumentInfo(new[] { "<$variable> <question>" }, true, 2), new InputCommand(), CommandFlags.SettingVariable) },
            { "jsonbeautify", new CommandInfo("jsonbeautify", ShellType.Shell, "Beautifies the JSON file", new CommandArgumentInfo(new[] { "<jsonfile> [output]" }, true, 1), new JsonBeautifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "jsonminify", new CommandInfo("jsonminify", ShellType.Shell, "Minifies the JSON file", new CommandArgumentInfo(new[] { "<jsonfile> [output]" }, true, 1), new JsonMinifyCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "keyinfo", new CommandInfo("keyinfo", ShellType.Shell, "Gets key information for a pressed key. Useful for debugging", new CommandArgumentInfo(), new KeyInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "langman", new CommandInfo("langman", ShellType.Shell, "Manage your languages", new CommandArgumentInfo(new[] { "<reload/load/unload> <customlanguagename>", "<list/reloadall>" }, true, 1, (_) => Languages.LanguageManager.CustomLanguages.Keys.Where((src) => src.StartsWith(_)).ToArray()), new LangManCommand(), CommandFlags.Strict) },
            { "license", new CommandInfo("license", ShellType.Shell, "Shows license information for the kernel", new CommandArgumentInfo(), new LicenseCommand()) },
            { "list", new CommandInfo("list", ShellType.Shell, "List file/folder contents in current folder", new CommandArgumentInfo(new[] { "[-showdetails|-suppressmessages|-recursive] [directory]" }, false, 0), new ListCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "listunits", new CommandInfo("listunits", ShellType.Shell, "Lists all available units", new CommandArgumentInfo(new[] { "<type>" }, true, 1, (_) => Quantity.Infos.Select((src) => src.Name).Where((src) => src.StartsWith(_)).ToArray()), new ListUnitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "lockscreen", new CommandInfo("lockscreen", ShellType.Shell, "Locks your screen with a password", new CommandArgumentInfo(), new LockScreenCommand()) },
            { "logout", new CommandInfo("logout", ShellType.Shell, "Logs you out", new CommandArgumentInfo(), new LogoutCommand(), CommandFlags.NoMaintenance) },
            { "lsdbgdev", new CommandInfo("lsdbgdev", ShellType.Shell, "Lists debugging devices connected", new CommandArgumentInfo(), new LsDbgDevCommand(), CommandFlags.Strict | CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "lsvars", new CommandInfo("lsvars", ShellType.Shell, "Lists available UESH variables", new CommandArgumentInfo(), new LsVarsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "mail", new CommandInfo("mail", ShellType.Shell, "Opens the mail client", new CommandArgumentInfo(new[] { "[emailAddress]" }, false, 0), new MailCommand()) },
            { "md", new CommandInfo("md", ShellType.Shell, "Creates a directory", new CommandArgumentInfo(new[] { "<directory>" }, true, 1), new MdCommand()) },
            { "meteor", new CommandInfo("meteor", ShellType.Shell, "You are a spaceship and the meteors are coming to destroy you. Can you save it?", new CommandArgumentInfo(), new MeteorCommand()) },
            { "mkfile", new CommandInfo("mkfile", ShellType.Shell, "Makes a new file", new CommandArgumentInfo(new[] { "<file>" }, true, 1), new MkFileCommand()) },
            { "mktheme", new CommandInfo("mktheme", ShellType.Shell, "Makes a new theme", new CommandArgumentInfo(new[] { "<themeName>" }, true, 1), new MkThemeCommand()) },
            { "modman", new CommandInfo("modman", ShellType.Shell, "Manage your mods", new CommandArgumentInfo(new[] { "<start/stop/info/reload/install/uninstall> <modfilename>", "<list/listparts> [modname]", "<reloadall/stopall/startall>" }, true, 1), new ModManCommand(), CommandFlags.Strict) },
            { "modmanual", new CommandInfo("modmanual", ShellType.Shell, "Mod manual", new CommandArgumentInfo(new[] { "[-list] <ManualTitle>" }, true, 1), new ModManualCommand()) },
            { "move", new CommandInfo("move", ShellType.Shell, "Moves a file to another directory", new CommandArgumentInfo(new[] { "<source> <target>" }, true, 2), new MoveCommand()) },
            { "open", new CommandInfo("open", ShellType.Shell, "Opens a URL", new CommandArgumentInfo(new[] { "<URL>" }, true, 1), new OpenCommand()) },
            { "perm", new CommandInfo("perm", ShellType.Shell, "Manage permissions for users", new CommandArgumentInfo(new[] { "<userName> <Administrator/Disabled/Anonymous> <Allow/Disallow>" }, true, 3, (_) => UserManagement.ListAllUsers().Where((src) => src.StartsWith(_)).ToArray()), new PermCommand(), CommandFlags.Strict) },
            { "ping", new CommandInfo("ping", ShellType.Shell, "Pings an address", new CommandArgumentInfo(new[] { "[times] <Address1> <Address2> ..." }, true, 1), new PingCommand()) },
            { "previewsplash", new CommandInfo("previewsplash", ShellType.Shell, "Previews the splash", new CommandArgumentInfo(new[] { "[splashName]" }, false, 0, (_) => SplashManager.Splashes.Keys.Where((src) => src.StartsWith(_)).ToArray()), new PreviewSplashCommand()) },
            { "put", new CommandInfo("put", ShellType.Shell, "Uploads a file to specified website", new CommandArgumentInfo(new[] { "<FileName> <URL>" }, true, 2), new PutCommand()) },
            { "reboot", new CommandInfo("reboot", ShellType.Shell, "Restarts your computer (WARNING: No syncing, because it is not a final kernel)", new CommandArgumentInfo(new[] { "[ip] [port]" }, false, 0), new RebootCommand()) },
            { "reloadconfig", new CommandInfo("reloadconfig", ShellType.Shell, "Reloads configuration file that is edited.", new CommandArgumentInfo(), new ReloadConfigCommand(), CommandFlags.Strict) },
            { "reloadsaver", new CommandInfo("reloadsaver", ShellType.Shell, "Reloads screensaver file in KSMods", new CommandArgumentInfo(new[] { "<customsaver>" }, true, 1), new ReloadSaverCommand(), CommandFlags.Strict) },
            { "retroks", new CommandInfo("retroks", ShellType.Shell, "Retro Kernel Simulator based on 0.0.4.1", new CommandArgumentInfo(), new RetroKSCommand()) },
            { "rexec", new CommandInfo("rexec", ShellType.Shell, "Remotely executes a command to remote PC", new CommandArgumentInfo(new[] { "<address> [port] <command>" }, true, 2), new RexecCommand(), CommandFlags.Strict) },
            { "rm", new CommandInfo("rm", ShellType.Shell, "Removes a directory or a file", new CommandArgumentInfo(new[] { "<directory/file>" }, true, 1), new RmCommand()) },
            { "rdebug", new CommandInfo("rdebug", ShellType.Shell, "Enables or disables remote debugging.", new CommandArgumentInfo(), new RdebugCommand(), CommandFlags.Strict) },
            { "reportbug", new CommandInfo("reportbug", ShellType.Shell, "A bug reporting prompt.", new CommandArgumentInfo(), new ReportBugCommand()) },
            { "rmuser", new CommandInfo("rmuser", ShellType.Shell, "Removes a user from the list", new CommandArgumentInfo(new[] { "<Username>" }, true, 1, (_) => UserManagement.ListAllUsers().Where((src) => src.StartsWith(_)).ToArray()), new RmUserCommand(), CommandFlags.Strict) },
            { "roulette", new CommandInfo("roulette", ShellType.Shell, "Russian Roulette", new CommandArgumentInfo(), new RouletteCommand()) },
            { "rss", new CommandInfo("rss", ShellType.Shell, "Opens an RSS shell to read the feeds", new CommandArgumentInfo(new[] { "[feedlink]" }, false, 0), new RssCommand()) },
            { "savecurrdir", new CommandInfo("savecurrdir", ShellType.Shell, "Saves the current directory to kernel configuration file", new CommandArgumentInfo(), new SaveCurrDirCommand(), CommandFlags.Strict) },
            { "savescreen", new CommandInfo("savescreen", ShellType.Shell, "Saves your screen from burn outs", new CommandArgumentInfo(new[] { "[saver]" }, false, 0, (_) => Screensaver.Screensavers.Keys.Where((src) => src.StartsWith(_)).ToArray()), new SaveScreenCommand()) },
            { "search", new CommandInfo("search", ShellType.Shell, "Searches for specified string in the provided file using regular expressions", new CommandArgumentInfo(new[] { "<Regexp> <File>" }, true, 2), new SearchCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "searchword", new CommandInfo("searchword", ShellType.Shell, "Searches for specified string in the provided file", new CommandArgumentInfo(new[] { "<StringEnclosedInDoubleQuotes> <File>" }, true, 2), new SearchWordCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "select", new CommandInfo("select", ShellType.Shell, "Provides a selection choice", new CommandArgumentInfo(new[] { "<$variable> <answers> <input> [answertitle1] [answertitle2] ..." }, true, 3), new SelectCommand(), CommandFlags.SettingVariable) },
            { "setsaver", new CommandInfo("setsaver", ShellType.Shell, "Sets up kernel screensavers", new CommandArgumentInfo(new[] { "<customsaver/builtinsaver>" }, true, 1), new SetSaverCommand(), CommandFlags.Strict) },
            { "settings", new CommandInfo("settings", ShellType.Shell, "Changes kernel configuration", new CommandArgumentInfo(new[] { "[-saver|-splash]" }, false, 0), new SettingsCommand(), CommandFlags.Strict) },
            { "set", new CommandInfo("set", ShellType.Shell, "Sets a variable to a value in a script", new CommandArgumentInfo(new[] { "<$variable> <value>" }, true, 2), new SetCommand(), CommandFlags.SettingVariable) },
            { "setrange", new CommandInfo("setrange", ShellType.Shell, "Creates a variable array with the provided values", new CommandArgumentInfo(new[] { "<$variablename> <value1> [value2] [value3] ..." }, true, 2), new SetRangeCommand(), CommandFlags.SettingVariable) },
            { "sftp", new CommandInfo("sftp", ShellType.Shell, "Lets you use an SSH FTP server", new CommandArgumentInfo(new[] { "<server>" }, false, 0), new SftpCommand()) },
            { "shownotifs", new CommandInfo("shownotifs", ShellType.Shell, "Shows all received notifications", new CommandArgumentInfo(), new ShowNotifsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "showtd", new CommandInfo("showtd", ShellType.Shell, "Shows date and time", new CommandArgumentInfo(), new ShowTdCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "showtdzone", new CommandInfo("showtdzone", ShellType.Shell, "Shows date and time in zones", new CommandArgumentInfo(new[] { "[-all] <timezone>" }, true, 1), new ShowTdZoneCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable) },
            { "shutdown", new CommandInfo("shutdown", ShellType.Shell, "The kernel will be shut down", new CommandArgumentInfo(new[] { "[ip] [port]" }, false, 0), new ShutdownCommand()) },
            { "snaker", new CommandInfo("snaker", ShellType.Shell, "The snake game!", new CommandArgumentInfo(), new SnakerCommand()) },
            { "solver", new CommandInfo("solver", ShellType.Shell, "See if you can solve mathematical equations on time", new CommandArgumentInfo(), new SolverCommand()) },
            { "speedpress", new CommandInfo("speedpress", ShellType.Shell, "See if you can press a key on time", new CommandArgumentInfo(new[] { "[-e|-m|-h|-v|-c] [timeout]" }, false, 0), new SpeedPressCommand()) },
            { "sshell", new CommandInfo("sshell", ShellType.Shell, "Connects to an SSH server.", new CommandArgumentInfo(new[] { "<address:port> <username>" }, true, 2), new SshellCommand()) },
            { "sshcmd", new CommandInfo("sshcmd", ShellType.Shell, "Connects to an SSH server to execute a command.", new CommandArgumentInfo(new[] { "<address:port> <username> \"<command>\"" }, true, 3), new SshcmdCommand()) },
            { "stopwatch", new CommandInfo("stopwatch", ShellType.Shell, "A simple stopwatch", new CommandArgumentInfo(), new StopwatchCommand()) },
            { "sumfile", new CommandInfo("sumfile", ShellType.Shell, "Calculates file sums.", new CommandArgumentInfo(new[] { "[-relative] <MD5/SHA1/SHA256/SHA384/SHA512/all> <file> [outputFile]" }, true, 2), new SumFileCommand()) },
            { "sumfiles", new CommandInfo("sumfiles", ShellType.Shell, "Calculates sums of files in specified directory.", new CommandArgumentInfo(new[] { "[-relative] <MD5/SHA1/SHA256/SHA384/SHA512/all> <dir> [outputFile]" }, true, 2), new SumFilesCommand()) },
            { "themesel", new CommandInfo("themesel", ShellType.Shell, "Selects a theme and sets it", new CommandArgumentInfo(new[] { "[Theme]" }, false, 0, (_) => ThemeTools.Themes.Keys.Where((src) => src.StartsWith(_)).ToArray()), new ThemeSelCommand()) },
            { "timer", new CommandInfo("timer", ShellType.Shell, "A simple timer", new CommandArgumentInfo(), new TimerCommand()) },
            { "unblockdbgdev", new CommandInfo("unblockdbgdev", ShellType.Shell, "Unblock a debug device by IP address", new CommandArgumentInfo(new[] { "<ipaddress>" }, true, 1), new UnblockDbgDevCommand(), CommandFlags.Strict) },
            { "unitconv", new CommandInfo("unitconv", ShellType.Shell, "Unit converter", new CommandArgumentInfo(new[] { "<unittype> <quantity> <sourceunit> <targetunit>" }, true, 4), new UnitConvCommand()) },
            { "unzip", new CommandInfo("unzip", ShellType.Shell, "Extracts a ZIP archive", new CommandArgumentInfo(new[] { "<zipfile> [path] [-createdir]" }, true, 1), new UnZipCommand()) },
            { "update", new CommandInfo("update", ShellType.Shell, "System update", new CommandArgumentInfo(), new UpdateCommand(), CommandFlags.Strict) },
            { "usermanual", new CommandInfo("usermanual", ShellType.Shell, "Takes you to our GitHub Wiki.", new CommandArgumentInfo(new[] { "[-modapi]" }, false, 0), new UserManualCommand()) },
            { "verify", new CommandInfo("verify", ShellType.Shell, "Verifies sanity of the file", new CommandArgumentInfo(new[] { "<MD5/SHA1/SHA256/SHA384/SHA512> <calculatedhash> <hashfile/expectedhash> <file>" }, true, 4), new VerifyCommand()) },
            { "weather", new CommandInfo("weather", ShellType.Shell, "Shows weather info for specified city. Uses OpenWeatherMap.", new CommandArgumentInfo(new[] { "[-list] <CityID/CityName> [apikey]" }, true, 1), new WeatherCommand()) },
            { "wrap", new CommandInfo("wrap", ShellType.Shell, "Wraps the console output", new CommandArgumentInfo(new[] { "<command>" }, true, 1), new WrapCommand()) },
            { "zip", new CommandInfo("zip", ShellType.Shell, "Creates a ZIP archive", new CommandArgumentInfo(new[] { "<zipfile> <path> [-fast|-nocomp|-nobasedir]" }, true, 2), new ZipCommand()) }
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
