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
using System.Collections.Generic;
using System.Text;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;
using Terminaux.Inputs.Interactive;
using Textify.General;

namespace Nitrocid.Base.Misc.Interactives
{
    internal class NotificationsCli : BaseInteractiveTui<Notification>, IInteractiveTui<Notification>
    {
        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_HELP01_BODY") + "\n\n" +
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/advanced-and-power-users/inner-workings/inner-essentials/system-notifications",
            }
        ];

        /// <inheritdoc/>
        public override IEnumerable<Notification> PrimaryDataSource =>
            NotificationManager.NotifRecents;

        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetInfoFromItem(Notification item)
        {
            // Generate the rendered text
            string name = item.Title;
            string description = item.Desc;
            StringBuilder builder = new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_IMPORTANCE") + $": {item.Priority}" + CharManager.NewLine);

            // If the notification is a progress one, go ahead and add progress info
            if (item.Type == NotificationType.Progress)
            {
                builder.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_PROGPERCENT") + $": {item.Progress}%");
                builder.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_PROGCOMPLETED") + $": {item.ProgressCompleted}");
                builder.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_PROGINDETERMINATE") + $": {item.ProgressIndeterminate}");
                builder.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_PROGSTATE") + $": {item.ProgressState}");
            }

            // Render them to the second pane
            return
                LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_NOTIFTITLE") + $": {name}" + CharManager.NewLine +
                $"{builder}" + CharManager.NewLine +
                $"{description}";
            ;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(Notification item) =>
            item.Title;

        /// <inheritdoc/>
        public override string GetEntryFromItem(Notification item) =>
            item.Title;

        internal void Dismiss(Notification? notification)
        {
            if (notification is null)
                return;
            var notifs = NotificationManager.NotifRecents;
            for (int i = notifs.Count - 1; i > 0; i--)
            {
                var notif = notifs[i];
                if (notif == notification)
                    NotificationManager.NotifDismiss(i);
            }
        }

        internal void DismissAll() =>
            NotificationManager.NotifDismissAll();

        internal static void OpenNotificationsCli()
        {
            var tui = new NotificationsCli();
            tui.Bindings.Add(new InteractiveTuiBinding<Notification>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_KEYBINDING_DISMISS"), ConsoleKey.Delete, (notif, _, _, _) => tui.Dismiss(notif)));
            tui.Bindings.Add(new InteractiveTuiBinding<Notification>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_NOTIFICATIONSCLI_KEYBINDING_DISMISSALL"), ConsoleKey.Delete, ConsoleModifiers.Control, (_, _, _, _) => tui.DismissAll()));
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
