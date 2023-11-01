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
using Nitrocid.Extras.Caffeine.Alarm;
using Nitrocid.Extras.Caffeine.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Caffeine
{
    internal class CaffeineInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "caffeine",
                new CommandInfo("caffeine", /* Localizable */ "Adds an alarm to alert you when your cup of tea or coffee is ready.",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "seconds", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                        })
                    }, new CaffeineCommand())
            },
        };

        string IAddon.AddonName => "Extras - Caffeine";

        AddonType IAddon.AddonType => AddonType.Optional;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, addonCommands.Values.ToArray());

        void IAddon.StopAddon()
        {
            AlarmListener.StopListener();
            CommandManager.UnregisterAddonCommands(ShellType.Shell, addonCommands.Keys.ToArray());
        }

        void IAddon.FinalizeAddon() =>
            AlarmListener.StartListener();
    }
}
