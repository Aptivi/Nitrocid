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

using System;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Shells.Mail.Tools;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.Mail
{
    /// <summary>
    /// Mail management module
    /// </summary>
    public partial class MailShell : BaseShell, IShell
    {
        /// <summary>
        /// Lists messages
        /// </summary>
        /// <param name="PageNum">Page number</param>
        /// <exception cref="ArgumentException"></exception>
        public void MailListMessages(int PageNum) =>
            MailTools.MailListMessages(ImapClient, Pop3Client, IMAP_CurrentDirectory, PageNum);

        /// <summary>
        /// Lists messages
        /// </summary>
        /// <param name="PageNum">Page number</param>
        /// <param name="MessagesInPage">Max messages in one page</param>
        /// <exception cref="ArgumentException"></exception>
        public void MailListMessages(int PageNum, int MessagesInPage) =>
            MailTools.MailListMessages(ImapClient, Pop3Client, IMAP_CurrentDirectory, PageNum, MessagesInPage);

        /// <summary>
        /// Removes a message
        /// </summary>
        /// <param name="MsgNumber">Message number</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool MailRemoveMessage(int MsgNumber) =>
            MailTools.MailRemoveMessage(ImapClient, Pop3Client, IMAP_CurrentDirectory, MsgNumber);

        /// <summary>
        /// Removes all mail that the specified sender has sent
        /// </summary>
        /// <param name="Sender">The sender name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool MailRemoveAllBySender(string Sender) =>
            MailTools.MailRemoveAllBySender(ImapClient, Pop3Client, IMAP_CurrentDirectory, Sender);

        /// <summary>
        /// Moves a message
        /// </summary>
        /// <param name="MsgNumber">Message number</param>
        /// <param name="TargetFolder">Target folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool MailMoveMessage(int MsgNumber, string TargetFolder)
        {
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED -> IMAP server is not connected
            if (ImapClient is null)
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED"));
            return MailTools.MailMoveMessage(ImapClient, IMAP_CurrentDirectory, MsgNumber, TargetFolder);
        }

        /// <summary>
        /// Moves all mail that the specified sender has sent
        /// </summary>
        /// <param name="Sender">The sender name</param>
        /// <param name="TargetFolder">Target folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool MailMoveAllBySender(string Sender, string TargetFolder)
        {
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED -> IMAP server is not connected
            if (ImapClient is null)
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED"));
            return MailTools.MailMoveAllBySender(ImapClient, IMAP_CurrentDirectory, Sender, TargetFolder);
        }
    }
}
