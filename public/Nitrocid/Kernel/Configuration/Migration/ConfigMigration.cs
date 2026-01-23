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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.Files;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Network.Connections;
using Nitrocid.Network.SpeedDial;
using Terminaux.Shell.Aliases;
using Terminaux.Shell.Commands;
using Nitrocid.Users;
using Nitrocid.Users.Login.Motd;
using System;
using System.Linq;

namespace Nitrocid.Kernel.Configuration.Migration
{
    internal static class ConfigMigration
    {
        internal static bool MigrateAllConfig()
        {
            bool migratedMain = MigrateMainKernelConfig();
            bool migratedSaver = MigrateScreensaverKernelConfig();
            bool migratedAliases = MigrateAliases();
            bool migratedUsers = MigrateUsers();
            bool migratedFtp = MigrateFtpSpeedDial();
            bool migratedSftp = MigrateSftpSpeedDial();
            bool migratedDevices = MigrateDebugDevices();
            bool migratedMotd = MigrateMotd();
            bool migratedMal = MigrateMal();
            return migratedMain && migratedSaver && migratedAliases && migratedUsers && migratedFtp && migratedSftp && migratedDevices && migratedMotd && migratedMal;
        }

        private static bool MigrateMainKernelConfig()
        {
            // Locate the old user config file and parse it
            string oldMainKernelConfigPath = ConfigOldPaths.GetOldKernelPath(ConfigOldPathType.Configuration);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old kernel config path {0}.", vars: [oldMainKernelConfigPath]);
            if (!FilesystemTools.FileExists(oldMainKernelConfigPath))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Can't find old kernel config path.");
                return true;
            }

            // Configuration file could be zero bytes, so check it.
            string contents = FilesystemTools.ReadAllTextNoBlock(oldMainKernelConfigPath);
            if (contents.Length == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Old kernel config file has zero bytes.");
                return true;
            }
            var oldMainKernelConfigToken = JToken.Parse(contents);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old kernel config token.");

            // Handle any possible errors here.
            try
            {
                // Find the known sections and get all the matching configuration names inside them to compare them to the
                // Nitrocid settings entry so that we can get the variable from just the name
                var configEntries = Config.MainConfig.SettingsEntries;
                foreach (var configEntry in configEntries)
                {
                    // Parse this section
                    var keys = configEntry.Keys;
                    string section = configEntry.Name == "Shell Presets" ? "Shell" : configEntry.Name;
                    var sectionToken = oldMainKernelConfigToken[section];
                    if (sectionToken is null)
                        continue;
                    foreach (var key in keys)
                    {
                        // Parse this key
                        string keyName = key.Name.Original;
                        string keyVar = key.Variable;
                        var keyToken = sectionToken[keyName];

                        // Filter some of the non-portable properties
                        string[] blacklistedVars = ["MotdFilePath", "MalFilePath"];
                        if (blacklistedVars.Contains(keyVar))
                            continue;
                        if (keyToken is JValue keyValue)
                        {
                            // For character settings, we need to convert the value to a string to get its first character, because
                            // this value in the old configuration could consist of more than one character, and the SettingsKeyType.SChar
                            // requires exactly one character.
                            var value =
                                key.Type == SettingsKeyType.SChar ? (keyValue.Value<string>() ?? " ")[0] :
                                key.Type == SettingsKeyType.SBoolean ? keyValue.Value<int>() :
                                key.Type == SettingsKeyType.SInt ? keyValue.Value<int>() :
                                key.Type == SettingsKeyType.SIntSlider ? keyValue.Value<int>() :
                                keyValue.Value;

                            // Now, set the value
                            key.KeyInput.SetValue(key, value, Config.MainConfig);
                        }
                    }
                }

                // Save the configuration
                Config.CreateConfig();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to migrate main kernel config! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
            return true;
        }

        private static bool MigrateScreensaverKernelConfig()
        {
            // Locate the old user config file and parse it
            string oldMainKernelConfigPath = ConfigOldPaths.GetOldKernelPath(ConfigOldPathType.Configuration);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old kernel config path {0}.", vars: [oldMainKernelConfigPath]);
            if (!FilesystemTools.FileExists(oldMainKernelConfigPath))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Can't find old kernel config path.");
                return true;
            }

            // Configuration file could be zero bytes, so check it.
            string contents = FilesystemTools.ReadAllTextNoBlock(oldMainKernelConfigPath);
            if (contents.Length == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Old kernel config file has zero bytes.");
                return true;
            }
            var oldMainKernelConfigToken = JToken.Parse(contents);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old kernel config token.");

            // Handle any possible errors here.
            try
            {
                // Find the known sections and get all the matching configuration names inside them to compare them to the
                // Nitrocid settings entry so that we can get the variable from just the name
                var extraSaversConfig = Config.GetKernelConfig("ExtraSaversConfig");

                // Parse also the base saver config entries, just in case we promote an old screensaver to the base kernel
                // in a future release.
                var baseSaverConfigEntries = Config.SaverConfig.SettingsEntries;
                SettingsEntry[][] configEntryGroups =
                    extraSaversConfig is not null && extraSaversConfig.SettingsEntries is not null ?
                    [baseSaverConfigEntries, extraSaversConfig.SettingsEntries] :
                    [baseSaverConfigEntries];
                for (int i = 0; i < configEntryGroups.Length; i++)
                {
                    SettingsEntry[] configEntries = configEntryGroups[i];
                    foreach (var configEntry in configEntries)
                    {
                        // Parse this section
                        var keys = configEntry.Keys;
                        string section = configEntry.Name;
                        var sectionToken = oldMainKernelConfigToken["Screensaver"]?[section];
                        if (sectionToken is null)
                            continue;
                        foreach (var key in keys)
                        {
                            // Parse this key
                            string keyName = key.Name.Original;
                            string keyVar = key.Variable;
                            var keyToken = sectionToken[keyName];
                            if (keyToken is JValue keyValue)
                            {
                                // For character settings, we need to convert the value to a string to get its first character, because
                                // this value in the old configuration could consist of more than one character, and the SettingsKeyType.SChar
                                // requires exactly one character.
                                var value =
                                    key.Type == SettingsKeyType.SChar ? (keyValue.Value<string>() ?? " ")[0] :
                                    key.Type == SettingsKeyType.SBoolean ? keyValue.Value<int>() :
                                    key.Type == SettingsKeyType.SInt ? keyValue.Value<int>() :
                                    key.Type == SettingsKeyType.SIntSlider ? keyValue.Value<int>() :
                                    keyValue.Value;

                                // Now, set the value
                                key.KeyInput.SetValue(key, value, i == 1 && extraSaversConfig is not null ? extraSaversConfig : Config.SaverConfig);
                            }
                        }
                    }
                }

                // Save the configuration
                Config.CreateConfig();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to migrate screensaver kernel config! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
            return true;
        }

        private static bool MigrateAliases()
        {
            // Locate the old alias file and parse it
            string oldAliasesPath = ConfigOldPaths.GetOldKernelPath(ConfigOldPathType.Aliases);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old alias path {0}.", vars: [oldAliasesPath]);
            if (!FilesystemTools.FileExists(oldAliasesPath))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Can't find old alias path.");
                return true;
            }

            // Aliases file could be zero bytes, so check it.
            string contents = FilesystemTools.ReadAllTextNoBlock(oldAliasesPath);
            if (contents.Length == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Old alias file has zero bytes.");
                return true;
            }
            var aliases = JsonConvert.DeserializeObject<AliasInfo[]>(contents) ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Failed to get alias information"));
            DebugWriter.WriteDebug(DebugLevel.I, "Got old alias token.");

            // Handle any possible errors here.
            try
            {
                // Iterate through all the aliases and add them conditionally
                foreach (var alias in aliases)
                {
                    string aliasName = alias.Alias;
                    string aliasCommand = alias.Command;
                    string aliasType = alias.Type;
                    if (!AliasManager.DoesAliasExist(aliasName, aliasType) && CommandManager.IsCommandFound(aliasCommand, aliasType))
                        AliasManager.AddAlias(aliasCommand, aliasName, aliasType);
                }

                // Save all the aliases
                AliasManager.SaveAliases();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to migrate aliases! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
            return true;
        }

        private static bool MigrateUsers()
        {
            // Locate the old user file and parse it
            string oldUsersPath = ConfigOldPaths.GetOldKernelPath(ConfigOldPathType.Users);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old user path {0}.", vars: [oldUsersPath]);
            if (!FilesystemTools.FileExists(oldUsersPath))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Can't find old user path.");
                return true;
            }

            // Users file could be zero bytes, so check it.
            string contents = FilesystemTools.ReadAllTextNoBlock(oldUsersPath);
            if (contents.Length == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Old user file has zero bytes.");
                return true;
            }
            var oldUsersToken = JToken.Parse(contents);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old user token.");

            // Handle any possible errors here.
            try
            {
                // Iterate through all the users and add them conditionally
                foreach (var user in oldUsersToken)
                {
                    string? username = (string?)user["username"];
                    string? password = (string?)user["password"];
                    var permissions = (JArray?)user["permissions"];
                    if (username is null)
                        continue;
                    if (password is null)
                        continue;
                    if (permissions is null)
                        continue;
                    if (!UserManagement.UserExists(username))
                        UserManagement.AddUser(username, password);
                    int userIndex = UserManagement.GetUserIndex(username);
                    UserManagement.Users[userIndex].Password = password;
                    var permissionsArray = permissions.Values<string>().ToArray();
                    if (permissionsArray.Contains(nameof(UserFlags.Administrator)))
                        UserManagement.Users[userIndex].Flags |= UserFlags.Administrator;
                    if (permissionsArray.Contains(nameof(UserFlags.Anonymous)))
                        UserManagement.Users[userIndex].Flags |= UserFlags.Anonymous;
                    if (permissionsArray.Contains(nameof(UserFlags.Disabled)))
                        UserManagement.Users[userIndex].Flags |= UserFlags.Disabled;
                }

                // Save the changes
                UserManagement.SaveUsers();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to migrate users! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
            return true;
        }

        private static bool MigrateFtpSpeedDial()
        {
            // Locate the old FTP speed dial file and parse it
            string oldFtpSpeedDialPath = ConfigOldPaths.GetOldKernelPath(ConfigOldPathType.FTPSpeedDial);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old FTP speed dial path {0}.", vars: [oldFtpSpeedDialPath]);
            if (!FilesystemTools.FileExists(oldFtpSpeedDialPath))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Can't find old FTP speed dial path.");
                return true;
            }

            // FTP speed dials file could be zero bytes, so check it.
            string contents = FilesystemTools.ReadAllTextNoBlock(oldFtpSpeedDialPath);
            if (contents.Length == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Old FTP speed dial file has zero bytes.");
                return true;
            }
            var oldFtpSpeedDialToken = JToken.Parse(contents);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old FTP speed dial token.");

            // Handle any possible errors here.
            try
            {
                // Iterate through all the FTP speed dials and add them conditionally
                foreach (var ftpSpeedDial in oldFtpSpeedDialToken)
                {
                    var info = ftpSpeedDial.First;
                    if (info is null)
                        continue;
                    string address = (string?)info["Address"] ?? "";
                    int port = (int?)info["Port"] ?? 21;
                    string user = (string?)info["User"] ?? "";
                    int type = (int?)info["Type"] ?? 0;
                    int encryptionMode = (int?)info["FTP Encryption Mode"] ?? 0;
                    SpeedDialTools.AddEntryToSpeedDial(address, port, NetworkConnectionType.FTP, false, [user, type, encryptionMode]);
                }

                // Save the entries
                SpeedDialTools.SaveAll();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to migrate FTP speed dials! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
            return true;
        }

        private static bool MigrateSftpSpeedDial()
        {
            // Locate the old SFTP speed dial file and parse it
            string oldSftpSpeedDialPath = ConfigOldPaths.GetOldKernelPath(ConfigOldPathType.SFTPSpeedDial);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old SFTP speed dial path {0}.", vars: [oldSftpSpeedDialPath]);
            if (!FilesystemTools.FileExists(oldSftpSpeedDialPath))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Can't find old SFTP speed dial path.");
                return true;
            }

            // SFTP speed dials file could be zero bytes, so check it.
            string contents = FilesystemTools.ReadAllTextNoBlock(oldSftpSpeedDialPath);
            if (contents.Length == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Old SFTP speed dial file has zero bytes.");
                return true;
            }
            var oldSftpSpeedDialToken = JToken.Parse(contents);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old SFTP speed dial token.");

            // Handle any possible errors here.
            try
            {
                // Iterate through all the SFTP speed dials and add them conditionally
                foreach (var sftpSpeedDial in oldSftpSpeedDialToken)
                {
                    var info = sftpSpeedDial.First;
                    if (info is null)
                        continue;
                    string address = (string?)info["Address"] ?? "";
                    int port = (int?)info["Port"] ?? 22;
                    string user = (string?)info["User"] ?? "";
                    int type = (int?)info["Type"] ?? 0;
                    int encryptionMode = (int?)info["SFTP Encryption Mode"] ?? 0;
                    SpeedDialTools.AddEntryToSpeedDial(address, port, NetworkConnectionType.SFTP, false, [user, type, encryptionMode]);
                }

                // Save the entries
                SpeedDialTools.SaveAll();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to migrate SFTP speed dials! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
            return true;
        }

        private static bool MigrateDebugDevices()
        {
            // Locate the old Remote debug device configuration file and parse it
            string oldDebugDevicesPath = ConfigOldPaths.GetOldKernelPath(ConfigOldPathType.DebugDevNames);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old remote debug device configuration path {0}.", vars: [oldDebugDevicesPath]);
            if (!FilesystemTools.FileExists(oldDebugDevicesPath))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Can't find old remote debug device configuration path.");
                return true;
            }

            // Remote debug device configurations file could be zero bytes, so check it.
            string contents = FilesystemTools.ReadAllTextNoBlock(oldDebugDevicesPath);
            if (contents.Length == 0)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Old remote debug device configuration file has zero bytes.");
                return true;
            }
            var oldDebugDevicesToken = JToken.Parse(contents);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old remote debug device configuration token.");

            // Handle any possible errors here.
            try
            {
                // Iterate through all the Remote debug device configurations and add them conditionally
                foreach (var debugDevices in oldDebugDevicesToken)
                {
                    string address = ((JProperty)debugDevices).Name;
                    var info = ((JProperty)debugDevices).Value;
                    string name = (string?)info["Name"] ?? "";
                    bool blocked = (bool?)info["Blocked"] ?? false;
                    var chatHistory = (JArray?)info["ChatHistory"];
                    var device = RemoteDebugTools.AddDevice(address, false);
                    device.name = name;
                    device.blocked = blocked;
                    if (chatHistory is null)
                        continue;
                    device.chatHistory = chatHistory.Values<string>().Cast<string>().ToList();
                }

                // Save the entries
                RemoteDebugTools.SaveAllDevices();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to migrate remote debug device configurations! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
            return true;
        }

        private static bool MigrateMotd()
        {
            // Locate the old MOTD file and parse it
            string oldMotdPath = ConfigOldPaths.GetOldKernelPath(ConfigOldPathType.MOTD);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old MOTD path {0}.", vars: [oldMotdPath]);
            if (!FilesystemTools.FileExists(oldMotdPath))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Can't find MOTD configuration path.");
                return true;
            }

            // Handle any possible errors here.
            try
            {
                string contents = FilesystemTools.ReadAllTextNoBlock(oldMotdPath);
                MotdParse.SetMotd(contents);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to migrate MOTD! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
            return true;
        }

        private static bool MigrateMal()
        {
            // Locate the old MAL file and parse it
            string oldMalPath = ConfigOldPaths.GetOldKernelPath(ConfigOldPathType.MAL);
            DebugWriter.WriteDebug(DebugLevel.I, "Got old MAL path {0}.", vars: [oldMalPath]);
            if (!FilesystemTools.FileExists(oldMalPath))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Can't find MAL configuration path.");
                return true;
            }

            // Handle any possible errors here.
            try
            {
                string contents = FilesystemTools.ReadAllTextNoBlock(oldMalPath);
                MalParse.SetMal(contents);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Failed to migrate MAL! {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
            return true;
        }
    }
}
