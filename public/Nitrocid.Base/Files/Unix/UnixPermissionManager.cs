//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Files.Unix
{
    /// <summary>
    /// Unix permission manager
    /// </summary>
    public static class UnixPermissionManager
    {
        /// <summary>
        /// Calculates the permission number according to specified permission types
        /// </summary>
        /// <param name="permissionType">Permission types</param>
        /// <returns>Permission number</returns>
        public static int Calculate(UnixPermissionType permissionType)
        {
            int permNum = 0;
            if (permissionType.HasFlag(UnixPermissionType.Execute))
                permNum += (int)UnixPermissionType.Execute;
            if (permissionType.HasFlag(UnixPermissionType.Write))
                permNum += (int)UnixPermissionType.Write;
            if (permissionType.HasFlag(UnixPermissionType.Read))
                permNum += (int)UnixPermissionType.Read;
            return permNum;
        }

        /// <summary>
        /// Gets the permission type from a permission number
        /// </summary>
        /// <param name="permNum">Permission number to process</param>
        /// <returns>Processed permission type</returns>
        public static UnixPermissionType GetTypeFrom(int permNum)
        {
            UnixPermissionType permissionType = 0;
            var permNumCalculated = (UnixPermissionType)permNum;
            if (permNumCalculated.HasFlag(UnixPermissionType.Execute))
                permissionType |= UnixPermissionType.Execute;
            if (permNumCalculated.HasFlag(UnixPermissionType.Write))
                permissionType |= UnixPermissionType.Write;
            if (permNumCalculated.HasFlag(UnixPermissionType.Read))
                permissionType |= UnixPermissionType.Read;
            return permissionType;
        }

        /// <summary>
        /// Gets the permission type from a permission representation
        /// </summary>
        /// <param name="permRepresentation">Permission representation (rwx, r-x, ...)</param>
        /// <returns>Processed permission type</returns>
        public static UnixPermissionType GetTypeFrom(string permRepresentation)
        {
            // TODO: NKS_FILES_UNIX_EXCEPTION_REPRESENTATIONEMPTY -> Permission representation string is empty.
            if (string.IsNullOrEmpty(permRepresentation))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_UNIX_EXCEPTION_REPRESENTATIONEMPTY"));

            // TODO: NKS_FILES_UNIX_EXCEPTION_REPRESENTATIONLENGTH -> Permission representation string length must be three.
            if (permRepresentation.Length != 3)
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_UNIX_EXCEPTION_REPRESENTATIONLENGTH"));

            UnixPermissionType permissionType = 0;
            for (int i = 0; i < permRepresentation.Length; i++)
            {
                char perm = permRepresentation[i];
                switch (perm)
                {
                    case 'r':
                        permissionType |= UnixPermissionType.Read;
                        break;
                    case 'w':
                        permissionType |= UnixPermissionType.Write;
                        break;
                    case 'x':
                        permissionType |= UnixPermissionType.Execute;
                        break;
                    case '-':
                        break;
                    default:
                        // TODO: NKS_FILES_UNIX_EXCEPTION_REPRESENTATIONINVALID -> Invalid representation at position
                        throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_UNIX_EXCEPTION_REPRESENTATIONINVALID") + $" {i + 1}: {perm}");
                }
            }
            return permissionType;
        }

        /// <summary>
        /// Gets permission descriptors from a chmod number
        /// </summary>
        /// <param name="chmodNum">chmod permission number (for example, 755 or 644)</param>
        /// <returns>An array of three Unix permission descriptors for <see cref="UnixPermissionScope.User"/>, <see cref="UnixPermissionScope.Group"/>, and <see cref="UnixPermissionScope.Other"/></returns>
        public static UnixPermissionDescriptor[] GetDescriptors(int chmodNum)
        {
            var descriptors = new UnixPermissionDescriptor[3];

            // Make initial descriptor classes
            descriptors[0] = new(0, UnixPermissionScope.User);
            descriptors[1] = new(0, UnixPermissionScope.Group);
            descriptors[2] = new(0, UnixPermissionScope.Other);

            // Process the chmod number and verify that three digits are provided
            chmodNum = Math.Abs(chmodNum);
            if (chmodNum == 0)
                return descriptors;
            int digits = (int)Math.Floor(Math.Log10(chmodNum) + 1);

            // Get first three digits from the chmod number
            if (digits >= 3)
                chmodNum = (int)Math.Truncate(chmodNum / Math.Pow(10, digits - 3));

            // Process the chmod number to make permission descriptors
            for (int i = 0; i < 3; i++)
            {
                int permNum = chmodNum % 10;
                descriptors[2 - i].Types = GetTypeFrom(permNum);
                chmodNum /= 10;
            }
            return descriptors;
        }
    }
}
