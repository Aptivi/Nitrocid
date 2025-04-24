//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Extras.TimeInfo.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;

namespace Nitrocid.Extras.TimeInfo
{
    internal class TimeInfoInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("gettimeinfo", /* Localizable */ "Gets the date and time information",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "date", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Date and time to print info from"
                        })
                    ],
                    [
                        new SwitchInfo("now", /* Localizable */ "Gets the current date and time information", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1,
                            AcceptsValues = false
                        })
                    ])
                ], new GetTimeInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("expiry", /* Localizable */ "Gets the product expiry information",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "production", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Production date (look at either the side or the bottom of the product)"
                        }),
                        new CommandArgumentPart(true, "expiry", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Expiry date or time period (either explicitly written in the same spot or time spans, such as 6 months or 1 year)"
                        })
                    ],
                    [
                        new SwitchInfo("implicit", /* Localizable */ "Whether the target product doesn't specify the expiry date explicitly", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ExpiryCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasTimeInfo);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);

        void IAddon.FinalizeAddon()
        { }
    }
}
