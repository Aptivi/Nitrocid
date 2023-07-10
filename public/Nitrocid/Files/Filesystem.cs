
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Linq;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Languages;
using KS.Kernel.Debugging;
using KS.Kernel;
using KS.Kernel.Exceptions;
using KS.Kernel.Configuration;
using KS.Drivers;
using KS.Misc.Text;
using IOPath = System.IO.Path;

namespace KS.Files
{
    /// <summary>
    /// Filesystem module
    /// </summary>
    public static class Filesystem
    {

        /// <summary>
        /// Shows the filesystem progress
        /// </summary>
        public static bool ShowFilesystemProgress =>
            Config.MainConfig.ShowFilesystemProgress;

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
            Path ??= "";
            Source ??= "";
            ThrowOnInvalidPath(Path);
            ThrowOnInvalidPath(Source);

            // Unescape the characters
            Path = DriverHandler.CurrentRegexpDriver.Unescape(Path);
            Source = DriverHandler.CurrentRegexpDriver.Unescape(Source);

            // Replace backslashes with slashes if any.
            Path = Path.Replace(@"\", "/");
            Source = Source.Replace(@"\", "/");

            // Append current directory to path
            if (KernelPlatform.IsOnWindows() & !Path.Contains(":/") | KernelPlatform.IsOnUnix() & !Path.StartsWith("/"))
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
                if (Checking.FileExists(Path) | Checking.FolderExists(Path))
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
            if (KernelPlatform.IsOnWindows() & (Path.Contains("$i30") | Path.Contains(@"\\.\globalroot\device\condrv\kernelconnect")))
            {
                DebugWriter.WriteDebug(DebugLevel.F, "Trying to access invalid path. Path was {0}", Path);
                throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Trying to access invalid path."), nameof(Path));
            }
        }

    }
}
