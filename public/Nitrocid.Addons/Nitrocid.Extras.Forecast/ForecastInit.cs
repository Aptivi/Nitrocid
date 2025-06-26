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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Modifications;
using System.Linq;
using Nettify.Weather;
using Nitrocid.Languages;

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
            ConfigTools.IsCustomSettingBuiltin(nameof(ForecastConfig)) ? (ForecastConfig)Config.baseConfigurations[nameof(ForecastConfig)] : new ForecastConfig();

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => new(new Dictionary<string, Delegate>()
        {
            { nameof(Forecast.Forecast.GetWeatherInfo), new Func<double, double, WeatherForecastInfo>(Forecast.Forecast.GetWeatherInfo) },
            { nameof(Forecast.Forecast.GetWeatherInfo) + "2", new Func<double, double, string, WeatherForecastInfo>(Forecast.Forecast.GetWeatherInfo) },
            { nameof(Forecast.Forecast.PrintWeatherInfo), new Action<double, double>(Forecast.Forecast.PrintWeatherInfo) },
            { nameof(Forecast.Forecast.PrintWeatherInfo) + "2", new Action<double, double, string>(Forecast.Forecast.PrintWeatherInfo) },
            { nameof(Forecast.Forecast.GetWeatherInfoOwm), new Func<long, WeatherForecastInfo>(Forecast.Forecast.GetWeatherInfoOwm) },
            { nameof(Forecast.Forecast.GetWeatherInfoOwm) + "2", new Func<long, string, WeatherForecastInfo>(Forecast.Forecast.GetWeatherInfoOwm) },
            { nameof(Forecast.Forecast.GetWeatherInfoOwm) + "3", new Func<string, WeatherForecastInfo>(Forecast.Forecast.GetWeatherInfoOwm) },
            { nameof(Forecast.Forecast.GetWeatherInfoOwm) + "4", new Func<string, string, WeatherForecastInfo>(Forecast.Forecast.GetWeatherInfoOwm) },
            { nameof(Forecast.Forecast.PrintWeatherInfoOwm), new Action<string>(Forecast.Forecast.PrintWeatherInfoOwm) },
            { nameof(Forecast.Forecast.PrintWeatherInfoOwm) + "2", new Action<string, string>(Forecast.Forecast.PrintWeatherInfoOwm) },
        });

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => new(new Dictionary<string, PropertyInfo>()
        {
            { nameof(Forecast.Forecast.PreferredUnit), typeof(Forecast.Forecast).GetProperty(nameof(Forecast.Forecast.PreferredUnit)) ?? throw new Exception(Translate.DoTranslation("There is no property info for") + $" {nameof(Forecast.Forecast.PreferredUnit)}") },
        });

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

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
