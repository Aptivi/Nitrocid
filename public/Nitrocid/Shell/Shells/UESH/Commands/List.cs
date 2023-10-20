
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
using System.Linq;
using KS.Files;
using KS.Files.Folders;
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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool ShowFileDetails = parameters.SwitchesList.Contains("-showdetails") || Listing.ShowFileDetailsList;
            bool SuppressUnauthorizedMessage = parameters.SwitchesList.Contains("-suppressmessages") || FilesystemTools.SuppressUnauthorizedMessages;
            bool Recursive = parameters.SwitchesList.Contains("-recursive");
            if (parameters.ArgumentsList.Length == 0)
            {
                Listing.List(CurrentDirectory.CurrentDir, ShowFileDetails, SuppressUnauthorizedMessage, Listing.SortList, Recursive);
            }
            else
            {
                foreach (string Directory in parameters.ArgumentsList)
                {
                    string direct = FilesystemTools.NeutralizePath(Directory);
                    Listing.List(direct, ShowFileDetails, SuppressUnauthorizedMessage, Listing.SortList, Recursive);
                }
            }
            return 0;
        }

    }
}
