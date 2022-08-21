
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

using System;
using System.IO;
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Platform;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;

namespace KS.Files.Print
{
    public static class DirectoryInfoPrinter
    {

        /// <summary>
        /// Prints the directory information to the console
        /// </summary>
        public static void PrintDirectoryInfo(FileSystemInfo DirectoryInfo)
        {
            PrintDirectoryInfo(DirectoryInfo, Listing.ShowFileDetailsList);
        }

        /// <summary>
        /// Prints the directory information to the console
        /// </summary>
        public static void PrintDirectoryInfo(FileSystemInfo DirectoryInfo, bool ShowDirectoryDetails)
        {
            if (Checking.FolderExists(DirectoryInfo.FullName))
            {
                // Get all file sizes in a folder
                long TotalSize = SizeGetter.GetAllSizesInFolder((DirectoryInfo)DirectoryInfo);

                // Print information
                if (DirectoryInfo.Attributes == FileAttributes.Hidden & Flags.HiddenFiles | !DirectoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    if (KernelPlatform.IsOnWindows() & (!DirectoryInfo.Name.StartsWith(".") | DirectoryInfo.Name.StartsWith(".") & Flags.HiddenFiles) | KernelPlatform.IsOnUnix())
                    {
                        TextWriterColor.Write("- " + DirectoryInfo.Name + "/", false, ColorTools.ColTypes.ListEntry);
                        if (ShowDirectoryDetails)
                        {
                            TextWriterColor.Write(": ", false, ColorTools.ColTypes.ListEntry);
                            TextWriterColor.Write(Translate.DoTranslation("{0}, Created in {1} {2}, Modified in {3} {4}"), false, ColorTools.ColTypes.ListValue, TotalSize.FileSizeToString(), DirectoryInfo.CreationTime.ToShortDateString(), DirectoryInfo.CreationTime.ToShortTimeString(), DirectoryInfo.LastWriteTime.ToShortDateString(), DirectoryInfo.LastWriteTime.ToShortTimeString());
                        }
                        Console.WriteLine();
                    }
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Directory {0} not found"), true, ColorTools.ColTypes.Error, DirectoryInfo.FullName);
                DebugWriter.Wdbg(DebugLevel.I, "IO.FolderExists = {0}", Checking.FolderExists(DirectoryInfo.FullName));
            }
        }

    }
}