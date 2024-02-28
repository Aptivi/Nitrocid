//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using System.Collections.Generic;
using System.Linq;
using System;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Drivers.RNG;
using Nitrocid.Drivers.Filesystem;
using Nitrocid.Drivers.Encoding;
using Nitrocid.Drivers.HardwareProber;
using Nitrocid.Drivers.Network;
using Nitrocid.Languages;
using Nitrocid.Drivers.Sorting;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Drivers.DebugLogger;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Drivers.Input;
using Nitrocid.Drivers.Regexp;
using Nitrocid.Drivers.Console;
using Nitrocid.Drivers.Network.Bases;
using Nitrocid.Drivers.RNG.Bases;
using Nitrocid.Drivers.Filesystem.Bases;
using Nitrocid.Drivers.Console.Bases;
using Nitrocid.Drivers.Encryption.Bases;
using Nitrocid.Drivers.Regexp.Bases;
using Nitrocid.Drivers.DebugLogger.Bases;
using Nitrocid.Drivers.Encoding.Bases;
using Nitrocid.Drivers.HardwareProber.Bases;
using Nitrocid.Drivers.Sorting.Bases;
using Nitrocid.Drivers.Input.Bases;
using Nitrocid.ConsoleBase.Inputs;

namespace Nitrocid.Drivers
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
                    { "MonoCompat", new TerminalMonoCompat() },
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
                    { "Optimized", new OptimizedRandom() },

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
                    { "Default", new DefaultFilesystem() },

#if !SPECIFIERREL
                    // Below are excluded from the final release
                    { "DefaultDebug", new DefaultFilesystemDebug() }
#endif
                }
            },
            {
                DriverTypes.Encryption, new()
                {
                    { "Default", new SHA256() },
                    { "SHA256", new SHA256() },
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
            },
            {
                DriverTypes.Encoding, new()
                {
                    { "Default", new AesEncoding() },
                    { "RSA", new RsaEncoding() },
                    { "BASE64", new Base64Encoding() },
                }
            },
            {
                DriverTypes.HardwareProber, new()
                {
                    { "Default", new DefaultHardwareProber() },
                }
            },
            {
                DriverTypes.Sorting, new()
                {
                    { "Default", new DefaultSorting() },
                    { "Quick", new QuickSorting() },
                    { "Selection", new SelectionSorting() },
                    { "Merge", new MergeSorting() },
                }
            },
            {
                DriverTypes.Input, new()
                {
                    { "Default", new DefaultInput() },
                }
            }
        };

        internal static Dictionary<DriverTypes, Dictionary<string, IDriver>> customDrivers = new()
        {
            { DriverTypes.Console,              new() },
            { DriverTypes.RNG,                  new() },
            { DriverTypes.Network,              new() },
            { DriverTypes.Filesystem,           new() },
            { DriverTypes.Encryption,           new() },
            { DriverTypes.Regexp,               new() },
            { DriverTypes.DebugLogger,          new() },
            { DriverTypes.Encoding,             new() },
            { DriverTypes.HardwareProber,       new() },
            { DriverTypes.Sorting,              new() },
            { DriverTypes.Input,                new() },
        };

        internal static Dictionary<DriverTypes, IDriver> currentDrivers = new()
        {
            { DriverTypes.Console,              drivers[DriverTypes.Console]["Default"] },
            { DriverTypes.RNG,                  drivers[DriverTypes.RNG]["Default"] },
            { DriverTypes.Network,              drivers[DriverTypes.Network]["Default"] },
            { DriverTypes.Filesystem,           drivers[DriverTypes.Filesystem]["Default"] },
            { DriverTypes.Encryption,           drivers[DriverTypes.Encryption]["Default"] },
            { DriverTypes.Regexp,               drivers[DriverTypes.Regexp]["Default"] },
            { DriverTypes.DebugLogger,          drivers[DriverTypes.DebugLogger]["Default"] },
            { DriverTypes.Encoding,             drivers[DriverTypes.Encoding]["Default"] },
            { DriverTypes.HardwareProber,       drivers[DriverTypes.HardwareProber]["Default"] },
            { DriverTypes.Sorting,              drivers[DriverTypes.Sorting]["Default"] },
            { DriverTypes.Input,                drivers[DriverTypes.Input]["Default"] },
        };

        internal static Dictionary<Type, DriverTypes> knownTypes = new()
        {
            { typeof(IConsoleDriver),           DriverTypes.Console },
            { typeof(IRandomDriver),            DriverTypes.RNG },
            { typeof(INetworkDriver),           DriverTypes.Network },
            { typeof(IFilesystemDriver),        DriverTypes.Filesystem },
            { typeof(IEncryptionDriver),        DriverTypes.Encryption },
            { typeof(IRegexpDriver),            DriverTypes.Regexp },
            { typeof(IDebugLoggerDriver),       DriverTypes.DebugLogger },
            { typeof(IEncodingDriver),          DriverTypes.Encoding },
            { typeof(IHardwareProberDriver),    DriverTypes.HardwareProber },
            { typeof(ISortingDriver),           DriverTypes.Sorting },
            { typeof(IInputDriver),             DriverTypes.Input },
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
        /// Gets the current encoding driver (use this when possible)
        /// </summary>
        public static IEncodingDriver CurrentEncodingDriverLocal =>
            begunLocal ? (IEncodingDriver)currentDriversLocal[DriverTypes.Encoding] : CurrentEncodingDriver;

        /// <summary>
        /// Gets the current hardware prober driver (use this when possible)
        /// </summary>
        public static IHardwareProberDriver CurrentHardwareProberDriverLocal =>
            begunLocal ? (IHardwareProberDriver)currentDriversLocal[DriverTypes.HardwareProber] : CurrentHardwareProberDriver;

        /// <summary>
        /// Gets the current sorting driver (use this when possible)
        /// </summary>
        public static ISortingDriver CurrentSortingDriverLocal =>
            begunLocal ? (ISortingDriver)currentDriversLocal[DriverTypes.Sorting] : CurrentSortingDriver;

        /// <summary>
        /// Gets the current input driver (use this when possible)
        /// </summary>
        public static IInputDriver CurrentInputDriverLocal =>
            begunLocal ? (IInputDriver)currentDriversLocal[DriverTypes.Input] : CurrentInputDriver;

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
        /// Gets the system-wide current encoding driver
        /// </summary>
        public static IEncodingDriver CurrentEncodingDriver =>
            (IEncodingDriver)currentDrivers[DriverTypes.Encoding];

        /// <summary>
        /// Gets the system-wide current hardware prober driver
        /// </summary>
        public static IHardwareProberDriver CurrentHardwareProberDriver =>
            (IHardwareProberDriver)currentDrivers[DriverTypes.HardwareProber];

        /// <summary>
        /// Gets the system-wide current sorting driver
        /// </summary>
        public static ISortingDriver CurrentSortingDriver =>
            (ISortingDriver)currentDrivers[DriverTypes.Sorting];

        /// <summary>
        /// Gets the system-wide current input driver
        /// </summary>
        public static IInputDriver CurrentInputDriver =>
            (IInputDriver)currentDrivers[DriverTypes.Input];

        /// <summary>
        /// Gets the driver
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <param name="name">The driver name</param>
        /// <returns>The driver responsible for performing operations according to driver type</returns>
        public static TResult GetDriver<TResult>(string name)
            where TResult : IDriver
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, get the actual driver from name
            return (TResult)GetDriver(driverType, name);
        }

        /// <summary>
        /// Gets the driver
        /// </summary>
        /// <param name="driverType">The required driver type</param>
        /// <param name="name">The driver name</param>
        /// <returns>The driver responsible for performing operations according to driver type</returns>
        public static IDriver GetDriver(DriverTypes driverType, string name)
        {
            // Get the actual driver from name
            if (IsBuiltin(driverType, name))
            {
                // Found a driver under the kernel driver list
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver {0}, type {1}, found under the built-in driver list.", name, driverType.ToString());
                return drivers[driverType][name];
            }
            else if (IsRegistered(driverType, name))
            {
                // Found a driver under the custom driver list
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver {0}, type {1}, found under the custom driver list.", name, driverType.ToString());
                return customDrivers[driverType][name];
            }
            else
            {
                // Found no driver under both lists
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver {0}, type {1}, not found in any list.", name, driverType.ToString());
                return GetFallbackDriver(driverType);
            }
        }

        /// <summary>
        /// Gets the driver name
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <param name="driver">Driver to query its name from the key</param>
        /// <returns>Driver name</returns>
        public static string GetDriverName<TResult>(IDriver driver)
            where TResult : IDriver
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, get the actual driver from name
            return GetDriverName(driverType, driver);
        }

        /// <summary>
        /// Gets the driver name
        /// </summary>
        /// <param name="driverType">The required driver type</param>
        /// <param name="driver">Driver to query its name from the key</param>
        /// <returns>Driver name</returns>
        public static string GetDriverName(DriverTypes driverType, IDriver driver)
        {
            // Get the actual driver from name
            if (IsBuiltin(driverType, driver))
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
                return GetFallbackDriverName(driverType);
            }
        }

        /// <summary>
        /// Gets the drivers
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <returns>List of drivers with their instances</returns>
        public static Dictionary<string, IDriver> GetDrivers<TResult>()
            where TResult : IDriver
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, get the list of drivers
            return GetDrivers(driverType);
        }

        /// <summary>
        /// Gets the drivers
        /// </summary>
        /// <param name="driverType">The required driver type</param>
        /// <returns>List of drivers with their instances</returns>
        public static Dictionary<string, IDriver> GetDrivers(DriverTypes driverType)
        {
            // Exclude internal drivers from the list
            var filteredDrivers = drivers[driverType].Where((kvp) => !kvp.Value.DriverInternal);
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
            where TResult : IDriver
        {
            // Get the drivers and fetch their names
            var drivers = GetDrivers<TResult>();
            return drivers.Select((kvp) => kvp.Key).ToArray();
        }

        /// <summary>
        /// Gets the driver names
        /// </summary>
        /// <param name="driverType">The required driver type</param>
        /// <returns>List of driver names</returns>
        public static string[] GetDriverNames(DriverTypes driverType)
        {
            // Get the drivers and fetch their names
            var drivers = GetDrivers(driverType);
            return drivers.Select((kvp) => kvp.Key).ToArray();
        }

        /// <summary>
        /// Gets the fallback (default) driver
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <returns>The driver responsible for performing operations according to driver type</returns>
        public static TResult GetFallbackDriver<TResult>()
            where TResult : IDriver
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, get the fallback (default) driver from name
            return (TResult)GetFallbackDriver(driverType);
        }

        /// <summary>
        /// Gets the fallback (default) driver
        /// </summary>
        /// <param name="driverType">The required driver type</param>
        /// <returns>The driver responsible for performing operations according to driver type</returns>
        public static IDriver GetFallbackDriver(DriverTypes driverType)
        {
            // Get the fallback (default) driver from name
            if (IsBuiltin(driverType, "Default"))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver, type {0}, fallback.", driverType.ToString());
                return drivers[driverType]["Default"];
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Kernel driver, type {0}, no fallback found!", driverType.ToString());
                throw new KernelException(KernelExceptionType.Hardware, Translate.DoTranslation("No fallback driver found."));
            }
        }

        /// <summary>
        /// Gets the fallback (default) name
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <returns>Fallback (default) name</returns>
        public static string GetFallbackDriverName<TResult>()
            where TResult : IDriver
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, get the fallback (default) driver from name
            return GetFallbackDriverName(driverType);
        }

        /// <summary>
        /// Gets the fallback (default) name
        /// </summary>
        /// <param name="driverType">The required driver type</param>
        /// <returns>Fallback (default) name</returns>
        public static string GetFallbackDriverName(DriverTypes driverType)
        {
            // Get the fallback (default) driver from name
            if (IsBuiltin(driverType, "Default"))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver, type {0}, fallback.", driverType.ToString());
                return "Default";
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Kernel driver, type {0}, no fallback found!", driverType.ToString());
                throw new KernelException(KernelExceptionType.Hardware, Translate.DoTranslation("No fallback driver found."));
            }
        }

        /// <summary>
        /// Gets the current driver
        /// </summary>
        /// <param name="driverType">The required driver type</param>
        /// <returns>The current driver responsible for performing operations according to driver type</returns>
        public static IDriver GetCurrentDriver(DriverTypes driverType)
        {
            // Get the current driver
            return currentDrivers[driverType];
        }

        /// <summary>
        /// Gets the current local driver
        /// </summary>
        /// <param name="driverType">The required driver type</param>
        /// <returns>The current local driver responsible for performing operations according to driver type</returns>
        public static IDriver GetCurrentDriverLocal(DriverTypes driverType)
        {
            // Get the current driver
            return currentDriversLocal[driverType];
        }

        /// <summary>
        /// Gets the current driver
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <returns>The current driver responsible for performing operations according to driver type</returns>
        public static TResult GetCurrentDriver<TResult>()
            where TResult : IDriver
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, get the current driver
            return (TResult)GetCurrentDriver(driverType);
        }

        /// <summary>
        /// Gets the current local driver
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <returns>The current local driver responsible for performing operations according to driver type</returns>
        public static TResult GetCurrentDriverLocal<TResult>()
            where TResult : IDriver
        {
            // First, infer the type from the TResult
            var driverType = InferDriverTypeFromDriverInterfaceType<TResult>();

            // Then, get the current local driver
            return (TResult)GetCurrentDriverLocal(driverType);
        }

        /// <summary>
        /// Registers the driver
        /// </summary>
        /// <typeparam name="TDriver">Type of driver to register</typeparam>
        /// <param name="driver">Driver to be registered</param>
        public static void RegisterDriver<TDriver>(IDriver driver)
            where TDriver : IDriver
        {
            // First, infer the type from the TDriver
            var driverType = InferDriverTypeFromDriverInterfaceType<TDriver>();

            // Then, register the driver
            RegisterDriver(driverType, driver);
        }

        /// <summary>
        /// Registers the driver
        /// </summary>
        /// <param name="type">Type of driver to register</param>
        /// <param name="driver">Driver to be registered</param>
        public static void RegisterDriver(DriverTypes type, IDriver driver)
        {
            if (driver is null)
                throw new KernelException(KernelExceptionType.DriverHandler, Translate.DoTranslation("Can't register a non-driver or a null driver."));

            // Now, get the driver name
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
        /// <typeparam name="TDriver">Type of driver to unregister</typeparam>
        /// <param name="name">Driver name to be unregistered</param>
        public static void UnregisterDriver<TDriver>(string name)
            where TDriver : IDriver
        {
            // First, infer the type from the TDriver
            var driverType = InferDriverTypeFromDriverInterfaceType<TDriver>();

            // Then, register the driver
            UnregisterDriver(driverType, name);
        }

        /// <summary>
        /// Unregisters the driver
        /// </summary>
        /// <param name="type">Type of driver to unregister</param>
        /// <param name="name">Driver name to be unregistered</param>
        public static void UnregisterDriver(DriverTypes type, string name)
        {
            if (IsRegistered(type, name) && !IsBuiltin(type, name) && customDrivers[type][name].DriverType == type)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Unregistered driver {0} [{1}] under type {2}", name, customDrivers[type][name].GetType().Name, type.ToString());
                customDrivers[type].Remove(name);
            }
        }

        /// <summary>
        /// Is the driver built-in?
        /// </summary>
        /// <typeparam name="TDriver">Type of driver to query</typeparam>
        /// <param name="name">Driver name</param>
        /// <returns>True if built-in. Otherwise, false.</returns>
        public static bool IsBuiltin<TDriver>(string name)
            where TDriver : IDriver
        {
            // First, infer the type from the TDriver
            var driverType = InferDriverTypeFromDriverInterfaceType<TDriver>();

            // Then, query the driver
            return IsBuiltin(driverType, name);
        }

        /// <summary>
        /// Is the driver built-in?
        /// </summary>
        /// <typeparam name="TDriver">Type of driver to query</typeparam>
        /// <param name="driver">Driver to query its name from the key</param>
        /// <returns>True if built-in. Otherwise, false.</returns>
        public static bool IsBuiltin<TDriver>(IDriver driver)
            where TDriver : IDriver
        {
            // First, infer the type from the TDriver
            var driverType = InferDriverTypeFromDriverInterfaceType<TDriver>();

            // Then, query the driver
            return IsBuiltin(driverType, driver);
        }

        /// <summary>
        /// Is the driver built-in?
        /// </summary>
        /// <param name="type">Driver type</param>
        /// <param name="name">Driver name</param>
        /// <returns>True if built-in. Otherwise, false.</returns>
        public static bool IsBuiltin(DriverTypes type, string name)
        {
            bool registered = drivers[type].ContainsKey(name);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered built-in {0} for {1}? {2}", name, type.ToString(), registered);
            return registered;
        }

        /// <summary>
        /// Is the driver built-in?
        /// </summary>
        /// <param name="type">Driver type</param>
        /// <param name="driver">Driver to query its name from the key</param>
        /// <returns>True if built-in. Otherwise, false.</returns>
        public static bool IsBuiltin(DriverTypes type, IDriver driver)
        {
            bool registered = drivers[type].ContainsValue(driver);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered built-in {0} for {1}? {2}", driver.GetType().Name, type.ToString(), registered);
            return registered;
        }

        /// <summary>
        /// Is the driver registered?
        /// </summary>
        /// <typeparam name="TDriver">Type of driver to query</typeparam>
        /// <param name="name">Driver name</param>
        /// <returns>True if registered. Otherwise, false.</returns>
        public static bool IsRegistered<TDriver>(string name)
            where TDriver : IDriver
        {
            // First, infer the type from the TDriver
            var driverType = InferDriverTypeFromDriverInterfaceType<TDriver>();

            // Then, query the driver
            return IsRegistered(driverType, name);
        }

        /// <summary>
        /// Is the driver registered?
        /// </summary>
        /// <typeparam name="TDriver">Type of driver to query</typeparam>
        /// <param name="driver">Driver to query its name from the key</param>
        /// <returns>True if registered. Otherwise, false.</returns>
        public static bool IsRegistered<TDriver>(IDriver driver)
            where TDriver : IDriver
        {
            // First, infer the type from the TDriver
            var driverType = InferDriverTypeFromDriverInterfaceType<TDriver>();

            // Then, query the driver
            return IsRegistered(driverType, driver);
        }

        /// <summary>
        /// Is the driver registered?
        /// </summary>
        /// <param name="type">Driver type</param>
        /// <param name="name">Driver name</param>
        /// <returns>True if registered. Otherwise, false.</returns>
        public static bool IsRegistered(DriverTypes type, string name)
        {
            bool registered = customDrivers[type].ContainsKey(name) || IsBuiltin(type, name);
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
            bool registered = customDrivers[type].ContainsValue(driver) || IsBuiltin(type, driver);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered {0} for {1}? {2}", driver.GetType().Name, type.ToString(), registered);
            return registered;
        }

        /// <summary>
        /// Sets the kernel driver
        /// </summary>
        /// <param name="name">Name of the available kernel driver to set to</param>
        /// <exception cref="KernelException"></exception>
        public static void SetDriver<T>(string name)
            where T : IDriver
        {
            // First, infer the type from the T
            var driverType = InferDriverTypeFromDriverInterfaceType<T>();

            // Then, try to set the driver
            SetDriver(driverType, name);
        }

        /// <summary>
        /// Sets the kernel driver
        /// </summary>
        /// <param name="driverType">Driver type</param>
        /// <param name="name">Name of the available kernel driver to set to</param>
        /// <exception cref="KernelException"></exception>
        public static void SetDriver(DriverTypes driverType, string name)
        {
            // Try to set the driver
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to set driver to {0} for type {1}...", name, driverType.ToString());
            currentDrivers[driverType] = GetDriver(driverType, name);
            SetDriverLocal(driverType, name);
        }

        /// <summary>
        /// Sets the kernel driver
        /// </summary>
        /// <param name="name">Name of the available kernel driver to set to</param>
        /// <exception cref="KernelException"></exception>
        public static void SetDriverSafe<T>(string name)
            where T : IDriver
        {
            // First, infer the type from the T
            var driverType = InferDriverTypeFromDriverInterfaceType<T>();

            // Then, try to set the driver
            SetDriverSafe(driverType, name);
        }

        /// <summary>
        /// Sets the kernel driver
        /// </summary>
        /// <param name="driverType">Driver type</param>
        /// <param name="name">Name of the available kernel driver to set to</param>
        /// <exception cref="KernelException"></exception>
        public static void SetDriverSafe(DriverTypes driverType, string name)
        {
            // Try to set the driver
            var drivers = GetDrivers(driverType);
            if (!drivers.ContainsKey(name))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Nonexistent driver {0} for type {1}, so setting to default...", name, driverType.ToString());
                currentDrivers[driverType] = GetDriver(driverType, "Default");
                SetDriverLocal(driverType, "Default");
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Setting driver to {0} for type {1}...", name, driverType.ToString());
                currentDrivers[driverType] = GetDriver(driverType, name);
                SetDriverLocal(driverType, name);
            }
        }

        /// <summary>
        /// Begins to use the local driver
        /// </summary>
        /// <param name="name">Name of the available kernel driver to set to</param>
        public static void BeginLocalDriver<T>(string name)
            where T : IDriver
        {
            if (begunLocal)
                return;

            // First, infer the type from the T
            var driverType = InferDriverTypeFromDriverInterfaceType<T>();

            // Then, try to set the driver
            BeginLocalDriver(driverType, name);
        }

        /// <summary>
        /// Begins to use the local driver
        /// </summary>
        /// <param name="driverType">Driver type</param>
        /// <param name="name">Name of the available kernel driver to set to</param>
        public static void BeginLocalDriver(DriverTypes driverType, string name)
        {
            if (begunLocal)
                return;

            // Try to set the driver
            begunLocal = true;
            SetDriverLocal(driverType, name);
            DebugWriter.WriteDebug(DebugLevel.I, "Local driver {0} has begun.", name);
        }

        /// <summary>
        /// Begins to use the local driver
        /// </summary>
        /// <param name="name">Name of the available kernel driver to set to</param>
        public static void BeginLocalDriverSafe<T>(string name)
            where T : IDriver
        {
            if (begunLocal)
                return;

            // First, infer the type from the T
            var driverType = InferDriverTypeFromDriverInterfaceType<T>();

            // Then, try to set the driver
            BeginLocalDriverSafe(driverType, name);
        }

        /// <summary>
        /// Begins to use the local driver
        /// </summary>
        /// <param name="driverType">Driver type</param>
        /// <param name="name">Name of the available kernel driver to set to</param>
        public static void BeginLocalDriverSafe(DriverTypes driverType, string name)
        {
            if (begunLocal)
                return;

            // Try to set the driver
            begunLocal = true;
            var drivers = GetDrivers(driverType);
            if (!drivers.ContainsKey(name))
                SetDriverLocal(driverType, "Default");
            else
                SetDriverLocal(driverType, name);
            DebugWriter.WriteDebug(DebugLevel.I, "Local driver {0} has begun.", name);
        }

        /// <summary>
        /// Ends using the local driver
        /// </summary>
        public static void EndLocalDriver<T>()
            where T : IDriver
        {
            if (!begunLocal)
                return;

            // First, infer the type from the T
            var driverType = InferDriverTypeFromDriverInterfaceType<T>();

            // Try to set the driver
            EndLocalDriver(driverType);
        }

        /// <summary>
        /// Ends using the local driver
        /// </summary>
        /// <param name="driverType">Driver type</param>
        public static void EndLocalDriver(DriverTypes driverType)
        {
            if (!begunLocal)
                return;

            // Try to set the driver
            currentDriversLocal[driverType] = currentDrivers[driverType];
            begunLocal = false;

            // Edge case for terminal drivers: Reset the Terminaux handler
            if (driverType == DriverTypes.Console)
            {
                InputTools.isWrapperInitialized = false;
                InputTools.InitializeTerminauxWrappers();
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Local driver has ended.");
        }

        internal static void SetDriverLocal<T>(string name)
            where T : IDriver
        {
            // First, infer the type from the T
            var driverType = InferDriverTypeFromDriverInterfaceType<T>();

            // Then, try to set the driver
            SetDriverLocal(driverType, name);
        }

        internal static void SetDriverLocal(DriverTypes driverType, string name)
        {
            // Try to set the driver
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to set driver to {0} for type {1}...", name, driverType.ToString());
            currentDriversLocal[driverType] = GetDriver(driverType, name);

            // Edge case for terminal drivers: Reset the Terminaux handler
            if (driverType == DriverTypes.Console)
            {
                InputTools.isWrapperInitialized = false;
                InputTools.InitializeTerminauxWrappers();
            }
        }

        internal static DriverTypes InferDriverTypeFromDriverInterfaceType<T>()
            where T : IDriver
        {
            DriverTypes driverType;
            var type = typeof(T);

            // Check the type
            if (!knownTypes.TryGetValue(type, out DriverTypes driverTypes))
                throw new KernelException(KernelExceptionType.DriverHandler, Translate.DoTranslation("Failed to infer driver type from unknown type") + $" {type.Name} [{type.FullName}]");

            // Now, actually infer the type from the driver interface type
            driverType = driverTypes;
            DebugWriter.WriteDebug(DebugLevel.I, "Inferred {0} for type {1}", driverType.ToString(), type.Name);
            return driverType;
        }

        internal static DriverTypes InferDriverTypeFromTypeName(string name)
        {
            DriverTypes driverType;

            // Check the type
            if (!Enum.TryParse(name, out DriverTypes driverTypes))
                throw new KernelException(KernelExceptionType.DriverHandler, Translate.DoTranslation("Failed to infer driver type from unknown type") + $" {name}");

            // Now, actually infer the type from the name
            driverType = driverTypes;
            DebugWriter.WriteDebug(DebugLevel.I, "Inferred {0} for type {1}", driverType.ToString(), name);
            return driverType;
        }

        internal static void RegisterBaseDriver<TDriver>(IDriver driver)
            where TDriver : IDriver
        {
            // First, infer the type from the TDriver
            var driverType = InferDriverTypeFromDriverInterfaceType<TDriver>();

            // Then, try to set the driver
            RegisterBaseDriver(driverType, driver);
        }

        internal static void RegisterBaseDriver(DriverTypes type, IDriver driver)
        {
            DebugCheck.AssertNull(driver, "driver should not be null");
            string name = driver.DriverName;
            if (!IsRegistered(type, name) && driver.DriverType == type)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Registered driver {0} [{1}] under type {2}", name, driver.GetType().Name, type.ToString());
                drivers[type].Add(name, driver);
            }
        }

        internal static void UnregisterBaseDriver<TDriver>(string name)
            where TDriver : IDriver
        {
            // First, infer the type from the TDriver
            var driverType = InferDriverTypeFromDriverInterfaceType<TDriver>();

            // Then, try to set the driver
            UnregisterBaseDriver(driverType, name);
        }

        internal static void UnregisterBaseDriver(DriverTypes type, string name)
        {
            if (IsRegistered(type, name) && drivers[type][name].DriverType == type)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Unregistered driver {0} [{1}] under type {2}", name, drivers[type][name].GetType().Name, type.ToString());
                drivers[type].Remove(name);
            }
        }
    }
}
