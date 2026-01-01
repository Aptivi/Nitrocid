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

using System;
using System.IO;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Events;

namespace Nitrocid.Files.Folders
{
    /// <summary>
    /// Current directory module
    /// </summary>
    public static class CurrentDirectory
    {

        internal static string _CurrentDirectory = PathsManagement.HomePath;

        /// <summary>
        /// The current directory
        /// </summary>
        public static string CurrentDir =>
            Config.baseConfigurations is not null && Config.MainConfig is not null ? Config.MainConfig.CurrentDir : PathsManagement.HomePath;

        /// <summary>
        /// Sets the current working directory
        /// </summary>
        /// <param name="dir">A directory</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void SetCurrDir(string dir)
        {
            Config.MainConfig.CurrentDir = dir;

            // Raise event
            EventsManager.FireEvent(EventType.CurrentDirectoryChanged);
        }

        /// <summary>
        /// Tries to set the current working directory
        /// </summary>
        /// <param name="dir">A directory</param>
        /// <returns>True if successful; otherwise, false.</returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static bool TrySetCurrDir(string dir)
        {
            try
            {
                SetCurrDir(dir);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to set current directory: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

    }
}
