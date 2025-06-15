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
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;
using Nitrocid.Shell.Homepage;
using Nitrocid.Extras.Dates.Timers;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Dates
{
    internal class DatesInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("gettimeinfo", LanguageTools.GetLocalized("NKS_DATES_COMMAND_GETTIMEINFO_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "date", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_DATES_COMMAND_GETTIMEINFO_ARGUMENT_DATE_DESC")
                        })
                    ],
                    [
                        new SwitchInfo("now", LanguageTools.GetLocalized("NKS_DATES_COMMAND_GETTIMEINFO_SWITCH_NOW_DESC"), new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1,
                            AcceptsValues = false
                        })
                    ])
                ], new GetTimeInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),
            
            new CommandInfo("expiry", LanguageTools.GetLocalized("NKS_DATES_COMMAND_EXPIRY_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "production", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_DATES_COMMAND_EXPIRY_ARGUMENT_PRODUCTION_DESC")
                        }),
                        new CommandArgumentPart(true, "expiry", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_DATES_COMMAND_EXPIRY_ARGUMENT_EXPIRY_DESC")
                        })
                    ],
                    [
                        new SwitchInfo("implicit", LanguageTools.GetLocalized("NKS_DATES_COMMAND_EXPIRY_STATUS_IMPLICIT_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ExpiryCommand()),

            new CommandInfo("stopwatch", LanguageTools.GetLocalized("NKS_DATES_COMMAND_STOPWATCH_DESC"), new StopwatchCommand()),

            new CommandInfo("timer", LanguageTools.GetLocalized("NKS_DATES_COMMAND_TIMER_DESC"), new TimerCommand()),

            new CommandInfo("pomodoro", LanguageTools.GetLocalized("NKS_DATES_COMMAND_POMODORO_DESC"), new PomodoroCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasDates);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static DatesConfig DatesConfig =>
            (DatesConfig)Config.baseConfigurations[nameof(DatesConfig)];

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Dates", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            var config = new DatesConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Dates");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(DatesConfig));
            HomepageTools.UnregisterBuiltinAction("Timer");
            HomepageTools.UnregisterBuiltinAction("Stopwatch");
            HomepageTools.UnregisterBuiltinAction("Pomodoro");
        }

        void IAddon.FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction("Timer", TimerScreen.OpenTimer);
            HomepageTools.RegisterBuiltinAction("Stopwatch", StopwatchScreen.OpenStopwatch);
            HomepageTools.RegisterBuiltinAction("Pomodoro", PomodoroScreen.OpenPomodoro);
        }
    }
}
