
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

    'This dictionary is the definitions for commands.
    Public MailDefinitions As Dictionary(Of String, String)
    Public MailModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the definition dictionary to make it full of available commands.
    ''' </summary>
    Public Sub IMAPInitHelp()
        MailDefinitions = New Dictionary(Of String, String) From {{"cd", DoTranslation("Changes current mail directory")},
                                                                  {"exit", DoTranslation("Exits the IMAP shell")},
                                                                  {"help", DoTranslation("List of commands")},
                                                                  {"list", DoTranslation("Downloads messages and lists them")},
                                                                  {"lsdirs", DoTranslation("Lists directories in your mail address")},
                                                                  {"mkdir", DoTranslation("Makes a directory in the current working directory")},
                                                                  {"mv", DoTranslation("Moves a message")},
                                                                  {"mvall", DoTranslation("Moves all messages from recipient")},
                                                                  {"read", DoTranslation("Opens a message")},
                                                                  {"readenc", DoTranslation("Opens an encrypted message")},
                                                                  {"ren", DoTranslation("Renames a folder")},
                                                                  {"rm", DoTranslation("Removes a message")},
                                                                  {"rmall", DoTranslation("Removes all messages from recipient")},
                                                                  {"rmdir", DoTranslation("Removes a directory from the current working directory")},
                                                                  {"send", DoTranslation("Sends a message to an address")},
                                                                  {"sendenc", DoTranslation("Sends an encrypted message to an address")}}
    End Sub

    ''' <summary>
    ''' Shows the help and usage for a specified command, or displays a list of commands when nothing is specified.
    ''' </summary>
    ''' <param name="command">A command</param>
    Public Sub IMAPShowHelp(Optional ByVal command As String = "")
        If command = "" Then
            If simHelp = False Then
                W(DoTranslation("General commands:"), True, ColTypes.Neutral)
                For Each cmd As String In MailDefinitions.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, MailDefinitions(cmd))
                Next
                W(vbNewLine + DoTranslation("Mod commands:"), True, ColTypes.Neutral)
                If MailModDefs.Count = 0 Then W(DoTranslation("No mod commands."), True, ColTypes.Neutral)
                For Each cmd As String In MailModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, MailModDefs(cmd))
                Next
                W(vbNewLine + DoTranslation("Alias commands:"), True, ColTypes.Neutral)
                If MailShellAliases.Count = 0 Then W(DoTranslation("No alias commands."), True, ColTypes.Neutral)
                For Each cmd As String In MailShellAliases.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, MailDefinitions(MailShellAliases(cmd)))
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
        ElseIf command = "cd" Then
            W(DoTranslation("Usage:") + " cd <folder>: " + DoTranslation("Changes current mail directory"), True, ColTypes.Neutral)
        ElseIf command = "exit" Then
            W(DoTranslation("Usage:") + " exit: " + DoTranslation("Exits the IMAP shell"), True, ColTypes.Neutral)
        ElseIf command = "list" Then
            W(DoTranslation("Usage:") + " list [pagenum]: " + DoTranslation("Downloads messages and lists them"), True, ColTypes.Neutral)
        ElseIf command = "ls" Then
            W(DoTranslation("Usage:") + " lsdirs: " + DoTranslation("Lists directories in your mail address"), True, ColTypes.Neutral)
        ElseIf command = "mkdir" Then
            W(DoTranslation("Usage:") + " mkdir <foldername>: " + DoTranslation("Makes a directory in the current working directory"), True, ColTypes.Neutral)
        ElseIf command = "mv" Then
            W(DoTranslation("Usage:") + " mv <mailid> <targetfolder>: " + DoTranslation("Moves a message"), True, ColTypes.Neutral)
        ElseIf command = "mvall" Then
            W(DoTranslation("Usage:") + " mvall <sendername> <targetfolder>: " + DoTranslation("Moves all messages from recipient"), True, ColTypes.Neutral)
        ElseIf command = "read" Then
            W(DoTranslation("Usage:") + " read <mailid>: " + DoTranslation("Opens a message"), True, ColTypes.Neutral)
        ElseIf command = "readenc" Then
            W(DoTranslation("Usage:") + " readenc <mailid>: " + DoTranslation("Opens an encrypted message"), True, ColTypes.Neutral)
        ElseIf command = "ren" Then
            W(DoTranslation("Usage:") + " ren <oldfoldername> <newfoldername>: " + DoTranslation("Renames a folder"), True, ColTypes.Neutral)
        ElseIf command = "rm" Then
            W(DoTranslation("Usage:") + " rm <mailid>: " + DoTranslation("Removes a message"), True, ColTypes.Neutral)
        ElseIf command = "rmall" Then
            W(DoTranslation("Usage:") + " rmall <sendername>: " + DoTranslation("Removes all messages from recipient"), True, ColTypes.Neutral)
        ElseIf command = "rmdir" Then
            W(DoTranslation("Usage:") + " rmdir <foldername>: " + DoTranslation("Removes a directory from the current working directory"), True, ColTypes.Neutral)
        ElseIf command = "send" Then
            W(DoTranslation("Usage:") + " send: " + DoTranslation("Asks you to fill the required fields to send a message to an address"), True, ColTypes.Neutral)
        ElseIf command = "sendenc" Then
            W(DoTranslation("Usage:") + " sendenc: " + DoTranslation("Asks you to fill the required fields to send an encrypted message to an address"), True, ColTypes.Neutral)
        End If
    End Sub

End Module
