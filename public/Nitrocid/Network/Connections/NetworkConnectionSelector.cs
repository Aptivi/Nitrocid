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

using Terminaux.Inputs.Styles.Selection;
using Nitrocid.Languages;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Inputs.Styles;

namespace Nitrocid.Network.Connections
{
    internal static class NetworkConnectionSelector
    {
        internal static int ConnectionSelector(NetworkConnectionType connectionType) =>
            ConnectionSelector(connectionType.ToString());

        internal static int ConnectionSelector(string connectionType)
        {
            var connections = NetworkConnectionTools.GetNetworkConnections(connectionType);
            var connectionNames = connections.Select((connection) => connection.ConnectionOriginalUrl.ToString()).ToArray();

            // We need to prompt the user to select a connection or to establish a new connection so that the new shell can
            // attach to the selected connection
            var connectionsChoiceList = new List<InputChoiceInfo>();
            for (int i = 0; i < connectionNames.Length; i++)
            {
                string connectionUrl = connectionNames[i];
                connectionsChoiceList.Add(new InputChoiceInfo($"{i + 1}", connectionUrl));
            }

            return SelectionStyle.PromptSelection(Translate.DoTranslation("Select a connection. If you have no connections, you'll have to create a new connection. Additionally, you can use the speed dial feature to quickly connect to servers."),
                [.. connectionsChoiceList], [
                    new InputChoiceInfo($"{connectionNames.Length + 1}", Translate.DoTranslation("Create a new connection")),
                    new InputChoiceInfo($"{connectionNames.Length + 2}", Translate.DoTranslation("Use speed dial")),
                ]
            );
        }
    }
}
