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
using System.Linq;
using MailKit.Net.Imap;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;
using Nitrocid.ShellPacks.Shells.Mail.Tools;
using Terminaux.Shell.Shells;
using Textify.General;

namespace Nitrocid.ShellPacks.Shells.Mail
{
    /// <summary>
    /// Mail event handlers
    /// </summary>
    public partial class MailShell : BaseShell, IShell
    {

        /// <summary>
        /// Initializes the CountChanged handlers. Currently, it only supports inbox.
        /// </summary>
        public void InitializeHandlers()
        {
            if (ImapClient is null)
                return;
            var inbox = ImapClient?.Inbox ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_INBOXOBTAINFAILED"));
            inbox.CountChanged += OnCountChanged;
        }

        /// <summary>
        /// Releases the CountChanged handlers. Currently, it only supports inbox.
        /// </summary>
        public void ReleaseHandlers()
        {
            if (ImapClient is null)
                return;
            var inbox = ImapClient?.Inbox ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_INBOXOBTAINFAILED"));
            inbox.CountChanged -= OnCountChanged;
        }

        /// <summary>
        /// Executed when the CountChanged event is fired.
        /// </summary>
        /// <param name="sender">A folder</param>
        /// <param name="e">Event arguments</param>
        public void OnCountChanged(object? sender, EventArgs e)
        {
            if (sender is not ImapFolder folder)
                return;
            int newMessagesCount = 0;
            bool notify = false;
            if (ProtocolType == MailProtocolType.POP3)
            {
                if (POP3_Messages is null)
                    return;
                if (folder.Count > POP3_Messages.Count())
                    newMessagesCount = folder.Count - POP3_Messages.Count();
            }
            else
            {
                if (IMAP_Messages is null)
                    return;
                if (folder.Count > IMAP_Messages.Count())
                    newMessagesCount = folder.Count - IMAP_Messages.Count();
            }
            if (notify)
                NotificationManager.NotifySend(new Notification(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_NEWMESSAGES_NOTIFICATION_TITLE").FormatString(newMessagesCount), LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_NEWMESSAGES_NOTIFICATION_DESC"), NotificationPriority.Medium, NotificationType.Normal));
        }

    }
}
