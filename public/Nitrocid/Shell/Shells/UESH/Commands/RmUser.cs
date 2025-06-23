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
using Terminaux.Shell.Commands;
using Nitrocid.Users;
using Nitrocid.Security.Permissions;
using Nitrocid.Kernel.Debugging;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Removes uninvited or redundant user
    /// </summary>
    /// <remarks>
    /// This command lets you remove the uninvited or redundant user from the user dictionary that is initialized at the start of the kernel. It also removes password from the removed user if it has one.
    /// <br></br>
    /// However you can't remove your own user that is signed in.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class RmUserCommand : BaseCommand, ICommand
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

            UserManagement.RemoveUser(parameters.ArgumentsList[0]);
            if (!UserManagement.UserExists(parameters.ArgumentsList[0]))
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_RMUSER_REMOVED"), parameters.ArgumentsList[0]);
            return 0;
        }

    }
}
