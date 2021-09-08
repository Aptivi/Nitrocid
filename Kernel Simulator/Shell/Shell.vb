
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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
    Public ReadOnly PathLookupDelimiter As String = If(IsOnUnix(), ":", ";")
    ''' <summary>
    ''' All injected commands
    ''' </summary>
    Public InjectedCommands As New List(Of String)
    ''' <summary>
    ''' All mod commands
    ''' </summary>
    Public modcmnds As New ArrayList
    ''' <summary>
    ''' List of commands
    ''' </summary>
    Public ReadOnly Commands As New Dictionary(Of String, CommandInfo) From {{"adduser", New CommandInfo("adduser", ShellCommandType.Shell, "Adds users", "<userName> [password] [confirm]", True, 1, True)},
                                                                             {"alias", New CommandInfo("alias", ShellCommandType.Shell, "Adds aliases to commands", $"<rem/add> <{String.Join("/", [Enum].GetNames(GetType(AliasType)))}> <alias> <cmd>", True, 3, True)},
                                                                             {"arginj", New CommandInfo("arginj", ShellCommandType.Shell, "Injects arguments to the kernel (reboot required)", "[Arguments separated by spaces]", True, 1, True)},
                                                                             {"beep", New CommandInfo("beep", ShellCommandType.Shell, "Beep in 'n' Hz and time in 'n' milliseconds", "<37-32767 Hz> <milliseconds>", True, 2)},
                                                                             {"blockdbgdev", New CommandInfo("blockdbgdev", ShellCommandType.Shell, "Block a debug device by IP address", "<ipaddress>", True, 1, True)},
                                                                             {"calc", New CommandInfo("calc", ShellCommandType.Shell, "Calculator to calculate expressions.", "<expression>", True, 1)},
                                                                             {"cat", New CommandInfo("cat", ShellCommandType.Shell, "Prints content of file to console", "<file>", True, 1, False, True)},
                                                                             {"cdbglog", New CommandInfo("cdbglog", ShellCommandType.Shell, "Deletes everything in debug log", "", False, 0, True)},
                                                                             {"chattr", New CommandInfo("chattr", ShellCommandType.Shell, "Changes attribute of a file", "<file> +/-<attributes>", True, 2)},
                                                                             {"chdir", New CommandInfo("chdir", ShellCommandType.Shell, "Changes directory", "<directory/..>", True, 1)},
                                                                             {"chhostname", New CommandInfo("chhostname", ShellCommandType.Shell, "Changes host name", "<HostName>", True, 1, True)},
                                                                             {"chlang", New CommandInfo("chlang", ShellCommandType.Shell, "Changes language", "<language>", True, 1, True)},
                                                                             {"chmal", New CommandInfo("chmal", ShellCommandType.Shell, "Changes MAL, the MOTD After Login", "[Message]", False, 0, True)},
                                                                             {"chmotd", New CommandInfo("chmotd", ShellCommandType.Shell, "Changes MOTD, the Message Of The Day", "[Message]", False, 0, True)},
                                                                             {"choice", New CommandInfo("choice", ShellCommandType.Shell, "Makes user choices", "[-o|-t|-m] <$variable> <answers> <input>", True, 3, False, False, False, False, True)},
                                                                             {"chpwd", New CommandInfo("chpwd", ShellCommandType.Shell, "Changes password for current user", "<Username> <UserPass> <newPass> <confirm>", True, 4, True)},
                                                                             {"chusrname", New CommandInfo("chusrname", ShellCommandType.Shell, "Changes user name", "<oldUserName> <newUserName>", True, 2, True)},
                                                                             {"cls", New CommandInfo("cls", ShellCommandType.Shell, "Clears the screen", "", False, 0)},
                                                                             {"copy", New CommandInfo("copy", ShellCommandType.Shell, "Creates another copy of a file under different directory or name.", "<source> <target>", True, 2)},
                                                                             {"dirinfo", New CommandInfo("dirinfo", ShellCommandType.Shell, "Provides information about a directory", "<directory>", True, 1)},
                                                                             {"disconndbgdev", New CommandInfo("disconndbgdev", ShellCommandType.Shell, "Disconnect a debug device", "<ip>", True, 1, True)},
                                                                             {"dismissnotif", New CommandInfo("dismissnotif", ShellCommandType.Shell, "Dismisses a notification", "<notificationNumber>", True, 1)},
                                                                             {"echo", New CommandInfo("echo", ShellCommandType.Shell, "Writes text into the console", "<text>", False, 0)},
                                                                             {"edit", New CommandInfo("edit", ShellCommandType.Shell, "Edits a text file", "<file>", True, 1)},
                                                                             {"fileinfo", New CommandInfo("fileinfo", ShellCommandType.Shell, "Provides information about a file", "<file>", True, 1)},
                                                                             {"firedevents", New CommandInfo("firedevents", ShellCommandType.Shell, "Lists all fired events", "", False, 0)},
                                                                             {"ftp", New CommandInfo("ftp", ShellCommandType.Shell, "Use an FTP shell to interact with servers", "<server>", False, 0)},
                                                                             {"gettimeinfo", New CommandInfo("gettimeinfo", ShellCommandType.Shell, "Gets the date and time information", "<date>", True, 1)},
                                                                             {"get", New CommandInfo("get", ShellCommandType.Shell, "Downloads a file to current working directory", "<URL> [username]", True, 1)},
                                                                             {"help", New CommandInfo("help", ShellCommandType.Shell, "Help page", "[command]", False, 0)},
                                                                             {"hwinfo", New CommandInfo("hwinfo", ShellCommandType.Shell, "Prints hardware information", "<HardwareType>", True, 1, False, True)},
                                                                             {"input", New CommandInfo("input", ShellCommandType.Shell, "Allows user to enter input", "<$variable> <question>", True, 2, False, False, False, False, True)},
                                                                             {"jsonbeautify", New CommandInfo("jsonbeautify", ShellCommandType.Shell, "Beautifies the JSON file", "<jsonfile> [output]", True, 1, False, True)},
                                                                             {"jsonminify", New CommandInfo("jsonminify", ShellCommandType.Shell, "Minifies the JSON file", "<jsonfile> [output]", True, 1, False, True)},
                                                                             {"list", New CommandInfo("list", ShellCommandType.Shell, "List file/folder contents in current folder", "[directory]", False, 0, False, True)},
                                                                             {"lockscreen", New CommandInfo("lockscreen", ShellCommandType.Shell, "Locks your screen with a password", "", False, 0)},
                                                                             {"logout", New CommandInfo("logout", ShellCommandType.Shell, "Logs you out", "", False, 0, False, False, True)},
                                                                             {"loteresp", New CommandInfo("loteresp", ShellCommandType.Shell, "Respond to love or hate comments.", "", False, 0)},
                                                                             {"lsdbgdev", New CommandInfo("lsdbgdev", ShellCommandType.Shell, "Lists debugging devices connected", "", False, 0, True, True)},
                                                                             {"lsmail", New CommandInfo("lsmail", ShellCommandType.Shell, "Lists all mails in the specific user.", "[emailAddress]", False, 0)},
                                                                             {"mathbee", New CommandInfo("mathbee", ShellCommandType.Shell, "See if you can solve mathematical equations on time", "", False, 0)},
                                                                             {"md", New CommandInfo("md", ShellCommandType.Shell, "Creates a directory", "<directory>", True, 1)},
                                                                             {"mkfile", New CommandInfo("mkfile", ShellCommandType.Shell, "Makes a new file", "<file>", True, 1)},
                                                                             {"mktheme", New CommandInfo("mktheme", ShellCommandType.Shell, "Makes a new theme", "<themeName>", True, 1)},
                                                                             {"modman", New CommandInfo("modman", ShellCommandType.Shell, "Manage your mods", "<start/stop/info/reload> <modfilename> | <list/reloadall/stopall/startall>", True, 1, True)},
                                                                             {"modmanual", New CommandInfo("modmanual", ShellCommandType.Shell, "Mod manual", "<ManualTitle/list>", True, 1)},
                                                                             {"move", New CommandInfo("move", ShellCommandType.Shell, "Moves a file to another directory", "<source> <target>", True, 2)},
                                                                             {"netinfo", New CommandInfo("netinfo", ShellCommandType.Shell, "Lists information about all available interfaces", "", False, 0, True, True)},
                                                                             {"perm", New CommandInfo("perm", ShellCommandType.Shell, "Manage permissions for users", "<userName> <Administrator/Disabled/Anonymous> <Allow/Disallow>", True, 3, True)},
                                                                             {"ping", New CommandInfo("ping", ShellCommandType.Shell, "Pings an address", "[times] <Address1> <Address2> ...", True, 1)},
                                                                             {"put", New CommandInfo("put", ShellCommandType.Shell, "Uploads a file to specified website", "<FileName> <URL> [username]", True, 2)},
                                                                             {"reboot", New CommandInfo("reboot", ShellCommandType.Shell, "Restarts your computer (WARNING: No syncing, because it is not a final kernel)", "[ip] [port]", False, 0)},
                                                                             {"reloadconfig", New CommandInfo("reloadconfig", ShellCommandType.Shell, "Reloads configuration file that is edited.", "", False, 0, True)},
                                                                             {"reloadsaver", New CommandInfo("reloadsaver", ShellCommandType.Shell, "Reloads screensaver file in KSMods", "<customsaver>", True, 1, True)},
                                                                             {"rexec", New CommandInfo("rexec", ShellCommandType.Shell, "Remotely executes a command to remote PC", "<address> [port] <command>", True, 2, True)},
                                                                             {"rm", New CommandInfo("rm", ShellCommandType.Shell, "Removes a directory or a file", "<directory/file>", True, 1)},
                                                                             {"rdebug", New CommandInfo("rdebug", ShellCommandType.Shell, "Enables or disables remote debugging.", "", False, 0, True)},
                                                                             {"reportbug", New CommandInfo("reportbug", ShellCommandType.Shell, "A bug reporting prompt.", "", False, 0)},
                                                                             {"rmuser", New CommandInfo("rmuser", ShellCommandType.Shell, "Removes a user from the list", "<Username>", True, 1, True)},
                                                                             {"rss", New CommandInfo("rss", ShellCommandType.Shell, "Opens an RSS shell to read the feeds", "[feedlink]", False, 0)},
                                                                             {"savecurrdir", New CommandInfo("savecurrdir", ShellCommandType.Shell, "Saves the current directory to kernel configuration file", "", False, 0, True)},
                                                                             {"savescreen", New CommandInfo("savescreen", ShellCommandType.Shell, "Saves your screen from burn outs", "[saver]", False, 0)},
                                                                             {"search", New CommandInfo("search", ShellCommandType.Shell, "Searches for specified string in the provided file using regular expressions", "<Regexp> <File>", True, 2)},
                                                                             {"searchword", New CommandInfo("searchword", ShellCommandType.Shell, "Searches for specified string in the provided file", "<StringEnclosedInDoubleQuotes> <File>", True, 2)},
                                                                             {"setsaver", New CommandInfo("setsaver", ShellCommandType.Shell, "Sets up kernel screensavers", "<customsaver/builtinsaver>", True, 1, True)},
                                                                             {"setthemes", New CommandInfo("setthemes", ShellCommandType.Shell, "Sets up kernel themes", "<Theme>", True, 1)},
                                                                             {"settings", New CommandInfo("settings", ShellCommandType.Shell, "Changes kernel configuration", "", False, 0, True)},
                                                                             {"set", New CommandInfo("set", ShellCommandType.Shell, "Sets a variable to a value in a script", "<$variable> <value>", True, 2, False, False, False, False, True)},
                                                                             {"sftp", New CommandInfo("sftp", ShellCommandType.Shell, "Lets you use an SSH FTP server", "<server>", False, 0)},
                                                                             {"shownotifs", New CommandInfo("shownotifs", ShellCommandType.Shell, "Shows all received notifications", "", False, 0)},
                                                                             {"showtd", New CommandInfo("showtd", ShellCommandType.Shell, "Shows date and time", "", False, 0)},
                                                                             {"showtdzone", New CommandInfo("showtdzone", ShellCommandType.Shell, "Shows date and time in zones", "<timezone/all>", True, 1, False, True)},
                                                                             {"shutdown", New CommandInfo("shutdown", ShellCommandType.Shell, "The kernel will be shut down", "[ip] [port]", False, 0)},
                                                                             {"speedpress", New CommandInfo("speedpress", ShellCommandType.Shell, "See if you can press a key on time", "<e/m/h>", True, 1)},
                                                                             {"spellbee", New CommandInfo("spellbee", ShellCommandType.Shell, "See if you can spell words correctly on time", "", False, 0)},
                                                                             {"sshell", New CommandInfo("sshell", ShellCommandType.Shell, "Connects to an SSH server.", "<address:port> <username>", True, 2)},
                                                                             {"sshcmd", New CommandInfo("sshcmd", ShellCommandType.Shell, "Connects to an SSH server to execute a command.", "<address:port> <username> ""<command>""", True, 3)},
                                                                             {"sumfile", New CommandInfo("sumfile", ShellCommandType.Shell, "Calculates file sums.", "<MD5/SHA1/SHA256/SHA384/SHA512/all> <file> [outputFile]", True, 2)},
                                                                             {"sumfiles", New CommandInfo("sumfiles", ShellCommandType.Shell, "Calculates sums of files in specified directory.", "<MD5/SHA1/SHA256/SHA384/SHA512/all> <dir> [outputFile]", True, 2)},
                                                                             {"sysinfo", New CommandInfo("sysinfo", ShellCommandType.Shell, "System information", "", False, 0)},
                                                                             {"unblockdbgdev", New CommandInfo("unblockdbgdev", ShellCommandType.Shell, "Unblock a debug device by IP address", "<ipaddress>", True, 1, True)},
                                                                             {"unzip", New CommandInfo("unzip", ShellCommandType.Shell, "Extracts a ZIP archive", "<zipfile> [path] [-createdir]", True, 1)},
                                                                             {"update", New CommandInfo("update", ShellCommandType.Shell, "System update", "", False, 0, True)},
                                                                             {"usermanual", New CommandInfo("usermanual", ShellCommandType.Shell, "Takes you to our GitHub Wiki.", "", False, 0)},
                                                                             {"verify", New CommandInfo("verify", ShellCommandType.Shell, "Verifies sanity of the file", "<MD5/SHA1/SHA256/SHA384/SHA512> <calculatedhash> <hashfile/expectedhash> <file>", True, 4)},
                                                                             {"weather", New CommandInfo("weather", ShellCommandType.Shell, "Shows weather info for specified city. Uses OpenWeatherMap.", "<CityID/CityName/listcities>", True, 1)},
                                                                             {"wrap", New CommandInfo("wrap", ShellCommandType.Shell, "Wraps the console output", "<command>", True, 1)},
                                                                             {"zip", New CommandInfo("zip", ShellCommandType.Shell, "Creates a ZIP archive", "<zipfile> <path> [-fast/-nocomp] [-nobasedir]", True, 2)},
                                                                             {"zipshell", New CommandInfo("zipshell", ShellCommandType.Shell, "Opens a ZIP archive", "<zipfile>", True, 1)}}

    ''' <summary>
    ''' Initializes the shell.
    ''' </summary>
    Public Sub InitializeShell()
        'Let CTRL+C cancel running command
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf TCancelCommand

        While True
            If LogoutRequested Then
                Wdbg("I", "Requested log out: {0}", LogoutRequested)
                LogoutRequested = False
                LoggedIn = False
                Exit Sub
            ElseIf Not InSaver Then
                Try
                    'Try to probe injected commands
                    Wdbg("I", "Probing injected commands using GetLine(True)...")
                    GetLine(True, "")

                    'Enable cursor (We put it here to avoid repeated "CursorVisible = True" statements in different command codes)
                    Console.CursorVisible = True

                    'Write a prompt
                    If DefConsoleOut IsNot Nothing Then
                        Console.SetOut(DefConsoleOut)
                    End If
                    CommandPromptWrite()
                    DisposeAll()

                    'Set an input color
                    Wdbg("I", "ColoredShell is {0}", ColoredShell)
                    SetInputColor()

                    'Wait for command
                    Wdbg("I", "Waiting for command")
                    EventManager.RaiseShellInitialized()
                    Dim strcommand As String = Console.ReadLine()

                    If Not InSaver Then
                        'Fire event of PreRaiseCommand
                        EventManager.RaisePreExecuteCommand(strcommand)

                        'Check for a type of command
                        If Not (strcommand = Nothing Or strcommand?.StartsWith(" ") = True) Then
                            Dim Done As Boolean = False
                            Dim Commands As String() = strcommand.Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
                            For Each Command As String In Commands
                                Dim Parts As String() = Command.SplitEncloseDoubleQuotes(" ")
                                Wdbg("I", "Mod commands probing started with {0} from {1}", Command, strcommand)
                                If modcmnds.Contains(Parts(0)) Then
                                    Done = True
                                    Wdbg("I", "Mod command: {0}", Parts(0))
                                    ExecuteModCommand(Command)
                                End If
                                Wdbg("I", "Aliases probing started with {0} from {1}", Command, strcommand)
                                If Aliases.Keys.Contains(Parts(0)) Then
                                    Done = True
                                    Wdbg("I", "Alias: {0}", Parts(0))
                                    Command = Command.Replace($"""{Parts(0)}""", Parts(0))
                                    ExecuteAlias(Command, Parts(0))
                                End If
                                If Done = False Then
                                    Wdbg("I", "Executing built-in command")
                                    GetLine(False, Command)
                                End If
                            Next
                        End If

                        'Fix race condition between shell initialization and starting the event handler thread
                        If strcommand Is Nothing Then
                            Thread.Sleep(30)
                        End If

                        'Fire an event of PostExecuteCommand
                        EventManager.RaisePostExecuteCommand(strcommand)
                    End If
                Catch ex As Exception
                    WStkTrc(ex)
                    W(DoTranslation("There was an error in the shell.") + vbNewLine + "Error {0}: {1}", True, ColTypes.Error, ex.GetType.FullName, ex.Message)
                    Continue While
                End Try
            End If
        End While
    End Sub

    ''' <summary>
    ''' Writes the input for command prompt
    ''' </summary>
    Public Sub CommandPromptWrite()

        Wdbg("I", "ShellPromptStyle = {0}", ShellPromptStyle)
        If ShellPromptStyle <> "" And Not maintenance Then
            Dim ParsedPromptStyle As String = ProbePlaces(ShellPromptStyle)
            ParsedPromptStyle.ConvertVTSequences
            W(ParsedPromptStyle, False, ColTypes.Gray)
            If adminList(CurrentUser) = True Then
                W(" # ", False, ColTypes.Gray)
            Else
                W(" $ ", False, ColTypes.Gray)
            End If
        ElseIf ShellPromptStyle = "" And Not maintenance Then
            If adminList(CurrentUser) = True Then
                W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, CurrentUser) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, HName) : W("]{0} # ", False, ColTypes.Gray, CurrDir)
            ElseIf maintenance Then
                W(DoTranslation("Maintenance Mode") + "> ", False, ColTypes.Gray)
            Else
                W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, CurrentUser) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, HName) : W("]{0} $ ", False, ColTypes.Gray, CurrDir)
            End If
        Else
            W(DoTranslation("Maintenance Mode") + "> ", False, ColTypes.Gray)
        End If

    End Sub

    ''' <summary>
    ''' Parses a specified command.
    ''' </summary>
    ''' <param name="ArgsMode">Specify if it runs using arguments</param>
    ''' <param name="strcommand">Specify command</param>
    ''' <param name="OutputPath">Optional (non-)neutralized output path</param>
    ''' <param name="IsInvokedByKernelArgument">Indicates whether it was invoked by kernel argument parse (for internal use only)</param>
    Public Sub GetLine(ByVal ArgsMode As Boolean, ByVal strcommand As String, Optional ByVal IsInvokedByKernelArgument As Boolean = False, Optional ByVal OutputPath As String = "")
        'If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
        Wdbg("I", "Does the command contain the redirection sign "">>>"" or "">>""? {0} and {1}", strcommand.Contains(">>>"), strcommand.Contains(">>"))
        Dim OutputTextWriter As StreamWriter
        Dim OutputStream As FileStream
        If strcommand.Contains(">>>") Then
            Wdbg("I", "Output redirection found with append.")
            DefConsoleOut = Console.Out
            Dim OutputFileName As String = strcommand.Substring(strcommand.LastIndexOf(">") + 2)
            OutputStream = New FileStream(NeutralizePath(OutputFileName), FileMode.Append, FileAccess.Write)
            OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
            Console.SetOut(OutputTextWriter)
            strcommand = strcommand.Replace(" >>> " + OutputFileName, "")
        ElseIf strcommand.Contains(">>") Then
            Wdbg("I", "Output redirection found with overwrite.")
            DefConsoleOut = Console.Out
            Dim OutputFileName As String = strcommand.Substring(strcommand.LastIndexOf(">") + 2)
            OutputStream = New FileStream(NeutralizePath(OutputFileName), FileMode.OpenOrCreate, FileAccess.Write)
            OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
            Console.SetOut(OutputTextWriter)
            strcommand = strcommand.Replace(" >> " + OutputFileName, "")
        End If

        'Checks to see if the user provided optional path
        If Not String.IsNullOrWhiteSpace(OutputPath) Then
            Wdbg("I", "Optional output redirection found using OutputPath ({0}).", OutputPath)
            DefConsoleOut = Console.Out
            OutputStream = New FileStream(NeutralizePath(OutputPath), FileMode.OpenOrCreate, FileAccess.Write)
            OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
            Console.SetOut(OutputTextWriter)
        End If

        'Reads command written by user
        Try
            If ArgsMode = False Then
                If Not (strcommand = Nothing Or strcommand.StartsWithAnyOf({" ", "#"}) = True) Then
                    Console.Title = $"{ConsoleTitle} - {strcommand}"

                    'Parse script command (if any)
                    Dim scriptArgs As List(Of String) = strcommand.Split({".uesh "}, StringSplitOptions.RemoveEmptyEntries).ToList
                    scriptArgs.RemoveAt(0)

                    'Get the index of the first space
                    Dim indexCmd As Integer = strcommand.IndexOf(" ")
                    Dim cmdArgs As String = strcommand 'Command with args
                    Wdbg("I", "Prototype indexCmd and strcommand: {0}, {1}", indexCmd, strcommand)
                    If indexCmd = -1 Then indexCmd = strcommand.Length
                    strcommand = strcommand.Substring(0, indexCmd)
                    Wdbg("I", "Finished indexCmd and strcommand: {0}, {1}", indexCmd, strcommand)

                    'Scan PATH for file existence and set file name as needed
                    Dim TargetFile As String = ""
                    Dim TargetFileName As String = ""
                    FileExistsInPath(strcommand, TargetFile)
                    If String.IsNullOrEmpty(TargetFile) Then TargetFile = NeutralizePath(strcommand)
                    If TryParsePath(TargetFile) Then TargetFileName = Path.GetFileName(TargetFile)

                    'Check to see if a user is able to execute a command
                    If Commands.ContainsKey(strcommand) Then
                        If adminList(CurrentUser) = False And Commands(strcommand).Strict Then
                            Wdbg("W", "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", strcommand)
                            W(DoTranslation("You don't have permission to use {0}"), True, ColTypes.Error, strcommand)
                        ElseIf maintenance = True And Commands(strcommand).NoMaintenance Then
                            Wdbg("W", "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", strcommand)
                            W(DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode."), True, ColTypes.Error, strcommand)
                        ElseIf IsInvokedByKernelArgument And (strcommand.StartsWith("logout") Or strcommand.StartsWith("shutdown") Or strcommand.StartsWith("reboot")) Then
                            Wdbg("W", "Cmd exec {0} failed: cmd is one of ""logout"" or ""shutdown"" or ""reboot""", strcommand)
                            W(DoTranslation("Shell message: Command {0} is not allowed to run on log in."), True, ColTypes.Error, strcommand)
                        ElseIf (adminList(CurrentUser) = True And Commands(strcommand).Strict) Or Commands.ContainsKey(strcommand) Then
                            Wdbg("I", "Cmd exec {0} succeeded. Running with {1}", strcommand, cmdArgs)
                            StartCommandThread = New Thread(AddressOf GetCommand.ExecuteCommand) With {.Name = "Shell Command Thread"}
                            StartCommandThread.Start(cmdArgs)
                            StartCommandThread.Join()
                        End If
                    ElseIf TryParsePath(TargetFile) Then
                        If File.Exists(TargetFile) And Not TargetFile.EndsWith(".uesh") Then
                            Wdbg("I", "Cmd exec {0} succeeded because file is found.", strcommand)
                            Try
                                'Create a new instance of process
                                If TryParsePath(TargetFile) Then
                                    cmdArgs = cmdArgs.Replace(TargetFileName, "")
                                    cmdArgs.RemoveNullsOrWhitespacesAtTheBeginning
                                    Wdbg("I", "Command: {0}, Arguments: {1}", TargetFile, cmdArgs)
                                    Dim CommandProcess As New Process
                                    Dim CommandProcessStart As New ProcessStartInfo With {.RedirectStandardInput = True,
                                                                                          .RedirectStandardOutput = True,
                                                                                          .RedirectStandardError = True,
                                                                                          .FileName = TargetFile,
                                                                                          .Arguments = cmdArgs,
                                                                                          .WorkingDirectory = CurrDir,
                                                                                          .CreateNoWindow = True,
                                                                                          .WindowStyle = ProcessWindowStyle.Hidden,
                                                                                          .UseShellExecute = False}
                                    CommandProcess.StartInfo = CommandProcessStart
                                    AddHandler CommandProcess.OutputDataReceived, AddressOf ExecutableOutput
                                    AddHandler CommandProcess.ErrorDataReceived, AddressOf ExecutableOutput

                                    'Start the process
                                    Wdbg("I", "Starting...")
                                    CommandProcess.Start()
                                    CommandProcess.BeginOutputReadLine()
                                    CommandProcess.BeginErrorReadLine()
                                    While Not CommandProcess.HasExited Or Not CancelRequested
                                        If CommandProcess.HasExited Then
                                            Exit While
                                        ElseIf CancelRequested Then
                                            CommandProcess.Kill()
                                            Exit While
                                        End If
                                    End While
                                End If
                            Catch ex As Exception
                                Wdbg("E", "Failed to start process: {0}", ex.Message)
                                W(DoTranslation("Failed to start ""{0}"": {1}"), True, ColTypes.Error, strcommand, ex.Message)
                                WStkTrc(ex)
                            End Try
                        ElseIf File.Exists(TargetFile) And TargetFile.EndsWith(".uesh") Then
                            Wdbg("I", "Cmd exec {0} succeeded because it's a UESH script.", strcommand)
                            Execute(TargetFile, scriptArgs.Join(" "))
                        Else
                            Wdbg("W", "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", strcommand, indexCmd)
                            W(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), True, ColTypes.Error, strcommand)
                        End If
                    Else
                        Wdbg("W", "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", strcommand, indexCmd)
                        W(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), True, ColTypes.Error, strcommand)
                    End If
                End If
            ElseIf ArgsMode = True And CommandFlag = True Then
                CommandFlag = False
                For Each cmd In InjectedCommands
                    GetLine(False, cmd, True)
                Next
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command.") + vbNewLine + DoTranslation("Error {0}: {1}") + vbNewLine + "{2}", True, ColTypes.Error,
                  ex.GetType.FullName, ex.Message, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command.") + vbNewLine + DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message)
            End If
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

    ''' <summary>
    ''' Translates alias to actual command, preserving arguments
    ''' </summary>
    ''' <param name="aliascmd">Specifies the alias with arguments</param>
    Sub ExecuteAlias(Base As String, ByVal aliascmd As String)
        Wdbg("I", "Translating alias {0} to {1}...", aliascmd, Aliases(aliascmd))
        Dim actualCmd As String = Base.Replace(aliascmd, Aliases(aliascmd))
        StartCommandThread = New Thread(AddressOf GetCommand.ExecuteCommand) With {.Name = "Shell Command Thread"}
        StartCommandThread.Start(actualCmd)
        StartCommandThread.Join()
    End Sub

    ''' <summary>
    ''' Handles executable output
    ''' </summary>
    ''' <param name="sendingProcess">Sender</param>
    ''' <param name="outLine">Output</param>
    Private Sub ExecutableOutput(sendingProcess As Object, outLine As DataReceivedEventArgs)
        Wdbg("I", outLine.Data)
        W(outLine.Data, True, ColTypes.Neutral)
    End Sub

End Module
