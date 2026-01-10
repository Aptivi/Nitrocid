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
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Themes.Colors;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Security.Permissions;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists all connections
    /// </summary>
    /// <remarks>
    /// This command lists all the connections, including open connections.
    /// </remarks>
    class LsConnectionsCommand : BaseCommand, ICommand
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

            var shellTypes = Enum.GetNames<NetworkConnectionType>();
            foreach (var shellType in shellTypes)
            {
                var connections = NetworkConnectionTools.GetNetworkConnections(shellType);
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONNECTIONS_LISTING") + $" {shellType}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                foreach (var connection in connections)
                {
                    TextWriterColor.Write($"- {connection.ConnectionName} -> {connection.ConnectionOriginalUrl}");
                    TextWriterColor.Write($"  {connection.ConnectionUri}");
                    if (!connection.ConnectionIsInstance)
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_TASKMANTUI_KERNELALIVE"), $"{connection.ConnectionAlive}", indent: 1);
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONNECTIONS_INSTANCE"), $"{connection.ConnectionInstance}", indent: 1);
                }
            }
            return 0;
        }

    }
}
