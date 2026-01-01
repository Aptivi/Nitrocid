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

namespace Nitrocid.Security.Permissions
{
    /// <summary>
    /// All permission types
    /// </summary>
    public enum PermissionTypes
    {
        /// <summary>
        /// Allows the user to manage power
        /// </summary>
        ManagePower = 1,
        /// <summary>
        /// Allows the user to manage permissions
        /// </summary>
        ManagePermissions = 2,
        /// <summary>
        /// Allows the user to run strict commands
        /// </summary>
        RunStrictCommands = 4,
        /// <summary>
        /// Allows the user to perform the filesystem operations
        /// </summary>
        ManageFilesystem = 8,
        /// <summary>
        /// Allows the user to manipulate with the kernel settings
        /// </summary>
        ManipulateSettings = 16,
        /// <summary>
        /// Allows the user to execute UESH scripts
        /// </summary>
        ExecuteScripts = 32,
        /// <summary>
        /// Allows the user to execute processes
        /// </summary>
        ExecuteProcesses = 64,
        /// <summary>
        /// Allows the user to manage the users
        /// </summary>
        ManageUsers = 128,
        /// <summary>
        /// Allows the user to manage the kernel mods
        /// </summary>
        ManageMods = 256,
        /// <summary>
        /// Allows the user to manage the user groups
        /// </summary>
        ManageGroups = 512,
        /// <summary>
        /// Allows the user to run mod commands that depend on inter-mod communication
        /// </summary>
        IntermodCommunication = 1024,
        /// <summary>
        /// Allows the user to open an administrative shell
        /// </summary>
        OpenAdminShell = 2048,
        /// <summary>
        /// Allows the user to open a debug shell
        /// </summary>
        OpenDebugShell = 4096,
        /// <summary>
        /// Allows the user to run mod commands that depend on inter-mod communication
        /// </summary>
        InteraddonCommunication = 8192,
        /// <summary>
        /// Allows the user to use the sudo command
        /// </summary>
        UseSudo = 16384,
        /// <summary>
        /// Allows the user to manage the drivers
        /// </summary>
        ManageDrivers = 32768,
    }
}
