
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
using System.Reflection;
using KS.Kernel;
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
        public static string ExecPath => System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
        /// Retro Kernel Simulator download path
        /// </summary>
        public static string RetroKSDownloadPath
        {
            get
            {
#if NETCOREAPP
                if (KernelPlatform.IsOnUnix())
                {
                    return Environment.GetEnvironmentVariable("HOME") + "/.config/retroks/exec/coreclr";
                }
                else
                {
                    return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/RetroKS/exec/coreclr").Replace("\\", "/");
                }
#else
                if (KernelPlatform.IsOnUnix())
                {
                    return Environment.GetEnvironmentVariable("HOME") + "/.config/retroks/exec/fx";
                }
                else
                {
                    return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/RetroKS/exec/fx").Replace(@"\", "/");
                }
#endif
            }
        }

        // Kernel Simulator paths

        /// <summary>
        /// Mods path
        /// </summary>
        public static string ModsPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/KSMods/");
            Filesystem.NeutralizePath(HomePath + "/KSMods/");

        /// <summary>
        /// Configuration path
        /// </summary>
        public static string ConfigurationPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/KernelConfig.json");
            Filesystem.NeutralizePath(HomePath + "/KernelConfig.json");

        /// <summary>
        /// Debugging path
        /// </summary>
        public static string DebuggingPath =>
            Filesystem.NeutralizePath(AppDataPath + "/kernelDbg.log");

        /// <summary>
        /// Aliases path
        /// </summary>
        public static string AliasesPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/Aliases.json");
            Filesystem.NeutralizePath(HomePath + "/Aliases.json");

        /// <summary>
        /// Users path
        /// </summary>
        public static string UsersPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/Users.json");
            Filesystem.NeutralizePath(HomePath + "/Users.json");

        /// <summary>
        /// FTPSpeedDial path
        /// </summary>
        public static string FTPSpeedDialPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/FTP_SpeedDial.json");
            Filesystem.NeutralizePath(HomePath + "/FTP_SpeedDial.json");

        /// <summary>
        /// SFTPSpeedDial path
        /// </summary>
        public static string SFTPSpeedDialPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/SFTP_SpeedDial.json");
            Filesystem.NeutralizePath(HomePath + "/SFTP_SpeedDial.json");

        /// <summary>
        /// DebugDevNames path
        /// </summary>
        public static string DebugDevNamesPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/DebugDeviceNames.json");
            Filesystem.NeutralizePath(HomePath + "/DebugDeviceNames.json");

        /// <summary>
        /// MOTD path
        /// </summary>
        public static string MOTDPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/MOTD.txt");
            Filesystem.NeutralizePath(HomePath + "/MOTD.txt");

        /// <summary>
        /// MAL path
        /// </summary>
        public static string MALPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/MAL.txt");
            Filesystem.NeutralizePath(HomePath + "/MAL.txt");

        /// <summary>
        /// CustomSaverSettings path
        /// </summary>
        public static string CustomSaverSettingsPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/CustomSaverSettings.json");
            Filesystem.NeutralizePath(HomePath + "/CustomSaverSettings.json");

        /// <summary>
        /// Events path
        /// </summary>
        public static string EventsPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/KSEvents/");
            Filesystem.NeutralizePath(HomePath + "/KSEvents/");

        /// <summary>
        /// Reminders path
        /// </summary>
        public static string RemindersPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/KSReminders/");
            Filesystem.NeutralizePath(HomePath + "/KSReminders/");

        /// <summary>
        /// CustomLanguages path
        /// </summary>
        public static string CustomLanguagesPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/KSLanguages/");
            Filesystem.NeutralizePath(HomePath + "/KSLanguages/");

        /// <summary>
        /// CustomSplashes path
        /// </summary>
        public static string CustomSplashesPath =>
            // return Filesystem.NeutralizePath(AppDataPath + "/KSSplashes/");
            Filesystem.NeutralizePath(HomePath + "/KSSplashes/");

        /// <summary>
        /// Journalling path
        /// </summary>
        public static string JournallingPath =>
            Filesystem.NeutralizePath(AppDataPath + "/KSJournal.json");

        /// <summary>
        /// Gets the neutralized kernel path
        /// </summary>
        /// <param name="PathType">Kernel path type</param>
        /// <returns>A kernel path</returns>
        /// <exception cref="Kernel.Exceptions.InvalidKernelPathException"></exception>
        public static string GetKernelPath(KernelPathType PathType)
        {
            switch (PathType)
            {
                case KernelPathType.Aliases:
                    {
                        return AliasesPath;
                    }
                case KernelPathType.Configuration:
                    {
                        return ConfigurationPath;
                    }
                case KernelPathType.CustomLanguages:
                    {
                        return CustomLanguagesPath;
                    }
                case KernelPathType.CustomSaverSettings:
                    {
                        return CustomSaverSettingsPath;
                    }
                case KernelPathType.CustomSplashes:
                    {
                        return CustomSplashesPath;
                    }
                case KernelPathType.DebugDevNames:
                    {
                        return DebugDevNamesPath;
                    }
                case KernelPathType.Debugging:
                    {
                        return DebuggingPath;
                    }
                case KernelPathType.Events:
                    {
                        return EventsPath;
                    }
                case KernelPathType.FTPSpeedDial:
                    {
                        return FTPSpeedDialPath;
                    }
                case KernelPathType.MAL:
                    {
                        return MALPath;
                    }
                case KernelPathType.Mods:
                    {
                        return ModsPath;
                    }
                case KernelPathType.MOTD:
                    {
                        return MOTDPath;
                    }
                case KernelPathType.Reminders:
                    {
                        return RemindersPath;
                    }
                case KernelPathType.SFTPSpeedDial:
                    {
                        return SFTPSpeedDialPath;
                    }
                case KernelPathType.Users:
                    {
                        return UsersPath;
                    }
                case KernelPathType.Journalling:
                    {
                        return JournallingPath;
                    }

                default:
                    {
                        throw new Kernel.Exceptions.InvalidKernelPathException(Translate.DoTranslation("Invalid kernel path type."));
                    }
            }
        }

    }
}
