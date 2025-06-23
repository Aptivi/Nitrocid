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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using Terminaux.Shell.Commands;
using Nitrocid.Users.Groups;
using Nitrocid.Users;
using Nitrocid.Kernel.Debugging;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.Shell.Shells.UESH.Commands
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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, ThemeColorType.Error, parameters.CommandText);
                return -4;
            }

            PermissionsTools.Demand(PermissionTypes.ManageGroups);
            GroupManagement.RemoveUserFromGroup(parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_RMUSERFROMGROUP_SUCCESS"), parameters.ArgumentsList[0], parameters.ArgumentsList[1]);
            return 0;
        }

    }
}
