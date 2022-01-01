
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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
Imports MimeKit.Text

Public Module MailShell

    'Variables
    Public ReadOnly MailCommands As New Dictionary(Of String, CommandInfo) From {{"cd", New CommandInfo("cd", ShellCommandType.MailShell, "Changes current mail directory", {"<folder>"}, True, 1, New Mail_CdCommand)},
                                                                                 {"exit", New CommandInfo("exit", ShellCommandType.MailShell, "Exits the IMAP shell", {}, False, 0, New Mail_ExitCommand)},
                                                                                 {"help", New CommandInfo("help", ShellCommandType.MailShell, "List of commands", {"[command]"}, False, 0, New Mail_HelpCommand)},
                                                                                 {"lsdirs", New CommandInfo("lsdirs", ShellCommandType.MailShell, "Lists directories in your mail address", {}, False, 0, New Mail_LsDirsCommand)},
                                                                                 {"list", New CommandInfo("list", ShellCommandType.MailShell, "Downloads messages and lists them", {"[pagenum]"}, False, 0, New Mail_ListCommand)},
                                                                                 {"mkdir", New CommandInfo("mkdir", ShellCommandType.MailShell, "Makes a directory in the current working directory", {"<foldername>"}, True, 1, New Mail_MkdirCommand)},
                                                                                 {"mv", New CommandInfo("mv", ShellCommandType.MailShell, "Moves a message", {"<mailid> <targetfolder>"}, True, 2, New Mail_MvCommand)},
                                                                                 {"mvall", New CommandInfo("mvall", ShellCommandType.MailShell, "Moves all messages from recipient", {"<sendername> <targetfolder>"}, True, 2, New Mail_MvAllCommand)},
                                                                                 {"read", New CommandInfo("read", ShellCommandType.MailShell, "Opens a message", {"<mailid>"}, True, 1, New Mail_ReadCommand)},
                                                                                 {"readenc", New CommandInfo("readenc", ShellCommandType.MailShell, "Opens an encrypted message", {"<mailid>"}, True, 1, New Mail_ReadEncCommand)},
                                                                                 {"ren", New CommandInfo("ren", ShellCommandType.MailShell, "Renames a folder", {"<oldfoldername> <newfoldername>"}, True, 2, New Mail_RenCommand)},
                                                                                 {"rm", New CommandInfo("rm", ShellCommandType.MailShell, "Removes a message", {"<mailid>"}, True, 1, New Mail_RmCommand)},
                                                                                 {"rmall", New CommandInfo("rmall", ShellCommandType.MailShell, "Removes all messages from recipient", {"<sendername>"}, True, 1, New Mail_RmAllCommand)},
                                                                                 {"rmdir", New CommandInfo("rmdir", ShellCommandType.MailShell, "Removes a directory from the current working directory", {"<foldername>"}, True, 1, New Mail_RmdirCommand)},
                                                                                 {"send", New CommandInfo("send", ShellCommandType.MailShell, "Sends a message to an address", {}, False, 0, New Mail_SendCommand)},
                                                                                 {"sendenc", New CommandInfo("sendenc", ShellCommandType.MailShell, "Sends an encrypted message to an address", {}, False, 0, New Mail_SendEncCommand)}}
    Public IMAP_CurrentDirectory As String = "Inbox"
    Public MailModCommands As New ArrayList
    Public MailShellPromptStyle As String = ""
    Public Mail_NotifyNewMail As Boolean = True
    Public Mail_ImapPingInterval As Integer = 30000
    Public Mail_SmtpPingInterval As Integer = 30000
    Public Mail_POP3PingInterval As Integer = 30000
    Public Mail_MaxMessagesInPage As Integer = 10
    Public Mail_UsePop3 As Boolean
    Public Mail_TextFormat As TextFormat = TextFormat.Plain
    Public Mail_ShowProgress As Boolean = True
    Public Mail_ProgressStyle As String = ""
    Public Mail_ProgressStyleSingle As String = ""
    Public ReadOnly Mail_Progress As New MailTransferProgress
    Friend ExitRequested, KeepAlive As Boolean
    Friend IMAP_Messages As IEnumerable(Of UniqueId)

    ''' <summary>
    ''' Initializes the shell of the mail client
    ''' </summary>
    ''' <param name="Address">An e-mail address or username. This is used to show address in command input.</param>
    Sub OpenMailShell(Address As String)
        'Send ping to keep the connection alive
        Dim IMAP_NoOp As New Thread(AddressOf IMAPKeepConnection) With {.Name = "IMAP Keep Connection"}
        IMAP_NoOp.Start()
        Wdbg(DebugLevel.I, "Made new thread about IMAPKeepConnection()")
        If Not Mail_UsePop3 Then
            Dim SMTP_NoOp As New Thread(AddressOf SMTPKeepConnection) With {.Name = "SMTP Keep Connection"}
            SMTP_NoOp.Start()
            Wdbg(DebugLevel.I, "Made new thread about SMTPKeepConnection()")
        Else
            Dim POP3_NoOp As New Thread(AddressOf POP3KeepConnection) With {.Name = "POP3 Keep Connection"}
            POP3_NoOp.Start()
            Wdbg(DebugLevel.I, "Made new thread about POP3KeepConnection()")
        End If

        'Add handler for IMAP and SMTP
        SwitchCancellationHandler(ShellCommandType.MailShell)
        KernelEventManager.RaiseIMAPShellInitialized()

        While Not ExitRequested
            SyncLock MailCancelSync
                'Populate messages
                PopulateMessages()
                If Mail_NotifyNewMail Then InitializeHandlers()

                'Initialize prompt
                If DefConsoleOut IsNot Nothing Then
                    Console.SetOut(DefConsoleOut)
                End If
                Wdbg(DebugLevel.I, "MailShellPromptStyle = {0}", MailShellPromptStyle)
                If MailShellPromptStyle = "" Then
                    Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.UserName, Mail_Authentication.UserName) : Write("|", False, ColTypes.Gray) : Write("{0}", False, ColTypes.HostName, Address) : Write("] ", False, ColTypes.Gray) : Write("{0} > ", False, ColTypes.Gray, IMAP_CurrentDirectory)
                Else
                    Dim ParsedPromptStyle As String = ProbePlaces(MailShellPromptStyle)
                    ParsedPromptStyle.ConvertVTSequences
                    Write(ParsedPromptStyle, False, ColTypes.Gray)
                End If
                SetInputColor()

                'Listen for a command
                Dim cmd As String = Console.ReadLine
                If Not (cmd = Nothing Or cmd?.StartsWithAnyOf({" ", "#"}) = True) Then
                    KernelEventManager.RaiseIMAPPreExecuteCommand(cmd)
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
                        Write(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands."), True, ColTypes.Error, words(0))
                    End If
                    KernelEventManager.RaiseIMAPPostExecuteCommand(cmd)
                End If
            End SyncLock
        End While

        'Disconnect the session
        IMAP_CurrentDirectory = "Inbox"
        If KeepAlive Then
            Wdbg(DebugLevel.W, "Exit requested, but not disconnecting.")
        Else
            Wdbg(DebugLevel.W, "Exit requested. Disconnecting host...")
            If Mail_NotifyNewMail Then ReleaseHandlers()
            IMAP_Client.Disconnect(True)
            SMTP_Client.Disconnect(True)
            POP3_Client.Disconnect(True)
        End If
        ExitRequested = False

        'Restore handler
        SwitchCancellationHandler(LastShellType)
    End Sub

End Module
