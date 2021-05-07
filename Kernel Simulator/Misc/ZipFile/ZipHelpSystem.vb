
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

    Public ZipShell_HelpEntries As Dictionary(Of String, String)
    Public ZipShell_ModHelpEntries As New Dictionary(Of String, String)

    Public Sub ZipShell_UpdateHelp()
        ZipShell_HelpEntries = New Dictionary(Of String, String) From {{"help", DoTranslation("Lists available commands")},
                                                                       {"exit", DoTranslation("Exits the ZIP shell")},
                                                                       {"chdir", DoTranslation("Changes directory")},
                                                                       {"cdir", DoTranslation("Gets current local directory")},
                                                                       {"chadir", DoTranslation("Changes archive directory")},
                                                                       {"list", DoTranslation("Lists all files inside the archive")},
                                                                       {"get", DoTranslation("Extracts a file to a specified directory or a current directory")}}
    End Sub

    Public Sub ZipShell_GetHelp(Optional ByVal Command As String = "")
        If Command = "" Then
            For Each HelpKey As String In ZipShell_HelpEntries.Keys
                W("- {0}: ", False, ColTypes.ListEntry, HelpKey)
                W(ZipShell_HelpEntries(HelpKey), True, ColTypes.ListValue)
            Next
            For Each HelpKey As String In ZipShell_ModHelpEntries.Keys
                W("- {0}: ", False, ColTypes.ListEntry, HelpKey)
                W(ZipShell_ModHelpEntries(HelpKey), True, ColTypes.ListValue)
            Next
        ElseIf Command = "help" Then
            W(DoTranslation("Usage:") + " help [command]", True, ColTypes.Neutral)
        ElseIf Command = "exit" Then
            W(DoTranslation("Usage:") + " exit", True, ColTypes.Neutral)
        ElseIf Command = "chdir" Then
            W(DoTranslation("Usage:") + " chdir <directory>", True, ColTypes.Neutral)
        ElseIf Command = "cdir" Then
            W(DoTranslation("Usage:") + " cdir", True, ColTypes.Neutral)
        ElseIf Command = "chadir" Then
            W(DoTranslation("Usage:") + " chadir <archivedirectory>", True, ColTypes.Neutral)
        ElseIf Command = "list" Then
            W(DoTranslation("Usage:") + " list [directory]", True, ColTypes.Neutral)
        ElseIf Command = "get" Then
            W(DoTranslation("Usage:") + " get <entry> [where] [-absolute]", True, ColTypes.Neutral)
        End If
    End Sub

End Module
