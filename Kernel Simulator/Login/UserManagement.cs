using System;
using System.Collections.Generic;

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

using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Encryption;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Login
{
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
            Permissions
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
        /// <exception cref="Exceptions.UserCreationException"></exception>
        public static bool InitializeUser(string uninitUser, string unpassword = "", bool ComputationNeeded = true, bool ModifyExisting = false)
        {
            try
            {
                // Compute hash of a password
                var Regexp = new Regex("^([a-fA-F0-9]{64})$");
                if (ComputationNeeded)
                {
                    unpassword = Encryption.GetEncryptedString(unpassword, Encryption.Algorithms.SHA256);
                    DebugWriter.Wdbg(DebugLevel.I, "Hash computed.");
                }
                else if (!Regexp.IsMatch(unpassword))
                {
                    throw new InvalidOperationException("Trying to add unencrypted password to users list.");
                }

                // Add user locally
                if (!Login.Users.ContainsKey(uninitUser))
                {
                    Login.Users.Add(uninitUser, unpassword);
                }
                else if (Login.Users.ContainsKey(uninitUser) & ModifyExisting)
                {
                    Login.Users[uninitUser] = unpassword;
                }

                // Add user globally
                if (!(UsersToken.Count == 0))
                {
                    var UserExists = default(bool);
                    var ExistingIndex = default(int);
                    foreach (JObject UserToken in UsersToken)
                    {
                        if ((UserToken["username"].ToString() ?? "") == (uninitUser ?? ""))
                        {
                            UserExists = true;
                            break;
                        }
                        ExistingIndex += 1;
                    }
                    if (!UserExists)
                    {
                        var NewUser = new JObject(new JProperty("username", uninitUser), new JProperty("password", unpassword), new JProperty("permissions", new JArray()));
                        UsersToken.Add(NewUser);
                    }
                    else if (UserExists & ModifyExisting)
                    {
                        UsersToken[ExistingIndex]["password"] = unpassword;
                    }
                }
                else
                {
                    var NewUser = new JObject(new JProperty("username", uninitUser), new JProperty("password", unpassword), new JProperty("permissions", new JArray()));
                    UsersToken.Add(NewUser);
                }
                File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented));

                // Ready permissions
                DebugWriter.Wdbg(DebugLevel.I, "Username {0} added. Readying permissions...", uninitUser);
                PermissionManagement.InitPermissionsForNewUser(uninitUser);
                return true;
            }
            catch (Exception ex)
            {
                throw new Kernel.Exceptions.UserCreationException(Translate.DoTranslation("Error trying to add username.") + Kernel.Kernel.NewLine + Translate.DoTranslation("Error {0}: {1}"), ex, ex.GetType().FullName, ex.Message);
                DebugWriter.WStkTrc(ex);
            }
            return false;
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
                if ((UserToken["username"].ToString() ?? "") == (User ?? ""))
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
                if ((UserToken["username"].ToString() ?? "") == (User ?? ""))
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
                        case UserProperty.Permissions:
                            {
                                throw new NotSupportedException("Use AddPermission and RemovePermission for this.");
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
        /// <exception cref="Exceptions.UserCreationException"></exception>
        public static bool AddUser(string newUser, string newPassword = "")
        {
            // Adds user
            DebugWriter.Wdbg(DebugLevel.I, "Creating user {0}...", newUser);
            if (newUser.Contains(" "))
            {
                DebugWriter.Wdbg(DebugLevel.W, "There are spaces in username.");
                throw new Kernel.Exceptions.UserCreationException(Translate.DoTranslation("Spaces are not allowed."));
            }
            else if (newUser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray()) != -1)
            {
                DebugWriter.Wdbg(DebugLevel.W, "There are special characters in username.");
                throw new Kernel.Exceptions.UserCreationException(Translate.DoTranslation("Special characters are not allowed."));
            }
            else if (string.IsNullOrEmpty(newUser))
            {
                DebugWriter.Wdbg(DebugLevel.W, "Username is blank.");
                throw new Kernel.Exceptions.UserCreationException(Translate.DoTranslation("Blank username."));
            }
            else if (!Login.Users.ContainsKey(newUser))
            {
                try
                {
                    if (string.IsNullOrEmpty(newPassword))
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Initializing user with no password");
                        InitializeUser(newUser);
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Initializing user with password");
                        InitializeUser(newUser, newPassword);
                    }
                    Kernel.Kernel.KernelEventManager.RaiseUserAdded(newUser);
                    return true;
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to create user {0}: {1}", ex.Message);
                    DebugWriter.WStkTrc(ex);
                    throw new Kernel.Exceptions.UserCreationException(Translate.DoTranslation("usrmgr: Failed to create username {0}: {1}"), ex, newUser, ex.Message);
                }
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.W, "User {0} already found.", newUser);
                throw new Kernel.Exceptions.UserCreationException(Translate.DoTranslation("usrmgr: Username {0} is already found"), newUser);
            }
            return false;
        }

        /// <summary>
        /// Removes a user from users database
        /// </summary>
        /// <param name="user">A user</param>
        /// <exception cref="Exceptions.UserManagementException"></exception>
        /// <remarks>This sub is an accomplice of in-shell command arguments.</remarks>
        public static void RemoveUser(string user)
        {
            if (user.Contains(" "))
            {
                DebugWriter.Wdbg(DebugLevel.W, "There are spaces in username.");
                throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("Spaces are not allowed."));
            }
            else if (user.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray()) != -1)
            {
                DebugWriter.Wdbg(DebugLevel.W, "There are special characters in username.");
                throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("Special characters are not allowed."));
            }
            else if (string.IsNullOrEmpty(user))
            {
                DebugWriter.Wdbg(DebugLevel.W, "Username is blank.");
                throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("Blank username."));
            }
            else if (Login.Users.ContainsKey(user) == false)
            {
                DebugWriter.Wdbg(DebugLevel.W, "Username {0} not found in list", user);
                throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("User {0} not found."), user);
            }
            // Try to remove user
            else if (Login.Users.Keys.ToArray().Contains(user) & user == "root")
            {
                DebugWriter.Wdbg(DebugLevel.W, "User is root, and is a system account");
                throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("User {0} isn't allowed to be removed."), user);
            }
            else if (Login.Users.Keys.ToArray().Contains(user) & (user ?? "") == (Login.CurrentUser?.Username ?? ""))
            {
                DebugWriter.Wdbg(DebugLevel.W, "User has logged in, so can't delete self.");
                throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("User {0} is already logged in. Log-out and log-in as another admin."), user);
            }
            else if (Login.Users.Keys.ToArray().Contains(user) & user != "root")
            {
                try
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Removing permissions...");
                    PermissionManagement.UserPermissions.Remove(user);

                    // Remove user
                    DebugWriter.Wdbg(DebugLevel.I, "Removing username {0}...", user);
                    Login.Users.Remove(user);

                    // Remove user from Users.json
                    foreach (JObject UserToken in UsersToken)
                    {
                        if ((UserToken["username"].ToString() ?? "") == (user ?? ""))
                        {
                            UserToken.Remove();
                            break;
                        }
                    }
                    File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented));

                    // Raise event
                    Kernel.Kernel.KernelEventManager.RaiseUserRemoved(user);
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("Error trying to remove username.") + Kernel.Kernel.NewLine + Translate.DoTranslation("Error {0}: {1}"), ex, ex.Message);
                }
            }
        }

        /// <summary>
        /// Removes a user from users database
        /// </summary>
        /// <param name="user">A user</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Exceptions.UserManagementException"></exception>
        /// <remarks>This sub is an accomplice of in-shell command arguments.</remarks>
        public static bool TryRemoveUser(string user)
        {
            try
            {
                RemoveUser(user);
                return true;
            }
            catch (Exception ex)
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
            if (Login.Users.ContainsKey(OldName))
            {
                if (!Login.Users.ContainsKey(Username))
                {
                    try
                    {
                        // Store user password
                        string Temporary = Login.Users[OldName];

                        // Rename username in dictionary
                        Login.Users.Remove(OldName);
                        Login.Users.Add(Username, Temporary);
                        PermissionManagement.PermissionEditForNewUser(OldName, Username);

                        // Rename username in Users.json
                        SetUserProperty(OldName, UserProperty.Username, Username);

                        // Raise event
                        Kernel.Kernel.KernelEventManager.RaiseUsernameChanged(OldName, Username);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WStkTrc(ex);
                        throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("Failed to rename user. {0}"), ex, ex.Message);
                    }
                }
                else
                {
                    throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("The new name you entered is already found."));
                }
            }
            else
            {
                throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("User {0} not found."), OldName);
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
            catch (Exception ex)
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
            PermissionManagement.AddPermission(PermissionManagement.PermissionType.Administrator, "root");
        }

        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="Target">Target username</param>
        /// <param name="CurrentPass">Current user password</param>
        /// <param name="NewPass">New user password</param>
        /// <exception cref="Exceptions.UserManagementException"></exception>
        public static void ChangePassword(string Target, string CurrentPass, string NewPass)
        {
            CurrentPass = Encryption.GetEncryptedString(CurrentPass, Encryption.Algorithms.SHA256);
            if ((CurrentPass ?? "") == (Login.Users[Target] ?? ""))
            {
                if (PermissionManagement.HasPermission(Login.CurrentUser.Username, PermissionManagement.PermissionType.Administrator) & Login.Users.ContainsKey(Target))
                {
                    // Change password locally
                    NewPass = Encryption.GetEncryptedString(NewPass, Encryption.Algorithms.SHA256);
                    Login.Users[Target] = NewPass;

                    // Change password globally
                    SetUserProperty(Target, UserProperty.Password, NewPass);

                    // Raise event
                    Kernel.Kernel.KernelEventManager.RaiseUserPasswordChanged(Target);
                }
                else if (PermissionManagement.HasPermission(Login.CurrentUser.Username, PermissionManagement.PermissionType.Administrator) & !Login.Users.ContainsKey(Target))
                {
                    throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("User not found"));
                }
                else if (PermissionManagement.HasPermission(Target, PermissionManagement.PermissionType.Administrator) & !PermissionManagement.HasPermission(Login.CurrentUser.Username, PermissionManagement.PermissionType.Administrator))
                {
                    throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("You are not authorized to change password of {0} because the target was an admin."), Target);
                }
            }
            else
            {
                throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("Wrong user password."));
            }
        }

        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="Target">Target username</param>
        /// <param name="CurrentPass">Current user password</param>
        /// <param name="NewPass">New user password</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Exceptions.UserManagementException"></exception>
        public static bool TryChangePassword(string Target, string CurrentPass, string NewPass)
        {
            try
            {
                ChangePassword(Target, CurrentPass, NewPass);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Lists all users and includes anonymous and disabled users if enabled.
        /// </summary>
        public static List<string> ListAllUsers()
        {
            return ListAllUsers(IncludeAnonymous, IncludeDisabled);
        }

        /// <summary>
        /// Lists all users and includes anonymous and disabled users if enabled.
        /// </summary>
        /// <param name="IncludeAnonymous">Include anonymous users</param>
        /// <param name="IncludeDisabled">Include disabled users</param>
        public static List<string> ListAllUsers(bool IncludeAnonymous = false, bool IncludeDisabled = false)
        {
            var UsersList = new List<string>(Login.Users.Keys);
            if (!IncludeAnonymous)
            {
                UsersList.RemoveAll(new Predicate<string>(x => PermissionManagement.HasPermission(x, PermissionManagement.PermissionType.Anonymous) == true));
            }
            if (!IncludeDisabled)
            {
                UsersList.RemoveAll(new Predicate<string>(x => PermissionManagement.HasPermission(x, PermissionManagement.PermissionType.Disabled) == true));
            }
            return UsersList;
        }

        /// <summary>
        /// Selects a user from the <see cref="ListAllUsers(Boolean, Boolean)"/> list
        /// </summary>
        /// <param name="UserNumber">The user number. This is NOT an index!</param>
        /// <returns>The username which is selected</returns>
        public static string SelectUser(int UserNumber)
        {
            return SelectUser(UserNumber, IncludeAnonymous, IncludeDisabled);
        }

        /// <summary>
        /// Selects a user from the <see cref="ListAllUsers(Boolean, Boolean)"/> list
        /// </summary>
        /// <param name="UserNumber">The user number. This is NOT an index!</param>
        /// <param name="IncludeAnonymous">Include anonymous users</param>
        /// <param name="IncludeDisabled">Include disabled users</param>
        /// <returns>The username which is selected</returns>
        public static string SelectUser(int UserNumber, bool IncludeAnonymous = false, bool IncludeDisabled = false)
        {
            var UsersList = ListAllUsers(IncludeAnonymous, IncludeDisabled);
            string SelectedUsername = UsersList[UserNumber - 1];
            return Login.Users.Keys.First(x => (x ?? "") == (SelectedUsername ?? ""));
        }

        /// <summary>
        /// Handles the prompts for setting up a first user
        /// </summary>
        public static void FirstUserTrigger()
        {
            int Step = 1;
            string AnswerUsername = "";
            string AnswerPassword = "";
            string AnswerRootPassword = "";
            var AnswerType = default(int);

            // First, select user name
            TextWriterColor.Write(Translate.DoTranslation("It looks like you've got no user except root. This is bad. We'll guide you how to create one."), true, ColorTools.ColTypes.Neutral);
            while (Step == 1)
            {
                TextWriterColor.Write(Translate.DoTranslation("Write your username."), true, ColorTools.ColTypes.Neutral);
                TextWriterColor.Write(">> ", false, ColorTools.ColTypes.Input);
                AnswerUsername = Input.ReadLine();
                DebugWriter.Wdbg(DebugLevel.I, "Answer: {0}", AnswerUsername);
                if (string.IsNullOrWhiteSpace(AnswerUsername) & ReadLineReboot.ReadLine.ReadRanToCompletion)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Username is not valid. Returning...");
                    TextWriterColor.Write(Translate.DoTranslation("You must write your username."), true, ColorTools.ColTypes.Error);
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                    Console.ReadKey();
                }
                else
                {
                    Step += 1;
                }
            }

            // Second, write password
            while (Step == 2)
            {
                TextWriterColor.Write(Translate.DoTranslation("Write your password."), true, ColorTools.ColTypes.Neutral);
                TextWriterColor.Write(">> ", false, ColorTools.ColTypes.Input);
                AnswerPassword = Input.ReadLineNoInput();
                DebugWriter.Wdbg(DebugLevel.I, "Answer: {0}", AnswerPassword);
                if (string.IsNullOrWhiteSpace(AnswerPassword) & ReadLineReboot.ReadLine.ReadRanToCompletion)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Password is not valid. Returning...");
                    TextWriterColor.Write(Translate.DoTranslation("You must write your password."), true, ColorTools.ColTypes.Error);
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                    Console.ReadKey();
                }
                else
                {
                    Step += 1;
                }
            }

            // Third, select account type
            while (Step == 3)
            {
                TextWriterColor.Write(Translate.DoTranslation("Select account type.") + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Neutral);
                TextWriterColor.Write(" 1) " + Translate.DoTranslation("Administrator: This account type has the most power in the kernel, allowing you to use system management programs."), true, ColorTools.ColTypes.Option);
                TextWriterColor.Write(" 2) " + Translate.DoTranslation("Normal User: This account type is slightly more restricted than administrators."), true, ColorTools.ColTypes.Option);
                TextWriterColor.Write(Kernel.Kernel.NewLine + ">> ", false, ColorTools.ColTypes.Input);
                if (int.TryParse(Input.ReadLine(), out AnswerType))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Answer: {0}", AnswerType);
                    switch (AnswerType)
                    {
                        case 1:
                        case 2:
                            {
                                Step += 1; // ???
                                break;
                            }

                        default:
                            {
                                DebugWriter.Wdbg(DebugLevel.W, "Option is not valid. Returning...");
                                TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, ColorTools.ColTypes.Error, AnswerType);
                                TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                                Console.ReadKey();
                                break;
                            }
                    }
                }
                else if (ReadLineReboot.ReadLine.ReadRanToCompletion)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Answer is not numeric.");
                    TextWriterColor.Write(Translate.DoTranslation("The answer must be numeric."), true, ColorTools.ColTypes.Error);
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                    Console.ReadKey();
                }
            }

            // Fourth, write root password
            while (Step == 4)
            {
                if ((Login.Users["root"] ?? "") == (Encryption.GetEmptyHash(Encryption.Algorithms.SHA256) ?? ""))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Write the administrator password. Make sure that you don't use this account unless you really know what you're doing."), true, ColorTools.ColTypes.Neutral);
                    TextWriterColor.Write(">> ", false, ColorTools.ColTypes.Input);
                    AnswerRootPassword = Input.ReadLineNoInput();
                    DebugWriter.Wdbg(DebugLevel.I, "Answer: {0}", AnswerPassword);
                    if (string.IsNullOrWhiteSpace(AnswerPassword) & ReadLineReboot.ReadLine.ReadRanToCompletion)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Password is not valid. Returning...");
                        TextWriterColor.Write(Translate.DoTranslation("You must write the administrator password."), true, ColorTools.ColTypes.Error);
                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                        Console.ReadKey();
                    }
                    else
                    {
                        Step += 1;
                    }
                }
                else
                {
                    Step += 1;
                }
            }

            // Finally, create an account and change root password
            AddUser(AnswerUsername, AnswerPassword);
            if (AnswerType == 1)
                PermissionManagement.AddPermission(PermissionManagement.PermissionType.Administrator, AnswerUsername);

            // Actually change the root password if specified
            if (!string.IsNullOrEmpty(AnswerRootPassword))
            {
                AnswerRootPassword = Encryption.GetEncryptedString(AnswerRootPassword, Encryption.Algorithms.SHA256);
                SetUserProperty("root", UserProperty.Password, AnswerRootPassword);
                Login.Users["root"] = AnswerRootPassword;
            }

            // Write a congratulating message
            TextWriterColor.Write(Translate.DoTranslation("Congratulations! You've made a new account! To finish this off, log in as your new account."), true, ColorTools.ColTypes.Neutral);
        }

        /// <summary>
        /// Checks to see if the user exists
        /// </summary>
        /// <param name="User">The target user</param>
        public static bool UserExists(string User)
        {
            return Login.Users.ContainsKey(User);
        }

        /// <summary>
        /// Gets the unique user identifier for the current user
        /// </summary>
        public static string GetUserDollarSign()
        {
            return GetUserDollarSign(Login.CurrentUser.Username);
        }

        /// <summary>
        /// Gets the unique user identifier
        /// </summary>
        /// <param name="User">The target user</param>
        public static string GetUserDollarSign(string User)
        {
            if (UserExists(User))
            {
                if (PermissionManagement.HasPermission(User, PermissionManagement.PermissionType.Administrator))
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
                throw new Kernel.Exceptions.UserManagementException(Translate.DoTranslation("User not found"));
            }
        }

    }
}