//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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
using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.ConsoleBase.Writers.FancyWriters.Tools;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using Nitrocid.Extras.Calendar.Calendar.Events;
using KS.ConsoleBase;
using KS.Kernel.Time.Calendars;
using KS.Kernel.Time.Converters;
using KS.Kernel.Time;

namespace Nitrocid.Extras.Calendar.Calendar
{
    /// <summary>
    /// Calendar printing module
    /// </summary>
    public static class CalendarPrint
    {

        private static readonly EventInfo[] baseEvents = new EventInfo[]
        {
            new(new(2018, 2, 22), /* Localizable */ "Nitrocid KS Release Anniversary", true, 2, 22, 2, 22, "Gregorian"),
            new(new(2018, 2, 22), /* Localizable */ "Ramadan", true, 9, 1, 10, 1, "Hijri"),
        };

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar() =>
            PrintCalendar(DateTime.Today.Year, DateTime.Today.Month);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar(CalendarTypes calendar) =>
            PrintCalendar(DateTime.Today.Year, DateTime.Today.Month, calendar);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar(int Year, int Month, CalendarTypes calendar = CalendarTypes.Gregorian)
        {
            var calendarInstance = CalendarTools.GetCalendar(calendar);
            var CalendarDays = calendarInstance.Culture.DateTimeFormat.DayNames;
            var CalendarMonths = calendarInstance.Culture.DateTimeFormat.MonthNames;
            var CalendarData = new string[6, CalendarDays.Length];
            var maxDate = calendarInstance.Calendar.GetDaysInMonth(Year, Month);
            var selectedDate = new DateTime(Year, Month, TimeDateTools.KernelDateTime.Day > maxDate ? 1 : TimeDateTools.KernelDateTime.Day);
            var (year, month, day, _) = TimeDateConverters.GetDateFromCalendar(selectedDate, calendar);
            var DateTo = new DateTime(year, month, calendarInstance.Calendar.GetDaysInMonth(year, month));
            int CurrentWeek = 1;
            string CalendarTitle = CalendarMonths[month - 1] + " " + year;
            var CalendarCellOptions = new List<CellOptions>();

            // Populate the calendar data
            TextWriterWhereColor.WriteWhereKernelColor(CalendarTitle, (int)Math.Round((ConsoleWrapper.WindowWidth - CalendarTitle.Length) / 2d), ConsoleWrapper.CursorTop, true, KernelColorType.TableTitle);
            TextWriterColor.Write();
            for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
            {
                var CurrentDate = new DateTime(year, month, CurrentDay);
                if (CurrentDate.DayOfWeek == 0)
                    CurrentWeek += 1;
                int CurrentWeekIndex = CurrentWeek - 1;
                string CurrentDayMark = $" {CurrentDay} ";
                bool ReminderMarked = false;
                bool EventMarked = false;
                bool IsWeekend = CurrentDate.DayOfWeek == DayOfWeek.Friday | CurrentDate.DayOfWeek == DayOfWeek.Saturday;
                bool IsToday = CurrentDate == TimeDateTools.KernelDateTime.Date;

                // Dim out the weekends
                if (IsWeekend)
                {
                    var WeekendOptions = new CellOptions((int)CurrentDate.DayOfWeek + 1, CurrentWeek)
                    {
                        ColoredCell = true,
                        CellColor = KernelColorTools.GetColor(KernelColorType.WeekendDay),
                        CellBackgroundColor = KernelColorTools.GetColor(KernelColorType.Background)
                    };
                    CalendarCellOptions.Add(WeekendOptions);
                }

                // Highlight today
                if (IsToday)
                {
                    var TodayOptions = new CellOptions((int)CurrentDate.DayOfWeek + 1, CurrentWeek)
                    {
                        ColoredCell = true,
                        CellColor = KernelColorTools.GetColor(KernelColorType.TodayDay),
                        CellBackgroundColor = KernelColorTools.GetColor(KernelColorType.Background)
                    };
                    CalendarCellOptions.Add(TodayOptions);
                }

                // Know where and how to put the day number
                foreach (ReminderInfo Reminder in ReminderManager.Reminders)
                {
                    var rDate = Reminder.ReminderDate.Date;
                    var (rYear, rMonth, rDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(rDate.Year, rDate.Month, rDate.Day), calendar);
                    rDate = new(rYear, rMonth, rDay);
                    if (rDate == CurrentDate & !ReminderMarked)
                    {
                        CurrentDayMark = $"({CurrentDay})";
                        ReminderMarked = true;
                    }
                }
                foreach (EventInfo EventInstance in EventManager.CalendarEvents.Union(baseEvents))
                {
                    EventInstance.UpdateEventInfo(new DateTime(Year, 1, 1));
                    var nDate = EventInstance.EventDate.Date;
                    var sDate = EventInstance.Start.Date;
                    var eDate = EventInstance.End.Date;
                    var (nYear, nMonth, nDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(nDate.Year, nDate.Month, nDate.Day), calendar);
                    var (sYear, sMonth, sDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(sDate.Year, sDate.Month, sDate.Day), calendar);
                    var (eYear, eMonth, eDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(eDate.Year, eDate.Month, eDate.Day), calendar);
                    nDate = new(nYear, nMonth, nDay);
                    sDate = new(sYear, sMonth, sDay);
                    eDate = new(eYear, eMonth, eDay);
                    if (((EventInstance.IsYearly && CurrentDate >= sDate && CurrentDate <= eDate) ||
                         (!EventInstance.IsYearly && CurrentDate == nDate)) && !EventMarked)
                    {
                        var EventCell = new CellOptions((int)CurrentDate.DayOfWeek + 1, CurrentWeek)
                        {
                            ColoredCell = true,
                            CellColor = KernelColorTools.GetColor(KernelColorType.EventDay),
                            CellBackgroundColor = KernelColorTools.GetColor(KernelColorType.Background)
                        };
                        CalendarCellOptions.Add(EventCell);
                        EventMarked = true;
                    }
                }
                CalendarData[CurrentWeekIndex, (int)CurrentDate.DayOfWeek] = CurrentDayMark;
            }
            TableColor.WriteTable(CalendarDays, CalendarData, 2, true, CalendarCellOptions);
        }

    }
}
