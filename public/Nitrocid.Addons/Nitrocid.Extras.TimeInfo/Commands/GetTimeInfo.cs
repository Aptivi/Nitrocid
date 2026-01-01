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
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
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
                TextWriterColor.Write("-- " + Translate.DoTranslation("Information for") + " {0} --" + CharManager.NewLine, TimeDateRenderers.Render(DateTimeInfo));
                TextWriterColor.Write(Translate.DoTranslation("Milliseconds:") + " {0}", DateTimeInfo.Millisecond);
                TextWriterColor.Write(Translate.DoTranslation("Seconds:") + " {0}", DateTimeInfo.Second);
                TextWriterColor.Write(Translate.DoTranslation("Minutes:") + " {0}", DateTimeInfo.Minute);
                TextWriterColor.Write(Translate.DoTranslation("Hours:") + " {0}", DateTimeInfo.Hour);
                TextWriterColor.Write(Translate.DoTranslation("Days:") + " {0}", DateTimeInfo.Day);
                TextWriterColor.Write(Translate.DoTranslation("Months:") + " {0}", DateTimeInfo.Month);
                TextWriterColor.Write(Translate.DoTranslation("Year:") + " {0}" + CharManager.NewLine, DateTimeInfo.Year);

                // Whole date and time
                TextWriterColor.Write(Translate.DoTranslation("Date:") + " {0}", TimeDateRenderers.RenderDate(DateTimeInfo));
                TextWriterColor.Write(Translate.DoTranslation("Time:") + " {0}" + CharManager.NewLine, TimeDateRenderers.RenderTime(DateTimeInfo));

                // Some more info
                TextWriterColor.Write(Translate.DoTranslation("Day of Year:") + " {0}", DateTimeInfo.DayOfYear);
                TextWriterColor.Write(Translate.DoTranslation("Day of Week:") + " {0}" + CharManager.NewLine, DateTimeInfo.DayOfWeek.ToString());

                // Conversions
                TextWriterColor.Write(Translate.DoTranslation("Binary:") + " {0}", DateTimeInfo.ToBinary());
                TextWriterColor.Write(Translate.DoTranslation("Local Time:") + " {0}", TimeDateRenderers.Render(DateTimeInfo.ToLocalTime()));
                TextWriterColor.Write(Translate.DoTranslation("Universal Time:") + " {0}", TimeDateRenderers.Render(DateTimeInfo.ToUniversalTime()));
                TextWriterColor.Write(Translate.DoTranslation("Unix Time:") + " {0}" + CharManager.NewLine, TimeDateConverters.DateToUnix(DateTimeInfo));

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
                TextWriters.Write(Translate.DoTranslation("Failed to parse date information for") + " {0}. " + Translate.DoTranslation("Ensure that the format is correct."), true, KernelColorType.Error, parameters.ArgumentsList[0]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.TimeDate);
            }
        }

    }
}
