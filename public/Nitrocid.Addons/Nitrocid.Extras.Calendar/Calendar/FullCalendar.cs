//
// Terminaux  Copyright (C) 2023-2025  Aptivi
//
// This file is part of Terminaux
//
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using StdCalendar = System.Globalization.Calendar;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using Nitrocid.Extras.Calendar.Calendar.Events;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer;

namespace Nitrocid.Extras.Calendar.Calendar
{
    /// <summary>
    /// Calendars renderable
    /// </summary>
    public class FullCalendar : IStaticRenderable
    {
        internal const int calendarWidth = 4 + (6 * 6);
        internal const int calendarHeight = 13;
        private int year = 0;
        private int month = 0;
        private CultureInfo culture = new("en-US");
        private Color headerColor = ColorTools.CurrentForegroundColor;
        private Color weekendColor = ColorTools.CurrentForegroundColor;
        private Color todayColor = ColorTools.CurrentForegroundColor;
        private Color foregroundColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private BorderSettings borderSettings = new();
        private bool useColors = true;

        /// <summary>
        /// Left position of the calendar
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Top position of the calendar
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Full calendar width (constant)
        /// </summary>
        public int Width =>
            calendarWidth;

        /// <summary>
        /// Full calendar height (constant)
        /// </summary>
        public int Height =>
            calendarHeight;

        /// <summary>
        /// Year of the calendar
        /// </summary>
        public int Year
        {
            get => year;
            set => year = value;
        }

        /// <summary>
        /// Month of the calendar
        /// </summary>
        public int Month
        {
            get => month;
            set => month = value;
        }

        /// <summary>
        /// Culture info that the calendar uses
        /// </summary>
        public CultureInfo Culture
        {
            get => culture;
            set => culture = value;
        }

        /// <summary>
        /// Calendar header color
        /// </summary>
        public Color HeaderColor
        {
            get => headerColor;
            set => headerColor = value;
        }

        /// <summary>
        /// Calendar day weekend indicator color
        /// </summary>
        public Color WeekendColor
        {
            get => weekendColor;
            set => weekendColor = value;
        }

        /// <summary>
        /// Current day color
        /// </summary>
        public Color TodayColor
        {
            get => todayColor;
            set => todayColor = value;
        }

        /// <summary>
        /// Foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Border settings to use
        /// </summary>
        public BorderSettings BorderSettings
        {
            get => borderSettings;
            set => borderSettings = value;
        }

        /// <summary>
        /// Calendar instance that is obtained from a culture
        /// </summary>
        public StdCalendar Calendar =>
            Culture.DateTimeFormat.Calendar;

        /// <summary>
        /// Whether to highlight the current day or to highlight a specified day
        /// </summary>
        public bool HighlightToday { get; set; } = true;

        /// <summary>
        /// Highlight a specified day
        /// </summary>
        public DateTime HighlightedDay { get; set; } = DateTime.Today;

        /// <summary>
        /// Renders a calendar
        /// </summary>
        /// <returns>Rendered calendar that will be used by the renderer</returns>
        public string Render()
        {
            var calendarRendered = new StringBuilder();
            var calendarDays = culture.DateTimeFormat.DayNames;
            var calendarMonths = culture.DateTimeFormat.MonthNames;
            var calendarWeek = culture.DateTimeFormat.FirstDayOfWeek;
            var maxDate = Calendar.GetDaysInMonth(year, month);
            var selectedDate = new DateTime(year, month, DateTime.Now.Day > maxDate ? 1 : DateTime.Now.Day);
            var (calYear, calMonth, _, _) = GetDateFromCalendar(selectedDate, culture);
            var DateTo = new DateTime(calYear, calMonth, Calendar.GetDaysInMonth(calYear, calMonth));
            int CurrentWeek = 1;
            string CalendarTitle = calendarMonths[calMonth - 1] + " " + calYear;

            // Re-arrange the days according to the first day of week
            Dictionary<DayOfWeek, int> mappedDays = [];
            int dayOfWeek = (int)calendarWeek;
            for (int i = 0; i < calendarDays.Length; i++)
            {
                var day = (DayOfWeek)dayOfWeek;
                mappedDays.Add(day, i);
                dayOfWeek++;
                if (dayOfWeek > 6)
                    dayOfWeek = 0;
            }

            // Write the calendar title in a box
            var boxForeground = ForegroundColor;
            var background = BackgroundColor;
            int boxLeft = Left;
            int boxTop = Top;
            int boxWidth = Width;
            int boxHeight = Height;
            var border = new Border()
            {
                Title = CalendarTitle,
                Left = boxLeft,
                Top = boxTop,
                InteriorWidth = boxWidth,
                InteriorHeight = boxHeight,
                Color = boxForeground,
                TextColor = boxForeground,
                TextSettings = new()
                {
                    Alignment = TextAlignment.Middle
                },
                BackgroundColor = background,
            };
            calendarRendered.Append(border.Render());

            // Make a calendar
            int dayPosX;
            int dayPosY = Top + 3;
            for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
            {
                // Populate some variables
                var CurrentDate = new DateTime(year, month, CurrentDay);
                if (CurrentDate.DayOfWeek == calendarWeek)
                {
                    CurrentWeek += 1;
                    dayPosY += 2;
                }
                int currentDay = mappedDays[CurrentDate.DayOfWeek] + 1;
                dayPosX = boxLeft + 1 + 6 * (currentDay - 1);
                string CurrentDayMark;

                // Some flags
                var dateHighlight = HighlightToday ? DateTime.Today : HighlightedDay;
                bool ReminderMarked = false;
                bool EventMarked = false;
                bool IsWeekend = currentDay > 5;
                bool IsToday = CurrentDate == dateHighlight;
                var foreground =
                    IsToday ? TodayColor :
                    IsWeekend ? WeekendColor :
                    ForegroundColor;

                // Know where and how to put the day number
                foreach (ReminderInfo Reminder in ReminderManager.Reminders)
                {
                    var rDate = Reminder.ReminderDate.Date;
                    var (rYear, rMonth, rDay, _) = GetDateFromCalendar(new DateTime(rDate.Year, rDate.Month, rDate.Day), culture);
                    rDate = new(rYear, rMonth, rDay);
                    if (rDate == CurrentDate & !ReminderMarked)
                        ReminderMarked = true;
                }
                foreach (EventInfo EventInstance in EventManager.CalendarEvents.Union(EventManager.baseEvents))
                {
                    EventInstance.UpdateEventInfo(new DateTime(DateTime.Today.Year, 1, 1));
                    var nDate = EventInstance.EventDate.Date;
                    var sDate = EventInstance.Start.Date;
                    var eDate = EventInstance.End.Date;
                    var (nYear, nMonth, nDay, _) = GetDateFromCalendar(new DateTime(nDate.Year, nDate.Month, nDate.Day), culture);
                    var (sYear, sMonth, sDay, _) = GetDateFromCalendar(new DateTime(sDate.Year, sDate.Month, sDate.Day), culture);
                    var (eYear, eMonth, eDay, _) = GetDateFromCalendar(new DateTime(eDate.Year, eDate.Month, eDate.Day), culture);
                    nDate = new(nYear, nMonth, nDay);
                    sDate = new(sYear, sMonth, sDay);
                    eDate = new(eYear, eMonth, eDay);
                    if (((EventInstance.IsYearly && CurrentDate >= sDate && CurrentDate <= eDate) ||
                         (!EventInstance.IsYearly && CurrentDate == nDate)) && !EventMarked)
                    {
                        foreground = IsToday ? KernelColorTools.GetColor(KernelColorType.TodayDay) : KernelColorTools.GetColor(KernelColorType.EventDay);
                        EventMarked = true;
                    }
                }
                string markStart = ReminderMarked && EventMarked ? "[" : ReminderMarked ? "(" : EventMarked ? "<" : " ";
                string markEnd = ReminderMarked && EventMarked ? "]" : ReminderMarked ? ")" : EventMarked ? ">" : " ";
                CurrentDayMark = $"{markStart}{CurrentDay}{markEnd}";
                if (useColors)
                {
                    calendarRendered.Append(
                        $"{ColorTools.RenderSetConsoleColor(foreground)}" +
                        $"{ColorTools.RenderSetConsoleColor(background, true)}"
                    );
                }
                calendarRendered.Append(
                    CsiSequences.GenerateCsiCursorPosition(dayPosX + 1, dayPosY + 1) +
                    CurrentDayMark
                );
            }

            // Make a single-letter day indicator
            int dayIndicatorPosX = boxLeft + 1;
            int dayIndicatorPosY = boxTop + 1;
            for (int i = 0; i < mappedDays.Count; i++)
            {
                string dayName = $"{calendarDays[(int)mappedDays.Keys.ElementAt(i)]}";
                char dayChar = char.ToUpper(dayName[0]);
                if (useColors)
                {
                    calendarRendered.Append(
                        $"{ColorTools.RenderSetConsoleColor(HeaderColor)}" +
                        $"{ColorTools.RenderSetConsoleColor(background, true)}"
                    );
                }
                calendarRendered.Append(
                    CsiSequences.GenerateCsiCursorPosition(dayIndicatorPosX + 6 * i + 2, dayIndicatorPosY + 1) +
                    dayChar
                );
            }

            // Finalize everything
            if (useColors)
            {
                calendarRendered.Append(
                    ColorTools.RenderRevertForeground() +
                    ColorTools.RenderRevertBackground()
                );
            }
            return calendarRendered.ToString();
        }

        private static (int year, int month, int day, StdCalendar calendarInfo) GetDateFromCalendar(DateTime dt, CultureInfo culture)
        {
            var calendarInfo = culture.DateTimeFormat.Calendar;
            int year = calendarInfo.GetYear(dt);
            int month = calendarInfo.GetMonth(dt);
            int day = calendarInfo.GetDayOfMonth(dt);
            return (year, month, day, calendarInfo);
        }

        /// <summary>
        /// Makes a new instance of the calendar renderer
        /// </summary>
        public FullCalendar()
        { }
    }
}
