﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using KS.Kernel.Configuration;
using KS.Network.Base.Connections;
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

        internal static IEnumerable<UniqueId> IMAP_Messages;
        internal static int imapPingInterval = 30000;
        internal static int smtpPingInterval = 30000;
        internal static int maxMessagesInPage = 10;
        internal static NetworkConnection Client;

        /// <summary>
        /// IMAP current directory name
        /// </summary>
        public static string IMAP_CurrentDirectory { get; set; } = "Inbox";
        /// <summary>
        /// Notify on new mail arrival
        /// </summary>
        public static bool NotifyNewMail =>
            Config.MainConfig.MailNotifyNewMail;
        /// <summary>
        /// IMAP ping interval in milliseconds
        /// </summary>
        public static int ImapPingInterval =>
            Config.MainConfig.MailImapPingInterval;
        /// <summary>
        /// SMTP ping interval in milliseconds
        /// </summary>
        public static int SmtpPingInterval =>
            Config.MainConfig.MailSmtpPingInterval;
        /// <summary>
        /// Max messages per page
        /// </summary>
        public static int MaxMessagesInPage =>
            Config.MainConfig.MailMaxMessagesInPage;
        /// <summary>
        /// Message text format
        /// </summary>
        public static TextFormat TextFormat =>
            (TextFormat)Config.MainConfig.MailTextFormat;
        /// <summary>
        /// Show progress on mail transfer
        /// </summary>
        public static bool ShowProgress =>
            Config.MainConfig.ShowProgress;
        /// <summary>
        /// Mail progress style
        /// </summary>
        public static string ProgressStyle =>
            Config.MainConfig.MailProgressStyle;
        /// <summary>
        /// Mail progress style (single)
        /// </summary>
        public static string ProgressStyleSingle =>
            Config.MainConfig.MailProgressStyleSingle;
        /// <summary>
        /// The mail progress
        /// </summary>
        public readonly static MailTransferProgress Progress = new();

    }
}