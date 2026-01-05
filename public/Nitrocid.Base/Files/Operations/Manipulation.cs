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

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// Miscellaneous file manipulation routines
    /// </summary>
    public static partial class FilesystemTools
    {
        /// <summary>
        /// Clears the contents of a file
        /// </summary>
        /// <param name="path">Path to an existing file</param>
        public static void ClearFile(string path) =>
            DriverHandler.CurrentFilesystemDriverLocal.ClearFile(path);

        /// <summary>
        /// Wraps the text file contents with 78 columns per line (depending on the current driver).
        /// </summary>
        /// <param name="path">Path to an existing text file</param>
        public static void WrapTextFile(string path) =>
            DriverHandler.CurrentFilesystemDriverLocal.WrapTextFile(path);

        /// <summary>
        /// Wraps the text file contents with 78 columns per line.
        /// </summary>
        /// <param name="path">Path to an existing text file</param>
        /// <param name="columns">How many columns until wrapping begins?</param>
        public static void WrapTextFile(string path, int columns) =>
            DriverHandler.CurrentFilesystemDriverLocal.WrapTextFile(path, columns);

        /// <summary>
        /// Compares between two text files
        /// </summary>
        /// <param name="pathOne">Path to the first file to be compared</param>
        /// <param name="pathTwo">Path to the second file to be compared</param>
        /// <returns>A list of tuples containing an affected line number, a line from the first file, and a line from the second file</returns>
        public static (int line, string one, string two)[] Compare(string pathOne, string pathTwo) =>
            DriverHandler.CurrentFilesystemDriverLocal.Compare(pathOne, pathTwo);

        /// <summary>
        /// Combines the text files and puts the combined output to the array
        /// </summary>
        /// <param name="Input">An input file</param>
        /// <param name="TargetInputs">The target inputs to merge</param>
        public static string[] CombineTextFiles(string Input, string[] TargetInputs) =>
            DriverHandler.CurrentFilesystemDriverLocal.CombineTextFiles(Input, TargetInputs);

        /// <summary>
        /// Combines the binary files and puts the combined output to the array
        /// </summary>
        /// <param name="Input">An input file</param>
        /// <param name="TargetInputs">The target inputs to merge</param>
        public static byte[] CombineBinaryFiles(string Input, string[] TargetInputs) =>
            DriverHandler.CurrentFilesystemDriverLocal.CombineBinaryFiles(Input, TargetInputs);

        /// <summary>
        /// Splits a single file to multiple chunks (identifiable with <c>&lt;FILENAME&gt;.&lt;EXT&gt;.C0000</c>, and secondaries with the suffix of <c>.C&lt;CHUNKNUM&gt;</c>)
        /// </summary>
        /// <param name="inputFile">Input file to split</param>
        /// <param name="chunkSize">Chunk size in bytes</param>
        public static string[] SplitFile(string inputFile, long chunkSize = 104_857_600)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            return SplitFile(inputFile, FilesystemTools.CurrentDir, chunkSize);
        }

        /// <summary>
        /// Splits a single file to multiple chunks (identifiable with <c>&lt;FILENAME&gt;.&lt;EXT&gt;.C0000</c>, and secondaries with the suffix of <c>.C&lt;CHUNKNUM&gt;</c>)
        /// </summary>
        /// <param name="inputFile">Input file to split</param>
        /// <param name="outputDirectory">Output directory</param>
        /// <param name="chunkSize">Chunk size in bytes</param>
        public static string[] SplitFile(string inputFile, string outputDirectory, long chunkSize = 104_857_600)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            return DriverHandler.CurrentFilesystemDriverLocal.SplitFile(inputFile, outputDirectory, chunkSize);
        }

        /// <summary>
        /// Groups chunks to a single file
        /// </summary>
        /// <param name="inputFile">Input file name (automatically searches for chunk files identifiable with <c>&lt;FILENAME&gt;.&lt;EXT&gt;.C0000</c>)</param>
        public static string GroupFile(string inputFile)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            return GroupFile(inputFile, FilesystemTools.CurrentDir);
        }

        /// <summary>
        /// Groups chunks to a single file
        /// </summary>
        /// <param name="inputFile">Input file to group</param>
        /// <param name="outputDirectory">Output directory</param>
        public static string GroupFile(string inputFile, string outputDirectory)
        {
            if (!PrivacyConsentTools.ConsentPermission(ConsentedPermissionType.FilesystemWrite))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NOCONSENT"));
            return DriverHandler.CurrentFilesystemDriverLocal.GroupFile(inputFile, outputDirectory);
        }
    }
}
