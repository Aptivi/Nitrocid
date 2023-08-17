
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
using System.Linq;
using KS.Kernel.Debugging;
using KS.Drivers.DebugLogger.Bases;
using KS.Drivers.DebugLogger;

namespace KS.Drivers
{
    /// <summary>
    /// The driver handler routine class
    /// </summary>
    public static class DriverHandler
    {
        internal static bool begunLocal = false;
        internal static Dictionary<DriverTypes, Dictionary<string, IDriver>> drivers = new()
        {
            { 
                DriverTypes.Console, new()
                {
                    { "Default", new Terminal() },
                    { "File", new File() },
                    { "FileSequence", new FileSequence() },
                    { "Null", new Null() },
                    { "Buffered", new Buffered() },

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
            },
            { 
                DriverTypes.DebugLogger, new()
                { 
                    { "Default", new DefaultDebugLogger() },
                    { "Console", new ConsoleDebugLogger() },
                    { "UnitTest", new UnitTestDebugLogger() },
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
            { DriverTypes.DebugLogger,  new() },
        };

        internal static Dictionary<DriverTypes, IDriver> currentDrivers = new()
        {
            { DriverTypes.Console,      drivers[DriverTypes.Console]["Default"] },
            { DriverTypes.RNG,          drivers[DriverTypes.RNG]["Default"] },
            { DriverTypes.Network,      drivers[DriverTypes.Network]["Default"] },
            { DriverTypes.Filesystem,   drivers[DriverTypes.Filesystem]["Default"] },
            { DriverTypes.Encryption,   drivers[DriverTypes.Encryption]["Default"] },
            { DriverTypes.Regexp,       drivers[DriverTypes.Regexp]["Default"] },
            { DriverTypes.DebugLogger,  drivers[DriverTypes.DebugLogger]["Default"] },
        };

        internal static Dictionary<DriverTypes, IDriver> currentDriversLocal = new(currentDrivers);

        /// <summary>
        /// Gets the current random driver (use this when possible)
        /// </summary>
        public static IRandomDriver CurrentRandomDriverLocal =>
            begunLocal ? (IRandomDriver)currentDriversLocal[DriverTypes.RNG] : CurrentRandomDriver;

        /// <summary>
        /// Gets the current console driver (use this when possible)
        /// </summary>
        public static IConsoleDriver CurrentConsoleDriverLocal =>
            begunLocal ? (IConsoleDriver)currentDriversLocal[DriverTypes.Console] : CurrentConsoleDriver;

        /// <summary>
        /// Gets the current network driver (use this when possible)
        /// </summary>
        public static INetworkDriver CurrentNetworkDriverLocal =>
            begunLocal ? (INetworkDriver)currentDriversLocal[DriverTypes.Network] : CurrentNetworkDriver;

        /// <summary>
        /// Gets the current filesystem driver (use this when possible)
        /// </summary>
        public static IFilesystemDriver CurrentFilesystemDriverLocal =>
            begunLocal ? (IFilesystemDriver)currentDriversLocal[DriverTypes.Filesystem] : CurrentFilesystemDriver;

        /// <summary>
        /// Gets the current encryption driver (use this when possible)
        /// </summary>
        public static IEncryptionDriver CurrentEncryptionDriverLocal =>
            begunLocal ? (IEncryptionDriver)currentDriversLocal[DriverTypes.Encryption] : CurrentEncryptionDriver;

        /// <summary>
        /// Gets the current regexp driver (use this when possible)
        /// </summary>
        public static IRegexpDriver CurrentRegexpDriverLocal =>
            begunLocal ? (IRegexpDriver)currentDriversLocal[DriverTypes.Regexp] : CurrentRegexpDriver;

        /// <summary>
        /// Gets the current debug logger driver (use this when possible)
        /// </summary>
        public static IDebugLoggerDriver CurrentDebugLoggerDriverLocal =>
            begunLocal ? (IDebugLoggerDriver)currentDriversLocal[DriverTypes.DebugLogger] : CurrentDebugLoggerDriver;

        /// <summary>
        /// Gets the system-wide current random driver
        /// </summary>
        public static IRandomDriver CurrentRandomDriver =>
            (IRandomDriver)currentDrivers[DriverTypes.RNG];

        /// <summary>
        /// Gets the system-wide current console driver
        /// </summary>
        public static IConsoleDriver CurrentConsoleDriver =>
            (IConsoleDriver)currentDrivers[DriverTypes.Console];

        /// <summary>
        /// Gets the system-wide current network driver
        /// </summary>
        public static INetworkDriver CurrentNetworkDriver =>
            (INetworkDriver)currentDrivers[DriverTypes.Network];

        /// <summary>
        /// Gets the system-wide current filesystem driver
        /// </summary>
        public static IFilesystemDriver CurrentFilesystemDriver =>
            (IFilesystemDriver)currentDrivers[DriverTypes.Filesystem];

        /// <summary>
        /// Gets the system-wide current encryption driver
        /// </summary>
        public static IEncryptionDriver CurrentEncryptionDriver =>
            (IEncryptionDriver)currentDrivers[DriverTypes.Encryption];

        /// <summary>
        /// Gets the system-wide current regexp driver
        /// </summary>
        public static IRegexpDriver CurrentRegexpDriver =>
            (IRegexpDriver)currentDrivers[DriverTypes.Regexp];

        /// <summary>
        /// Gets the system-wide current debug logger driver
        /// </summary>
        public static IDebugLoggerDriver CurrentDebugLoggerDriver =>
            (IDebugLoggerDriver)currentDrivers[DriverTypes.DebugLogger];

        /// <summary>
        /// Gets the driver
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <param name="name">The driver name</param>
        /// <returns>The driver responsible for performing operations according to driver type</returns>
        public static TResult GetDriver<TResult>(string name)
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, get the actual driver from name
            if (drivers[driverType].ContainsKey(name))
            {
                // Found a driver under the kernel driver list
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver {0}, type {1}, found under the built-in driver list.", name, driverType.ToString());
                return (TResult)drivers[driverType][name];
            }
            else if (IsRegistered(driverType, name))
            {
                // Found a driver under the custom driver list
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver {0}, type {1}, found under the custom driver list.", name, driverType.ToString());
                return (TResult)customDrivers[driverType][name];
            }
            else
            {
                // Found no driver under both lists
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver {0}, type {1}, not found in any list.", name, driverType.ToString());
                return (TResult)drivers[driverType]["Default"];
            }
        }

        /// <summary>
        /// Gets the driver name
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <param name="driver">Driver to query its name from the key</param>
        /// <returns>Driver name</returns>
        public static string GetDriverName<TResult>(IDriver driver)
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, get the actual driver from name
            if (drivers[driverType].ContainsValue(driver))
            {
                // Found a driver under the kernel driver list
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver of type {0}, type {1}, found under the built-in driver list.", driver.GetType().Name, driverType.ToString());
                return drivers[driverType]
                    .Single((kvp) => kvp.Value == driver).Key;
            }
            else if (IsRegistered(driverType, driver))
            {
                // Found a driver under the custom driver list
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver of type {0}, type {1}, found under the custom driver list.", driver.GetType().Name, driverType.ToString());
                return customDrivers[driverType]
                    .Single((kvp) => kvp.Value == driver).Key;
            }
            else
            {
                // Found no driver under both lists
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver of type {0}, type {1}, not found in any list.", driver.GetType().Name, driverType.ToString());
                return "Default";
            }
        }

        /// <summary>
        /// Gets the drivers
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <returns>List of drivers with their instances</returns>
        public static Dictionary<string, IDriver> GetDrivers<TResult>()
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, exclude internal drivers from the list
            var filteredDrivers       = drivers[driverType].Where((kvp) => !kvp.Value.DriverInternal);
            var filteredCustomDrivers = customDrivers[driverType].Where((kvp) => !kvp.Value.DriverInternal);
            DebugWriter.WriteDebug(DebugLevel.I, "For type {0}, driver counts:", driverType.ToString());
            DebugWriter.WriteDebug(DebugLevel.I, "Initial drivers: {0}, custom: {1}", drivers[driverType].Count, customDrivers[driverType].Count);
            DebugWriter.WriteDebug(DebugLevel.I, "Filtered drivers: {0}, custom: {1}", filteredDrivers.Count(), filteredCustomDrivers.Count());

            // Then, get the list of drivers
            DebugWriter.WriteDebug(DebugLevel.I, "Returning unified driver list");
            return filteredDrivers.Union(filteredCustomDrivers).ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);
        }

        /// <summary>
        /// Gets the driver names
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <returns>List of driver names</returns>
        public static string[] GetDriverNames<TResult>()
        {
            // Get the drivers and fetch their names
            var drivers = GetDrivers<TResult>();
            return drivers.Select((kvp) => kvp.Key).ToArray();
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
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Registered driver {0} [{1}] under type {2}", name, driver.GetType().Name, type.ToString());
                customDrivers[type].Add(name, driver);
            }
        }

        /// <summary>
        /// Unregisters the driver
        /// </summary>
        /// <param name="type">Type of driver to unregister</param>
        /// <param name="name">Driver name to be unregistered</param>
        public static void UnregisterDriver(DriverTypes type, string name)
        {
            if (IsRegistered(type, name) && customDrivers[type][name].DriverType == type)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Unregistered driver {0} [{1}] under type {2}", name, customDrivers[type][name].GetType().Name, type.ToString());
                customDrivers[type].Remove(name);
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
            bool registered = customDrivers[type].ContainsKey(name) || drivers[type].ContainsKey(name);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered {0} for {1}? {2}", name, type.ToString(), registered);
            return registered;
        }

        /// <summary>
        /// Is the driver registered?
        /// </summary>
        /// <param name="type">Driver type</param>
        /// <param name="driver">Driver to query its name from the key</param>
        /// <returns>True if registered. Otherwise, false.</returns>
        public static bool IsRegistered(DriverTypes type, IDriver driver)
        {
            bool registered = customDrivers[type].ContainsValue(driver) || drivers[type].ContainsValue(driver);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered {0} for {1}? {2}", driver.GetType().Name, type.ToString(), registered);
            return registered;
        }

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
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to set driver to {0} for type {1}...", name, driverType.ToString());
            currentDrivers[driverType] = (IDriver)GetDriver<T>(name);
            SetDriverLocal<T>(name);
        }

        /// <summary>
        /// Sets the kernel driver
        /// </summary>
        /// <param name="name">Name of the available kernel driver to set to</param>
        /// <exception cref="KernelException"></exception>
        public static void SetDriverSafe<T>(string name)
        {
            // First, infer the type from the T
            var driverType = InferDriverTypeFromDriverInterfaceType<T>();

            // Then, try to set the driver
            var drivers = GetDrivers<T>();
            if (!drivers.ContainsKey(name))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Nonexistent driver {0} for type {1}, so setting to default...", name, driverType.ToString());
                currentDrivers[driverType] = (IDriver)GetDriver<T>("Default");
                SetDriverLocal<T>("Default");
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Setting driver to {0} for type {1}...", name, driverType.ToString());
                currentDrivers[driverType] = (IDriver)GetDriver<T>(name);
                SetDriverLocal<T>(name);
            }
        }

        /// <summary>
        /// Begins to use the local driver
        /// </summary>
        /// <param name="name">Name of the available kernel driver to set to</param>
        public static void BeginLocalDriver<T>(string name)
        {
            if (begunLocal)
                return;

            // Try to set the driver
            SetDriverLocal<T>(name);
            begunLocal = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Local driver {0} has begun.", name);
        }

        /// <summary>
        /// Begins to use the local driver
        /// </summary>
        /// <param name="name">Name of the available kernel driver to set to</param>
        public static void BeginLocalDriverSafe<T>(string name)
        {
            if (begunLocal)
                return;

            // Try to set the driver
            var drivers = GetDrivers<T>();
            if (!drivers.ContainsKey(name))
                SetDriverLocal<T>("Default");
            else
                SetDriverLocal<T>(name);
            begunLocal = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Local driver {0} has begun.", name);
        }

        /// <summary>
        /// Ends using the local driver
        /// </summary>
        public static void EndLocalDriver<T>()
        {
            if (!begunLocal)
                return;

            // First, infer the type from the T
            var driverType = InferDriverTypeFromDriverInterfaceType<T>();

            // Try to set the driver
            currentDriversLocal[driverType] = currentDrivers[driverType];
            begunLocal = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Local driver has ended.");
        }

        internal static void SetDriverLocal<T>(string name)
        {
            // First, infer the type from the T
            var driverType = InferDriverTypeFromDriverInterfaceType<T>();

            // Then, try to set the driver
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to set driver to {0} for type {1}...", name, driverType.ToString());
            currentDriversLocal[driverType] = (IDriver)GetDriver<T>(name);
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
            else if (typeof(T) == typeof(IDebugLoggerDriver))
                driverType = DriverTypes.DebugLogger;
            DebugWriter.WriteDebug(DebugLevel.I, "Inferred {0} for type {1}", driverType.ToString(), typeof(T).Name);
            return driverType;
        }
    }
}
