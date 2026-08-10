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

using Nitrocid.Base.Kernel;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Returns true or false depending on the kernel mode
    /// </summary>
    class IsModeCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "ismode";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ISMODE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo([],
                [
                    new SwitchInfo("s", /* Localizable */ "NKS_MISC_SPLASHES_WELCOME_SAFEMODE", new SwitchOptions()
                    {
                        AcceptsValues = false,
                        ConflictsWith = ["d", "m"]
                    }),
                    new SwitchInfo("d", /* Localizable */ "NKS_MISC_SPLASHES_WELCOME_DEBUGMODE", new SwitchOptions()
                    {
                        AcceptsValues = false,
                        ConflictsWith = ["s", "m"]
                    }),
                    new SwitchInfo("m", /* Localizable */ "NKS_MISC_SPLASHES_WELCOME_MAINTENANCE", new SwitchOptions()
                    {
                        AcceptsValues = false,
                        ConflictsWith = ["s", "d"]
                    }),
                    new SwitchInfo("v", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ISMODE_ARGUMENT_VERBOSE_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false,
                    }),
                ], true)
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool safeModeCheck = parameters.ContainsSwitch("-s");
            bool debugModeCheck = parameters.ContainsSwitch("-d");
            bool maintenanceModeCheck = parameters.ContainsSwitch("-m");
            bool verbose = parameters.ContainsSwitch("-v");
            bool result = false;

            if (!safeModeCheck && !debugModeCheck && !maintenanceModeCheck)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ISMODE_NOTSPECIFIED"), true, ThemeColorType.Error);
                return 46;
            }

            if (safeModeCheck)
                result = KernelEntry.SafeMode;
            if (debugModeCheck)
                result = KernelEntry.DebugMode;
            if (maintenanceModeCheck)
                result = KernelEntry.Maintenance;

            if (verbose)
                TextWriterColor.Write(result.ToString());
            return 0;
        }
    }
}
