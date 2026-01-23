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

using System.Collections.Generic;
using System.Linq;
using Nitrocid.Kernel.Debugging;
using System.Collections.ObjectModel;
using Nitrocid.Kernel.Starting.Environment;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Kernel.Starting.Bootloader.Apps
{
    /// <summary>
    /// Bootable application manager
    /// </summary>
    public static class BootManager
    {
        private static readonly Dictionary<string, BaseEnvironment> bootApps = new()
        {
            {"Nitrocid KS", EnvironmentTools.mainEnvironment}
        };

        /// <summary>
        /// Gets boot applications
        /// </summary>
        public static ReadOnlyDictionary<string, BaseEnvironment> GetBootApps() =>
            new(bootApps);

        /// <summary>
        /// Adds a bootable application
        /// </summary>
        /// <param name="name">Bootable application name</param>
        /// <param name="environment">Application environment</param>
        /// <exception cref="KernelException"></exception>
        public static void AddBootApp(string name, BaseEnvironment environment)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new KernelException(KernelExceptionType.Bootloader, Translate.DoTranslation("Boot app name is not specified"));
            if (environment is null)
                throw new KernelException(KernelExceptionType.Bootloader, Translate.DoTranslation("Boot app environment is not specified"));
            if (CheckBootApp(name))
                throw new KernelException(KernelExceptionType.Bootloader, Translate.DoTranslation("Boot app already exists"));
            bootApps.Add(name, environment);
        }

        /// <summary>
        /// Removes a bootable application
        /// </summary>
        /// <param name="name">Bootable application name</param>
        /// <exception cref="KernelException"></exception>
        public static void RemoveBootApp(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new KernelException(KernelExceptionType.Bootloader, Translate.DoTranslation("Boot app name is not specified"));
            if (!CheckBootApp(name))
                throw new KernelException(KernelExceptionType.Bootloader, Translate.DoTranslation("Boot app doesn't exist"));
            bootApps.Remove(name);
        }

        /// <summary>
        /// Checks for a bootable application
        /// </summary>
        /// <param name="name">Bootable application name</param>
        /// <exception cref="KernelException"></exception>
        public static bool CheckBootApp(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new KernelException(KernelExceptionType.Bootloader, Translate.DoTranslation("Boot app name is not specified"));
            return bootApps.ContainsKey(name);
        }

        /// <summary>
        /// Gets boot application by name
        /// </summary>
        public static BaseEnvironment? GetBootApp(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new KernelException(KernelExceptionType.Bootloader, Translate.DoTranslation("Boot app name is not specified"));
            bootApps.TryGetValue(name, out var info);
            DebugWriter.WriteDebug(DebugLevel.I, "Got boot app {0}!", vars: [name]);
            return info;
        }

        /// <summary>
        /// Gets boot application name by index
        /// </summary>
        public static string GetBootAppNameByIndex(int index)
        {
            for (int i = 0; i < bootApps.Count; i++)
            {
                if (i == index)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Got boot app at index {0}!", vars: [index]);
                    return bootApps.ElementAt(index).Key;
                }
            }

            DebugWriter.WriteDebug(DebugLevel.W, "No boot app at index {0}. Returning empty string...", vars: [index]);
            return "";
        }
    }
}
