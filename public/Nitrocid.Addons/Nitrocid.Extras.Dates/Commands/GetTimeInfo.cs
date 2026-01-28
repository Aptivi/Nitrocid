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

using System;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Time;
using Calendrier;
using Nitrocid.Base.Kernel.Time.Converters;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Textify.General;

namespace Nitrocid.Extras.Dates.Commands
{
    /// <summary>
    /// Shows time information
    /// </summary>
    /// <remarks>
    /// This shows you the detailed time information, including the time analysis, binary representation, and even the Unix time.
    /// </remarks>
    class GetTimeInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool getNow = parameters.SwitchesList.Length > 0 && parameters.ContainsSwitch("-now");
            DateTime DateTimeInfo = TimeDateTools.KernelDateTime;
            if (getNow || DateTime.TryParse(parameters.ArgumentsList[0], out DateTimeInfo))
            {
                SeparatorWriterColor.WriteSeparator(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_HEADER") + " {0}", true, TimeDateRenderers.Render(DateTimeInfo));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_MS"), "{0}".FormatString(DateTimeInfo.Millisecond));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_SECONDS"), "{0}".FormatString(DateTimeInfo.Second));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_MINUTES"), "{0}".FormatString(DateTimeInfo.Minute));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_HOURS"), "{0}".FormatString(DateTimeInfo.Hour));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DAYS"), "{0}".FormatString(DateTimeInfo.Day));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_MONTHS"), "{0}".FormatString(DateTimeInfo.Month));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_YEARS"), "{0}".FormatString(DateTimeInfo.Year));
                TextWriterRaw.Write();

                // Whole date and time
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DATE"), "{0}".FormatString(TimeDateRenderers.RenderDate(DateTimeInfo)));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_TIME"), "{0}".FormatString(TimeDateRenderers.RenderTime(DateTimeInfo)));
                TextWriterRaw.Write();

                // Some more info
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DAYOFYEAR"), "{0}".FormatString(DateTimeInfo.DayOfYear));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DAYOFWEEK"), "{0}".FormatString(DateTimeInfo.DayOfWeek.ToString()));
                TextWriterRaw.Write();

                // Conversions
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_BINARY"), "{0}".FormatString(DateTimeInfo.ToBinary()));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_LOCALTIME"), "{0}".FormatString(TimeDateRenderers.Render(DateTimeInfo.ToLocalTime())));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_UNIVERSALTIME"), "{0}".FormatString(TimeDateRenderers.Render(DateTimeInfo.ToUniversalTime())));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_UNIXTIME"), "{0}".FormatString(TimeDateConverters.DateToUnix(DateTimeInfo)));
                TextWriterRaw.Write();

                // For the calendars
                foreach (var calendar in Enum.GetNames(typeof(CalendarTypes)))
                {
                    var calendarInstance = CalendarTools.GetCalendar(calendar);
                    ListEntryWriterColor.WriteListEntry(calendar, TimeDateRenderers.Render(DateTimeInfo, calendarInstance));
                }
                return 0;
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DATEINFOCANTPARSE1") + " {0}. " + LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DATEINFOCANTPARSE2"), true, ThemeColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.TimeDate);
            }
        }

    }
}
