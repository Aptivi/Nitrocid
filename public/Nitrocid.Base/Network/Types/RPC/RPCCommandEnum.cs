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

namespace Nitrocid.Base.Network.Types.RPC
{
    /// <summary>
    /// Enumeration for RPC command
    /// </summary>
    public enum RPCCommandEnum
    {
        /// <summary>
        /// &lt;Request:Shutdown&gt;: Shuts down the remote kernel. Usage: &lt;Request:Shutdown&gt;(IP)
        /// </summary>
        Shutdown,
        /// <summary>
        /// &lt;Request:Reboot&gt;: Reboots the remote kernel. Usage: &lt;Request:Reboot&gt;(IP)
        /// </summary>
        Reboot,
        /// <summary>
        /// &lt;Request:RebootSafe&gt;: Reboots the remote kernel to safe mode. Usage: &lt;Request:RebootSafe&gt;(IP)
        /// </summary>
        RebootSafe,
        /// <summary>
        /// &lt;Request:RebootMaintenance&gt;: Reboots the remote kernel to maintenance mode. Usage: &lt;Request:RebootMaintenance&gt;(IP)
        /// </summary>
        RebootMaintenance,
        /// <summary>
        /// &lt;Request:RebootDebug&gt;: Reboots the remote kernel to debug. Usage: &lt;Request:RebootDebug&gt;(IP)
        /// </summary>
        RebootDebug,
        /// <summary>
        /// &lt;Request:SaveScr&gt;: Saves the screen remotely. Usage: &lt;Request:SaveScr&gt;(IP)
        /// </summary>
        SaveScr,
        /// <summary>
        /// &lt;Request:Exec&gt;: Executes a command remotely. Usage: &lt;Request:Exec&gt;(Command)
        /// </summary>
        Exec,
        /// <summary>
        /// &lt;Request:Acknowledge&gt;: Pings the remote kernel silently. Usage: &lt;Request:Acknowledge&gt;(IP)
        /// </summary>
        Acknowledge,
        /// <summary>
        /// &lt;Request:Ping&gt;: Pings the remote kernel with notification. Usage: &lt;Request:Ping&gt;(IP)
        /// </summary>
        Ping,
        /// <summary>
        /// &lt;Request:Version&gt;: Returns the Nitrocid version. Usage: &lt;Request:Version&gt;(IP)
        /// </summary>
        Version,
        /// <summary>
        /// &lt;Request:VersionCode&gt;: Returns the Nitrocid version code. Usage: &lt;Request:VersionCode&gt;(IP)
        /// </summary>
        VersionCode,
        /// <summary>
        /// &lt;Request:ApiVersion&gt;: Returns the Nitrocid mod API version. Usage: &lt;Request:ApiVersion&gt;(IP)
        /// </summary>
        ApiVersion,
        /// <summary>
        /// &lt;Request:ApiVersionCode&gt;: Returns the Nitrocid mod API version code. Usage: &lt;Request:ApiVersionCode&gt;(IP)
        /// </summary>
        ApiVersionCode,
    }
}
