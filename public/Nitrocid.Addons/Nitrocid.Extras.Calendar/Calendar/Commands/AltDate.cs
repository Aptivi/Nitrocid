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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Time.Calendars;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using System;
using System.Linq;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Determine how to show date and time
            bool showDate = true;
            bool showTime = true;
            bool useUtc = false;
            if (parameters.SwitchesList.Length > 0)
            {
                showDate = parameters.SwitchesList.Contains("-date") || parameters.SwitchesList.Contains("-full");
                showTime = parameters.SwitchesList.Contains("-time") || parameters.SwitchesList.Contains("-full");
                useUtc = parameters.SwitchesList.Contains("-utc");
                if (!showDate && !showTime)
                    showDate = showTime = true;
            }

            // Determine the culture
            string culture = parameters.ArgumentsList[0];
            if (!Enum.TryParse(culture, out CalendarTypes calendarType))
            {
                TextWriters.Write(Translate.DoTranslation("Culture isn't found.") + $" {culture}", true, KernelColorType.Error);
                return 16;
            }
            var cultureInstance = CalendarTools.GetCalendar(calendarType);

            // Now, show the date and the time
            if (showDate)
            {
                if (useUtc)
                {
                    string rendered = TimeDateRenderersUtc.RenderDateUtc(cultureInstance);
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
                else
                {
                    string rendered = TimeDateRenderers.RenderDate(cultureInstance);
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
            }
            if (showTime)
            {
                if (useUtc)
                {
                    string rendered = TimeDateRenderersUtc.RenderTimeUtc(cultureInstance);
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
                else
                {
                    string rendered = TimeDateRenderers.RenderTime(cultureInstance);
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
            }
            return 0;
        }
    }
}
