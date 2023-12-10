//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.Kernel.Configuration;
using KS.Kernel.Time.Converters;
using KS.Kernel.Time.Timezones;
using System;

namespace KS.Kernel.Time
{
    /// <summary>
    /// Time and date module
    /// </summary>
    public static class TimeDateTools
    {

        /// <summary>
        /// The kernel time and date
        /// </summary>
        public static DateTime KernelDateTime =>
            TimeZones.useSystemTimezone ? DateTime.Now : TimeDateConverters.GetDateTimeFromZone(DateTime.Now, TimeZones.defaultZoneName);

        /// <summary>
        /// The kernel time and date (UTC)
        /// </summary>
        public static DateTime KernelDateTimeUtc =>
            DateTime.UtcNow;

        /// <summary>
        /// Whether to show the current time before login
        /// </summary>
        public static bool ShowCurrentTimeBeforeLogin =>
            Config.MainConfig.ShowCurrentTimeBeforeLogin;

        /// <summary>
        /// Show Time/Date on corner
        /// </summary>
        public static bool CornerTimeDate =>
            Config.MainConfig.CornerTimeDate;

        /// <summary>
        /// Whether or not to render time and dates short or long
        /// </summary>
        public static bool LongTimeDate =>
            Config.MainConfig.LongTimeDate;

        /// <summary>
        /// Gets the remaining time from now
        /// </summary>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <returns>A <see cref="TimeSpan"/> instance indicating remaining time from now</returns>
        public static TimeSpan GetRemainingTimeFromNow(int Milliseconds) =>
            GetRemainingTimeFrom(KernelDateTime, Milliseconds);

        /// <summary>
        /// Gets the remaining time from the selected date and time
        /// </summary>
        /// <param name="moment">A moment in time in which will be compared into how many milliseconds remaining</param>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <returns>A <see cref="TimeSpan"/> instance indicating remaining time from now</returns>
        public static TimeSpan GetRemainingTimeFrom(DateTime moment, int Milliseconds)
        {
            var RemainingTime = moment.AddMilliseconds(Milliseconds) - moment;
            return RemainingTime;
        }

    }
}
