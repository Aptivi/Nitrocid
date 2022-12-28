
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

using System.Collections.Generic;

namespace KSConverter
{
    static class ConverterTools
    {
        /// <summary>
        /// Gets all paths that are used in 0.0.15.x or earlier kernels.
        /// </summary>
        /// <param name="AppendedPath">Optional path to append</param>
        public static Dictionary<string, string> GetOldPaths(string AppendedPath)
        {
            // Initialize all needed variables
            var OldPaths = new Dictionary<string, string>();

            // Check to see if we're appending new path name
            if (!string.IsNullOrEmpty(AppendedPath))
                AppendedPath = $"/{AppendedPath}";

            // Populate our dictionary with old paths
            OldPaths.Add("Configuration", GetHomeDirectory() + $"{AppendedPath}/kernelConfig.ini");
            OldPaths.Add("Aliases", GetHomeDirectory() + $"{AppendedPath}/aliases.csv");
            OldPaths.Add("Users", GetHomeDirectory() + $"{AppendedPath}/users.csv");
            OldPaths.Add("FTPSpeedDial", GetHomeDirectory() + $"{AppendedPath}/ftp_speeddial.csv");
            OldPaths.Add("BlockedDevices", GetHomeDirectory() + $"{AppendedPath}/blocked_devices.csv");

            // Return it.
            return OldPaths;
        }

        /// <summary>
        /// Gets all paths that are used in 0.0.16.x or later kernels.
        /// </summary>
        public static Dictionary<string, string> GetNewPaths()
        {
            // Initialize all needed variables
            var NewPaths = new Dictionary<string, string>
            {
                // Populate our dictionary with old paths
                { "Configuration", GetHomeDirectory() + "/KernelConfig.json" },
                { "Aliases", GetHomeDirectory() + "/Aliases.json" },
                { "Users", GetHomeDirectory() + "/Users.json" },
                { "FTPSpeedDial", GetHomeDirectory() + "/FTP_SpeedDial.json" },
                { "DebugDevNames", GetHomeDirectory() + "/DebugDeviceNames.json" }
            };

            // Return it.
            return NewPaths;
        }

        /// <summary>
        /// Gets home directory depending on platform
        /// </summary>
        public static string GetHomeDirectory() => KS.Files.Paths.HomePath;

    }
}
