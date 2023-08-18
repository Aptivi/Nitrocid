
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Time.Renderers;
using KS.Shell.ShellBase.Commands;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows the current time and date
    /// </summary>
    /// <remarks>
    /// If you want to know what time is it without repeatedly going into the clock, you can use this command to show you the current time and date, as well as your time zone.
    /// </remarks>
    class DateCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // Determine how to show date and time
            bool showDate = true;
            bool showTime = true;
            bool useUtc = false;
            if (ListSwitchesOnly.Length > 0)
            {
                showDate = ListSwitchesOnly.Contains("-date") || ListSwitchesOnly.Contains("-full");
                showTime = ListSwitchesOnly.Contains("-time") || ListSwitchesOnly.Contains("-full");
                useUtc = ListSwitchesOnly.Contains("-utc");
                if (!showDate && !showTime)
                    showDate = showTime = true;
            }

            // Now, show the date and the time
            if (showDate)
            {
                if (useUtc)
                    TextWriterColor.Write(TimeDateRenderersUtc.RenderDateUtc());
                else
                    TextWriterColor.Write(TimeDateRenderers.RenderDate());
            }
            if (showTime)
            {
                if (useUtc)
                    TextWriterColor.Write(TimeDateRenderersUtc.RenderTimeUtc());
                else
                    TextWriterColor.Write(TimeDateRenderers.RenderTime());
            }
        }
    }
}
