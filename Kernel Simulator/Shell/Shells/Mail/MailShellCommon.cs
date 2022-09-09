
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using KS.Network.Mail.Transfer;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Mail.Commands;
using MailKit;
using MimeKit.Text;

namespace KS.Shell.Shells.Mail
{
    /// <summary>
    /// Mail shell common module
    /// </summary>
    public static class MailShellCommon
    {

        /// <summary>
        /// Mail shell commands
        /// </summary>
        public readonly static Dictionary<string, CommandInfo> MailCommands = new()
        {
            { "cd", new CommandInfo("cd", ShellType.MailShell, "Changes current mail directory", new CommandArgumentInfo(new[] { "<folder>" }, true, 1), new Mail_CdCommand()) },
            { "lsdirs", new CommandInfo("lsdirs", ShellType.MailShell, "Lists directories in your mail address", new CommandArgumentInfo(), new Mail_LsDirsCommand()) },
            { "list", new CommandInfo("list", ShellType.MailShell, "Downloads messages and lists them", new CommandArgumentInfo(new[] { "[pagenum]" }, false, 0), new Mail_ListCommand()) },
            { "mkdir", new CommandInfo("mkdir", ShellType.MailShell, "Makes a directory in the current working directory", new CommandArgumentInfo(new[] { "<foldername>" }, true, 1), new Mail_MkdirCommand()) },
            { "mv", new CommandInfo("mv", ShellType.MailShell, "Moves a message", new CommandArgumentInfo(new[] { "<mailid> <targetfolder>" }, true, 2), new Mail_MvCommand()) },
            { "mvall", new CommandInfo("mvall", ShellType.MailShell, "Moves all messages from recipient", new CommandArgumentInfo(new[] { "<sendername> <targetfolder>" }, true, 2), new Mail_MvAllCommand()) },
            { "read", new CommandInfo("read", ShellType.MailShell, "Opens a message", new CommandArgumentInfo(new[] { "<mailid>" }, true, 1), new Mail_ReadCommand()) },
            { "readenc", new CommandInfo("readenc", ShellType.MailShell, "Opens an encrypted message", new CommandArgumentInfo(new[] { "<mailid>" }, true, 1), new Mail_ReadEncCommand()) },
            { "ren", new CommandInfo("ren", ShellType.MailShell, "Renames a folder", new CommandArgumentInfo(new[] { "<oldfoldername> <newfoldername>" }, true, 2), new Mail_RenCommand()) },
            { "rm", new CommandInfo("rm", ShellType.MailShell, "Removes a message", new CommandArgumentInfo(new[] { "<mailid>" }, true, 1), new Mail_RmCommand()) },
            { "rmall", new CommandInfo("rmall", ShellType.MailShell, "Removes all messages from recipient", new CommandArgumentInfo(new[] { "<sendername>" }, true, 1), new Mail_RmAllCommand()) },
            { "rmdir", new CommandInfo("rmdir", ShellType.MailShell, "Removes a directory from the current working directory", new CommandArgumentInfo(new[] { "<foldername>" }, true, 1), new Mail_RmdirCommand()) },
            { "send", new CommandInfo("send", ShellType.MailShell, "Sends a message to an address", new CommandArgumentInfo(), new Mail_SendCommand()) },
            { "sendenc", new CommandInfo("sendenc", ShellType.MailShell, "Sends an encrypted message to an address", new CommandArgumentInfo(), new Mail_SendEncCommand()) }
        };
        /// <summary>
        /// IMAP current directory name
        /// </summary>
        public static string IMAP_CurrentDirectory = "Inbox";
        /// <summary>
        /// Notify on new mail arrival
        /// </summary>
        public static bool Mail_NotifyNewMail = true;
        /// <summary>
        /// IMAP ping interval in milliseconds
        /// </summary>
        public static int Mail_ImapPingInterval = 30000;
        /// <summary>
        /// SMTP ping interval in milliseconds
        /// </summary>
        public static int Mail_SmtpPingInterval = 30000;
        /// <summary>
        /// Max messages per page
        /// </summary>
        public static int Mail_MaxMessagesInPage = 10;
        /// <summary>
        /// Message text format
        /// </summary>
        public static TextFormat Mail_TextFormat = TextFormat.Plain;
        /// <summary>
        /// Show progress on mail transfer
        /// </summary>
        public static bool Mail_ShowProgress = true;
        /// <summary>
        /// Mail progress style
        /// </summary>
        public static string Mail_ProgressStyle = "";
        /// <summary>
        /// Mail progress style (single)
        /// </summary>
        public static string Mail_ProgressStyleSingle = "";
        /// <summary>
        /// The mail progress
        /// </summary>
        public readonly static MailTransferProgress Mail_Progress = new();
        internal readonly static Dictionary<string, CommandInfo> MailModCommands = new();
        internal static bool KeepAlive;
        internal static IEnumerable<UniqueId> IMAP_Messages;

    }
}
