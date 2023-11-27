//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using KS.Drivers;
using KS.Drivers.Encryption;
using KS.Files;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KS.Kernel.Events;
using KS.Users.Login;
using KS.Kernel.Configuration;
using KS.Misc.Text.Probers.Regexp;
using KS.Files.Operations;
using KS.Files.Operations.Querying;
using KS.Files.Paths;

namespace KS.Users
{
    /// <summary>
    /// User management module
    /// </summary>
    public static class UserManagement
    {

        /// <summary>
        /// Include anonymous users in list
        /// </summary>
        public static bool IncludeAnonymous =>
            Config.MainConfig.IncludeAnonymous;
        /// <summary>
        /// Include disabled users in list
        /// </summary>
        public static bool IncludeDisabled =>
            Config.MainConfig.IncludeDisabled;
        /// <summary>
        /// Current username
        /// </summary>
        public static UserInfo CurrentUser =>
            CurrentUserInfo;

        internal static readonly UserInfo fallbackRootAccount = new("root", Encryption.GetEncryptedString("", "SHA256"), [], "System Account", "", [], UserFlags.Administrator, []);
        internal static UserInfo CurrentUserInfo = fallbackRootAccount;
        internal static List<UserInfo> Users = [CurrentUserInfo];
        private static readonly List<UserInfo> LockedUsers = [];

        // ---------- User Management ----------
        /// <summary>
        /// Initializes the uninitialized user (usually a new user)
        /// </summary>
        /// <param name="uninitUser">A new user</param>
        /// <param name="unpassword">A password of a user in encrypted form</param>
        /// <param name="ComputationNeeded">Whether or not a password encryption is needed</param>
        /// <param name="ModifyExisting">Changes the password of the existing user</param>
        /// <returns>True if successful; False if successful</returns>
        public static bool InitializeUser(string uninitUser, string unpassword = "", bool ComputationNeeded = true, bool ModifyExisting = false)
        {
            try
            {
                // Check the lock
                if (IsLocked(uninitUser))
                    throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Trying to modify existing account while it's locked"));

                // Compute hash of a password
                var Regexp = DriverHandler.GetDriver<IEncryptionDriver>("SHA256").HashRegex;
                if (ComputationNeeded)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Computing hash...");
                    unpassword = Encryption.GetEncryptedString(unpassword, "SHA256");
                    DebugWriter.WriteDebug(DebugLevel.I, "Hash computed.");
                }
                else if (!Regexp.IsMatch(unpassword))
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Unencrypted password!");
                    throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Trying to add unencrypted password to users list."));
                }

                // Add user locally
                var initedUser = new UserInfo(uninitUser, unpassword, [], "", "", [], UserFlags.None, []);
                if (!UserExists(uninitUser))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Added user {0}!", uninitUser);
                    Users.Add(initedUser);
                }
                else if (UserExists(uninitUser) & ModifyExisting)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Modifying user {0}...", uninitUser);
                    int userIndex = GetUserIndex(uninitUser);
                    Users[userIndex] = initedUser;
                }

                // Add user globally
                SaveUsers();

                // Ready permissions
                DebugWriter.WriteDebug(DebugLevel.I, "Username {0} added. Readying permissions...", uninitUser);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.UserCreation, Translate.DoTranslation("Error trying to add username.") + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), ex, ex.GetType().FullName, ex.Message);
            }
        }

        /// <summary>
        /// Reads the user file and adds them to the list.
        /// </summary>
        public static void InitializeUsers()
        {
            // First, check to see if we have the file
            string UsersPath = PathsManagement.GetKernelPath(KernelPathType.Users);
            if (!Checking.FileExists(UsersPath))
                SaveUsers();

            // Get the content and parse it
            string UsersTokenContent = Reading.ReadContentsText(PathsManagement.GetKernelPath(KernelPathType.Users));
            JArray userInfoArrays = (JArray)JsonConvert.DeserializeObject(UsersTokenContent);

            // Now, get each user from the config file
            List<UserInfo> users = [];
            int rootIdx = 0;
            bool sawRoot = false;
            foreach (var userInfoArray in userInfoArrays)
            {
                // Add the user info to the users list after populating it
                UserInfo userInfo = (UserInfo)JsonConvert.DeserializeObject(userInfoArray.ToString(), typeof(UserInfo));
                users.Add(userInfo);
                if (userInfo.Username == "root")
                    sawRoot = true;
                if (!sawRoot)
                    rootIdx++;
            }

            // Check the root user for administrator status
            if (users.Count > 0)
            {
                // Get root account
                var root = users[rootIdx];
                if (!root.Flags.HasFlag(UserFlags.Administrator))
                {
                    // Either it's an upgrade from the old user format, or a malicious mod removed admin from root.
                    DebugWriter.WriteDebug(DebugLevel.W, "Root account doesn't have admin status. Setting...");
                    root.Flags |= UserFlags.Administrator;
                }
            }

            // Install values
            Users = users;
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="newUser">A new user</param>
        /// <param name="newPassword">A password</param>
        public static void AddUser(string newUser, string newPassword = "")
        {
            // Adds user
            DebugWriter.WriteDebug(DebugLevel.I, "Creating user {0}...", newUser);
            if (ValidateUsername(newUser, false) && !UserExists(newUser))
            {
                try
                {
                    if (string.IsNullOrEmpty(newPassword))
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Initializing user with no password");
                        InitializeUser(newUser);
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Initializing user with password");
                        InitializeUser(newUser, newPassword);
                    }
                    EventsManager.FireEvent(EventType.UserAdded, newUser);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to create user {0}: {1}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.UserCreation, Translate.DoTranslation("usrmgr: Failed to create username {0}: {1}"), ex, newUser, ex.Message);
                }
            }
            else
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Can't add username. Make sure that the username doesn't exist."));
        }

        /// <summary>
        /// Removes a user from users database
        /// </summary>
        /// <param name="user">A user</param>
        /// <remarks>This sub is an accomplice of in-shell command arguments.</remarks>
        public static void RemoveUser(string user)
        {
            // Check the lock
            if (IsLocked(user))
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Trying to modify existing account while it's locked"));
            if (!ValidateUsername(user))
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Can't remove username. Make sure that the username exists."));

            // Try to remove user
            if (user == "root")
            {
                DebugWriter.WriteDebug(DebugLevel.W, "User is root, and is a system account");
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User {0} isn't allowed to be removed."), user);
            }
            else if (user == CurrentUser?.Username)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "User has logged in, so can't delete self.");
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User {0} is already logged in. Log-out and log-in as another admin."), user);
            }
            else
            {
                try
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Removing permissions...");

                    // Remove user
                    DebugWriter.WriteDebug(DebugLevel.I, "Removing username {0}...", user);
                    var userInfo = GetUser(user);
                    Users.Remove(userInfo);

                    // Remove user from Users.json
                    SaveUsers();

                    // Raise event
                    EventsManager.FireEvent(EventType.UserRemoved, user);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Error trying to remove username.") + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), ex, ex.Message);
                }
            }
        }

        /// <summary>
        /// Removes a user from users database
        /// </summary>
        /// <param name="user">A user</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <remarks>This sub is an accomplice of in-shell command arguments.</remarks>
        public static bool TryRemoveUser(string user)
        {
            try
            {
                RemoveUser(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Changes the username
        /// </summary>
        /// <param name="OldName">Old username</param>
        /// <param name="Username">New username</param>
        public static void ChangeUsername(string OldName, string Username)
        {
            if (UserExists(OldName))
            {
                // Check the lock
                if (IsLocked(OldName))
                    throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Trying to modify existing account while it's locked"));
                if (!UserExists(Username))
                {
                    try
                    {
                        // Store user info
                        var oldInfo = GetUser(OldName);
                        var newInfo = new UserInfo(Username, oldInfo.Password, oldInfo.Permissions, oldInfo.FullName, oldInfo.PreferredLanguage, oldInfo.Groups, oldInfo.Flags, oldInfo.CustomSettings);

                        // Rename username in dictionary
                        Users.Remove(oldInfo);
                        Users.Add(newInfo);

                        // Rename username in Users.json
                        SaveUsers();

                        // Raise event
                        EventsManager.FireEvent(EventType.UsernameChanged, OldName, Username);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebugStackTrace(ex);
                        throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Failed to rename user. {0}"), ex, ex.Message);
                    }
                }
                else
                {
                    throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("The new name you entered is already found."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User {0} not found."), OldName);
            }
        }

        /// <summary>
        /// Changes the username
        /// </summary>
        /// <param name="OldName">Old username</param>
        /// <param name="Username">New username</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryChangeUsername(string OldName, string Username)
        {
            try
            {
                ChangeUsername(OldName, Username);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="Target">Target username</param>
        /// <param name="CurrentPass">Current user password</param>
        /// <param name="NewPass">New user password</param>
        public static void ChangePassword(string Target, string CurrentPass, string NewPass)
        {
            bool currentUserAdmin = GetUser(CurrentUser.Username).Flags.HasFlag(UserFlags.Administrator);
            bool targetUserAdmin = GetUser(Target).Flags.HasFlag(UserFlags.Administrator);

            if (!UserExists(Target))
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User not found"));

            // Check the lock
            if (IsLocked(Target))
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Trying to modify existing account while it's locked"));

            CurrentPass = Encryption.GetEncryptedString(CurrentPass, "SHA256");
            if (CurrentPass == GetUser(Target).Password)
            {
                if (currentUserAdmin & UserExists(Target))
                {
                    // Change password locally
                    NewPass = Encryption.GetEncryptedString(NewPass, "SHA256");
                    int userIndex = GetUserIndex(Target);
                    Users[userIndex].Password = NewPass;

                    // Change password globally
                    SaveUsers();

                    // Raise event
                    EventsManager.FireEvent(EventType.UserPasswordChanged, Target);
                }
                else if (targetUserAdmin & !currentUserAdmin)
                {
                    throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("You are not authorized to change password of {0} because the target was an admin."), Target);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Wrong user password."));
            }
        }

        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="Target">Target username</param>
        /// <param name="CurrentPass">Current user password</param>
        /// <param name="NewPass">New user password</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryChangePassword(string Target, string CurrentPass, string NewPass)
        {
            try
            {
                ChangePassword(Target, CurrentPass, NewPass);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Lists all users and includes anonymous and disabled users if enabled.
        /// </summary>
        public static List<string> ListAllUsers() => ListAllUsers(IncludeAnonymous, IncludeDisabled);

        /// <summary>
        /// Lists all users and includes anonymous and disabled users if enabled.
        /// </summary>
        /// <param name="IncludeAnonymous">Include anonymous users</param>
        /// <param name="IncludeDisabled">Include disabled users</param>
        public static List<string> ListAllUsers(bool IncludeAnonymous = false, bool IncludeDisabled = false)
        {
            var UsersList = new List<string>(Users.Select((userInfo) => userInfo.Username));
            if (!IncludeAnonymous)
                UsersList.RemoveAll(x => GetUser(x).Flags.HasFlag(UserFlags.Anonymous));
            if (!IncludeDisabled)
                UsersList.RemoveAll(x => GetUser(x).Flags.HasFlag(UserFlags.Disabled));

            return UsersList;
        }

        /// <summary>
        /// Selects a user from the <see cref="ListAllUsers(bool, bool)"/> list
        /// </summary>
        /// <param name="UserNumber">The user number. This is NOT an index!</param>
        /// <returns>The username which is selected</returns>
        public static string SelectUser(int UserNumber) =>
            SelectUser(UserNumber, IncludeAnonymous, IncludeDisabled);

        /// <summary>
        /// Selects a user from the <see cref="ListAllUsers(bool, bool)"/> list
        /// </summary>
        /// <param name="UserNumber">The user number. This is NOT an index!</param>
        /// <param name="IncludeAnonymous">Include anonymous users</param>
        /// <param name="IncludeDisabled">Include disabled users</param>
        /// <returns>The username which is selected</returns>
        public static string SelectUser(int UserNumber, bool IncludeAnonymous = false, bool IncludeDisabled = false)
        {
            var UsersList = ListAllUsers(IncludeAnonymous, IncludeDisabled);
            string SelectedUsername = UsersList[UserNumber - 1];
            return GetUser(SelectedUsername).Username;
        }

        /// <summary>
        /// Checks to see if the user exists
        /// </summary>
        /// <param name="User">The target user</param>
        public static bool UserExists(string User) =>
            Users.Any((userinfo) => userinfo.Username == User);

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userName">The user</param>
        /// <returns>User information</returns>
        public static UserInfo GetUser(string userName)
        {
            // Check to see if we have the target user
            if (!UserExists(userName))
                throw new KernelException(KernelExceptionType.NoSuchUser);

            return Users.FirstOrDefault(x => x.Username == userName);
        }

        /// <summary>
        /// Gets a user index
        /// </summary>
        /// <param name="userName">The user</param>
        /// <returns>User index</returns>
        public static int GetUserIndex(string userName)
        {
            // Check to see if we have the target user
            if (!UserExists(userName))
                throw new KernelException(KernelExceptionType.NoSuchUser);

            return Users.FindIndex(x => x.Username == userName);
        }

        /// <summary>
        /// Gets the unique user identifier for the current user
        /// </summary>
        public static string GetUserDollarSign() =>
            GetUserDollarSign(CurrentUser.Username);

        /// <summary>
        /// Gets the unique user identifier
        /// </summary>
        /// <param name="User">The target user</param>
        public static string GetUserDollarSign(string User)
        {
            if (UserExists(User))
                if (GetUser(User).Flags.HasFlag(UserFlags.Administrator))
                    return "#";
                else
                    return "$";
            else
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User not found"));
        }

        /// <summary>
        /// Validates the username
        /// </summary>
        /// <param name="User">The user name to be validated</param>
        /// <param name="CheckExistence">Checks for existence</param>
        /// <returns>True if the user doesn't contain spaces and unknown characters and is found and not disabled</returns>
        public static bool ValidateUsername(string User, bool CheckExistence = true)
        {
            if (User.Contains(' '))
            {
                // Usernames shouldn't contain spaces
                DebugWriter.WriteDebug(DebugLevel.W, "Spaces found in username.");
                EventsManager.FireEvent(EventType.LoginError, User, LoginErrorReasons.Spaces);
                return false;
            }
            else if (!RegexpTools.IsMatch(User, @"^[\w.-]+$"))
            {
                // Usernames shouldn't contain unknown characters
                DebugWriter.WriteDebug(DebugLevel.W, "Unknown characters found in username.");
                EventsManager.FireEvent(EventType.LoginError, User, LoginErrorReasons.SpecialCharacters);
                return false;
            }
            else if (CheckExistence && !UserExists(User))
            {
                // User should exist
                DebugWriter.WriteDebug(DebugLevel.E, "Username not found.");
                EventsManager.FireEvent(EventType.LoginError, User, LoginErrorReasons.NotFound);
                return false;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Username correct. Finding if the user is disabled...");
                if (!UserExists(User) || !GetUser(User).Flags.HasFlag(UserFlags.Disabled))
                {
                    // User is not disabled
                    DebugWriter.WriteDebug(DebugLevel.I, "User validation complete");
                    return true;
                }
                else
                {
                    // User is disabled
                    DebugWriter.WriteDebug(DebugLevel.W, "User can't log in. (User is in disabled list)");
                    EventsManager.FireEvent(EventType.LoginError, User, LoginErrorReasons.Disabled);
                    return false;
                }
            }
        }

        /// <summary>
        /// Validates the password
        /// </summary>
        /// <param name="User">Username of the target</param>
        /// <param name="Password">Password of the target</param>
        /// <returns>True if correct</returns>
        public static bool ValidatePassword(string User, string Password)
        {
            // If the user is not even valid, assume that the password is wrong
            if (!ValidateUsername(User))
                return false;

            // Encrypt the password with SHA256
            Password = Encryption.GetEncryptedString(Password, "SHA256");
            DebugWriter.WriteDebug(DebugLevel.I, "Hash computed.");

            // Now, check to see if the password matches
            if (GetUser(User).Password == Password)
            {
                // Password matches
                DebugWriter.WriteDebug(DebugLevel.I, "Password written correctly.");
                return true;
            }
            else
            {
                // Password doesn't match
                DebugWriter.WriteDebug(DebugLevel.I, "Password written wrong...");
                EventsManager.FireEvent(EventType.LoginError, User, LoginErrorReasons.WrongPassword);
                return false;
            }
        }

        /// <summary>
        /// Locks a user
        /// </summary>
        /// <param name="User">A username to lock</param>
        public static void LockUser(string User)
        {
            if (!IsLocked(User))
                LockedUsers.Add(GetUser(User));
        }

        /// <summary>
        /// Unlocks a user
        /// </summary>
        /// <param name="User">A username to unlock</param>
        public static void UnlockUser(string User)
        {
            if (IsLocked(User))
                LockedUsers.Remove(GetUser(User));
        }

        /// <summary>
        /// Checks to see if the user is locked
        /// </summary>
        /// <param name="User">A username to check</param>
        /// <returns>True if locked; False otherwise</returns>
        public static bool IsLocked(string User) =>
            LockedUsers.Any((ui) => ui.Username == User);

        internal static void SaveUsers()
        {
            // Make a JSON file to save all user information files
            string userInfosSerialized = JsonConvert.SerializeObject(Users.ToArray(), Formatting.Indented);
            Writing.WriteContentsText(PathsManagement.UsersPath, userInfosSerialized);
        }
    }
}
