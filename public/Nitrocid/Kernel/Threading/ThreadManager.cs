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

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Extensions;
using Terminaux.Base;
using Terminaux.Inputs;

namespace Nitrocid.Kernel.Threading
{
    /// <summary>
    /// Thread management module
    /// </summary>
    public static class ThreadManager
    {

        internal static List<KernelThread> kernelThreads = [];

        /// <summary>
        /// Gets the operating system threads that Nitrocid makes use of within the host OS
        /// </summary>
        public static ProcessThreadCollection OperatingSystemThreads =>
            Process.GetCurrentProcess().Threads;

        /// <summary>
        /// Gets all the kernel threads
        /// </summary>
        public static List<KernelThread> KernelThreads =>
            kernelThreads;

        /// <summary>
        /// Gets active threads
        /// </summary>
        public static List<KernelThread> ActiveThreads =>
            kernelThreads.Where(x => x.IsAlive).ToList();

        /// <summary>
        /// Stops all threads
        /// </summary>
        internal static void StopAllThreads()
        {
            foreach (KernelThread KernelThread in KernelThreads)
                KernelThread.Stop();
        }

        /// <summary>
        /// Sleeps until a key is input.
        /// </summary>
        public static void SleepUntilInput() =>
            Input.ReadPointerOrKey();

        /// <summary>
        /// Sleeps until either the time specified, or a key is input.
        /// </summary>
        /// <param name="Time">Time in milliseconds</param>
        public static bool SleepUntilInput(long Time)
        {
            bool result = SpinWait.SpinUntil(() =>
            {
                var key = Input.ReadPointerOrKeyNoBlock();
                return key.EventType == InputEventType.Mouse || key.EventType == InputEventType.Keyboard;
            }, (int)Time);
            return result;
        }

        /// <summary>
        /// Sleeps until either the time specified, or the current thread is no longer alive.
        /// </summary>
        /// <param name="Time">Time in milliseconds</param>
        public static bool SleepNoBlock(long Time) =>
            Thread.CurrentThread.Join((int)Time);

        /// <summary>
        /// Sleeps until either the time specified, or the thread is no longer alive.
        /// </summary>
        /// <param name="Time">Time in milliseconds</param>
        /// <param name="ThreadWork">The working thread</param>
        public static bool SleepNoBlock(long Time, Thread ThreadWork) =>
            ThreadWork.Join((int)Time);

        /// <summary>
        /// Sleeps until either the time specified, or the thread is no longer alive.
        /// </summary>
        /// <param name="Time">Time in milliseconds</param>
        /// <param name="ThreadWork">The working thread</param>
        public static bool SleepNoBlock(long Time, KernelThread ThreadWork) =>
            ThreadWork.Wait((int)Time);

        /// <summary>
        /// Gets the actual milliseconds time from the sleep time provided
        /// </summary>
        /// <param name="Time">Sleep time</param>
        /// <returns>How many milliseconds did it really sleep?</returns>
        public static int GetActualMilliseconds(int Time)
        {
            var SleepStopwatch = new Stopwatch();
            SleepStopwatch.Start();
            Thread.Sleep(Time);
            SleepStopwatch.Stop();
            return (int)SleepStopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Gets the actual ticks from the sleep time provided
        /// </summary>
        /// <param name="Time">Sleep time</param>
        /// <returns>How many ticks did it really sleep?</returns>
        public static long GetActualTicks(int Time)
        {
            var SleepStopwatch = new Stopwatch();
            SleepStopwatch.Start();
            Thread.Sleep(Time);
            SleepStopwatch.Stop();
            return SleepStopwatch.ElapsedTicks;
        }

        /// <summary>
        /// Gets the actual time span from the sleep time provided
        /// </summary>
        /// <param name="Time">Sleep time</param>
        /// <returns>The time span which describes the actual time span from the sleep time provided</returns>
        public static TimeSpan GetActualTimeSpan(int Time)
        {
            var SleepStopwatch = new Stopwatch();
            SleepStopwatch.Start();
            Thread.Sleep(Time);
            SleepStopwatch.Stop();
            return SleepStopwatch.Elapsed;
        }

        /// <summary>
        /// Gets all thread backtraces
        /// </summary>
        /// <returns>A dictionary containing thread names and addresses as keys and stack traces as values</returns>
        public static Dictionary<string, string[]> GetThreadBacktraces()
        {
            try
            {
                var addonType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasDiagnostics, "Nitrocid.Extras.Diagnostics.Tools.DiagnosticsTools");
                var resultObj = InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasDiagnostics, nameof(GetThreadBacktraces), addonType);
                if (resultObj is Dictionary<string, string[]> result)
                    return result;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Can't get thread backtraces: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return [];
        }

    }
}
