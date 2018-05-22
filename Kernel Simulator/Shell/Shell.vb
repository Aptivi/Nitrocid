
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

Module Shell

    'Available Commands (availableCommands())
    'Admin-Only commands (strictCmds())
    Public ueshversion As String = "0.0.4"                  'Current shell version
    Public strcommand As String                             'Written Command
    Public availableCommands() As String = {"help", "logout", "version", "currentdir", "list", "changedir", "cdir", "ls", "chdir", "cd", "read", "echo", "choice", _
                                            "lsdrivers", "shutdown", "reboot", "disco", "future-eyes-destroyer", "beep", "annoying-sound", "adduser", "chmotd", _
                                            "chhostname", "showmotd", "fed", "hwprobe", "ping", "lsnet", "lsnettree", "showtd", "chpwd", "sysinfo", "arginj", _
                                            "panicsim", "setcolors", "rmuser", "cls", "perm", "chusrname", "setthemes", "netinfo", "calc", "scical", "unitconv", _
                                            "md", "mkdir", "rd", "rmdir", "debuglog", "reloadconfig"}
    Public strictCmds() As String = {"adduser", "perm", "arginj", "chhostname", "chmotd", "chusrname", "rmuser", "netinfo", "debuglog", "reloadconfig"}

    'For contributors: For each added command, you should also add a command in availableCommands array so there is no problems detecting your new command.
    '                  For each added admin command, you should also add a command in strictCmds array after performing above procedure so there is no problems 
    '                  checking if user has Admin permission to use your new admin command.

    Sub initializeShell()

        'Initialize Shell
        getLine(True)
        commandPromptWrite()
        DisposeExit.DisposeAll()
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        strcommand = System.Console.ReadLine()
        getLine()

    End Sub

    Sub commandPromptWrite()

            If adminList(signedinusrnm) = True Then
                W("[", "def") : W("{0}", "userName", signedinusrnm)
                W("@", "def") : W("{0}", "hostName", My.Settings.HostName)
                W("]{0} # ", "def", currDir)
            Else
                W("[", "def") : W("{0}", "userName", signedinusrnm)
                W("@", "def") : W("{0}", "hostName", My.Settings.HostName)
                W("]{0} $ ", "def", currDir)
            End If

    End Sub

    Sub getLine(Optional ByVal ArgsMode As Boolean = False)

        'Reads command written by user
        Try
            If ArgsMode = False Then
                If (strcommand = Nothing Or strcommand.StartsWith(" ") = True) Then
                    initializeShell()
                Else
                    Dim groupCmds() As String = strcommand.Split({":"c}, StringSplitOptions.RemoveEmptyEntries)
                    For Each cmd In groupCmds
                        Dim indexCmd As Integer = cmd.IndexOf(" ")
                        If (indexCmd = -1) Then
                            indexCmd = cmd.Count
                            cmd = cmd.Substring(0, indexCmd)
                        End If
                        If (adminList(signedinusrnm) = False And strictCmds.Contains(cmd.Substring(0, indexCmd)) = True) Then
                            Wdbg("Cmd exec {0} failed: adminList.ASSERT(signedinusrnm) = False, strictCmds.Cont({0}.Substr(0, {1})) = True", True, cmd.Substring(0, indexCmd), indexCmd)
                            Wln("You don't have permission to use {0}", "neutralText", cmd.Substring(0, indexCmd))
                        ElseIf (maintenance = True And cmd.Contains("logout")) Then
                            Wdbg("Cmd exec {0} failed: maintenance = True && input.Cont(""logout"") = True", True, cmd.Substring(0, indexCmd), indexCmd)
                            Wln("Shell message: The requested command {0} is not allowed to run in maintenance mode.", "neutralText", cmd.Substring(0, indexCmd))
                        ElseIf (adminList(signedinusrnm) = True And strictCmds.Contains(cmd.Substring(0, indexCmd)) = True) Or (availableCommands.Contains(cmd.Substring(0, indexCmd))) Then
                            Wdbg("Cmd exec: {0}", True, cmd.Substring(0, indexCmd))
                            GetCommand.ExecuteCommand(cmd)
                        Else
                            Wdbg("Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", True, cmd.Substring(0, indexCmd), indexCmd)
                            Wln("Shell message: The requested command {0} is not found. See 'help' for available commands.", "neutralText", cmd.Substring(0, indexCmd))
                        End If
                    Next
                    initializeShell()
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
                        If (cmd = Nothing Or cmd.StartsWith(" ") = True) Then
                            initializeShell()
                        Else
                            If (adminList(signedinusrnm) = True And strictCmds.Contains(cmd.Substring(0, indexCmd)) = True) Then
                                Wdbg("Cmd exec: {0}", True, cmd.Substring(0, indexCmd))
                                GetCommand.ExecuteCommand(cmd)
                            ElseIf (adminList(signedinusrnm) = False And strictCmds.Contains(cmd.Substring(0, indexCmd)) = True) Then
                                Wdbg("Cmd exec {0} failed: adminList.ASSERT(signedinusrnm) = False, strictCmds.Cont({0}.Substr(0, {1})) = True", True, cmd.Substring(0, indexCmd), indexCmd)
                                Wln("You don't have permission to use {0}", "neutralText", cmd.Substring(0, indexCmd))
                            ElseIf (cmd = "logout" Or cmd = "shutdown" Or cmd = "reboot") Then
                                Wdbg("Cmd exec {0} failed: {0} = (""logout"" | ""shutdown"" | ""reboot"") = True", True, cmd.Substring(0, indexCmd))
                                Wln("Shell message: Command {0} is not allowed to run on log in.", "neutralText", cmd)
                            Else
                                Wdbg("Cmd exec: {0}", True, cmd.Substring(0, indexCmd))
                                GetCommand.ExecuteCommand(cmd)
                            End If
                        End If
                    Else
                        Wdbg("Cmd exec {0} failed: availableCmds.Cont({0}.Substring(0, {1})) = False", True, cmd.Substring(0, indexCmd), indexCmd)
                        Wln("Shell message: The requested command {0} is not found.", "neutralText", cmd.Substring(0, cmd.Count - 1))
                    End If
                Next
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln("Error trying to execute command." + vbNewLine + "Error {0}: {1}" + vbNewLine + "{2}", "neutralText", _
                    Err.Number, Err.Description, ex.StackTrace)
                Wdbg(ex.StackTrace, True)
            Else
                Wln("Error trying to execute command." + vbNewLine + "Error {0}: {1}", "neutralText", Err.Number, Err.Description)
            End If
        End Try

    End Sub

End Module
