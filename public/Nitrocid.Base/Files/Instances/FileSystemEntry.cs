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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files;
using System.Diagnostics;
using System.IO;

namespace Nitrocid.Base.Files.Instances
{
    /// <summary>
    /// Proxy class for <see cref="FileSystemInfo"/>
    /// </summary>
    [DebuggerDisplay("[{Type}] {FilePath}")]
    public class FileSystemEntry
    {
        private readonly string filePath;
        private readonly string filePathUnneutralized;
        private readonly long fileSize;
        private readonly FileSystemEntryType fileType;

        /// <summary>
        /// Does this file or directory entry exist?
        /// </summary>
        public bool Exists =>
            FilesystemTools.Exists(filePath);

        /// <summary>
        /// Gets the file entry type
        /// </summary>
        public FileSystemEntryType Type =>
            fileType;

        /// <summary>
        /// Gets the original file path that was passed to the constructor
        /// </summary>
        public string OriginalFilePath =>
            filePathUnneutralized;

        /// <summary>
        /// Gets the actual file path processed by <see cref="FilesystemTools.NeutralizePath(string, bool)"/>
        /// </summary>
        public string FilePath =>
            filePath;

        /// <summary>
        /// Gets the file size. -1 if the entry is a directory or non-existent
        /// </summary>
        public long FileSize =>
            fileSize;

        /// <summary>
        /// Gets the base entry from the unprocessed file path
        /// </summary>
        public FileSystemInfo BaseEntryUnprocessed =>
            Type == FileSystemEntryType.File ? new FileInfo(OriginalFilePath) :
            Type == FileSystemEntryType.Directory ? new DirectoryInfo(OriginalFilePath) :
            throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_INSTANCES_EXCEPTION_BASEENTRYFAILED") + $" {OriginalFilePath}");

        /// <summary>
        /// Gets the base entry from the processed file path
        /// </summary>
        public FileSystemInfo BaseEntry =>
            Type == FileSystemEntryType.File ? new FileInfo(FilePath) :
            Type == FileSystemEntryType.Directory ? new DirectoryInfo(FilePath) :
            throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_INSTANCES_EXCEPTION_BASEENTRYFAILED") + $" {FilePath}");

        /// <summary>
        /// Makes a new instance of this class
        /// </summary>
        /// <param name="filePath">File path (will be neutralized by <see cref="FilesystemTools.NeutralizePath(string, bool)"/>)</param>
        public FileSystemEntry(string filePath)
        {
            this.filePath = FilesystemTools.NeutralizePath(filePath);
            filePathUnneutralized = filePath;
            fileSize = -1;
            fileType =
                FilesystemTools.FolderExists(filePath) ? FileSystemEntryType.Directory :
                FilesystemTools.FileExists(filePath) ? FileSystemEntryType.File :
                FileSystemEntryType.Nonexistent;
            if (Type == FileSystemEntryType.File)
                fileSize = new FileInfo(FilePath).Length;
        }
    }
}
