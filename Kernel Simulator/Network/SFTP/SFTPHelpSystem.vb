
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

Public Module SFTPHelpSystem

    'This dictionary is the definitions for commands.
    Public SFTPModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub SFTPShowHelp(Optional ByVal command As String = "")
        'Check to see if command exists
        If Not String.IsNullOrWhiteSpace(command) And SFTPCommands.ContainsKey(command) Then
            Dim HelpDefinition As String = SFTPCommands(command).GetTranslatedHelpEntry
            Select Case command
                Case "pwdl"
                    W(DoTranslation("Usage:") + " pwdl: " + HelpDefinition, True, ColTypes.Neutral)
                Case "pwdr"
                    W(DoTranslation("Usage:") + " pwdr: " + HelpDefinition, True, ColTypes.Neutral)
                Case "connect"
                    W(DoTranslation("Usage:") + " connect <server>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "cdl"
                    W(DoTranslation("Usage:") + " cdl <directory>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "cdr"
                    W(DoTranslation("Usage:") + " cdr <directory>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "del"
                    W(DoTranslation("Usage:") + " del <file>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "disconnect"
                    W(DoTranslation("Usage:") + " disconnect: " + HelpDefinition, True, ColTypes.Neutral)
                Case "get"
                    W(DoTranslation("Usage:") + " get <file>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "exit"
                    W(DoTranslation("Usage:") + " exit: " + HelpDefinition, True, ColTypes.Neutral)
                Case "lsl"
                    W(DoTranslation("Usage:") + " lsl [dir]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "ldr"
                    W(DoTranslation("Usage:") + " lsr [dir]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "put"
                    W(DoTranslation("Usage:") + " put <file>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "quickconnect"
                    W(DoTranslation("Usage:") + " quickconnect: " + HelpDefinition, True, ColTypes.Neutral)
            End Select
        ElseIf String.IsNullOrWhiteSpace(command) Then
            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In SFTPCommands.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, SFTPCommands(cmd).GetTranslatedHelpEntry)
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If SFTPModDefs.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In SFTPModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, SFTPModDefs(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If SFTPShellAliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In SFTPShellAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, SFTPCommands(SFTPShellAliases(cmd)).GetTranslatedHelpEntry)
                Next
            Else
                For Each cmd As String In SFTPCommands.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In SFTPModDefs.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                W(String.Join(", ", SFTPShellAliases.Keys), True, ColTypes.ListEntry)
            End If
        Else
            W(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, command)
        End If
    End Sub

End Module
