
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
using KS.Kernel.Debugging;
using System;
using System.Collections.Generic;

namespace KS.Drivers
{
    /// <summary>
    /// The driver handler routine class
    /// </summary>
    public static class DriverHandler
    {
        internal static string currentRandomDriver = "Default";

        private static Dictionary<string, IRandomDriver> randomDrivers = new()
        {
            { "Default", new DefaultRandom() }
        };

        /// <summary>
        /// Gets the current random driver
        /// </summary>
        public static IRandomDriver CurrentRandomDriver { get => (IRandomDriver)GetDriver(DriverTypes.RNG, currentRandomDriver); }

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
                    // Try to get the driver from the name.
                    bool found = randomDrivers.TryGetValue(name, out IRandomDriver driver);

                    // If found, bail.
                    if (found)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Got current driver of type {0}: {1}", type.ToString(), name);
                        return driver;
                    }
                    break;
            }

            // We didn't find anything, so return default KS driver.
            DebugWriter.WriteDebug(DebugLevel.W, "Got default kernel driver of type {0} because {1} is not found on {0}'s driver database.", type.ToString(), name);
            return randomDrivers["Default"];
        }

    }
}
