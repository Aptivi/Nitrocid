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

using Nitrocid.Extras.Timers.Commands;
using Nitrocid.Extras.Timers.Settings;
using Nitrocid.Kernel.Configuration;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using System.Linq;
using Nitrocid.Shell.Homepage;
using Nitrocid.Extras.Timers.Timers;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Timers
{
    internal class TimersInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("stopwatch", LanguageTools.GetLocalized("NKS_DATES_COMMAND_STOPWATCH_DESC"), new StopwatchCommand()),

            new CommandInfo("timer", LanguageTools.GetLocalized("NKS_DATES_COMMAND_TIMER_DESC"), new TimerCommand()),

            new CommandInfo("pomodoro", LanguageTools.GetLocalized("NKS_DATES_COMMAND_POMODORO_DESC"), new PomodoroCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasTimers);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasTimers);

        internal static TimersConfig TimersConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(TimersConfig)) ? (TimersConfig)Config.baseConfigurations[nameof(TimersConfig)] : Config.GetFallbackKernelConfig<TimersConfig>();

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Timers.Resources.Languages.Output.Localizations", typeof(TimersInit).Assembly));
            var config = new TimersConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(TimersConfig));
            HomepageTools.UnregisterBuiltinAction("Timer");
            HomepageTools.UnregisterBuiltinAction("Stopwatch");
            HomepageTools.UnregisterBuiltinAction("Pomodoro");
        }

        public void FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_DATES_HOMEPAGE_TIMER"), TimerScreen.OpenTimer);
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_DATES_HOMEPAGE_STOPWATCH"), StopwatchScreen.OpenStopwatch);
            HomepageTools.RegisterBuiltinAction(LanguageTools.GetLocalized("NKS_DATES_HOMEPAGE_POMODORO"), PomodoroScreen.OpenPomodoro);
        }
    }
}
