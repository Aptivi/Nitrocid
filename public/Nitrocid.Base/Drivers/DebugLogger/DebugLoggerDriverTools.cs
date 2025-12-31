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

using Nitrocid.Base.Kernel.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Base.Drivers.DebugLogger
{
    /// <summary>
    /// DebugLogger driver tools (strongly typed)
    /// </summary>
    public static class DebugLoggerDriverTools
    {
        /// <summary>
        /// Sets the regular expression driver
        /// </summary>
        /// <param name="name">Name of the available regular expression driver to set to</param>
        /// <exception cref="KernelException"></exception>
        public static void SetDebugLoggerDriver(string name) =>
            DriverHandler.SetDriverSafe<IDebugLoggerDriver>(name);

        /// <summary>
        /// Gets the regular expression drivers
        /// </summary>
        /// <exception cref="KernelException"></exception>
        public static List<IDriver> GetDebugLoggerDrivers()
        {
            // First, exclude internal drivers from the list
            var filteredDrivers = DriverHandler.drivers[DriverTypes.DebugLogger].Where((kvp) => !kvp.DriverInternal);
            var filteredCustomDrivers = DriverHandler.customDrivers[DriverTypes.DebugLogger].Where((kvp) => !kvp.DriverInternal);

            // Then, get the list of drivers
            return filteredDrivers.Union(filteredCustomDrivers).ToList();
        }

        /// <summary>
        /// Gets the regular expression driver names
        /// </summary>
        /// <returns>List of driver names</returns>
        public static string[] GetDebugLoggerDriverNames()
        {
            // Get the drivers and fetch their names
            var drivers = GetDebugLoggerDrivers();
            return drivers.Select((kvp) => kvp.DriverName).ToArray();
        }
    }
}
