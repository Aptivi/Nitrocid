
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

Public Module TextEditHelpSystem

    Public TextEdit_ModHelpEntries As New Dictionary(Of String, String)

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub TextEdit_GetHelp(Optional ByVal Command As String = "")
        'Check to see if command exists
        If Not String.IsNullOrWhiteSpace(Command) And TextEdit_Commands.ContainsKey(Command) Then
            Dim HelpDefinition As String = TextEdit_Commands(Command).GetTranslatedHelpEntry
            Select Case Command
                Case "help"
                    W(DoTranslation("Usage:") + " help [command]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "exit"
                    W(DoTranslation("Usage:") + " exit: " + HelpDefinition, True, ColTypes.Neutral)
                Case "exitnosave"
                    W(DoTranslation("Usage:") + " exitnosave: " + HelpDefinition, True, ColTypes.Neutral)
                Case "save"
                    W(DoTranslation("Usage:") + " save: " + HelpDefinition, True, ColTypes.Neutral)
                Case "print"
                    W(DoTranslation("Usage:") + " print [linenumber]: " + HelpDefinition, True, ColTypes.Neutral)
                Case "addline"
                    W(DoTranslation("Usage:") + " addline <text>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "delline"
                    W(DoTranslation("Usage:") + " delline <linenumber>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "replace"
                    W(DoTranslation("Usage:") + " replace ""<word/phrase>"" ""<word/phrase>"": " + HelpDefinition, True, ColTypes.Neutral)
                Case "replaceinline"
                    W(DoTranslation("Usage:") + " replaceinline ""<word/phrase>"" ""<word/phrase>"" <linenumber>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "delword"
                    W(DoTranslation("Usage:") + " delword ""<word/phrase>"" <linenumber>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "delcharnum"
                    W(DoTranslation("Usage:") + " delcharnum <charnumber> <linenumber>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "querychar"
                    W(DoTranslation("Usage:") + " querychar <char> <linenumber/all>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "queryword"
                    W(DoTranslation("Usage:") + " queryword ""<word/phrase>"" <linenumber/all>: " + HelpDefinition, True, ColTypes.Neutral)
                Case "clear"
                    W(DoTranslation("Usage:") + " clear: " + HelpDefinition, True, ColTypes.Neutral)
            End Select
        ElseIf String.IsNullOrWhiteSpace(Command) Then
            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In TextEdit_Commands.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, TextEdit_Commands(cmd).GetTranslatedHelpEntry)
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If TextEdit_ModHelpEntries.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In TextEdit_ModHelpEntries.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, TextEdit_ModHelpEntries(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If TextShellAliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In TextShellAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, TextEdit_Commands(TextShellAliases(cmd)).GetTranslatedHelpEntry)
                Next
            Else
                For Each cmd As String In TextEdit_Commands.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                For Each cmd As String In TextEdit_ModHelpEntries.Keys
                    W("{0}, ", False, ColTypes.ListEntry, cmd)
                Next
                W(String.Join(", ", TextShellAliases.Keys), True, ColTypes.ListEntry)
            End If
        Else
            W(DoTranslation("No help for command ""{0}""."), True, ColTypes.Error, Command)
        End If
    End Sub

End Module
