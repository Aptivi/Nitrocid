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

using System.Linq;
using Nitrocid.Drivers;
using Nitrocid.Kernel.Hardware;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows hardware information
    /// </summary>
    /// <remarks>
    /// This shows you the detailed hardware information, including the CPU information and its features.
    /// </remarks>
    class HwInfoCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "hwinfo";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_HWINFO_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "hardwareType", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => DriverHandler.CurrentHardwareProberDriverLocal.SupportedHardwareTypes.Union(["all"]).ToArray(),
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_HWINFO_ARGUMENT_TYPE_DESC"
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.Wrappable | CommandFlags.RedirectionSupported;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            string hardwareType = parameters.ArgumentsList[0];
            HardwareList.ListHardware(hardwareType);
            return 0;
        }

    }
}
