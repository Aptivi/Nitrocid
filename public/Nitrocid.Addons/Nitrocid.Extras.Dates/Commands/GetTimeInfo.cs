//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
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
using Terminaux.Shell.Shells;

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

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool getNow = parameters.SwitchesList.Length > 0 && parameters.ContainsSwitch("-now");
            string date = parameters.ArgumentsList[0];
            DateTime dateTimeInfo = TimeDateTools.KernelDateTime;
            if (getNow || DateTime.TryParse(date, out dateTimeInfo))
            {
                SeparatorWriterColor.WriteSeparator(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_HEADER") + $" {TimeDateRenderers.Render(dateTimeInfo)}", true);
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_MS"), $"{dateTimeInfo.Millisecond}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_SECONDS"), $"{dateTimeInfo.Second}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_MINUTES"), $"{dateTimeInfo.Minute}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_HOURS"), $"{dateTimeInfo.Hour}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DAYS"), $"{dateTimeInfo.Day}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_MONTHS"), $"{dateTimeInfo.Month}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_YEARS"), $"{dateTimeInfo.Year}");
                TextWriterRaw.Write();

                // Whole date and time
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DATE"), TimeDateRenderers.RenderDate(dateTimeInfo));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_TIME"), TimeDateRenderers.RenderTime(dateTimeInfo));
                TextWriterRaw.Write();

                // Some more info
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DAYOFYEAR"), $"{dateTimeInfo.DayOfYear}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DAYOFWEEK"), dateTimeInfo.DayOfWeek.ToString());
                TextWriterRaw.Write();

                // Conversions
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_BINARY"), $"{dateTimeInfo.ToBinary()}");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_LOCALTIME"), TimeDateRenderers.Render(dateTimeInfo.ToLocalTime()));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_UNIVERSALTIME"), TimeDateRenderers.Render(dateTimeInfo.ToUniversalTime()));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_UNIXTIME"), $"{TimeDateConverters.DateToUnix(dateTimeInfo)}");
                TextWriterRaw.Write();

                // For the calendars
                foreach (var calendar in Enum.GetNames<CalendarTypes>())
                {
                    var calendarInstance = CalendarTools.GetCalendar(calendar);
                    ListEntryWriterColor.WriteListEntry(calendar, TimeDateRenderers.Render(dateTimeInfo, calendarInstance));
                }
                return 0;
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DATEINFOCANTPARSE1") + " {0}. " + LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DATEINFOCANTPARSE2"), true, ThemeColorType.Error, date);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.TimeDate);
            }
        }

    }
}
