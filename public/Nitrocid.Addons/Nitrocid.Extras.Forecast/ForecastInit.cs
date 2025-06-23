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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Extras.Forecast.Forecast.Commands;
using Nitrocid.Extras.Forecast.Localized;
using Nitrocid.Extras.Forecast.Settings;
using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Forecast
{
    internal class ForecastInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("weather", /* Localizable */ "NKS_FORECAST_COMMAND_WEATHER_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new SwitchInfo("tui", /* Localizable */ "NKS_FORECAST_COMMAND_WEATHER_SWITCH_TUI_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false,
                        })
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "latitude", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_FORECAST_COMMAND_WEATHER_ARGUMENT_LATITUDE_DESC"
                        }),
                        new CommandArgumentPart(true, "longitude", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_FORECAST_COMMAND_WEATHER_ARGUMENT_LONGITUDE_DESC"
                        }),
                        new CommandArgumentPart(false, "apikey", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_FORECAST_COMMAND_WEATHER_ARGUMENT_APIKEY_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("list", /* Localizable */ "NKS_FORECAST_COMMAND_WEATHER_SWITCH_LIST_DESC", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 3,
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        })
                    ])
                ], new WeatherCommand()),
            new CommandInfo("weather-old", /* Localizable */ "NKS_FORECAST_COMMAND_WEATHEROLD_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "CityID/CityName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_FORECAST_COMMAND_WEATHEROLD_ARGUMENT_CITY_DESC"
                        }),
                        new CommandArgumentPart(false, "apikey", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_FORECAST_COMMAND_WEATHEROLD_ARGUMENT_APIKEY_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("list", /* Localizable */ "NKS_FORECAST_COMMAND_WEATHEROLD_SWITCH_LIST_DESC", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 2,
                            AcceptsValues = false
                        })
                    ])
                ], new WeatherOldCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasForecast);

        string IAddon.AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasForecast);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static ForecastConfig ForecastConfig =>
            (ForecastConfig)Config.baseConfigurations[nameof(ForecastConfig)];

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Forecast", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
            var config = new ForecastConfig();
            ConfigTools.RegisterBaseSetting(config);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Forecast");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(ForecastConfig));
        }
    }
}
