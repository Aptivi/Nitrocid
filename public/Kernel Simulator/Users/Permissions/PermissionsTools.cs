
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

using KS.Kernel.Exceptions;
using KS.Users.Login;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.Users.Permissions
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
            IsPermissionGranted(Login.Login.CurrentUser.Username, permissionType);

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

            // If admin, always granted
            if ((bool)UserManagement.GetUserProperty(User, UserManagement.UserProperty.Admin))
                return true;

            // Now, query the user for permissions
            return Login.Login.Users[User].Permissions.Contains(permissionType.ToString());
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

            // Check to see if the current user is granted permission management or not
            if (!IsPermissionGranted(PermissionTypes.ManagePermissions))
                throw new KernelException(KernelExceptionType.PermissionDenied);

            // Get all the permission types
            foreach (PermissionTypes type in Enum.GetValues(typeof(PermissionTypes)))
            {
                // Check to see if one or more permissions exist
                if (permissionType.HasFlag(type))
                {
                    // Exists! Check the user permissions to see if the permission is already granted
                    var perms = new List<string>(Login.Login.Users[User].Permissions);
                    if (!perms.Contains(type.ToString()))
                    {
                        // Permission is not already granted. Add it
                        perms.Add(type.ToString());

                        // Now, change the permission variable
                        Login.Login.Users[User].Permissions = perms.ToArray();
                    }
                }
            }

            // Save the changes
            UserManagement.SetUserProperty(User, UserManagement.UserProperty.Permissions, JToken.FromObject(Login.Login.Users[User].Permissions));
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

            // Check to see if the current user is granted permission management or not
            if (!IsPermissionGranted(PermissionTypes.ManagePermissions))
                throw new KernelException(KernelExceptionType.PermissionDenied);

            // Get all the permission types
            foreach (PermissionTypes type in Enum.GetValues(typeof(PermissionTypes)))
            {
                // Check to see if one or more permissions exist
                if (permissionType.HasFlag(type))
                {
                    // Exists! Check the user permissions to see if the permission is already revoked
                    var perms = new List<string>(Login.Login.Users[User].Permissions);
                    if (perms.Contains(type.ToString()))
                    {
                        // Permission is not already revoked. Revoke it
                        perms.Remove(type.ToString());

                        // Now, change the permission variable
                        Login.Login.Users[User].Permissions = perms.ToArray();
                    }
                }
            }

            // Save the changes
            UserManagement.SetUserProperty(User, UserManagement.UserProperty.Permissions, JToken.FromObject(Login.Login.Users[User].Permissions));
        }
    }
}
