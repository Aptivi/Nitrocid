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

using Nitrocid.Base.Kernel.Time.Timezones;
using Nitrocid.Base.Languages;
using Terminaux.Inputs.Interactive;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command => 
            "showtdzone";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "timezone", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_ARGUMENT_TIMEZONE_DESC"
                    }),
                ],
                [
                    new SwitchInfo("all", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_SWITCH_ALL_DESC", new SwitchOptions()
                    {
                        OptionalizeLastRequiredArguments = 1,
                        AcceptsValues = false
                    }),
                    new SwitchInfo("selection", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTDZONE_SWITCH_SELECTION_DESC", new SwitchOptions()
                    {
                        OptionalizeLastRequiredArguments = 1,
                        AcceptsValues = false
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string timezone = parameters.ArgumentsList[0];
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
                else if (!TimeZoneRenderers.ShowTimeZones(timezone))
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SHOWTDZONE_INCORRECT"), true, ThemeColorType.Error);
            }
            return 0;
        }

    }
}
