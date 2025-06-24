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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Base.Kernel.Time.Alarm
{
    /// <summary>
    /// Alarm tools
    /// </summary>
    public static class AlarmTools
    {
        internal static Dictionary<string, AlarmEntry> alarms = [];

        /// <summary>
        /// Checks to see if the alarm is registered
        /// </summary>
        /// <param name="alarmId">String identifier of an alarm</param>
        /// <returns>True if found, false if not.</returns>
        public static bool IsAlarmRegistered(string alarmId)
        {
            // Check the alarm name
            if (string.IsNullOrEmpty(alarmId))
                throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_ALARM_EXCEPTION_NONAME"));

            // Now, check the ID
            return alarms.Any((kvp) => kvp.Key == alarmId);
        }

        /// <summary>
        /// Gets an alarm from ID
        /// </summary>
        /// <param name="alarmId">String identifier of an alarm</param>
        /// <returns>A key value pair instance of the whole alarm.</returns>
        public static KeyValuePair<string, AlarmEntry> GetAlarmFromId(string alarmId)
        {
            // Check the alarm name
            if (string.IsNullOrEmpty(alarmId))
                throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_ALARM_EXCEPTION_NONAME"));
            if (!IsAlarmRegistered(alarmId))
                throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_ALARM_EXCEPTION_NOALARM"));

            // Now, check the ID
            return alarms.First((kvp) => kvp.Key == alarmId);
        }

        /// <summary>
        /// Starts an alarm
        /// </summary>
        /// <param name="alarmId">String identifier of the alarm</param>
        /// <param name="alarmName">Name of the alarm</param>
        /// <param name="alarmDesc">Description of the alarm</param>
        /// <param name="alarmValue">Alarm value</param>
        public static void StartAlarm(string alarmId, string alarmName, int alarmValue, string alarmDesc = "")
        {
            // Check the alarm value
            if (alarmValue < 0)
                alarmValue = 1;
            if (IsAlarmRegistered(alarmId))
                alarmId += $" [{alarms.Count}]";

            // Now, start the alarm
            var alarmDate = TimeDateTools.KernelDateTime.AddSeconds(alarmValue);
            if (alarmDate < TimeDateTools.KernelDateTime)
                throw new KernelException(KernelExceptionType.Alarm, LanguageTools.GetLocalized("NKS_KERNEL_TIME_ALARM_EXCEPTION_LESSTHANPRESENT"));
            var entryInstance = new AlarmEntry(alarmName, alarmDesc, alarmDate);
            alarms.Add(alarmId, entryInstance);
        }

        /// <summary>
        /// Stops an alarm
        /// </summary>
        /// <param name="alarmIdx">Index of an alarm</param>
        public static void StopAlarm(int alarmIdx)
        {
            // Check the alarm index value
            if (alarmIdx < 0)
                alarmIdx = 0;
            if (alarmIdx >= alarms.Count)
                throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_ALARM_EXCEPTION_INVALIDALARMNUM"));

            // Now, get an alarm and stop it
            var alarm = alarms.ElementAt(alarmIdx);
            StopAlarm(alarm.Key);
        }

        /// <summary>
        /// Stops an alarm
        /// </summary>
        /// <param name="alarmId">String identifier of an alarm</param>
        public static void StopAlarm(string alarmId)
        {
            // Check the alarm name
            if (string.IsNullOrEmpty(alarmId))
                throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_ALARM_EXCEPTION_NONAME"));
            if (!IsAlarmRegistered(alarmId))
                throw new KernelException(KernelExceptionType.TimeDate, LanguageTools.GetLocalized("NKS_KERNEL_TIME_ALARM_EXCEPTION_NOALARM"));

            // Now, get an alarm and stop it
            if (alarms.Remove(alarmId))
                AlarmListener.hasRemovedAlarm = true;
        }
    }
}
