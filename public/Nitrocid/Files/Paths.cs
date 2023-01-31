
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

using System;
using System.Reflection;
using KS.Kernel;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Files
{
    /// <summary>
    /// Paths module
    /// </summary>
    public static class Paths
    {

        // Basic paths

        /// <summary>
        /// Path to KS executable folder
        /// </summary>
        public static string ExecPath =>
            System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Platform-dependent home path
        /// </summary>
        public static string HomePath
        {
            get
            {
                if (KernelPlatform.IsOnUnix())
                {
                    return Environment.GetEnvironmentVariable("HOME");
                }
                else
                {
                    return Environment.GetEnvironmentVariable("USERPROFILE").Replace(@"\", "/");
                }
            }
        }

        /// <summary>
        /// Platform-dependent application data path
        /// </summary>
        public static string AppDataPath
        {
            get
            {
                if (KernelPlatform.IsOnUnix())
                {
                    return Environment.GetEnvironmentVariable("HOME") + "/.config/ks";
                }
                else
                {
                    return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/KS").Replace("\\", "/");
                }
            }
        }

        /// <summary>
        /// Platform-dependent temp path
        /// </summary>
        public static string TempPath
        {
            get
            {
                if (KernelPlatform.IsOnUnix())
                {
                    return "/tmp";
                }
                else
                {
                    return Environment.GetEnvironmentVariable("TEMP").Replace(@"\", "/");
                }
            }
        }

        /// <summary>
        /// Retro Nitrocid KS download path
        /// </summary>
        public static string RetroKSDownloadPath
        {
            get
            {
                if (KernelPlatform.IsOnUnix())
                {
                    return Environment.GetEnvironmentVariable("HOME") + "/.config/retroks/exec/coreclr";
                }
                else
                {
                    return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/RetroKS/exec/coreclr").Replace("\\", "/");
                }
            }
        }

        // Nitrocid KS paths

        /// <summary>
        /// Mods path
        /// </summary>
        public static string ModsPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSMods/");

        /// <summary>
        /// Configuration path
        /// </summary>
        public static string ConfigurationPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KernelConfig.json");

        /// <summary>
        /// Debugging path
        /// </summary>
        public static string DebuggingPath =>
            Filesystem.NeutralizePath(AppDataPath + "/kernelDbg.log");

        /// <summary>
        /// Aliases path
        /// </summary>
        public static string AliasesPath =>
            Filesystem.NeutralizePath(AppDataPath + "/Aliases.json");

        /// <summary>
        /// Users path
        /// </summary>
        public static string UsersPath =>
            Filesystem.NeutralizePath(AppDataPath + "/Users.json");

        /// <summary>
        /// FTPSpeedDial path
        /// </summary>
        public static string FTPSpeedDialPath =>
            Filesystem.NeutralizePath(AppDataPath + "/FTP_SpeedDial.json");

        /// <summary>
        /// SFTPSpeedDial path
        /// </summary>
        public static string SFTPSpeedDialPath =>
            Filesystem.NeutralizePath(AppDataPath + "/SFTP_SpeedDial.json");

        /// <summary>
        /// DebugDevNames path
        /// </summary>
        public static string DebugDevNamesPath =>
            Filesystem.NeutralizePath(AppDataPath + "/DebugDeviceNames.json");

        /// <summary>
        /// MOTD path
        /// </summary>
        public static string MOTDPath =>
            Filesystem.NeutralizePath(AppDataPath + "/MOTD.txt");

        /// <summary>
        /// MAL path
        /// </summary>
        public static string MALPath =>
            Filesystem.NeutralizePath(AppDataPath + "/MAL.txt");

        /// <summary>
        /// CustomSaverSettings path
        /// </summary>
        public static string CustomSaverSettingsPath =>
            Filesystem.NeutralizePath(AppDataPath + "/CustomSaverSettings.json");

        /// <summary>
        /// Events path
        /// </summary>
        public static string EventsPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSEvents/");

        /// <summary>
        /// Reminders path
        /// </summary>
        public static string RemindersPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSReminders/");

        /// <summary>
        /// CustomLanguages path
        /// </summary>
        public static string CustomLanguagesPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSLanguages/");

        /// <summary>
        /// CustomSplashes path
        /// </summary>
        public static string CustomSplashesPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSSplashes/");

        /// <summary>
        /// Journalling path
        /// </summary>
        public static string JournallingPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSJournal.json");

        /// <summary>
        /// Screensavers path
        /// </summary>
        public static string ScreensaversPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSScreensavers/");

        /// <summary>
        /// Gets the neutralized kernel path
        /// </summary>
        /// <param name="PathType">Kernel path type</param>
        /// <returns>A kernel path</returns>
        public static string GetKernelPath(KernelPathType PathType)
        {
            return PathType switch
            {
                KernelPathType.Aliases =>               AliasesPath,
                KernelPathType.Configuration =>         ConfigurationPath,
                KernelPathType.CustomLanguages =>       CustomLanguagesPath,
                KernelPathType.CustomSaverSettings =>   CustomSaverSettingsPath,
                KernelPathType.CustomSplashes =>        CustomSplashesPath,
                KernelPathType.DebugDevNames =>         DebugDevNamesPath,
                KernelPathType.Debugging =>             DebuggingPath,
                KernelPathType.Events =>                EventsPath,
                KernelPathType.FTPSpeedDial =>          FTPSpeedDialPath,
                KernelPathType.MAL =>                   MALPath,
                KernelPathType.Mods =>                  ModsPath,
                KernelPathType.MOTD =>                  MOTDPath,
                KernelPathType.Reminders =>             RemindersPath,
                KernelPathType.SFTPSpeedDial =>         SFTPSpeedDialPath,
                KernelPathType.Users =>                 UsersPath,
                KernelPathType.Journalling =>           JournallingPath,
                KernelPathType.Screensavers =>          ScreensaversPath,
                _ => throw new KernelException(KernelExceptionType.InvalidKernelPath, Translate.DoTranslation("Invalid kernel path type.")),
            };
        }

    }
}
