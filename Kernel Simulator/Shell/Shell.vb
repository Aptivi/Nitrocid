
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports System.IO
Imports System.Text.RegularExpressions
Imports KS.Files.PathLookup
Imports KS.Files.Querying
Imports KS.Misc.Execution
Imports KS.Modifications
Imports KS.Scripting
Imports KS.Shell.Commands
Imports KS.Shell.ShellBase.Aliases
Imports KS.Shell.UnifiedCommands

Namespace Shell
    Public Module Shell

        Friend ProcessStartCommandThread As New KernelThread("Executable Command Thread", False, AddressOf ExecuteProcess)
        Friend ReadOnly ModCommands As New Dictionary(Of String, CommandInfo)
        Friend ReadOnly InjectedCommands As New List(Of String)

        ''' <summary>
        ''' Whether the shell is colored or not
        ''' </summary>
        Public ColoredShell As Boolean = True
        ''' <summary>
        ''' Shell prompt style
        ''' </summary>
        Public ShellPromptStyle As String = ""
        ''' <summary>
        ''' Specifies where to lookup for executables in these paths. Same as in PATH implementation.
        ''' </summary>
        Public PathsToLookup As String = Environment.GetEnvironmentVariable("PATH")
        ''' <summary>
        ''' Path lookup delimiter, depending on the operating system
        ''' </summary>
        Public ReadOnly PathLookupDelimiter As String = Path.PathSeparator
        ''' <summary>
        ''' List of commands
        ''' </summary>
        Public ReadOnly Commands As New Dictionary(Of String, CommandInfo) From {
            {"adduser", New CommandInfo("adduser", ShellType.Shell, "Adds users", New CommandArgumentInfo({"<userName> [password] [confirm]"}, True, 1), New AddUserCommand, True)},
            {"alias", New CommandInfo("alias", ShellType.Shell, "Adds aliases to commands", New CommandArgumentInfo({$"<rem/add> <{String.Join("/", [Enum].GetNames(GetType(ShellType)))}> <alias> <cmd>"}, True, 3), New AliasCommand, True)},
            {"arginj", New CommandInfo("arginj", ShellType.Shell, "Injects arguments to the kernel (reboot required)", New CommandArgumentInfo({"[Arguments separated by spaces]"}, True, 1), New ArgInjCommand, True, False, False, False, False)},
            {"beep", New CommandInfo("beep", ShellType.Shell, "Beep in 'n' Hz and time in 'n' milliseconds", New CommandArgumentInfo({"<37-32767 Hz> <milliseconds>"}, True, 2), New BeepCommand)},
            {"blockdbgdev", New CommandInfo("blockdbgdev", ShellType.Shell, "Block a debug device by IP address", New CommandArgumentInfo({"<ipaddress>"}, True, 1), New BlockDbgDevCommand, True)},
            {"calc", New CommandInfo("calc", ShellType.Shell, "Calculator to calculate expressions.", New CommandArgumentInfo({"<expression>"}, True, 1), New CalcCommand)},
            {"calendar", New CommandInfo("calendar", ShellType.Shell, "Calendar, event, and reminder manager", New CommandArgumentInfo({"<show> [year] [month]", "<event> <add> <date> <title>", "<event> <remove> <eventid>", "<event> <list>", "<event> <saveall>", "<reminder> <add> <dateandtime> <title>", "<reminder> <remove> <reminderid>", "<reminder> <list>", "<reminder> <saveall>"}, True, 1), New CalendarCommand)},
            {"cat", New CommandInfo("cat", ShellType.Shell, "Prints content of file to console", New CommandArgumentInfo({"[-lines|-nolines] <file>"}, True, 1), New CatCommand, False, True, False, False, False)},
            {"cdbglog", New CommandInfo("cdbglog", ShellType.Shell, "Deletes everything in debug log", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New CdbgLogCommand, True)},
            {"chattr", New CommandInfo("chattr", ShellType.Shell, "Changes attribute of a file", New CommandArgumentInfo({"<file> +/-<attributes>"}, True, 2), New ChAttrCommand, False, False, False, False, False)},
            {"chdir", New CommandInfo("chdir", ShellType.Shell, "Changes directory", New CommandArgumentInfo({"<directory/..>"}, True, 1), New ChDirCommand)},
            {"chhostname", New CommandInfo("chhostname", ShellType.Shell, "Changes host name", New CommandArgumentInfo({"<HostName>"}, True, 1), New ChHostNameCommand, True)},
            {"chlang", New CommandInfo("chlang", ShellType.Shell, "Changes language", New CommandArgumentInfo({"[-alwaystransliterated|-alwaystranslated|-force] <language>"}, True, 1), New ChLangCommand, True, False, False, False, False)},
            {"chmal", New CommandInfo("chmal", ShellType.Shell, "Changes MAL, the MOTD After Login", New CommandArgumentInfo({"[Message]"}, False, 0), New ChMalCommand, True)},
            {"chmotd", New CommandInfo("chmotd", ShellType.Shell, "Changes MOTD, the Message Of The Day", New CommandArgumentInfo({"[Message]"}, False, 0), New ChMotdCommand, True)},
            {"choice", New CommandInfo("choice", ShellType.Shell, "Makes user choices", New CommandArgumentInfo({"[-o|-t|-m|-a] [-multiple|-single] <$variable> <answers> <input> [answertitle1] [answertitle2] ..."}, True, 3), New ChoiceCommand, False, False, False, False, True)},
            {"chpwd", New CommandInfo("chpwd", ShellType.Shell, "Changes password for current user", New CommandArgumentInfo({"<Username> <UserPass> <newPass> <confirm>"}, True, 4), New ChPwdCommand, True)},
            {"chusrname", New CommandInfo("chusrname", ShellType.Shell, "Changes user name", New CommandArgumentInfo({"<oldUserName> <newUserName>"}, True, 2), New ChUsrNameCommand, True)},
            {"clearfiredevents", New CommandInfo("clearfiredevents", ShellType.Shell, "Clears all fired events", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New ClearFiredEventsCommand)},
            {"cls", New CommandInfo("cls", ShellType.Shell, "Clears the screen", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New ClsCommand)},
            {"colorhextorgb", New CommandInfo("colorhextorgb", ShellType.Shell, "Converts the hexadecimal representation of the color to RGB numbers.", New CommandArgumentInfo({"<#RRGGBB>"}, True, 1), New ColorHexToRgbCommand)},
            {"colorhextorgbks", New CommandInfo("colorhextorgbks", ShellType.Shell, "Converts the hexadecimal representation of the color to RGB numbers in KS format.", New CommandArgumentInfo({"<#RRGGBB>"}, True, 1), New ColorHexToRgbKSCommand)},
            {"colorrgbtohex", New CommandInfo("colorrgbtohex", ShellType.Shell, "Converts the color RGB numbers to hex.", New CommandArgumentInfo({"<R> <G> <B>"}, True, 3), New ColorRgbToHexCommand)},
            {"combine", New CommandInfo("combine", ShellType.Shell, "Combines the two text files or more into the output file.", New CommandArgumentInfo({"<output> <input1> <input2> [input3] ..."}, True, 3), New CombineCommand)},
            {"convertlineendings", New CommandInfo("convertlineendings", ShellType.Shell, "Converts the line endings to format for the current platform or to specified custom format", New CommandArgumentInfo({"<textfile> [-w|-u|-m]"}, True, 1), New ConvertLineEndingsCommand)},
            {"copy", New CommandInfo("copy", ShellType.Shell, "Creates another copy of a file under different directory or name.", New CommandArgumentInfo({"<source> <target>"}, True, 2), New CopyCommand)},
            {"dict", New CommandInfo("dict", ShellType.Shell, "The English Dictionary", New CommandArgumentInfo({"<word>"}, True, 1), New DictCommand)},
            {"dirinfo", New CommandInfo("dirinfo", ShellType.Shell, "Provides information about a directory", New CommandArgumentInfo({"<directory>"}, True, 1), New DirInfoCommand)},
            {"disconndbgdev", New CommandInfo("disconndbgdev", ShellType.Shell, "Disconnect a debug device", New CommandArgumentInfo({"<ip>"}, True, 1), New DisconnDbgDevCommand, True)},
            {"dismissnotif", New CommandInfo("dismissnotif", ShellType.Shell, "Dismisses a notification", New CommandArgumentInfo({"<notificationNumber>"}, True, 1), New DismissNotifCommand)},
            {"dismissnotifs", New CommandInfo("dismissnotifs", ShellType.Shell, "Dismisses all notifications", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New DismissNotifsCommand)},
            {"echo", New CommandInfo("echo", ShellType.Shell, "Writes text into the console", New CommandArgumentInfo({"[text]"}, False, 0), New EchoCommand)},
            {"edit", New CommandInfo("edit", ShellType.Shell, "Edits a text file", New CommandArgumentInfo({"<file>"}, True, 1), New EditCommand)},
            {"exit", New CommandInfo("exit", ShellType.Shell, "Exits the shell if running on subshell", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New ExitCommand)},
            {"fileinfo", New CommandInfo("fileinfo", ShellType.Shell, "Provides information about a file", New CommandArgumentInfo({"<file>"}, True, 1), New FileInfoCommand)},
            {"find", New CommandInfo("find", ShellType.Shell, "Finds a file in the specified directory or in the current directory", New CommandArgumentInfo({"<file> [directory]"}, True, 1), New FindCommand)},
            {"firedevents", New CommandInfo("firedevents", ShellType.Shell, "Lists all fired events", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New FiredEventsCommand)},
            {"ftp", New CommandInfo("ftp", ShellType.Shell, "Use an FTP shell to interact with servers", New CommandArgumentInfo({"<server>"}, False, 0), New FtpCommand)},
            {"genname", New CommandInfo("genname", ShellType.Shell, "Name and surname generator", New CommandArgumentInfo({"[namescount] [nameprefix] [namesuffix] [surnameprefix] [surnamesuffix]"}, False, 0), New GenNameCommand)},
            {"gettimeinfo", New CommandInfo("gettimeinfo", ShellType.Shell, "Gets the date and time information", New CommandArgumentInfo({"<date>"}, True, 1), New GetTimeInfoCommand)},
            {"get", New CommandInfo("get", ShellType.Shell, "Downloads a file to current working directory", New CommandArgumentInfo({"<URL>"}, True, 1), New Get_Command)},
            {"help", New CommandInfo("help", ShellType.Shell, "Help page", New CommandArgumentInfo({"[command]"}, False, 0, AddressOf HelpCommand.ListCmds), New HelpCommand)},
            {"hexedit", New CommandInfo("hexedit", ShellType.Shell, "Edits a binary file", New CommandArgumentInfo({"<file>"}, True, 1), New HexEditCommand)},
            {"http", New CommandInfo("http", ShellType.Shell, "Starts the HTTP shell", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New HttpCommand)},
            {"hwinfo", New CommandInfo("hwinfo", ShellType.Shell, "Prints hardware information", New CommandArgumentInfo({"<HardwareType>"}, True, 1), New HwInfoCommand, False, True, False, False, False)},
            {"if", New CommandInfo("if", ShellType.Shell, "Executes commands once the UESH expressions are satisfied", New CommandArgumentInfo({"<uesh-expression> <command>"}, True, 2), New IfCommand)},
            {"input", New CommandInfo("input", ShellType.Shell, "Allows user to enter input", New CommandArgumentInfo({"<$variable> <question>"}, True, 2), New InputCommand, False, False, False, False, True)},
            {"jsonbeautify", New CommandInfo("jsonbeautify", ShellType.Shell, "Beautifies the JSON file", New CommandArgumentInfo({"<jsonfile> [output]"}, True, 1), New JsonBeautifyCommand, False, True)},
            {"jsonminify", New CommandInfo("jsonminify", ShellType.Shell, "Minifies the JSON file", New CommandArgumentInfo({"<jsonfile> [output]"}, True, 1), New JsonMinifyCommand, False, True)},
            {"jsonshell", New CommandInfo("jsonshell", ShellType.Shell, "Opens the JSON shell", New CommandArgumentInfo({"<jsonfile>"}, True, 1), New JsonShellCommand)},
            {"keyinfo", New CommandInfo("keyinfo", ShellType.Shell, "Gets key information for a pressed key. Useful for debugging", New CommandArgumentInfo({""}, False, 0), New KeyInfoCommand)},
            {"langman", New CommandInfo("langman", ShellType.Shell, "Manage your languages", New CommandArgumentInfo({"<reload/load/unload> <customlanguagename>", "<list/reloadall>"}, True, 1), New LangManCommand, True)},
            {"list", New CommandInfo("list", ShellType.Shell, "List file/folder contents in current folder", New CommandArgumentInfo({"[-showdetails|-suppressmessages] [directory]"}, False, 0), New ListCommand, False, True, False, False, False)},
            {"lockscreen", New CommandInfo("lockscreen", ShellType.Shell, "Locks your screen with a password", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New LockScreenCommand)},
            {"logout", New CommandInfo("logout", ShellType.Shell, "Logs you out", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New LogoutCommand, False, False, True)},
            {"lovehate", New CommandInfo("lovehate", ShellType.Shell, "Respond to love or hate comments.", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New LoveHateCommand)},
            {"lsdbgdev", New CommandInfo("lsdbgdev", ShellType.Shell, "Lists debugging devices connected", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New LsDbgDevCommand, True, True)},
            {"lsvars", New CommandInfo("lsvars", ShellType.Shell, "Lists available UESH variables", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New LsVarsCommand, False, True)},
            {"mail", New CommandInfo("mail", ShellType.Shell, "Opens the mail client", New CommandArgumentInfo({"[emailAddress]"}, False, 0), New MailCommand)},
            {"md", New CommandInfo("md", ShellType.Shell, "Creates a directory", New CommandArgumentInfo({"<directory>"}, True, 1), New MdCommand)},
            {"meteor", New CommandInfo("meteor", ShellType.Shell, "You are a spaceship and the meteors are coming to destroy you. Can you save it?", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New MeteorCommand)},
            {"mkfile", New CommandInfo("mkfile", ShellType.Shell, "Makes a new file", New CommandArgumentInfo({"<file>"}, True, 1), New MkFileCommand)},
            {"mktheme", New CommandInfo("mktheme", ShellType.Shell, "Makes a new theme", New CommandArgumentInfo({"<themeName>"}, True, 1), New MkThemeCommand)},
            {"modman", New CommandInfo("modman", ShellType.Shell, "Manage your mods", New CommandArgumentInfo({"<start/stop/info/reload/install/uninstall> <modfilename>", "<list/listparts> [modname]", "<reloadall/stopall/startall>"}, True, 1), New ModManCommand, True)},
            {"modmanual", New CommandInfo("modmanual", ShellType.Shell, "Mod manual", New CommandArgumentInfo({"[-list] <ManualTitle>"}, True, 1), New ModManualCommand)},
            {"move", New CommandInfo("move", ShellType.Shell, "Moves a file to another directory", New CommandArgumentInfo({"<source> <target>"}, True, 2), New MoveCommand)},
            {"netinfo", New CommandInfo("netinfo", ShellType.Shell, "Lists information about all available interfaces", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New NetInfoCommand, True, True)},
            {"open", New CommandInfo("open", ShellType.Shell, "Opens a URL", New CommandArgumentInfo({"<URL>"}, True, 1), New OpenCommand)},
            {"perm", New CommandInfo("perm", ShellType.Shell, "Manage permissions for users", New CommandArgumentInfo({"<userName> <Administrator/Disabled/Anonymous> <Allow/Disallow>"}, True, 3), New PermCommand, True)},
            {"ping", New CommandInfo("ping", ShellType.Shell, "Pings an address", New CommandArgumentInfo({"[times] <Address1> <Address2> ..."}, True, 1), New PingCommand)},
            {"put", New CommandInfo("put", ShellType.Shell, "Uploads a file to specified website", New CommandArgumentInfo({"<FileName> <URL>"}, True, 2), New PutCommand)},
            {"rarshell", New CommandInfo("rarshell", ShellType.Shell, "The RAR shell", New CommandArgumentInfo({"<rarfile>"}, True, 1), New RarShellCommand)},
            {"reboot", New CommandInfo("reboot", ShellType.Shell, "Restarts your computer (WARNING: No syncing, because it is not a final kernel)", New CommandArgumentInfo({"[ip] [port]"}, False, 0), New RebootCommand)},
            {"reloadconfig", New CommandInfo("reloadconfig", ShellType.Shell, "Reloads configuration file that is edited.", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New ReloadConfigCommand, True, False, False, False, True)},
            {"reloadsaver", New CommandInfo("reloadsaver", ShellType.Shell, "Reloads screensaver file in KSMods", New CommandArgumentInfo({"<customsaver>"}, True, 1), New ReloadSaverCommand, True, False, False, False, True)},
            {"retroks", New CommandInfo("retroks", ShellType.Shell, "Retro Kernel Simulator based on 0.0.4.1", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RetroKSCommand)},
            {"rexec", New CommandInfo("rexec", ShellType.Shell, "Remotely executes a command to remote PC", New CommandArgumentInfo({"<address> [port] <command>"}, True, 2), New RexecCommand, True)},
            {"rm", New CommandInfo("rm", ShellType.Shell, "Removes a directory or a file", New CommandArgumentInfo({"<directory/file>"}, True, 1), New RmCommand)},
            {"rdebug", New CommandInfo("rdebug", ShellType.Shell, "Enables or disables remote debugging.", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New RdebugCommand, True)},
            {"reportbug", New CommandInfo("reportbug", ShellType.Shell, "A bug reporting prompt.", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New ReportBugCommand)},
            {"rmuser", New CommandInfo("rmuser", ShellType.Shell, "Removes a user from the list", New CommandArgumentInfo({"<Username>"}, True, 1), New RmUserCommand, True)},
            {"rss", New CommandInfo("rss", ShellType.Shell, "Opens an RSS shell to read the feeds", New CommandArgumentInfo({"[feedlink]"}, False, 0), New RssCommand)},
            {"savecurrdir", New CommandInfo("savecurrdir", ShellType.Shell, "Saves the current directory to kernel configuration file", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New SaveCurrDirCommand, True)},
            {"savescreen", New CommandInfo("savescreen", ShellType.Shell, "Saves your screen from burn outs", New CommandArgumentInfo({"[saver]"}, False, 0), New SaveScreenCommand)},
            {"search", New CommandInfo("search", ShellType.Shell, "Searches for specified string in the provided file using regular expressions", New CommandArgumentInfo({"<Regexp> <File>"}, True, 2), New SearchCommand)},
            {"searchword", New CommandInfo("searchword", ShellType.Shell, "Searches for specified string in the provided file", New CommandArgumentInfo({"<StringEnclosedInDoubleQuotes> <File>"}, True, 2), New SearchWordCommand)},
            {"select", New CommandInfo("select", ShellType.Shell, "Provides a selection choice", New CommandArgumentInfo({"<$variable> <answers> <input> [answertitle1] [answertitle2] ..."}, True, 3), New SelectCommand, False, False, False, False, True)},
            {"setsaver", New CommandInfo("setsaver", ShellType.Shell, "Sets up kernel screensavers", New CommandArgumentInfo({"<customsaver/builtinsaver>"}, True, 1), New SetSaverCommand, True, False, False, False, False)},
            {"setthemes", New CommandInfo("setthemes", ShellType.Shell, "Sets up kernel themes", New CommandArgumentInfo({"<Theme>"}, True, 1), New SetThemesCommand, False, False, False, False, False)},
            {"settings", New CommandInfo("settings", ShellType.Shell, "Changes kernel configuration", New CommandArgumentInfo({"[-saver|-splash]"}, False, 0), New SettingsCommand, True, False, False, False, False)},
            {"set", New CommandInfo("set", ShellType.Shell, "Sets a variable to a value in a script", New CommandArgumentInfo({"<$variable> <value>"}, True, 2), New SetCommand, False, False, False, False, True)},
            {"setrange", New CommandInfo("setrange", ShellType.Shell, "Creates a variable array with the provided values", New CommandArgumentInfo({"<$variablename> <value1> [value2] [value3] ..."}, True, 2), New SetRangeCommand, False, False, False, False, True)},
            {"sftp", New CommandInfo("sftp", ShellType.Shell, "Lets you use an SSH FTP server", New CommandArgumentInfo({"<server>"}, False, 0), New SftpCommand)},
            {"shownotifs", New CommandInfo("shownotifs", ShellType.Shell, "Shows all received notifications", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New ShowNotifsCommand)},
            {"showtd", New CommandInfo("showtd", ShellType.Shell, "Shows date and time", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New ShowTdCommand)},
            {"showtdzone", New CommandInfo("showtdzone", ShellType.Shell, "Shows date and time in zones", New CommandArgumentInfo({"[-all] <timezone>"}, True, 1), New ShowTdZoneCommand, False, True, False, False, False)},
            {"shutdown", New CommandInfo("shutdown", ShellType.Shell, "The kernel will be shut down", New CommandArgumentInfo({"[ip] [port]"}, False, 0), New ShutdownCommand)},
            {"snaker", New CommandInfo("snaker", ShellType.Shell, "The snake game!", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New SnakerCommand)},
            {"solver", New CommandInfo("solver", ShellType.Shell, "See if you can solve mathematical equations on time", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New SolverCommand)},
            {"speedpress", New CommandInfo("speedpress", ShellType.Shell, "See if you can press a key on time", New CommandArgumentInfo({"[-e|-m|-h|-v|-c] [timeout]"}, False, 0), New SpeedPressCommand, False, False, False, False, False)},
            {"spellbee", New CommandInfo("spellbee", ShellType.Shell, "See if you can spell words correctly on time", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New SpellBeeCommand)},
            {"sshell", New CommandInfo("sshell", ShellType.Shell, "Connects to an SSH server.", New CommandArgumentInfo({"<address:port> <username>"}, True, 2), New SshellCommand)},
            {"sshcmd", New CommandInfo("sshcmd", ShellType.Shell, "Connects to an SSH server to execute a command.", New CommandArgumentInfo({"<address:port> <username> ""<command>"""}, True, 3), New SshcmdCommand)},
            {"stopwatch", New CommandInfo("stopwatch", ShellType.Shell, "A simple stopwatch", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New StopwatchCommand)},
            {"sumfile", New CommandInfo("sumfile", ShellType.Shell, "Calculates file sums.", New CommandArgumentInfo({"[-relative] <MD5/SHA1/SHA256/SHA384/SHA512/all> <file> [outputFile]"}, True, 2), New SumFileCommand)},
            {"sumfiles", New CommandInfo("sumfiles", ShellType.Shell, "Calculates sums of files in specified directory.", New CommandArgumentInfo({"[-relative] <MD5/SHA1/SHA256/SHA384/SHA512/all> <dir> [outputFile]"}, True, 2), New SumFilesCommand)},
            {"sysinfo", New CommandInfo("sysinfo", ShellType.Shell, "System information", New CommandArgumentInfo({"[-s|-h|-u|-m|-l|-a]"}, False, 0), New SysInfoCommand, False, False, False, False, False)},
            {"testshell", New CommandInfo("testshell", ShellType.Shell, "Opens a test shell", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New TestShellCommand, True)},
            {"timer", New CommandInfo("timer", ShellType.Shell, "A simple timer", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New TimerCommand)},
            {"unblockdbgdev", New CommandInfo("unblockdbgdev", ShellType.Shell, "Unblock a debug device by IP address", New CommandArgumentInfo({"<ipaddress>"}, True, 1), New UnblockDbgDevCommand, True)},
            {"unitconv", New CommandInfo("unitconv", ShellType.Shell, "Unit converter", New CommandArgumentInfo({"<unittype> <quantity> <sourceunit> <targetunit>"}, True, 4), New UnitConvCommand)},
            {"unzip", New CommandInfo("unzip", ShellType.Shell, "Extracts a ZIP archive", New CommandArgumentInfo({"<zipfile> [path] [-createdir]"}, True, 1), New UnZipCommand, False, False, False, False, False)},
            {"update", New CommandInfo("update", ShellType.Shell, "System update", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New UpdateCommand, True)},
            {"usermanual", New CommandInfo("usermanual", ShellType.Shell, "Takes you to our GitHub Wiki.", New CommandArgumentInfo({"[-modapi]"}, False, 0), New UserManualCommand)},
            {"verify", New CommandInfo("verify", ShellType.Shell, "Verifies sanity of the file", New CommandArgumentInfo({"<MD5/SHA1/SHA256/SHA384/SHA512> <calculatedhash> <hashfile/expectedhash> <file>"}, True, 4), New VerifyCommand)},
            {"weather", New CommandInfo("weather", ShellType.Shell, "Shows weather info for specified city. Uses OpenWeatherMap.", New CommandArgumentInfo({"[-list] <CityID/CityName> [apikey]"}, True, 1), New WeatherCommand, False, False, False, False, True)},
            {"wrap", New CommandInfo("wrap", ShellType.Shell, "Wraps the console output", New CommandArgumentInfo({"<command>"}, True, 1), New WrapCommand, False, False, False, False, True)},
            {"zip", New CommandInfo("zip", ShellType.Shell, "Creates a ZIP archive", New CommandArgumentInfo({"<zipfile> <path> [-fast|-nocomp|-nobasedir]"}, True, 2), New ZipCommand, False, False, False, False, False)},
            {"zipshell", New CommandInfo("zipshell", ShellType.Shell, "Opens a ZIP archive", New CommandArgumentInfo({"<zipfile>"}, True, 1), New ZipShellCommand)}
        }
        ''' <summary>
        ''' List of unified commands
        ''' </summary>
        Public ReadOnly UnifiedCommandDict As New Dictionary(Of String, CommandInfo) From {
            {"presets", New CommandInfo("presets", ShellType.Shell, "Opens the shell preset library", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New PresetsUnifiedCommand)}
        }

        ''' <summary>
        ''' Parses a specified command.
        ''' </summary>
        ''' <param name="FullCommand">The full command string</param>
        ''' <param name="IsInvokedByKernelArgument">Indicates whether it was invoked by kernel argument parse (for internal use only)</param>
        ''' <param name="OutputPath">Optional (non-)neutralized output path</param>
        ''' <param name="ShellType">Shell type</param>
        ''' <remarks>All new shells implemented either in KS or by mods should use this routine to allow effective and consistent line parsing.</remarks>
        Public Sub GetLine(FullCommand As String, Optional IsInvokedByKernelArgument As Boolean = False, Optional OutputPath As String = "", Optional ShellType As ShellType = ShellType.Shell)
            'Check for sanity
            If String.IsNullOrEmpty(FullCommand) Then FullCommand = ""

            'Variables
            Dim OutputTextWriter As StreamWriter
            Dim OutputStream As FileStream

            'Check for a type of command
            Dim SplitCommands As String() = FullCommand.Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
            For Each Command As String In SplitCommands
                Dim Done As Boolean = False

                'Check to see if the command is a comment
                If Not (Command = Nothing Or Command?.StartsWithAnyOf({" ", "#"})) Then
                    Dim Parts As String() = Regex.Split(Command, "(?<=^[^""]*(?:""[^""]*""[^""]*)*) (?=(?:[^""]*""[^""]*"")*[^""]*$)").Select(Function(m) If(m.StartsWith("""") And m.EndsWith(""""), m.ReleaseDoubleQuotes(), m)).ToArray()

                    'Iterate through mod commands
                    Wdbg(DebugLevel.I, "Mod commands probing started with {0} from {1}", Command, FullCommand)
                    If ListModCommands(ShellType).ContainsKey(Parts(0)) Then
                        Done = True
                        Wdbg(DebugLevel.I, "Mod command: {0}", Parts(0))
                        ExecuteModCommand(Command)
                    End If

                    'Iterate through alias commands
                    Wdbg(DebugLevel.I, "Aliases probing started with {0} from {1}", Command, FullCommand)
                    If GetAliasesListFromType(ShellType).ContainsKey(Parts(0)) Then
                        Done = True
                        Wdbg(DebugLevel.I, "Alias: {0}", Parts(0))
                        ExecuteAlias(Command, ShellType)
                    End If

                    'Execute the built-in command
                    If Done = False Then
                        Wdbg(DebugLevel.I, "Executing built-in command")

                        'If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
                        Wdbg(DebugLevel.I, "Does the command contain the redirection sign "">>>"" or "">>""? {0} and {1}", Command.Contains(">>>"), Command.Contains(">>"))
                        If Command.Contains(">>>") Then
                            Wdbg(DebugLevel.I, "Output redirection found with append.")
                            Dim OutputFileName As String = Command.Substring(Command.LastIndexOf(">") + 2)
                            DefConsoleOut = Console.Out
                            OutputStream = New FileStream(NeutralizePath(OutputFileName), FileMode.Append, FileAccess.Write)
                            OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
                            Console.SetOut(OutputTextWriter)
                            Command = Command.Replace(" >>> " + OutputFileName, "")
                        ElseIf Command.Contains(">>") Then
                            Wdbg(DebugLevel.I, "Output redirection found with overwrite.")
                            Dim OutputFileName As String = Command.Substring(Command.LastIndexOf(">") + 2)
                            DefConsoleOut = Console.Out
                            OutputStream = New FileStream(NeutralizePath(OutputFileName), FileMode.OpenOrCreate, FileAccess.Write)
                            OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
                            Console.SetOut(OutputTextWriter)
                            Command = Command.Replace(" >> " + OutputFileName, "")
                        End If

                        'Checks to see if the user provided optional path
                        If Not String.IsNullOrWhiteSpace(OutputPath) Then
                            Wdbg(DebugLevel.I, "Optional output redirection found using OutputPath ({0}).", OutputPath)
                            DefConsoleOut = Console.Out
                            OutputStream = New FileStream(NeutralizePath(OutputPath), FileMode.OpenOrCreate, FileAccess.Write)
                            OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
                            Console.SetOut(OutputTextWriter)
                        End If

                        'Reads command written by user
                        Try
                            Dim EntireCommand As String = Command
                            If Not (Command = Nothing Or Command.StartsWithAnyOf({" ", "#"}) = True) Then
                                Console.Title = $"{ConsoleTitle} - {Command}"

                                'Parse script command (if any)
                                Dim scriptArgs As List(Of String) = Command.Split({".uesh "}, StringSplitOptions.RemoveEmptyEntries).ToList
                                scriptArgs.RemoveAt(0)

                                'Get the index of the first space
                                Dim indexCmd As Integer = Command.IndexOf(" ")
                                Dim cmdArgs As String = Command 'Command with args
                                Wdbg(DebugLevel.I, "Prototype indexCmd and Command: {0}, {1}", indexCmd, Command)
                                If indexCmd = -1 Then indexCmd = Command.Length
                                Command = Command.Substring(0, indexCmd)
                                Wdbg(DebugLevel.I, "Finished indexCmd and Command: {0}, {1}", indexCmd, Command)

                                'Scan PATH for file existence and set file name as needed
                                Dim TargetFile As String = ""
                                Dim TargetFileName As String = ""
                                FileExistsInPath(Command, TargetFile)
                                If String.IsNullOrEmpty(TargetFile) Then TargetFile = NeutralizePath(Command)
                                If TryParsePath(TargetFile) Then TargetFileName = Path.GetFileName(TargetFile)

                                'Check to see if a user is able to execute a command
                                Dim Commands As Dictionary(Of String, CommandInfo) = GetCommands(ShellType)
                                If Commands.ContainsKey(Command) Then
                                    If ShellType = ShellType.Shell Then
                                        If HasPermission(CurrentUser.Username, PermissionType.Administrator) = False And Commands(Command).Strict Then
                                            Wdbg(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", Command)
                                            Write(DoTranslation("You don't have permission to use {0}"), True, ColTypes.Error, Command)
                                            Exit Try
                                        End If
                                    End If

                                    If Maintenance = True And Commands(Command).NoMaintenance Then
                                        Wdbg(DebugLevel.W, "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", Command)
                                        Write(DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode."), True, ColTypes.Error, Command)
                                    ElseIf IsInvokedByKernelArgument And (Command.StartsWith("logout") Or Command.StartsWith("shutdown") Or Command.StartsWith("reboot")) Then
                                        Wdbg(DebugLevel.W, "Cmd exec {0} failed: cmd is one of ""logout"" or ""shutdown"" or ""reboot""", Command)
                                        Write(DoTranslation("Shell message: Command {0} is not allowed to run on log in."), True, ColTypes.Error, Command)
                                    Else
                                        Wdbg(DebugLevel.I, "Cmd exec {0} succeeded. Running with {1}", Command, cmdArgs)
                                        Dim Params As New ExecuteCommandThreadParameters(EntireCommand, ShellType, Nothing)

                                        'Since we're probably trying to run a command using the alternative command threads, if the main shell command thread
                                        'is running, use that to execute the command. This ensures that commands like "wrap" that also execute commands from the
                                        'shell can do their job.
                                        Dim ShellInstance As ShellInfo = ShellStack(ShellStack.Count - 1)
                                        Dim StartCommandThread As KernelThread = ShellInstance.ShellCommandThread
                                        Dim CommandThreadValid As Boolean = True
                                        If StartCommandThread.IsAlive Then
                                            If ShellInstance.AltCommandThreads.Count > 0 Then
                                                StartCommandThread = ShellInstance.AltCommandThreads(ShellInstance.AltCommandThreads.Count - 1)
                                            Else
                                                Wdbg(DebugLevel.W, "Cmd exec {0} failed: Alt command threads are not there.")
                                                CommandThreadValid = False
                                            End If
                                        End If
                                        If CommandThreadValid Then
                                            StartCommandThread.Start(Params)
                                            StartCommandThread.Wait()
                                            StartCommandThread.Stop()
                                        End If
                                    End If
                                ElseIf TryParsePath(TargetFile) And ShellType = ShellType.Shell Then
                                    'If we're in the UESH shell, parse the script file or executable file
                                    If FileExists(TargetFile) And Not TargetFile.EndsWith(".uesh") Then
                                        Wdbg(DebugLevel.I, "Cmd exec {0} succeeded because file is found.", Command)
                                        Try
                                            'Create a new instance of process
                                            If TryParsePath(TargetFile) Then
                                                cmdArgs = cmdArgs.Replace(TargetFileName, "")
                                                cmdArgs.RemoveNullsOrWhitespacesAtTheBeginning()
                                                Wdbg(DebugLevel.I, "Command: {0}, Arguments: {1}", TargetFile, cmdArgs)
                                                Dim Params As New ExecuteProcessThreadParameters(TargetFile, cmdArgs)
                                                ProcessStartCommandThread.Start(Params)
                                                ProcessStartCommandThread.Wait()
                                                ProcessStartCommandThread.Stop()
                                            End If
                                        Catch ex As Exception
                                            Wdbg(DebugLevel.E, "Failed to start process: {0}", ex.Message)
                                            Write(DoTranslation("Failed to start ""{0}"": {1}"), True, ColTypes.Error, Command, ex.Message)
                                            WStkTrc(ex)
                                        End Try
                                    ElseIf FileExists(TargetFile) And TargetFile.EndsWith(".uesh") Then
                                        Try
                                            Wdbg(DebugLevel.I, "Cmd exec {0} succeeded because it's a UESH script.", Command)
                                            Execute(TargetFile, scriptArgs.Join(" "))
                                        Catch ex As Exception
                                            Write(DoTranslation("Error trying to execute script: {0}"), True, ColTypes.Error, ex.Message)
                                            WStkTrc(ex)
                                        End Try
                                    Else
                                        Wdbg(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", Command, indexCmd)
                                        Write(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), True, ColTypes.Error, Command)
                                    End If
                                Else
                                    Wdbg(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", Command, indexCmd)
                                    Write(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), True, ColTypes.Error, Command)
                                End If
                            End If
                        Catch ex As Exception
                            WStkTrc(ex)
                            Write(DoTranslation("Error trying to execute command.") + NewLine +
                                  DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message)
                        End Try
                    End If
                End If
            Next
            Console.Title = ConsoleTitle

            'Restore console output to its original state if any
#Disable Warning BC42104
            If DefConsoleOut IsNot Nothing Then
                Console.SetOut(DefConsoleOut)
                OutputTextWriter?.Close()
            End If
#Enable Warning BC42104
        End Sub

    End Module
End Namespace
