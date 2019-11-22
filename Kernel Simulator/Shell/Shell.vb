
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Public Module Shell

    'Available Commands  (availableCommands)
    'Admin-Only commands (strictCmds)
    'Obsolete commands   (obsoleteCmds)
    Public ColoredShell As Boolean = True                   'To fix known bug
    Public strcommand As String                             'Written Command
    Public availableCommands() As String = {"help", "logout", "list", "chdir", "cdir", "read", "shutdown", "reboot", "adduser", "chmotd",
                                            "chhostname", "showtd", "chpwd", "sysinfo", "arginj", "setcolors", "rmuser", "cls", "perm", "chusrname",
                                            "setthemes", "netinfo", "md", "rd", "debuglog", "reloadconfig", "showtdzone", "alias", "chmal",
                                            "savescreen", "lockscreen", "setsaver", "reloadsaver", "ftp", "usermanual", "cdbglog", "sses", "chlang",
                                            "reloadmods", "get", "lsdbgdev", "disconndbgdev", "lset", "move", "copy", "search", "listdrives",
                                            "listparts", "sumfile", "rdebug", "speak", "spellbee", "mathbee", "loteresp",
                                            "sshell", "bsynth"}
    Public strictCmds() As String = {"adduser", "perm", "arginj", "chhostname", "chmotd", "chusrname", "rmuser", "netinfo", "debuglog",
                                     "reloadconfig", "alias", "chmal", "setsaver", "reloadsaver", "cdbglog", "chlang", "reloadmods", "lsdbgdev", "disconndbgdev",
                                     "lset", "listdrives", "listparts", "rdebug"}
    Public obsoleteCmds() As String = {}
    Public modcmnds As New ArrayList

    'For contributors: For each added command, you should add a command to availableCommands array so there is no problems detecting your new command.
    '                  For each added admin command, you should add a command to strictCmds array after performing above procedure so there are no problems checking if user has Admin permission to use your new admin command.
    '                  For each obsolete command, you should add a command to obsoleteCmds array so there are no problems checking if your command is obsolete.
    'Initialize Shell
    Public Sub InitializeShell()
        While True
            If LogoutRequested Then
                Wdbg("Requested log out: {0}", LogoutRequested)
                LogoutRequested = False
                Exit Sub
            Else
                Try
                    'Try to probe injected commands
                    Wdbg("Probing injected commands using GetLine(True)...")
                    GetLine(True)

                    'Enable cursor (We put it here to avoid repeated "CursorVisible = True" statements in different command codes.
                    Console.CursorVisible = True

                    'Write a prompt
                    CommandPromptWrite()
                    DisposeAll()

                    'Set an input color
                    Wdbg("ColoredShell is {0}", ColoredShell)
                    If ColoredShell = True Then Console.ForegroundColor = CType(inputColor, ConsoleColor)

                    'Wait for command
                    EventManager.RaiseShellInitialized()
                    strcommand = Console.ReadLine()

                    'Fire event of PreRaiseCommand
                    EventManager.RaisePreExecuteCommand()

                    'Check for a type of command
                    If Not (strcommand = Nothing Or strcommand.StartsWith(" ") = True) Then
                        Dim Done As Boolean = False
                        Dim Parts As String() = strcommand.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                        Wdbg("Mod commands probing started with {0}", strcommand)
                        For Each c As String In modcmnds
                            If Parts(0) = c Then
                                Done = True
                                Wdbg("Mod command: {0}", strcommand)
                                ExecuteModCommand(strcommand)
                            End If
                        Next
                        Wdbg("Aliases probing started with {0}", strcommand)
                        For Each a As String In aliases.Keys
                            If Parts(0) = a Then
                                Done = True
                                Wdbg("Alias: {0}", a)
                                ExecuteAlias(a)
                            End If
                        Next
                        If Done = False Then
                            Wdbg("Executing built-in command")
                            GetLine()
                        End If
                    End If

                    'Fire an event of PostExecuteCommand
                    EventManager.RaisePostExecuteCommand()
                Catch ex As Exception
                    If DebugMode = True Then
                        W(DoTranslation("There was an error in the shell.", currentLang) + vbNewLine + "Error {0}: {1}" + vbNewLine + "{2}", True, ColTypes.Neutral,
                            Err.Number, Err.Description, ex.StackTrace)
                        WStkTrc(ex)
                    Else
                        W(DoTranslation("There was an error in the shell.", currentLang) + vbNewLine + "Error {0}: {1}", True, ColTypes.Neutral, Err.Number, Err.Description)
                    End If
                    Continue While
                End Try
            End If
        End While
    End Sub

    Public Sub CommandPromptWrite()

        If adminList(signedinusrnm) = True Then
            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, signedinusrnm) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, HName) : W("]{0} # ", False, ColTypes.Gray, CurrDir)
        ElseIf maintenance = True Then
            W("Maintenance Mode>", False, ColTypes.Gray)
        Else
            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, signedinusrnm) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, HName) : W("]{0} $ ", False, ColTypes.Gray, CurrDir)
        End If

    End Sub

    Public Sub GetLine(Optional ByVal ArgsMode As Boolean = False)

        'Reads command written by user
        Try
            If ArgsMode = False Then
                If Not (strcommand = Nothing Or strcommand.StartsWith(" ") = True) Then
                    Dim groupCmds() As String = strcommand.Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
                    For Each cmd In groupCmds
                        'Get the index of the first space
                        Dim indexCmd As Integer = cmd.IndexOf(" ")
                        Dim cmdArgs As String = cmd 'Command with args
                        Wdbg("Prototype indexCmd and cmd: {0}, {1}", indexCmd, cmd)
                        If indexCmd = -1 Then indexCmd = cmd.Count
                        cmd = cmd.Substring(0, indexCmd)
                        Wdbg("Finished indexCmd and cmd: {0}, {1}", indexCmd, cmd)

                        'Check to see if a user is able to execute a command
                        If adminList(signedinusrnm) = False And strictCmds.Contains(cmd) = True Then
                            Wdbg("Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", cmd)
                            W(DoTranslation("You don't have permission to use {0}", currentLang), True, ColTypes.Neutral, cmd)
                        ElseIf maintenance = True And cmd.Contains("logout") Then
                            Wdbg("Cmd exec {0} failed: In maintenance mode. Assertion of input.Contains(""logout"") is True", cmd)
                            W(DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode.", currentLang), True, ColTypes.Neutral, cmd)
                        ElseIf (adminList(signedinusrnm) = True And strictCmds.Contains(cmd) = True) Or availableCommands.Contains(cmd) Then
                            Wdbg("Cmd exec {0} succeeded", cmd)
                            GetCommand.ExecuteCommand(cmdArgs)
                        Else
                            Wdbg("Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", cmd, indexCmd)
                            W(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands.", currentLang), True, ColTypes.Neutral, cmd)
                        End If
                    Next
                End If
            ElseIf ArgsMode = True And CommandFlag = True Then
                CommandFlag = False
                For Each cmd In argcmds
                    'Get the index of the first space
                    Dim indexCmd As Integer = cmd.IndexOf(" ")
                    Dim cmdArgs As String = cmd 'Command with args
                    Wdbg("Prototype indexCmd and cmd: {0}, {1}", indexCmd, cmd)
                    If indexCmd = -1 Then indexCmd = cmd.Count
                    cmd = cmd.Substring(0, indexCmd)
                    Wdbg("Finished indexCmd and cmd: {0}, {1}", indexCmd, cmd)

                    'Check to see if a user is able to execute a command
                    If availableCommands.Contains(cmd) Then
                        If Not (cmdArgs = Nothing Or cmdArgs.StartsWith(" ") = True) Then
                            If adminList(signedinusrnm) = True And strictCmds.Contains(cmd) = True Then
                                Wdbg("Cmd exec {0} succeeded", cmd)
                                GetCommand.ExecuteCommand(cmdArgs)
                            ElseIf adminList(signedinusrnm) = False And strictCmds.Contains(cmd) = True Then
                                Wdbg("Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", cmd)
                                W(DoTranslation("You don't have permission to use {0}", currentLang), True, ColTypes.Neutral, cmd)
                            ElseIf cmd = "logout" Or cmd = "shutdown" Or cmd = "reboot" Then
                                Wdbg("Cmd exec {0} failed: cmd is one of ""logout"" or ""shutdown"" or ""reboot""", cmd)
                                W(DoTranslation("Shell message: Command {0} is not allowed to run on log in.", currentLang), True, ColTypes.Neutral, cmd)
                            Else
                                Wdbg("Cmd exec {0} succeeded", cmd)
                                GetCommand.ExecuteCommand(cmdArgs)
                            End If
                        End If
                    Else
                        Wdbg("Cmd exec {0} failed: availableCmds.Contains({0}) is False", cmd)
                        W(DoTranslation("Shell message: The requested command {0} is not found.", currentLang), True, ColTypes.Neutral, cmd)
                    End If
                Next
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command.", currentLang) + vbNewLine + DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Neutral,
                  Err.Number, Err.Description, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command.", currentLang) + vbNewLine + DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Neutral, Err.Number, Err.Description)
            End If
        End Try

    End Sub

End Module
