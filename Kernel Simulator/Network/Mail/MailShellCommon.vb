
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
Imports KS.Network.Mail.Commands
Imports KS.Network.Mail.Transfer

Namespace Network.Mail
    Public Module MailShellCommon

        'Variables
        Public ReadOnly MailCommands As New Dictionary(Of String, CommandInfo) From {
            {"cd", New CommandInfo("cd", ShellType.MailShell, "Changes current mail directory", New CommandArgumentInfo({"<folder>"}, True, 1), New Mail_CdCommand)},
            {"lsdirs", New CommandInfo("lsdirs", ShellType.MailShell, "Lists directories in your mail address", New CommandArgumentInfo(), New Mail_LsDirsCommand)},
            {"list", New CommandInfo("list", ShellType.MailShell, "Downloads messages and lists them", New CommandArgumentInfo({"[pagenum]"}, False, 0), New Mail_ListCommand)},
            {"mkdir", New CommandInfo("mkdir", ShellType.MailShell, "Makes a directory in the current working directory", New CommandArgumentInfo({"<foldername>"}, True, 1), New Mail_MkdirCommand)},
            {"mv", New CommandInfo("mv", ShellType.MailShell, "Moves a message", New CommandArgumentInfo({"<mailid> <targetfolder>"}, True, 2), New Mail_MvCommand)},
            {"mvall", New CommandInfo("mvall", ShellType.MailShell, "Moves all messages from recipient", New CommandArgumentInfo({"<sendername> <targetfolder>"}, True, 2), New Mail_MvAllCommand)},
            {"read", New CommandInfo("read", ShellType.MailShell, "Opens a message", New CommandArgumentInfo({"<mailid>"}, True, 1), New Mail_ReadCommand)},
            {"readenc", New CommandInfo("readenc", ShellType.MailShell, "Opens an encrypted message", New CommandArgumentInfo({"<mailid>"}, True, 1), New Mail_ReadEncCommand)},
            {"ren", New CommandInfo("ren", ShellType.MailShell, "Renames a folder", New CommandArgumentInfo({"<oldfoldername> <newfoldername>"}, True, 2), New Mail_RenCommand)},
            {"rm", New CommandInfo("rm", ShellType.MailShell, "Removes a message", New CommandArgumentInfo({"<mailid>"}, True, 1), New Mail_RmCommand)},
            {"rmall", New CommandInfo("rmall", ShellType.MailShell, "Removes all messages from recipient", New CommandArgumentInfo({"<sendername>"}, True, 1), New Mail_RmAllCommand)},
            {"rmdir", New CommandInfo("rmdir", ShellType.MailShell, "Removes a directory from the current working directory", New CommandArgumentInfo({"<foldername>"}, True, 1), New Mail_RmdirCommand)},
            {"send", New CommandInfo("send", ShellType.MailShell, "Sends a message to an address", New CommandArgumentInfo(), New Mail_SendCommand)},
            {"sendenc", New CommandInfo("sendenc", ShellType.MailShell, "Sends an encrypted message to an address", New CommandArgumentInfo(), New Mail_SendEncCommand)}
        }
        Public IMAP_CurrentDirectory As String = "Inbox"
        Public Mail_NotifyNewMail As Boolean = True
        Public Mail_ImapPingInterval As Integer = 30000
        Public Mail_SmtpPingInterval As Integer = 30000
        Public Mail_MaxMessagesInPage As Integer = 10
        Public Mail_TextFormat As TextFormat = TextFormat.Plain
        Public Mail_ShowProgress As Boolean = True
        Public Mail_ProgressStyle As String = ""
        Public Mail_ProgressStyleSingle As String = ""
        Public ReadOnly Mail_Progress As New MailTransferProgress
        Friend ReadOnly MailModCommands As New Dictionary(Of String, CommandInfo)
        Friend KeepAlive As Boolean
        Friend IMAP_Messages As IEnumerable(Of UniqueId)

    End Module
End Namespace
