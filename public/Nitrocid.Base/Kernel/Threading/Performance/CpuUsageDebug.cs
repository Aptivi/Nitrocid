//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Kernel.Time;
using Nitrocid.Base.Kernel.Time.Renderers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Nitrocid.Base.Kernel.Threading.Performance
{
    internal static class CpuUsageDebug
    {
        internal static bool usageUpdateEnabled;
        internal static int usageIntervalUpdatePeriod = 1000;
        private static DateTime previousDate = TimeDateTools.KernelDateTimeUtc;
        private static TimeSpan previousProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
        private static readonly KernelThread usageUpdateThread = new("CPU Usage Update Thread", true, HandleCpuUsagePrint);

        internal static void RunCpuUsageDebugger()
        {
            if (!usageUpdateThread.IsAlive && usageUpdateEnabled && KernelEntry.DebugMode)
                usageUpdateThread.Start();
        }

        private static void HandleCpuUsagePrint()
        {
            var threadsUsages = new List<(int, DateTime, TimeSpan)>();
            while (!PowerManager.KernelShutdown && usageUpdateEnabled)
            {
                // First, get the CPU usage
                var currentProcess = Process.GetCurrentProcess();
                var oldDate = previousDate;
                var oldCpuTime = previousProcessorTime;
                var newDate = TimeDateTools.KernelDateTimeUtc;
                var newCpuTime = currentProcess.TotalProcessorTime;
                var usedMilliseconds = (newCpuTime - oldCpuTime).TotalMilliseconds;
                var totalMilliseconds = (newDate - oldDate).TotalMilliseconds;
                var totalUsage = usedMilliseconds / (Environment.ProcessorCount * totalMilliseconds);

                // Then, write the output
                var totalUsagePercent = totalUsage * 100;
                DebugWriter.WriteDebug(DebugLevel.I, $"CPU usage from {TimeDateRenderers.Render(oldDate, FormatType.Short)} to {TimeDateRenderers.Render(newDate, FormatType.Short)}: {totalUsagePercent}%");

                // Then, write the thread usages
                var processedThreads = new List<(int, DateTime, TimeSpan)>();
                int idx = 0;
                foreach (ProcessThread thread in currentProcess.Threads)
                {
                    try
                    {
                        // First, get the necessary values
                        var oldDateThread = threadsUsages.Count > idx ? threadsUsages[idx].Item2 : TimeDateTools.KernelDateTimeUtc;
                        var oldCpuTimeThread = threadsUsages.Count > idx ? threadsUsages[idx].Item3 : thread.TotalProcessorTime;
                        var newDateThread = TimeDateTools.KernelDateTimeUtc;
                        var newCpuTimeThread = thread.TotalProcessorTime;
                        var usedMillisecondsThread = (newCpuTimeThread - oldCpuTimeThread).TotalMilliseconds;
                        var totalMillisecondsThread = (newDateThread - oldDateThread).TotalMilliseconds;
                        var totalUsageThread = usedMillisecondsThread / (Environment.ProcessorCount * totalMillisecondsThread);

                        // Write the output
                        var totalUsagePercentThread = totalUsageThread * 100;
                        DebugWriter.WriteDebug(DebugLevel.I, $"  - Thread [{thread.Id}]: CPU usage from {TimeDateRenderers.Render(oldDateThread, FormatType.Short)} to {TimeDateRenderers.Render(newDateThread, FormatType.Short)}: {totalUsagePercentThread}%");

                        // Add the processed thread
                        processedThreads.Add((idx, newDateThread, newCpuTimeThread));
                    }
                    catch (InvalidOperationException ioex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, $"  - Thread [{thread.Id}]: Can't get CPU usage: {ioex.Message}");
                    }
                    finally
                    {
                        idx++;
                    }
                }
                threadsUsages = processedThreads;

                // Finally, update the cache and wait  until the shutdown is requested.
                previousDate = newDate;
                previousProcessorTime = newCpuTime;
                SpinWait.SpinUntil(() => PowerManager.KernelShutdown || !usageUpdateEnabled, usageIntervalUpdatePeriod);
            }
        }
    }
}
