
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

Public Module MailShell

    'Variables
    Public ReadOnly MailCommands As New Dictionary(Of String, CommandInfo) From {{"cd", New CommandInfo("cd", ShellCommandType.MailShell, "Changes current mail directory", "<folder>", True, 1, New Mail_CdCommand)},
                                                                                 {"exit", New CommandInfo("exit", ShellCommandType.MailShell, "Exits the IMAP shell", "", False, 0, New Mail_ExitCommand)},
                                                                                 {"help", New CommandInfo("help", ShellCommandType.MailShell, "List of commands", "[command]", False, 0, New Mail_HelpCommand)},
                                                                                 {"lsdirs", New CommandInfo("lsdirs", ShellCommandType.MailShell, "Lists directories in your mail address", "", False, 0, New Mail_LsDirsCommand)},
                                                                                 {"list", New CommandInfo("list", ShellCommandType.MailShell, "Downloads messages and lists them", "[pagenum]", False, 0, New Mail_ListCommand)},
                                                                                 {"mkdir", New CommandInfo("mkdir", ShellCommandType.MailShell, "Makes a directory in the current working directory", "<foldername>", True, 1, New Mail_MkdirCommand)},
                                                                                 {"mv", New CommandInfo("mv", ShellCommandType.MailShell, "Moves a message", "<mailid> <targetfolder>", True, 2, New Mail_MvCommand)},
                                                                                 {"mvall", New CommandInfo("mvall", ShellCommandType.MailShell, "Moves all messages from recipient", "<sendername> <targetfolder>", True, 2, New Mail_MvAllCommand)},
                                                                                 {"read", New CommandInfo("read", ShellCommandType.MailShell, "Opens a message", "<mailid>", True, 1, New Mail_ReadCommand)},
                                                                                 {"readenc", New CommandInfo("readenc", ShellCommandType.MailShell, "Opens an encrypted message", "<mailid>", True, 1, New Mail_ReadEncCommand)},
                                                                                 {"ren", New CommandInfo("ren", ShellCommandType.MailShell, "Renames a folder", "<oldfoldername> <newfoldername>", True, 2, New Mail_RenCommand)},
                                                                                 {"rm", New CommandInfo("rm", ShellCommandType.MailShell, "Removes a message", "<mailid>", True, 1, New Mail_RmCommand)},
                                                                                 {"rmall", New CommandInfo("rmall", ShellCommandType.MailShell, "Removes all messages from recipient", "<sendername>", True, 1, New Mail_RmAllCommand)},
                                                                                 {"rmdir", New CommandInfo("rmdir", ShellCommandType.MailShell, "Removes a directory from the current working directory", "<foldername>", True, 1, New Mail_RmdirCommand)},
                                                                                 {"send", New CommandInfo("send", ShellCommandType.MailShell, "Sends a message to an address", "", False, 0, New Mail_SendCommand)},
                                                                                 {"sendenc", New CommandInfo("sendenc", ShellCommandType.MailShell, "Sends an encrypted message to an address", "", False, 0, New Mail_SendEncCommand)}}
    Friend IMAP_Messages As IEnumerable(Of UniqueId)
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
        Wdbg(DebugLevel.I, "Made new thread about IMAPKeepConnection()")
        Dim SMTP_NoOp As New Thread(AddressOf SMTPKeepConnection) With {.Name = "SMTP Keep Connection"}
        SMTP_NoOp.Start()
        Wdbg(DebugLevel.I, "Made new thread about SMTPKeepConnection()")

        'Add handler for IMAP and SMTP
        SwitchCancellationHandler(ShellCommandType.MailShell)
        EventManager.RaiseIMAPShellInitialized()

        While Not ExitRequested
            'Populate messages
            PopulateMessages()
            InitializeHandlers()

            'Initialize prompt
            If DefConsoleOut IsNot Nothing Then
                Console.SetOut(DefConsoleOut)
            End If
            Wdbg(DebugLevel.I, "MailShellPromptStyle = {0}", MailShellPromptStyle)
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
            If Not (cmd = Nothing Or cmd?.StartsWithAnyOf({" ", "#"}) = True) Then
                EventManager.RaiseIMAPPreExecuteCommand(cmd)
                Dim words As String() = cmd.SplitEncloseDoubleQuotes(" ")
                Wdbg(DebugLevel.I, $"Is the command found? {MailCommands.ContainsKey(words(0))}")
                If MailCommands.ContainsKey(words(0)) Then
                    Wdbg(DebugLevel.I, "Command found.")
                    Dim Params As New ExecuteCommandThreadParameters(cmd, ShellCommandType.MailShell, Nothing)
                    MailStartCommandThread = New Thread(AddressOf ExecuteCommand) With {.Name = "Mail Command Thread"}
                    MailStartCommandThread.Start(Params)
                    MailStartCommandThread.Join()
                ElseIf MailModCommands.Contains(words(0)) Then
                    Wdbg(DebugLevel.I, "Mod command found.")
                    ExecuteModCommand(cmd)
                ElseIf MailShellAliases.Keys.Contains(words(0)) Then
                    Wdbg(DebugLevel.I, "Mail shell alias command found.")
                    cmd = cmd.Replace($"""{words(0)}""", words(0))
                    ExecuteMailAlias(cmd)
                ElseIf Not cmd.StartsWith(" ") Then
                    Wdbg(DebugLevel.E, "Command not found. Reopening shell...")
                    W(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands."), True, ColTypes.Error, words(0))
                End If
                EventManager.RaiseIMAPPostExecuteCommand(cmd)
            Else
                Thread.Sleep(30) 'This is to fix race condition between mail shell initialization and starting the event handler thread
            End If
        End While

        'Disconnect the session
        IMAP_CurrentDirectory = "Inbox"
        If KeepAlive Then
            Wdbg(DebugLevel.W, "Exit requested, but not disconnecting.")
        Else
            Wdbg(DebugLevel.W, "Exit requested. Disconnecting host...")
            ReleaseHandlers()
            IMAP_Client.Disconnect(True)
            SMTP_Client.Disconnect(True)
        End If
        ExitRequested = False

        'Restore handler
        SwitchCancellationHandler(LastShellType)
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
                Wdbg(DebugLevel.W, "Connection state is inconsistent. Stopping IMAPKeepConnection()...")
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
                Wdbg(DebugLevel.W, "Connection state is inconsistent. Stopping SMTPKeepConnection()...")
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
                    Wdbg(DebugLevel.I, "Opened inbox")
                    IMAP_Messages = IMAP_Client.Inbox.Search(SearchQuery.All).Reverse
                    Wdbg(DebugLevel.I, "Messages count: {0} messages", IMAP_Messages.LongCount)
                Else
                    Dim Folder As MailFolder = OpenFolder(IMAP_CurrentDirectory)
                    Wdbg(DebugLevel.I, "Opened {0}", IMAP_CurrentDirectory)
                    IMAP_Messages = Folder.Search(SearchQuery.All).Reverse
                    Wdbg(DebugLevel.I, "Messages count: {0} messages", IMAP_Messages.LongCount)
                End If
            End SyncLock
        End If
    End Sub

    ''' <summary>
    ''' Executed when the CountChanged event is fired.
    ''' </summary>
    ''' <param name="Sender">A folder</param>
    ''' <param name="e">Event arguments</param>
    Private Sub OnCountChanged(Sender As Object, e As EventArgs)
        Dim Folder As ImapFolder = Sender
        If Folder.Count > IMAP_Messages.Count Then
            Dim NewMessagesCount As Integer = Folder.Count - IMAP_Messages.Count
            NotifySend(New Notification(DoTranslation("{0} new messages arrived in inbox.").FormatString(NewMessagesCount),
                                        DoTranslation("Open ""lsmail"" to see them."),
                                        NotifPriority.Medium, NotifType.Normal))
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
    ''' Handles WebAlert sent by Gmail
    ''' </summary>
    Sub HandleWebAlert(sender As Object, e As WebAlertEventArgs)
        Wdbg(DebugLevel.I, "WebAlert URI: {0}", e.WebUri.AbsoluteUri)
        W(e.Message, True, ColTypes.Warning)
        W(DoTranslation("Opening URL... Make sure to follow the steps shown on the screen."), True, ColTypes.Neutral)
        Process.Start(e.WebUri.AbsoluteUri).WaitForExit()
    End Sub

End Module
