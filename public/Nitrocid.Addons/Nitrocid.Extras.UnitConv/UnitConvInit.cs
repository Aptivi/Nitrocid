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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Extras.UnitConv.Commands;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using System.Linq;
using UnitsNet;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using Nitrocid.Extras.UnitConv.Tools;
using Nitrocid.Shell.Homepage;

namespace Nitrocid.Extras.UnitConv
{
    internal class UnitConvInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("listunits", /* Localizable */ "Lists all available units",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Quantity.Infos.Select((src) => src.Name).ToArray(),
                            ArgumentDescription = /* Localizable */ "Unit type"
                        }),
                    ])
                ], new ListUnitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("unitconv", /* Localizable */ "Unit converter",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "unittype", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => Quantity.Infos.Select((src) => src.Name).ToArray(),
                            ArgumentDescription = /* Localizable */ "Unit type"
                        }),
                        new CommandArgumentPart(true, "quantity", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Quantity in the source unit to convert"
                        }),
                        new CommandArgumentPart(true, "sourceunit", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Source unit"
                        }),
                        new CommandArgumentPart(true, "targetunit", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Target unit"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", /* Localizable */ "Use the TUI version of the unit converter", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 4,
                            AcceptsValues = false
                        })
                    ])
                ], new UnitConvCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasUnitConv);

        void IAddon.StartAddon() =>
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            HomepageTools.UnregisterBuiltinAction("Unit Converter");
        }

        void IAddon.FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "Unit Converter", UnitConvTools.OpenUnitConvTui);
        }
    }
}
