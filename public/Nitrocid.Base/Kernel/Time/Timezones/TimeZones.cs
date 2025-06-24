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

using System;
using System.Collections.Generic;
using System.Linq;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Time.Converters;

namespace Nitrocid.Base.Kernel.Time.Timezones
{
    /// <summary>
    /// Time zone module
    /// </summary>
    public static class TimeZones
    {
        internal static bool useSystemTimezone = true;
        internal static string defaultZoneName = TimeZoneInfo.Local.Id;
        private static string[]? recognizedZones;

        /// <summary>
        /// Populates names of all the time zones (IANA on Unix).
        /// </summary>
        public static string[] GetTimeZoneNames()
        {
            CheckZoneInfoDirectory();

            // Get the cached zones
            if (recognizedZones is not null)
                return recognizedZones;

            // Get all system time zones (IANA on Unix)
            var Zones = TimeZoneInfo.GetSystemTimeZones().ToArray();
            var ZoneTimes = new List<string>();
            //DebugWriter.WriteDebug(DebugLevel.I, "Found {0} time zones.", vars: [Zones.Length]);

            // Adds date and time to every single time zone to the list
            foreach (var Zone in Zones)
                ZoneTimes.Add(Zone.Id);
            //DebugWriter.WriteDebug(DebugLevel.I, "ZoneTimes = {0}", vars: [ZoneTimes.Count]);

            // Return the populated array
            recognizedZones = [.. ZoneTimes];
            return recognizedZones;
        }

        /// <summary>
        /// Populates current time in all of the time zones (IANA on Unix).
        /// </summary>
        public static Dictionary<string, DateTime> GetTimeZoneTimes()
        {
            CheckZoneInfoDirectory();

            // Get all system time zones (IANA on Unix)
            var Zones = GetTimeZoneNames();
            var ZoneTimes = new Dictionary<string, DateTime>();
            DebugWriter.WriteDebug(DebugLevel.I, "Found {0} time zones.", vars: [Zones.Length]);

            // Adds date and time to every single time zone to the list
            foreach (var Zone in Zones)
            {
                var time = TimeDateConverters.GetDateTimeFromZone(TimeDateTools.KernelDateTime, Zone);
                ZoneTimes.Add(Zone, time);
            }
            DebugWriter.WriteDebug(DebugLevel.I, "ZoneTimes = {0}", vars: [ZoneTimes.Count]);

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
            CheckZoneInfoDirectory();
            var ZoneTimes = GetTimeZoneNames();
            bool ZoneFound = ZoneTimes.Contains(zone);
            return ZoneFound;
        }

        /// <summary>
        /// Gets the time zone info from the name
        /// </summary>
        /// <param name="zone">Zone name (usually the zone ID)</param>
        /// <returns>An instance of <see cref="TimeZoneInfo"/></returns>
        /// <exception cref="KernelException"></exception>
        public static TimeZoneInfo GetZoneInfo(string zone)
        {
            CheckZoneInfoDirectory();
            if (TimeZoneExists(zone))
                return TimeZoneInfo.FindSystemTimeZoneById(zone);
            throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_TIMEZONES_EXCEPTION_NOTFOUND2") + $" {zone}");
        }

        /// <summary>
        /// Gets the current time zone info from the name
        /// </summary>
        /// <returns>An instance of <see cref="TimeZoneInfo"/></returns>
        /// <exception cref="KernelException"></exception>
        public static TimeZoneInfo GetCurrentZoneInfo()
        {
            CheckZoneInfoDirectory();
            if (useSystemTimezone)
                return TimeZoneInfo.Local;
            if (TimeZoneExists(defaultZoneName))
                return TimeZoneInfo.FindSystemTimeZoneById(defaultZoneName);
            throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_TIMEZONES_EXCEPTION_NOTFOUND2") + $" {defaultZoneName}");
        }

        internal static void CheckZoneInfoDirectory()
        {
            // Check to see if tzdata is installed (only on Unix)
            if (KernelPlatform.IsOnUnix() && !FilesystemTools.FolderExists("/usr/share/zoneinfo"))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "System is on Unix but /usr/share/zoneinfo is not installed!");
                throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_TIMEZONES_EXCEPTION_NEEDSTZDATA"));
            }
        }
    }
}
