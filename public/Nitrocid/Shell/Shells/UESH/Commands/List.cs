
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Instances;
using KS.Files.Print;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can list contents inside the current directory, or specified folder
    /// </summary>
    /// <remarks>
    /// If you don't know what's in the directory, or in the current directory, you can use this command to list folder contents in the colorful way.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-showdetails</term>
    /// <description>Shows the details of the files and folders</description>
    /// </item>
    /// <item>
    /// <term>-suppressmessages</term>
    /// <description>Suppresses the "unauthorized" messages</description>
    /// </item>
    /// <item>
    /// <term>-recursive</term>
    /// <description>Recursively lists files and folders</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// </remarks>
    class ListCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            bool ShowFileDetails = ListSwitchesOnly.Contains("-showdetails") || Listing.ShowFileDetailsList;
            bool SuppressUnauthorizedMessage = ListSwitchesOnly.Contains("-suppressmessages") || Flags.SuppressUnauthorizedMessages;
            bool Recursive = ListSwitchesOnly.Contains("-recursive");
            if (ListArgsOnly.Length == 0)
            {
                Listing.List(CurrentDirectory.CurrentDir, ShowFileDetails, SuppressUnauthorizedMessage, Listing.SortList, Recursive);
            }
            else
            {
                foreach (string Directory in ListArgsOnly)
                {
                    string direct = Filesystem.NeutralizePath(Directory);
                    Listing.List(direct, ShowFileDetails, SuppressUnauthorizedMessage, Listing.SortList, Recursive);
                }
            }
            return 0;
        }

        public override int ExecuteDumb(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            bool ShowFileDetails = ListSwitchesOnly.Contains("-showdetails") || Listing.ShowFileDetailsList;
            bool SuppressUnauthorizedMessage = ListSwitchesOnly.Contains("-suppressmessages") || Flags.SuppressUnauthorizedMessages;
            List<FileSystemEntry> entries;
            if (ListArgsOnly?.Length == 0)
            {
                entries = Listing.CreateList(CurrentDirectory.CurrentDir);
                foreach (var file in entries)
                {
                    try
                    {
                        var info = file.BaseEntry;
                        if (info is FileInfo)
                            FileInfoPrinter.PrintFileInfo(file, ShowFileDetails);
                        else if (info is DirectoryInfo)
                            DirectoryInfoPrinter.PrintDirectoryInfo(file, ShowFileDetails);
                    }
                    catch (UnauthorizedAccessException uaex)
                    {
                        if (!SuppressUnauthorizedMessage)
                            TextWriterColor.Write("- " + Translate.DoTranslation("You are not authorized to get info for {0}."), true, KernelColorType.Error, file);
                        DebugWriter.WriteDebugStackTrace(uaex);
                    }
                }
            }
            else
            {
                foreach (string Directory in ListArgsOnly)
                {
                    string direct = Filesystem.NeutralizePath(Directory);
                    entries = Listing.CreateList(direct);
                    foreach (var file in entries)
                    {
                        try
                        {
                            var info = file.BaseEntry;
                            if (info is FileInfo)
                                FileInfoPrinter.PrintFileInfo(file, ShowFileDetails);
                            else if (info is DirectoryInfo)
                                DirectoryInfoPrinter.PrintDirectoryInfo(file, ShowFileDetails);
                        }
                        catch (UnauthorizedAccessException uaex)
                        {
                            if (!SuppressUnauthorizedMessage)
                                TextWriterColor.Write("- " + Translate.DoTranslation("You are not authorized to get info for {0}."), true, KernelColorType.Error, file);
                            DebugWriter.WriteDebugStackTrace(uaex);
                        }
                    }
                }
            }
            return 0;
        }

    }
}
