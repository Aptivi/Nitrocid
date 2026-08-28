//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Extras.Forecast.Forecast.Commands;
using Nitrocid.Extras.Forecast.Settings;
using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Commands;
using Nitrocid.Kernel.Extensions;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Forecast
{
    internal class ForecastInit : IAddon
    {
        private readonly BaseCommand[] addonCommands =
        [
            new WeatherCommand(),
            new WeatherOldCommand(),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasForecast);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasForecast);

        internal static ForecastConfig ForecastConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ForecastConfig)) ? (ForecastConfig)Config.baseConfigurations[nameof(ForecastConfig)] : Config.GetFallbackKernelConfig<ForecastConfig>();

        public void FinalizeAddon()
        { }

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Forecast.Resources.Languages.Output.Localizations", typeof(ForecastInit).Assembly));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
            var config = new ForecastConfig();
            ConfigTools.RegisterBaseSetting(config);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(ForecastConfig));
        }
    }
}
