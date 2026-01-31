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

namespace Nitrocid.Extras.TimeInfo.Commands
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
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Information for") + " {0}", true, TimeDateRenderers.Render(DateTimeInfo));
                TextWriters.Write(Translate.DoTranslation("Milliseconds:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, DateTimeInfo.Millisecond);
                TextWriters.Write(Translate.DoTranslation("Seconds:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, DateTimeInfo.Second);
                TextWriters.Write(Translate.DoTranslation("Minutes:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, DateTimeInfo.Minute);
                TextWriters.Write(Translate.DoTranslation("Hours:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, DateTimeInfo.Hour);
                TextWriters.Write(Translate.DoTranslation("Days:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, DateTimeInfo.Day);
                TextWriters.Write(Translate.DoTranslation("Months:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, DateTimeInfo.Month);
                TextWriters.Write(Translate.DoTranslation("Year:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, DateTimeInfo.Year);
                TextWriterRaw.Write();

                // Whole date and time
                TextWriters.Write(Translate.DoTranslation("Date:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, TimeDateRenderers.RenderDate(DateTimeInfo));
                TextWriters.Write(Translate.DoTranslation("Time:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, TimeDateRenderers.RenderTime(DateTimeInfo));
                TextWriterRaw.Write();

                // Some more info
                TextWriters.Write(Translate.DoTranslation("Day of Year:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, DateTimeInfo.DayOfYear);
                TextWriters.Write(Translate.DoTranslation("Day of Week:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, DateTimeInfo.DayOfWeek.ToString());
                TextWriterRaw.Write();

                // Conversions
                TextWriters.Write(Translate.DoTranslation("Binary:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, DateTimeInfo.ToBinary());
                TextWriters.Write(Translate.DoTranslation("Local Time:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, TimeDateRenderers.Render(DateTimeInfo.ToLocalTime()));
                TextWriters.Write(Translate.DoTranslation("Universal Time:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, TimeDateRenderers.Render(DateTimeInfo.ToUniversalTime()));
                TextWriters.Write(Translate.DoTranslation("Unix Time:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write("{0}", KernelColorType.ListValue, TimeDateConverters.DateToUnix(DateTimeInfo));
                TextWriterRaw.Write();

                // For the calendars
                foreach (var calendar in Enum.GetNames(typeof(CalendarTypes)))
                {
                    var calendarInstance = CalendarTools.GetCalendar(calendar);
                    TextWriters.Write("{0}: ", false, KernelColorType.ListEntry, calendar);
                    TextWriters.Write("{0}", KernelColorType.ListValue, TimeDateRenderers.Render(DateTimeInfo, calendarInstance));
                }
                return 0;
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Failed to parse date information for") + " {0}. " + Translate.DoTranslation("Ensure that the format is correct."), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.TimeDate);
            }
        }

    }
}
