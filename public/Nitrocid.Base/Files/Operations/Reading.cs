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

using Nitrocid.Base.Drivers;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Security.Privacy;
using Nitrocid.Base.Security.Privacy.Consents;
using System.IO;

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// File reading module
    /// </summary>
    public static partial class FilesystemTools
    {
        /// <summary>
        /// Reads the contents of a file and writes it to the array. This is blocking and will put a lock on the file until read.
        /// </summary>
        /// <param name="filename">Full path to file</param>
        /// <returns>An array full of file contents</returns>
        public static string[] ReadContents(string filename)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemRead))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            return DriverHandler.CurrentFilesystemDriverLocal.ReadContents(filename);
        }

        /// <summary>
        /// Reads the contents of a file and writes it to the string. This is blocking and will put a lock on the file until read.
        /// </summary>
        /// <param name="filename">Full path to file</param>
        /// <returns>A text full of file contents</returns>
        public static string ReadContentsText(string filename)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemRead))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            return DriverHandler.CurrentFilesystemDriverLocal.ReadContentsText(filename);
        }

        /// <summary>
        /// Opens a file, reads all lines, and returns the array of lines
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Array of lines</returns>
        public static string[] ReadAllLinesNoBlock(string path)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemRead))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            return DriverHandler.CurrentFilesystemDriverLocal.ReadAllLinesNoBlock(path);
        }

        /// <summary>
        /// Opens a file, reads all lines, and returns the string of lines
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>String of lines</returns>
        public static string ReadAllTextNoBlock(string path)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemRead))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            return DriverHandler.CurrentFilesystemDriverLocal.ReadAllTextNoBlock(path);
        }

        /// <summary>
        /// Reads all the bytes
        /// </summary>
        /// <param name="path">Path to the file</param>
        public static byte[] ReadAllBytes(string path)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemRead))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            return DriverHandler.CurrentFilesystemDriverLocal.ReadAllBytes(path);
        }

        /// <summary>
        /// Reads all the bytes
        /// </summary>
        /// <param name="path">Path to the file</param>
        public static byte[] ReadAllBytesNoBlock(string path)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemRead))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            return DriverHandler.CurrentFilesystemDriverLocal.ReadAllBytesNoBlock(path);
        }

        /// <summary>
        /// Reads all the characters in the stream until the end and seeks the stream to the beginning, if possible.
        /// </summary>
        /// <param name="stream">The stream reader</param>
        /// <returns>Contents of the stream</returns>
        public static string ReadToEndAndSeek(ref StreamReader stream) =>
            DriverHandler.CurrentFilesystemDriverLocal.ReadToEndAndSeek(ref stream);
    }
}
