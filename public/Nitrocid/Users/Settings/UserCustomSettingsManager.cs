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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Users.Settings
{
    /// <summary>
    /// Custom settings manager for users
    /// </summary>
    public static class UserCustomSettingsManager
    {
        /// <summary>
        /// Adds a settings entry to the user configuration file
        /// </summary>
        /// <param name="user">Target user name</param>
        /// <param name="key">Target key</param>
        /// <param name="value">Value to assign the key to</param>
        public static void AddSettingsEntryToUser(string user, string key, object[] value)
        {
            // Check to see if we have the target user
            if (!UserManagement.UserExists(user))
                throw new KernelException(KernelExceptionType.NoSuchUser);

            // Check to see if we have the key
            if (DoesSettingsEntryExist(user, key))
                throw new KernelException(KernelExceptionType.CustomSettings, LanguageTools.GetLocalized("NKS_USERS_SETTINGS_EXCEPTION_KEYEXISTS"), key);

            // Now, add the key with its value to the user info and then save the file
            int userIndex = UserManagement.GetUserIndex(user);
            if (UserManagement.Users[userIndex].CustomSettings is null)
                UserManagement.Users[userIndex].CustomSettings = [];
            UserManagement.Users[userIndex].CustomSettings.Add(key, value);

            // Just save the file!
            UserManagement.SaveUsers();
        }

        /// <summary>
        /// Modifies (sets) a settings entry in the user configuration file
        /// </summary>
        /// <param name="user">Target user name</param>
        /// <param name="key">Target key</param>
        /// <param name="value">Value to assign the key to</param>
        public static void ModifySettingsEntryInUser(string user, string key, object[] value)
        {
            // Check to see if we have the target user
            if (!UserManagement.UserExists(user))
                throw new KernelException(KernelExceptionType.NoSuchUser);

            // Check to see if we have the key
            if (!DoesSettingsEntryExist(user, key))
                throw new KernelException(KernelExceptionType.CustomSettings, LanguageTools.GetLocalized("NKS_USERS_SETTINGS_EXCEPTION_KEYNOTEXISTS"), key);

            // Now, modify the key with its value in the user info and then save the file
            int userIndex = UserManagement.GetUserIndex(user);
            UserManagement.Users[userIndex].CustomSettings[key] = value;

            // Just save the file!
            UserManagement.SaveUsers();
        }

        /// <summary>
        /// Removes a settings entry from the user configuration file
        /// </summary>
        /// <param name="user">Target user name</param>
        /// <param name="key">Target key</param>
        public static void RemoveSettingsEntryFromUser(string user, string key)
        {
            // Check to see if we have the target user
            if (!UserManagement.UserExists(user))
                throw new KernelException(KernelExceptionType.NoSuchUser);

            // Check to see if we have the key
            if (!DoesSettingsEntryExist(user, key))
                throw new KernelException(KernelExceptionType.CustomSettings, LanguageTools.GetLocalized("NKS_USERS_SETTINGS_EXCEPTION_KEYNOTEXISTS"), key);

            // Now, remove the key from the user info and then save the file
            int userIndex = UserManagement.GetUserIndex(user);
            UserManagement.Users[userIndex].CustomSettings.Remove(key);

            // Just save the file!
            UserManagement.SaveUsers();
        }

        /// <summary>
        /// Checks to see if a settings entry is found in the user configuration file
        /// </summary>
        /// <param name="user">Target user name</param>
        /// <param name="key">Target key</param>
        public static bool DoesSettingsEntryExist(string user, string key)
        {
            // Check to see if we have the target user
            if (!UserManagement.UserExists(user))
                throw new KernelException(KernelExceptionType.NoSuchUser);

            // Check to see if there is an entry. If null, just return false.
            int userIndex = UserManagement.GetUserIndex(user);
            var customSettings = UserManagement.Users[userIndex].CustomSettings;
            if (customSettings is null)
                return false;
            return customSettings.ContainsKey(key);
        }

        /// <summary>
        /// gets a settings entry value from the user configuration file
        /// </summary>
        /// <param name="user">Target user name</param>
        /// <param name="key">Target key</param>
        public static object[] GetSettingsEntryFromUser(string user, string key)
        {
            // Check to see if we have the target user
            if (!UserManagement.UserExists(user))
                throw new KernelException(KernelExceptionType.NoSuchUser);

            // Check to see if we have the key
            if (!DoesSettingsEntryExist(user, key))
                throw new KernelException(KernelExceptionType.CustomSettings, LanguageTools.GetLocalized("NKS_USERS_SETTINGS_EXCEPTION_KEYNOTEXISTS"), key);

            // Get the entry.
            int userIndex = UserManagement.GetUserIndex(user);
            var customSettings = UserManagement.Users[userIndex].CustomSettings;
            if (customSettings is null)
                return [];
            return customSettings[key];
        }
    }
}
