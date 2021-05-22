
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

Imports System.Threading
Imports MailKit
Imports MailKit.Net.Imap
Imports MailKit.Search

Module MailShell

    'Variables
    Public MailCommands As New Dictionary(Of String, CommandInfo) From {{"cd", New CommandInfo("cd", ShellCommandType.MailShell, True, 1)},
                                                                        {"exit", New CommandInfo("exit", ShellCommandType.MailShell, False, 0)},
                                                                        {"help", New CommandInfo("help", ShellCommandType.MailShell, False, 0)},
                                                                        {"lsdirs", New CommandInfo("lsdirs", ShellCommandType.MailShell, False, 0)},
                                                                        {"list", New CommandInfo("list", ShellCommandType.MailShell, False, 0)},
                                                                        {"mkdir", New CommandInfo("mkdir", ShellCommandType.MailShell, True, 1)},
                                                                        {"mv", New CommandInfo("mv", ShellCommandType.MailShell, True, 2)},
                                                                        {"mvall", New CommandInfo("mvall", ShellCommandType.MailShell, True, 2)},
                                                                        {"read", New CommandInfo("read", ShellCommandType.MailShell, True, 1)},
                                                                        {"readenc", New CommandInfo("readenc", ShellCommandType.MailShell, True, 1)},
                                                                        {"ren", New CommandInfo("ren", ShellCommandType.MailShell, True, 2)},
                                                                        {"rm", New CommandInfo("rm", ShellCommandType.MailShell, True, 1)},
                                                                        {"rmall", New CommandInfo("rmall", ShellCommandType.MailShell, True, 1)},
                                                                        {"rmdir", New CommandInfo("rmdir", ShellCommandType.MailShell, True, 1)},
                                                                        {"send", New CommandInfo("send", ShellCommandType.MailShell, False, 0)},
                                                                        {"sendenc", New CommandInfo("sendenc", ShellCommandType.MailShell, False, 0)}}
    Public IMAP_Messages As IEnumerable(Of UniqueId)
    Public IMAP_CurrentDirectory As String = "Inbox"
    Friend ExitRequested, KeepAlive As Boolean
    Public MailModCommands As New ArrayList
    Public MailShellPromptStyle As String = ""

    ''' <summary>
    ''' Initializes the shell of the mail client
    ''' </summary>
    ''' <param name="Address">An e-mail address or username. This is used to show address in command input.</param>
    Sub OpenMailShell(Address As String)
        'Send ping to keep the connection alive
        Dim IMAP_NoOp As New Thread(AddressOf IMAPKeepConnection) With {.Name = "IMAP Keep Connection"}
        IMAP_NoOp.Start()
        Wdbg("I", "Made new thread about IMAPKeepConnection()")
        Dim SMTP_NoOp As New Thread(AddressOf SMTPKeepConnection) With {.Name = "SMTP Keep Connection"}
        SMTP_NoOp.Start()
        Wdbg("I", "Made new thread about SMTPKeepConnection()")

        'Add handler for IMAP and SMTP
        AddHandler Console.CancelKeyPress, AddressOf MailCancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
        EventManager.RaiseIMAPShellInitialized()

        While Not ExitRequested
            'Populate messages
            PopulateMessages()
            InitializeHandlers()

            'Initialize prompt
            If Not IsNothing(DefConsoleOut) Then
                Console.SetOut(DefConsoleOut)
            End If
            Wdbg("I", "MailShellPromptStyle = {0}", MailShellPromptStyle)
            If MailShellPromptStyle = "" Then
                W("[", False, ColTypes.Gray) : W("{0}", False, ColTypes.UserName, Mail_Authentication.UserName) : W("@", False, ColTypes.Gray) : W("{0}", False, ColTypes.HostName, Address) : W("] ", False, ColTypes.Gray) : W("{0} > ", False, ColTypes.Gray, IMAP_CurrentDirectory)
            Else
                Dim ParsedPromptStyle As String = ProbePlaces(MailShellPromptStyle)
                ParsedPromptStyle.ConvertVTSequences
                W(ParsedPromptStyle, False, ColTypes.Gray)
            End If
            SetInputColor()

            'Listen for a command
            Dim cmd As String = Console.ReadLine
            Dim args As String = ""
            If Not (cmd = Nothing Or cmd?.StartsWith(" ") = True) Then
                Wdbg("I", "Original command: {0}", cmd)
                If cmd.Contains(" ") And Not cmd.StartsWith(" ") Then
                    Wdbg("I", "Found arguments in command. Parsing...")
                    args = cmd.Substring(cmd.IndexOf(" ") + 1)
                    cmd = cmd.Remove(cmd.IndexOf(" "))
                    Wdbg("I", "Command: ""{0}"", Arguments: ""{1}""", cmd, args)
                End If
                EventManager.RaiseIMAPPreExecuteCommand(cmd + " " + args)

                'Execute a command
                Wdbg("I", "Executing command...")
                If MailCommands.ContainsKey(cmd) Then
                    Wdbg("I", "Command found.")
                    MailStartCommandThread = New Thread(AddressOf Mail_ExecuteCommand) With {.Name = "Mail Command Thread"}
                    MailStartCommandThread.Start({cmd, args})
                    MailStartCommandThread.Join()
                ElseIf MailModCommands.Contains(cmd) Then
                    Wdbg("I", "Mod command found.")
                    ExecuteModCommand(cmd + " " + args)
                ElseIf MailShellAliases.Keys.Contains(cmd) Then
                    Wdbg("I", "Mail shell alias command found.")
                    ExecuteMailAlias(cmd + " " + args)
                ElseIf Not cmd.StartsWith(" ") Then
                    Wdbg("E", "Command not found. Reopening shell...")
                    W(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands."), True, ColTypes.Error, cmd)
                End If
                EventManager.RaiseIMAPPostExecuteCommand(cmd + " " + args)
            Else
                Thread.Sleep(30) 'This is to fix race condition between mail shell initialization and starting the event handler thread
            End If
        End While

        'Disconnect the session
        IMAP_CurrentDirectory = "Inbox"
        If KeepAlive Then
            Wdbg("W", "Exit requested, but not disconnecting.")
        Else
            Wdbg("W", "Exit requested. Disconnecting host...")
            ReleaseHandlers()
            IMAP_Client.Disconnect(True)
            SMTP_Client.Disconnect(True)
        End If
        ExitRequested = False

        'Restore handler
        AddHandler Console.CancelKeyPress, AddressOf CancelCommand
        RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
    End Sub

    ''' <summary>
    ''' [IMAP] Tries to keep the connection going
    ''' </summary>
    Sub IMAPKeepConnection()
        'Every 30 seconds, send a ping to IMAP server
        While IMAP_Client.IsConnected
            Thread.Sleep(30000)
            If IMAP_Client.IsConnected Then
                SyncLock IMAP_Client.SyncRoot
                    IMAP_Client.NoOp()
                End SyncLock
                PopulateMessages()
            Else
                Wdbg("W", "Connection state is inconsistent. Stopping IMAPKeepConnection()...")
                Thread.CurrentThread.Abort()
            End If
        End While
    End Sub

    ''' <summary>
    ''' [SMTP] Tries to keep the connection going
    ''' </summary>
    Sub SMTPKeepConnection()
        'Every 30 seconds, send a ping to IMAP server
        While SMTP_Client.IsConnected
            Thread.Sleep(30000)
            If SMTP_Client.IsConnected Then
                SyncLock SMTP_Client.SyncRoot
                    SMTP_Client.NoOp()
                End SyncLock
            Else
                Wdbg("W", "Connection state is inconsistent. Stopping SMTPKeepConnection()...")
                Thread.CurrentThread.Abort()
            End If
        End While
    End Sub

    ''' <summary>
    ''' Populates e-mail messages
    ''' </summary>
    Public Sub PopulateMessages()
        If IMAP_Client.IsConnected Then
            SyncLock IMAP_Client.SyncRoot
                If IMAP_CurrentDirectory = "" Or IMAP_CurrentDirectory = "Inbox" Then
                    IMAP_Client.Inbox.Open(FolderAccess.ReadWrite)
                    Wdbg("I", "Opened inbox")
                    IMAP_Messages = IMAP_Client.Inbox.Search(SearchQuery.All).Reverse
                    Wdbg("I", "Messages count: {0} messages", IMAP_Messages.LongCount)
                Else
                    Dim Folder As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                    Wdbg("I", "Opened {0}", IMAP_CurrentDirectory)
                    IMAP_Messages = Folder.Search(SearchQuery.All).Reverse
                    Wdbg("I", "Messages count: {0} messages", IMAP_Messages.LongCount)
                End If
            End SyncLock
        End If
    End Sub

    ''' <summary>
    ''' Executed when the CountChanged event is fired.
    ''' </summary>
    ''' <param name="Sender">A folder</param>
    ''' <param name="e">Event arguments</param>
    Private Sub OnCountChanged(ByVal Sender As Object, ByVal e As EventArgs)
        Dim Folder As ImapFolder = Sender
        If Folder.Count > IMAP_Messages.Count Then
            Dim NewMessagesCount As Integer = Folder.Count - IMAP_Messages.Count
            NotifySend(New Notification With {.Title = DoTranslation("{0} new messages arrived in inbox.").FormatString(NewMessagesCount),
                                              .Desc = DoTranslation("Open ""lsmail"" to see them."),
                                              .Priority = NotifPriority.Medium})
        End If
    End Sub

    ''' <summary>
    ''' Initializes the CountChanged handlers. Currently, it only supports inbox.
    ''' </summary>
    Public Sub InitializeHandlers()
        AddHandler IMAP_Client.Inbox.CountChanged, AddressOf OnCountChanged
    End Sub

    ''' <summary>
    ''' Releases the CountChanged handlers. Currently, it only supports inbox.
    ''' </summary>
    Public Sub ReleaseHandlers()
        RemoveHandler IMAP_Client.Inbox.CountChanged, AddressOf OnCountChanged
    End Sub

    ''' <summary>
    ''' Executes the mail shell alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    Sub ExecuteMailAlias(ByVal aliascmd As String)
        Dim FirstWordCmd As String = aliascmd.Split(" "c)(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, MailShellAliases(FirstWordCmd))
        Wdbg("I", "Actual command: {0}", actualCmd)
        MailStartCommandThread = New Thread(AddressOf Mail_ExecuteCommand) With {.Name = "Mail Command Thread"}
        MailStartCommandThread.Start({MailShellAliases(FirstWordCmd), actualCmd.Replace(MailShellAliases(FirstWordCmd), "")})
        MailStartCommandThread.Join()
    End Sub

End Module
