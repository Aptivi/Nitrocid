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

using Nitrocid.Kernel.Time.Calendars.Types;
using System;
using System.Collections.Generic;

namespace Nitrocid.Kernel.Time.Calendars
{
    /// <summary>
    /// Calendar tools
    /// </summary>
    public static class CalendarTools
    {
        private static readonly Dictionary<CalendarTypes, BaseCalendar> calendars = new()
        {
            { CalendarTypes.Chinese, new ChineseCalendar() },
            { CalendarTypes.Gregorian, new GregorianCalendar() },
            { CalendarTypes.Hijri, new HijriCalendar() },
            { CalendarTypes.Japanese, new JapaneseCalendar() },
            { CalendarTypes.Persian, new PersianCalendar() },
            { CalendarTypes.SaudiHijri, new SaudiHijriCalendar() },
            { CalendarTypes.Taiwanese, new TaiwaneseCalendar() },
            { CalendarTypes.ThaiBuddhist, new ThaiBuddhistCalendar() },
            { CalendarTypes.Variant, new VariantCalendar() },
        };

        /// <summary>
        /// Gets a calendar
        /// </summary>
        /// <param name="typeName">Type name to look up</param>
        /// <returns>An instance of BaseCalendar if found. Otherwise, null.</returns>
        public static BaseCalendar GetCalendar(string typeName) =>
            GetCalendar(GetCalendarTypeFromName(typeName));

        /// <summary>
        /// Gets a calendar
        /// </summary>
        /// <param name="type">Calendar type to get a calendar from</param>
        /// <returns>An instance of BaseCalendar if found. Otherwise, null.</returns>
        public static BaseCalendar GetCalendar(CalendarTypes type) =>
            calendars[type];

        /// <summary>
        /// Gets a calendar type from the calendar type name
        /// </summary>
        /// <param name="typeName">Type name to look up</param>
        /// <returns>The resulting value of <see cref="CalendarTypes"/></returns>
        public static CalendarTypes GetCalendarTypeFromName(string typeName)
        {
            if (Enum.TryParse(typeName, out CalendarTypes result))
                return result;
            return CalendarTypes.Gregorian;
        }
    }
}
