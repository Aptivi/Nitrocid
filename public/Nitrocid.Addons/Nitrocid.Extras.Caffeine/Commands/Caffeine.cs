﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Time.Alarm;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Caffeine.Commands
{
    /// <summary>
    /// Sets up your coffee or tea alarm
    /// </summary>
    /// <remarks>
    /// This command schedules your tea or coffee alarm to make the kernel emit a sound when it's ready.
    /// </remarks>
    class CaffeineCommand : BaseCommand, ICommand
    {

        private static readonly Dictionary<string, int> caffeines = new()
        {
            { /* Localizable */ "American Coffee",  60 * 5 },
            { /* Localizable */ "Red Tea",          60 * 10 },
            { /* Localizable */ "Green Tea",        60 * 10 },
        };

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool abortCurrentAlarm = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-abort");
            if (abortCurrentAlarm)
            {
                if (!AlarmTools.IsAlarmRegistered("Caffeine"))
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("No caffeine alerts to abort."), KernelColorType.Error);
                    return 32;
                }
                var (id, name) = AlarmTools.alarms.Keys.Last((alarm) => alarm.id.Contains("Caffeine"));
                AlarmTools.StopAlarm(id);
            }
            else
            {
                string secsOrName = parameters.ArgumentsList[0];
                if (!int.TryParse(secsOrName, out int alarmSeconds) && !caffeines.TryGetValue(secsOrName, out alarmSeconds))
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("The seconds in which your cup will be ready is invalid."), KernelColorType.Error);
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("If you're trying to supply a name of the drink, check out the list below:"), KernelColorType.Tip);
                    ListWriterColor.WriteList(caffeines);
                    return 26;
                }
                AlarmTools.StartAlarm("Caffeine", Translate.DoTranslation("Your cup is now ready!"), alarmSeconds);
            }
            return 0;
        }

    }
}