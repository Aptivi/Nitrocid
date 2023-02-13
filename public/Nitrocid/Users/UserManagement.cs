
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KS.Drivers;
using KS.Drivers.Encryption;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KS.Kernel.Events;
using KS.Users.Login;
using KS.Users.Permissions;
using KS.Misc.Probers.Regexp;

namespace KS.Users
{
    /// <summary>
    /// User management module
    /// </summary>
    public static class UserManagement
    {

        // Variables
        /// <summary>
        /// Include anonymous users in list
        /// </summary>
        public static bool IncludeAnonymous { set; get; }
        /// <summary>
        /// Include disabled users in list
        /// </summary>
        public static bool IncludeDisabled { set; get; }
        /// <summary>
        /// The users token
        /// </summary>
        internal static JArray UsersToken;

        /// <summary>
        /// A user property
        /// </summary>
        public enum UserProperty
        {
            /// <summary>
            /// Username
            /// </summary>
            Username,
            /// <summary>
            /// Password
            /// </summary>
            Password,
            /// <summary>
            /// The user is an administrative account
            /// </summary>
            Admin,
            /// <summary>
            /// The user is anonymous
            /// </summary>
            Anonymous,
            /// <summary>
            /// The user is disabled
            /// </summary>
            Disabled,
            /// <summary>
            /// List of permissions
            /// </summary>
            Permissions,
            /// <summary>
            /// Full name
            /// </summary>
            FullName,
            /// <summary>
            /// Preferred language
            /// </summary>
            PreferredLanguage,
        }

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
                // Compute hash of a password
                var Regexp = DriverHandler.GetDriver<IEncryptionDriver>("SHA256").HashRegex;
                if (ComputationNeeded)
                {
                    unpassword = Encryption.GetEncryptedString(unpassword, "SHA256");
                    DebugWriter.WriteDebug(DebugLevel.I, "Hash computed.");
                }
                else if (!Regexp.IsMatch(unpassword))
                {
                    throw new KernelException(KernelExceptionType.UserManagement, "Trying to add unencrypted password to users list.");
                }

                // Add user locally
                var initedUser = new UserInfo(uninitUser, unpassword, Array.Empty<string>(), "", "");
                if (!UserExists(uninitUser))
                {
                    Login.Login.Users.Add(uninitUser, initedUser);
                }
                else if (UserExists(uninitUser) & ModifyExisting)
                {
                    Login.Login.Users[uninitUser] = initedUser;
                }

                // Add user globally
                if (!(UsersToken.Count == 0))
                {
                    var UserExists = false;
                    var ExistingIndex = 0;
                    foreach (JToken UserToken in UsersToken)
                    {
                        if (UserToken["username"].ToString() == uninitUser)
                        {
                            UserExists = true;
                            break;
                        }
                        ExistingIndex += 1;
                    }
                    if (!UserExists)
                    {
                        var NewUser = new JObject(
                            new JProperty("username", uninitUser),
                            new JProperty("password", unpassword),
                            new JProperty("admin", false),
                            new JProperty("anonymous", false),
                            new JProperty("disabled", false),
                            new JProperty("permissions", Array.Empty<string>()),
                            new JProperty("fullname", ""),
                            new JProperty("preferredlanguage", "")
                        );
                        UsersToken.Add(NewUser);
                    }
                    else if (UserExists & ModifyExisting)
                    {
                        UsersToken[ExistingIndex]["password"] = unpassword;
                    }
                }
                else
                {
                    var NewUser = new JObject(
                        new JProperty("username", uninitUser),
                        new JProperty("password", unpassword),
                        new JProperty("admin", false),
                        new JProperty("anonymous", false),
                        new JProperty("disabled", false),
                        new JProperty("permissions", Array.Empty<string>()),
                        new JProperty("fullname", ""),
                        new JProperty("preferredlanguage", "")
                    );
                    UsersToken.Add(NewUser);
                }
                File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented));

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
            // Opens file stream
            string UsersTokenContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Users));
            var UninitUsersToken = JArray.Parse(!string.IsNullOrEmpty(UsersTokenContent) ? UsersTokenContent : "[]");
            foreach (var UserToken in UninitUsersToken)
            {
                string user = (string)UserToken["username"];
                var perms = UserToken["permissions"]?.ToArray() ?? Array.Empty<JToken>();
                InitializeUser(user, (string)UserToken["password"], false);
                foreach (var perm in perms)
                    if (Enum.TryParse(typeof(PermissionTypes), (string)perm, out object permEnum))
                        PermissionsTools.GrantPermission(user, (PermissionTypes)permEnum);
                string fullname = (string)UserToken["fullname"];
                string preferredlanguage = (string)UserToken["preferredlanguage"];
                Login.Login.Users[user].FullName = fullname;
                Login.Login.Users[user].PreferredLanguage = preferredlanguage;
            }
        }

        /// <summary>
        /// Loads user token
        /// </summary>
        public static void LoadUserToken()
        {
            if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.Users)))
                File.Create(Paths.GetKernelPath(KernelPathType.Users)).Close();
            string UsersTokenContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Users));
            UsersToken = JArray.Parse(!string.IsNullOrEmpty(UsersTokenContent) ? UsersTokenContent : "[]");
        }

        /// <summary>
        /// Gets user property
        /// </summary>
        /// <param name="User">Target user</param>
        /// <param name="PropertyType">Property type</param>
        /// <returns>JSON token of user property</returns>
        public static JToken GetUserProperty(string User, UserProperty PropertyType)
        {
            foreach (var UserToken in UsersToken)
                if (UserToken["username"].ToString() == User)
                    return UserToken.SelectToken(PropertyType.ToString().ToLower());
            return null;
        }

        /// <summary>
        /// Sets user property
        /// </summary>
        /// <param name="User">Target user</param>
        /// <param name="PropertyType">Property type</param>
        /// <param name="Value">Value</param>
        public static void SetUserProperty(string User, UserProperty PropertyType, JToken Value)
        {
            foreach (var UserToken in UsersToken)
            {
                if (UserToken["username"].ToString() == User)
                {
                    string propertyTypeNameJson = PropertyType.ToString().ToLower();
                    UserToken[propertyTypeNameJson] = Value;
                }
            }
            File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented));
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
            if (ValidateUsername(user))
            {
                // Try to remove user
                if (user == "root")
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "User is root, and is a system account");
                    throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User {0} isn't allowed to be removed."), user);
                }
                else if (user == (Login.Login.CurrentUser?.Username))
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
                        Login.Login.Users.Remove(user);

                        // Remove user from Users.json
                        foreach (var UserToken in UsersToken)
                        {
                            if (UserToken["username"].ToString() == user)
                            {
                                UserToken.Remove();
                                break;
                            }
                        }
                        File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented));

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
            else
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Can't remove username. Make sure that the username exists."));
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
                if (!UserExists(Username))
                {
                    try
                    {
                        // Store user info
                        var oldInfo = Login.Login.Users[OldName];
                        var newInfo = new UserInfo(Username, oldInfo.Password, oldInfo.Permissions, oldInfo.FullName, oldInfo.PreferredLanguage);

                        // Rename username in dictionary
                        Login.Login.Users.Remove(OldName);
                        Login.Login.Users.Add(Username, newInfo);

                        // Rename username in Users.json
                        SetUserProperty(OldName, UserProperty.Username, Username);

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
        /// Initializes root account
        /// </summary>
        public static void InitializeSystemAccount()
        {
            string systemAccountName = "root";
            if (Checking.FileExists(Paths.GetKernelPath(KernelPathType.Users)))
            {
                if (GetUserProperty(systemAccountName, UserProperty.Password) is not null)
                {
                    InitializeUser(systemAccountName, (string)GetUserProperty(systemAccountName, UserProperty.Password), false, true);
                }
                else
                {
                    InitializeUser(systemAccountName, "", true, true);
                }
            }
            else
            {
                InitializeUser(systemAccountName, "", true, true);
            }
            SetUserProperty(systemAccountName, UserProperty.Admin, true);
        }

        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="Target">Target username</param>
        /// <param name="CurrentPass">Current user password</param>
        /// <param name="NewPass">New user password</param>
        public static void ChangePassword(string Target, string CurrentPass, string NewPass)
        {
            bool currentUserAdmin = (bool)GetUserProperty(Login.Login.CurrentUser.Username, UserProperty.Admin);
            bool targetUserAdmin = (bool)GetUserProperty(Target, UserProperty.Admin);
            CurrentPass = Encryption.GetEncryptedString(CurrentPass, "SHA256");
            if (CurrentPass == Login.Login.Users[Target].Password)
            {
                if (currentUserAdmin & UserExists(Target))
                {
                    // Change password locally
                    NewPass = Encryption.GetEncryptedString(NewPass, "SHA256");
                    Login.Login.Users[Target].Password = NewPass;

                    // Change password globally
                    SetUserProperty(Target, UserProperty.Password, NewPass);

                    // Raise event
                    EventsManager.FireEvent(EventType.UserPasswordChanged, Target);
                }
                else if (currentUserAdmin & !UserExists(Target))
                {
                    throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User not found"));
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
            var UsersList = new List<string>(Login.Login.Users.Keys);
            if (!IncludeAnonymous)
                UsersList.RemoveAll(new Predicate<string>(x => (bool)GetUserProperty(x, UserProperty.Anonymous) == true));
            if (!IncludeDisabled)
                UsersList.RemoveAll(new Predicate<string>(x => (bool)GetUserProperty(x, UserProperty.Disabled) == true));

            return UsersList;
        }

        /// <summary>
        /// Selects a user from the <see cref="ListAllUsers(bool, bool)"/> list
        /// </summary>
        /// <param name="UserNumber">The user number. This is NOT an index!</param>
        /// <returns>The username which is selected</returns>
        public static string SelectUser(int UserNumber) => SelectUser(UserNumber, IncludeAnonymous, IncludeDisabled);

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
            return Login.Login.Users.Keys.First(x => x == SelectedUsername);
        }

        /// <summary>
        /// Checks to see if the user exists
        /// </summary>
        /// <param name="User">The target user</param>
        public static bool UserExists(string User) => Login.Login.Users.ContainsKey(User);

        /// <summary>
        /// Gets the unique user identifier for the current user
        /// </summary>
        public static string GetUserDollarSign() => GetUserDollarSign(Login.Login.CurrentUser.Username);

        /// <summary>
        /// Gets the unique user identifier
        /// </summary>
        /// <param name="User">The target user</param>
        public static string GetUserDollarSign(string User)
        {
            if (UserExists(User))
            {
                if ((bool)GetUserProperty(User, UserProperty.Admin))
                {
                    return "#";
                }
                else
                {
                    return "$";
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User not found"));
            }
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
                if (!UserExists(User) || !(bool)GetUserProperty(User, UserProperty.Disabled))
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
            if (Login.Login.Users.TryGetValue(User, out UserInfo UserPassword) && UserPassword.Password == Password)
            {
                // Password matches
                DebugWriter.WriteDebug(DebugLevel.I, "Password written correctly.");
                return true;
            }
            else
            {
                // Password doesn't match
                DebugWriter.WriteDebug(DebugLevel.I, "Passowrd written wrong...");
                EventsManager.FireEvent(EventType.LoginError, User, LoginErrorReasons.WrongPassword);
                return false;
            }
        }

    }
}
