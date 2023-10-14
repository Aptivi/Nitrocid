
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

using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using KS.Misc.Screensaver;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using Nitrocid.Extras.NameGen.Commands;
using Nitrocid.Extras.NameGen.Screensavers;
using Nitrocid.Extras.NameGen.Settings;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.NameGen
{
    internal class NameGenInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "genname",
                new CommandInfo("genname", ShellType.Shell, /* Localizable */ "Name and surname generator",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "namescount"),
                            new CommandArgumentPart(false, "nameprefix"),
                            new CommandArgumentPart(false, "namesuffix"),
                            new CommandArgumentPart(false, "surnameprefix"),
                            new CommandArgumentPart(false, "surnamesuffix"),
                        }, new[] {
                            new SwitchInfo("t", /* Localizable */ "Generate nametags (umlauts are currently not supported)", new SwitchOptions()
                            {
                                AcceptsValues = false
                            }),
                            new SwitchInfo("male", /* Localizable */ "Generate names using the male names list", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "female", "both" },
                                AcceptsValues = false,
                            }),
                            new SwitchInfo("female", /* Localizable */ "Generate names using the female names list", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "male", "both" },
                                AcceptsValues = false,
                            }),
                            new SwitchInfo("both", /* Localizable */ "Generate names using the unified names list", new SwitchOptions()
                            {
                                ConflictsWith = new[] { "female", "male" },
                                AcceptsValues = false,
                            }),
                        }, true)
                    }, new GenNameCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable)
            },
        };

        string IAddon.AddonName => "Extras - NameGen";

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static NameGenSaversConfig SaversConfig =>
            (NameGenSaversConfig)Config.baseConfigurations[nameof(NameGenSaversConfig)];

        void IAddon.StartAddon()
        {
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());
            ScreensaverManager.Screensavers.Add("personlookup", new PersonLookupDisplay());

            // Then, initialize configuration in a way that no mod can play with them
            var saversConfig = new NameGenSaversConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
            ScreensaverManager.Screensavers.Remove("personlookup");
            ConfigTools.UnregisterBaseSetting(nameof(NameGenSaversConfig));
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
