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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using System;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Time.Timezones;

namespace Nitrocid.Base.Kernel.Time.Renderers
{
    /// <summary>
    /// Miscellaneous time and date renderers
    /// </summary>
    public static class TimeDateMiscRenderers
    {
        /// <summary>
        /// Gets the remaining time from now
        /// </summary>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <returns>Remaining time from now in a TimeSpan representation</returns>
        public static TimeSpan GetRemainingTimeFromNow(int Milliseconds) =>
            GetRemainingTimeFrom(TimeDateTools.KernelDateTime, Milliseconds);

        /// <summary>
        /// Gets the remaining time from the specified date and time
        /// </summary>
        /// <param name="moment">A moment in time in which will be compared into how many milliseconds remaining</param>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <returns>Remaining time from the specified date and time in a TimeSpan representation</returns>
        public static TimeSpan GetRemainingTimeFrom(DateTime moment, int Milliseconds)
        {
            var RemainingTime = moment.AddMilliseconds(Milliseconds) - moment;
            return RemainingTime;
        }

        /// <summary>
        /// Gets the remaining time from now
        /// </summary>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <returns>Remaining time from now in a string representation</returns>
        public static string RenderRemainingTimeFromNow(int Milliseconds) =>
            RenderRemainingTimeFromNow(Milliseconds, TimeDateRenderConstants.FullTimeFormat);

        /// <summary>
        /// Gets the remaining time from now
        /// </summary>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <param name="format">A format which will be used to render the time remaining</param>
        /// <returns>Remaining time from now in a string representation</returns>
        public static string RenderRemainingTimeFromNow(int Milliseconds, string format) =>
            RenderRemainingTimeFrom(TimeDateTools.KernelDateTime, Milliseconds, format);

        /// <summary>
        /// Gets the remaining time from the specified date and time
        /// </summary>
        /// <param name="moment">A moment in time in which will be compared into how many milliseconds remaining</param>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <returns>Remaining time from the specified date and time in a string representation</returns>
        public static string RenderRemainingTimeFrom(DateTime moment, int Milliseconds) =>
            RenderRemainingTimeFrom(moment, Milliseconds, TimeDateRenderConstants.FullTimeFormat);

        /// <summary>
        /// Gets the remaining time from the specified date and time
        /// </summary>
        /// <param name="moment">A moment in time in which will be compared into how many milliseconds remaining</param>
        /// <param name="Milliseconds">The milliseconds interval</param>
        /// <param name="format">A format which will be used to render the time remaining</param>
        /// <returns>Remaining time from the specified date and time in a string representation</returns>
        public static string RenderRemainingTimeFrom(DateTime moment, int Milliseconds, string format)
        {
            var RemainingTime = moment.AddMilliseconds(Milliseconds) - moment;
            string RemainingTimeString = RemainingTime.ToString(format, CultureManager.CurrentCulture);
            return RemainingTimeString;
        }

        /// <summary>
        /// Shows current time, date, and timezone.
        /// </summary>
        public static void ShowCurrentTimes()
        {
            try
            {
                TextWriterColor.Write
                (
                    LanguageTools.GetLocalized("NKS_KERNEL_TIME_RENDERERS_TODAYIS") + " {0} @ {1} ({2}), {3} @ UTC",
                    TimeDateRenderers.Render(), TimeZones.GetCurrentZoneInfo().StandardName, TimeZoneRenderers.ShowTimeZoneUtcOffsetStringLocal(),
                    TimeDateRenderersUtc.RenderUtc()
                );
            }
            catch (KernelException kex) when (kex.ExceptionType == KernelExceptionType.TimeDate)
            {
                TextWriterColor.Write
                (
                    LanguageTools.GetLocalized("NKS_KERNEL_TIME_RENDERERS_TODAYIS") + " {0}, {1} @ UTC",
                    TimeDateRenderers.Render(), TimeDateRenderersUtc.RenderUtc()
                );
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Can't show time of day with {nameof(ShowCurrentTimes)}(): {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_TIME_RENDERERS_TIMESHOWFAILED") + $" {ex.Message}", ThemeColorType.Error);
            }
        }
    }
}
