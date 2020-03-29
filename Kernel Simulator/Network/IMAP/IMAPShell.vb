
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

Imports MailKit
Imports MailKit.Search

Module IMAPShell

    'Variables
    Public IMAP_AvailableCommands() As String = {"help", "exit", "list", "read"}
    Public IMAP_Messages As IList(Of UniqueId)
    Friend ExitRequested As Boolean

    Sub OpenShell(Address As String)
        While Not ExitRequested
            'Populate messages
            'TODO: Populate also all folders, not just Inbox.
            IMAP_Client.Inbox.Open(FolderAccess.ReadOnly)
            IMAP_Messages = IMAP_Client.Inbox.Search(SearchQuery.All)

            'Initialize prompt
            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, IMAP_Authentication.UserName) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, Address) : W("] ", False, ColTypes.Gray)

            'Listen for a command
            Dim cmd As String = Console.ReadLine
            Dim args As String = ""
            If cmd.Contains(" ") And Not cmd.StartsWith(" ") Then
                args = cmd.Substring(cmd.IndexOf(" ") + 1)
                cmd = cmd.Remove(cmd.IndexOf(" "))
            End If

            'Execute a command
            If IMAP_AvailableCommands.Contains(cmd) Then
                IMAP_ExecuteCommand(cmd, args)
            ElseIf Not cmd.StartsWith(" ") Then
                W(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands.", currentLang), True, ColTypes.Neutral, cmd)
            End If
        End While

        IMAP_Client.Disconnect(True)
    End Sub

End Module
