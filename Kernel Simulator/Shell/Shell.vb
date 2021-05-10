
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
    ''' Written command
    ''' </summary>
    Public strcommand As String
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
    ''' All mod commands
    ''' </summary>
    Public modcmnds As New ArrayList
    ''' <summary>
    ''' List of commands
    ''' </summary>
    Public Commands As New Dictionary(Of String, CommandInfo) From {{"adduser", New CommandInfo("adduser", ShellCommandType.Shell, True, 1, True, False, False, False)},
                                                                    {"alias", New CommandInfo("alias", ShellCommandType.Shell, True, 3, True, False, False, False)},
                                                                    {"arginj", New CommandInfo("arginj", ShellCommandType.Shell, True, 1, True, False, False, False)},
                                                                    {"beep", New CommandInfo("beep", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"blockdbgdev", New CommandInfo("blockdbgdev", ShellCommandType.Shell, True, 1, True, False, False, False)},
                                                                    {"calc", New CommandInfo("calc", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"cat", New CommandInfo("cat", ShellCommandType.Shell, True, 1, False, True, False, False)},
                                                                    {"cdbglog", New CommandInfo("cdbglog", ShellCommandType.Shell, False, 0, True, False, False, False)},
                                                                    {"chattr", New CommandInfo("chattr", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"chdir", New CommandInfo("chdir", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"chhostname", New CommandInfo("chhostname", ShellCommandType.Shell, True, 1, True, False, False, False)},
                                                                    {"chlang", New CommandInfo("chlang", ShellCommandType.Shell, True, 1, True, False, False, False)},
                                                                    {"chmal", New CommandInfo("chmal", ShellCommandType.Shell, False, 0, True, False, False, False)},
                                                                    {"chmotd", New CommandInfo("chmotd", ShellCommandType.Shell, False, 0, True, False, False, False)},
                                                                    {"choice", New CommandInfo("choice", ShellCommandType.Shell, True, 3, False, False, False, False)},
                                                                    {"chpwd", New CommandInfo("chpwd", ShellCommandType.Shell, True, 4, True, False, False, False)},
                                                                    {"chusrname", New CommandInfo("chusrname", ShellCommandType.Shell, True, 2, True, False, False, False)},
                                                                    {"cls", New CommandInfo("cls", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"copy", New CommandInfo("copy", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"dirinfo", New CommandInfo("dirinfo", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"disconndbgdev", New CommandInfo("disconndbgdev", ShellCommandType.Shell, True, 1, True, False, False, False)},
                                                                    {"dismissnotif", New CommandInfo("dismissnotif", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"echo", New CommandInfo("echo", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"edit", New CommandInfo("edit", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"fileinfo", New CommandInfo("fileinfo", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"firedevents", New CommandInfo("firedevents", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"ftp", New CommandInfo("ftp", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"get", New CommandInfo("get", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"help", New CommandInfo("help", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"hwinfo", New CommandInfo("hwinfo", ShellCommandType.Shell, False, 0, False, True, False, False)},
                                                                    {"input", New CommandInfo("input", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"list", New CommandInfo("list", ShellCommandType.Shell, False, 0, False, True, False, False)},
                                                                    {"lockscreen", New CommandInfo("lockscreen", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"logout", New CommandInfo("logout", ShellCommandType.Shell, False, 0, False, False, True, False)},
                                                                    {"loteresp", New CommandInfo("loteresp", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"lsdbgdev", New CommandInfo("lsdbgdev", ShellCommandType.Shell, False, 0, True, True, False, False)},
                                                                    {"lsmail", New CommandInfo("lsmail", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"mathbee", New CommandInfo("mathbee", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"md", New CommandInfo("md", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"mkfile", New CommandInfo("mkfile", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"mktheme", New CommandInfo("mktheme", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"move", New CommandInfo("move", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"netinfo", New CommandInfo("netinfo", ShellCommandType.Shell, False, 0, True, True, False, False)},
                                                                    {"perm", New CommandInfo("perm", ShellCommandType.Shell, True, 3, True, False, False, False)},
                                                                    {"ping", New CommandInfo("ping", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"put", New CommandInfo("put", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"reboot", New CommandInfo("reboot", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"reloadconfig", New CommandInfo("reloadconfig", ShellCommandType.Shell, False, 0, True, False, False, False)},
                                                                    {"reloadmods", New CommandInfo("reloadmods", ShellCommandType.Shell, False, 0, True, False, False, False)},
                                                                    {"reloadsaver", New CommandInfo("reloadsaver", ShellCommandType.Shell, True, 1, True, False, False, False)},
                                                                    {"rexec", New CommandInfo("rexec", ShellCommandType.Shell, True, 2, True, False, False, False)},
                                                                    {"rm", New CommandInfo("rm", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"rdebug", New CommandInfo("rdebug", ShellCommandType.Shell, False, 0, True, False, False, False)},
                                                                    {"reportbug", New CommandInfo("reportbug", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"rmuser", New CommandInfo("rmuser", ShellCommandType.Shell, True, 1, True, False, False, False)},
                                                                    {"savecurrdir", New CommandInfo("savecurrdir", ShellCommandType.Shell, False, 0, True, False, False, False)},
                                                                    {"savescreen", New CommandInfo("savescreen", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"search", New CommandInfo("search", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"setsaver", New CommandInfo("setsaver", ShellCommandType.Shell, True, 1, True, False, False, False)},
                                                                    {"setthemes", New CommandInfo("setthemes", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"settings", New CommandInfo("settings", ShellCommandType.Shell, False, 0, True, False, False, False)},
                                                                    {"set", New CommandInfo("set", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"sftp", New CommandInfo("sftp", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"shownotifs", New CommandInfo("shownotifs", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"showtd", New CommandInfo("showtd", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"showtdzone", New CommandInfo("showtdzone", ShellCommandType.Shell, True, 1, False, True, False, False)},
                                                                    {"shutdown", New CommandInfo("shutdown", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"spellbee", New CommandInfo("spellbee", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"sshell", New CommandInfo("sshell", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"sshcmd", New CommandInfo("sshcmd", ShellCommandType.Shell, True, 3, False, False, False, False)},
                                                                    {"sumfile", New CommandInfo("sumfile", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"sumfiles", New CommandInfo("sumfiles", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"sysinfo", New CommandInfo("sysinfo", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"unblockdbgdev", New CommandInfo("unblockdbgdev", ShellCommandType.Shell, True, 1, True, False, False, False)},
                                                                    {"unzip", New CommandInfo("unzip", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"update", New CommandInfo("update", ShellCommandType.Shell, False, 0, True, False, False, False)},
                                                                    {"usermanual", New CommandInfo("usermanual", ShellCommandType.Shell, False, 0, False, False, False, False)},
                                                                    {"verify", New CommandInfo("verify", ShellCommandType.Shell, True, 4, False, False, False, False)},
                                                                    {"weather", New CommandInfo("weather", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"wrap", New CommandInfo("wrap", ShellCommandType.Shell, True, 1, False, False, False, False)},
                                                                    {"zip", New CommandInfo("zip", ShellCommandType.Shell, True, 2, False, False, False, False)},
                                                                    {"zipshell", New CommandInfo("zipshell", ShellCommandType.Shell, True, 1, False, False, False, False)}}

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
                    If Not IsNothing(DefConsoleOut) Then
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
                    strcommand = Console.ReadLine()

                    If Not InSaver Then
                        'Fire event of PreRaiseCommand
                        EventManager.RaisePreExecuteCommand(strcommand)

                        'Check for a type of command
                        If Not (strcommand = Nothing Or strcommand?.StartsWith(" ") = True) Then
                            Dim Done As Boolean = False
                            Dim Commands As String() = strcommand.Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
                            For Each Command As String In Commands
                                Dim Parts As String() = Command.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
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
                                    ExecuteAlias(Command, Parts(0))
                                End If
                                If Done = False Then
                                    Wdbg("I", "Executing built-in command")
                                    GetLine(False, Command)
                                End If
                            Next
                        End If

                        'Fix race condition between shell initialization and starting the event handler thread
                        If IsNothing(strcommand) Then
                            Thread.Sleep(30)
                        End If

                        'Fire an event of PostExecuteCommand
                        EventManager.RaisePostExecuteCommand(strcommand)
                    End If
                Catch ex As Exception
                    WStkTrc(ex)
                    W(DoTranslation("There was an error in the shell.") + vbNewLine + "Error {0}: {1}", True, ColTypes.Err, Err.Number, ex.Message)
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
            If adminList(signedinusrnm) = True Then
                W(" # ", False, ColTypes.Gray)
            Else
                W(" $ ", False, ColTypes.Gray)
            End If
        ElseIf ShellPromptStyle = "" And Not maintenance Then
            If adminList(signedinusrnm) = True Then
                W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, signedinusrnm) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, HName) : W("]{0} # ", False, ColTypes.Gray, CurrDir)
            ElseIf maintenance Then
                W(DoTranslation("Maintenance Mode") + "> ", False, ColTypes.Gray)
            Else
                W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, signedinusrnm) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, HName) : W("]{0} $ ", False, ColTypes.Gray, CurrDir)
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
                If Not (strcommand = Nothing Or strcommand.StartsWith(" ") = True) Then
                    Console.Title = $"{ConsoleTitle} - {strcommand}"

                    'Parse script command (if any)
                    Dim scriptArgs As List(Of String) = strcommand.Split({".uesh "}, StringSplitOptions.RemoveEmptyEntries).ToList
                    scriptArgs.RemoveAt(0)

                    'Get the index of the first space
                    Dim indexCmd As Integer = strcommand.IndexOf(" ")
                    Dim cmdArgs As String = strcommand 'Command with args
                    Wdbg("I", "Prototype indexCmd and strcommand: {0}, {1}", indexCmd, strcommand)
                    If indexCmd = -1 Then indexCmd = strcommand.Count
                    strcommand = strcommand.Substring(0, indexCmd)
                    Wdbg("I", "Finished indexCmd and strcommand: {0}, {1}", indexCmd, strcommand)

                    'Scan PATH for file existence and set file name as needed
                    Dim TargetFile As String = ""
                    Dim TargetFileName As String
                    FileExistsInPath(strcommand, TargetFile)
                    TargetFileName = Path.GetFileName(TargetFile)

                    'Check to see if a user is able to execute a command
                    If Commands.ContainsKey(strcommand) Then
                        If adminList(signedinusrnm) = False And Commands(strcommand).Strict Then
                            Wdbg("W", "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", strcommand)
                            W(DoTranslation("You don't have permission to use {0}"), True, ColTypes.Err, strcommand)
                        ElseIf maintenance = True And Commands(strcommand).NoMaintenance Then
                            Wdbg("W", "Cmd exec {0} failed: In maintenance mode. {0} is in NoMaintenanceCmds", strcommand)
                            W(DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode."), True, ColTypes.Err, strcommand)
                        ElseIf IsInvokedByKernelArgument And (strcommand.StartsWith("logout") Or strcommand.StartsWith("shutdown") Or strcommand.StartsWith("reboot")) Then
                            Wdbg("W", "Cmd exec {0} failed: cmd is one of ""logout"" or ""shutdown"" or ""reboot""", strcommand)
                            W(DoTranslation("Shell message: Command {0} is not allowed to run on log in."), True, ColTypes.Err, strcommand)
                        ElseIf (adminList(signedinusrnm) = True And Commands(strcommand).Strict) Or Commands.ContainsKey(strcommand) Then
                            Wdbg("I", "Cmd exec {0} succeeded. Running with {1}", strcommand, cmdArgs)
                            StartCommandThread = New Thread(AddressOf GetCommand.ExecuteCommand)
                            StartCommandThread.Start(cmdArgs)
                            StartCommandThread.Join()
                        End If
                    Else
                        If File.Exists(TargetFile) And Not TargetFile.EndsWith(".uesh") Then
                            Wdbg("I", "Cmd exec {0} succeeded because file is found.", strcommand)
                            Try
                                'Create a new instance of process
                                cmdArgs = cmdArgs.Replace(TargetFileName, "")
                                cmdArgs.RemoveNullsOrWhitespacesAtTheBeginning
                                Wdbg("I", "Command: {0}, Arguments: {1}", TargetFile, cmdArgs)
                                Dim CommandProcess As New Process
                                Dim CommandProcessStart As New ProcessStartInfo With {.RedirectStandardInput = True,
                                                                                      .RedirectStandardOutput = True,
                                                                                      .RedirectStandardError = True,
                                                                                      .FileName = TargetFile,
                                                                                      .Arguments = cmdArgs,
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
                                CommandProcess.WaitForExit()
                            Catch ex As Exception
                                Wdbg("E", "Failed to start process: {0}", ex.Message)
                                W(DoTranslation("Failed to start ""{0}"": {1}"), True, ColTypes.Err, strcommand, ex.Message)
                                WStkTrc(ex)
                            End Try
                        ElseIf File.Exists(TargetFile) And TargetFile.EndsWith(".uesh") Then
                            Wdbg("I", "Cmd exec {0} succeeded because it's a UESH script.", strcommand)
                            Execute(TargetFile, scriptArgs.Join(" "))
                        Else
                            Wdbg("W", "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", strcommand, indexCmd)
                            W(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands."), True, ColTypes.Err, strcommand)
                        End If
                    End If
                End If
            ElseIf ArgsMode = True And CommandFlag = True Then
                CommandFlag = False
                For Each cmd In argcmds
                    GetLine(False, cmd, True)
                Next
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command.") + vbNewLine + DoTranslation("Error {0}: {1}") + vbNewLine + "{2}", True, ColTypes.Err,
                  Err.Number, ex.Message, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command.") + vbNewLine + DoTranslation("Error {0}: {1}"), True, ColTypes.Err, Err.Number, ex.Message)
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
        StartCommandThread = New Thread(AddressOf GetCommand.ExecuteCommand)
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
