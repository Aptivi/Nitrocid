
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

using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Users.Groups;
using KS.Users.Permissions;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Removes uninvited or redundant user from the group
    /// </summary>
    /// <remarks>
    /// This command lets you remove an uninvited or redundant user from the group.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class RmUserFromGroupCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            PermissionsTools.Demand(PermissionTypes.ManageGroups);
            GroupManagement.RemoveUserFromGroup(ListArgsOnly[0], ListArgsOnly[1]);
            TextWriterColor.Write(Translate.DoTranslation("Removed {0} from group {1}."), ListArgsOnly[0], ListArgsOnly[1]);
        }

    }
}
