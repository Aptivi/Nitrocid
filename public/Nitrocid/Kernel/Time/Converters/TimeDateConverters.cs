
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

using KS.Kernel.Time.Calendars;
using System;
using System.Globalization;

namespace KS.Kernel.Time.Converters
{
    /// <summary>
    /// Date and time conversion module
    /// </summary>
    public static class TimeDateConverters
    {

        /// <summary>
        /// Unix epoch (1970/1/1)
        /// </summary>
        public readonly static DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// Converts the date to Unix time (seconds since 1970/1/1) (UTC)
        /// </summary>
        public static double DateToUnix(DateTime TargetDate) =>
            (TargetDate - UnixEpoch).TotalSeconds;

        /// <summary>
        /// Converts the Unix time (seconds since 1970/1/1) to date (UTC)
        /// </summary>
        public static DateTime UnixToDate(double UnixTime) =>
            UnixEpoch.AddSeconds(UnixTime);

        /// <summary>
        /// Gets a date from a specified calendar
        /// </summary>
        /// <param name="calendar">Calendar to use during conversion</param>
        /// <returns>Resulting year, month, day, and a calendar information instance</returns>
        public static (int year, int month, int day, Calendar calendarInfo) GetDateFromCalendar(CalendarTypes calendar) =>
            GetDateFromCalendar(TimeDateTools.KernelDateTime, calendar);

        /// <summary>
        /// Gets a date from a specified calendar
        /// </summary>
        /// <param name="dt">Date and time</param>
        /// <param name="calendar">Calendar to use during conversion</param>
        /// <returns>Resulting year, month, day, and a calendar information instance</returns>
        public static (int year, int month, int day, Calendar calendarInfo) GetDateFromCalendar(DateTime dt, CalendarTypes calendar)
        {
            var calendarInstance = CalendarTools.GetCalendar(calendar);
            var calendarInfo = calendarInstance.Culture.DateTimeFormat.Calendar;
            int year = calendarInfo.GetYear(dt);
            int month = calendarInfo.GetMonth(dt);
            int day = calendarInfo.GetDayOfMonth(dt);
            return (year, month, day, calendarInfo);
        }

    }
}
