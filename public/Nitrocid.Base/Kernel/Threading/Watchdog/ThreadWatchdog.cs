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
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Nitrocid.Base.Kernel.Threading.Watchdog
{
    internal static class ThreadWatchdog
    {
        private static readonly KernelThread watchdogThread = new("Kernel thread watchdog thread", true, Watch) { isCritical = true };
        private static readonly string[] whitelistedThreads =
        [
            "Notification Thread",
            "Remote Debug Chat Thread",
            "Remote Debug Thread",
            "Screensaver timeout thread",
            "RPC Thread"
        ];

        internal static void StartWatchdog()
        {
            if (!watchdogThread.IsAlive)
                watchdogThread.Start();
        }

        internal static KernelThread[] GetCriticalThreads() =>
            ThreadManager.KernelThreads.Where((thread) => thread.IsCritical).ToArray();

        internal static void EnsureAllCriticalThreadsStarted()
        {
            var threads = GetCriticalThreads();

            // Check to see if all the critical threads have started
            var unstartedCriticals = threads.Where((thread) => !thread.IsAlive && !whitelistedThreads.Contains(thread.Name)).ToArray();
            if (unstartedCriticals.Length > 0)
                KernelPanic.KernelError(KernelErrorLevel.U, true, 5, LanguageTools.GetLocalized("NKS_KERNEL_THREADING_WATCHDOG_CRITICALSNOSTART") + " [{1}]", null, unstartedCriticals.Length, string.Join(", ", unstartedCriticals.Select((thread) => $"{thread.Name} [{thread.BaseThread.ThreadState}]")));
        }

        private static void Watch()
        {
            try
            {
                while (!PowerManager.RebootRequested)
                {
                    // Get the list of threads and supervise them
                    var threads = GetCriticalThreads();
                    var deadThreads = new List<KernelThread>();
                    foreach (var thread in threads)
                    {
                        // Don't check threads that haven't started yet. Watchdog can run at early boot.
                        if (thread.BaseThread.ThreadState.HasFlag(ThreadState.Unstarted))
                            continue;

                        // Now, check the thread states
                        if (!thread.IsAlive && !thread.IsStopping)
                            deadThreads.Add(thread);
                    }

                    // Check to see if we have dead threads
                    if (deadThreads.Count > 0)
                        KernelPanic.KernelError(KernelErrorLevel.U, true, 5, LanguageTools.GetLocalized("NKS_KERNEL_THREADING_WATCHDOG_DEADTHREADS") + " [{1}]", null, deadThreads.Count, string.Join(", ", deadThreads.Select((thread) => thread.Name)));

                    // Sleep to avoid CPU usage.
                    Thread.Sleep(100);
                }
            }
            catch (ThreadInterruptedException ex)
            {
                // Watchdog interrupted
                DebugWriter.WriteDebug(DebugLevel.W, "Kernel thread supervisor (watchdog) is stopping: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            catch (Exception ex)
            {
                // Watchdog error, so reboot
                DebugWriter.WriteDebug(DebugLevel.F, "Kernel thread supervisor (watchdog) failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelPanic.KernelError(KernelErrorLevel.U, true, 5, LanguageTools.GetLocalized("NKS_KERNEL_THREADING_WATCHDOG_FAILURE") + ": {0}", ex, ex.Message);
            }
        }
    }
}
