
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using KS.Kernel;
using KS.Misc.Platform;
using KS.Misc.Writers.DebugWriters;

namespace KS.Files.Querying
{
    public static class SizeGetter
    {

        /// <summary>
        /// Gets all file sizes in a folder, depending on the kernel setting <see cref="FullParseMode"/>
        /// </summary>
        /// <param name="DirectoryInfo">Directory information</param>
        /// <returns>Directory Size</returns>
        public static long GetAllSizesInFolder(DirectoryInfo DirectoryInfo)
        {
            return GetAllSizesInFolder(DirectoryInfo, Flags.FullParseMode);
        }

        /// <summary>
        /// Gets all file sizes in a folder, and optionally parses the entire folder
        /// </summary>
        /// <param name="DirectoryInfo">Directory information</param>
        /// <returns>Directory Size</returns>
        public static long GetAllSizesInFolder(DirectoryInfo DirectoryInfo, bool FullParseMode)
        {
            List<FileInfo> Files;
            if (FullParseMode)
            {
                Files = DirectoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
            }
            else
            {
                Files = DirectoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly).ToList();
            }
            DebugWriter.Wdbg(DebugLevel.I, "{0} files to be parsed", Files.Count);
            long TotalSize = 0L; // In bytes
            foreach (FileInfo DFile in Files)
            {
                if (DFile.Attributes == FileAttributes.Hidden & Flags.HiddenFiles | !DFile.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    if (KernelPlatform.IsOnWindows() & (!DFile.Name.StartsWith(".") | DFile.Name.StartsWith(".") & Flags.HiddenFiles) | KernelPlatform.IsOnUnix())
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "File {0}, Size {1} bytes", DFile.Name, DFile.Length);
                        TotalSize += DFile.Length;
                    }
                }
            }
            return TotalSize;
        }

    }
}