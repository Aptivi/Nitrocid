//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using System.IO;
using System.Linq;
using IOPath = System.IO.Path;
using System.Threading;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Drivers;
using Nitrocid.Files.Folders;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Files.Instances;
using Nitrocid.Files.Operations.Querying;
using Textify.General;

namespace Nitrocid.Files
{
    /// <summary>
    /// Filesystem module
    /// </summary>
    public static class FilesystemTools
    {

        private const int maxLockTimeoutMs = 300000;

        /// <summary>
        /// Shows the filesystem progress
        /// </summary>
        public static bool ShowFilesystemProgress =>
            Config.MainConfig.ShowFilesystemProgress;

        /// <summary>
        /// Whether or not to parse whole directory for size
        /// </summary>
        public static bool FullParseMode =>
            Config.MainConfig.FullParseMode;

        /// <summary>
        /// Whether or not to show hidden files
        /// </summary>
        public static bool HiddenFiles =>
            Config.MainConfig.HiddenFiles;

        /// <summary>
        /// Print the line numbers while listing file contents
        /// </summary>
        public static bool PrintLineNumbers =>
            Config.MainConfig.PrintLineNumbers;

        /// <summary>
        /// Whether to suppress the unauthorized messages while listing directory contents
        /// </summary>
        public static bool SuppressUnauthorizedMessages =>
            Config.MainConfig.SuppressUnauthorizedMessages;

        /// <summary>
        /// Simplifies the path to the correct one. It converts the path format to the unified format.
        /// </summary>
        /// <param name="Path">Target path, be it a file or a folder</param>
        /// <param name="Strict">If path is not found, throw exception. Otherwise, neutralize anyway.</param>
        /// <returns>Absolute path</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string NeutralizePath(string Path, bool Strict = false) =>
            NeutralizePath(Path, CurrentDirectory.CurrentDir, Strict);

        /// <summary>
        /// Simplifies the path to the correct one. It converts the path format to the unified format.
        /// </summary>
        /// <param name="Path">Target path, be it a file or a folder</param>
        /// <param name="Source">Source path in which the target is found. Must be a directory</param>
        /// <param name="Strict">If path is not found, throw exception. Otherwise, neutralize anyway.</param>
        /// <returns>Absolute path</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string NeutralizePath(string Path, string Source, bool Strict = false)
        {
            // Warning: There should be no debug statements until the strict check point.
            Path ??= "";
            Source ??= "";
            ThrowOnInvalidPath(Path);
            ThrowOnInvalidPath(Source);

            // Unescape the characters
            Path = DriverHandler.CurrentRegexpDriverLocal.Unescape(Path.Replace(@"\", "/"));
            Source = DriverHandler.CurrentRegexpDriverLocal.Unescape(Source.Replace(@"\", "/"));

            // Append current directory to path
            if (!Checking.Rooted(Path))
                if (!Source.EndsWith("/"))
                    Path = $"{Source}/{Path}";
                else
                    Path = $"{Source}{Path}";

            // Replace last occurrences of current directory of path with nothing.
            if (!string.IsNullOrEmpty(Source))
                if (Path.Contains(Source) & Path.AllIndexesOf(Source).Count() > 1)
                    Path = Path.ReplaceLastOccurrence(Source, "");

            // Finalize the path in case NeutralizePath didn't normalize it correctly.
            Path = IOPath.GetFullPath(Path).Replace(@"\", "/");

            // If strict, checks for existence of file
            if (Strict)
                if (Checking.Exists(Path))
                    return Path;
                else
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Neutralized a non-existent path.") + " {0}", Path);
            else
                return Path;
        }

        /// <summary>
        /// Mitigates Windows 10/11 NTFS corruption/Blue Screen of Death (BSOD) bug
        /// </summary>
        /// <param name="Path">Target path</param>
        /// <remarks>
        /// - When we try to access the secret NTFS bitmap path, which contains <b>$i30</b>, from the partition root path, we'll trigger the "Your disk is corrupt." <br></br>
        /// - When we try to access the <b>kernelconnect</b> secret device from the system partition root path, we'll trigger the BSOD. <br></br><br></br>
        /// This sub will try to prevent access to these paths on unpatched systems and patched systems by throwing <see cref="ArgumentException"/>
        /// </remarks>
        public static void ThrowOnInvalidPath(string Path)
        {
            if (string.IsNullOrEmpty(Path))
                return;
            if (KernelPlatform.IsOnWindows() && (Path.Contains("$i30") || Path.Contains(@"\\.\globalroot\device\condrv\kernelconnect")))
            {
                DebugWriter.WriteDebug(DebugLevel.F, "Trying to access invalid path. Path was {0}", Path);
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Trying to access invalid path. This check was done to prevent older Windows 10 systems that didn't update to the April 2021 patch or higher from accessing these paths known to cause either the NTFS filesystem corruption or the Blue Screen of Death (BSOD) issue. This implies that the caller is attempting to cause a Denial of Service (DoS) and should be fixed, or that the user input is malicious."));
            }
        }

        /// <summary>
        /// Checks to see if the file is locked
        /// </summary>
        /// <param name="Path">Path to check the file</param>
        /// <returns>True if locked; false otherwise.</returns>
        public static bool IsFileLocked(string Path)
        {
            Path = NeutralizePath(Path);

            // We can't perform this operation on nonexistent file
            if (!Checking.FileExists(Path))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File {0} not found."), Path);

            // Try to open the file exclusively to check to see if we can open the file or just error out with sharing violation
            // error.
            try
            {
                // Open the file stream
                using FileStream targetFile = new(Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                targetFile.ReadByte();
                return false;
            }
            catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 32)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "File {0} is locked: {1}", Path, ex.Message);
                return true;
            }
        }

        /// <summary>
        /// Checks to see if the folder is locked
        /// </summary>
        /// <param name="Path">Path to check the folder</param>
        /// <returns>True if locked; false otherwise.</returns>
        public static bool IsFolderLocked(string Path)
        {
            Path = NeutralizePath(Path);

            // We can't perform this operation on nonexistent folder
            if (!Checking.FolderExists(Path))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Directory {0} not found."), Path);

            // Check every file inside the folder and its subdirectories for lock
            var files = Listing.GetFilesystemEntries(Path, false, true);
            foreach (string file in files)
            {
                if (IsLocked(file))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the file or the folder is locked
        /// </summary>
        /// <param name="Path">Path to check the file or the folder</param>
        /// <returns>True if locked; false otherwise.</returns>
        public static bool IsLocked(string Path)
        {
            Path = NeutralizePath(Path);

            // We can't perform this operation on nonexistent file
            if (!Checking.Exists(Path))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File or folder {0} not found."), Path);

            // Wait until the lock is released
            var info = new FileSystemEntry(Path);
            return info.Type == FileSystemEntryType.Directory ? IsFolderLocked(Path) : IsFileLocked(Path);
        }

        /// <summary>
        /// Waits until the file is unlocked (lock released)
        /// </summary>
        /// <param name="Path">Path to check the file</param>
        /// <param name="lockMs">How many milliseconds to wait before querying the lock</param>
        public static void WaitForLockRelease(string Path, int lockMs = 1000)
        {
            Path = NeutralizePath(Path);

            // We can't perform this operation on nonexistent path
            if (!Checking.Exists(Path))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File or folder {0} not found."), Path);

            // We also can't wait for lock too little or too much
            if (lockMs < 100 || lockMs > 60000)
                lockMs = 1000;

            // Wait until the lock is released
            int estimatedLockMs = 0;
            while (IsLocked(Path))
            {
                Thread.Sleep(lockMs);

                // If the file is still locked, add the estimated lock time to check for timeout
                if (IsLocked(Path))
                {
                    estimatedLockMs += lockMs;
                    if (estimatedLockMs > maxLockTimeoutMs)
                        throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File or folder {0} is still locked even after waiting for {1} seconds."), Path, maxLockTimeoutMs / 1000);
                }
            }
        }

        /// <summary>
        /// Waits infinitely until the file is unlocked (lock released)
        /// </summary>
        /// <param name="Path">Path to check the file</param>
        public static void WaitForLockReleaseIndefinite(string Path)
        {
            Path = NeutralizePath(Path);

            // We can't perform this operation on nonexistent file
            if (!Checking.Exists(Path))
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("File or folder {0} not found."), Path);

            // Wait until the lock is released
            var info = new FileSystemEntry(Path);
            SpinWait.SpinUntil(() => !IsLocked(Path));
        }

    }
}
