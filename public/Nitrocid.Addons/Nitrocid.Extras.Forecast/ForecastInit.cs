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
using Nitrocid.Extras.Forecast.Forecast.Commands;
using Nitrocid.Extras.Forecast.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;

namespace Nitrocid.Extras.Forecast
{
    internal class ForecastInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("weather", /* Localizable */ "Shows weather info for specified city. Uses The Weather Channel from IBM.",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("tui", /* Localizable */ "Weather info in an interactive TUI", new SwitchOptions()
                        {
                            AcceptsValues = false,
                        })
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "latitude", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Latitude to use"
                        }),
                        new CommandArgumentPart(true, "longitude", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Longitude to use"
                        }),
                        new CommandArgumentPart(false, "apikey", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Weather.com API key"
                        }),
                    ],
                    [
                        new SwitchInfo("list", /* Localizable */ "Shows all the available cities and their latitude/longitude pairs", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 3,
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        })
                    ])
                ], new WeatherCommand()),
            new CommandInfo("weather-old", /* Localizable */ "Shows weather info for specified city. Uses OpenWeatherMap.",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "CityID/CityName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "City ID or city name known to OpenWeatherMap"
                        }),
                        new CommandArgumentPart(false, "apikey", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "OpenWeatherMap API key"
                        }),
                    ],
                    [
                        new SwitchInfo("list", /* Localizable */ "Shows all the available cities", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 2,
                            AcceptsValues = false
                        })
                    ])
                ], new WeatherOldCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasForecast);

        internal static ForecastConfig ForecastConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ForecastConfig)) ? (ForecastConfig)Config.baseConfigurations[nameof(ForecastConfig)] : Config.GetFallbackKernelConfig<ForecastConfig>();

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon()
        {
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
            var config = new ForecastConfig();
            ConfigTools.RegisterBaseSetting(config);
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(ForecastConfig));
        }
    }
}
