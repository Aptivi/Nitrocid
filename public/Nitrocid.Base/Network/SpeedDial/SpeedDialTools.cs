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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Nitrocid.Base.Files;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.Paths;

namespace Nitrocid.Base.Network.SpeedDial
{
    /// <summary>
    /// Speed dial management tools
    /// </summary>
    public static class SpeedDialTools
    {

        private static List<SpeedDialEntry> speedDialEntries = [];

        /// <summary>
        /// Gets a speed dial entry
        /// </summary>
        /// <param name="Address">Address to look for</param>
        /// <param name="Port">Port to look for</param>
        /// <param name="SpeedDialType">Speed dial type to look for</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="arguments">Arguments to look for</param>
        /// <returns>A <see cref="SpeedDialEntry"/> instance if found</returns>
        public static SpeedDialEntry? GetSpeedDialEntry(string Address, int Port, NetworkConnectionType SpeedDialType, string username, string password, Dictionary<string, object> arguments) =>
            GetSpeedDialEntry(Address, Port, SpeedDialType.ToString(), username, password, arguments);

        /// <summary>
        /// Gets a speed dial entry
        /// </summary>
        /// <param name="Address">Address to look for</param>
        /// <param name="Port">Port to look for</param>
        /// <param name="SpeedDialType">Speed dial type to look for</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="arguments">Arguments to look for</param>
        /// <returns>A <see cref="SpeedDialEntry"/> instance if found</returns>
        public static SpeedDialEntry? GetSpeedDialEntry(string Address, int Port, string SpeedDialType, string username, string password, Dictionary<string, object> arguments)
        {
            if (speedDialEntries.Count == 0)
                return null;
            var entry = speedDialEntries.FirstOrDefault((sde) =>
                sde.Address == Address &&
                sde.Port == Port &&
                sde.Type == SpeedDialType &&
                sde.Username == username &&
                sde.Password == password &&
                sde.Options.SequenceEqual(arguments)
            );
            return entry;
        }

        /// <summary>
        /// Gets a speed dial entry
        /// </summary>
        /// <param name="Address">Address to look for</param>
        /// <param name="Port">Port to look for</param>
        /// <param name="SpeedDialType">Speed dial type to look for</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>A <see cref="SpeedDialEntry"/> instance if found</returns>
        public static SpeedDialEntry? GetSpeedDialEntry(string Address, int Port, NetworkConnectionType SpeedDialType, string username, string password) =>
            GetSpeedDialEntry(Address, Port, SpeedDialType.ToString(), username, password);

        /// <summary>
        /// Gets a speed dial entry
        /// </summary>
        /// <param name="Address">Address to look for</param>
        /// <param name="Port">Port to look for</param>
        /// <param name="SpeedDialType">Speed dial type to look for</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>A <see cref="SpeedDialEntry"/> instance if found</returns>
        public static SpeedDialEntry? GetSpeedDialEntry(string Address, int Port, string SpeedDialType, string username, string password)
        {
            if (speedDialEntries.Count == 0)
                return null;
            var entry = speedDialEntries.FirstOrDefault((sde) =>
                sde.Address == Address &&
                sde.Port == Port &&
                sde.Type == SpeedDialType &&
                sde.Username == username &&
                sde.Password == password
            );
            return entry;
        }

        /// <summary>
        /// Adds an entry to speed dial
        /// </summary>
        /// <param name="Address">A speed dial address</param>
        /// <param name="Port">A speed dial port</param>
        /// <param name="SpeedDialType">Speed dial type</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="ThrowException">Optionally throw exception</param>
        /// <param name="arguments">List of arguments to pass to the entry</param>
        public static void AddEntryToSpeedDial(string Address, int Port, NetworkConnectionType SpeedDialType, string username, string password, bool ThrowException = true, Dictionary<string, object>? arguments = null) =>
            AddEntryToSpeedDial(Address, Port, SpeedDialType.ToString(), username, password, ThrowException, arguments);

        /// <summary>
        /// Adds an entry to speed dial
        /// </summary>
        /// <param name="Address">A speed dial address</param>
        /// <param name="Port">A speed dial port</param>
        /// <param name="SpeedDialType">Speed dial type</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="ThrowException">Optionally throw exception</param>
        /// <param name="arguments">List of arguments to pass to the entry</param>
        public static void AddEntryToSpeedDial(string Address, int Port, string SpeedDialType, string username, string password, bool ThrowException = true, Dictionary<string, object>? arguments = null)
        {
            // Parse the entry
            arguments ??= [];
            var entryCheck = GetSpeedDialEntry(Address, Port, SpeedDialType, username, password, arguments);
            if (entryCheck is null)
            {
                // Check the type
                if (!NetworkConnectionTools.ConnectionTypeExists(SpeedDialType))
                {
                    if (ThrowException)
                        throw new KernelException(KernelExceptionType.Network, LanguageTools.GetLocalized("NKS_NETWORK_SPEEDDIAL_EXCEPTION_TYPENOTFOUND"));
                    else
                        return;
                }

                // The entry doesn't exist. Go ahead and create it.
                var dialEntry = new SpeedDialEntry(Address, Port, SpeedDialType, username, password, arguments);

                // Add the entry and write it to the file
                speedDialEntries.Add(dialEntry);
                SaveAll();
            }
            else if (ThrowException)
            {
                // Entry already exists! Throw an exception if needed.
                throw new KernelException(KernelExceptionType.Network, LanguageTools.GetLocalized("NKS_NETWORK_SPEEDDIAL_EXCEPTION_ENTRYALREADYEXISTS"));
            }
        }

        /// <summary>
        /// Adds an entry to speed dial
        /// </summary>
        /// <param name="Address">A speed dial address</param>
        /// <param name="Port">A speed dial port</param>
        /// <param name="SpeedDialType">Speed dial type</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="ThrowException">Optionally throw exception</param>
        /// <param name="arguments">List of arguments to pass to the entry</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryAddEntryToSpeedDial(string Address, int Port, NetworkConnectionType SpeedDialType, string username, string password, bool ThrowException = true, Dictionary<string, object>? arguments = null) =>
            TryAddEntryToSpeedDial(Address, Port, SpeedDialType.ToString(), username, password, ThrowException, arguments);

        /// <summary>
        /// Adds an entry to speed dial
        /// </summary>
        /// <param name="Address">A speed dial address</param>
        /// <param name="Port">A speed dial port</param>
        /// <param name="SpeedDialType">Speed dial type</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="ThrowException">Optionally throw exception</param>
        /// <param name="arguments">List of arguments to pass to the entry</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryAddEntryToSpeedDial(string Address, int Port, string SpeedDialType, string username, string password, bool ThrowException = true, Dictionary<string, object>? arguments = null)
        {
            try
            {
                AddEntryToSpeedDial(Address, Port, SpeedDialType, username, password, ThrowException, arguments);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Lists all speed dial entries
        /// </summary>
        /// <returns>A list</returns>
        public static SpeedDialEntry[] ListSpeedDialEntries() =>
            [.. speedDialEntries];

        /// <summary>
        /// Lists all speed dial entries by type
        /// </summary>
        /// <returns>A list</returns>
        public static SpeedDialEntry[] ListSpeedDialEntriesByType(NetworkConnectionType SpeedDialType) =>
            ListSpeedDialEntriesByType(SpeedDialType.ToString());

        /// <summary>
        /// Lists all speed dial entries by type
        /// </summary>
        /// <returns>A list</returns>
        public static SpeedDialEntry[] ListSpeedDialEntriesByType(string SpeedDialType) =>
            speedDialEntries.Where((sde) => sde.Type == SpeedDialType).ToArray();

        /// <summary>
        /// Saves all the speed dial entries
        /// </summary>
        public static void SaveAll() =>
            FilesystemTools.WriteContentsText(PathsManagement.GetKernelPath(KernelPathType.SpeedDial), JsonConvert.SerializeObject(speedDialEntries, Formatting.Indented));

        /// <summary>
        /// Loads all the speed dial entries
        /// </summary>
        public static void LoadAll()
        {
            string path = PathsManagement.GetKernelPath(KernelPathType.SpeedDial);
            if (!FilesystemTools.FileExists(path))
                return;
            speedDialEntries = JsonConvert.DeserializeObject<List<SpeedDialEntry>>(FilesystemTools.ReadContentsText(path)) ?? [];
        }
    }
}
