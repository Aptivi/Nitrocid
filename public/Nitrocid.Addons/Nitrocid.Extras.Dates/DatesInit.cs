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

using Nitrocid.Extras.Dates.Commands;
using Nitrocid.Extras.Dates.Settings;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Base.Kernel.Extensions;
using System.Linq;
using Nitrocid.Base.Shell.Homepage;
using Nitrocid.Extras.Dates.Timers;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Core.Languages;

namespace Nitrocid.Extras.Dates
{
    internal class DatesInit : IAddon
    {
        private readonly List<BaseCommand> addonCommands =
        [
            new GetTimeInfoCommand(),
            new ExpiryCommand(),
            new StopwatchCommand(),
            new TimerCommand(),
            new PomodoroCommand(),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasDates);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasDates);

        internal static DatesConfig DatesConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(DatesConfig)) ? (DatesConfig)Config.baseConfigurations[nameof(DatesConfig)] : Config.GetFallbackKernelConfig<DatesConfig>();

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Dates.Resources.Languages.Output.Localizations", typeof(DatesInit).Assembly));
            var config = new DatesConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);

            // Add homepage entries
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_DATES_HOMEPAGE_TIMER", TimerScreen.OpenTimer);
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_DATES_HOMEPAGE_STOPWATCH", StopwatchScreen.OpenStopwatch);
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_DATES_HOMEPAGE_POMODORO", PomodoroScreen.OpenPomodoro);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(DatesConfig));
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_DATES_HOMEPAGE_TIMER");
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_DATES_HOMEPAGE_STOPWATCH");
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_DATES_HOMEPAGE_POMODORO");
        }
    }
}
