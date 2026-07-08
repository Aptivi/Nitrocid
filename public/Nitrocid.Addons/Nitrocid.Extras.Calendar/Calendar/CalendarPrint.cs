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
using Calendrier;
using Nitrocid.Base.Kernel.Time;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;

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
        public static void PrintCalendar(CalendarTypes calendar) =>
            PrintCalendar(TimeDateTools.KernelDateTime.Year, TimeDateTools.KernelDateTime.Month, calendar);

        /// <summary>
        /// Prints the table of the calendar
        /// </summary>
        public static void PrintCalendar(int Year, int Month, CalendarTypes calendar = CalendarTypes.Variant)
        {
            var calendarInstance = CalendarTools.GetCalendar(calendar);
            var calendars = new Calendars()
            {
                Year = Year,
                Month = Month,
                TodayColor = ThemeColorsTools.GetColor(ThemeColorType.TodayDay),
                WeekendColor = ThemeColorsTools.GetColor(ThemeColorType.WeekendDay),
                Culture = calendarInstance.Culture,
            };
            TextWriterRaw.WritePlain(calendars.Render());
        }
    }
}
