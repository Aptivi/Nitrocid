
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

using System;
using KS.Languages;
using KS.Misc.Notifications;

namespace KS.Misc.Calendar.Reminders
{
    /// <summary>
    /// Reminder information class
    /// </summary>
    public class ReminderInfo
    {

        private bool ReminderNotified;
        /// <summary>
        /// Reminder date
        /// </summary>
        public DateTime ReminderDate { get; set; }
        /// <summary>
        /// Reminder title
        /// </summary>
        public string ReminderTitle { get; set; }
        /// <summary>
        /// Reminder importance
        /// </summary>
        public Notifications.NotificationManager.NotifPriority ReminderImportance { get; set; }

        /// <summary>
        /// Notifies the user about the reminder
        /// </summary>
        protected internal void NotifyReminder()
        {
            if (!ReminderNotified)
            {
                var ReminderNotification = new Notification(ReminderTitle, Translate.DoTranslation("Don't miss this!"), ReminderImportance, Notifications.NotificationManager.NotifType.Normal);
                Notifications.NotificationManager.NotifySend(ReminderNotification);
                ReminderNotified = true;
            }
        }

    }
}
