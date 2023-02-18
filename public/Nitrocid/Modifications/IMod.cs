
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

using System;
using System.Collections.Generic;
using KS.Kernel.Events;
using KS.Shell.ShellBase.Commands;

namespace KS.Modifications
{
    /// <summary>
    /// Interface for mods
    /// </summary>
    public interface IMod
    {
        /// <summary>
        /// List of commands for mod
        /// </summary>
        Dictionary<string, CommandInfo> Commands { get; set; }
        /// <summary>
        /// Mod name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Name of part of mod
        /// </summary>
        string ModPart { get; set; }
        /// <summary>
        /// Mod version
        /// </summary>
        string Version { get; set; }
        /// <summary>
        /// Minimum supported API version that the mod supports
        /// </summary>
        Version MinimumSupportedApiVersion { get; }
        /// <summary>
        /// Code executed when starting mod
        /// </summary>
        void StartMod();
        /// <summary>
        /// Code executed when stopping mod
        /// </summary>
        void StopMod();
        /// <summary>
        /// Code executed when initializing events
        /// </summary>
        /// <param name="Event">Event</param>
        void InitEvents(EventType Event);
        /// <summary>
        /// Code executed when initializing events
        /// </summary>
        /// <param name="Event">Event</param>
        /// <param name="Args">Arguments</param>
        void InitEvents(EventType Event, params object[] Args);
    }
}
