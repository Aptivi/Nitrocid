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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.Calendar.Calendar.Events;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Calendars;
using Nitrocid.Kernel.Time.Converters;
using Terminaux.Base;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Nitrocid.Extras.Calendar.Calendar
{
    /// <summary>
    /// Calendar printing module
    /// </summary>
    public static class CalendarPrint
    {
        internal const int calendarWidth = 5 + (6 * 6);
        internal const int calendarHeight = 7;

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar() =>
            PrintCalendar(TimeDateTools.KernelDateTime.Year, TimeDateTools.KernelDateTime.Month, CalendarTypes.Variant);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        /// <param name="calendar">Calendar type</param>
        public static void PrintCalendar(CalendarTypes calendar) =>
            PrintCalendar(TimeDateTools.KernelDateTime.Year, TimeDateTools.KernelDateTime.Month, calendar);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        /// <param name="calendar">Calendar instance</param>
        public static void PrintCalendar(BaseCalendar calendar) =>
            PrintCalendar(TimeDateTools.KernelDateTime.Year, TimeDateTools.KernelDateTime.Month, calendar);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        /// <param name="Year">Year to use</param>
        /// <param name="Month">Month to use</param>
        /// <param name="calendar">Calendar type</param>
        public static void PrintCalendar(int Year, int Month, CalendarTypes calendar)
        {
            // Render the calendar
            var calendarInstance = CalendarTools.GetCalendar(calendar);
            PrintCalendar(Year, Month, calendarInstance);
        }

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        /// <param name="Year">Year to use</param>
        /// <param name="Month">Month to use</param>
        /// <param name="calendar">Calendar instance</param>
        public static void PrintCalendar(int Year, int Month, BaseCalendar calendar)
        {
            // Render the calendar
            var calendars = RenderCalendar(Year, Month, calendar);
            TextWriterRaw.WritePlain(calendars.Render());
        }

        internal static Calendars RenderCalendar() =>
            RenderCalendar(TimeDateTools.KernelDateTime.Year, TimeDateTools.KernelDateTime.Month, CalendarTypes.Variant);

        internal static Calendars RenderCalendar(CalendarTypes calendar) =>
            RenderCalendar(TimeDateTools.KernelDateTime.Year, TimeDateTools.KernelDateTime.Month, calendar);

        internal static Calendars RenderCalendar(BaseCalendar calendar) =>
            RenderCalendar(TimeDateTools.KernelDateTime.Year, TimeDateTools.KernelDateTime.Month, calendar);

        internal static Calendars RenderCalendar(int Year, int Month, CalendarTypes calendar)
        {
            var calendarInstance = CalendarTools.GetCalendar(calendar);
            return RenderCalendar(Year, Month, calendarInstance);
        }

        internal static Calendars RenderCalendar(int Year, int Month, BaseCalendar calendar)
        {
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
                Culture = calendar.Culture,
            };
            return calendars;
        }
    }
}
