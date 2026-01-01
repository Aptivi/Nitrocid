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

using Nitrocid.Drivers;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Security.Privacy;
using Nitrocid.Security.Privacy.Consents;

namespace Nitrocid.Files.Operations
{
    /// <summary>
    /// Writing tools for files
    /// </summary>
    public static class Writing
    {

        /// <summary>
        /// Writes the contents to the file. This is blocking and will put a lock on the file until written.
        /// </summary>
        /// <param name="filename">Full path to file</param>
        /// <param name="contents">File contents to write to</param>
        public static void WriteContents(string filename, string[] contents)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Access to the path is denied due to no consent."));
            DriverHandler.CurrentFilesystemDriverLocal.WriteContents(filename, contents);
        }

        /// <summary>
        /// Writes the contents to the file. This is blocking and will put a lock on the file until written.
        /// </summary>
        /// <param name="filename">Full path to file</param>
        /// <param name="contents">File contents to write to</param>
        public static void WriteContentsText(string filename, string contents)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Access to the path is denied due to no consent."));
            DriverHandler.CurrentFilesystemDriverLocal.WriteContentsText(filename, contents);
        }

        /// <summary>
        /// Opens a file, writes all lines, and closes it
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="contents">File contents to write to</param>
        public static void WriteAllLinesNoBlock(string path, string[] contents)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Access to the path is denied due to no consent."));
            DriverHandler.CurrentFilesystemDriverLocal.WriteAllLinesNoBlock(path, contents);
        }

        /// <summary>
        /// Opens a file, writes all lines, and closes it
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="contents">File contents to write to</param>
        public static void WriteAllTextNoBlock(string path, string contents)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Access to the path is denied due to no consent."));
            DriverHandler.CurrentFilesystemDriverLocal.WriteAllTextNoBlock(path, contents);
        }

        /// <summary>
        /// Writes all the bytes
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <param name="contents">File contents to write to</param>
        public static void WriteAllBytes(string path, byte[] contents)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Access to the path is denied due to no consent."));
            DriverHandler.CurrentFilesystemDriverLocal.WriteAllBytes(path, contents);
        }

        /// <summary>
        /// Writes all the bytes
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <param name="contents">File contents to write to</param>
        public static void WriteAllBytesNoBlock(string path, byte[] contents)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Access to the path is denied due to no consent."));
            DriverHandler.CurrentFilesystemDriverLocal.WriteAllBytesNoBlock(path, contents);
        }
    }
}
