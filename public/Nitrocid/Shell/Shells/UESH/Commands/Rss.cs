
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Base.Connections;
using KS.Network.RSS;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using System.Collections.Generic;
using System;
using System.Linq;
using KS.Network.RSS.Instance;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens an RSS shell
    /// </summary>
    /// <remarks>
    /// You can interact with the RSS shell to connect to a feed server and interact with them.
    /// </remarks>
    class RssCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                if (ListArgsOnly.Length == 0)
                {
                    // Select a connection according to user input
                    int selectedConnection = NetworkConnectionSelector.ConnectionSelector(NetworkConnectionType.RSS);
                    var availableConnectionInstances = NetworkConnectionTools.GetNetworkConnections(NetworkConnectionType.RSS);
                    int availableConnections = NetworkConnectionTools.GetNetworkConnections(NetworkConnectionType.RSS).Length;

                    // Now, check to see if the user selected "Create a new connection"
                    NetworkConnection connection;
                    if (selectedConnection == availableConnections + 1)
                    {
                        // Prompt the user to provide connection information
                        string address = Input.ReadLine(Translate.DoTranslation("Enter the server address:") + " ");
                        connection = NetworkConnectionTools.EstablishConnection("RSS connection", address, NetworkConnectionType.RSS, new RSSFeed(address, RSSFeedType.Infer));
                    }
                    else
                    {
                        // User selected connection
                        connection = availableConnectionInstances[selectedConnection - 1];
                    }

                    // Use that information to start the shell
                    ShellStart.StartShell(ShellType.RSSShell, connection);
                }
                else
                {
                    // Check to see if the provided address has an already existing connection
                    string address = ListArgsOnly[0];
                    var availableConnectionInstances = NetworkConnectionTools.GetNetworkConnections(NetworkConnectionType.RSS).Where((connection) => connection.ConnectionUri.OriginalString == address).ToArray();
                    if (availableConnectionInstances.Any())
                    {
                        var connectionNames = availableConnectionInstances.Select((connection) => connection.ConnectionUri.ToString()).ToArray();
                        var connectionsChoiceList = new List<InputChoiceInfo>();
                        for (int i = 0; i < connectionNames.Length; i++)
                        {
                            string connectionUrl = connectionNames[i];
                            connectionsChoiceList.Add(new InputChoiceInfo($"{i + 1}", connectionUrl));
                        }

                        // Get connection from user selection
                        int selectedConnectionNumber = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a connection."), connectionsChoiceList);
                        ShellStart.StartShell(ShellType.RSSShell, availableConnectionInstances[selectedConnectionNumber - 1]);
                    }
                    else
                        ShellStart.StartShell(ShellType.RSSShell, NetworkConnectionTools.EstablishConnection("RSS connection", address, NetworkConnectionType.RSS, new RSSFeed(address, RSSFeedType.Infer)));
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Unknown shell error:") + " {0}", true, KernelColorType.Error, ex.Message);
            }
        }

    }
}
