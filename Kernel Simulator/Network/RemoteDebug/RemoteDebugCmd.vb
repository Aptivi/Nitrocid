
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

Imports System.IO

Module RemoteDebugCmd
    'TODO: Custom aliases for r-debug command
    Public DebugCmds As String() = {"trace", "t", "username", "usr", "help", "h", "exit", "e"}
    Sub ParseCmd(ByVal CmdString As String, ByVal SocketStreamWriter As StreamWriter, ByVal Address As String)
        Dim CmdArgs As List(Of String) = CmdString.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).ToList
        Dim CmdName As String = CmdArgs(0)
        CmdArgs.RemoveAt(0)
        If CmdName = "trace" Or CmdName = "t" Then
            'Print stack trace command code
            If dbgStackTraces.Count <> 0 Then
                If CmdArgs.Count <> 0 Then
                    Try
                        SocketStreamWriter.WriteLine(dbgStackTraces(CmdArgs(0)))
                    Catch ex As Exception
                        SocketStreamWriter.WriteLine("Index {0} invalid. There are {1} stack traces. Index is zero-based, so try subtracting by 1.", CmdArgs(0), dbgStackTraces.Count)
                    End Try
                Else
                    SocketStreamWriter.WriteLine(dbgStackTraces(0))
                End If
            Else
                SocketStreamWriter.WriteLine("No stack trace")
            End If
        ElseIf CmdName = "username" Or CmdName = "usr" Then
            'Current username
            SocketStreamWriter.WriteLine(signedinusrnm)
        ElseIf CmdName = "help" Or CmdName = "h" Then
            'Help command code
            SocketStreamWriter.WriteLine("- trace <TraceNumber>: Shows last stack trace on exception" + vbNewLine +
                                         "- username: Shows current username in the session" + vbNewLine +
                                         "- exit: Disconnects you from the debugger")
        ElseIf CmdName = "exit" Or CmdName = "e" Then
            'Exit command code
            DisconnectDbgDev(Address)
        End If
    End Sub
End Module
