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
using System.IO;
using System.Linq;
using IOPath = System.IO.Path;
using System.Threading;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Files.Instances;
using Textify.General;
using Nitrocid.Misc.Text.Probers.Regexp;
using Nitrocid.Misc.Interactives;
using Terminaux.Inputs.Interactive;
using Nitrocid.Files.Paths;

namespace Nitrocid.Files
{
    /// <summary>
    /// Filesystem module
    /// </summary>
    public static partial class FilesystemTools
    {

        private const int maxLockTimeoutMs = 300000;

        /// <summary>
        /// Simplifies the path to the correct one. It converts the path format to the unified format.
        /// </summary>
        /// <param name="Path">Target path, be it a file or a folder</param>
        /// <param name="Strict">If path is not found, throw exception. Otherwise, neutralize anyway.</param>
        /// <returns>Absolute path</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string NeutralizePath(string? Path, bool Strict = false) =>
            NeutralizePath(Path, FilesystemTools.CurrentDir, Strict);

        /// <summary>
        /// Simplifies the path to the correct one. It converts the path format to the unified format.
        /// </summary>
        /// <param name="Path">Target path, be it a file or a folder</param>
        /// <param name="Source">Source path in which the target is found. Must be a directory</param>
        /// <param name="Strict">If path is not found, throw exception. Otherwise, neutralize anyway.</param>
        /// <returns>Absolute path</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static string NeutralizePath(string? Path, string? Source, bool Strict = false)
        {
            // Warning: There should be no debug statements until the strict check point.
            Path ??= "";
            Source ??= "";

            // Unescape the characters
            Path = RegexpTools.Unescape(Path.Replace(@"\", "/"));
            Source = RegexpTools.Unescape(Source.Replace(@"\", "/"));

            // Append current directory to path
            if (!FilesystemTools.Rooted(Path))
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
                if (FilesystemTools.Exists(Path))
                    return Path;
                else
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_NEUTRALIZENONEXISTENTPATH") + " {0}", Path);
            else
                return Path;
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
            if (!FilesystemTools.FileExists(Path))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILENOTFOUND2"), Path);

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
                DebugWriter.WriteDebug(DebugLevel.W, "File {0} is locked: {1}", vars: [Path, ex.Message]);
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
            if (!FilesystemTools.FolderExists(Path))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_DIRECTORYNOTFOUND1"), Path);

            // Check every file inside the folder and its subdirectories for lock
            var files = FilesystemTools.GetFilesystemEntries(Path, false, true);
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
            if (!FilesystemTools.Exists(Path))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILEORFOLDERNOTFOUND"), Path);

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
            if (!FilesystemTools.Exists(Path))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILEORFOLDERNOTFOUND"), Path);

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
                        throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_LOCKTIMEOUT"), Path, maxLockTimeoutMs / 1000);
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
            if (!FilesystemTools.Exists(Path))
                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_FILEORFOLDERNOTFOUND"), Path);

            // Wait until the lock is released
            var info = new FileSystemEntry(Path);
            SpinWait.SpinUntil(() => !IsLocked(Path));
        }

        internal static void OpenFileManagerTui() =>
            OpenFileManagerTui(PathsManagement.HomePath, PathsManagement.HomePath);

        internal static void OpenFileManagerTui(string firstPanePath, string secondPanePath)
        {
            var tui = new FileManagerCli
            {
                firstPanePath = FolderExists(firstPanePath) ? firstPanePath : PathsManagement.HomePath,
                secondPanePath = FolderExists(secondPanePath) ? secondPanePath : PathsManagement.HomePath,
            };
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_OPEN"), ConsoleKey.Enter, (entry1, _, entry2, _) => tui.Open(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_COPY"), ConsoleKey.F1, (entry1, _, entry2, _) => tui.CopyFileOrDir(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_MOVE"), ConsoleKey.F2, (entry1, _, entry2, _) => tui.MoveFileOrDir(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_DELETE"), ConsoleKey.F3, (entry1, _, entry2, _) => tui.RemoveFileOrDir(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_UP"), ConsoleKey.F4, (_, _, _, _) => tui.GoUp(), true));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_INFO"), ConsoleKey.F5, (entry1, _, entry2, _) => tui.PrintFileSystemEntry(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_GOTO"), ConsoleKey.F6, (_, _, _, _) => tui.GoTo(), true));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_COPYTO"), ConsoleKey.F1, ConsoleModifiers.Shift, (entry1, _, entry2, _) => tui.CopyTo(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_MOVETO"), ConsoleKey.F2, ConsoleModifiers.Shift, (entry1, _, entry2, _) => tui.MoveTo(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_RENAME"), ConsoleKey.F9, (entry1, _, entry2, _) => tui.Rename(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_NEWFOLDER"), ConsoleKey.F10, (_, _, _, _) => tui.MakeDir(), true));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_HASHTO"), ConsoleKey.F11, (entry1, _, entry2, _) => tui.Hash(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_VERIFYTO"), ConsoleKey.F12, (entry1, _, entry2, _) => tui.Verify(entry1, entry2)));
            tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_PREVIEW"), ConsoleKey.P, (entry1, _, entry2, _) => tui.Preview(entry1, entry2)));
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
