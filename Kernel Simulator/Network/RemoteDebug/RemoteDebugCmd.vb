
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

Module RemoteDebugCmd

    Public DebugCommands As New Dictionary(Of String, CommandInfo) From {{"exit", New CommandInfo("exit", ShellCommandType.RemoteDebugShell, False, 0, False, False, False, False)},
                                                                         {"help", New CommandInfo("help", ShellCommandType.RemoteDebugShell, False, 0, False, False, False, False)},
                                                                         {"register", New CommandInfo("register", ShellCommandType.RemoteDebugShell, True, 1, False, False, False, False)},
                                                                         {"trace", New CommandInfo("trace", ShellCommandType.RemoteDebugShell, True, 1, False, False, False, False)},
                                                                         {"username", New CommandInfo("username", ShellCommandType.RemoteDebugShell, False, 0, False, False, False, False)}}
    Public DebugModCmds As New ArrayList

    ''' <summary>
    ''' Client command parsing.
    ''' </summary>
    ''' <param name="CmdString">A specified command. It may contain arguments.</param>
    ''' <param name="SocketStreamWriter">A socket stream writer</param>
    ''' <param name="Address">An IP address</param>
    Sub ParseCmd(ByVal CmdString As String, ByVal SocketStreamWriter As StreamWriter, ByVal Address As String)
        EventManager.RaiseRemoteDebugExecuteCommand(Address, CmdString)
        Dim CmdArgs As List(Of String) = CmdString.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).ToList
        Dim CmdName As String = CmdArgs(0)
        CmdArgs.RemoveAt(0)
        Try
            If CmdName = "trace" Then
                'Print stack trace command code
                If dbgStackTraces.Count <> 0 Then
                    If CmdArgs.Count <> 0 Then
                        Try
                            SocketStreamWriter.WriteLine(dbgStackTraces(CmdArgs(0)))
                        Catch ex As Exception
                            SocketStreamWriter.WriteLine(DoTranslation("Index {0} invalid. There are {1} stack traces. Index is zero-based, so try subtracting by 1."), CmdArgs(0), dbgStackTraces.Count)
                        End Try
                    Else
                        SocketStreamWriter.WriteLine(dbgStackTraces(0))
                    End If
                Else
                    SocketStreamWriter.WriteLine(DoTranslation("No stack trace"))
                End If
            ElseIf CmdName = "username" Then
                'Current username
                SocketStreamWriter.WriteLine(signedinusrnm)
            ElseIf CmdName = "register" Then
                'Register to remote debugger so we can set device name
                If String.IsNullOrWhiteSpace(GetDeviceProperty(Address, DeviceProperty.Name)) Then
                    SetDeviceProperty(Address, DeviceProperty.Name, CmdArgs(0))
                    dbgConns(dbgConns.ElementAt(DebugDevices.GetIndexOfKey(DebugDevices.GetKeyFromValue(Address))).Key) = CmdArgs(0)
                    SocketStreamWriter.WriteLine(DoTranslation("Hi, {0}!").FormatString(CmdArgs(0)))
                Else
                    SocketStreamWriter.WriteLine(DoTranslation("You're already registered."))
                End If
            ElseIf CmdName = "help" Then
                'Help command code
                If CmdArgs.Count <> 0 Then
                    RDebugShowHelp(CmdArgs(0), SocketStreamWriter)
                Else
                    For Each cmd As String In RDebugDefinitions.Keys
                        SocketStreamWriter.WriteLine("- /{0}: {1}", cmd, RDebugDefinitions(cmd))
                    Next
                    For Each cmd As String In RDebugModDefs.Keys
                        SocketStreamWriter.WriteLine("- /{0}: {1}", cmd, RDebugModDefs(cmd))
                    Next
                    For Each cmd As String In RemoteDebugAliases.Keys
                        SocketStreamWriter.WriteLine("- /{0}: {1}", cmd, RDebugDefinitions(RemoteDebugAliases(cmd)))
                    Next
                End If
            ElseIf CmdName = "exit" Then
                'Exit command code
                DisconnectDbgDev(Address)
            End If
        Catch ex As Exception
            SocketStreamWriter.WriteLine(DoTranslation("Error executing remote debug command {0}: {1}"), CmdName, ex.Message)
            EventManager.RaiseRemoteDebugCommandError(Address, CmdString, ex)
        End Try
    End Sub
End Module
