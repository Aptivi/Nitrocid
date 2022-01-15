
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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
Imports System.Threading

'TODO: Make full use of the Shell interface (this shell and all the supported shells)
Public Module Shell

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
    Public PathsToLookup As String = Environ("PATH")
    ''' <summary>
    ''' Path lookup delimiter, depending on the operating system
    ''' </summary>
    Public ReadOnly PathLookupDelimiter As String = Path.PathSeparator
    ''' <summary>
    ''' All injected commands
    ''' </summary>
    Public InjectedCommands As New List(Of String)
    ''' <summary>
    ''' All mod commands
    ''' </summary>
    Public ModCommands As New ArrayList
    ''' <summary>
    ''' List of commands
    ''' </summary>
    Public ReadOnly Commands As New Dictionary(Of String, CommandInfo) From {{"adduser", New CommandInfo("adduser", ShellCommandType.Shell, "Adds users", {"<userName> [password] [confirm]"}, True, 1, New AddUserCommand, True)},
                                                                             {"alias", New CommandInfo("alias", ShellCommandType.Shell, "Adds aliases to commands", {$"<rem/add> <{String.Join("/", [Enum].GetNames(GetType(ShellCommandType)))}> <alias> <cmd>"}, True, 3, New AliasCommand, True)},
                                                                             {"arginj", New CommandInfo("arginj", ShellCommandType.Shell, "Injects arguments to the kernel (reboot required)", {"[Arguments separated by spaces]"}, True, 1, New ArgInjCommand, True, False, False, False, False, New Action(AddressOf (New ArgInjCommand).HelpHelper))},
                                                                             {"beep", New CommandInfo("beep", ShellCommandType.Shell, "Beep in 'n' Hz and time in 'n' milliseconds", {"<37-32767 Hz> <milliseconds>"}, True, 2, New BeepCommand)},
                                                                             {"blockdbgdev", New CommandInfo("blockdbgdev", ShellCommandType.Shell, "Block a debug device by IP address", {"<ipaddress>"}, True, 1, New BlockDbgDevCommand, True)},
                                                                             {"calc", New CommandInfo("calc", ShellCommandType.Shell, "Calculator to calculate expressions.", {"<expression>"}, True, 1, New CalcCommand)},
                                                                             {"calendar", New CommandInfo("calendar", ShellCommandType.Shell, "Calendar, event, and reminder manager", {"<show> [year] [month]", "<event> <add> <date> <title>", "<event> <remove> <eventid>", "<event> <list>", "<event> <saveall>", "<reminder> <add> <dateandtime> <title>", "<reminder> <remove> <reminderid>", "<reminder> <list>", "<reminder> <saveall>"}, True, 1, New CalendarCommand)},
                                                                             {"cat", New CommandInfo("cat", ShellCommandType.Shell, "Prints content of file to console", {"[-lines|-nolines] <file>"}, True, 1, New CatCommand, False, True, False, False, False, New Action(AddressOf (New CatCommand).HelpHelper))},
                                                                             {"cdbglog", New CommandInfo("cdbglog", ShellCommandType.Shell, "Deletes everything in debug log", {}, False, 0, New CdbgLogCommand, True)},
                                                                             {"chattr", New CommandInfo("chattr", ShellCommandType.Shell, "Changes attribute of a file", {"<file> +/-<attributes>"}, True, 2, New ChAttrCommand, False, False, False, False, False, New Action(AddressOf (New ChAttrCommand).HelpHelper))},
                                                                             {"chdir", New CommandInfo("chdir", ShellCommandType.Shell, "Changes directory", {"<directory/..>"}, True, 1, New ChDirCommand)},
                                                                             {"chhostname", New CommandInfo("chhostname", ShellCommandType.Shell, "Changes host name", {"<HostName>"}, True, 1, New ChHostNameCommand, True)},
                                                                             {"chlang", New CommandInfo("chlang", ShellCommandType.Shell, "Changes language", {"[-alwaystransliterated|-alwaystranslated|-force] <language>"}, True, 1, New ChLangCommand, True, False, False, False, False, New Action(AddressOf (New ChLangCommand).HelpHelper))},
                                                                             {"chmal", New CommandInfo("chmal", ShellCommandType.Shell, "Changes MAL, the MOTD After Login", {"[Message]"}, False, 0, New ChMalCommand, True)},
                                                                             {"chmotd", New CommandInfo("chmotd", ShellCommandType.Shell, "Changes MOTD, the Message Of The Day", {"[Message]"}, False, 0, New ChMotdCommand, True)},
                                                                             {"choice", New CommandInfo("choice", ShellCommandType.Shell, "Makes user choices", {"[-o|-t|-m|-a] [-multiple|-single] <$variable> <answers> <input> [answertitle1] [answertitle2] ..."}, True, 3, New ChoiceCommand, False, False, False, False, True, New Action(AddressOf (New ChoiceCommand).HelpHelper))},
                                                                             {"chpwd", New CommandInfo("chpwd", ShellCommandType.Shell, "Changes password for current user", {"<Username> <UserPass> <newPass> <confirm>"}, True, 4, New ChPwdCommand, True)},
                                                                             {"chusrname", New CommandInfo("chusrname", ShellCommandType.Shell, "Changes user name", {"<oldUserName> <newUserName>"}, True, 2, New ChUsrNameCommand, True)},
                                                                             {"clearfiredevents", New CommandInfo("clearfiredevents", ShellCommandType.Shell, "Clears all fired events", {}, False, 0, New ClearFiredEventsCommand)},
                                                                             {"cls", New CommandInfo("cls", ShellCommandType.Shell, "Clears the screen", {}, False, 0, New ClsCommand)},
                                                                             {"combine", New CommandInfo("combine", ShellCommandType.Shell, "Combines the two text files or more into the output file.", {"<output> <input1> <input2> [input3] ..."}, True, 3, New CombineCommand)},
                                                                             {"convertlineendings", New CommandInfo("convertlineendings", ShellCommandType.Shell, "Converts the line endings to format for the current platform or to specified custom format", {"<textfile> [-w|-u|-m]"}, True, 1, New ConvertLineEndingsCommand)},
                                                                             {"copy", New CommandInfo("copy", ShellCommandType.Shell, "Creates another copy of a file under different directory or name.", {"<source> <target>"}, True, 2, New CopyCommand)},
                                                                             {"dirinfo", New CommandInfo("dirinfo", ShellCommandType.Shell, "Provides information about a directory", {"<directory>"}, True, 1, New DirInfoCommand)},
                                                                             {"disconndbgdev", New CommandInfo("disconndbgdev", ShellCommandType.Shell, "Disconnect a debug device", {"<ip>"}, True, 1, New DisconnDbgDevCommand, True)},
                                                                             {"dismissnotif", New CommandInfo("dismissnotif", ShellCommandType.Shell, "Dismisses a notification", {"<notificationNumber>"}, True, 1, New DismissNotifCommand)},
                                                                             {"dismissnotifs", New CommandInfo("dismissnotifs", ShellCommandType.Shell, "Dismisses all notifications", {}, False, 0, New DismissNotifsCommand)},
                                                                             {"echo", New CommandInfo("echo", ShellCommandType.Shell, "Writes text into the console", {"<text>"}, False, 0, New EchoCommand)},
                                                                             {"edit", New CommandInfo("edit", ShellCommandType.Shell, "Edits a text file", {"<file>"}, True, 1, New EditCommand)},
                                                                             {"exit", New CommandInfo("exit", ShellCommandType.Shell, "Exits the shell if running on subshell", {}, False, 0, New ExitCommand)},
                                                                             {"fileinfo", New CommandInfo("fileinfo", ShellCommandType.Shell, "Provides information about a file", {"<file>"}, True, 1, New FileInfoCommand)},
                                                                             {"find", New CommandInfo("find", ShellCommandType.Shell, "Finds a file in the specified directory or in the current directory", {"<file> [directory]"}, True, 1, New FindCommand)},
                                                                             {"firedevents", New CommandInfo("firedevents", ShellCommandType.Shell, "Lists all fired events", {}, False, 0, New FiredEventsCommand)},
                                                                             {"ftp", New CommandInfo("ftp", ShellCommandType.Shell, "Use an FTP shell to interact with servers", {"<server>"}, False, 0, New FtpCommand)},
                                                                             {"gettimeinfo", New CommandInfo("gettimeinfo", ShellCommandType.Shell, "Gets the date and time information", {"<date>"}, True, 1, New GetTimeInfoCommand)},
                                                                             {"get", New CommandInfo("get", ShellCommandType.Shell, "Downloads a file to current working directory", {"<URL> [username]"}, True, 1, New Get_Command)},
                                                                             {"help", New CommandInfo("help", ShellCommandType.Shell, "Help page", {"[command]"}, False, 0, New HelpCommand)},
                                                                             {"http", New CommandInfo("http", ShellCommandType.Shell, "Starts the HTTP shell", {}, False, 0, New HttpCommand)},
                                                                             {"hwinfo", New CommandInfo("hwinfo", ShellCommandType.Shell, "Prints hardware information", {"<HardwareType>"}, True, 1, New HwInfoCommand, False, True, False, False, False, New Action(AddressOf (New HwInfoCommand).HelpHelper))},
                                                                             {"if", New CommandInfo("if", ShellCommandType.Shell, "Executes commands once the UESH expressions are satisfied", {"<uesh-expression> <command>"}, True, 2, New IfCommand)},
                                                                             {"input", New CommandInfo("input", ShellCommandType.Shell, "Allows user to enter input", {"<$variable> <question>"}, True, 2, New InputCommand, False, False, False, False, True)},
                                                                             {"jsonbeautify", New CommandInfo("jsonbeautify", ShellCommandType.Shell, "Beautifies the JSON file", {"<jsonfile> [output]"}, True, 1, New JsonBeautifyCommand, False, True)},
                                                                             {"jsonminify", New CommandInfo("jsonminify", ShellCommandType.Shell, "Minifies the JSON file", {"<jsonfile> [output]"}, True, 1, New JsonMinifyCommand, False, True)},
                                                                             {"jsonshell", New CommandInfo("jsonshell", ShellCommandType.Shell, "Opens the JSON shell", {"<jsonfile>"}, True, 1, New JsonShellCommand)},
                                                                             {"langman", New CommandInfo("langman", ShellCommandType.Shell, "Manage your languages", {"<reload/load/unload> <customlanguagename>", "<list/reloadall>"}, True, 1, New LangManCommand, True)},
                                                                             {"list", New CommandInfo("list", ShellCommandType.Shell, "List file/folder contents in current folder", {"[-showdetails|-suppressmessages] [directory]"}, False, 0, New ListCommand, False, True, False, False, False, New Action(AddressOf (New ListCommand).HelpHelper))},
                                                                             {"lockscreen", New CommandInfo("lockscreen", ShellCommandType.Shell, "Locks your screen with a password", {}, False, 0, New LockScreenCommand)},
                                                                             {"logout", New CommandInfo("logout", ShellCommandType.Shell, "Logs you out", {}, False, 0, New LogoutCommand, False, False, True)},
                                                                             {"lovehate", New CommandInfo("lovehate", ShellCommandType.Shell, "Respond to love or hate comments.", {}, False, 0, New LoveHateCommand)},
                                                                             {"lsdbgdev", New CommandInfo("lsdbgdev", ShellCommandType.Shell, "Lists debugging devices connected", {}, False, 0, New LsDbgDevCommand, True, True)},
                                                                             {"mail", New CommandInfo("mail", ShellCommandType.Shell, "Opens the mail client", {"[emailAddress]"}, False, 0, New MailCommand)},
                                                                             {"mathbee", New CommandInfo("mathbee", ShellCommandType.Shell, "See if you can solve mathematical equations on time", {}, False, 0, New MathBeeCommand)},
                                                                             {"md", New CommandInfo("md", ShellCommandType.Shell, "Creates a directory", {"<directory>"}, True, 1, New MdCommand)},
                                                                             {"mkfile", New CommandInfo("mkfile", ShellCommandType.Shell, "Makes a new file", {"<file>"}, True, 1, New MkFileCommand)},
                                                                             {"mktheme", New CommandInfo("mktheme", ShellCommandType.Shell, "Makes a new theme", {"<themeName>"}, True, 1, New MkThemeCommand)},
                                                                             {"modman", New CommandInfo("modman", ShellCommandType.Shell, "Manage your mods", {"<start/stop/info/reload/install/uninstall> <modfilename>", "<list/reloadall/stopall/startall>"}, True, 1, New ModManCommand, True)},
                                                                             {"modmanual", New CommandInfo("modmanual", ShellCommandType.Shell, "Mod manual", {"[-list] <ManualTitle>"}, True, 1, New ModManualCommand)},
                                                                             {"move", New CommandInfo("move", ShellCommandType.Shell, "Moves a file to another directory", {"<source> <target>"}, True, 2, New MoveCommand)},
                                                                             {"netinfo", New CommandInfo("netinfo", ShellCommandType.Shell, "Lists information about all available interfaces", {}, False, 0, New NetInfoCommand, True, True)},
                                                                             {"open", New CommandInfo("open", ShellCommandType.Shell, "Opens a URL", {"<URL>"}, True, 1, New OpenCommand)},
                                                                             {"perm", New CommandInfo("perm", ShellCommandType.Shell, "Manage permissions for users", {"<userName> <Administrator/Disabled/Anonymous> <Allow/Disallow>"}, True, 3, New PermCommand, True)},
                                                                             {"ping", New CommandInfo("ping", ShellCommandType.Shell, "Pings an address", {"[times] <Address1> <Address2> ..."}, True, 1, New PingCommand)},
                                                                             {"put", New CommandInfo("put", ShellCommandType.Shell, "Uploads a file to specified website", {"<FileName> <URL> [username]"}, True, 2, New PutCommand)},
                                                                             {"reboot", New CommandInfo("reboot", ShellCommandType.Shell, "Restarts your computer (WARNING: No syncing, because it is not a final kernel)", {"[ip] [port]"}, False, 0, New RebootCommand)},
                                                                             {"reloadconfig", New CommandInfo("reloadconfig", ShellCommandType.Shell, "Reloads configuration file that is edited.", {}, False, 0, New ReloadConfigCommand, True, False, False, False, True, New Action(AddressOf (New ReloadConfigCommand).HelpHelper))},
                                                                             {"reloadsaver", New CommandInfo("reloadsaver", ShellCommandType.Shell, "Reloads screensaver file in KSMods", {"<customsaver>"}, True, 1, New ReloadSaverCommand, True, False, False, False, True, New Action(AddressOf (New ReloadSaverCommand).HelpHelper))},
                                                                             {"rexec", New CommandInfo("rexec", ShellCommandType.Shell, "Remotely executes a command to remote PC", {"<address> [port] <command>"}, True, 2, New RexecCommand, True)},
                                                                             {"rm", New CommandInfo("rm", ShellCommandType.Shell, "Removes a directory or a file", {"<directory/file>"}, True, 1, New RmCommand)},
                                                                             {"rdebug", New CommandInfo("rdebug", ShellCommandType.Shell, "Enables or disables remote debugging.", {}, False, 0, New RdebugCommand, True)},
                                                                             {"reportbug", New CommandInfo("reportbug", ShellCommandType.Shell, "A bug reporting prompt.", {}, False, 0, New ReportBugCommand)},
                                                                             {"rmuser", New CommandInfo("rmuser", ShellCommandType.Shell, "Removes a user from the list", {"<Username>"}, True, 1, New RmUserCommand, True)},
                                                                             {"rss", New CommandInfo("rss", ShellCommandType.Shell, "Opens an RSS shell to read the feeds", {"[feedlink]"}, False, 0, New RssCommand)},
                                                                             {"savecurrdir", New CommandInfo("savecurrdir", ShellCommandType.Shell, "Saves the current directory to kernel configuration file", {}, False, 0, New SaveCurrDirCommand, True)},
                                                                             {"saversettings", New CommandInfo("settings", ShellCommandType.Shell, "Changes screensaver configuration", {}, False, 0, New SaverSettingsCommand, True)},
                                                                             {"savescreen", New CommandInfo("savescreen", ShellCommandType.Shell, "Saves your screen from burn outs", {"[saver]"}, False, 0, New SaveScreenCommand)},
                                                                             {"search", New CommandInfo("search", ShellCommandType.Shell, "Searches for specified string in the provided file using regular expressions", {"<Regexp> <File>"}, True, 2, New SearchCommand)},
                                                                             {"searchword", New CommandInfo("searchword", ShellCommandType.Shell, "Searches for specified string in the provided file", {"<StringEnclosedInDoubleQuotes> <File>"}, True, 2, New SearchWordCommand)},
                                                                             {"select", New CommandInfo("select", ShellCommandType.Shell, "Provides a selection choice", {"<$variable> <answers> <input> [answertitle1] [answertitle2] ..."}, True, 3, New SelectCommand, False, False, False, False, True)},
                                                                             {"setsaver", New CommandInfo("setsaver", ShellCommandType.Shell, "Sets up kernel screensavers", {"<customsaver/builtinsaver>"}, True, 1, New SetSaverCommand, True, False, False, False, False, New Action(AddressOf (New SetSaverCommand).HelpHelper))},
                                                                             {"setthemes", New CommandInfo("setthemes", ShellCommandType.Shell, "Sets up kernel themes", {"<Theme>"}, True, 1, New SetThemesCommand, False, False, False, False, False, New Action(AddressOf (New SetThemesCommand).HelpHelper))},
                                                                             {"settings", New CommandInfo("settings", ShellCommandType.Shell, "Changes kernel configuration", {}, False, 0, New SettingsCommand, True)},
                                                                             {"set", New CommandInfo("set", ShellCommandType.Shell, "Sets a variable to a value in a script", {"<$variable> <value>"}, True, 2, New SetCommand, False, False, False, False, True)},
                                                                             {"setrange", New CommandInfo("setrange", ShellCommandType.Shell, "Creates a variable array with the provided values", {"<$variablename> <value1> [value2] [value3] ..."}, True, 2, New SetRangeCommand, False, False, False, False, True)},
                                                                             {"sftp", New CommandInfo("sftp", ShellCommandType.Shell, "Lets you use an SSH FTP server", {"<server>"}, False, 0, New SftpCommand)},
                                                                             {"shownotifs", New CommandInfo("shownotifs", ShellCommandType.Shell, "Shows all received notifications", {}, False, 0, New ShowNotifsCommand)},
                                                                             {"showtd", New CommandInfo("showtd", ShellCommandType.Shell, "Shows date and time", {}, False, 0, New ShowTdCommand)},
                                                                             {"showtdzone", New CommandInfo("showtdzone", ShellCommandType.Shell, "Shows date and time in zones", {"[-all] <timezone>"}, True, 1, New ShowTdZoneCommand, False, True, False, False, False, New Action(AddressOf (New ShowTdZoneCommand).HelpHelper))},
                                                                             {"shutdown", New CommandInfo("shutdown", ShellCommandType.Shell, "The kernel will be shut down", {"[ip] [port]"}, False, 0, New ShutdownCommand)},
                                                                             {"speedpress", New CommandInfo("speedpress", ShellCommandType.Shell, "See if you can press a key on time", {"[-e|-m|-h|-v|-c] [timeout]"}, False, 0, New SpeedPressCommand, False, False, False, False, False, New Action(AddressOf (New SpeedPressCommand).HelpHelper))},
                                                                             {"spellbee", New CommandInfo("spellbee", ShellCommandType.Shell, "See if you can spell words correctly on time", {}, False, 0, New SpellBeeCommand)},
                                                                             {"sshell", New CommandInfo("sshell", ShellCommandType.Shell, "Connects to an SSH server.", {"<address:port> <username>"}, True, 2, New SshellCommand)},
                                                                             {"sshcmd", New CommandInfo("sshcmd", ShellCommandType.Shell, "Connects to an SSH server to execute a command.", {"<address:port> <username> ""<command>"""}, True, 3, New SshcmdCommand)},
                                                                             {"stopwatch", New CommandInfo("stopwatch", ShellCommandType.Shell, "A simple stopwatch", {}, False, 0, New StopwatchCommand)},
                                                                             {"sumfile", New CommandInfo("sumfile", ShellCommandType.Shell, "Calculates file sums.", {"<MD5/SHA1/SHA256/SHA384/SHA512/all> <file> [outputFile]"}, True, 2, New SumFileCommand)},
                                                                             {"sumfiles", New CommandInfo("sumfiles", ShellCommandType.Shell, "Calculates sums of files in specified directory.", {"<MD5/SHA1/SHA256/SHA384/SHA512/all> <dir> [outputFile]"}, True, 2, New SumFilesCommand)},
                                                                             {"sysinfo", New CommandInfo("sysinfo", ShellCommandType.Shell, "System information", {"[-s|-h|-u|-m|-l|-a]"}, False, 0, New SysInfoCommand, False, False, False, False, False, New Action(AddressOf (New SysInfoCommand).HelpHelper))},
                                                                             {"testshell", New CommandInfo("testshell", ShellCommandType.Shell, "Opens a test shell", {}, False, 0, New TestShellCommand, True)},
                                                                             {"timer", New CommandInfo("timer", ShellCommandType.Shell, "A simple timer", {}, False, 0, New TimerCommand)},
                                                                             {"unblockdbgdev", New CommandInfo("unblockdbgdev", ShellCommandType.Shell, "Unblock a debug device by IP address", {"<ipaddress>"}, True, 1, New UnblockDbgDevCommand, True)},
                                                                             {"unzip", New CommandInfo("unzip", ShellCommandType.Shell, "Extracts a ZIP archive", {"<zipfile> [path] [-createdir]"}, True, 1, New UnZipCommand, False, False, False, False, False, New Action(AddressOf (New UnZipCommand).HelpHelper))},
                                                                             {"update", New CommandInfo("update", ShellCommandType.Shell, "System update", {}, False, 0, New UpdateCommand, True)},
                                                                             {"usermanual", New CommandInfo("usermanual", ShellCommandType.Shell, "Takes you to our GitHub Wiki.", {}, False, 0, New UserManualCommand)},
                                                                             {"verify", New CommandInfo("verify", ShellCommandType.Shell, "Verifies sanity of the file", {"<MD5/SHA1/SHA256/SHA384/SHA512> <calculatedhash> <hashfile/expectedhash> <file>"}, True, 4, New VerifyCommand)},
                                                                             {"weather", New CommandInfo("weather", ShellCommandType.Shell, "Shows weather info for specified city. Uses OpenWeatherMap.", {"[-list] <CityID/CityName> [apikey]"}, True, 1, New WeatherCommand, False, False, False, False, True, New Action(AddressOf (New WeatherCommand).HelpHelper))},
                                                                             {"wrap", New CommandInfo("wrap", ShellCommandType.Shell, "Wraps the console output", {"<command>"}, True, 1, New WrapCommand, False, False, False, False, True, New Action(AddressOf (New WrapCommand).HelpHelper))},
                                                                             {"zip", New CommandInfo("zip", ShellCommandType.Shell, "Creates a ZIP archive", {"<zipfile> <path> [-fast|-nocomp|-nobasedir]"}, True, 2, New ZipCommand, False, False, False, False, False, New Action(AddressOf (New ZipCommand).HelpHelper))},
                                                                             {"zipshell", New CommandInfo("zipshell", ShellCommandType.Shell, "Opens a ZIP archive", {"<zipfile>"}, True, 1, New ZipShellCommand)}}

    ''' <summary>
    ''' Parses a specified command.
    ''' </summary>
    ''' <param name="strcommand">Specify command</param>
    ''' <param name="IsInvokedByKernelArgument">Indicates whether it was invoked by kernel argument parse (for internal use only)</param>
    ''' <param name="OutputPath">Optional (non-)neutralized output path</param>
    Public Sub GetLine(strcommand As String, Optional IsInvokedByKernelArgument As Boolean = False, Optional OutputPath As String = "")
        'If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
        Wdbg(DebugLevel.I, "Does the command contain the redirection sign "">>>"" or "">>""? {0} and {1}", strcommand.Contains(">>>"), strcommand.Contains(">>"))
        Dim OutputTextWriter As StreamWriter
        Dim OutputStream As FileStream
        If strcommand.Contains(">>>") Then
            Wdbg(DebugLevel.I, "Output redirection found with append.")
            DefConsoleOut = Console.Out
            Dim OutputFileName As String = strcommand.Substring(strcommand.LastIndexOf(">") + 2)
            OutputStream = New FileStream(NeutralizePath(OutputFileName), FileMode.Append, FileAccess.Write)
            OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
            Console.SetOut(OutputTextWriter)
            strcommand = strcommand.Replace(" >>> " + OutputFileName, "")
        ElseIf strcommand.Contains(">>") Then
            Wdbg(DebugLevel.I, "Output redirection found with overwrite.")
            DefConsoleOut = Console.Out
            Dim OutputFileName As String = strcommand.Substring(strcommand.LastIndexOf(">") + 2)
            OutputStream = New FileStream(NeutralizePath(OutputFileName), FileMode.OpenOrCreate, FileAccess.Write)
            OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
            Console.SetOut(OutputTextWriter)
            strcommand = strcommand.Replace(" >> " + OutputFileName, "")
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
            Dim EntireCommand As String = strcommand
            If Not (strcommand = Nothing Or strcommand.StartsWithAnyOf({" ", "#"}) = True) Then
                Console.Title = $"{ConsoleTitle} - {strcommand}"

                'Parse script command (if any)
                Dim scriptArgs As List(Of String) = strcommand.Split({".uesh "}, StringSplitOptions.RemoveEmptyEntries).ToList
                scriptArgs.RemoveAt(0)

                'Get the index of the first space
                Dim indexCmd As Integer = strcommand.IndexOf(" ")
                Dim cmdArgs As String = strcommand 'Command with args
                Wdbg(DebugLevel.I, "Prototype indexCmd and strcommand: {0}, {1}", indexCmd, strcommand)
                If indexCmd = -1 Then indexCmd = strcommand.Length
                strcommand = strcommand.Substring(0, indexCmd)
                Wdbg(DebugLevel.I, "Finished indexCmd and strcommand: {0}, {1}", indexCmd, strcommand)

                'Scan PATH for file existence and set file name as needed
                Dim TargetFile As String = ""
                Dim TargetFileName As String = ""
                FileExistsInPath(strcommand, TargetFile)
                If String.IsNullOrEmpty(TargetFile) Then TargetFile = NeutralizePath(strcommand)
                If TryParsePath(TargetFile) Then TargetFileName = Path.GetFileName(TargetFile)

                'Check to see if a user is able to execute a command
                If Commands.ContainsKey(strcommand) Then
                    If HasPermission(CurrentUser.Username, PermissionType.Administrator) = False And Commands(strcommand).Strict Then
                        Wdbg(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", strcommand)
                        Write(DoTranslation("You don't have permission to use {0}"), True, ColTypes.Error, strcommand)
                    ElseIf Maintenance = True And Commands(strcommand).NoMaintenance Then
                        Wdbg(DebugLevel.W, "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", strcommand)
                        Write(DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode."), True, ColTypes.Error, strcommand)
                    ElseIf IsInvokedByKernelArgument And (strcommand.StartsWith("logout") Or strcommand.StartsWith("shutdown") Or strcommand.StartsWith("reboot")) Then
                        Wdbg(DebugLevel.W, "Cmd exec {0} failed: cmd is one of ""logout"" or ""shutdown"" or ""reboot""", strcommand)
                        Write(DoTranslation("Shell message: Command {0} is not allowed to run on log in."), True, ColTypes.Error, strcommand)
                    ElseIf (HasPermission(CurrentUser.Username, PermissionType.Administrator) And Commands(strcommand).Strict) Or Commands.ContainsKey(strcommand) Then
                        Wdbg(DebugLevel.I, "Cmd exec {0} succeeded. Running with {1}", strcommand, cmdArgs)
                        Dim Params As New ExecuteCommandThreadParameters(EntireCommand, ShellCommandType.Shell, Nothing)
                        StartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "Shell Command Thread"}
                        StartCommandThread.Start(Params)
                        StartCommandThread.Join()
                    End If
                ElseIf TryParsePath(TargetFile) Then
                    If FileExists(TargetFile) And Not TargetFile.EndsWith(".uesh") Then
                        Wdbg(DebugLevel.I, "Cmd exec {0} succeeded because file is found.", strcommand)
                        Try
                            'Create a new instance of process
                            If TryParsePath(TargetFile) Then
                                cmdArgs = cmdArgs.Replace(TargetFileName, "")
                                cmdArgs.RemoveNullsOrWhitespacesAtTheBeginning
                                Wdbg(DebugLevel.I, "Command: {0}, Arguments: {1}", TargetFile, cmdArgs)
                                Dim Params As New ExecuteProcessThreadParameters(TargetFile, cmdArgs)
                                ProcessStartCommandThread = New Thread(AddressOf ExecuteProcess) With {.Name = "Executable Command Thread"}
                                ProcessStartCommandThread.Start(Params)
                                ProcessStartCommandThread.Join()
                            End If
                        Catch ex As Exception
                            Wdbg(DebugLevel.E, "Failed to start process: {0}", ex.Message)
                            Write(DoTranslation("Failed to start ""{0}"": {1}"), True, ColTypes.Error, strcommand, ex.Message)
                            WStkTrc(ex)
                        End Try
                    ElseIf FileExists(TargetFile) And TargetFile.EndsWith(".uesh") Then
                        Wdbg(DebugLevel.I, "Cmd exec {0} succeeded because it's a UESH script.", strcommand)
                        Execute(TargetFile, scriptArgs.Join(" "))
                    Else
                        Wdbg(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", strcommand, indexCmd)
                        Write(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), True, ColTypes.Error, strcommand)
                    End If
                Else
                    Wdbg(DebugLevel.W, "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", strcommand, indexCmd)
                    Write(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), True, ColTypes.Error, strcommand)
                End If
            End If
        Catch ex As Exception
            WStkTrc(ex)
            Write(DoTranslation("Error trying to execute command.") + vbNewLine +
                  DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message)
        End Try
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
