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

using Nitrocid.Network.Types.RPC;
using Terminaux.Shell.Commands;
using System;
using Nitrocid.Security.Permissions;
using Nitrocid.Users;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Remote execution
    /// </summary>
    /// <remarks>
    /// This command can be used to remotely execute KS commands.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class RexecCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, KernelColorType.Error, parameters.CommandText);
                return -4;
            }

            if (parameters.ArgumentsList.Length == 2)
                RPCCommands.SendCommand("<Request:Exec>(" + parameters.ArgumentsList[1] + ")", parameters.ArgumentsList[0]);
            else
                RPCCommands.SendCommand("<Request:Exec>(" + parameters.ArgumentsList[2] + ")", parameters.ArgumentsList[0], Convert.ToInt32(parameters.ArgumentsList[1]));
            return 0;
        }

    }
}
