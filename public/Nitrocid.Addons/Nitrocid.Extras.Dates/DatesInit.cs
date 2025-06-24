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

using Nitrocid.Extras.Dates.Commands;
using Nitrocid.Extras.Dates.Localized;
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
using Nitrocid.Base.Languages;

namespace Nitrocid.Extras.Dates
{
    internal class DatesInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("gettimeinfo", /* Localizable */ "NKS_DATES_COMMAND_GETTIMEINFO_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "date", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_DATES_COMMAND_GETTIMEINFO_ARGUMENT_DATE_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("now", /* Localizable */ "NKS_DATES_COMMAND_GETTIMEINFO_SWITCH_NOW_DESC", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1,
                            AcceptsValues = false
                        })
                    ])
                ], new GetTimeInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),
            
            new CommandInfo("expiry", /* Localizable */ "NKS_DATES_COMMAND_EXPIRY_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "production", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_DATES_COMMAND_EXPIRY_ARGUMENT_PRODUCTION_DESC"
                        }),
                        new CommandArgumentPart(true, "expiry", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_DATES_COMMAND_EXPIRY_ARGUMENT_EXPIRY_DESC"
                        })
                    ],
                    [
                        new SwitchInfo("implicit", /* Localizable */ "NKS_DATES_COMMAND_EXPIRY_STATUS_IMPLICIT_DESC", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ExpiryCommand()),

            new CommandInfo("stopwatch", /* Localizable */ "NKS_DATES_COMMAND_STOPWATCH_DESC", new StopwatchCommand()),

            new CommandInfo("timer", /* Localizable */ "NKS_DATES_COMMAND_TIMER_DESC", new TimerCommand()),

            new CommandInfo("pomodoro", /* Localizable */ "NKS_DATES_COMMAND_POMODORO_DESC", new PomodoroCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasDates);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasDates);

        public ModLoadPriority AddonType => ModLoadPriority.Optional;

        internal static DatesConfig DatesConfig =>
            (DatesConfig)Config.baseConfigurations[nameof(DatesConfig)];

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            var config = new DatesConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(DatesConfig));
            HomepageTools.UnregisterBuiltinAction("NKS_DATES_HOMEPAGE_TIMER");
            HomepageTools.UnregisterBuiltinAction("NKS_DATES_HOMEPAGE_STOPWATCH");
            HomepageTools.UnregisterBuiltinAction("NKS_DATES_HOMEPAGE_POMODORO");
        }

        public void FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction("NKS_DATES_HOMEPAGE_TIMER", TimerScreen.OpenTimer);
            HomepageTools.RegisterBuiltinAction("NKS_DATES_HOMEPAGE_STOPWATCH", StopwatchScreen.OpenStopwatch);
            HomepageTools.RegisterBuiltinAction("NKS_DATES_HOMEPAGE_POMODORO", PomodoroScreen.OpenPomodoro);
        }
    }
}
