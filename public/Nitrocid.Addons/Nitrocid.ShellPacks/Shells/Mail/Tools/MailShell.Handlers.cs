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
            var client = (ImapClient)((Client?.ConnectionInstance) ?? [])[0];
            var inbox = client.Inbox ??
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_INBOXOBTAINFAILED"));
            inbox.CountChanged += OnCountChanged;
        }

        /// <summary>
        /// Releases the CountChanged handlers. Currently, it only supports inbox.
        /// </summary>
        public void ReleaseHandlers()
        {
            var client = (ImapClient)((Client?.ConnectionInstance) ?? [])[0];
            var inbox = client.Inbox ??
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
            var messages = IMAP_Messages ?? [];
            if (folder.Count > messages.Count())
            {
                int NewMessagesCount = folder.Count - messages.Count();
                NotificationManager.NotifySend(new Notification(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_NEWMESSAGES_NOTIFICATION_TITLE").FormatString(NewMessagesCount), LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_NEWMESSAGES_NOTIFICATION_DESC"), NotificationPriority.Medium, NotificationType.Normal));
            }
        }

    }
}
