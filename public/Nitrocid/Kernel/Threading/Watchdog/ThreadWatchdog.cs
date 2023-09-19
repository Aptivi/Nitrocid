
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KS.Kernel.Threading.Watchdog
{
    internal static class ThreadWatchdog
    {
        private static readonly KernelThread watchdogThread = new("Kernel thread watchdog thread", true, Watch) { isCritical = true };

        internal static void StartWatchdog()
        {
            if (!watchdogThread.IsAlive)
                watchdogThread.Start();
        }

        private static void Watch()
        {
            try
            {
                while (!Flags.RebootRequested)
                {
                    // Get the list of threads and supervise them
                    var threads = ThreadManager.KernelThreads.Where((thread) => thread.IsCritical).ToArray();
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
                        KernelPanic.KernelError(KernelErrorLevel.U, true, 5, Translate.DoTranslation("Kernel thread supervisor detected {0} dead threads.") + " [{1}]", null, deadThreads.Count, deadThreads.Select((thread) => thread.Name));

                    // Sleep to avoid CPU usage.
                    Thread.Sleep(100);
                }
            }
            catch (ThreadInterruptedException ex)
            {
                // Watchdog interrupted
                DebugWriter.WriteDebug(DebugLevel.W, "Kernel thread supervisor (watchdog) is stopping: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            catch (Exception ex)
            {
                // Watchdog error, so reboot
                DebugWriter.WriteDebug(DebugLevel.F, "Kernel thread supervisor (watchdog) failed: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                KernelPanic.KernelError(KernelErrorLevel.U, true, 5, Translate.DoTranslation("Kernel thread supervisor failed") + ": {0}", ex, ex.Message);
            }
        }
    }
}
