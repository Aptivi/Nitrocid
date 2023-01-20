
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS.Misc.Calendar
{
    /// <summary>
    /// Calendar tools
    /// </summary>
    public static class CalendarTools
    {
        /// <summary>
        /// Alternative calendar
        /// </summary>
        public static CalendarTypes AltCalendar { get; set; } = CalendarTypes.Hijri;

        /// <summary>
        /// Gets the culture from the culture type
        /// </summary>
        /// <param name="calendar">Calendar type</param>
        public static CultureInfo GetCultureFromCalendar(CalendarTypes calendar)
        {
            switch (calendar)
            {
                case CalendarTypes.Gregorian:
                    return new CultureInfo("en-US");
                case CalendarTypes.Hijri:
                    var Cult = new CultureInfo("ar");
                    Cult.DateTimeFormat.Calendar = new HijriCalendar();
                    return Cult;
                case CalendarTypes.Persian:
                    return new CultureInfo("fa");
                case CalendarTypes.SaudiHijri:
                    return new CultureInfo("ar-SA");
                case CalendarTypes.ThaiBuddhist:
                    return new CultureInfo("th-TH");
                default:
                    return new CultureInfo("en-US");
            }
        }
    }
}
