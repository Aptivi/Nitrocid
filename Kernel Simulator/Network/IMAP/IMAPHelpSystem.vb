
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Public Module IMAPHelpSystem

    'This dictionary is the definitions for commands.
    Public IMAP_definitions As Dictionary(Of String, String)

    Public Sub IMAPInitHelp()
        IMAP_definitions = New Dictionary(Of String, String) From {{"exit", DoTranslation("Exits the IMAP shell", currentLang)},
                                                                   {"help", DoTranslation("List of commands", currentLang)},
                                                                   {"list", DoTranslation("Downloads messages and lists them", currentLang)},
                                                                   {"read", DoTranslation("Opens a message", currentLang)}}
    End Sub

    Public Sub IMAPShowHelp(Optional ByVal cmd As String = "")
        If cmd = "" Then
            For Each cmnd As String In IMAP_definitions.Keys
                W("- {0}: ", False, ColTypes.HelpCmd, cmnd) : W("{0}", True, ColTypes.HelpDef, IMAP_definitions(cmnd))
            Next
        ElseIf cmd = "exit" Then
            W(DoTranslation("Usage:", currentLang) + " exit: " + DoTranslation("Exits the IMAP shell", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "list" Then
            W(DoTranslation("Usage:", currentLang) + " list [pagenum]: " + DoTranslation("Downloads messages and lists them", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "read" Then
            W(DoTranslation("Usage:", currentLang) + " read <mailid>: " + DoTranslation("Opens a message", currentLang), True, ColTypes.Neutral)
        End If
    End Sub

End Module
