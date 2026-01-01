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
using Nitrocid.Extras.Caffeine.Commands;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using System.Linq;

namespace Nitrocid.Extras.Caffeine
{
    internal class CaffeineInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("caffeine", /* Localizable */ "Adds an alarm to alert you when your cup of tea or coffee is ready.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "secondsOrName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Either specify a number of seconds or a drink name"
                        }),
                    ],
                    [
                        new SwitchInfo("abort", /* Localizable */ "Aborts an alarm that alerts you when your cup of tea or coffee is ready.", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1
                        })
                    ])
                ], new CaffeineCommand())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCaffeine);

        void IAddon.StartAddon() =>
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);

        void IAddon.FinalizeAddon()
        { }
    }
}
