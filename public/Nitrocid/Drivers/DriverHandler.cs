//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Nitrocid.Drivers.Network.Bases;
using Nitrocid.Drivers.RNG.Bases;
using Nitrocid.Drivers.Filesystem.Bases;
using Nitrocid.Drivers.Encryption.Bases;
using Nitrocid.Drivers.Regexp.Bases;
using Nitrocid.Drivers.DebugLogger.Bases;
using Nitrocid.Drivers.Encoding.Bases;
using Nitrocid.Drivers.HardwareProber.Bases;
using Nitrocid.Drivers.Sorting.Bases;
using Nitrocid.Drivers.Input.Bases;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Drivers.EncodingAsymmetric.Bases;
using Nitrocid.Drivers.EncodingAsymmetric;

namespace Nitrocid.Drivers
{
    /// <summary>
    /// The driver handler routine class
    /// </summary>
    public static class DriverHandler
    {
        internal static bool begunLocal = false;
        internal static Dictionary<DriverTypes, List<IDriver>> drivers = new()
        {
            {
                DriverTypes.RNG, new()
                {
                    new DefaultRandom(),
                    new StandardRandom(),
                    new OptimizedRandom(),

#if !SPECIFIERREL
                    // Below are excluded from the final release
                    new DefaultRandomDebug(),
                    new StandardRandomDebug()
#endif
                }
            },
            {
                DriverTypes.Network, new()
                {
                    new DefaultNetwork()
                }
            },
            {
                DriverTypes.Filesystem, new()
                {
                    new DefaultFilesystem(),

#if !SPECIFIERREL
                    // Below are excluded from the final release
                    new DefaultFilesystemDebug()
#endif
                }
            },
            {
                DriverTypes.Encryption, new()
                {
                    new SHA256(),
                    new SHA512()
                }
            },
            {
                DriverTypes.Regexp, new()
                {
                    new DefaultRegexp()
                }
            },
            {
                DriverTypes.DebugLogger, new()
                {
                    new DefaultDebugLogger(),
                    new ConsoleDebugLogger(),
                    new UnitTestDebugLogger(),
                }
            },
            {
                DriverTypes.Encoding, new()
                {
                    new AesEncoding(),
                }
            },
            {
                DriverTypes.HardwareProber, new()
                {
                    new DefaultHardwareProber(),
                }
            },
            {
                DriverTypes.Sorting, new()
                {
                    new DefaultSorting(),
                    new QuickSorting(),
                    new SelectionSorting(),
                    new MergeSorting(),
                }
            },
            {
                DriverTypes.Input, new()
                {
                    new DefaultInput(),
                }
            },
            {
                DriverTypes.EncodingAsymmetric, new()
                {
                    new Base64Encoding(),
                    new RsaEncoding(),
                }
            },
        };

        internal static Dictionary<DriverTypes, List<IDriver>> customDrivers = new()
        {
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
            { DriverTypes.EncodingAsymmetric,   new() },
        };

        internal static Dictionary<DriverTypes, IDriver> currentDrivers = new()
        {
            { DriverTypes.RNG,                  drivers[DriverTypes.RNG][0] },
            { DriverTypes.Network,              drivers[DriverTypes.Network][0] },
            { DriverTypes.Filesystem,           drivers[DriverTypes.Filesystem][0] },
            { DriverTypes.Encryption,           drivers[DriverTypes.Encryption][0] },
            { DriverTypes.Regexp,               drivers[DriverTypes.Regexp][0] },
            { DriverTypes.DebugLogger,          drivers[DriverTypes.DebugLogger][0] },
            { DriverTypes.Encoding,             drivers[DriverTypes.Encoding][0] },
            { DriverTypes.HardwareProber,       drivers[DriverTypes.HardwareProber][0] },
            { DriverTypes.Sorting,              drivers[DriverTypes.Sorting][0] },
            { DriverTypes.Input,                drivers[DriverTypes.Input][0] },
            { DriverTypes.EncodingAsymmetric,   drivers[DriverTypes.EncodingAsymmetric][0] },
        };

        internal static Dictionary<Type, DriverTypes> knownTypes = new()
        {
            { typeof(IRandomDriver),                DriverTypes.RNG },
            { typeof(INetworkDriver),               DriverTypes.Network },
            { typeof(IFilesystemDriver),            DriverTypes.Filesystem },
            { typeof(IEncryptionDriver),            DriverTypes.Encryption },
            { typeof(IRegexpDriver),                DriverTypes.Regexp },
            { typeof(IDebugLoggerDriver),           DriverTypes.DebugLogger },
            { typeof(IEncodingDriver),              DriverTypes.Encoding },
            { typeof(IHardwareProberDriver),        DriverTypes.HardwareProber },
            { typeof(ISortingDriver),               DriverTypes.Sorting },
            { typeof(IInputDriver),                 DriverTypes.Input },
            { typeof(IEncodingAsymmetricDriver),    DriverTypes.EncodingAsymmetric },
        };

        internal static Dictionary<DriverTypes, IDriver> currentDriversLocal = new(currentDrivers);

        /// <summary>
        /// Gets the current random driver (use this when possible)
        /// </summary>
        public static IRandomDriver CurrentRandomDriverLocal =>
            begunLocal ? (IRandomDriver)currentDriversLocal[DriverTypes.RNG] : CurrentRandomDriver;

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
        /// Gets the current asymmetric encoding driver (use this when possible)
        /// </summary>
        public static IEncodingAsymmetricDriver CurrentEncodingAsymmetricDriverLocal =>
            begunLocal ? (IEncodingAsymmetricDriver)currentDriversLocal[DriverTypes.EncodingAsymmetric] : CurrentEncodingAsymmetricDriver;

        /// <summary>
        /// Gets the system-wide current random driver
        /// </summary>
        public static IRandomDriver CurrentRandomDriver =>
            (IRandomDriver)currentDrivers[DriverTypes.RNG];

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
        /// Gets the system-wide current asymmetric encoding driver
        /// </summary>
        public static IEncodingAsymmetricDriver CurrentEncodingAsymmetricDriver =>
            (IEncodingAsymmetricDriver)currentDrivers[DriverTypes.EncodingAsymmetric];

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
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver {0}, type {1}, found under the built-in driver list.", vars: [name, driverType.ToString()]);
                int idx = GetIndexFromBuiltinDriverName(driverType, name);
                return drivers[driverType][idx];
            }
            else if (IsRegistered(driverType, name))
            {
                // Found a driver under the custom driver list
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver {0}, type {1}, found under the custom driver list.", vars: [name, driverType.ToString()]);
                int idx = GetIndexFromCustomDriverName(driverType, name);
                return customDrivers[driverType][idx];
            }
            else
            {
                // Found no driver under both lists
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver {0}, type {1}, not found in any list.", vars: [name, driverType.ToString()]);
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
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver of type {0}, type {1}, found under the built-in driver list.", vars: [driver.GetType().Name, driverType.ToString()]);
                return drivers[driverType]
                    .Single((kvp) => kvp == driver).DriverName;
            }
            else if (IsRegistered(driverType, driver))
            {
                // Found a driver under the custom driver list
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver of type {0}, type {1}, found under the custom driver list.", vars: [driver.GetType().Name, driverType.ToString()]);
                return customDrivers[driverType]
                    .Single((kvp) => kvp == driver).DriverName;
            }
            else
            {
                // Found no driver under both lists
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver of type {0}, type {1}, not found in any list.", vars: [driver.GetType().Name, driverType.ToString()]);
                return GetFallbackDriverName(driverType);
            }
        }

        /// <summary>
        /// Gets the drivers
        /// </summary>
        /// <typeparam name="TResult">The required driver type</typeparam>
        /// <returns>List of drivers with their instances</returns>
        public static List<IDriver> GetDrivers<TResult>()
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
        public static List<IDriver> GetDrivers(DriverTypes driverType)
        {
            // Exclude internal drivers from the list
            var filteredDrivers = drivers[driverType].Where((kvp) => !kvp.DriverInternal);
            var filteredCustomDrivers = customDrivers[driverType].Where((kvp) => !kvp.DriverInternal);
            DebugWriter.WriteDebug(DebugLevel.I, "For type {0}, driver counts:", vars: [driverType.ToString()]);
            DebugWriter.WriteDebug(DebugLevel.I, "Initial drivers: {0}, custom: {1}", vars: [drivers[driverType].Count, customDrivers[driverType].Count]);
            DebugWriter.WriteDebug(DebugLevel.I, "Filtered drivers: {0}, custom: {1}", vars: [filteredDrivers.Count(), filteredCustomDrivers.Count()]);

            // Then, get the list of drivers
            DebugWriter.WriteDebug(DebugLevel.I, "Returning unified driver list");
            return filteredDrivers.Union(filteredCustomDrivers).ToList();
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
            return drivers.Select((kvp) => kvp.DriverName).ToArray();
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
            return drivers.Select((kvp) => kvp.DriverName).ToArray();
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
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver, type {0}, fallback.", vars: [driverType.ToString()]);
                return drivers[driverType][0];
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Kernel driver, type {0}, no fallback found!", vars: [driverType.ToString()]);
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
                DebugWriter.WriteDebug(DebugLevel.I, "Kernel driver, type {0}, fallback.", vars: [driverType.ToString()]);
                return "Default";
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Kernel driver, type {0}, no fallback found!", vars: [driverType.ToString()]);
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
                DebugWriter.WriteDebug(DebugLevel.I, "Registered driver {0} [{1}] under type {2}", vars: [name, driver.GetType().Name, type.ToString()]);
                customDrivers[type].Add(driver);
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
            if (IsRegistered(type, name) && !IsBuiltin(type, name))
            {
                int idx = GetIndexFromCustomDriverName(type, name);
                var driver = customDrivers[type][idx];
                if (driver.DriverType == type)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Unregistered driver {0} [{1}] under type {2}", vars: [name, driver.GetType().Name, type.ToString()]);
                    customDrivers[type].Remove(driver);
                }
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
            bool registered = drivers[type].Any((drv) => drv.DriverName == name);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered built-in {0} for {1}? {2}", vars: [name, type.ToString(), registered]);
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
            bool registered = drivers[type].Any((drv) => drv == driver);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered built-in {0} for {1}? {2}", vars: [driver.GetType().Name, type.ToString(), registered]);
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
            bool registered = customDrivers[type].Any((drv) => drv.DriverName == name) || IsBuiltin(type, name);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered {0} for {1}? {2}", vars: [name, type.ToString(), registered]);
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
            bool registered = customDrivers[type].Any((drv) => drv == driver) || IsBuiltin(type, driver);
            DebugWriter.WriteDebug(DebugLevel.I, "Registered {0} for {1}? {2}", vars: [driver.GetType().Name, type.ToString(), registered]);
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
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to set driver to {0} for type {1}...", vars: [name, driverType.ToString()]);
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
            if (!drivers.Any((drv) => drv.DriverName == name))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Nonexistent driver {0} for type {1}, so setting to default...", vars: [name, driverType.ToString()]);
                currentDrivers[driverType] = GetDriver(driverType, "Default");
                SetDriverLocal(driverType, "Default");
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Setting driver to {0} for type {1}...", vars: [name, driverType.ToString()]);
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
            DebugWriter.WriteDebug(DebugLevel.I, "Local driver {0} has begun.", vars: [name]);
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
            if (!drivers.Any((drv) => drv.DriverName == name))
                SetDriverLocal(driverType, "Default");
            else
                SetDriverLocal(driverType, name);
            DebugWriter.WriteDebug(DebugLevel.I, "Local driver {0} has begun.", vars: [name]);
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
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to set driver to {0} for type {1}...", vars: [name, driverType.ToString()]);
            currentDriversLocal[driverType] = GetDriver(driverType, name);
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
            DebugWriter.WriteDebug(DebugLevel.I, "Inferred {0} for type {1}", vars: [driverType.ToString(), type.Name]);
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
            DebugWriter.WriteDebug(DebugLevel.I, "Inferred {0} for type {1}", vars: [driverType.ToString(), name]);
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
                DebugWriter.WriteDebug(DebugLevel.I, "Registered driver {0} [{1}] under type {2}", vars: [name, driver.GetType().Name, type.ToString()]);
                drivers[type].Add(driver);
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
            if (IsRegistered(type, name))
            {
                int idx = GetIndexFromBuiltinDriverName(type, name);
                var driver = drivers[type][idx];
                if (driver.DriverType == type)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Unregistered driver {0} [{1}] under type {2}", vars: [name, driver.GetType().Name, type.ToString()]);
                    drivers[type].Remove(driver);
                }
            }
        }

        internal static int GetIndexFromBuiltinDriverName(DriverTypes type, string name)
        {
            if (IsRegistered(type, name) && IsBuiltin(type, name))
            {
                var list = drivers[type];
                for (int i = 0; i < list.Count; i++)
                {
                    IDriver driver = list[i];
                    if (driver.DriverType == type && driver.DriverName == name)
                        return i;
                }
            }
            return 0;
        }

        internal static int GetIndexFromCustomDriverName(DriverTypes type, string name)
        {
            if (IsRegistered(type, name) && !IsBuiltin(type, name))
            {
                var list = customDrivers[type];
                for (int i = 0; i < list.Count; i++)
                {
                    IDriver driver = list[i];
                    if (driver.DriverType == type && driver.DriverName == name)
                        return i;
                }
            }
            return 0;
        }

        internal static int GetIndexFromDriverName(DriverTypes type, string name)
        {
            if (IsRegistered(type, name))
            {
                var list = GetDrivers(type);
                for (int i = 0; i < list.Count; i++)
                {
                    IDriver driver = list[i];
                    if (driver.DriverType == type && driver.DriverName == name)
                        return i;
                }
            }
            return 0;
        }
    }
}
