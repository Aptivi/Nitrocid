//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Files;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;

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
        public override string Command => 
            "list";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_LIST_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "directory", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_ARGUMENT_DIRECTORY_DESC"
                    }),
                ],
                [
                    new SwitchInfo("showdetails", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_SHOWDETAILS_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("suppressmessages", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_SUPPRESSMESSAGES_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("recursive", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_RECURSIVE_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false,
                        ConflictsWith = ["tree"]
                    }),
                    new SwitchInfo("tree", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_LIST_SWITCH_TREE_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false,
                        ConflictsWith = ["recursive"]
                    })
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool showFileDetails = parameters.ContainsSwitch("-showdetails") || Config.MainConfig.ShowFileDetailsList;
            bool suppressUnauthorizedMessage = parameters.ContainsSwitch("-suppressmessages") || Config.MainConfig.SuppressUnauthorizedMessages;
            bool recursive = parameters.ContainsSwitch("-recursive");
            bool tree = parameters.ContainsSwitch("-tree");
            string[] directories = parameters.ArgumentsList.Length > 0 ? parameters.ArgumentsList : [FilesystemTools.CurrentDir];
            foreach (string Directory in directories)
            {
                string direct = FilesystemTools.NeutralizePath(Directory);
                if (tree)
                    FilesystemTools.ListTree(direct, suppressUnauthorizedMessage, Config.MainConfig.SortList);
                else
                    FilesystemTools.List(direct, showFileDetails, suppressUnauthorizedMessage, Config.MainConfig.SortList, recursive);
            }
            return 0;
        }

    }
}
