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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Files.Paths
{
    /// <summary>
    /// Paths module
    /// </summary>
    public static class PathsManagement
    {
        internal static bool isTest = false;
        private static readonly string[] knownKernelPathTypes = Enum.GetNames<KernelPathType>();
        private static readonly Dictionary<string, string> customPaths = [];
        private static readonly Dictionary<string, (Func<string>, bool)> knownPaths = new()
        {
            { $"{KernelPathType.Aliases}",             (() => AliasesPath, true) },
            { $"{KernelPathType.Configuration}",       (() => ConfigurationPath, true) },
            { $"{KernelPathType.CustomLanguages}",     (() => CustomLanguagesPath, true) },
            { $"{KernelPathType.DebugDevices}",        (() => DebugDevicesPath, true) },
            { $"{KernelPathType.Events}",              (() => EventsPath, true) },
            { $"{KernelPathType.SpeedDial}",           (() => SpeedDialPath, true) },
            { $"{KernelPathType.MAL}",                 (() => MALPath, true) },
            { $"{KernelPathType.Mods}",                (() => ModsPath, true) },
            { $"{KernelPathType.MOTD}",                (() => MOTDPath, true) },
            { $"{KernelPathType.Reminders}",           (() => RemindersPath, true) },
            { $"{KernelPathType.Users}",               (() => UsersPath, true) },
            { $"{KernelPathType.Journaling}",          (() => JournalingPath, true) },
            { $"{KernelPathType.Contacts}",            (() => ContactsPath, true) },
            { $"{KernelPathType.ContactsImport}",      (() => ContactsImportPath, true) },
            { $"{KernelPathType.SaverConfiguration}",  (() => SaverConfigurationPath, true) },
            { $"{KernelPathType.DriverConfiguration}", (() => DriverConfigurationPath, true) },
            { $"{KernelPathType.ToDoList}",            (() => ToDoListPath, true) },
            { $"{KernelPathType.UserGroups}",          (() => UserGroupsPath, true) },
            { $"{KernelPathType.Addons}",              (() => AddonsPath, false) },
            { $"{KernelPathType.ShellHistories}",      (() => ShellHistoriesPath, true) },
            { $"{KernelPathType.Consents}",            (() => ConsentsPath, true) },
            { $"{KernelPathType.ExtensionHandlers}",   (() => ExtensionHandlersPath, true) },
            { $"{KernelPathType.NotificationRecents}", (() => NotificationRecentsPath, true) },
            { $"{KernelPathType.WidgetConfiguration}", (() => WidgetConfigurationPath, true) },
            { $"{KernelPathType.AddonsEssentials}",    (() => AddonsEssentialsPath, false) },
        };

        /// <summary>
        /// Path to KS executable folder
        /// </summary>
        public static string ExecPath
        {
            get
            {
                string? dirName = Path.GetDirectoryName(typeof(PathsManagement).Assembly.Location);
                if (dirName is null)
                    return "";
                return KernelPlatform.IsOnUnix() ? dirName : dirName.Replace(@"\", "/");
            }
        }

        /// <summary>
        /// Platform-dependent home path
        /// </summary>
        public static string HomePath
        {
            get
            {
                if (KernelPlatform.IsOnUnix())
                    return Environment.GetEnvironmentVariable("HOME") ?? "";
                else
                    return (Environment.GetEnvironmentVariable("USERPROFILE") ?? "").Replace(@"\", "/");
            }
        }

        /// <summary>
        /// Platform-dependent application data path
        /// </summary>
        public static string AppDataPath
        {
            get
            {
                if (!isTest)
                {
                    if (KernelPlatform.IsOnUnix())
                        return Environment.GetEnvironmentVariable("HOME") + "/.config/ks";
                    else
                        return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/KS").Replace("\\", "/");
                }
                else
                    return TempPath + "/kstest";
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
                    return "/tmp";
                else
                    return (Environment.GetEnvironmentVariable("TEMP") ?? "").Replace(@"\", "/");
            }
        }

        /// <summary>
        /// Path to KS addons folder
        /// </summary>
        public static string AddonsPath =>
            FilesystemTools.NeutralizePath(ExecPath + "/Addons");

        /// <summary>
        /// Path to KS essential addons folder
        /// </summary>
        public static string AddonsEssentialsPath =>
            FilesystemTools.NeutralizePath(ExecPath + "/Addons.Essentials");

        /// <summary>
        /// Mods path
        /// </summary>
        public static string ModsPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KSMods/");

        /// <summary>
        /// Configuration path
        /// </summary>
        public static string ConfigurationPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KernelMainConfig.json");

        /// <summary>
        /// Aliases path
        /// </summary>
        public static string AliasesPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/Aliases.json");

        /// <summary>
        /// Users path
        /// </summary>
        public static string UsersPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/Users.json");

        /// <summary>
        /// Speed dial path
        /// </summary>
        public static string SpeedDialPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/SpeedDial.json");

        /// <summary>
        /// Debug devices path
        /// </summary>
        public static string DebugDevicesPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/DebugDevices.json");

        /// <summary>
        /// MOTD path
        /// </summary>
        public static string MOTDPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/MOTD.txt");

        /// <summary>
        /// MAL path
        /// </summary>
        public static string MALPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/MAL.txt");

        /// <summary>
        /// Events path
        /// </summary>
        public static string EventsPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KSEvents/");

        /// <summary>
        /// Reminders path
        /// </summary>
        public static string RemindersPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KSReminders/");

        /// <summary>
        /// CustomLanguages path
        /// </summary>
        public static string CustomLanguagesPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KSLanguages/");

        /// <summary>
        /// Journaling path
        /// </summary>
        public static string JournalingPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KSJournal.json");

        /// <summary>
        /// Contacts path
        /// </summary>
        public static string ContactsPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KSContacts/");

        /// <summary>
        /// Contacts path
        /// </summary>
        public static string ContactsImportPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KSContactsImport/");

        /// <summary>
        /// Configuration path
        /// </summary>
        public static string SaverConfigurationPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KernelSaverConfig.json");

        /// <summary>
        /// Configuration path
        /// </summary>
        public static string DriverConfigurationPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KernelDriverConfig.json");

        /// <summary>
        /// Configuration path
        /// </summary>
        public static string SplashConfigurationPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KernelSplashConfig.json");

        /// <summary>
        /// User groups path
        /// </summary>
        public static string UserGroupsPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/UserGroups.json");

        /// <summary>
        /// To-do list path
        /// </summary>
        public static string ToDoListPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/ToDoList.json");

        /// <summary>
        /// Shell histories path
        /// </summary>
        public static string ShellHistoriesPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/ShellHistories.json");

        /// <summary>
        /// Privacy consents path
        /// </summary>
        public static string ConsentsPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/Consents.json");

        /// <summary>
        /// Extension handlers path
        /// </summary>
        public static string ExtensionHandlersPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/ExtensionHandlers.json");

        /// <summary>
        /// Recent notifications path
        /// </summary>
        public static string NotificationRecentsPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/NotificationRecents.json");

        /// <summary>
        /// Configuration path
        /// </summary>
        public static string WidgetConfigurationPath =>
            FilesystemTools.NeutralizePath(AppDataPath + "/KernelWidgetsConfig.json");

        /// <summary>
        /// Gets the kernel path name from the list of known path types
        /// </summary>
        /// <param name="PathType">Kernel path type</param>
        /// <returns>A kernel path name</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetKernelPathName(KernelPathType PathType)
        {
            string typeString = $"{PathType}";
            foreach (string typeName in knownKernelPathTypes)
            {
                if (typeName == typeString)
                    return typeName;
            }
            throw new KernelException(KernelExceptionType.InvalidKernelPath, LanguageTools.GetLocalized("NKS_FILES_PATHS_PATHTYPEINVALID"));
        }

        /// <summary>
        /// Gets the neutralized kernel path
        /// </summary>
        /// <param name="PathType">Kernel path type</param>
        /// <returns>A kernel path</returns>
        public static string GetKernelPath(KernelPathType PathType)
        {
            string name = GetKernelPathName(PathType);
            return GetKernelPath(name);
        }

        /// <summary>
        /// Gets the neutralized kernel path
        /// </summary>
        /// <param name="PathType">Kernel path type</param>
        /// <returns>A kernel path</returns>
        public static string GetKernelPath(string PathType)
        {
            if (knownPaths.TryGetValue(PathType, out (Func<string>, bool) pathDelegate))
                return pathDelegate.Item1();
            if (customPaths.TryGetValue(PathType, out string? path))
                return path;
            throw new KernelException(KernelExceptionType.InvalidKernelPath, LanguageTools.GetLocalized("NKS_FILES_PATHS_PATHTYPEINVALID"));
        }

        /// <summary>
        /// Checks to see if the kernel path is built-in
        /// </summary>
        /// <param name="pathType">Path type to check</param>
        /// <returns>True if registered in the built-in path list. Otherwise, false.</returns>
        public static bool IsKernelPathBuiltin(string pathType) =>
            knownKernelPathTypes.Contains(pathType);

        /// <summary>
        /// Checks to see if the kernel path is registered
        /// </summary>
        /// <param name="pathType">Path type to check</param>
        /// <returns>True if registered in either the built-in or the custom path list. Otherwise, false.</returns>
        public static bool IsKernelPathRegistered(string pathType) =>
            IsKernelPathBuiltin(pathType) ||
            customPaths.ContainsKey(pathType);

        /// <summary>
        /// Registers a custom kernel path
        /// </summary>
        /// <param name="pathType">Path type to register</param>
        /// <param name="path">Path to a target (doesn't necessarily need to exist)</param>
        /// <exception cref="KernelException"></exception>
        public static void RegisterKernelPath(string pathType, string path)
        {
            // Check to see if the path type has been provided
            if (string.IsNullOrEmpty(pathType))
                throw new KernelException(KernelExceptionType.InvalidKernelPath, LanguageTools.GetLocalized("NKS_FILES_PATHS_EXCEPTION_NOTYPE"));

            // Now, check for registration
            if (IsKernelPathRegistered(pathType))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_PATHS_EXCEPTION_TYPEEXISTS"));

            // Check the path (not necessarily exists)
            if (string.IsNullOrEmpty(path))
                throw new KernelException(KernelExceptionType.InvalidKernelPath, LanguageTools.GetLocalized("NKS_FILES_PATHS_EXCEPTION_NOPATH"));

            // Now, register the kernel path to the custom path list
            customPaths.Add(pathType, path);
        }

        /// <summary>
        /// Unregisters a custom kernel path
        /// </summary>
        /// <param name="pathType">Path type to unregister</param>
        /// <exception cref="KernelException"></exception>
        public static void UnregisterKernelPath(string pathType)
        {
            // Check to see if the path type has been provided
            if (string.IsNullOrEmpty(pathType))
                throw new KernelException(KernelExceptionType.InvalidKernelPath, LanguageTools.GetLocalized("NKS_FILES_PATHS_EXCEPTION_NOTYPE"));

            // Now, check for registration
            if (!IsKernelPathRegistered(pathType))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_PATHS_EXCEPTION_TYPENOTEXISTS"));

            // Now, unregister the kernel path to the custom path list
            customPaths.Remove(pathType);
        }

        /// <summary>
        /// Checks to see if the provided path is resettable
        /// </summary>
        /// <param name="PathType">Kernel path type</param>
        /// <returns>True if we're able to wipe. Otherwise, false.</returns>
        public static bool IsResettable(KernelPathType PathType)
        {
            string name = GetKernelPathName(PathType);
            if (knownPaths.TryGetValue(name, out (Func<string>, bool) pathDelegate))
                return pathDelegate.Item2;
            throw new KernelException(KernelExceptionType.InvalidKernelPath, LanguageTools.GetLocalized("NKS_FILES_PATHS_PATHTYPEINVALID"));
        }

    }
}
