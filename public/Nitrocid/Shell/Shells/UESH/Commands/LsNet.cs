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

using Terminaux.Colors.Themes.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Network;
using Terminaux.Shell.Commands;
using System;
using System.Net;
using Nitrocid.Security.Permissions;
using Nitrocid.Users;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists network devices
    /// </summary>
    /// <remarks>
    /// This command lists all the network devices by their IP addresses and, if possible, their names.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class LsNetCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, ThemeColorType.Error, parameters.CommandText);
                return -4;
            }

            foreach (IPAddress Device in NetworkTools.GetOnlineDevicesInNetwork())
            {
                string address = string.Join(".", Device.GetAddressBytes());
                string host = LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSNET_UNKNOWNHOST");

                // Try to get host name from device
                try
                {
                    host = Dns.GetHostEntry(address).HostName;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get host name from target device {0}: {1}", vars: [address, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                }

                // Print info
                TextWriters.Write($"- {address}: ", false, ThemeColorType.ListEntry);
                TextWriters.Write(host, true, ThemeColorType.ListValue);
            }
            return 0;
        }

    }
}
