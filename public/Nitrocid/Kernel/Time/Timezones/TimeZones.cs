
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
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Files.Operations.Querying;

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
            var Zones = TimeZoneInfo.GetSystemTimeZones().ToArray();
            var ZoneTimes = new Dictionary<string, DateTime>();
            DebugWriter.WriteDebug(DebugLevel.I, "Found {0} time zones.", Zones.Length);

            // Run a cleanup in the list
            ZoneTimes.Clear();
            DebugWriter.WriteDebug(DebugLevel.I, "Cleaned up zoneTimes.");

            // Adds date and time to every single time zone to the list
            foreach (var Zone in Zones)
                ZoneTimes.Add(Zone.Id, TimeZoneInfo.ConvertTime(TimeDateTools.KernelDateTime, TimeZoneInfo.FindSystemTimeZoneById(Zone.Id)));
            DebugWriter.WriteDebug(DebugLevel.I, "ZoneTimes = {0}", ZoneTimes.Count);

            // Return the populated array
            CheckZoneInfoDirectory();
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

        internal static void CheckZoneInfoDirectory()
        {
            // Check to see if tzdata is installed (only on Unix)
            if (KernelPlatform.IsOnUnix() && !Checking.FolderExists("/usr/share/zoneinfo"))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "System is on Unix but /usr/share/zoneinfo is not installed!");
                throw new KernelException(KernelExceptionType.TimeDate, Translate.DoTranslation("The time zone information package is not installed yet on your Linux system. Install 'tzdata' using your distribution's package manager."));
            }
        }
    }
}
