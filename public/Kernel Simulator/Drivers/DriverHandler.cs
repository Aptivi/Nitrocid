
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Drivers.RNG;
using KS.Drivers.Console;
using KS.Kernel.Debugging;
using System.Collections.Generic;
using KS.Drivers.Console.Consoles;
using KS.Drivers.RNG.Randoms;

namespace KS.Drivers
{
    /// <summary>
    /// The driver handler routine class
    /// </summary>
    public static class DriverHandler
    {
        internal static string currentRandomDriver = "Default";
        internal static string currentConsoleDriver = "Terminal";

        private readonly static Dictionary<string, IRandomDriver> randomDrivers = new()
        {
            { "Default", new DefaultRandom() }
        };

        private readonly static Dictionary<string, IConsoleDriver> consoleDrivers = new()
        {
            { "Terminal", new Terminal() },
            { "File", new File() },
            { "Null", new Null() },

#if !SPECIFIERREL
            // Below are excluded from the final release
            { "TerminalDebug", new TerminalDebug() }
#endif
        };

        /// <summary>
        /// Gets the current random driver
        /// </summary>
        public static IRandomDriver CurrentRandomDriver => 
            GetRandomDriver();

        /// <summary>
        /// Gets the current console driver
        /// </summary>
        public static IConsoleDriver CurrentConsoleDriver => 
            GetConsoleDriver();

        /// <summary>
        /// Gets the driver
        /// </summary>
        /// <param name="type">The driver type</param>
        /// <param name="name">The driver name</param>
        /// <returns>The driver responsible for performing operations according to driver <paramref name="type"/></returns>
        public static IDriver GetDriver(DriverTypes type, string name)
        {
            switch (type)
            {
                case DriverTypes.RNG:
                    return GetRandomDriver(name);
                case DriverTypes.Console:
                    return GetConsoleDriver(name);
            }

            // We shouldn't be here
            DebugWriter.WriteDebug(DebugLevel.E, "We shouldn't be returning null here. Are you sure that it's of type {0} with name {1}?", type, name);
            return null;
        }

        #region Individual driver getters
        internal static IRandomDriver GetRandomDriver() =>
            GetRandomDriver(currentRandomDriver);

        internal static IRandomDriver GetRandomDriver(string name)
        {
            // Try to get the driver from the name.
            bool found = randomDrivers.TryGetValue(name, out IRandomDriver rdriver);

            // If found, bail.
            if (found)
            {
                return rdriver;
            }
            else
            {
                // We didn't find anything, so return default KS driver.
                DebugWriter.WriteDebug(DebugLevel.W, "Got default kernel driver because {0} is not found in the driver database.", name);
                return randomDrivers["Default"];
            }
        }

        internal static IConsoleDriver GetConsoleDriver() =>
            GetConsoleDriver(currentConsoleDriver);

        internal static IConsoleDriver GetConsoleDriver(string name)
        {
            // Try to get the driver from the name.
            bool found = consoleDrivers.TryGetValue(name, out IConsoleDriver cdriver);

            // If found, bail.
            if (found)
            {
                return cdriver;
            }
            else
            {
                // We didn't find anything, so return default KS driver.
                DebugWriter.WriteDebug(DebugLevel.W, "Got default kernel driver because {0} is not found in the driver database.", name);
                return consoleDrivers["Terminal"];
            }
        }
        #endregion

    }
}
