//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Calendars;
using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;

namespace Nitrocid.Extras.Calendar.Calendar.Events
{
    /// <summary>
    /// Event information class
    /// </summary>
    public class EventInfo
    {

        internal DateTime start = DateTime.Today;
        internal DateTime end = DateTime.Today;
        private bool EventNotified;

        [JsonProperty(nameof(EventDate))]
        private DateTime eventDate;
        [JsonProperty(nameof(EventTitle))]
        private string eventTitle = "";
        [JsonProperty(nameof(IsYearly))]
        private bool isYearly;
        [JsonProperty(nameof(StartMonth))]
        private int startMonth;
        [JsonProperty(nameof(StartDay))]
        private int startDay;
        [JsonProperty(nameof(EndMonth))]
        private int endMonth;
        [JsonProperty(nameof(EndDay))]
        private int endDay;
        [JsonProperty(nameof(Calendar))]
        private string calendar = "";

        [JsonIgnore]
        private readonly int origStartMonth;
        [JsonIgnore]
        private readonly int origStartDay;
        [JsonIgnore]
        private readonly int origEndMonth;
        [JsonIgnore]
        private readonly int origEndDay;

        /// <summary>
        /// Event date
        /// </summary>
        [JsonIgnore]
        public DateTime EventDate =>
            eventDate;

        /// <summary>
        /// Event title
        /// </summary>
        [JsonIgnore]
        public string EventTitle =>
            eventTitle;

        /// <summary>
        /// Is this event a yearly event?
        /// </summary>
        [JsonIgnore]
        public bool IsYearly =>
            isYearly;

        /// <summary>
        /// The month in which the event starts
        /// </summary>
        [JsonIgnore]
        public int StartMonth =>
            startMonth;

        /// <summary>
        /// The day in which the event starts
        /// </summary>
        [JsonIgnore]
        public int StartDay
            => startDay;

        /// <summary>
        /// The start <see cref="DateTime"/> instance representing the start of the event
        /// </summary>
        [JsonIgnore]
        public DateTime Start =>
            start;

        /// <summary>
        /// The month in which the event ends
        /// </summary>
        [JsonIgnore]
        public int EndMonth =>
            endMonth;

        /// <summary>
        /// The day in which the event ends
        /// </summary>
        [JsonIgnore]
        public int EndDay
            => endDay;

        /// <summary>
        /// The end <see cref="DateTime"/> instance representing the end of the event
        /// </summary>
        [JsonIgnore]
        public DateTime End =>
            end;

        /// <summary>
        /// The calendar name in which the event is assigned to
        /// </summary>
        [JsonIgnore]
        public string Calendar
            => calendar;

        /// <summary>
        /// Notifies the user about the event
        /// </summary>
        protected internal void NotifyEvent()
        {
            if (!EventNotified)
            {
                var EventNotification = new Notification(EventTitle, Translate.DoTranslation("Now it's an event day!"), NotificationPriority.Medium, NotificationType.Normal);
                NotificationManager.NotifySend(EventNotification);
                EventNotified = true;
            }
        }

        internal void UpdateEventInfo(DateTime target) =>
            UpdateEventInfo(target, EventDate, EventTitle, IsYearly, origStartMonth, origStartDay, origEndMonth, origEndDay, Calendar);

        internal void UpdateEventInfo(DateTime target, DateTime eventDate, string eventTitle, bool isYearly, int startMonth, int startDay, int endMonth, int endDay, string calendar)
        {
            this.eventDate = eventDate;
            start = eventDate;
            end = eventDate;
            this.eventTitle = eventTitle;
            this.isYearly = isYearly;
            this.startMonth = startMonth;
            this.startDay = startDay;
            this.endMonth = endMonth;
            this.endDay = endDay;
            this.calendar = calendar;

            if (!Enum.TryParse(calendar, out CalendarTypes calendarType))
                calendarType = CalendarTypes.Gregorian;

            // If the calendar is not Gregorian (for example, Hijri), convert that to Gregorian using the target date
            if (calendarType != CalendarTypes.Gregorian)
            {
                var calendarInstance = CalendarTools.GetCalendar(calendarType);
                int year = calendarInstance.Culture.DateTimeFormat.Calendar.GetYear(target);
                int yearEnd = year;
                int monthStart = this.startMonth;
                int monthEnd = this.endMonth;
                var dayStart = this.startDay;
                var dayEnd = this.endDay;
                if (monthEnd < monthStart)
                    yearEnd++;
                var dateTimeStart = new DateTime(year, monthStart, dayStart, calendarInstance.Culture.DateTimeFormat.Calendar);
                var dateTimeEnd = new DateTime(yearEnd, monthEnd, dayEnd, calendarInstance.Culture.DateTimeFormat.Calendar);
                this.startMonth = dateTimeStart.Month;
                this.endMonth = dateTimeEnd.Month;
                this.startDay = dateTimeStart.Day;
                this.endDay = dateTimeEnd.Day;
            }

            // Month sanity checks
            this.startMonth =
                this.startMonth < 1 ? 1 :
                this.startMonth > 12 ? 12 :
                this.startMonth;
            this.endMonth =
                this.endMonth < 1 ? 1 :
                this.endMonth > 12 ? 12 :
                this.endMonth;

            // Day sanity checks
            int maxDayNumStart = DateTime.DaysInMonth(TimeDateTools.KernelDateTime.Year, this.startMonth);
            int maxDayNumEnd = DateTime.DaysInMonth(TimeDateTools.KernelDateTime.Year, this.endMonth);
            this.startDay =
                this.startDay < 1 ? 1 :
                this.startDay > maxDayNumStart ? maxDayNumStart :
                this.startDay;
            this.endDay =
                this.endDay < 1 ? 1 :
                this.endDay > maxDayNumEnd ? maxDayNumEnd :
                this.endDay;

            // Check to see if the end is earlier than the start
            start = new(target.Year, this.startMonth, this.startDay);
            end = new(target.Year, this.endMonth, this.endDay);
            if (start > end)
            {
                // End is earlier than start! Swap the two values so that:
                //    start = end;
                //    end = start;
                (end, start) = (start, end);

                // Deal with the start and the end
                if (this.startMonth > this.endMonth)
                    end = end.AddYears(1);
                else if (this.startDay > this.endDay)
                    (this.endDay, this.startDay) = (this.startDay, this.endDay);
            }
        }

        internal EventInfo(DateTime eventDate, string eventTitle) :
            this(eventDate, eventTitle, false, 0, 0, 0, 0, "Gregorian")
        { }

        internal EventInfo(DateTime eventDate, string eventTitle, bool isYearly, int startMonth, int startDay, int endMonth, int endDay, string calendar)
        {
            origStartMonth = startMonth;
            origStartDay = startDay;
            origEndMonth = endMonth;
            origEndDay = endDay;
            UpdateEventInfo(TimeDateTools.KernelDateTime, eventDate, eventTitle, isYearly, startMonth, startDay, endMonth, endDay, calendar);
        }

        [JsonConstructor]
        internal EventInfo()
        { }
    }
}
