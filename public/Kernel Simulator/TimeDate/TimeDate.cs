
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.TimeDate
{
    /// <summary>
    /// Time and date module
    /// </summary>
    public static class TimeDate
    {

        /// <summary>
        /// The kernel time and date
        /// </summary>
        public static DateTime KernelDateTime => DateTime.Now;
        /// <summary>
        /// The kernel time and date (UTC)
        /// </summary>
        public static DateTime KernelDateTimeUtc => DateTime.UtcNow;

        /// <summary>
        /// Specifies the time/date format type.
        /// </summary>
        public enum FormatType
        {
            /// <summary>
            /// Long time/date format
            /// </summary>
            Long,
            /// <summary>
            /// Short time/date format
            /// </summary>
            Short
        }

        /// <summary>
        /// Shows current time, date, and timezone.
        /// </summary>
        public static void ShowCurrentTimes()
        {
            TextWriterColor.Write("datetime: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Current time is {0}"), true, KernelColorType.ListValue, TimeDateRenderers.RenderTime());
            TextWriterColor.Write("datetime: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Today is {0}"), true, KernelColorType.ListValue, TimeDateRenderers.RenderDate());
            TextWriterColor.Write("datetime: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Time and date in UTC: {0}"), true, KernelColorType.ListValue, TimeDateRenderersUtc.RenderUtc());
            TextWriterColor.Write("datetime: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Time Zone:") + " {0} ({1})", true, KernelColorType.ListValue, TimeZoneInfo.Local.StandardName, TimeZoneInfo.Local.GetUtcOffset(KernelDateTime).ToString((TimeZoneInfo.Local.GetUtcOffset(KernelDateTime) < TimeSpan.Zero ? @"\-" : @"\+") + @"hh\:mm\:ss"));
        }

        /// <summary>
        /// Gets the remaining time from now
        /// </summary>
        /// <param name="Milliseconds">The milliseconds interval</param>
        public static string GetRemainingTimeFromNow(int Milliseconds)
        {
            var ThisMoment = KernelDateTime;
            var RemainingTime = ThisMoment.AddMilliseconds(Milliseconds) - ThisMoment;
            string RemainingTimeString = RemainingTime.ToString(@"d\.hh\:mm\:ss\.fff", CultureManager.CurrentCult);
            return RemainingTimeString;
        }

    }
}
