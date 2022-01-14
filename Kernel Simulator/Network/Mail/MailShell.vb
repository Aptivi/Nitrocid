
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

Imports MailKit
Imports MimeKit.Text

Public Module MailShellCommon

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
    Friend KeepAlive As Boolean
    Friend IMAP_Messages As IEnumerable(Of UniqueId)

End Module
