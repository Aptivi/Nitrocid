//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.Extras.Forecast.Forecast.Commands;
using Nitrocid.Extras.Forecast.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;

namespace Nitrocid.Extras.Forecast
{
    internal class ForecastInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "weather",
                new CommandInfo("weather", /* Localizable */ "Shows weather info for specified city. Uses OpenWeatherMap.",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "CityID/CityName"),
                            new CommandArgumentPart(false, "apikey"),
                        ],
                        [
                            new SwitchInfo("list", /* Localizable */ "Shows all the available cities", new SwitchOptions()
                            {
                                OptionalizeLastRequiredArguments = 2,
                                AcceptsValues = false
                            })
                        ])
                    ], new WeatherCommand())
            },
        };

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasForecast);

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static ForecastConfig ForecastConfig =>
            (ForecastConfig)Config.baseConfigurations[nameof(ForecastConfig)];

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon()
        {
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands.Values]);
            var config = new ForecastConfig();
            ConfigTools.RegisterBaseSetting(config);
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Keys]);
            ConfigTools.UnregisterBaseSetting(nameof(ForecastConfig));
        }
    }
}
