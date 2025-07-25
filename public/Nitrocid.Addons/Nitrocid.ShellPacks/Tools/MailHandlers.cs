//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using System;
using System.Linq;
using MailKit;
using MailKit.Net.Imap;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;
using Textify.General;
using SpecProbe.Software.Platform;
using Nitrocid.ShellPacks.Shells.Mail;

namespace Nitrocid.ShellPacks.Tools
{
    /// <summary>
    /// Mail event handlers
    /// </summary>
    public static class MailHandlers
    {

        /// <summary>
        /// Initializes the CountChanged handlers. Currently, it only supports inbox.
        /// </summary>
        public static void InitializeHandlers() => ((ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0]).Inbox.CountChanged += OnCountChanged;

        /// <summary>
        /// Releases the CountChanged handlers. Currently, it only supports inbox.
        /// </summary>
        public static void ReleaseHandlers() => ((ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0]).Inbox.CountChanged -= OnCountChanged;

        /// <summary>
        /// Handles WebAlert sent by Gmail
        /// </summary>
        public static void HandleWebAlert(object? sender, WebAlertEventArgs e)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "WebAlert URI: {0}", vars: [e.WebUri.AbsoluteUri]);
            TextWriterColor.Write(e.Message, true, ThemeColorType.Warning);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_WEBALERT_OPENING"));
            PlatformHelper.PlatformOpen(e.WebUri.AbsoluteUri);
        }

        /// <summary>
        /// Executed when the CountChanged event is fired.
        /// </summary>
        /// <param name="sender">A folder</param>
        /// <param name="e">Event arguments</param>
        public static void OnCountChanged(object? sender, EventArgs e)
        {
            if (sender is not ImapFolder folder)
                return;
            var messages = MailShellCommon.IMAP_Messages ?? [];
            if (folder.Count > messages.Count())
            {
                int NewMessagesCount = folder.Count - messages.Count();
                NotificationManager.NotifySend(new Notification(LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_NEWMESSAGES_NOTIFICATION_TITLE").FormatString(NewMessagesCount), LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_NEWMESSAGES_NOTIFICATION_DESC"), NotificationPriority.Medium, NotificationType.Normal));
            }
        }

    }
}
