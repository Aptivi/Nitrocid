
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using MailKit;
using MimeKit.Text;

namespace KS.Shell.Shells.Mail
{
    /// <summary>
    /// Mail shell common module
    /// </summary>
    public static class MailShellCommon
    {

        internal static bool KeepAlive;
        internal static IEnumerable<UniqueId> IMAP_Messages;
        internal static int imapPingInterval = 30000;
        internal static int smtpPingInterval = 30000;
        internal static int maxMessagesInPage = 10;

        /// <summary>
        /// IMAP current directory name
        /// </summary>
        public static string IMAP_CurrentDirectory { get; set; } = "Inbox";
        /// <summary>
        /// Notify on new mail arrival
        /// </summary>
        public static bool Mail_NotifyNewMail { get; set; } = true;
        /// <summary>
        /// IMAP ping interval in milliseconds
        /// </summary>
        public static int Mail_ImapPingInterval
        {
            get => imapPingInterval;
            set => imapPingInterval = value < 0 ? 30000 : value;
        }
        /// <summary>
        /// SMTP ping interval in milliseconds
        /// </summary>
        public static int Mail_SmtpPingInterval
        {
            get => smtpPingInterval;
            set => smtpPingInterval = value < 0 ? 30000 : value;
        }
        /// <summary>
        /// Max messages per page
        /// </summary>
        public static int Mail_MaxMessagesInPage
        {
            get => maxMessagesInPage;
            set => maxMessagesInPage = value < 0 ? 10 : value;
        }
        /// <summary>
        /// Message text format
        /// </summary>
        public static TextFormat Mail_TextFormat { get; set; } = TextFormat.Plain;
        /// <summary>
        /// Show progress on mail transfer
        /// </summary>
        public static bool Mail_ShowProgress { get; set; } = true;
        /// <summary>
        /// Mail progress style
        /// </summary>
        public static string Mail_ProgressStyle { get; set; } = "";
        /// <summary>
        /// Mail progress style (single)
        /// </summary>
        public static string Mail_ProgressStyleSingle { get; set; } = "";
        /// <summary>
        /// The mail progress
        /// </summary>
        public readonly static MailTransferProgress Mail_Progress = new();

    }
}
