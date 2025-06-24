//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Security.Privacy;
using Nitrocid.Base.Security.Privacy.Consents;

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// Removing file operations module
    /// </summary>
    public static partial class FilesystemTools
    {

        /// <summary>
        /// Removes a directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        public static void RemoveDirectory(string Target)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            DriverHandler.CurrentFilesystemDriverLocal.RemoveDirectory(Target, Config.MainConfig.ShowFilesystemProgress);
        }

        /// <summary>
        /// Removes a directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        /// <param name="ShowProgress">Whether or not to show what files are being removed</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        public static void RemoveDirectory(string Target, bool ShowProgress, bool secureRemove = false)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            DriverHandler.CurrentFilesystemDriverLocal.RemoveDirectory(Target, ShowProgress, secureRemove);
        }

        /// <summary>
        /// Removes a directory
        /// </summary>
        /// <param name="Target">Target directory</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveDirectory(string Target, bool secureRemove = false)
        {
            try
            {
                RemoveDirectory(Target, secureRemove);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Removes a file
        /// </summary>
        /// <param name="Target">Target directory</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        public static void RemoveFile(string Target, bool secureRemove = false)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            DriverHandler.CurrentFilesystemDriverLocal.RemoveFile(Target, secureRemove);
        }

        /// <summary>
        /// Removes a file
        /// </summary>
        /// <param name="Target">Target directory</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveFile(string Target, bool secureRemove = false)
        {
            try
            {
                RemoveFile(Target, secureRemove);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Removes file or directory
        /// </summary>
        /// <param name="Target">Path to file or directory</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        public static void RemoveFileOrDir(string Target, bool secureRemove = false)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            DriverHandler.CurrentFilesystemDriverLocal.RemoveFileOrDir(Target, secureRemove);
        }

        /// <summary>
        /// Removes a file or directory
        /// </summary>
        /// <param name="Target">Target file or directory</param>
        /// <param name="secureRemove">Securely remove file by filling it with zeroes</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryRemoveFileOrDir(string Target, bool secureRemove = false)
        {
            try
            {
                RemoveFileOrDir(Target, secureRemove);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

    }
}
