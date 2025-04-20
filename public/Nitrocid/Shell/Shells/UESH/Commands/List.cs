﻿//
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

using Nitrocid.Files;
using Nitrocid.Files.Folders;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;

namespace Nitrocid.Shell.Shells.UESH.Commands
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
            bool ShowFileDetails = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-showdetails") || Config.MainConfig.ShowFileDetailsList;
            bool SuppressUnauthorizedMessage = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-suppressmessages") || Config.MainConfig.SuppressUnauthorizedMessages;
            bool Recursive = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-recursive");
            bool tree = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-tree");
            if (parameters.ArgumentsList.Length == 0)
            {
                if (tree)
                    Listing.ListTree(CurrentDirectory.CurrentDir, SuppressUnauthorizedMessage, Config.MainConfig.SortList);
                else
                    Listing.List(CurrentDirectory.CurrentDir, ShowFileDetails, SuppressUnauthorizedMessage, Config.MainConfig.SortList, Recursive);
            }
            else
            {
                foreach (string Directory in parameters.ArgumentsList)
                {
                    string direct = FilesystemTools.NeutralizePath(Directory);
                    if (tree)
                        Listing.ListTree(direct, SuppressUnauthorizedMessage, Config.MainConfig.SortList);
                    else
                        Listing.List(direct, ShowFileDetails, SuppressUnauthorizedMessage, Config.MainConfig.SortList, Recursive);
                }
            }
            return 0;
        }

    }
}
