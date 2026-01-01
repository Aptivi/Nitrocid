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

namespace Nitrocid.Kernel.Power
{
    /// <summary>
    /// Kernel power modes
    /// </summary>
    public enum PowerMode
    {
        /// <summary>
        /// Kernel will shut down
        /// </summary>
        Shutdown,
        /// <summary>
        /// Kernel will reboot
        /// </summary>
        Reboot,
        /// <summary>
        /// Kernel will reboot to safe mode (disables all mods)
        /// </summary>
        RebootSafe,
        /// <summary>
        /// Kernel will reboot to maintenance mode (disables all mods and configs)
        /// </summary>
        RebootMaintenance,
        /// <summary>
        /// Kernel will reboot to debug
        /// </summary>
        RebootDebug,
        /// <summary>
        /// Kernel will remotely shutdown another kernel on the network (if RPC is running here and there)
        /// </summary>
        RemoteShutdown,
        /// <summary>
        /// Kernel will remotely reboot another kernel on the network (if RPC is running here and there)
        /// </summary>
        RemoteRestart,
        /// <summary>
        /// Kernel will remotely reboot another kernel to safe mode on the network (if RPC is running here and there)
        /// </summary>
        RemoteRestartSafe,
        /// <summary>
        /// Kernel will remotely reboot another kernel to safe mode on the network (if RPC is running here and there)
        /// </summary>
        RemoteRestartMaintenance,
        /// <summary>
        /// Kernel will remotely reboot another kernel to safe mode on the network (if RPC is running here and there)
        /// </summary>
        RemoteRestartDebug,
    }
}
