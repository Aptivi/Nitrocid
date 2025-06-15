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
using Newtonsoft.Json;
using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;

namespace Nitrocid.Extras.Calendar.Calendar.Reminders
{
    /// <summary>
    /// Reminder information class
    /// </summary>
    public class ReminderInfo
    {

        private bool ReminderNotified;
        [JsonProperty(nameof(ReminderDate))]
        private readonly DateTime reminderDate;
        [JsonProperty(nameof(ReminderTitle))]
        private readonly string reminderTitle = "";
        [JsonProperty(nameof(ReminderImportance))]
        private readonly NotificationPriority reminderImportance;

        /// <summary>
        /// Reminder date
        /// </summary>
        [JsonIgnore]
        public DateTime ReminderDate
            => reminderDate;

        /// <summary>
        /// Reminder title
        /// </summary>
        [JsonIgnore]
        public string ReminderTitle
            => reminderTitle;

        /// <summary>
        /// Reminder importance
        /// </summary>
        [JsonIgnore]
        public NotificationPriority ReminderImportance
            => reminderImportance;

        /// <summary>
        /// Notifies the user about the reminder
        /// </summary>
        protected internal void NotifyReminder()
        {
            if (!ReminderNotified)
            {
                var ReminderNotification = new Notification(ReminderTitle, LanguageTools.GetLocalized("NKS_CALENDAR_REMINDER_NOTIFICATION"), ReminderImportance, NotificationType.Normal);
                NotificationManager.NotifySend(ReminderNotification);
                ReminderNotified = true;
            }
        }

        [JsonConstructor]
        internal ReminderInfo()
        { }

        internal ReminderInfo(DateTime reminderDate, string reminderTitle, NotificationPriority reminderImportance)
        {
            this.reminderDate = reminderDate;
            this.reminderTitle = reminderTitle;
            this.reminderImportance = reminderImportance;
        }

    }
}
