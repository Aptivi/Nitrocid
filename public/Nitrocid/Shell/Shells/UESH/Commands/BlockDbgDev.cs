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
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Nitrocid.Security.Permissions;
using Nitrocid.Users;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can block an IP address from entering the remote debugger.
    /// </summary>
    /// <remarks>
    /// If you wanted to moderate the remote debugger and block a device from joining it because it either causes problems or kept flooding the chat, you may use this command to block such offenders.
    /// <br></br>
    /// This command is available to administrators only. The blocked device can be unblocked using the unblockdbgdev command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class BlockDbgDevCommand : BaseCommand, ICommand
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

            string address = parameters.ArgumentsList[0];
            var device = RemoteDebugTools.GetDeviceFromIp(address);
            if (!device.Blocked)
            {
                if (RemoteDebugTools.TryAddToBlockList(address))
                {
                    TextWriterColor.Write(Translate.DoTranslation("{0} can't join remote debug now."), address);
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Failed to block {0}."), address);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.RemoteDebugDeviceOperation);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("{0} is already blocked."), address);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.RemoteDebugDeviceOperation);
            }
        }

    }
}
