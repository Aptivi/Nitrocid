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
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Users.Groups
{
    /// <summary>
    /// Group management routines
    /// </summary>
    public static class GroupManagement
    {
        internal static List<GroupInfo> AvailableGroups = [];

        /// <summary>
        /// Adds a group
        /// </summary>
        /// <param name="groupName">The group which will be added</param>
        public static void AddGroup(string groupName)
        {
            // Check the current login for permissions
            PermissionsTools.Demand(PermissionTypes.ManageGroups);

            // Check to see if we have the target group
            if (DoesGroupExist(groupName))
                throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("Can't add a group with the same name."));

            // Add the new group to the group list
            GroupInfo group = new(groupName, []);
            AvailableGroups.Add(group);
            SaveGroups();
        }

        /// <summary>
        /// Removes a group
        /// </summary>
        /// <param name="groupName">The group which will be removed</param>
        public static void RemoveGroup(string groupName)
        {
            // Check the current login for permissions
            PermissionsTools.Demand(PermissionTypes.ManageGroups);

            // Check to see if we have the target group
            if (!DoesGroupExist(groupName))
                throw new KernelException(KernelExceptionType.NoSuchGroup, Translate.DoTranslation("Can't remove a nonexistent group."));

            // Add the new group to the group list
            GroupInfo? group = GetGroup(groupName) ??
                throw new KernelException(KernelExceptionType.NoSuchGroup, Translate.DoTranslation("Can't remove a nonexistent group."));
            AvailableGroups.Remove(group);
            SaveGroups();
        }

        /// <summary>
        /// Adds a user to a group
        /// </summary>
        /// <param name="user">User which will be added to the group</param>
        /// <param name="groupName">The group that the user will join</param>
        public static void AddUserToGroup(string user, string groupName)
        {
            // Check the current login for permissions
            PermissionsTools.Demand(PermissionTypes.ManageGroups);

            // Check to see if we have the target group and user
            if (!UserManagement.UserExists(user))
                throw new KernelException(KernelExceptionType.NoSuchUser);
            if (!DoesGroupExist(groupName))
                throw new KernelException(KernelExceptionType.NoSuchGroup, Translate.DoTranslation("Can't add a user to a nonexistent group."));

            // Get a user and add them to the group
            int userIndex = UserManagement.GetUserIndex(user);
            var userGroups = new List<string>(UserManagement.Users[userIndex].Groups);
            if (userGroups.Contains(groupName))
                throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("User has already joined the group!"));
            userGroups.Add(groupName);
            DebugWriter.WriteDebug(DebugLevel.I, "Added user {0} to group {1}.", vars: [user, groupName]);
            UserManagement.Users[userIndex].Groups = [.. userGroups];
            UserManagement.SaveUsers();
        }

        /// <summary>
        /// Removes a user from the group
        /// </summary>
        /// <param name="user">User which will be removed from the group</param>
        /// <param name="groupName">The group that the user will leave</param>
        public static void RemoveUserFromGroup(string user, string groupName)
        {
            // Check the current login for permissions
            PermissionsTools.Demand(PermissionTypes.ManageGroups);

            // Check to see if we have the target group and user
            if (!UserManagement.UserExists(user))
                throw new KernelException(KernelExceptionType.NoSuchUser);
            if (!DoesGroupExist(groupName))
                throw new KernelException(KernelExceptionType.NoSuchGroup, Translate.DoTranslation("Can't remove a user from a nonexistent group."));

            // Get a user and remove them from the group
            int userIndex = UserManagement.GetUserIndex(user);
            var userGroups = new List<string>(UserManagement.Users[userIndex].Groups);
            if (!userGroups.Contains(groupName))
                throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("User has already left the group!"));
            userGroups.Remove(groupName);
            DebugWriter.WriteDebug(DebugLevel.I, "Removed user {0} from group {1}.", vars: [user, groupName]);
            UserManagement.Users[userIndex].Groups = [.. userGroups];
            UserManagement.SaveUsers();
        }

        /// <summary>
        /// Does a group exist?
        /// </summary>
        /// <param name="groupName">The group</param>
        /// <returns>True we have a group with such name. False otherwise.</returns>
        public static bool DoesGroupExist(string groupName) =>
            AvailableGroups.Any((x) => x.GroupName == groupName);

        /// <summary>
        /// Is a user found in a group?
        /// </summary>
        /// <param name="user">User which will be queried from the group</param>
        /// <param name="groupName">The group</param>
        /// <returns>True if the user is in group. False otherwise.</returns>
        public static bool IsUserInGroup(string user, string groupName)
        {
            // Check to see if we have the target group
            if (!DoesGroupExist(groupName))
                throw new KernelException(KernelExceptionType.NoSuchGroup, Translate.DoTranslation("Can't query a user from a nonexistent group."));

            // Get the user group array first, then check to see if we have a group entry for a user
            var userInstance = UserManagement.GetUser(user) ??
                throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("Can't get user info for") + $" {user}");
            string[] groupNames = userInstance.Groups;
            DebugWriter.WriteDebug(DebugLevel.I, "User {0} in group {1}? Refer to: [{2}]", vars: [user, groupName, string.Join(", ", groupNames)]);
            return groupNames.Length > 0 && groupNames.Any((group) => group == groupName);
        }

        /// <summary>
        /// Gets the user groups
        /// </summary>
        /// <param name="user">User which will be queried from the groups</param>
        /// <returns>Group information array containing info about joined groups</returns>
        public static GroupInfo[] GetUserGroups(string user)
        {
            // Check to see if we have the target user
            if (!UserManagement.UserExists(user))
                throw new KernelException(KernelExceptionType.NoSuchUser);

            // Get the user group array first, then compare against all the group elements for the group name
            var userInstance = UserManagement.GetUser(user) ??
                throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("Can't get user info for") + $" {user}");
            string[] groupNames = userInstance.Groups;
            DebugWriter.WriteDebug(DebugLevel.I, "User {0}'s groups: [{1}]", vars: [user, string.Join(", ", groupNames)]);
            return AvailableGroups.Where((group) => groupNames.Contains(group.GroupName)).ToArray();
        }

        /// <summary>
        /// Gets a group
        /// </summary>
        /// <param name="groupName">The group</param>
        /// <returns>Group information</returns>
        public static GroupInfo? GetGroup(string groupName)
        {
            // Check to see if we have the target group
            if (!DoesGroupExist(groupName))
                throw new KernelException(KernelExceptionType.NoSuchGroup);

            return AvailableGroups.FirstOrDefault(x => x.GroupName == groupName);
        }

        /// <summary>
        /// Gets a group index
        /// </summary>
        /// <param name="groupName">The group</param>
        /// <returns>Group index</returns>
        public static int GetGroupIndex(string groupName)
        {
            // Check to see if we have the target group
            if (!DoesGroupExist(groupName))
                throw new KernelException(KernelExceptionType.NoSuchGroup);

            return AvailableGroups.FindIndex(x => x.GroupName == groupName);
        }

        internal static void InitializeGroups()
        {
            // First, check to see if we have the groups file
            if (!FilesystemTools.FileExists(PathsManagement.UserGroupsPath))
                SaveGroups();

            // Get the group information instances to the user groups path
            string groupInfosJson = FilesystemTools.ReadContentsText(PathsManagement.UserGroupsPath);
            JArray? groupInfoArrays = (JArray?)JsonConvert.DeserializeObject(groupInfosJson) ??
                throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("Can't deserialize group info array"));
            List<GroupInfo> groups = [];
            foreach (var groupInfoArray in groupInfoArrays)
            {
                GroupInfo groupInfo = JsonConvert.DeserializeObject<GroupInfo>(groupInfoArray.ToString()) ??
                    throw new KernelException(KernelExceptionType.GroupManagement, Translate.DoTranslation("Can't deserialize group info"));
                groups.Add(groupInfo);
            }
            AvailableGroups = groups;
        }

        internal static void SaveGroups()
        {
            // Make a JSON file to save all group information files
            string groupInfosSerialized = JsonConvert.SerializeObject(AvailableGroups.ToArray(), Formatting.Indented);
            FilesystemTools.WriteContentsText(PathsManagement.UserGroupsPath, groupInfosSerialized);
        }

        internal static void ChangePermissionInternal(string groupName, string[] newPermissions)
        {
            // Get the group index to change the permissions
            int index = GetGroupIndex(groupName);
            AvailableGroups[index].Permissions = newPermissions;
            SaveGroups();
        }
    }
}
