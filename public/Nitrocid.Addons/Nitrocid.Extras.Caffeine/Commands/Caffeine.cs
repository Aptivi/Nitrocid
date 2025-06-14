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
using Nitrocid.Kernel.Time.Alarm;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
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
        private static Dictionary<string, (string, int)> Caffeines => new()
        {
            { "American Coffee", (LanguageTools.GetLocalized("NKS_CAFFEINE_AMERICANCOFFEE", "Nitrocid.Extras.Caffeine"), 60 * 5) },
            { "Red Tea",         (LanguageTools.GetLocalized("NKS_CAFFEINE_REDTEA", "Nitrocid.Extras.Caffeine"), 60 * 10) },
            { "Green Tea",       (LanguageTools.GetLocalized("NKS_CAFFEINE_GREENTEA", "Nitrocid.Extras.Caffeine"), 60 * 10) },
        };

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool abortCurrentAlarm = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-abort");
            if (abortCurrentAlarm)
            {
                if (!AlarmTools.IsAlarmRegistered("Caffeine"))
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_CAFFEINE_NOALERTS", "Nitrocid.Extras.Caffeine"), KernelColorType.Error);
                    return 32;
                }
                var id = AlarmTools.alarms.Keys.Last((alarm) => alarm.Contains("Caffeine"));
                AlarmTools.StopAlarm(id);
            }
            else
            {
                string secsOrName = parameters.ArgumentsList[0];
                bool nameSpecified = Caffeines.ContainsKey(secsOrName);
                if (!int.TryParse(secsOrName, out int alarmSeconds))
                {
                    if (!Caffeines.TryGetValue(secsOrName, out var alarmSpecifier))
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_CAFFEINE_INVALIDSECONDS", "Nitrocid.Extras.Caffeine"), KernelColorType.Error);
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_CAFFEINE_INVALIDSECONDSTIP", "Nitrocid.Extras.Caffeine"), KernelColorType.Tip);
                        TextWriters.WriteList(Caffeines);
                        return 26;
                    }
                    alarmSeconds = alarmSpecifier.Item2;
                    secsOrName = alarmSpecifier.Item1;
                }
                AlarmTools.StartAlarm("Caffeine", LanguageTools.GetLocalized("NKS_CAFFEINE_ALARMNAME", "Nitrocid.Extras.Caffeine"), alarmSeconds, nameSpecified ? secsOrName : "");
            }
            return 0;
        }
    }
}
