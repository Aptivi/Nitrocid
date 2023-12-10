//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Languages;
using KS.Network.Base.Connections;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.UESH.Commands
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
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Connections for type") + $" {shellType}", true);
                foreach (var connection in connections)
                {
                    TextWriterColor.Write($"- {connection.ConnectionName} -> {connection.ConnectionOriginalUrl}");
                    TextWriterColor.Write($"  {connection.ConnectionUri}");
                    if (!connection.ConnectionIsInstance)
                        ListEntryWriterColor.WriteListEntry(Translate.DoTranslation("Alive"), $"{connection.ConnectionAlive}", 1);
                    ListEntryWriterColor.WriteListEntry(Translate.DoTranslation("Instance"), $"{connection.ConnectionInstance}", 1);
                }
            }
            return 0;
        }

    }
}
