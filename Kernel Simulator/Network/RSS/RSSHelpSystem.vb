
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

Public Module RSSHelpSystem

    'This dictionary is the definitions for commands.
    Public RSSDefinitions As Dictionary(Of String, String)
    Public RSSModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the help definition so it reflects the available commands
    ''' </summary>
    Public Sub InitRSSHelp()
        RSSDefinitions = New Dictionary(Of String, String) From {{"articleinfo", DoTranslation("Gets the article info")},
                                                                 {"chfeed", DoTranslation("Changes the feed link")},
                                                                 {"feedinfo", DoTranslation("Gets the feed info")},
                                                                 {"list", DoTranslation("Lists all feeds")},
                                                                 {"read", DoTranslation("Reads a feed in a web browser")},
                                                                 {"exit", DoTranslation("Exits RSS shell and returns to kernel")},
                                                                 {"help", DoTranslation("Shows help screen")}}
    End Sub

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub RSSShowHelp(Optional ByVal command As String = "")

        If command = "" Then
            If simHelp = False Then
                For Each cmd As String In RSSDefinitions.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, RSSDefinitions(cmd))
                Next
                For Each cmd As String In RSSModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, RSSModDefs(cmd))
                Next
            Else
                W(String.Join(", ", RSSCommands.Keys), True, ColTypes.Neutral)
            End If
        ElseIf command = "articleinfo" Then
            W(DoTranslation("Usage:") + " articleinfo <feednum>", True, ColTypes.Neutral)
        ElseIf command = "chfeed" Then
            W(DoTranslation("Usage:") + " chfeed <feedurl>", True, ColTypes.Neutral)
        ElseIf command = "exit" Then
            W(DoTranslation("Usage:") + " exit", True, ColTypes.Neutral)
        ElseIf command = "feedinfo" Then
            W(DoTranslation("Usage:") + " feedinfo", True, ColTypes.Neutral)
        ElseIf command = "list" Then
            W(DoTranslation("Usage:") + " list", True, ColTypes.Neutral)
        ElseIf command = "read" Then
            W(DoTranslation("Usage:") + " read <feednum>", True, ColTypes.Neutral)
        End If

    End Sub

End Module
