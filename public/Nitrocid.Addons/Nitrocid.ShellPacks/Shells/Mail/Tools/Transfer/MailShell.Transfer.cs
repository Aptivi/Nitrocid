//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using MailKit;
using MimeKit;
using Nitrocid.ShellPacks.Shells.Mail.Tools;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.Mail
{
    /// <summary>
    /// Mail transfer module
    /// </summary>
    public partial class MailShell : BaseShell, IShell
    {
        /// <summary>
        /// Prints content of message to console
        /// </summary>
        /// <param name="MessageNum">Message number</param>
        public void MailPrintMessage(int MessageNum) =>
            MailTools.MailPrintMessage(ImapClient, IMAP_CurrentDirectory, MessageNum);

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body (only text. See <see cref="MailSendMessage(string, string, MimeEntity)"/> for more.)</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public bool MailSendMessage(string Recipient, string Subject, string Body) =>
            MailTools.MailSendMessage(SmtpClient, NetworkCredential.UserName, Recipient, Subject, Body);

        /// <summary>
        /// Sends a message with advanced features like attachments
        /// </summary>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public bool MailSendMessage(string Recipient, string Subject, MimeEntity Body) =>
            MailTools.MailSendMessage(SmtpClient, NetworkCredential.UserName, Recipient, Subject, Body);

        /// <summary>
        /// Sends an encrypted message with advanced features like attachments
        /// </summary>
        /// <param name="Recipient">Recipient name</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public bool MailSendEncryptedMessage(string Recipient, string Subject, MimeEntity Body) =>
            MailTools.MailSendEncryptedMessage(SmtpClient, NetworkCredential.UserName, Recipient, Subject, Body);

        /// <summary>
        /// Populates e-mail messages
        /// </summary>
        public IEnumerable<UniqueId>? PopulateMessages() =>
            MailTools.PopulateMessages(ImapClient, IMAP_CurrentDirectory);
    }
}
