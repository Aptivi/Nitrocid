
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

    Public ReadOnly DebugCommands As New Dictionary(Of String, CommandInfo) From {{"exit", New CommandInfo("exit", ShellCommandType.RemoteDebugShell, "Disconnects you from the debugger", "", False, 0)},
                                                                                  {"help", New CommandInfo("help", ShellCommandType.RemoteDebugShell, "Shows help screen", "[command]", False, 0)},
                                                                                  {"register", New CommandInfo("register", ShellCommandType.RemoteDebugShell, "Sets device username", "<username>", True, 1)},
                                                                                  {"trace", New CommandInfo("trace", ShellCommandType.RemoteDebugShell, "Shows last stack trace on exception", "<tracenumber>", True, 1)},
                                                                                  {"username", New CommandInfo("username", ShellCommandType.RemoteDebugShell, "Shows current username in the session", "", False, 0)}}
    Public DebugModCmds As New ArrayList

    ''' <summary>
    ''' Client command parsing.
    ''' </summary>
    ''' <param name="CmdString">A specified command. It may contain arguments.</param>
    ''' <param name="SocketStreamWriter">A socket stream writer</param>
    ''' <param name="Address">An IP address</param>
    Sub ParseCmd(CmdString As String, SocketStreamWriter As StreamWriter, Address As String)
        EventManager.RaiseRemoteDebugExecuteCommand(Address, CmdString)
        Dim ArgumentInfo As New ProvidedCommandArgumentsInfo(CmdString, ShellCommandType.RemoteDebugShell)
        Dim Command As String = ArgumentInfo.Command
        Dim eqargs() As String = ArgumentInfo.ArgumentsList
        Dim strArgs As String = ArgumentInfo.ArgumentsText
        Dim RequiredArgumentsProvided As Boolean = ArgumentInfo.RequiredArgumentsProvided

        Try
            Select Case Command
                Case "trace"
                    'Print stack trace command code
                    If dbgStackTraces.Count <> 0 Then
                        If eqargs?.Length <> 0 Then
                            Try
                                SocketStreamWriter.WriteLine(dbgStackTraces(eqargs(0)))
                            Catch ex As Exception
                                SocketStreamWriter.WriteLine(DoTranslation("Index {0} invalid. There are {1} stack traces. Index is zero-based, so try subtracting by 1."), eqargs(0), dbgStackTraces.Count)
                            End Try
                        Else
                            SocketStreamWriter.WriteLine(dbgStackTraces(0))
                        End If
                    Else
                        SocketStreamWriter.WriteLine(DoTranslation("No stack trace"))
                    End If
                Case "username"
                    'Current username
                    SocketStreamWriter.WriteLine(CurrentUser)
                Case "register"
                    'Register to remote debugger so we can set device name
                    If String.IsNullOrWhiteSpace(GetDeviceProperty(Address, DeviceProperty.Name)) Then
                        If eqargs.Length <> 0 Then
                            SetDeviceProperty(Address, DeviceProperty.Name, eqargs(0))
                            dbgConns(dbgConns.ElementAt(DebugDevices.GetIndexOfKey(DebugDevices.GetKeyFromValue(Address))).Key) = eqargs(0)
                            SocketStreamWriter.WriteLine(DoTranslation("Hi, {0}!").FormatString(eqargs(0)))
                        Else
                            SocketStreamWriter.WriteLine(DoTranslation("You need to write your name."))
                        End If
                    Else
                        SocketStreamWriter.WriteLine(DoTranslation("You're already registered."))
                    End If
                Case "help"
                    'Help command code
                    If eqargs?.Length <> 0 Then
                        ShowHelp(eqargs(0), ShellCommandType.RemoteDebugShell, SocketStreamWriter)
                    Else
                        ShowHelp("", ShellCommandType.RemoteDebugShell, SocketStreamWriter)
                    End If
                Case "exit"
                    'Exit command code
                    DisconnectDbgDev(Address)
            End Select
        Catch ex As Exception
            SocketStreamWriter.WriteLine(DoTranslation("Error executing remote debug command {0}: {1}"), Command, ex.Message)
            EventManager.RaiseRemoteDebugCommandError(Address, CmdString, ex)
        End Try
    End Sub
End Module
