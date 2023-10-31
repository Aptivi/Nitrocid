//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Kernel.Extensions;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using Nitrocid.Extras.UnitConv.Commands;
using System.Collections.Generic;
using System.Linq;
using UnitsNet;

namespace Nitrocid.Extras.UnitConv
{
    internal class UnitConvInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "listunits",
                new CommandInfo("listunits", /* Localizable */ "Lists all available units",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "type", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => Quantity.Infos.Select((src) => src.Name).ToArray()
                            }),
                        })
                    }, new ListUnitsCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },

            { "unitconv",
                new CommandInfo("unitconv", /* Localizable */ "Unit converter",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "unittype", new CommandArgumentPartOptions()
                            {
                                AutoCompleter = (_) => Quantity.Infos.Select((src) => src.Name).ToArray()
                            }),
                            new CommandArgumentPart(true, "quantity", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(true, "sourceunit"),
                            new CommandArgumentPart(true, "targetunit"),
                        }, new[]
                        {
                            new SwitchInfo("tui", /* Localizable */ "Use the TUI version of the unit converter", new SwitchOptions()
                            {
                                OptionalizeLastRequiredArguments = 4,
                                AcceptsValues = false
                            })
                        })
                    }, new UnitConvCommand())
            },
        };

        string IAddon.AddonName => "Extras - UnitConv";

        AddonType IAddon.AddonType => AddonType.Optional;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());

        void IAddon.FinalizeAddon()
        { }
    }
}
