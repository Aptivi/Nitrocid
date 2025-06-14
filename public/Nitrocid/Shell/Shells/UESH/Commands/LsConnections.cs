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

using System;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Languages;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Network.Connections;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Shell.Shells.UESH.Commands
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
            var shellTypes = Enum.GetNames<NetworkConnectionType>();
            foreach (var shellType in shellTypes)
            {
                var connections = NetworkConnectionTools.GetNetworkConnections(shellType);
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_LSCONNECTIONS_LISTING") + $" {shellType}", KernelColorTools.GetColor(KernelColorType.ListTitle));
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
