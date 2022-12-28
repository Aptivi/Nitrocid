
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Users.Groups
{
    /// <summary>
    /// Group management module
    /// </summary>
    public static class GroupManagement
    {

        internal static Dictionary<string, GroupType> UserGroups = new();

        /// <summary>
        /// This enumeration lists all group types.
        /// </summary>
        public enum GroupType : int
        {
            /// <summary>
            /// User has no groups
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
        /// It specifies whether or not to allow group
        /// </summary>
        public enum GroupManagementMode : int
        {
            /// <summary>
            /// Adds the group to the user properties
            /// </summary>
            Allow = 1,
            /// <summary>
            /// Removes the group from the user properties
            /// </summary>
            Disallow
        }

        /// <summary>
        /// Manages groups
        /// </summary>
        /// <param name="PermType">A type of group</param>
        /// <param name="Username">A specified username</param>
        /// <param name="GroupMode">Whether to allow or disallow a specified type for a user</param>
        public static void Group(GroupType PermType, string Username, GroupManagementMode GroupMode)
        {

            // Adds user into group lists.
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Mode: {0}", GroupMode);
                if (GroupMode == GroupManagementMode.Allow)
                {
                    AddGroup(PermType, Username);
                    TextWriterColor.Write(Translate.DoTranslation("The user {0} has been added to the \"{1}\" list."), Username, PermType.ToString());
                }
                else if (GroupMode == GroupManagementMode.Disallow)
                {
                    RemoveGroup(PermType, Username);
                    TextWriterColor.Write(Translate.DoTranslation("The user {0} has been removed from the \"{1}\" list."), Username, PermType.ToString());
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Mode is invalid");
                    TextWriterColor.Write(Translate.DoTranslation("Invalid mode {0}"), true, KernelColorType.Error, GroupMode);
                }
            }
            catch (Exception ex)
            {
                TextWriterColor.Write(Translate.DoTranslation("You have either found a bug, or the group you tried to add or remove is already done, or other error.") + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, KernelColorType.Error, ex.GetType().FullName, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        /// <summary>
        /// Adds user to one of group types
        /// </summary>
        /// <param name="PermType">Whether it be Admin or Disabled</param>
        /// <param name="Username">A username to be managed</param>
        public static void AddGroup(GroupType PermType, string Username)
        {
            // Sets the required groups to false.
            if (Login.Login.Users.Keys.ToArray().Contains(Username))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Type is {0}", PermType);
                switch (PermType)
                {
                    case GroupType.Administrator:
                        {
                            UserGroups[Username] = UserGroups[Username] + (int)GroupType.Administrator;
                            break;
                        }
                    case GroupType.Disabled:
                        {
                            UserGroups[Username] = UserGroups[Username] + (int)GroupType.Disabled;
                            break;
                        }
                    case GroupType.Anonymous:
                        {
                            UserGroups[Username] = UserGroups[Username] + (int)GroupType.Anonymous;
                            break;
                        }

                    default:
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Type is invalid");
                            throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("Failed to add user into group lists: invalid type {0}"), PermType);
                        }
                }
                DebugWriter.WriteDebug(DebugLevel.I, "User {0} group added; value is now: {1}", Username, UserGroups[Username]);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "User {0} not found on list", Username);
                throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("Failed to add user into group lists: invalid user {0}"), Username);
            }

            // Save changes
            foreach (JObject UserToken in UserManagement.UsersToken)
            {
                if (UserToken["username"].ToString() == Username)
                {
                    if (Convert.ToBoolean(!((JArray)UserToken["groups"]).ToObject<List<string>>().Contains(PermType.ToString())))
                    {
                        ((JArray)UserToken["groups"]).Add(PermType.ToString());
                    }
                }
            }
            File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UserManagement.UsersToken, Formatting.Indented));
        }

        /// <summary>
        /// Adds user to one of group types
        /// </summary>
        /// <param name="PermType">Whether it be Admin or Disabled</param>
        /// <param name="Username">A username to be managed</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryAddGroup(GroupType PermType, string Username)
        {
            try
            {
                AddGroup(PermType, Username);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes user from one of group types
        /// </summary>
        /// <param name="PermType">Whether it be Admin or Disabled</param>
        /// <param name="Username">A username to be managed</param>
        public static void RemoveGroup(GroupType PermType, string Username)
        {
            // Sets the required groups to false.
            if (Login.Login.Users.Keys.ToArray().Contains(Username) & Username != (Login.Login.CurrentUser?.Username))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Type is {0}", PermType);
                switch (PermType)
                {
                    case GroupType.Administrator:
                        {
                            UserGroups[Username] = UserGroups[Username] - (int)GroupType.Administrator;
                            break;
                        }
                    case GroupType.Disabled:
                        {
                            UserGroups[Username] = UserGroups[Username] - (int)GroupType.Disabled;
                            break;
                        }
                    case GroupType.Anonymous:
                        {
                            UserGroups[Username] = UserGroups[Username] - (int)GroupType.Anonymous;
                            break;
                        }

                    default:
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Type is invalid");
                            throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("Failed to remove user from group lists: invalid type {0}"), PermType);
                        }
                }
                DebugWriter.WriteDebug(DebugLevel.I, "User {0} group removed; value is now: {1}", Username, UserGroups[Username]);
            }
            else if (Username == Login.Login.CurrentUser.Username)
            {
                throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("You are already logged in."));
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "User {0} not found on list", Username);
                throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("Failed to remove user from group lists: invalid user {0}"), Username);
            }

            // Save changes
            foreach (JObject UserToken in UserManagement.UsersToken)
            {
                if (UserToken["username"].ToString() == Username)
                {
                    List<string> GroupArray = (List<string>)UserToken["groups"].ToObject(typeof(List<string>));
                    GroupArray.Remove(PermType.ToString());
                    UserToken["groups"] = JArray.FromObject(GroupArray);
                }
            }
            File.WriteAllText(Paths.GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UserManagement.UsersToken, Formatting.Indented));
        }

        /// <summary>
        /// Removes user from one of group types
        /// </summary>
        /// <param name="PermType">Whether it be Admin or Disabled</param>
        /// <param name="Username">A username to be managed</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveGroup(GroupType PermType, string Username)
        {
            try
            {
                RemoveGroup(PermType, Username);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Edits the group database for new user name
        /// </summary>
        /// <param name="OldName">Old username</param>
        /// <param name="Username">New username</param>
        public static void GroupEditForNewUser(string OldName, string Username)
        {
            // Edit username
            if (UserGroups.ContainsKey(OldName))
            {
                try
                {
                    // Store groups
                    var UserOldGroups = UserGroups[OldName];

                    // Remove old user entry
                    DebugWriter.WriteDebug(DebugLevel.I, "Removing {0} from groups list...", OldName);
                    UserGroups.Remove(OldName);

                    // Add new user entry
                    UserGroups.Add(Username, UserOldGroups);
                    DebugWriter.WriteDebug(DebugLevel.I, "Added {0} to groups list with value of {1}", Username, UserGroups[Username]);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("You have either found a bug, or the group you tried to edit for a new user has failed.") + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), ex, ex.GetType().FullName, ex.Message);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("One of the group lists doesn't contain username {0}."), OldName);
            }
        }

        /// <summary>
        /// Edits the group database for new user name
        /// </summary>
        /// <param name="OldName">Old username</param>
        /// <param name="Username">New username</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryGroupEditForNewUser(string OldName, string Username)
        {
            try
            {
                GroupEditForNewUser(OldName, Username);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes groups for a new user with default settings
        /// </summary>
        /// <param name="NewUser">A new user name</param>
        public static void InitGroupsForNewUser(string NewUser)
        {
            // Initialize groups locally
            if (!UserGroups.ContainsKey(NewUser))
                UserGroups.Add(NewUser, GroupType.None);
        }

        /// <summary>
        /// Initializes groups for a new user with default settings
        /// </summary>
        /// <param name="NewUser">A new user name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryInitGroupsForNewUser(string NewUser)
        {
            try
            {
                InitGroupsForNewUser(NewUser);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Loads groups for all users
        /// </summary>
        public static void LoadGroups()
        {
            foreach (JObject UserToken in UserManagement.UsersToken)
            {
                string User = (string)UserToken["username"];
                UserGroups[User] = GroupType.None;
                foreach (string Perm in (JArray)UserToken["groups"])
                {
                    switch (Perm)
                    {
                        case "Administrator":
                            {
                                UserGroups[User] = UserGroups[User] + (int)GroupType.Administrator;
                                break;
                            }
                        case "Disabled":
                            {
                                UserGroups[User] = UserGroups[User] + (int)GroupType.Disabled;
                                break;
                            }
                        case "Anonymous":
                            {
                                UserGroups[User] = UserGroups[User] + (int)GroupType.Anonymous;
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Loads groups for all users
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryLoadGroups()
        {
            try
            {
                LoadGroups();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the groups for the user
        /// </summary>
        /// <param name="Username">Target username</param>
        /// <returns>Group type enumeration for the current user, or none if the user isn't found or has no groups</returns>
        public static GroupType GetGroups(string Username)
        {
            if (string.IsNullOrEmpty(Username))
                Username = "";
            return UserGroups.ContainsKey(Username) ? UserGroups[Username] : GroupType.None;
        }

        /// <summary>
        /// Checks to see if the user has a specific group
        /// </summary>
        /// <param name="Username">Target username</param>
        /// <param name="SpecificGroup">Specific group type</param>
        /// <returns>True if the user has group; False otherwise</returns>
        public static bool HasGroup(string Username, GroupType SpecificGroup)
        {
            var SpecificGroups = GetGroups(Username);
            return SpecificGroups.HasFlag(SpecificGroup);
        }

    }
}
