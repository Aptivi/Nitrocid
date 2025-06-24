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

using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Time.Calendars;
using Nitrocid.Base.Kernel.Time.Timezones;
using System;
using System.Globalization;

namespace Nitrocid.Base.Kernel.Time.Converters
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
        /// Converts the date to Unix time (milliseconds since 1970/1/1) (UTC)
        /// </summary>
        public static double DateToUnixMs(DateTime TargetDate) =>
            (TargetDate - UnixEpoch).TotalMilliseconds;

        /// <summary>
        /// Converts the Unix time (milliseconds since 1970/1/1) to date (UTC)
        /// </summary>
        public static DateTime UnixToDateMs(double UnixTime) =>
            UnixEpoch.AddMilliseconds(UnixTime);

        /// <summary>
        /// Converts the date to Unix time (microseconds since 1970/1/1) (UTC)
        /// </summary>
        public static double DateToUnixUs(DateTime TargetDate) =>
            (TargetDate - UnixEpoch).TotalMicroseconds;

        /// <summary>
        /// Converts the Unix time (microseconds since 1970/1/1) to date (UTC)
        /// </summary>
        public static DateTime UnixToDateUs(double UnixTime) =>
            UnixEpoch.AddMicroseconds(UnixTime);

        /// <summary>
        /// Gets a date from a specified calendar
        /// </summary>
        /// <param name="calendarType">Calendar to use during conversion</param>
        /// <returns>Resulting year, month, day, and a calendar information instance</returns>
        public static (int year, int month, int day, Calendar calendarInfo) GetDateFromCalendar(string calendarType) =>
            GetDateFromCalendar(TimeDateTools.KernelDateTime, CalendarTools.GetCalendarTypeFromName(calendarType));

        /// <summary>
        /// Gets a date from a specified calendar
        /// </summary>
        /// <param name="dt">Date and time</param>
        /// <param name="calendarType">Calendar to use during conversion</param>
        /// <returns>Resulting year, month, day, and a calendar information instance</returns>
        public static (int year, int month, int day, Calendar calendarInfo) GetDateFromCalendar(DateTime dt, string calendarType) =>
            GetDateFromCalendar(dt, CalendarTools.GetCalendarTypeFromName(calendarType));

        /// <summary>
        /// Gets a date from a specified calendar without accessing the culture information, depending on the implementation of the calendar
        /// </summary>
        /// <param name="calendarType">Calendar to use during conversion</param>
        /// <returns>Resulting year, month, day, and a calendar information instance</returns>
        /// <remarks>
        /// Nitrocid calendars and all calendars that don't override the base <see cref="ICalendar.Calendar"/> usually access the
        /// culture information to get the calendar. You can override the <see cref="ICalendar.Calendar"/> property so that it
        /// doesn't access the culture information to make this function behave differently from the <see cref="GetDateFromCalendar(CalendarTypes)"/>
        /// and the <see cref="GetDateFromCalendar(DateTime, CalendarTypes)"/> functions that access the culture information.
        /// </remarks>
        public static (int year, int month, int day, Calendar calendarInfo) GetDateFromCalendarNoCulture(string calendarType) =>
            GetDateFromCalendarNoCulture(TimeDateTools.KernelDateTime, CalendarTools.GetCalendarTypeFromName(calendarType));

        /// <summary>
        /// Gets a date from a specified calendar without accessing the culture information, depending on the implementation of the calendar
        /// </summary>
        /// <param name="dt">Date and time</param>
        /// <param name="calendarType">Calendar to use during conversion</param>
        /// <returns>Resulting year, month, day, and a calendar information instance</returns>
        /// <remarks>
        /// Nitrocid calendars and all calendars that don't override the base <see cref="ICalendar.Calendar"/> usually access the
        /// culture information to get the calendar. You can override the <see cref="ICalendar.Calendar"/> property so that it
        /// doesn't access the culture information to make this function behave differently from the <see cref="GetDateFromCalendar(CalendarTypes)"/>
        /// and the <see cref="GetDateFromCalendar(DateTime, CalendarTypes)"/> functions that access the culture information.
        /// </remarks>
        public static (int year, int month, int day, Calendar calendarInfo) GetDateFromCalendarNoCulture(DateTime dt, string calendarType) =>
            GetDateFromCalendarNoCulture(dt, CalendarTools.GetCalendarTypeFromName(calendarType));

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

        /// <summary>
        /// Gets a date from a specified calendar without accessing the culture information, depending on the implementation of the calendar
        /// </summary>
        /// <param name="calendar">Calendar to use during conversion</param>
        /// <returns>Resulting year, month, day, and a calendar information instance</returns>
        /// <remarks>
        /// Nitrocid calendars and all calendars that don't override the base <see cref="ICalendar.Calendar"/> usually access the
        /// culture information to get the calendar. You can override the <see cref="ICalendar.Calendar"/> property so that it
        /// doesn't access the culture information to make this function behave differently from the <see cref="GetDateFromCalendar(CalendarTypes)"/>
        /// and the <see cref="GetDateFromCalendar(DateTime, CalendarTypes)"/> functions that access the culture information.
        /// </remarks>
        public static (int year, int month, int day, Calendar calendarInfo) GetDateFromCalendarNoCulture(CalendarTypes calendar) =>
            GetDateFromCalendarNoCulture(TimeDateTools.KernelDateTime, calendar);

        /// <summary>
        /// Gets a date from a specified calendar without accessing the culture information, depending on the implementation of the calendar
        /// </summary>
        /// <param name="dt">Date and time</param>
        /// <param name="calendar">Calendar to use during conversion</param>
        /// <returns>Resulting year, month, day, and a calendar information instance</returns>
        /// <remarks>
        /// Nitrocid calendars and all calendars that don't override the base <see cref="ICalendar.Calendar"/> usually access the
        /// culture information to get the calendar. You can override the <see cref="ICalendar.Calendar"/> property so that it
        /// doesn't access the culture information to make this function behave differently from the <see cref="GetDateFromCalendar(CalendarTypes)"/>
        /// and the <see cref="GetDateFromCalendar(DateTime, CalendarTypes)"/> functions that access the culture information.
        /// </remarks>
        public static (int year, int month, int day, Calendar calendarInfo) GetDateFromCalendarNoCulture(DateTime dt, CalendarTypes calendar)
        {
            var calendarInstance = CalendarTools.GetCalendar(calendar);
            var calendarInfo = calendarInstance.Calendar;
            int year = calendarInfo.GetYear(dt);
            int month = calendarInfo.GetMonth(dt);
            int day = calendarInfo.GetDayOfMonth(dt);
            return (year, month, day, calendarInfo);
        }

        /// <summary>
        /// Gets the date and the time from the specified timezone
        /// </summary>
        /// <param name="zone">Time zone ID</param>
        public static DateTime GetDateTimeFromZone(string zone) =>
            GetDateTimeFromZone(TimeDateTools.KernelDateTime, TimeZones.GetZoneInfo(zone));

        /// <summary>
        /// Gets the date and the time from the specified timezone
        /// </summary>
        /// <param name="zone">Time zone info instance</param>
        public static DateTime GetDateTimeFromZone(TimeZoneInfo zone) =>
            GetDateTimeFromZone(TimeDateTools.KernelDateTime, zone);

        /// <summary>
        /// Gets the date and the time from the specified timezone
        /// </summary>
        /// <param name="date">Specified date</param>
        /// <param name="zone">Time zone ID</param>
        public static DateTime GetDateTimeFromZone(DateTime date, string zone) =>
            GetDateTimeFromZone(date, TimeZones.GetZoneInfo(zone));

        /// <summary>
        /// Gets the date and the time from the specified timezone
        /// </summary>
        /// <param name="date">Specified date</param>
        /// <param name="zone">Time zone info instance</param>
        public static DateTime GetDateTimeFromZone(DateTime date, TimeZoneInfo zone) =>
            TimeZoneInfo.ConvertTime(date, zone);
    }
}
