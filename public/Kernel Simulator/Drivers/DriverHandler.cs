
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
            { "Null", new Null() }
        };

        /// <summary>
        /// Gets the current random driver
        /// </summary>
        public static IRandomDriver CurrentRandomDriver { get => (IRandomDriver)GetDriver(DriverTypes.RNG, currentRandomDriver); }

        /// <summary>
        /// Gets the current console driver
        /// </summary>
        public static IConsoleDriver CurrentConsoleDriver { get => (IConsoleDriver)GetDriver(DriverTypes.Console, currentConsoleDriver); }

        /// <summary>
        /// Gets the driver
        /// </summary>
        /// <param name="type">The driver type</param>
        /// <param name="name">The driver name</param>
        /// <returns>The driver responsible for performing operations according to driver <paramref name="type"/></returns>
        public static IDriver GetDriver(DriverTypes type, string name)
        {
            bool found;
            switch (type)
            {
                case DriverTypes.RNG:
                    // Try to get the driver from the name.
                    found = randomDrivers.TryGetValue(name, out IRandomDriver rdriver);

                    // If found, bail.
                    if (found)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Got current driver of type {0}: {1}", type.ToString(), name);
                        return rdriver;
                    }
                    else
                    {
                        // We didn't find anything, so return default KS driver.
                        DebugWriter.WriteDebug(DebugLevel.W, "Got default kernel driver of type {0} because {1} is not found on {0}'s driver database.", type.ToString(), name);
                        return randomDrivers["Default"];
                    }
                case DriverTypes.Console:
                    // Try to get the driver from the name.
                    found = consoleDrivers.TryGetValue(name, out IConsoleDriver cdriver);

                    // If found, bail.
                    if (found)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Got current driver of type {0}: {1}", type.ToString(), name);
                        return cdriver;
                    }
                    else
                    {
                        // We didn't find anything, so return default KS driver.
                        DebugWriter.WriteDebug(DebugLevel.W, "Got default kernel driver of type {0} because {1} is not found on {0}'s driver database.", type.ToString(), name);
                        return consoleDrivers["Terminal"];
                    }
            }

            // We shouldn't be here
            DebugWriter.WriteDebug(DebugLevel.E, "We shouldn't be returning null here. Are you sure that it's of type {0} with name {1}?", type, name);
            return null;
        }

    }
}
