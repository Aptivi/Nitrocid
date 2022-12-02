
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
using System.Linq;

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
            { "Default", new DefaultRandom() },

#if !SPECIFIERREL
            // Below are excluded from the final release
            { "DefaultDebug", new DefaultRandomDebug() }
#endif
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

        private readonly static Dictionary<string, IRandomDriver> customRandomDrivers = new();
        private readonly static Dictionary<string, IConsoleDriver> customConsoleDrivers = new();

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

        /// <summary>
        /// Registers the driver
        /// </summary>
        /// <param name="type">Type of driver to register</param>
        /// <param name="driver">Driver to be registered</param>
        public static void RegisterDriver(DriverTypes type, IDriver driver)
        {
            string name = driver.DriverName;
            switch (type)
            {
                case DriverTypes.RNG:
                    if (!IsRegistered(type, name))
                        customRandomDrivers.Add(name, (IRandomDriver)driver);
                    break;
                case DriverTypes.Console:
                    if (!IsRegistered(type, name))
                        customConsoleDrivers.Add(name, (IConsoleDriver)driver);
                    break;
            }
        }

        /// <summary>
        /// Unregisters the driver
        /// </summary>
        /// <param name="type">Type of driver to unregister</param>
        /// <param name="name">Driver name to be unregistered</param>
        public static void UnregisterDriver(DriverTypes type, string name)
        {
            switch (type)
            {
                case DriverTypes.RNG:
                    if (IsRegistered(type, name))
                        customRandomDrivers.Remove(name);
                    break;
                case DriverTypes.Console:
                    if (IsRegistered(type, name))
                        customConsoleDrivers.Remove(name);
                    break;
            }
        }

        /// <summary>
        /// Is the driver registered?
        /// </summary>
        /// <param name="type">Driver type</param>
        /// <param name="name">Driver name</param>
        /// <returns>True if registered. Otherwise, false.</returns>
        public static bool IsRegistered(DriverTypes type, string name)
        {
            return type switch
            {
                DriverTypes.RNG     => customRandomDrivers.Keys.Contains(name),
                DriverTypes.Console => customConsoleDrivers.Keys.Contains(name),
                _                   => false,
            };
        }

        #region Individual driver getters
        internal static IRandomDriver GetRandomDriver() =>
            GetRandomDriver(currentRandomDriver);

        internal static IRandomDriver GetRandomDriver(string name)
        {
            // Try to get the driver from the name.
            bool found = randomDrivers.TryGetValue(name, out IRandomDriver rdriver);
            bool customFound = customRandomDrivers.TryGetValue(name, out IRandomDriver customrdriver);

            // If found, bail.
            if (found)
            {
                return rdriver;
            }
            else if (customFound)
            {
                return customrdriver;
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
            bool customFound = customConsoleDrivers.TryGetValue(name, out IConsoleDriver customcdriver);

            // If found, bail.
            if (found)
            {
                return cdriver;
            }
            else if (customFound)
            {
                return customcdriver;
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
