
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
Imports KS.Network.RemoteDebug.Commands
Imports KS.Network.RemoteDebug.Interface

Namespace Network.RemoteDebug
    Module RemoteDebugCmd

        Public ReadOnly DebugCommands As New Dictionary(Of String, CommandInfo) From {
            {"exit", New CommandInfo("exit", ShellType.RemoteDebugShell, "Disconnects you from the debugger", Array.Empty(Of String), False, 0, New Debug_ExitCommand)},
            {"help", New CommandInfo("help", ShellType.RemoteDebugShell, "Shows help screen", {"[command]"}, False, 0, New Debug_HelpCommand)},
            {"register", New CommandInfo("register", ShellType.RemoteDebugShell, "Sets device username", {"<username>"}, True, 1, New Debug_RegisterCommand)},
            {"trace", New CommandInfo("trace", ShellType.RemoteDebugShell, "Shows last stack trace on exception", {"<tracenumber>"}, True, 1, New Debug_TraceCommand)},
            {"username", New CommandInfo("username", ShellType.RemoteDebugShell, "Shows current username in the session", Array.Empty(Of String), False, 0, New Debug_UsernameCommand)}
        }
        Friend ReadOnly DebugModCmds As New Dictionary(Of String, CommandInfo)

        ''' <summary>
        ''' Client command parsing.
        ''' </summary>
        ''' <param name="CmdString">A specified command. It may contain arguments.</param>
        ''' <param name="SocketStreamWriter">A socket stream writer</param>
        ''' <param name="Address">An IP address</param>
        Sub ParseCmd(CmdString As String, SocketStreamWriter As StreamWriter, Address As String)
            KernelEventManager.RaiseRemoteDebugExecuteCommand(Address, CmdString)
            Dim ArgumentInfo As New ProvidedCommandArgumentsInfo(CmdString, ShellType.RemoteDebugShell)
            Dim Command As String = ArgumentInfo.Command
            Dim Args() As String = ArgumentInfo.ArgumentsList
            Dim StrArgs As String = ArgumentInfo.ArgumentsText
            Dim RequiredArgumentsProvided As Boolean = ArgumentInfo.RequiredArgumentsProvided

            Try
                Dim DebugCommandBase As RemoteDebugCommandExecutor = DebugCommands(Command).CommandBase
                DebugCommandBase.Execute(StrArgs, Args, SocketStreamWriter, Address)
            Catch ex As Exception
                SocketStreamWriter.WriteLine(DoTranslation("Error executing remote debug command {0}: {1}"), Command, ex.Message)
                KernelEventManager.RaiseRemoteDebugCommandError(Address, CmdString, ex)
            End Try
        End Sub
    End Module
End Namespace
