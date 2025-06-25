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
using System.Collections.Generic;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using Nitrocid.Extras.Calendar.Calendar.Events;
using Terminaux.Writer.CyclicWriters.Graphical.Calendaring;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Time.Converters;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;
using Terminaux.Writer.CyclicWriters.Graphical;

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
            PrintCalendar(DateTime.Today.Year, DateTime.Today.Month);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar(CalendarTypes calendar) =>
            PrintCalendar(DateTime.Today.Year, DateTime.Today.Month, calendar);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar(int Year, int Month, CalendarTypes calendar = CalendarTypes.Variant)
        {
            var calendarInstance = CalendarTools.GetCalendar(calendar);
            var CalendarDays = calendarInstance.Culture.DateTimeFormat.DayNames;
            var CalendarMonths = calendarInstance.Culture.DateTimeFormat.MonthNames;
            var CalendarWeek = calendarInstance.Culture.DateTimeFormat.FirstDayOfWeek;
            var CalendarData = new string[7, CalendarDays.Length];
            var maxDate = calendarInstance.Calendar.GetDaysInMonth(Year, Month);
            var selectedDate = new DateTime(Year, Month, TimeDateTools.KernelDateTime.Day > maxDate ? 1 : TimeDateTools.KernelDateTime.Day);
            var (year, month, _, _) = TimeDateConverters.GetDateFromCalendar(selectedDate, calendar);
            var DateTo = new DateTime(year, month, calendarInstance.Calendar.GetDaysInMonth(year, month));
            int CurrentWeek = 1;
            string CalendarTitle = CalendarMonths[month - 1] + " " + year;
            var CalendarCellOptions = new List<CellOptions>();

            // Re-arrange the days according to the first day of week
            Dictionary<DayOfWeek, int> mappedDays = [];
            int dayOfWeek = (int)CalendarWeek;
            for (int i = 0; i < CalendarDays.Length; i++)
            {
                var day = (DayOfWeek)dayOfWeek;
                mappedDays.Add(day, i);
                dayOfWeek++;
                if (dayOfWeek > 6)
                    dayOfWeek = 0;
                CalendarData[0, i] = $"{day}";
            }

            // Populate the calendar data
            ThemeColorsTools.LoadBackground();
            TextWriterWhereColor.WriteWhere(CalendarTitle, (int)Math.Round((ConsoleWrapper.WindowWidth - CalendarTitle.Length) / 2d), 1, true, ThemeColorType.TableTitle);
            for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
            {
                var CurrentDate = new DateTime(year, month, CurrentDay);
                if (CurrentDate.DayOfWeek == CalendarWeek)
                    CurrentWeek += 1;
                int CurrentWeekIndex = CurrentWeek - 1;
                int currentDay = mappedDays[CurrentDate.DayOfWeek] + 1;
                string CurrentDayMark;
                bool ReminderMarked = false;
                bool EventMarked = false;
                bool IsWeekend = currentDay > 5;
                bool IsToday = CurrentDate == TimeDateTools.KernelDateTime.Date;

                // Dim out the weekends
                if (IsWeekend)
                {
                    var WeekendOptions = new CellOptions(currentDay, CurrentWeek)
                    {
                        ColoredCell = true,
                        CellColor = ThemeColorsTools.GetColor(ThemeColorType.WeekendDay),
                        CellBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background)
                    };
                    CalendarCellOptions.Add(WeekendOptions);
                }

                // Highlight today
                if (IsToday)
                {
                    var TodayOptions = new CellOptions(currentDay, CurrentWeek)
                    {
                        ColoredCell = true,
                        CellColor = ThemeColorsTools.GetColor(ThemeColorType.TodayDay),
                        CellBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background)
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
                        ReminderMarked = true;
                }
                foreach (EventInfo EventInstance in EventManager.CalendarEvents.Union(EventManager.baseEvents))
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
                        var EventCell = new CellOptions(currentDay, CurrentWeek)
                        {
                            ColoredCell = true,
                            CellColor = ThemeColorsTools.GetColor(ThemeColorType.EventDay),
                            CellBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background)
                        };
                        CalendarCellOptions.Add(EventCell);
                        EventMarked = true;
                    }
                }
                string markStart = ReminderMarked && EventMarked ? "[" : ReminderMarked ? "(" : EventMarked ? "<" : " ";
                string markEnd = ReminderMarked && EventMarked ? "]" : ReminderMarked ? ")" : EventMarked ? ">" : " ";
                CurrentDayMark = $"{markStart}{CurrentDay}{markEnd}";
                CalendarData[CurrentWeekIndex + 1, currentDay - 1] = CurrentDayMark;
            }

            var calendarTable = new Table()
            {
                Rows = CalendarData,
                Left = 2,
                Top = 3,
                Width = ConsoleWrapper.WindowWidth - 4,
                Height = ConsoleWrapper.WindowHeight - 4,
                Header = true,
                Settings = CalendarCellOptions,
            };
            TextWriterRaw.WriteRaw(calendarTable.Render());
        }

    }
}
