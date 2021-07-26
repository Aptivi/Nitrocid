
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

Public Module MailHelpSystem

    Public MailModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Shows the help and usage for a specified command, or displays a list of commands when nothing is specified.
    ''' </summary>
    ''' <param name="command">A command</param>
    Public Sub IMAPShowHelp(Optional ByVal command As String = "")
        'Check to see if command exists
        If Not String.IsNullOrWhiteSpace(command) And MailCommands.ContainsKey(command) Then
            Dim HelpDefinition As String = MailCommands(command).GetTranslatedHelpEntry
            Select Case command
                Case "cd"
                    W(DoTranslation("Usage:") + " cd <folder>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "exit"
                    W(DoTranslation("Usage:") + " exit: " + HelpDefinition, True, ColTypes.Neutral)
                Case "list"
                    W(DoTranslation("Usage:") + " list [pagenum]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "ls"
                    W(DoTranslation("Usage:") + " lsdirs: " + HelpDefinition, True, ColTypes.Neutral)
                Case "mkdir"
                    W(DoTranslation("Usage:") + " mkdir <foldername>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "mv"
                    W(DoTranslation("Usage:") + " mv <mailid> <targetfolder>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "mvall"
                    W(DoTranslation("Usage:") + " mvall <sendername> <targetfolder>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "read"
                    W(DoTranslation("Usage:") + " read <mailid>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "readenc"
                    W(DoTranslation("Usage:") + " readenc <mailid>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "ren"
                    W(DoTranslation("Usage:") + " ren <oldfoldername> <newfoldername>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "rm"
                    W(DoTranslation("Usage:") + " rm <mailid>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "rmall"
                    W(DoTranslation("Usage:") + " rmall <sendername>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "rmdir"
                    W(DoTranslation("Usage:") + " rmdir <foldername>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "send"
                    W(DoTranslation("Usage:") + " send: " + HelpDefinition, True, ColTypes.Neutral)
                Case "sendenc"
                    W(DoTranslation("Usage:") + " sendenc: " + HelpDefinition, True, ColTypes.Neutral)
            End Select
        ElseIf String.IsNullOrWhiteSpace(command) Then
            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In MailCommands.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, MailCommands(cmd).GetTranslatedHelpEntry)
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If MailModDefs.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In MailModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, MailModDefs(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If MailShellAliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In MailShellAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, MailCommands(MailShellAliases(cmd)).GetTranslatedHelpEntry)
                Next
            Else
                For Each cmd As String In MailCommands.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In MailModDefs.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                W(String.Join(", ", MailShellAliases.Keys), True, ColTypes.ListEntry)
            End If
        Else
            W(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, command)
        End If
    End Sub

End Module
