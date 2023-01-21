
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
        private readonly static Dictionary<string, IRandomDriver> randomDrivers = new()
        {
            { "Default", new DefaultRandom() },
            { "Cryptographic", new CryptographicRandom() },

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

        private readonly static Dictionary<string, INetworkDriver> networkDrivers = new()
        {
            { "Default", new DefaultNetwork() }
        };

        private readonly static Dictionary<string, IFilesystemDriver> filesystemDrivers = new()
        {
            { "Default", new DefaultFilesystem() }
        };

        private readonly static Dictionary<string, IEncryptionDriver> encryptionDrivers = new()
        {
            { "CRC32", new CRC32() },
            { "MD5", new MD5() },
            { "SHA1", new SHA1() },
            { "SHA256", new SHA256() },
            { "SHA384", new SHA384() },
            { "SHA512", new SHA512() },
        };

        private readonly static Dictionary<string, IRegexpDriver> regexpDrivers = new()
        {
            { "Default", new DefaultRegexp() }
        };

        private readonly static Dictionary<string, IRandomDriver> customRandomDrivers = new();
        private readonly static Dictionary<string, IConsoleDriver> customConsoleDrivers = new();
        private readonly static Dictionary<string, INetworkDriver> customNetworkDrivers = new();
        private readonly static Dictionary<string, IFilesystemDriver> customFilesystemDrivers = new();
        private readonly static Dictionary<string, IEncryptionDriver> customEncryptionDrivers = new();
        private readonly static Dictionary<string, IRegexpDriver> customRegexpDrivers = new();

        // Don't move this field to the top, or NullReferenceException will haunt you!!!
        internal static Dictionary<DriverTypes, IDriver> currentDrivers = new()
        {
            { DriverTypes.Console,      consoleDrivers["Terminal"] },
            { DriverTypes.RNG,          randomDrivers["Default"]  },
            { DriverTypes.Network,      networkDrivers["Default"]  },
            { DriverTypes.Filesystem,   filesystemDrivers["Default"]  },
            { DriverTypes.Encryption,   encryptionDrivers["SHA256"]   },
            { DriverTypes.Regexp,       regexpDrivers["Default"]  },
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
            switch (driverType)
            {
                case DriverTypes.RNG:
                    if (randomDrivers.ContainsKey(name))
                        // Found a driver under the kernel driver list
                        return (TResult)randomDrivers[name];
                    else if (IsRegistered(driverType, name))
                        // Found a driver under the custom driver list
                        return (TResult)customRandomDrivers[name];
                    else
                        // Found no driver under both lists
                        return (TResult)randomDrivers["SHA256"];
                    // Same goes as for below...
                case DriverTypes.Console:
                    if (consoleDrivers.ContainsKey(name))
                        return (TResult)consoleDrivers[name];
                    else if (IsRegistered(driverType, name))
                        return (TResult)customConsoleDrivers[name];
                    else
                        return (TResult)consoleDrivers["Terminal"];
                case DriverTypes.Network:
                    if (networkDrivers.ContainsKey(name))
                        return (TResult)networkDrivers[name];
                    else if (IsRegistered(driverType, name))
                        return (TResult)customNetworkDrivers[name];
                    else
                        return (TResult)networkDrivers["Terminal"];
                case DriverTypes.Filesystem:
                    if (filesystemDrivers.ContainsKey(name))
                        return (TResult)filesystemDrivers[name];
                    else if (IsRegistered(driverType, name))
                        return (TResult)customFilesystemDrivers[name];
                    else
                        return (TResult)filesystemDrivers["Terminal"];
                case DriverTypes.Encryption:
                    if (encryptionDrivers.ContainsKey(name))
                        return (TResult)encryptionDrivers[name];
                    else if (IsRegistered(driverType, name))
                        return (TResult)customEncryptionDrivers[name];
                    else
                        return (TResult)encryptionDrivers["Terminal"];
                case DriverTypes.Regexp:
                    if (regexpDrivers.ContainsKey(name))
                        return (TResult)regexpDrivers[name];
                    else if (IsRegistered(driverType, name))
                        return (TResult)customRegexpDrivers[name];
                    else
                        return (TResult)regexpDrivers["Terminal"];
            }

            // We shouldn't be here
            DebugWriter.WriteDebug(DebugLevel.E, "We shouldn't be returning null here. Are you sure that it's of type {0} with name {1}?", typeof(TResult).Name, name);
            return default;
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
                case DriverTypes.Network:
                    if (!IsRegistered(type, name))
                        customNetworkDrivers.Add(name, (INetworkDriver)driver);
                    break;
                case DriverTypes.Filesystem:
                    if (!IsRegistered(type, name))
                        customFilesystemDrivers.Add(name, (IFilesystemDriver)driver);
                    break;
                case DriverTypes.Encryption:
                    if (!IsRegistered(type, name))
                        customEncryptionDrivers.Add(name, (IEncryptionDriver)driver);
                    break;
                case DriverTypes.Regexp:
                    if (!IsRegistered(type, name))
                        customRegexpDrivers.Add(name, (IRegexpDriver)driver);
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
                case DriverTypes.Network:
                    if (IsRegistered(type, name))
                        customNetworkDrivers.Remove(name);
                    break;
                case DriverTypes.Filesystem:
                    if (IsRegistered(type, name))
                        customFilesystemDrivers.Remove(name);
                    break;
                case DriverTypes.Encryption:
                    if (IsRegistered(type, name))
                        customEncryptionDrivers.Remove(name);
                    break;
                case DriverTypes.Regexp:
                    if (IsRegistered(type, name))
                        customRegexpDrivers.Remove(name);
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
                DriverTypes.RNG         => customRandomDrivers.ContainsKey(name),
                DriverTypes.Console     => customConsoleDrivers.ContainsKey(name),
                DriverTypes.Network     => customNetworkDrivers.ContainsKey(name),
                DriverTypes.Filesystem  => customFilesystemDrivers.ContainsKey(name),
                DriverTypes.Encryption  => customEncryptionDrivers.ContainsKey(name),
                DriverTypes.Regexp      => customRegexpDrivers.ContainsKey(name),
                _                       => false,
            };
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
