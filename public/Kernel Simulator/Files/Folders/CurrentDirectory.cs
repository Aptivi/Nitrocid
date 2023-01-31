
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

using System;
using System.IO;
using Extensification.StringExts;
using KS.Files.Querying;
using KS.Kernel.Configuration;
using KS.Languages;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Kernel.Events;

namespace KS.Files.Folders
{
    /// <summary>
    /// Current directory module
    /// </summary>
    public static class CurrentDirectory
    {

        private static string _CurrentDirectory = Paths.HomePath;

        /// <summary>
        /// The current directory
        /// </summary>
        public static string CurrentDir
        {
            get
            {
                return _CurrentDirectory;
            }
            set
            {
                Filesystem.ThrowOnInvalidPath(value);
                value = Filesystem.NeutralizePath(value);
                if (Checking.FolderExists(value))
                {
                    _CurrentDirectory = value;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Directory {0} not found").FormatString(value));
                }
            }
        }

        /// <summary>
        /// Sets the current working directory
        /// </summary>
        /// <param name="dir">A directory</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void SetCurrDir(string dir)
        {
            CurrentDir = dir;

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

        /// <summary>
        /// Saves the current directory to configuration
        /// </summary>
        public static void SaveCurrDir()
        {
            var Token = ConfigTools.GetConfigCategory(ConfigCategory.Shell);
            ConfigTools.SetConfigValue(ConfigCategory.Shell, Token, "Current Directory", CurrentDir);
        }

        /// <summary>
        /// Tries to set the current directory to configuration
        /// </summary>
        /// <returns>True if successful; otherwise, false.</returns>
        public static bool TrySaveCurrDir()
        {
            try
            {
                SaveCurrDir();
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to save current directory: {0}", ex.Message);
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Failed to save current directory: {0}"), ex, ex.Message);
            }
        }

    }
}
