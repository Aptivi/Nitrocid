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
using Nitrocid.Kernel.Time.Calendars;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.Calendar.Calendar.Commands
{
    /// <summary>
    /// Shows the current time and date from alternative culture
    /// </summary>
    /// <remarks>
    /// If you want to know what time is it without repeatedly going into the clock, you can use this command to show you the current time and date, as well as your time zone.
    /// </remarks>
    class AltDateCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "altdate";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_ALTDATE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "culture", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_CALENDAR_COMMAND_ALTDATE_ARGUMENT_CULTURE_DESC"
                    })
                ],
                [
                    new SwitchInfo("date", /* Localizable */ "NKS_CALENDAR_COMMAND_ALTDATE_SWITCH_DATE_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["full", "time"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("time", /* Localizable */ "NKS_CALENDAR_COMMAND_ALTDATE_SWITCH_TIME_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["date", "full"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("full", /* Localizable */ "NKS_CALENDAR_COMMAND_ALTDATE_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["date", "time"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("utc", /* Localizable */ "NKS_CALENDAR_COMMAND_ALTDATE_SWITCH_UTC_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    })
                ], true)
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Determine how to show date and time
            bool showDate = true;
            bool showTime = true;
            bool useUtc = false;
            if (parameters.SwitchesList.Length > 0)
            {
                showDate = parameters.ContainsSwitch("-date") || parameters.ContainsSwitch("-full");
                showTime = parameters.ContainsSwitch("-time") || parameters.ContainsSwitch("-full");
                useUtc = parameters.ContainsSwitch("-utc");
                if (!showDate && !showTime)
                    showDate = showTime = true;
            }

            // Determine the culture
            string culture = parameters.ArgumentsList[0];
            if (!Enum.TryParse(culture, out CalendarTypes calendarType))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_CALENDAR_NOCULTURE") + $" {culture}", true, ThemeColorType.Error);
                return 16;
            }
            var cultureInstance = CalendarTools.GetCalendar(calendarType);

            // Now, show the date and the time
            if (showDate)
            {
                if (useUtc)
                    variableValue = TimeDateRenderersUtc.RenderDateUtc(cultureInstance);
                else
                    variableValue = TimeDateRenderers.RenderDate(cultureInstance);
            }
            if (showTime)
            {
                if (useUtc)
                    variableValue = TimeDateRenderersUtc.RenderTimeUtc(cultureInstance);
                else
                    variableValue = TimeDateRenderers.RenderTime(cultureInstance);
            }
            TextWriterColor.Write(variableValue);
            return 0;
        }
    }
}
