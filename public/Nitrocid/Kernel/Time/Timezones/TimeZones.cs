
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
using System.Linq;
using static System.TimeZoneInfo;
using KS.Kernel.Debugging;

namespace KS.Kernel.Time.Timezones
{
    /// <summary>
    /// Time zone module
    /// </summary>
    public static class TimeZones
    {
        /// <summary>
        /// Populates current time in all of the time zones (IANA on Unix).
        /// </summary>
        public static Dictionary<string, DateTime> GetTimeZones()
        {
            // Get all system time zones (IANA on Unix)
            var Zones = GetSystemTimeZones().ToArray();
            var ZoneTimes = new Dictionary<string, DateTime>();
            DebugWriter.WriteDebug(DebugLevel.I, "Found {0} time zones.", Zones.Length);

            // Run a cleanup in the list
            ZoneTimes.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Cleaned up zoneTimes.");

            // Adds date and time to every single time zone to the list
            foreach (var Zone in Zones)
                ZoneTimes.Add(Zone.Id, ConvertTime(TimeDateTools.KernelDateTime, FindSystemTimeZoneById(Zone.Id)));

            // Return the populated array
            return ZoneTimes;
        }

        /// <summary>
        /// Checks to see if the specified time zone exists
        /// </summary>
        /// <param name="zone">Target time zone name</param>
        /// <returns>True if found; false otherwise</returns>
        public static bool TimeZoneExists(string zone)
        {
            var ZoneTimes = GetTimeZones();
            bool ZoneFound = ZoneTimes.ContainsKey(zone);
            return ZoneFound;
        }
    }
}
