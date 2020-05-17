
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

Imports System.Threading
Imports MailKit
Imports MailKit.Search

Module IMAPShell

    'Variables
    Public IMAP_AvailableCommands() As String = {"help", "exit", "list", "read"}
    Public IMAP_Messages As IEnumerable(Of UniqueId)
    Friend ExitRequested As Boolean

    Sub OpenShell(Address As String)
        'Send ping to keep the connection alive
        Dim IMAP_NoOp As New Thread(AddressOf KeepConnection)
        IMAP_NoOp.Start()
        Wdbg("I", "Made new thread about KeepConnection()")

        While Not ExitRequested
            'Populate messages
            'TODO: Populate also all folders, not just Inbox.
            IMAP_Client.Inbox.Open(FolderAccess.ReadOnly)
            Wdbg("I", "Opened inbox")
            IMAP_Messages = IMAP_Client.Inbox.Search(SearchQuery.All).Reverse
            Wdbg("I", "Messages count: {0} messages", IMAP_Messages.LongCount)

            'Initialize prompt
            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, IMAP_Authentication.UserName) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, Address) : W("] ", False, ColTypes.Gray)

            'Listen for a command
            Dim cmd As String = Console.ReadLine
            Dim args As String = ""
            Wdbg("I", "Original command: {0}", cmd)
            If cmd.Contains(" ") And Not cmd.StartsWith(" ") Then
                Wdbg("I", "Found arguments in command. Parsing...")
                args = cmd.Substring(cmd.IndexOf(" ") + 1)
                cmd = cmd.Remove(cmd.IndexOf(" "))
                Wdbg("I", "Command: ""{0}"", Arguments: ""{1}""", cmd, args)
            End If

            'Execute a command
            Wdbg("I", "Executing command...")
            If IMAP_AvailableCommands.Contains(cmd) Then
                Wdbg("I", "Command found.")
                IMAP_ExecuteCommand(cmd, args)
            ElseIf Not cmd.StartsWith(" ") Then
                Wdbg("E", "Command not found. Reopening shell...")
                W(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands.", currentLang), True, ColTypes.Err, cmd)
            End If
        End While

        'Disconnect the session
        Wdbg("W", "Exit requested. Disconnecting host...")
        IMAP_Client.Disconnect(True)
    End Sub

    Sub KeepConnection()
        'Every 10 seconds, send a ping to IMAP server
        While IMAP_Client.IsConnected
            Thread.Sleep(10000)
            If IMAP_Client.IsConnected Then
                IMAP_Client.NoOp()
            Else
                Wdbg("W", "Connection state is inconsistent. Stopping KeepConnection()...")
                Thread.CurrentThread.Abort()
            End If
        End While
    End Sub

End Module
