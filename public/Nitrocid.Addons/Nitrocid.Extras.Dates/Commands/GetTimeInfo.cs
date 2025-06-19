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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Calendars;
using Nitrocid.Kernel.Time.Converters;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
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
            bool getNow = parameters.SwitchesList.Length > 0 && SwitchManager.ContainsSwitch(parameters.SwitchesList, "-now");
            DateTime DateTimeInfo = TimeDateTools.KernelDateTime;
            if (getNow || DateTime.TryParse(parameters.ArgumentsList[0], out DateTimeInfo))
            {
                TextWriterColor.Write("-- " + LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_HEADER") + " {0} --" + CharManager.NewLine, TimeDateRenderers.Render(DateTimeInfo));
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_MS") + " {0}", DateTimeInfo.Millisecond);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_SECONDS") + " {0}", DateTimeInfo.Second);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_MINUTES") + " {0}", DateTimeInfo.Minute);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_HOURS") + " {0}", DateTimeInfo.Hour);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DAYS") + " {0}", DateTimeInfo.Day);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_MONTHS") + " {0}", DateTimeInfo.Month);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_YEARS") + " {0}" + CharManager.NewLine, DateTimeInfo.Year);

                // Whole date and time
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DATE") + " {0}", TimeDateRenderers.RenderDate(DateTimeInfo));
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_TIME") + " {0}" + CharManager.NewLine, TimeDateRenderers.RenderTime(DateTimeInfo));

                // Some more info
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DAYOFYEAR") + " {0}", DateTimeInfo.DayOfYear);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DAYOFWEEK") + " {0}" + CharManager.NewLine, DateTimeInfo.DayOfWeek.ToString());

                // Conversions
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_BINARY") + " {0}", DateTimeInfo.ToBinary());
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_LOCALTIME") + " {0}", TimeDateRenderers.Render(DateTimeInfo.ToLocalTime()));
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_UNIVERSALTIME") + " {0}", TimeDateRenderers.Render(DateTimeInfo.ToUniversalTime()));
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_UNIXTIME") + " {0}" + CharManager.NewLine, TimeDateConverters.DateToUnix(DateTimeInfo));

                // For the calendars
                foreach (var calendar in Enum.GetNames(typeof(CalendarTypes)))
                {
                    var calendarInstance = CalendarTools.GetCalendar(calendar);
                    TextWriterColor.Write("{0}: {1}", calendar, TimeDateRenderers.Render(DateTimeInfo, calendarInstance));
                }
                return 0;
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DATEINFOCANTPARSE1") + " {0}. " + LanguageTools.GetLocalized("NKS_DATES_TIMEINFO_DATEINFOCANTPARSE2"), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.TimeDate);
            }
        }

    }
}
