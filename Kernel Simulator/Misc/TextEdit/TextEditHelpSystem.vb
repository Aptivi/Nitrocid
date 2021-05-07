
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

    Public TextEdit_HelpEntries As Dictionary(Of String, String)
    Public TextEdit_ModHelpEntries As New Dictionary(Of String, String)

    Public Sub TextEdit_UpdateHelp()
        TextEdit_HelpEntries = New Dictionary(Of String, String) From {{"help", DoTranslation("Lists available commands")},
                                                                       {"exit", DoTranslation("Exits the text editor and save unsaved changes")},
                                                                       {"exitnosave", DoTranslation("Exits the text editor")},
                                                                       {"save", DoTranslation("Saves the file")},
                                                                       {"print", DoTranslation("Prints the contents of the file with line numbers to the console")},
                                                                       {"addline", DoTranslation("Adds a new line with text at the end of the file")},
                                                                       {"delline", DoTranslation("Removes the specified line number")},
                                                                       {"replace", DoTranslation("Replaces a word or phrase with another one")},
                                                                       {"replaceinline", DoTranslation("Replaces a word or phrase with another one in a line")},
                                                                       {"delword", DoTranslation("Deletes a word or phrase from line number")},
                                                                       {"delcharnum", DoTranslation("Deletes a character from character number in specified line")},
                                                                       {"querychar", DoTranslation("Queries a character in a specified line or all lines")},
                                                                       {"queryword", DoTranslation("Queries a word in a specified line or all lines")},
                                                                       {"clear", DoTranslation("Clears the text file")}}
    End Sub

    Public Sub TextEdit_GetHelp(Optional ByVal Command As String = "")
        If Command = "" Then
            For Each HelpKey As String In TextEdit_HelpEntries.Keys
                W("- {0}: ", False, ColTypes.ListEntry, HelpKey)
                W(TextEdit_HelpEntries(HelpKey), True, ColTypes.ListValue)
            Next
            For Each HelpKey As String In TextEdit_ModHelpEntries.Keys
                W("- {0}: ", False, ColTypes.ListEntry, HelpKey)
                W(TextEdit_ModHelpEntries(HelpKey), True, ColTypes.ListValue)
            Next
        ElseIf Command = "help" Then
            W(DoTranslation("Usage:") + " help [command]", True, ColTypes.Neutral)
        ElseIf Command = "exit" Then
            W(DoTranslation("Usage:") + " exit", True, ColTypes.Neutral)
        ElseIf Command = "exitnosave" Then
            W(DoTranslation("Usage:") + " exitnosave", True, ColTypes.Neutral)
        ElseIf Command = "save" Then
            W(DoTranslation("Usage:") + " save", True, ColTypes.Neutral)
        ElseIf Command = "print" Then
            W(DoTranslation("Usage:") + " print [linenumber]", True, ColTypes.Neutral)
        ElseIf Command = "addline" Then
            W(DoTranslation("Usage:") + " addline ""<text>""", True, ColTypes.Neutral)
        ElseIf Command = "delline" Then
            W(DoTranslation("Usage:") + " delline <linenumber>", True, ColTypes.Neutral)
        ElseIf Command = "replace" Then
            W(DoTranslation("Usage:") + " replace ""<word/phrase>"" ""<word/phrase>""", True, ColTypes.Neutral)
        ElseIf Command = "replaceinline" Then
            W(DoTranslation("Usage:") + " replaceinline ""<word/phrase>"" ""<word/phrase>"" <linenumber>", True, ColTypes.Neutral)
        ElseIf Command = "delword" Then
            W(DoTranslation("Usage:") + " delword ""<word/phrase>"" <linenumber>", True, ColTypes.Neutral)
        ElseIf Command = "delcharnum" Then
            W(DoTranslation("Usage:") + " delcharnum <charnumber> <linenumber>", True, ColTypes.Neutral)
        ElseIf Command = "querychar" Then
            W(DoTranslation("Usage:") + " querychar <char> <linenumber/all>", True, ColTypes.Neutral)
        ElseIf Command = "queryword" Then
            W(DoTranslation("Usage:") + " queryword ""<word/phrase>"" <linenumber/all>", True, ColTypes.Neutral)
        ElseIf Command = "clear" Then
            W(DoTranslation("Usage:") + " clear", True, ColTypes.Neutral)
        End If
    End Sub

End Module
