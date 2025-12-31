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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Kernel.Debugging.RemoteDebug;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Security.Permissions;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lets you unblock a blocked debug device
    /// </summary>
    /// <remarks>
    /// If you wanted to let a device whose IP address is blocked join the remote debugging again, you can unblock it using this command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class UnblockDbgDevCommand : BaseCommand, ICommand
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

            string address = parameters.ArgumentsList[0];
            var device = RemoteDebugTools.GetDeviceFromIp(address);
            if (device.Blocked)
            {
                if (RemoteDebugTools.TryRemoveFromBlockList(address))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_UNBLOCKDBGDEV_SUCCESS"), address);
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_UNBLOCKDBGDEV_UNBLOCKFAILED"), address);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Debug);
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_UNBLOCKDBGDEV_NOTBLOCKED"), address);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Debug);
            }
        }

    }
}
