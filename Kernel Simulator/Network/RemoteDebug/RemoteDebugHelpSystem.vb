
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

    Public RDebugModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Shows the help entry for command.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub RDebugShowHelp(ByVal command As String, ByVal DeviceStream As StreamWriter)
        'Check to see if command exists
        If Not String.IsNullOrWhiteSpace(command) And DebugCommands.ContainsKey(command) Then
            Dim HelpDefinition As String = DebugCommands(command).GetTranslatedHelpEntry
            Select Case command
                Case "help"
                    DeviceStream.WriteLine(DoTranslation("Usage:") + " /help [command]: " + HelpDefinition)
                Case "trace"
                    DeviceStream.WriteLine(DoTranslation("Usage:") + " /trace <tracenumber>: " + HelpDefinition)
                Case "username"
                    DeviceStream.WriteLine(DoTranslation("Usage:") + " /username: " + HelpDefinition)
                Case "register"
                    DeviceStream.WriteLine(DoTranslation("Usage:") + " /register <username>: " + HelpDefinition)
                Case "exit"
                    DeviceStream.WriteLine(DoTranslation("Usage:") + " /exit: " + HelpDefinition)
            End Select
        ElseIf String.IsNullOrWhiteSpace(command) Then
            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In DebugCommands.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, DebugCommands(cmd).GetTranslatedHelpEntry)
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If RDebugModDefs.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In RDebugModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, RDebugModDefs(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If RemoteDebugAliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In RemoteDebugAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, DebugCommands(RemoteDebugAliases(cmd)).GetTranslatedHelpEntry)
                Next
            Else
                For Each cmd As String In DebugCommands.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In RDebugModDefs.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                W(String.Join(", ", RemoteDebugAliases.Keys), True, ColTypes.ListEntry)
            End If
        Else
            W(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, command)
        End If
    End Sub

End Module
