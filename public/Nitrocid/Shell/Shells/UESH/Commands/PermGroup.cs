//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using Nitrocid.Users;
using Terminaux.Shell.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages permissions for your group or other groups
    /// </summary>
    /// <remarks>
    /// If you want to manage permissions for your group or other groups, use this command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class PermGroupCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriters.Write(Translate.DoTranslation("You don't have permission to use {0}"), true, KernelColorType.Error, parameters.CommandText);
                return -4;
            }

            PermissionsTools.Demand(PermissionTypes.ManageGroups);
            string target = parameters.ArgumentsList[0];
            string mode = parameters.ArgumentsList[1];
            string perm = parameters.ArgumentsList[2];

            if (!Enum.TryParse(typeof(PermissionTypes), perm, out object? permission))
            {
                // Permission not found
                TextWriters.Write(Translate.DoTranslation("No such permission"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.PermissionManagement);
            }

            if (mode == "allow")
                // Granting permission.
                PermissionsTools.GrantPermissionGroup(target, (PermissionTypes)permission);
            else if (mode == "revoke")
                // Revoking permission.
                PermissionsTools.RevokePermissionGroup(target, (PermissionTypes)permission);
            else
            {
                // No mode
                TextWriters.Write(Translate.DoTranslation("No such permission mode"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.PermissionManagement);
            }
            return 0;
        }

    }
}
