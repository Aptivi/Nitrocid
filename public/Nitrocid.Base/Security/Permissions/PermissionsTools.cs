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

using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Journaling;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Users.Groups;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Base.Security.Permissions
{
    /// <summary>
    /// Permission tools
    /// </summary>
    public static class PermissionsTools
    {
        /// <summary>
        /// Checks to see whether the current user is granted permissions
        /// </summary>
        /// <param name="permissionType">A permission type to query</param>
        /// <returns>True if granted or if user is an admin. Otherwise, false.</returns>
        public static bool IsPermissionGranted(PermissionTypes permissionType) =>
            IsPermissionGranted(UserManagement.CurrentUser.Username, permissionType);

        /// <summary>
        /// Checks to see whether the user is granted permissions
        /// </summary>
        /// <param name="User">Target user</param>
        /// <param name="permissionType">A permission type to query</param>
        /// <returns>True if granted or if user is an admin. Otherwise, false.</returns>
        public static bool IsPermissionGranted(string User, PermissionTypes permissionType)
        {
            // Check to see if we have the target user
            if (!UserManagement.UserExists(User))
                throw new KernelException(KernelExceptionType.NoSuchUser);
            var user = UserManagement.GetUser(User) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);

            // If admin, always granted
            if (user.Flags.HasFlag(UserFlags.Administrator))
                return true;

            // Now, query the user for permissions
            return user.Permissions.Contains(permissionType.ToString()) ||
                   GroupManagement.GetUserGroups(User).Select((groupInfo) => groupInfo.Permissions.Contains(permissionType.ToString())).Contains(true);
        }

        /// <summary>
        /// Checks to see whether the current user is granted the permission. Fails if permission is not granted.
        /// </summary>
        /// <param name="permissionType">A permission type to query</param>
        /// <exception cref="KernelException"></exception>
        public static void Demand(PermissionTypes permissionType)
        {
            // Don't demand when kernel is errored
            if (KernelPanic.KernelErrored)
                return;

            // Get all the permission types
            DebugWriter.WriteDebug(DebugLevel.I, "Demanding permissions: {0}", vars: [permissionType.ToString()]);
            foreach (PermissionTypes type in Enum.GetValues(typeof(PermissionTypes)))
            {
                // Check to see if one or more permissions exist
                if (permissionType.HasFlag(type))
                {
                    bool granted = IsPermissionGranted(type);
                    DebugWriter.WriteDebug(DebugLevel.I, "Demanded permission {0}, and granted is {1}.", vars: [type.ToString(), granted]);
                    JournalManager.WriteJournal(LanguageTools.GetLocalized("NKS_SECURITY_PERMISSIONS_DEMANDING") + $" {type} [{permissionType}]: {granted}");
                    if (!granted)
                        throw new KernelException(KernelExceptionType.PermissionDenied, LanguageTools.GetLocalized("NKS_SECURITY_PERMISSIONS_EXCEPTION_UNGRANTED") + ": {0}", permissionType.ToString());
                }
            }
        }

        /// <summary>
        /// Grants the user a permission
        /// </summary>
        /// <param name="User">Username to give the permission to</param>
        /// <param name="permissionType">Permission types to grant</param>
        public static void GrantPermission(string User, PermissionTypes permissionType)
        {
            // Check to see if we have the target user
            if (!UserManagement.UserExists(User))
                throw new KernelException(KernelExceptionType.NoSuchUser);
            var user = UserManagement.GetUser(User) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);

            // Check to see if the current user is granted permission management or not
            Demand(PermissionTypes.ManagePermissions);
            DebugWriter.WriteDebug(DebugLevel.I, "Going ahead...");

            // Get all the permission types
            foreach (PermissionTypes type in Enum.GetValues(typeof(PermissionTypes)))
            {
                // Check to see if one or more permissions exist
                if (permissionType.HasFlag(type))
                {
                    // Exists! Check the user permissions to see if the permission is already granted
                    int userIndex = UserManagement.GetUserIndex(User);
                    var perms = new List<string>(user.Permissions);
                    DebugWriter.WriteDebug(DebugLevel.I, "Index of user: {0}, perms: {1} [{2}].", vars: [userIndex, perms.Count, string.Join(", ", perms)]);
                    if (!perms.Contains(type.ToString()))
                    {
                        // Permission is not already granted. Add it
                        perms.Add(type.ToString());
                        DebugWriter.WriteDebug(DebugLevel.I, "Granted permission {0} to user {1}", vars: [type.ToString(), User]);

                        // Now, change the permission variable
                        UserManagement.Users[userIndex].Permissions = [.. perms];
                    }
                }
            }

            // Save the changes
            DebugWriter.WriteDebug(DebugLevel.I, "Saving settings for user {0}", vars: [User]);
            UserManagement.SaveUsers();
        }

        /// <summary>
        /// Revokes the user a permission
        /// </summary>
        /// <param name="User">Username to revoke the permission from</param>
        /// <param name="permissionType">Permission types to revoke</param>
        public static void RevokePermission(string User, PermissionTypes permissionType)
        {
            // Check to see if we have the target user
            if (!UserManagement.UserExists(User))
                throw new KernelException(KernelExceptionType.NoSuchUser);
            var user = UserManagement.GetUser(User) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);

            // Check to see if the current user is granted permission management or not
            Demand(PermissionTypes.ManagePermissions);
            DebugWriter.WriteDebug(DebugLevel.I, "Going ahead...");

            // Get all the permission types
            foreach (PermissionTypes type in Enum.GetValues(typeof(PermissionTypes)))
            {
                // Check to see if one or more permissions exist
                if (permissionType.HasFlag(type))
                {
                    // Exists! Check the user permissions to see if the permission is already revoked
                    int userIndex = UserManagement.GetUserIndex(User);
                    var perms = new List<string>(user.Permissions);
                    DebugWriter.WriteDebug(DebugLevel.I, "Index of user: {0}, perms: {1} [{2}].", vars: [userIndex, perms.Count, string.Join(", ", perms)]);
                    if (perms.Contains(type.ToString()))
                    {
                        // Permission is not already revoked. Revoke it
                        perms.Remove(type.ToString());
                        DebugWriter.WriteDebug(DebugLevel.I, "Revoked permission {0} from user {1}", vars: [type.ToString(), User]);

                        // Now, change the permission variable
                        UserManagement.Users[userIndex].Permissions = [.. perms];
                    }
                }
            }

            // Save the changes
            DebugWriter.WriteDebug(DebugLevel.I, "Saving settings for user {0}", vars: [User]);
            UserManagement.SaveUsers();
        }

        /// <summary>
        /// Grants the group a permission
        /// </summary>
        /// <param name="Group">Group name to give the permission to</param>
        /// <param name="permissionType">Permission types to grant</param>
        public static void GrantPermissionGroup(string Group, PermissionTypes permissionType)
        {
            // Check to see if we have the target group
            if (!GroupManagement.DoesGroupExist(Group))
                throw new KernelException(KernelExceptionType.NoSuchGroup);
            var group = GroupManagement.GetGroup(Group) ??
                throw new KernelException(KernelExceptionType.NoSuchGroup);

            // Check to see if the current group is granted permission management or not
            Demand(PermissionTypes.ManagePermissions);
            DebugWriter.WriteDebug(DebugLevel.I, "Going ahead...");

            // Get all the permission types
            foreach (PermissionTypes type in Enum.GetValues(typeof(PermissionTypes)))
            {
                // Check to see if one or more permissions exist
                if (permissionType.HasFlag(type))
                {
                    // Exists! Check the group permissions to see if the permission is already granted
                    var perms = new List<string>(group.Permissions);
                    DebugWriter.WriteDebug(DebugLevel.I, "Permissions for group {0}: {1} [{2}].", vars: [Group, perms.Count, string.Join(", ", perms)]);
                    if (!perms.Contains(type.ToString()))
                    {
                        // Permission is not already granted. Add it
                        perms.Add(type.ToString());
                        DebugWriter.WriteDebug(DebugLevel.I, "Granted permission {0} to group {1}", vars: [type.ToString(), Group]);

                        // Now, change the permission variable
                        GroupManagement.ChangePermissionInternal(Group, [.. perms]);
                    }
                }
            }

            // Save the changes
            DebugWriter.WriteDebug(DebugLevel.I, "Saving settings for group {0}", vars: [Group]);
            GroupManagement.SaveGroups();
        }

        /// <summary>
        /// Revokes the group a permission
        /// </summary>
        /// <param name="Group">Group name to revoke the permission from</param>
        /// <param name="permissionType">Permission types to revoke</param>
        public static void RevokePermissionGroup(string Group, PermissionTypes permissionType)
        {
            // Check to see if we have the target group
            if (!GroupManagement.DoesGroupExist(Group))
                throw new KernelException(KernelExceptionType.NoSuchGroup);
            var group = GroupManagement.GetGroup(Group) ??
                throw new KernelException(KernelExceptionType.NoSuchGroup);

            // Check to see if the current group is granted permission management or not
            Demand(PermissionTypes.ManagePermissions);
            DebugWriter.WriteDebug(DebugLevel.I, "Going ahead...");

            // Get all the permission types
            foreach (PermissionTypes type in Enum.GetValues(typeof(PermissionTypes)))
            {
                // Check to see if one or more permissions exist
                if (permissionType.HasFlag(type))
                {
                    // Exists! Check the group permissions to see if the permission is already revoked
                    var perms = new List<string>(group.Permissions);
                    DebugWriter.WriteDebug(DebugLevel.I, "Permissions for group {0}: {1} [{2}].", vars: [Group, perms.Count, string.Join(", ", perms)]);
                    if (perms.Contains(type.ToString()))
                    {
                        // Permission is not already revoked. Revoke it
                        perms.Remove(type.ToString());
                        DebugWriter.WriteDebug(DebugLevel.I, "Revoked permission {0} from group {1}", vars: [type.ToString(), Group]);

                        // Now, change the permission variable
                        GroupManagement.ChangePermissionInternal(Group, [.. perms]);
                    }
                }
            }

            // Save the changes
            DebugWriter.WriteDebug(DebugLevel.I, "Saving settings for group {0}", vars: [Group]);
            GroupManagement.SaveGroups();
        }
    }
}
