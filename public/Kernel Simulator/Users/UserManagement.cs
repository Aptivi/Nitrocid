
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
using KS.Users.Groups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KS.Kernel.Events;

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
        public static bool IncludeAnonymous;
        /// <summary>
        /// Include disabled users in list
        /// </summary>
        public static bool IncludeDisabled;
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
            /// List of permissions
            /// </summary>
            Groups
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
        /// <exception cref="InvalidOperationException"></exception>
        public static bool InitializeUser(string uninitUser, string unpassword = "", bool ComputationNeeded = true, bool ModifyExisting = false)
        {
            try
            {
                // Compute hash of a password
                var Regexp = DriverHandler.GetEncryptionDriver("SHA256").HashRegex;
                if (ComputationNeeded)
                {
                    unpassword = Encryption.GetEncryptedString(unpassword, "SHA256");
                    DebugWriter.WriteDebug(DebugLevel.I, "Hash computed.");
                }
                else if (!Regexp.IsMatch(unpassword))
                {
                    throw new InvalidOperationException("Trying to add unencrypted password to users list.");
                }

                // Add user locally
                if (!Login.Login.Users.ContainsKey(uninitUser))
                {
                    Login.Login.Users.Add(uninitUser, unpassword);
                }
                else if (Login.Login.Users.ContainsKey(uninitUser) & ModifyExisting)
                {
                    Login.Login.Users[uninitUser] = unpassword;
                }

                // Add user globally
                if (!(UsersToken.Count == 0))
                {
                    var UserExists = default(bool);
                    var ExistingIndex = default(int);
                    foreach (JObject UserToken in UsersToken)
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
                        var NewUser = new JObject(new JProperty("username", uninitUser), new JProperty("password", unpassword), new JProperty("groups", new JArray()));
                        UsersToken.Add(NewUser);
                    }
                    else if (UserExists & ModifyExisting)
                    {
                        UsersToken[ExistingIndex]["password"] = unpassword;
                    }
                }
                else
                {
                    var NewUser = new JObject(new JProperty("username", uninitUser), new JProperty("password", unpassword), new JProperty("groups", new JArray()));
                    UsersToken.Add(NewUser);
                }
                File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented));

                // Ready permissions
                DebugWriter.WriteDebug(DebugLevel.I, "Username {0} added. Readying permissions...", uninitUser);
                GroupManagement.InitGroupsForNewUser(uninitUser);
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
            foreach (JObject UserToken in UninitUsersToken)
                InitializeUser((string)UserToken["username"], (string)UserToken["password"], false);
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
            foreach (JObject UserToken in UsersToken)
            {
                if (UserToken["username"].ToString() == User)
                {
                    return UserToken.SelectToken(PropertyType.ToString().ToLower());
                }
            }
            return null;
        }

        /// <summary>
        /// Sets user property
        /// </summary>
        /// <param name="User">Target user</param>
        /// <param name="PropertyType">Property type</param>
        /// <param name="Value">Value</param>
        public static void SetUserProperty(string User, UserProperty PropertyType, string Value)
        {
            foreach (JObject UserToken in UsersToken)
            {
                if (UserToken["username"].ToString() == User)
                {
                    switch (PropertyType)
                    {
                        case UserProperty.Username:
                            {
                                UserToken["username"] = Value;
                                break;
                            }
                        case UserProperty.Password:
                            {
                                UserToken["password"] = Value;
                                break;
                            }
                        case UserProperty.Groups:
                            {
                                throw new NotSupportedException("Use AddGroup and RemoveGroup for this.");
                            }

                        default:
                            {
                                throw new ArgumentException("Property type is invalid");
                            }
                    }
                }
            }
            File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented));
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="newUser">A new user</param>
        /// <param name="newPassword">A password</param>
        public static bool AddUser(string newUser, string newPassword = "")
        {
            // Adds user
            DebugWriter.WriteDebug(DebugLevel.I, "Creating user {0}...", newUser);
            if (newUser.Contains(" "))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "There are spaces in username.");
                throw new KernelException(KernelExceptionType.UserCreation, Translate.DoTranslation("Spaces are not allowed."));
            }
            else if (newUser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray()) != -1)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "There are special characters in username.");
                throw new KernelException(KernelExceptionType.UserCreation, Translate.DoTranslation("Special characters are not allowed."));
            }
            else if (string.IsNullOrEmpty(newUser))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Username is blank.");
                throw new KernelException(KernelExceptionType.UserCreation, Translate.DoTranslation("Blank username."));
            }
            else if (!Login.Login.Users.ContainsKey(newUser))
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
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to create user {0}: {1}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.UserCreation, Translate.DoTranslation("usrmgr: Failed to create username {0}: {1}"), ex, newUser, ex.Message);
                }
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "User {0} already found.", newUser);
                throw new KernelException(KernelExceptionType.UserCreation, Translate.DoTranslation("usrmgr: Username {0} is already found"), newUser);
            }
        }

        /// <summary>
        /// Removes a user from users database
        /// </summary>
        /// <param name="user">A user</param>
        /// <remarks>This sub is an accomplice of in-shell command arguments.</remarks>
        public static void RemoveUser(string user)
        {
            if (user.Contains(" "))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "There are spaces in username.");
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Spaces are not allowed."));
            }
            else if (user.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray()) != -1)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "There are special characters in username.");
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Special characters are not allowed."));
            }
            else if (string.IsNullOrEmpty(user))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Username is blank.");
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("Blank username."));
            }
            else if (Login.Login.Users.ContainsKey(user) == false)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Username {0} not found in list", user);
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User {0} not found."), user);
            }
            // Try to remove user
            else if (Login.Login.Users.Keys.ToArray().Contains(user) & user == "root")
            {
                DebugWriter.WriteDebug(DebugLevel.W, "User is root, and is a system account");
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User {0} isn't allowed to be removed."), user);
            }
            else if (Login.Login.Users.Keys.ToArray().Contains(user) & user == (Login.Login.CurrentUser?.Username))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "User has logged in, so can't delete self.");
                throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User {0} is already logged in. Log-out and log-in as another admin."), user);
            }
            else if (Login.Login.Users.Keys.ToArray().Contains(user) & user != "root")
            {
                try
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Removing permissions...");
                    GroupManagement.UserGroups.Remove(user);

                    // Remove user
                    DebugWriter.WriteDebug(DebugLevel.I, "Removing username {0}...", user);
                    Login.Login.Users.Remove(user);

                    // Remove user from Users.json
                    foreach (JObject UserToken in UsersToken)
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
            if (Login.Login.Users.ContainsKey(OldName))
            {
                if (!Login.Login.Users.ContainsKey(Username))
                {
                    try
                    {
                        // Store user password
                        string Temporary = Login.Login.Users[OldName];

                        // Rename username in dictionary
                        Login.Login.Users.Remove(OldName);
                        Login.Login.Users.Add(Username, Temporary);
                        GroupManagement.GroupEditForNewUser(OldName, Username);

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
            if (Checking.FileExists(Paths.GetKernelPath(KernelPathType.Users)))
            {
                if (GetUserProperty("root", UserProperty.Password) is not null)
                {
                    InitializeUser("root", (string)GetUserProperty("root", UserProperty.Password), false, true);
                }
                else
                {
                    InitializeUser("root", "", true, true);
                }
            }
            else
            {
                InitializeUser("root", "", true, true);
            }
            GroupManagement.AddGroup(GroupManagement.GroupType.Administrator, "root");
        }

        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="Target">Target username</param>
        /// <param name="CurrentPass">Current user password</param>
        /// <param name="NewPass">New user password</param>
        public static void ChangePassword(string Target, string CurrentPass, string NewPass)
        {
            CurrentPass = Encryption.GetEncryptedString(CurrentPass, "SHA256");
            if (CurrentPass == Login.Login.Users[Target])
            {
                if (GroupManagement.HasGroup(Login.Login.CurrentUser.Username, GroupManagement.GroupType.Administrator) & Login.Login.Users.ContainsKey(Target))
                {
                    // Change password locally
                    NewPass = Encryption.GetEncryptedString(NewPass, "SHA256");
                    Login.Login.Users[Target] = NewPass;

                    // Change password globally
                    SetUserProperty(Target, UserProperty.Password, NewPass);

                    // Raise event
                    EventsManager.FireEvent(EventType.UserPasswordChanged, Target);
                }
                else if (GroupManagement.HasGroup(Login.Login.CurrentUser.Username, GroupManagement.GroupType.Administrator) & !Login.Login.Users.ContainsKey(Target))
                {
                    throw new KernelException(KernelExceptionType.UserManagement, Translate.DoTranslation("User not found"));
                }
                else if (GroupManagement.HasGroup(Target, GroupManagement.GroupType.Administrator) & !GroupManagement.HasGroup(Login.Login.CurrentUser.Username, GroupManagement.GroupType.Administrator))
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
            {
                UsersList.RemoveAll(new Predicate<string>(x => GroupManagement.HasGroup(x, GroupManagement.GroupType.Anonymous) == true));
            }
            if (!IncludeDisabled)
            {
                UsersList.RemoveAll(new Predicate<string>(x => GroupManagement.HasGroup(x, GroupManagement.GroupType.Disabled) == true));
            }
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
                if (GroupManagement.HasGroup(User, GroupManagement.GroupType.Administrator))
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

    }
}
