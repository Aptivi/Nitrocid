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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using System;

namespace Nitrocid.Kernel.Time.Timezones
{
    /// <summary>
    /// Tools to allow you to render time zones
    /// </summary>
    public static class TimeZoneRenderers
    {
        /// <summary>
        /// Shows current time in selected time zone
        /// </summary>
        /// <param name="Zone">Time zone</param>
        /// <returns>True if found; False if not found</returns>
        public static bool ShowTimeZone(string Zone)
        {
            bool ZoneFound = TimeZones.TimeZoneExists(Zone);
            if (TimeZones.TimeZoneExists(Zone))
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_TIME_TIMEZONES_LISTENTRY") + " ({2})", Zone, GetZoneTimeString(Zone), ShowTimeZoneUtcOffset(Zone).ToString());
            return ZoneFound;
        }

        /// <summary>
        /// Shows current time in selected time zone
        /// </summary>
        /// <param name="Zone">Time zone to search</param>
        /// <returns>True if found; False if not found</returns>
        public static bool ShowTimeZones(string Zone)
        {
            var ZoneTimes = TimeZones.GetTimeZoneTimes();
            var ZoneFound = false;
            foreach (string ZoneName in ZoneTimes.Keys)
            {
                if (ZoneName.Contains(Zone))
                {
                    ZoneFound = true;
                    ShowTimeZone(ZoneName);
                }
            }
            return ZoneFound;
        }

        /// <summary>
        /// Shows current time in all time zones
        /// </summary>
        public static void ShowAllTimeZones()
        {
            var ZoneTimes = TimeZones.GetTimeZoneTimes();
            foreach (var TimeZone in ZoneTimes.Keys)
                ShowTimeZone(TimeZone);
        }

        /// <summary>
        /// Gets the zone time
        /// </summary>
        /// <param name="Zone">Time zone which we'll get the time from</param>
        /// <returns>The time in the specified time zone</returns>
        /// <exception cref="KernelException"></exception>
        public static DateTime GetZoneTime(string Zone)
        {
            if (!TimeZones.TimeZoneExists(Zone))
                throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_TIMEZONES_EXCEPTION_NOTFOUND1"), Zone);
            var ZoneTimes = TimeZones.GetTimeZoneTimes();
            return ZoneTimes[Zone];
        }

        /// <summary>
        /// Gets the zone time string
        /// </summary>
        /// <param name="Zone">Time zone which we'll get the time from</param>
        /// <returns>The time in the specified time zone in a string</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetZoneTimeString(string Zone) =>
            TimeDateRenderers.Render(GetZoneTime(Zone));

        /// <summary>
        /// Gets the zone time string
        /// </summary>
        /// <param name="Zone">Time zone which we'll get the time from</param>
        /// <param name="formatType">Format type</param>
        /// <returns>The time in the specified time zone in a string</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetZoneTimeString(string Zone, FormatType formatType) =>
            TimeDateRenderers.Render(GetZoneTime(Zone), formatType);

        /// <summary>
        /// Gets the zone time string
        /// </summary>
        /// <param name="Zone">Time zone which we'll get the time from</param>
        /// <param name="format">Custom format</param>
        /// <returns>The time in the specified time zone in a string</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetZoneTimeString(string Zone, string format) =>
            TimeDateRenderers.Render(GetZoneTime(Zone), format);

        /// <summary>
        /// Gets the zone time's date string
        /// </summary>
        /// <param name="Zone">Time zone which we'll get the time from</param>
        /// <returns>The date in the specified time zone in a string</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetZoneTimeDateString(string Zone) =>
            TimeDateRenderers.RenderDate(GetZoneTime(Zone));

        /// <summary>
        /// Gets the zone time's date string
        /// </summary>
        /// <param name="Zone">Time zone which we'll get the time from</param>
        /// <param name="formatType">Format type</param>
        /// <returns>The date in the specified time zone in a string</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetZoneTimeDateString(string Zone, FormatType formatType) =>
            TimeDateRenderers.RenderDate(GetZoneTime(Zone), formatType);

        /// <summary>
        /// Gets the time part of the zone time string
        /// </summary>
        /// <param name="Zone">Time zone which we'll get the time from</param>
        /// <returns>The time in the specified time zone in a string</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetZoneTimeTimeString(string Zone) =>
            TimeDateRenderers.RenderTime(GetZoneTime(Zone));

        /// <summary>
        /// Gets the time part of the zone time string
        /// </summary>
        /// <param name="Zone">Time zone which we'll get the time from</param>
        /// <param name="formatType">Format type</param>
        /// <returns>The time in the specified time zone in a string</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetZoneTimeTimeString(string Zone, FormatType formatType) =>
            TimeDateRenderers.RenderTime(GetZoneTime(Zone), formatType);

        /// <summary>
        /// Shows the time zone UTC offset
        /// </summary>
        /// <param name="Zone">Target time zone</param>
        /// <returns>A <see cref="TimeSpan"/> instance holding the UTC offset for the selected time zone</returns>
        public static TimeSpan ShowTimeZoneUtcOffset(string Zone)
        {
            if (!TimeZones.TimeZoneExists(Zone))
                throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_TIMEZONES_EXCEPTION_NOTFOUND1"), Zone);
            return TimeZoneInfo.FindSystemTimeZoneById(Zone).GetUtcOffset(TimeDateTools.KernelDateTime);
        }

        /// <summary>
        /// Shows the local time zone UTC offset
        /// </summary>
        /// <returns>A <see cref="TimeSpan"/> instance holding the UTC offset for the local time zone</returns>
        public static TimeSpan ShowTimeZoneUtcOffsetLocal()
        {
            // Return the UTC offset
            TimeZones.CheckZoneInfoDirectory();
            return TimeZones.GetCurrentZoneInfo().GetUtcOffset(TimeDateTools.KernelDateTime);
        }

        /// <summary>
        /// Shows the time zone UTC offset in a string from the local time zone
        /// </summary>
        /// <param name="Zone">Target time zone</param>
        /// <returns>A string holding the UTC offset for the selected time zone from the local time zone</returns>
        public static string ShowTimeZoneUtcOffsetString(string Zone) =>
            ShowTimeZoneUtcOffsetString(Zone, TimeDateRenderConstants.ShortTimeFormat);

        /// <summary>
        /// Shows the time zone UTC offset in a string from the local time zone
        /// </summary>
        /// <param name="Zone">Target time zone</param>
        /// <param name="format">Formatting the time</param>
        /// <returns>A string holding the UTC offset for the selected time zone from the local time zone</returns>
        public static string ShowTimeZoneUtcOffsetString(string Zone, string format) =>
            ShowTimeZoneUtcOffset(Zone).ToString((ShowTimeZoneUtcOffset(Zone) < TimeSpan.Zero ? TimeDateRenderConstants.MinusSignOffset : TimeDateRenderConstants.PlusSignOffset) + format);

        /// <summary>
        /// Shows the time zone UTC offset in a string from the local time zone
        /// </summary>
        /// <returns>A string holding the UTC offset for the local time zone</returns>
        public static string ShowTimeZoneUtcOffsetStringLocal() =>
            ShowTimeZoneUtcOffsetStringLocal(TimeDateRenderConstants.ShortTimeFormat);

        /// <summary>
        /// Shows the time zone UTC offset in a string from the local time zone
        /// </summary>
        /// <param name="format">Formatting the time</param>
        /// <returns>A string holding the UTC offset for the local time zone</returns>
        public static string ShowTimeZoneUtcOffsetStringLocal(string format) =>
            ShowTimeZoneUtcOffsetLocal().ToString((ShowTimeZoneUtcOffsetLocal() < TimeSpan.Zero ? TimeDateRenderConstants.MinusSignOffset : TimeDateRenderConstants.PlusSignOffset) + format);
    }
}
