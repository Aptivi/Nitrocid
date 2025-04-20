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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Time.Alarm;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Writer.CyclicWriters;

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
                    TextWriters.Write(Translate.DoTranslation("No caffeine alerts to abort."), KernelColorType.Error);
                    return 32;
                }
                var id = AlarmTools.alarms.Keys.Last((alarm) => alarm.Contains("Caffeine"));
                AlarmTools.StopAlarm(id);
            }
            else
            {
                string secsOrName = parameters.ArgumentsList[0];
                bool nameSpecified = caffeines.ContainsKey(secsOrName);
                if (!int.TryParse(secsOrName, out int alarmSeconds) && !caffeines.TryGetValue(secsOrName, out alarmSeconds))
                {
                    TextWriters.Write(Translate.DoTranslation("The seconds in which your cup will be ready is invalid."), KernelColorType.Error);
                    TextWriters.Write(Translate.DoTranslation("If you're trying to supply a name of the drink, check out the list below:"), KernelColorType.Tip);
                    TextWriters.WriteList(caffeines);
                    return 26;
                }
                AlarmTools.StartAlarm("Caffeine", Translate.DoTranslation("Your cup is now ready!"), alarmSeconds, nameSpecified ? Translate.DoTranslation(secsOrName) : "");
            }
            return 0;
        }

    }
}
