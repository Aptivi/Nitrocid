
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

Public Module RemoteDebugHelpSystem

    'This dictionary is the definitions for commands.
    Public RDebugDefinitions As Dictionary(Of String, String)
    Public RDebugModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the help definition so it reflects the available commands
    ''' </summary>
    Public Sub InitRDebugHelp()
        RDebugDefinitions = New Dictionary(Of String, String) From {{"trace", DoTranslation("Shows last stack trace on exception")},
                                                                    {"username", DoTranslation("Shows current username in the session")},
                                                                    {"register", DoTranslation("Sets device username")},
                                                                    {"exit", DoTranslation("Disconnects you from the debugger")}}
    End Sub

    ''' <summary>
    ''' Shows the help entry for command.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub RDebugShowHelp(ByVal command As String, ByVal DeviceStream As StreamWriter)

        If command = "trace" Then
            DeviceStream.WriteLine(DoTranslation("Usage:") + " /trace <tracenumber>")
        ElseIf command = "username" Then
            DeviceStream.WriteLine(DoTranslation("Usage:") + " /username")
        ElseIf command = "register" Then
            DeviceStream.WriteLine(DoTranslation("Usage:") + " /register <username>")
        ElseIf command = "exit" Then
            DeviceStream.WriteLine(DoTranslation("Usage:") + " /exit")
        End If

    End Sub

End Module
