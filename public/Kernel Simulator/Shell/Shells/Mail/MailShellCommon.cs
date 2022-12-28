
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
        internal static bool KeepAlive;
        internal static IEnumerable<UniqueId> IMAP_Messages;

    }
}
