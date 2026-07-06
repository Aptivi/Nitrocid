//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using Nitrocid.Base.Kernel.Debugging;

#if NKS_EXTENSIONS
using Nitrocid.Base.Kernel.Extensions;
#endif

namespace Nitrocid.Base.Kernel.Threading.Watchdog
{
    /// <summary>
    /// Kernel backtrace manager
    /// </summary>
    public static class ThreadBacktraces
    {
        /// <summary>
        /// Gets all thread backtraces
        /// </summary>
        /// <returns>A dictionary containing thread names and addresses as keys and stack traces as values</returns>
        public static Dictionary<string, string[]> GetThreadBacktraces()
        {
#if NKS_EXTENSIONS
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
#endif
            return [];
        }
    }
}
