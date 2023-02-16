
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using KS.Languages;
using KS.Misc.Calendar.Events;
using KS.Misc.Calendar.Reminders;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.ConsoleBase.Colors;

namespace KS.Misc.Calendar
{
    /// <summary>
    /// Calendar printing module
    /// </summary>
    public static class CalendarPrint
    {

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar() => PrintCalendar(DateTime.Today.Year, DateTime.Today.Month);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar(int Year, int Month)
        {
            var CalendarDays = CultureManager.CurrentCult.DateTimeFormat.DayNames;
            var CalendarMonths = CultureManager.CurrentCult.DateTimeFormat.MonthNames;
            var CalendarData = new string[6, CalendarDays.Length];
            var DateTo = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), CultureManager.CurrentCult.Calendar);
            int CurrentWeek = 1;
            string CalendarTitle = CalendarMonths[Month - 1] + " " + Year;
            var CalendarCellOptions = new List<CellOptions>();

            // Populate the calendar data
            TextWriterWhereColor.WriteWhere(CalendarTitle, (int)Math.Round((ConsoleBase.ConsoleWrapper.WindowWidth - CalendarTitle.Length) / 2d), ConsoleBase.ConsoleWrapper.CursorTop, true, KernelColorType.TableTitle);
            TextWriterColor.Write();
            for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
            {
                var CurrentDate = new DateTime(Year, Month, CurrentDay, CultureManager.CurrentCult.DateTimeFormat.Calendar);
                if (CurrentDate.DayOfWeek == 0)
                    CurrentWeek += 1;
                int CurrentWeekIndex = CurrentWeek - 1;
                string CurrentDayMark = $" {CurrentDay} ";
                bool ReminderMarked = false;
                bool EventMarked = false;
                bool IsWeekend = CurrentDate.DayOfWeek == DayOfWeek.Friday | CurrentDate.DayOfWeek == DayOfWeek.Saturday;
                bool IsToday = CurrentDate == DateTime.Today;

                // Dim out the weekends
                if (IsWeekend)
                {
                    var WeekendOptions = new CellOptions((int)CurrentDate.DayOfWeek + 1, CurrentWeek)
                    {
                        ColoredCell = true,
                        CellColor = ColorTools.GetColor(KernelColorType.WeekendDay),
                        CellBackgroundColor = ColorTools.GetColor(KernelColorType.Background)
                    };
                    CalendarCellOptions.Add(WeekendOptions);
                }

                // Highlight today
                if (IsToday)
                {
                    var TodayOptions = new CellOptions((int)CurrentDate.DayOfWeek + 1, CurrentWeek)
                    {
                        ColoredCell = true,
                        CellColor = ColorTools.GetColor(KernelColorType.TodayDay),
                        CellBackgroundColor = ColorTools.GetColor(KernelColorType.Background)
                    };
                    CalendarCellOptions.Add(TodayOptions);
                }

                // Know where and how to put the day number
                foreach (ReminderInfo Reminder in ReminderManager.Reminders)
                {
                    if (Reminder.ReminderDate.Date == CurrentDate & !ReminderMarked)
                    {
                        CurrentDayMark = $"({CurrentDay})";
                        ReminderMarked = true;
                    }
                }
                foreach (EventInfo EventInstance in EventManager.CalendarEvents)
                {
                    if (EventInstance.EventDate == CurrentDate & !EventMarked)
                    {
                        var EventCell = new CellOptions((int)CurrentDate.DayOfWeek + 1, CurrentWeek)
                        {
                            ColoredCell = true,
                            CellColor = ColorTools.GetColor(KernelColorType.EventDay),
                            CellBackgroundColor = ColorTools.GetColor(KernelColorType.Background)
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
