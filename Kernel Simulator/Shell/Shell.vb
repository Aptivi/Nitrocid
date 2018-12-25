
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

    'Available Commands (availableCommands())
    'Admin-Only commands (strictCmds())
    Public ColoredShell As Boolean = True                   'To fix known bug
    Public strcommand As String                             'Written Command
    Public availableCommands() As String = {"help", "logout", "list", "chdir", "cdir", "read", "echo", "choice", "shutdown", "reboot", _
                                            "adduser", "chmotd", "chhostname", "lscomp", "hwprobe", "ping", "lsnet", _
                                            "lsnettree", "showtd", "chpwd", "sysinfo", "arginj", "panicsim", "setcolors", "rmuser", _
                                            "cls", "perm", "chusrname", "setthemes", "netinfo", "calc", "scical", "unitconv", "md", "rd", _
                                            "debuglog", "reloadconfig", "showtdzone", "alias", "chmal", "savescreen", "lockscreen", _
                                            "setsaver", "loadsaver", "noaliases", "ftp", "useddeps", "usermanual", "currency"}
    Public strictCmds() As String = {"adduser", "perm", "arginj", "chhostname", "chmotd", "chusrname", "rmuser", "netinfo", "debuglog", _
                                     "reloadconfig", "alias", "chmal", "setsaver", "loadsaver"}
    Public modcmnds As New ArrayList

    'For contributors: For each added command, you should also add a command in availableCommands array so there is no problems detecting your new command.
    '                  For each added admin command, you should also add a command in strictCmds array after performing above procedure so there is no problems 
    '                  checking if user has Admin permission to use your new admin command.

    Public Sub initializeShell()

        'Variables
        Dim Done As Boolean = False

        'Initialize Shell
        While True
            Try
                'Try to probe injected commands
                Wdbg("Probing injected commands using GetLine(True)...")
                getLine(True)

                'Enable cursor (We put it here to avoid repeated "CursorVisible = True" statements in different command codes.
                Console.CursorVisible = True

                'Write a prompt
                commandPromptWrite()
                DisposeExit.DisposeAll()

                'Set an input color
                Wdbg("ColoredShell = {0}", ColoredShell)
                If (ColoredShell = True) Then System.Console.ForegroundColor = CType(inputColor, ConsoleColor)

                'Wait for command
                EventManager.RaiseShellInitialized()
                strcommand = System.Console.ReadLine()

                'Fire event of PreRaiseCommand
                EventManager.RaisePreExecuteCommand()

                'Check for a type of command
                If Not (strcommand = Nothing Or strcommand.StartsWith(" ") = True) Then
                    'Don't make "End If <newline> If" be "ElseIf", or no commands can be run properly.
                    If (modcmnds.Count - 1 <> -1) Then
                        Wdbg("Mod commands probing started with {0}", strcommand)
                        For Each c As String In modcmnds
                            Dim Parts As String() = strcommand.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
                            If (Parts(0) = c And strcommand.StartsWith(Parts(0))) Then
                                Done = True
                                GetModCommand.ExecuteModCommand(strcommand)
                            End If
                        Next
                    End If
                    If (aliases.Count - 1 <> -1) Then
                        Wdbg("Aliases probing started with {0}", strcommand)
                        For Each a As String In aliases.Keys
                            Wdbg("strcommand, a = {0}, {1}", strcommand, a)
                            If (strcommand.StartsWith(a)) Then
                                Done = True
                                GetAlias.ExecuteAlias(a)
                            End If
                        Next
                    End If
                    If (Done = False) Then
                        Wdbg("Executing built-in command")
                        getLine()
                    End If
                End If

                'Fire an event of PostExecuteCommand
                EventManager.RaisePostExecuteCommand()
            Catch ex As Exception
                If (DebugMode = True) Then
                    Wln(DoTranslation("There was an error in the shell.", currentLang) + vbNewLine + "Error {0}: {1}" + vbNewLine + "{2}", "neutralText",
                        Err.Number, Err.Description, ex.StackTrace)
                    Wdbg(ex.StackTrace, True)
                Else
                    Wln(DoTranslation("There was an error in the shell.", currentLang) + vbNewLine + "Error {0}: {1}", "neutralText", Err.Number, Err.Description)
                End If
                Continue While
            End Try
        End While

    End Sub

    Public Sub commandPromptWrite()

        If adminList(signedinusrnm) = True Then
            W("[", "def") : W("{0}", "userName", signedinusrnm) : W("@", "def") : W("{0}", "hostName", HName) : W("]{0} # ", "def", currDir)
        ElseIf (maintenance = True) Then
            W("Maintenance Mode>", "def")
        Else
            W("[", "def") : W("{0}", "userName", signedinusrnm) : W("@", "def") : W("{0}", "hostName", HName) : W("]{0} $ ", "def", currDir)
        End If

    End Sub

    Public Sub getLine(Optional ByVal ArgsMode As Boolean = False)

        'Reads command written by user
        Try
            If ArgsMode = False Then
                If Not (strcommand = Nothing Or strcommand.StartsWith(" ") = True) Then
                    Dim groupCmds() As String = strcommand.Split({" : "}, StringSplitOptions.RemoveEmptyEntries)
                    For Each cmd In groupCmds
                        Dim indexCmd As Integer = cmd.IndexOf(" ")
                        If (indexCmd = -1) Then
                            indexCmd = cmd.Count
                            cmd = cmd.Substring(0, indexCmd)
                        End If
                        If (adminList(signedinusrnm) = False And strictCmds.Contains(cmd.Substring(0, indexCmd)) = True) Then
                            Wdbg("Cmd exec {0} failed: adminList.ASSERT(signedinusrnm) = False, strictCmds.Cont({0}.Substr(0, {1})) = True", cmd.Substring(0, indexCmd), indexCmd)
                            Wln(DoTranslation("You don't have permission to use {0}", currentLang), "neutralText", cmd.Substring(0, indexCmd))
                        ElseIf (maintenance = True And cmd.Contains("logout")) Then
                            Wdbg("Cmd exec {0} failed: maintenance = True && input.Cont(""logout"") = True", cmd.Substring(0, indexCmd), indexCmd)
                            Wln(DoTranslation("Shell message: The requested command {0} is not allowed to run in maintenance mode.", currentLang), "neutralText", cmd.Substring(0, indexCmd))
                        ElseIf (adminList(signedinusrnm) = True And strictCmds.Contains(cmd.Substring(0, indexCmd)) = True) Or (availableCommands.Contains(cmd.Substring(0, indexCmd))) Then
                            Wdbg("Cmd exec: {0}", cmd.Substring(0, indexCmd))
                            GetCommand.ExecuteCommand(cmd)
                        Else
                            Wdbg("Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", cmd.Substring(0, indexCmd), indexCmd)
                            Wln(DoTranslation("Shell message: The requested command {0} is not found. See 'help' for available commands.", currentLang), "neutralText", cmd.Substring(0, indexCmd))
                        End If
                    Next
                End If
            ElseIf (ArgsMode = True And CommandFlag = True) Then
                CommandFlag = False
                For Each cmd In argcmds
                    Dim indexCmd As Integer = cmd.IndexOf(" ")
                    If (indexCmd = -1) Then
                        indexCmd = cmd.Count
                        cmd = cmd.Substring(0, indexCmd)
                    End If
                    If (availableCommands.Contains(cmd.Substring(0, indexCmd))) Then
                        If Not (cmd = Nothing Or cmd.StartsWith(" ") = True) Then
                            If (adminList(signedinusrnm) = True And strictCmds.Contains(cmd.Substring(0, indexCmd)) = True) Then
                                Wdbg("Cmd exec: {0}", cmd.Substring(0, indexCmd))
                                GetCommand.ExecuteCommand(cmd)
                            ElseIf (adminList(signedinusrnm) = False And strictCmds.Contains(cmd.Substring(0, indexCmd)) = True) Then
                                Wdbg("Cmd exec {0} failed: adminList.ASSERT(signedinusrnm) = False, strictCmds.Cont({0}.Substr(0, {1})) = True", cmd.Substring(0, indexCmd), indexCmd)
                                Wln(DoTranslation("You don't have permission to use {0}", currentLang), "neutralText", cmd.Substring(0, indexCmd))
                            ElseIf (cmd = "logout" Or cmd = "shutdown" Or cmd = "reboot") Then
                                Wdbg("Cmd exec {0} failed: {0} = (""logout"" | ""shutdown"" | ""reboot"") = True", cmd.Substring(0, indexCmd))
                                Wln(DoTranslation("Shell message: Command {0} is not allowed to run on log in.", currentLang), "neutralText", cmd)
                            Else
                                Wdbg("Cmd exec: {0}", cmd.Substring(0, indexCmd))
                                GetCommand.ExecuteCommand(cmd)
                            End If
                        End If
                    Else
                        Wdbg("Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", cmd.Substring(0, indexCmd), indexCmd)
                        Wln(DoTranslation("Shell message: The requested command {0} is not found.", currentLang), "neutralText", cmd.Substring(0, cmd.Count - 1))
                    End If
                Next
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln(DoTranslation("Error trying to execute command.", currentLang) + vbNewLine + DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", "neutralText",
                    Err.Number, Err.Description, ex.StackTrace)
                Wdbg(ex.StackTrace, True)
            Else
                Wln(DoTranslation("Error trying to execute command.", currentLang) + vbNewLine + DoTranslation("Error {0}: {1}", currentLang), "neutralText", Err.Number, Err.Description)
            End If
        End Try

    End Sub

End Module
