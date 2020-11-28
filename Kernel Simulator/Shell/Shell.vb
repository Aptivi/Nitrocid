
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

    'Available Commands  (availableCommands)
    'Admin-Only commands (strictCmds)
    'Obsolete commands   (obsoleteCmds)
    'For contributors: For each added command, you should add a command to availableCommands array so there are no problems detecting your new command.
    '                  For each added admin command, you should add a command to strictCmds array after performing above procedure so there are no problems checking if user has Admin permission to use your new admin command.
    '                  For each obsolete command, you should add a command to obsoleteCmds array so there are no problems checking if your command is obsolete.
    Public ColoredShell As Boolean = True                   'To fix known bug
    Public strcommand As String                             'Written Command
    Public availableCommands() As String = {"help", "logout", "list", "chdir", "cdir", "read", "shutdown", "reboot", "adduser", "chmotd",
                                            "chhostname", "showtd", "chpwd", "sysinfo", "arginj", "setcolors", "rmuser", "cls", "perm", "chusrname",
                                            "setthemes", "netinfo", "md", "rm", "debuglog", "reloadconfig", "showtdzone", "alias", "chmal",
                                            "savescreen", "lockscreen", "setsaver", "reloadsaver", "ftp", "usermanual", "cdbglog", "chlang",
                                            "reloadmods", "get", "put", "lsdbgdev", "disconndbgdev", "move", "copy", "search", "listdrives",
                                            "listparts", "sumfile", "rdebug", "spellbee", "mathbee", "loteresp", "sshell", "bsynth", "shownotifs",
                                            "dismissnotif", "rexec", "calc", "update", "sumfiles", "lsmail", "echo", "choice", "beep", "input", "mkfile",
                                            "edit", "blockdbgdev", "unblockdbgdev", "settings", "weather", "fileinfo", "dirinfo", "chattr", "ping", "verify"}
    Public strictCmds() As String = {"adduser", "perm", "arginj", "chhostname", "chmotd", "chusrname", "chpwd", "rmuser", "netinfo", "debuglog",
                                     "reloadconfig", "alias", "chmal", "setsaver", "reloadsaver", "cdbglog", "chlang", "reloadmods", "lsdbgdev", "disconndbgdev",
                                     "listdrives", "listparts", "rdebug", "rexec", "update", "blockdbgdev", "unblockdbgdev", "settings"}
    Public obsoleteCmds() As String = {}
    Public modcmnds As New ArrayList

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

                    'Enable cursor (We put it here to avoid repeated "CursorVisible = True" statements in different command codes.
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

                        'When pressing CTRL+C on shell after command execution, it can generate another prompt without making newline, so fix this.
                        If IsNothing(strcommand) Then
                            Console.WriteLine()
                            Thread.Sleep(30) 'This is to fix race condition between shell initialization and starting the event handler thread
                        End If

                        'Fire an event of PostExecuteCommand
                        EventManager.RaisePostExecuteCommand(strcommand)
                    End If
                Catch ex As Exception
                    If DebugMode = True Then
                        W(DoTranslation("There was an error in the shell.", currentLang) + vbNewLine + "Error {0}: {1}" + vbNewLine + "{2}", True, ColTypes.Err,
                          Err.Number, ex.Message, ex.StackTrace)
                        WStkTrc(ex)
                    Else
                        W(DoTranslation("There was an error in the shell.", currentLang) + vbNewLine + "Error {0}: {1}", True, ColTypes.Err, Err.Number, ex.Message)
                    End If
                    Continue While
                End Try
            End If
        End While
    End Sub

    ''' <summary>
    ''' Writes the input for command prompt
    ''' </summary>
    Public Sub CommandPromptWrite()

        If adminList(signedinusrnm) = True Then
            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, signedinusrnm) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, HName) : W("]{0} # ", False, ColTypes.Gray, CurrDir)
        ElseIf maintenance = True Then
            W("Maintenance Mode> ", False, ColTypes.Gray)
        Else
            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, signedinusrnm) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, HName) : W("]{0} $ ", False, ColTypes.Gray, CurrDir)
        End If

    End Sub

    ''' <summary>
    ''' Parses a specified command.
    ''' </summary>
    ''' <param name="ArgsMode">Specify if it runs using arguments</param>
    ''' <param name="strcommand">Specify command</param>
    Public Sub GetLine(ByVal ArgsMode As Boolean, ByVal strcommand As String)
        'If requested command has output redirection sign after arguments, remove it from final command string and set output to that file
        Wdbg("I", "Does the command contain the redirection sign "">>>"" or "">>""? {0} and {1}", strcommand.Contains(">>>"), strcommand.Contains(">>"))
        Dim OutputTextWriter As StreamWriter
        Dim OutputStream As FileStream
        If strcommand.Contains(">>>") Then
            Wdbg("I", "Output redirection found with append.")
            DefConsoleOut = Console.Out
            Dim OutputFileName As String = strcommand.Substring(strcommand.LastIndexOf(">") + 2)
            OutputStream = New FileStream(CurrDir + "/" + OutputFileName, FileMode.Append, FileAccess.Write)
            OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
            Console.SetOut(OutputTextWriter)
            strcommand = strcommand.Replace(" >>> " + OutputFileName, "")
        ElseIf strcommand.Contains(">>") Then
            Wdbg("I", "Output redirection found with overwrite.")
            DefConsoleOut = Console.Out
            Dim OutputFileName As String = strcommand.Substring(strcommand.LastIndexOf(">") + 2)
            OutputStream = New FileStream(CurrDir + "/" + OutputFileName, FileMode.OpenOrCreate, FileAccess.Write)
            OutputTextWriter = New StreamWriter(OutputStream) With {.AutoFlush = True}
            Console.SetOut(OutputTextWriter)
            strcommand = strcommand.Replace(" >> " + OutputFileName, "")
        End If

        'Reads command written by user
        Try
            If ArgsMode = False Then
                If Not (strcommand = Nothing Or strcommand.StartsWith(" ") = True) Then
                    Console.Title = $"{ConsoleTitle} - {strcommand}"

                    'Parse script command (if any)
                    Dim scriptArgs As List(Of String) = strcommand.Split({".uesh "}, StringSplitOptions.RemoveEmptyEntries).ToList
                    Dim scriptCmd As String = scriptArgs(0)
                    If scriptCmd.StartsWith("""") And scriptCmd.EndsWith("""") Then
                        scriptCmd = scriptCmd.Replace("""", "")
                    End If
                    If Not scriptCmd.EndsWith(".uesh") Then
                        scriptCmd += ".uesh"
                    End If
                    scriptArgs.RemoveAt(0)

                    'Get the index of the first space
                    Dim indexCmd As Integer = strcommand.IndexOf(" ")
                    Dim cmdArgs As String = strcommand 'Command with args
                    Wdbg("I", "Prototype indexCmd and strcommand: {0}, {1}", indexCmd, strcommand)
                    If indexCmd = -1 Then indexCmd = strcommand.Count
                    strcommand = strcommand.Substring(0, indexCmd)
                    Wdbg("I", "Finished indexCmd and strcommand: {0}, {1}", indexCmd, strcommand)

                    'Check to see if a user is able to execute a command
                    If adminList(signedinusrnm) = False And strictCmds.Contains(strcommand) = True Then
                        Wdbg("W", "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", strcommand)
                        W(DoTranslation("You don't have permission to use {0}", currentLang), True, ColTypes.Err, strcommand)
                    ElseIf maintenance = True And strcommand.Contains("logout") Then
                        Wdbg("W", "Cmd exec {0} failed: In maintenance mode. Assertion of input.Contains(""logout"") is True", strcommand)
                        W(DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode.", currentLang), True, ColTypes.Err, strcommand)
                    ElseIf (adminList(signedinusrnm) = True And strictCmds.Contains(strcommand) = True) Or availableCommands.Contains(strcommand) Then
                        Wdbg("W", "Cmd exec {0} succeeded", strcommand)
                        StartCommandThread = New Thread(AddressOf GetCommand.ExecuteCommand)
                        StartCommandThread.Start(cmdArgs)
                        StartCommandThread.Join()
                    ElseIf File.Exists(Path.GetFullPath(CurrDir + "/" + scriptCmd)) And scriptCmd.EndsWith(".uesh") Then
                        Wdbg("W", "Cmd exec {0} succeeded because it's a UESH script.", scriptCmd)
                        Execute(Path.GetFullPath(CurrDir + "/" + scriptCmd), scriptArgs.Join(" "))
                    Else
                        Wdbg("W", "Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", strcommand, indexCmd)
                        W(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands.", currentLang), True, ColTypes.Err, strcommand)
                    End If
                End If
            ElseIf ArgsMode = True And CommandFlag = True Then
                CommandFlag = False
                For Each cmd In argcmds
                    Console.Title = $"{ConsoleTitle} - {cmd}"
                    Dim scriptArgs As List(Of String) = strcommand.Split({".uesh "}, StringSplitOptions.RemoveEmptyEntries).ToList
                    Dim scriptCmd As String = scriptArgs(0)
                    If scriptCmd.StartsWith("""") And scriptCmd.EndsWith("""") Then
                        scriptCmd = scriptCmd.Replace("""", "")
                    End If
                    If Not scriptCmd.EndsWith(".uesh") Then
                        scriptCmd += ".uesh"
                    End If
                    scriptArgs.RemoveAt(0)

                    'Get the index of the first space
                    Dim indexCmd As Integer = cmd.IndexOf(" ")
                    Dim cmdArgs As String = cmd 'Command with args
                    Wdbg("I", "Prototype indexCmd and cmd: {0}, {1}", indexCmd, cmd)
                    If indexCmd = -1 Then indexCmd = cmd.Count
                    cmd = cmd.Substring(0, indexCmd)
                    Wdbg("I", "Finished indexCmd and cmd: {0}, {1}", indexCmd, cmd)

                    'Check to see if a user is able to execute a command
                    If availableCommands.Contains(cmd) Then
                        If Not (cmdArgs = Nothing Or cmdArgs.StartsWith(" ") = True) Then
                            If adminList(signedinusrnm) = True And strictCmds.Contains(cmd) = True Then
                                Wdbg("W", "Cmd exec {0} succeeded", cmd)
                                StartCommandThread = New Thread(AddressOf GetCommand.ExecuteCommand)
                                StartCommandThread.Start(cmdArgs)
                                StartCommandThread.Join()
                            ElseIf adminList(signedinusrnm) = False And strictCmds.Contains(cmd) = True Then
                                Wdbg("W", "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", cmd)
                                W(DoTranslation("You don't have permission to use {0}", currentLang), True, ColTypes.Err, cmd)
                            ElseIf cmd = "logout" Or cmd = "shutdown" Or cmd = "reboot" Then
                                Wdbg("W", "Cmd exec {0} failed: cmd is one of ""logout"" or ""shutdown"" or ""reboot""", cmd)
                                W(DoTranslation("Shell message: Command {0} is not allowed to run on log in.", currentLang), True, ColTypes.Err, cmd)
                            ElseIf File.Exists(Path.GetFullPath(CurrDir + "/" + strcommand)) And strcommand.EndsWith(".uesh") Then
                                Wdbg("W", "Cmd exec {0} succeeded because it's a UESH script.", strcommand)
                                Execute(Path.GetFullPath(CurrDir + "/" + strcommand), scriptArgs.Join(" "))
                            Else
                                Wdbg("W", "Cmd exec {0} succeeded", cmd)
                                StartCommandThread = New Thread(AddressOf GetCommand.ExecuteCommand)
                                StartCommandThread.Start(cmdArgs)
                                StartCommandThread.Join()
                            End If
                        End If
                    Else
                        Wdbg("W", "Cmd exec {0} failed: availableCmds.Contains({0}) is False", cmd)
                        W(DoTranslation("Shell message: The requested command {0} is not found.", currentLang), True, ColTypes.Err, cmd)
                    End If
                Next
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command.", currentLang) + vbNewLine + DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Err,
                  Err.Number, ex.Message, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command.", currentLang) + vbNewLine + DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Err, Err.Number, ex.Message)
            End If
        End Try
        Console.Title = ConsoleTitle

        'Restore console output to its original state if output redirection is used
#Disable Warning BC42104
        If strcommand.Contains(">>>") Then
            Console.SetOut(DefConsoleOut)
            OutputTextWriter.Close()
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

End Module
