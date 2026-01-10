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

using Terminaux.Themes.Colors;
using Terminaux.Inputs.Interactive;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Misc.Interactives;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Time.Timezones;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows current time and date in another timezone
    /// </summary>
    /// <remarks>
    /// If you need to know what time is it on another city or country, you can use this tool to tell you the current time and date in another country or city.
    /// <br></br>
    /// This command is multi-platform, and uses the IANA timezones on Unix systems and the Windows timezone system on Windows.
    /// <br></br>
    /// For example, if you need to use "Asia/Damascus" on the Unix systems, you will write "showtdzone Asia/Damascus." However on Windows 10, assuming we're on the summer season, you write showtdzone "Syria Daylight Time"
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-all</term>
    /// <description>Displays all timezones and their times and dates</description>
    /// </item>
    /// <item>
    /// <term>-selection</term>
    /// <description>Opens an interactive TUI in which you'll be able to see the world clock in real time</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class ShowTdZoneCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool ShowAll = parameters.ContainsSwitch("-all");
            bool useTui = parameters.ContainsSwitch("-selection");
            if (useTui)
            {
                var tui = new TimeZoneShowCli();
                InteractiveTuiTools.OpenInteractiveTui(tui);
            }
            else
            {
                if (ShowAll)
                    TimeZoneRenderers.ShowAllTimeZones();
                else if (!TimeZoneRenderers.ShowTimeZones(parameters.ArgumentsList[0]))
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SHOWTDZONE_INCORRECT"), true, ThemeColorType.Error);
            }
            return 0;
        }

    }
}
