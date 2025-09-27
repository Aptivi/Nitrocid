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
using Terminaux.Shell.Commands;
using System.Linq;
using Nitrocid.Base.Kernel.Time.Renderers;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows the current time and date
    /// </summary>
    /// <remarks>
    /// If you want to know what time is it without repeatedly going into the clock, you can use this command to show you the current time and date, as well as your time zone.
    /// </remarks>
    class DateCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
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

            // Now, show the date and the time
            if (showDate)
            {
                if (useUtc)
                {
                    string rendered = TimeDateRenderersUtc.RenderDateUtc();
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
                else
                {
                    string rendered = TimeDateRenderers.RenderDate();
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
            }
            if (showTime)
            {
                if (useUtc)
                {
                    string rendered = TimeDateRenderersUtc.RenderTimeUtc();
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
                else
                {
                    string rendered = TimeDateRenderers.RenderTime();
                    TextWriterColor.Write(rendered);
                    variableValue = rendered;
                }
            }
            return 0;
        }
    }
}
