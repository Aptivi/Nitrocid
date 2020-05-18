
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
    Public IMAP_AvailableCommands() As String = {"help", "cd", "lsdirs", "exit", "list", "read"}
    Public IMAP_Messages As IEnumerable(Of UniqueId)
    Public IMAP_CurrentDirectory As String = "Inbox"
    Friend ExitRequested As Boolean

    ''' <summary>
    ''' Initializes the shell of the IMAP client
    ''' </summary>
    ''' <param name="Address">An e-mail address or username. This is used to show address in command input.</param>
    Sub OpenShell(Address As String)
        'Send ping to keep the connection alive
        Dim IMAP_NoOp As New Thread(AddressOf KeepConnection)
        IMAP_NoOp.Start()
        Wdbg("I", "Made new thread about KeepConnection()")

        While Not ExitRequested
            'Populate messages
            If IMAP_CurrentDirectory = "" Or IMAP_CurrentDirectory = "Inbox" Then
                IMAP_Client.Inbox.Open(FolderAccess.ReadOnly)
                Wdbg("I", "Opened inbox")
                IMAP_Messages = IMAP_Client.Inbox.Search(SearchQuery.All).Reverse
                Wdbg("I", "Messages count: {0} messages", IMAP_Messages.LongCount)
            Else
                Dim Folder As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                Wdbg("I", "Opened {0}", IMAP_CurrentDirectory)
                IMAP_Messages = Folder.Search(SearchQuery.All).Reverse
                Wdbg("I", "Messages count: {0} messages", IMAP_Messages.LongCount)
            End If

            'Initialize prompt
            W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, IMAP_Authentication.UserName) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, Address) : W("] ", False, ColTypes.Gray) : W("{0} > ", False, ColTypes.Input, IMAP_CurrentDirectory)

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
        IMAP_CurrentDirectory = "Inbox"
        Wdbg("W", "Exit requested. Disconnecting host...")
        IMAP_Client.Disconnect(True)
    End Sub

    ''' <summary>
    ''' Tries to keep the connection going
    ''' </summary>
    Sub KeepConnection()
        'Every 10 seconds, send a ping to IMAP server
        While IMAP_Client.IsConnected
            Thread.Sleep(10000)
            If IMAP_Client.IsConnected Then
                SyncLock IMAP_Client.SyncRoot
                    IMAP_Client.NoOp()
                End SyncLock
            Else
                Wdbg("W", "Connection state is inconsistent. Stopping KeepConnection()...")
                Thread.CurrentThread.Abort()
            End If
        End While
    End Sub

    ''' <summary>
    ''' Locates the normal (not special) folder and opens it.
    ''' </summary>
    ''' <param name="FolderString">A folder to open (not a path)</param>
    ''' <returns>A folder</returns>
    Public Function OpenFolder(ByVal FolderString As String) As MailFolder
        Dim Opened As MailFolder
        Wdbg("I", "Personal namespace collection parsing started.")
        For Each nmspc As FolderNamespace In IMAP_Client.PersonalNamespaces
            Wdbg("I", "Namespace: {0}", nmspc.Path)
            For Each dir As MailFolder In IMAP_Client.GetFolders(nmspc)
                If dir.Name = FolderString Then
                    dir.Open(FolderAccess.ReadOnly)
                    Opened = dir
                End If
            Next
        Next

        Wdbg("I", "Shared namespace collection parsing started.")
        For Each nmspc As FolderNamespace In IMAP_Client.SharedNamespaces
            Wdbg("I", "Namespace: {0}", nmspc.Path)
            For Each dir As MailFolder In IMAP_Client.GetFolders(nmspc)
                If dir.Name = FolderString Then
                    dir.Open(FolderAccess.ReadOnly)
                    Opened = dir
                End If
            Next
        Next

        Wdbg("I", "Other namespace collection parsing started.")
        For Each nmspc As FolderNamespace In IMAP_Client.OtherNamespaces
            Wdbg("I", "Namespace: {0}", nmspc.Path)
            For Each dir As MailFolder In IMAP_Client.GetFolders(nmspc)
                If dir.Name = FolderString Then
                    dir.Open(FolderAccess.ReadOnly)
                    Opened = dir
                End If
            Next
        Next

#Disable Warning BC42104
        Return Opened
#Enable Warning BC42104
    End Function

End Module
