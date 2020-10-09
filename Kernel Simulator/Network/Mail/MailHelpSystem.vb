
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

Public Module MailHelpSystem

    'This dictionary is the definitions for commands.
    Public MailDefinitions As Dictionary(Of String, String)
    Public MailModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the definition dictionary to make it full of available commands.
    ''' </summary>
    Public Sub IMAPInitHelp()
        MailDefinitions = New Dictionary(Of String, String) From {{"cd", DoTranslation("Changes current mail directory", currentLang)},
                                                                  {"exit", DoTranslation("Exits the IMAP shell", currentLang)},
                                                                  {"help", DoTranslation("List of commands", currentLang)},
                                                                  {"list", DoTranslation("Downloads messages and lists them", currentLang)},
                                                                  {"lsdirs", DoTranslation("Lists directories in your mail address", currentLang)},
                                                                  {"mv", DoTranslation("Moves a message", currentLang)},
                                                                  {"mvall", DoTranslation("Moves all messages from recipient", currentLang)},
                                                                  {"read", DoTranslation("Opens a message", currentLang)},
                                                                  {"readenc", DoTranslation("Opens an encrypted message", currentLang)},
                                                                  {"rm", DoTranslation("Removes a message", currentLang)},
                                                                  {"rmall", DoTranslation("Removes all messages from recipient", currentLang)},
                                                                  {"send", DoTranslation("Sends a message to an address", currentLang)},
                                                                  {"sendenc", DoTranslation("Sends an encrypted message to an address", currentLang)}}
    End Sub

    ''' <summary>
    ''' Shows the help and usage for a specified command, or displays a list of commands when nothing is specified.
    ''' </summary>
    ''' <param name="cmd">A command</param>
    Public Sub IMAPShowHelp(Optional ByVal cmd As String = "")
        If cmd = "" Then
            For Each cmnd As String In MailDefinitions.Keys
                W("- {0}: ", False, ColTypes.HelpCmd, cmnd) : W("{0}", True, ColTypes.HelpDef, MailDefinitions(cmnd))
            Next
            For Each cmnd As String In MailModDefs.Keys
                W("- {0}: ", False, ColTypes.HelpCmd, cmnd) : W("{0}", True, ColTypes.HelpDef, MailModDefs(cmnd))
            Next
        ElseIf cmd = "cd" Then
            W(DoTranslation("Usage:", currentLang) + " cd <folder>: " + DoTranslation("Changes current mail directory", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "exit" Then
            W(DoTranslation("Usage:", currentLang) + " exit: " + DoTranslation("Exits the IMAP shell", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "list" Then
            W(DoTranslation("Usage:", currentLang) + " list [pagenum]: " + DoTranslation("Downloads messages and lists them", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "ls" Then
            W(DoTranslation("Usage:", currentLang) + " lsdirs: " + DoTranslation("Lists directories in your mail address", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "mv" Then
            W(DoTranslation("Usage:", currentLang) + " mv <mailid> <targetfolder>: " + DoTranslation("Moves a message", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "mvall" Then
            W(DoTranslation("Usage:", currentLang) + " mvall <sendername> <targetfolder>: " + DoTranslation("Moves all messages from recipient", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "read" Then
            W(DoTranslation("Usage:", currentLang) + " read <mailid>: " + DoTranslation("Opens a message", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "readenc" Then
            W(DoTranslation("Usage:", currentLang) + " readenc <mailid>: " + DoTranslation("Opens an encrypted message", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "rm" Then
            W(DoTranslation("Usage:", currentLang) + " rm <mailid>: " + DoTranslation("Removes a message", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "rmall" Then
            W(DoTranslation("Usage:", currentLang) + " rmall <sendername>: " + DoTranslation("Removes all messages from recipient", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "send" Then
            W(DoTranslation("Usage:", currentLang) + " send: " + DoTranslation("Asks you to fill the required fields to send a message to an address", currentLang), True, ColTypes.Neutral)
        ElseIf cmd = "sendenc" Then
            W(DoTranslation("Usage:", currentLang) + " sendenc: " + DoTranslation("Asks you to fill the required fields to send an encrypted message to an address", currentLang), True, ColTypes.Neutral)
        End If
    End Sub

End Module
