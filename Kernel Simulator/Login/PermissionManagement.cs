
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
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Login
{
    public static class PermissionManagement
    {

        internal static Dictionary<string, PermissionType> UserPermissions = new();

        /// <summary>
        /// This enumeration lists all permission types.
        /// </summary>
        public enum PermissionType : int
        {
            /// <summary>
            /// User has no permissions
            /// </summary>
            None = 0,
            /// <summary>
            /// This user is an administrator
            /// </summary>
            Administrator = 1,
            /// <summary>
            /// This user is disabled
            /// </summary>
            Disabled = 2,
            /// <summary>
            /// This user doesn't show in the available users list
            /// </summary>
            Anonymous = 4
        }

        /// <summary>
        /// It specifies whether or not to allow permission
        /// </summary>
        public enum PermissionManagementMode : int
        {
            /// <summary>
            /// Adds the permission to the user properties
            /// </summary>
            Allow = 1,
            /// <summary>
            /// Removes the permission from the user properties
            /// </summary>
            Disallow
        }

        /// <summary>
        /// Manages permissions
        /// </summary>
        /// <param name="PermType">A type of permission</param>
        /// <param name="Username">A specified username</param>
        /// <param name="PermissionMode">Whether to allow or disallow a specified type for a user</param>
        public static void Permission(PermissionType PermType, string Username, PermissionManagementMode PermissionMode)
        {

            // Adds user into permission lists.
            try
            {
                DebugWriter.Wdbg(DebugLevel.I, "Mode: {0}", PermissionMode);
                if (PermissionMode == PermissionManagementMode.Allow)
                {
                    AddPermission(PermType, Username);
                    TextWriterColor.Write(Translate.DoTranslation("The user {0} has been added to the \"{1}\" list."), true, ColorTools.ColTypes.Neutral, Username, PermType.ToString());
                }
                else if (PermissionMode == PermissionManagementMode.Disallow)
                {
                    RemovePermission(PermType, Username);
                    TextWriterColor.Write(Translate.DoTranslation("The user {0} has been removed from the \"{1}\" list."), true, ColorTools.ColTypes.Neutral, Username, PermType.ToString());
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Mode is invalid");
                    TextWriterColor.Write(Translate.DoTranslation("Invalid mode {0}"), true, ColorTools.ColTypes.Error, PermissionMode);
                }
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.") + Kernel.Kernel.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, ColorTools.ColTypes.Error, ex.GetType().FullName, ex.Message);
                DebugWriter.WStkTrc(ex);
            }
        }

        /// <summary>
        /// Adds user to one of permission types
        /// </summary>
        /// <param name="PermType">Whether it be Admin or Disabled</param>
        /// <param name="Username">A username to be managed</param>
        /// <exception cref="Kernel.Exceptions.PermissionManagementException"></exception>
        public static void AddPermission(PermissionType PermType, string Username)
        {
            // Sets the required permissions to false.
            if (Login.Users.Keys.ToArray().Contains(Username))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Type is {0}", PermType);
                switch (PermType)
                {
                    case PermissionType.Administrator:
                        {
                            UserPermissions[Username] = UserPermissions[Username] + (int)PermissionType.Administrator;
                            break;
                        }
                    case PermissionType.Disabled:
                        {
                            UserPermissions[Username] = UserPermissions[Username] + (int)PermissionType.Disabled;
                            break;
                        }
                    case PermissionType.Anonymous:
                        {
                            UserPermissions[Username] = UserPermissions[Username] + (int)PermissionType.Anonymous;
                            break;
                        }

                    default:
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Type is invalid");
                            throw new Kernel.Exceptions.PermissionManagementException(Translate.DoTranslation("Failed to add user into permission lists: invalid type {0}"), PermType);
                        }
                }
                DebugWriter.Wdbg(DebugLevel.I, "User {0} permission added; value is now: {1}", Username, UserPermissions[Username]);
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.W, "User {0} not found on list", Username);
                throw new Kernel.Exceptions.PermissionManagementException(Translate.DoTranslation("Failed to add user into permission lists: invalid user {0}"), Username);
            }

            // Save changes
            foreach (JObject UserToken in UserManagement.UsersToken)
            {
                if ((UserToken["username"].ToString() ?? "") == (Username ?? ""))
                {
                    if (Convert.ToBoolean(!((JArray)UserToken["permissions"]).ToObject<List<string>>().Contains(PermType.ToString())))
                    {
                        ((JArray)UserToken["permissions"]).Add(PermType.ToString());
                    }
                }
            }
            File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UserManagement.UsersToken, Formatting.Indented));
        }

        /// <summary>
        /// Adds user to one of permission types
        /// </summary>
        /// <param name="PermType">Whether it be Admin or Disabled</param>
        /// <param name="Username">A username to be managed</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Kernel.Exceptions.PermissionManagementException"></exception>
        public static bool TryAddPermission(PermissionType PermType, string Username)
        {
            try
            {
                AddPermission(PermType, Username);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes user from one of permission types
        /// </summary>
        /// <param name="PermType">Whether it be Admin or Disabled</param>
        /// <param name="Username">A username to be managed</param>
        /// <exception cref="Kernel.Exceptions.PermissionManagementException"></exception>
        public static void RemovePermission(PermissionType PermType, string Username)
        {
            // Sets the required permissions to false.
            if (Login.Users.Keys.ToArray().Contains(Username) & (Username ?? "") != (Login.CurrentUser?.Username ?? ""))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Type is {0}", PermType);
                switch (PermType)
                {
                    case PermissionType.Administrator:
                        {
                            UserPermissions[Username] = UserPermissions[Username] - (int)PermissionType.Administrator;
                            break;
                        }
                    case PermissionType.Disabled:
                        {
                            UserPermissions[Username] = UserPermissions[Username] - (int)PermissionType.Disabled;
                            break;
                        }
                    case PermissionType.Anonymous:
                        {
                            UserPermissions[Username] = UserPermissions[Username] - (int)PermissionType.Anonymous;
                            break;
                        }

                    default:
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Type is invalid");
                            throw new Kernel.Exceptions.PermissionManagementException(Translate.DoTranslation("Failed to remove user from permission lists: invalid type {0}"), PermType);
                        }
                }
                DebugWriter.Wdbg(DebugLevel.I, "User {0} permission removed; value is now: {1}", Username, UserPermissions[Username]);
            }
            else if ((Username ?? "") == (Login.CurrentUser.Username ?? ""))
            {
                throw new Kernel.Exceptions.PermissionManagementException(Translate.DoTranslation("You are already logged in."));
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.W, "User {0} not found on list", Username);
                throw new Kernel.Exceptions.PermissionManagementException(Translate.DoTranslation("Failed to remove user from permission lists: invalid user {0}"), Username);
            }

            // Save changes
            foreach (JObject UserToken in UserManagement.UsersToken)
            {
                if ((UserToken["username"].ToString() ?? "") == (Username ?? ""))
                {
                    List<string> PermissionArray = (List<string>)UserToken["permissions"].ToObject(typeof(List<string>));
                    PermissionArray.Remove(PermType.ToString());
                    UserToken["permissions"] = JArray.FromObject(PermissionArray);
                }
            }
            File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UserManagement.UsersToken, Formatting.Indented));
        }

        /// <summary>
        /// Removes user from one of permission types
        /// </summary>
        /// <param name="PermType">Whether it be Admin or Disabled</param>
        /// <param name="Username">A username to be managed</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Kernel.Exceptions.PermissionManagementException"></exception>
        public static bool TryRemovePermission(PermissionType PermType, string Username)
        {
            try
            {
                RemovePermission(PermType, Username);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Edits the permission database for new user name
        /// </summary>
        /// <param name="OldName">Old username</param>
        /// <param name="Username">New username</param>
        /// <exception cref="Kernel.Exceptions.PermissionManagementException"></exception>
        public static void PermissionEditForNewUser(string OldName, string Username)
        {
            // Edit username
            if (UserPermissions.ContainsKey(OldName))
            {
                try
                {
                    // Store permissions
                    var UserOldPermissions = UserPermissions[OldName];

                    // Remove old user entry
                    DebugWriter.Wdbg(DebugLevel.I, "Removing {0} from permissions list...", OldName);
                    UserPermissions.Remove(OldName);

                    // Add new user entry
                    UserPermissions.Add(Username, UserOldPermissions);
                    DebugWriter.Wdbg(DebugLevel.I, "Added {0} to permissions list with value of {1}", Username, UserPermissions[Username]);
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    throw new Kernel.Exceptions.PermissionManagementException(Translate.DoTranslation("You have either found a bug, or the permission you tried to edit for a new user has failed.") + Kernel.Kernel.NewLine + Translate.DoTranslation("Error {0}: {1}"), ex, ex.GetType().FullName, ex.Message);
                }
            }
            else
            {
                throw new Kernel.Exceptions.PermissionManagementException(Translate.DoTranslation("One of the permission lists doesn't contain username {0}."), OldName);
            }
        }

        /// <summary>
        /// Edits the permission database for new user name
        /// </summary>
        /// <param name="OldName">Old username</param>
        /// <param name="Username">New username</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Kernel.Exceptions.PermissionManagementException"></exception>
        public static bool TryPermissionEditForNewUser(string OldName, string Username)
        {
            try
            {
                PermissionEditForNewUser(OldName, Username);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes permissions for a new user with default settings
        /// </summary>
        /// <param name="NewUser">A new user name</param>
        /// <exception cref="Kernel.Exceptions.PermissionManagementException"></exception>
        public static void InitPermissionsForNewUser(string NewUser)
        {
            // Initialize permissions locally
            if (!UserPermissions.ContainsKey(NewUser))
                UserPermissions.Add(NewUser, PermissionType.None);
        }

        /// <summary>
        /// Initializes permissions for a new user with default settings
        /// </summary>
        /// <param name="NewUser">A new user name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Kernel.Exceptions.PermissionManagementException"></exception>
        public static bool TryInitPermissionsForNewUser(string NewUser)
        {
            try
            {
                InitPermissionsForNewUser(NewUser);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Loads permissions for all users
        /// </summary>
        /// <exception cref="Kernel.Exceptions.PermissionManagementException"></exception>
        public static void LoadPermissions()
        {
            foreach (JObject UserToken in UserManagement.UsersToken)
            {
                string User = (string)UserToken["username"];
                UserPermissions[User] = PermissionType.None;
                foreach (string Perm in (JArray)UserToken["permissions"])
                {
                    switch (Perm ?? "")
                    {
                        case "Administrator":
                            {
                                UserPermissions[User] = UserPermissions[User] + (int)PermissionType.Administrator;
                                break;
                            }
                        case "Disabled":
                            {
                                UserPermissions[User] = UserPermissions[User] + (int)PermissionType.Disabled;
                                break;
                            }
                        case "Anonymous":
                            {
                                UserPermissions[User] = UserPermissions[User] + (int)PermissionType.Anonymous;
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Loads permissions for all users
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Kernel.Exceptions.PermissionManagementException"></exception>
        public static bool TryLoadPermissions()
        {
            try
            {
                LoadPermissions();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the permissions for the user
        /// </summary>
        /// <param name="Username">Target username</param>
        /// <returns>Permission type enumeration for the current user, or none if the user isn't found or has no permissions</returns>
        public static PermissionType GetPermissions(string Username)
        {
            if (string.IsNullOrEmpty(Username))
                Username = "";
            return UserPermissions.ContainsKey(Username) ? UserPermissions[Username] : PermissionType.None;
        }

        /// <summary>
        /// Checks to see if the user has a specific permission
        /// </summary>
        /// <param name="Username">Target username</param>
        /// <param name="SpecificPermission">Specific permission type</param>
        /// <returns>True if the user has permission; False otherwise</returns>
        public static bool HasPermission(string Username, PermissionType SpecificPermission)
        {
            var SpecificPermissions = GetPermissions(Username);
            return SpecificPermissions.HasFlag(SpecificPermission);
        }

    }
}
