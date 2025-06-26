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
using Nitrocid.Extras.Timers.Commands;
using Nitrocid.Extras.Timers.Settings;
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
using Nitrocid.Shell.Homepage;
using Nitrocid.Extras.Timers.Timers;

namespace Nitrocid.Extras.Timers
{
    internal class TimersInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("stopwatch", /* Localizable */ "A simple stopwatch",
                [
                    new CommandArgumentInfo()
                ], new StopwatchCommand()),

            new CommandInfo("timer", /* Localizable */ "A simple timer",
                [
                    new CommandArgumentInfo()
                ], new TimerCommand()),

            new CommandInfo("pomodoro", /* Localizable */ "Pomodoro timer",
                [
                    new CommandArgumentInfo()
                ], new PomodoroCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasTimers);

        internal static TimersConfig TimersConfig =>
            (TimersConfig)Config.baseConfigurations[nameof(TimersConfig)];

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon()
        {
            var config = new TimersConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(TimersConfig));
            HomepageTools.UnregisterBuiltinAction("Timer");
            HomepageTools.UnregisterBuiltinAction("Stopwatch");
            HomepageTools.UnregisterBuiltinAction("Pomodoro Timer");
        }

        void IAddon.FinalizeAddon()
        {
            // Add homepage entries
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "Timer", TimerScreen.OpenTimer);
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "Stopwatch", StopwatchScreen.OpenStopwatch);
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "Pomodoro Timer", PomodoroScreen.OpenPomodoro);
        }
    }
}
