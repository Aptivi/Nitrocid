
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

using KS.Drivers.RNG;
using KS.Drivers.Console;
using System.Collections.Generic;
using KS.Drivers.Console.Consoles;
using KS.Drivers.RNG.Randoms;
using KS.Drivers.Network;
using KS.Drivers.Network.Bases;
using KS.Drivers.Filesystem;
using KS.Drivers.Filesystem.Bases;
using KS.Drivers.Encryption;
using KS.Drivers.Encryption.Encryptors;
using KS.Drivers.Regexp;
using KS.Drivers.Regexp.Bases;
using KS.Kernel.Exceptions;

namespace KS.Drivers
{
    /// <summary>
    /// The driver handler routine class
    /// </summary>
    public static class DriverHandler
    {
        internal static Dictionary<DriverTypes, Dictionary<string, IDriver>> drivers = new()
        {
            { 
                DriverTypes.Console, new()
                {
                    { "Default", new Terminal() },
                    { "File", new File() },
                    { "FileSequence", new FileSequence() },
                    { "Null", new Null() },

#if !SPECIFIERREL
                    // Below are excluded from the final release
                    { "TerminalDebug", new TerminalDebug() }
#endif
                }
            },
            { 
                DriverTypes.RNG, new()
                {
                    { "Default", new DefaultRandom() },
                    { "Standard", new StandardRandom() },

#if !SPECIFIERREL
                    // Below are excluded from the final release
                    { "DefaultDebug", new DefaultRandomDebug() },
                    { "StandardDebug", new StandardRandomDebug() }
#endif
                }
            },
            { 
                DriverTypes.Network, new()
                { 
                    { "Default", new DefaultNetwork() }
                }
            },
            { 
                DriverTypes.Filesystem, new()
                { 
                    { "Default", new DefaultFilesystem() }
                }
            },
            { 
                DriverTypes.Encryption, new()
                {
                    { "Default", new SHA256() },
                    { "CRC32", new CRC32() },
                    { "MD5", new MD5() },
                    { "SHA1", new SHA1() },
                    { "SHA256", new SHA256() },
                    { "SHA384", new SHA384() },
                    { "SHA512", new SHA512() }
                }
            },
            { 
                DriverTypes.Regexp, new()
                { 
                    { "Default", new DefaultRegexp() }
                }
            }
        };

        internal static Dictionary<DriverTypes, Dictionary<string, IDriver>> customDrivers = new()
        {
            { DriverTypes.Console,      new() },
            { DriverTypes.RNG,          new() },
            { DriverTypes.Network,      new() },
            { DriverTypes.Filesystem,   new() },
            { DriverTypes.Encryption,   new() },
            { DriverTypes.Regexp,       new() },
        };

        internal static Dictionary<DriverTypes, IDriver> currentDrivers = new()
        {
            { DriverTypes.Console,      drivers[DriverTypes.Console]["Default"] },
            { DriverTypes.RNG,          drivers[DriverTypes.RNG]["Default"] },
            { DriverTypes.Network,      drivers[DriverTypes.Network]["Default"] },
            { DriverTypes.Filesystem,   drivers[DriverTypes.Filesystem]["Default"] },
            { DriverTypes.Encryption,   drivers[DriverTypes.Encryption]["Default"] },
            { DriverTypes.Regexp,       drivers[DriverTypes.Regexp]["Default"] },
        };

        /// <summary>
        /// Gets the current random driver
        /// </summary>
        public static IRandomDriver CurrentRandomDriver =>
            (IRandomDriver)currentDrivers[DriverTypes.RNG];

        /// <summary>
        /// Gets the current console driver
        /// </summary>
        public static IConsoleDriver CurrentConsoleDriver =>
            (IConsoleDriver)currentDrivers[DriverTypes.Console];

        /// <summary>
        /// Gets the current network driver
        /// </summary>
        public static INetworkDriver CurrentNetworkDriver =>
            (INetworkDriver)currentDrivers[DriverTypes.Network];

        /// <summary>
        /// Gets the current filesystem driver
        /// </summary>
        public static IFilesystemDriver CurrentFilesystemDriver =>
            (IFilesystemDriver)currentDrivers[DriverTypes.Filesystem];

        /// <summary>
        /// Gets the current encryption driver
        /// </summary>
        public static IEncryptionDriver CurrentEncryptionDriver =>
            (IEncryptionDriver)currentDrivers[DriverTypes.Encryption];

        /// <summary>
        /// Gets the current regexp driver
        /// </summary>
        public static IRegexpDriver CurrentRegexpDriver =>
            (IRegexpDriver)currentDrivers[DriverTypes.Regexp];

        /// <summary>
        /// Gets the driver
        /// </summary>
        /// <param name="name">The driver name</param>
        /// <returns>The driver responsible for performing operations according to driver type</returns>
        public static TResult GetDriver<TResult>(string name)
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, get the actual driver from name
            if (drivers[driverType].ContainsKey(name))
                // Found a driver under the kernel driver list
                return (TResult)drivers[driverType][name];
            else if (IsRegistered(driverType, name))
                // Found a driver under the custom driver list
                return (TResult)customDrivers[driverType][name];
            else
                // Found no driver under both lists
                return (TResult)drivers[driverType]["Default"];
        }

        /// <summary>
        /// Registers the driver
        /// </summary>
        /// <param name="type">Type of driver to register</param>
        /// <param name="driver">Driver to be registered</param>
        public static void RegisterDriver<TDriver>(DriverTypes type, IDriver driver)
        {
            string name = driver.DriverName;
            if (!IsRegistered(type, name) && driver.DriverType == type)
                customDrivers[type].Add(name, driver);
        }

        /// <summary>
        /// Unregisters the driver
        /// </summary>
        /// <param name="type">Type of driver to unregister</param>
        /// <param name="name">Driver name to be unregistered</param>
        public static void UnregisterDriver(DriverTypes type, string name)
        {
            if (IsRegistered(type, name) && customDrivers[type][name].DriverType == type)
                customDrivers[type].Remove(name);
        }

        /// <summary>
        /// Is the driver registered?
        /// </summary>
        /// <param name="type">Driver type</param>
        /// <param name="name">Driver name</param>
        /// <returns>True if registered. Otherwise, false.</returns>
        public static bool IsRegistered(DriverTypes type, string name) =>
            customDrivers[type].ContainsKey(name) || drivers[type].ContainsKey(name);

        /// <summary>
        /// Sets the kernel driver
        /// </summary>
        /// <param name="name">Name of the available kernel driver to set to</param>
        /// <exception cref="KernelException"></exception>
        public static void SetDriver<T>(string name)
        {
            // First, infer the type from the T
            var driverType = InferDriverTypeFromDriverInterfaceType<T>();

            // Then, try to set the driver
            currentDrivers[driverType] = (IDriver)GetDriver<T>(name);
        }

        private static DriverTypes InferDriverTypeFromDriverInterfaceType<T>()
        {
            var driverType = default(DriverTypes);
            if (typeof(T) == typeof(IEncryptionDriver))
                driverType = DriverTypes.Encryption;
            else if (typeof(T) == typeof(IConsoleDriver))
                driverType = DriverTypes.Console;
            else if (typeof(T) == typeof(INetworkDriver))
                driverType = DriverTypes.Network;
            else if (typeof(T) == typeof(IFilesystemDriver))
                driverType = DriverTypes.Filesystem;
            else if (typeof(T) == typeof(IEncryptionDriver))
                driverType = DriverTypes.Encryption;
            else if (typeof(T) == typeof(IRegexpDriver))
                driverType = DriverTypes.Regexp;
            return driverType;
        }
    }
}
