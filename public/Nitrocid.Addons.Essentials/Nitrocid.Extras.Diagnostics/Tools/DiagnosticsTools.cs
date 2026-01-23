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

using Microsoft.Diagnostics.Runtime;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Diagnostics.Tools
{
    /// <summary>
    /// Diagnostics tools
    /// </summary>
    public static class DiagnosticsTools
    {
        /// <summary>
        /// Gets all thread backtraces
        /// </summary>
        /// <returns>A dictionary containing thread names and addresses as keys and stack traces as values</returns>
        public static Dictionary<string, string[]> GetThreadBacktraces()
        {
            var result = new Dictionary<string, string[]>();
            var pid = Environment.ProcessId;
            using var dataTarget = DataTarget.CreateSnapshotAndAttach(pid);
            ClrInfo runtimeInfo = dataTarget.ClrVersions[0];
            var runtime = runtimeInfo.CreateRuntime();
            foreach (var t in runtime.Threads)
            {
                var matchingThreads = ThreadManager.KernelThreads.Where((thread) => thread.ThreadId == t.ManagedThreadId).ToArray();
                string threadName = matchingThreads.Length > 0 ? matchingThreads[0].Name : Translate.DoTranslation("Not a Nitrocid KS thread");
                string[] trace = t.EnumerateStackTrace(true).Select(f =>
                {
                    if (f.Method != null)
                        return $"[0x{f.Method.NativeCode:X16}] @ {f.Method.Signature}";
                    return "[0x????????????????] @ ???";
                }
                ).ToArray();
                if (trace.Length > 0)
                    result.Add($"{threadName} [{t.ManagedThreadId}] @ 0x{t.Address:X16}", trace);
            }
            return result;
        }
    }
}
