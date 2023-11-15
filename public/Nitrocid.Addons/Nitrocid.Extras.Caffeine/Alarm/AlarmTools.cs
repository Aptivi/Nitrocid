//
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

using KS.Kernel.Time;
using System;
using System.Collections.Generic;

namespace Nitrocid.Extras.Caffeine.Alarm
{
    internal static class AlarmTools
    {
        internal static Dictionary<string, DateTime> alarms = [];

        internal static void StartAlarm(string alarmName, int alarmValue)
        {
            // Check the alarm value
            if (alarmValue < 0)
                alarmValue = 1;
            if (alarms.ContainsKey(alarmName))
                alarmName += $" [{alarms.Count}]";

            // Now, start the alarm
            var alarmDate = TimeDateTools.KernelDateTime.AddSeconds(alarmValue);
            alarms.Add(alarmName, alarmDate);
        }
    }
}
