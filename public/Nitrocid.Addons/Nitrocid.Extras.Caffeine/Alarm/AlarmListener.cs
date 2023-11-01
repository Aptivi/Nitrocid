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

using KS.Kernel.Debugging;
using KS.Kernel.Power;
using KS.Kernel.Threading;
using KS.Kernel.Time;
using KS.Languages;
using KS.Misc.Notifications;
using System;
using System.Collections.Generic;

namespace Nitrocid.Extras.Caffeine.Alarm
{
    internal static class AlarmListener
    {
        private static KernelThread alarmThread = new("Alarm Listener Thread", true, HandleAlarms);

        internal static void StartListener()
        {
            if (!alarmThread.IsAlive)
                alarmThread.Start();
        }

        internal static void StopListener()
        {
            if (alarmThread.IsAlive)
                alarmThread.Stop();
        }

        private static void HandleAlarms()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, $"Alarm listener started");

                // Loop through all the alarms
                List<string> notifiedAlarms = new();
                while (!PowerManager.RebootRequested)
                {
                    ThreadManager.SleepNoBlock(1);

                    // Fetch all alarms
                    foreach (var alarm in AlarmTools.alarms)
                    {
                        // Get the current date and time for comparison
                        var date = TimeDateTools.KernelDateTime;
                        if (date >= alarm.Value && !notifiedAlarms.Contains(alarm.Key))
                        {
                            // The alarm has been fired! Send a notification
                            notifiedAlarms.Add(alarm.Key);
                            var alarmNotif = new Notification(
                                Translate.DoTranslation("Alarm fired!"),
                                alarm.Key,
                                NotificationPriority.High, NotificationType.Normal
                            );
                            NotificationManager.NotifySend(alarmNotif);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Stopping the alarm listener: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }
    }
}
