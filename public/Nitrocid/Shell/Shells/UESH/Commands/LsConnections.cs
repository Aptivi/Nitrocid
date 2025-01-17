﻿//
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
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Network.Connections;
using Terminaux.Writer.CyclicWriters;
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
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Connections for type") + $" {shellType}", KernelColorTools.GetColor(KernelColorType.ListTitle));
                foreach (var connection in connections)
                {
                    TextWriterColor.Write($"- {connection.ConnectionName} -> {connection.ConnectionOriginalUrl}");
                    TextWriterColor.Write($"  {connection.ConnectionUri}");
                    if (!connection.ConnectionIsInstance)
                    {
                        var connectionAlive = new ListEntry()
                        {
                            Entry = Translate.DoTranslation("Alive"),
                            Value = $"{connection.ConnectionAlive}",
                            KeyColor = KernelColorTools.GetColor(KernelColorType.ListEntry),
                            ValueColor = KernelColorTools.GetColor(KernelColorType.ListValue),
                        };
                        TextWriterRaw.WritePlain(connectionAlive.Render());
                    }
                    var connectionInstance = new ListEntry()
                    {
                        Entry = Translate.DoTranslation("Instance"),
                        Value = $"{connection.ConnectionInstance}",
                        KeyColor = KernelColorTools.GetColor(KernelColorType.ListEntry),
                        ValueColor = KernelColorTools.GetColor(KernelColorType.ListValue),
                    };
                    TextWriterRaw.WritePlain(connectionInstance.Render());
                }
            }
            return 0;
        }

    }
}
