
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

Public Module ZipHelpSystem

    Public ZipShell_ModHelpEntries As New Dictionary(Of String, String)

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub ZipShell_GetHelp(Optional ByVal Command As String = "")
        'Check to see if command exists
        If Not String.IsNullOrWhiteSpace(Command) And ZipShell_Commands.ContainsKey(Command) Then
            Dim HelpDefinition As String = ZipShell_Commands(Command).GetTranslatedHelpEntry
            Select Case Command
                Case "help"
                    W(DoTranslation("Usage:") + " help [command]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "exit"
                    W(DoTranslation("Usage:") + " exit: " + HelpDefinition, True, ColTypes.Neutral)
                Case "chdir"
                    W(DoTranslation("Usage:") + " chdir <directory>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "cdir"
                    W(DoTranslation("Usage:") + " cdir: " + HelpDefinition, True, ColTypes.Neutral)
                Case "chadir"
                    W(DoTranslation("Usage:") + " chadir <archivedirectory>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "list"
                    W(DoTranslation("Usage:") + " list [directory]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "get"
                    W(DoTranslation("Usage:") + " get <entry> [where] [-absolute]: " + HelpDefinition, True, ColTypes.Neutral)
            End Select
        ElseIf String.IsNullOrWhiteSpace(Command) Then
            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In ZipShell_Commands.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, ZipShell_Commands(cmd).GetTranslatedHelpEntry)
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If ZipShell_ModHelpEntries.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In ZipShell_ModHelpEntries.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, ZipShell_ModHelpEntries(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If ZIPShellAliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In ZIPShellAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, ZipShell_Commands(ZIPShellAliases(cmd)).GetTranslatedHelpEntry)
                Next
            Else
                For Each cmd As String In ZipShell_Commands.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In ZipShell_ModHelpEntries.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                W(String.Join(", ", ZIPShellAliases.Keys), True, ColTypes.ListEntry)
            End If
        Else
            W(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, Command)
        End If
    End Sub

End Module
