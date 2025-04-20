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
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Modifications;
using System.Linq;
using Nitrocid.Extras.Stocks.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Extras.Stocks.Commands;
using Nitrocid.Users.Login.Widgets;
using Nitrocid.Extras.Stocks.Widgets;

namespace Nitrocid.Extras.Stocks
{
    internal class StocksInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("stock", /* Localizable */ "Gets an hourly stock information",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "company", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Short company symbol"
                        }),
                    ])
                ], new StockCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasStocks);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static StocksConfig StocksConfig =>
            (StocksConfig)Config.baseConfigurations[nameof(StocksConfig)];

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon()
        {
            var config = new StocksConfig();
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
            ConfigTools.RegisterBaseSetting(config);
            WidgetTools.AddBaseWidget(new StocksWidget());
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(StocksConfig));
            WidgetTools.RemoveBaseWidget(nameof(StocksWidget));
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
