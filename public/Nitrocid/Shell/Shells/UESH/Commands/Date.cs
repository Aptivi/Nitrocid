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

using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows the current time and date
    /// </summary>
    /// <remarks>
    /// If you want to know what time is it without repeatedly going into the clock, you can use this command to show you the current time and date, as well as your time zone.
    /// </remarks>
    class DateCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "date";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTD_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo([
                    new SwitchInfo("date", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DATE_SWITCH_DATE_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["time", "full"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("time", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DATE_SWITCH_TIME_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["date", "full"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("full", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SHOWTD_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["date", "time"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("utc", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_DATE_SWITCH_UTC_DESC", new SwitchOptions()
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

            // Now, show the date and the time
            if (showDate)
            {
                if (useUtc)
                    variableValue = TimeDateRenderersUtc.RenderDateUtc();
                else
                    variableValue = TimeDateRenderers.RenderDate();
            }
            if (showTime)
            {
                if (useUtc)
                    variableValue = TimeDateRenderersUtc.RenderTimeUtc();
                else
                    variableValue = TimeDateRenderers.RenderTime();
            }
            TextWriterColor.Write(variableValue);
            return 0;
        }
    }
}
