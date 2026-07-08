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
using System.Collections.Generic;
using System.Linq;
using Calendrier;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Time.Converters;
using Nitrocid.Extras.Calendar.Calendar.Events;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Nitrocid.Extras.Calendar.Calendar
{
    /// <summary>
    /// Calendar printing module
    /// </summary>
    public static class CalendarPrint
    {

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar() =>
            PrintCalendar(TimeDateTools.KernelDateTime.Year, TimeDateTools.KernelDateTime.Month);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        /// <param name="calendar">Calendar type</param>
        public static void PrintCalendar(CalendarTypes calendar) =>
            PrintCalendar(TimeDateTools.KernelDateTime.Year, TimeDateTools.KernelDateTime.Month, calendar);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        /// <param name="Year">Year to use</param>
        /// <param name="Month">Month to use</param>
        /// <param name="calendar">Calendar type</param>
        public static void PrintCalendar(int Year, int Month, CalendarTypes calendar = CalendarTypes.Variant)
        {
            // Render the calendar
            var calendars = RenderCalendar(Year, Month, calendar);
            TextWriterRaw.WritePlain(calendars.Render());
        }

        internal static Calendars RenderCalendar(int Year, int Month, CalendarTypes calendar = CalendarTypes.Variant)
        {
            var calendarInstance = CalendarTools.GetCalendar(calendar);
            var maxDate = DateTime.DaysInMonth(Year, Month);

            // List events and reminders
            var events = new List<DateTime>();
            var reminders = new List<DateTime>();
            for (int currentDay = 1; currentDay <= maxDate; currentDay++)
            {
                var currentDate = new DateTime(Year, Month, currentDay);

                // Know where and how to put the day number
                foreach (ReminderInfo Reminder in ReminderManager.Reminders)
                {
                    var rDate = Reminder.ReminderDate.Date;
                    if (rDate == currentDate)
                        reminders.Add(rDate);
                }
                foreach (EventInfo EventInstance in EventManager.CalendarEvents.Union(EventManager.baseEvents))
                {
                    EventInstance.UpdateEventInfo(new DateTime(Year, 1, 1));
                    var nDate = EventInstance.EventDate.Date;
                    var sDate = EventInstance.Start.Date;
                    var eDate = EventInstance.End.Date;
                    if (EventInstance.IsYearly && currentDate >= sDate && currentDate <= eDate)
                        events.Add(currentDate);
                    else if (!EventInstance.IsYearly && currentDate == nDate)
                        events.Add(nDate);
                }
            }

            // Render the calendar
            var calendars = new Calendars()
            {
                Year = Year,
                Month = Month,
                EventDates = [.. events],
                ReminderDates = [.. reminders],
                Culture = calendarInstance.Culture,
            };
            return calendars;
        }
    }
}
