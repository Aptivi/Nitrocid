
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
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Base.Connections;
using KS.Network.Mail;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using System.Collections.Generic;
using System;
using System.Linq;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens the mail shell
    /// </summary>
    /// <remarks>
    /// This command is an entry point to the mail shell that lets you view and list messages.
    /// <br></br>
    /// If no address is specified, it will prompt you for the address, password, and the mail server (IMAP) if the address is not found in the ISP database. Currently, it connects with necessary requirements to ensure successful connection.
    /// </remarks>
    class MailCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                if (ListArgsOnly.Length == 0)
                {
                    // Select a connection according to user input
                    int selectedConnection = NetworkConnectionSelector.ConnectionSelector(NetworkConnectionType.Mail);
                    var availableConnectionInstances = NetworkConnectionTools.GetNetworkConnections(NetworkConnectionType.Mail);
                    int availableConnections = NetworkConnectionTools.GetNetworkConnections(NetworkConnectionType.Mail).Length;

                    // Now, check to see if the user selected "Create a new connection"
                    NetworkConnection connectionImap;
                    NetworkConnection connectionSmtp;
                    if (selectedConnection == availableConnections + 1)
                    {
                        // Prompt the user to provide connection information
                        var connections = MailLogin.PromptUser();
                        connectionImap = connections.Item1;
                        connectionSmtp = connections.Item2;
                    }
                    else
                    {
                        // User selected connection
                        connectionImap = availableConnectionInstances[selectedConnection - 1];
                        connectionSmtp = availableConnectionInstances[selectedConnection];
                    }

                    // Use that information to start the shell
                    ShellStart.StartShell(ShellType.MailShell, connectionImap, connectionSmtp);
                }
                else
                {
                    // Check to see if the provided address has an already existing connection
                    string address = ListArgsOnly[0];
                    var availableConnectionInstances = NetworkConnectionTools.GetNetworkConnections(NetworkConnectionType.Mail).Where((connection) => connection.ConnectionUri.OriginalString == address).ToArray();
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
                        NetworkConnection connectionImap = availableConnectionInstances[selectedConnectionNumber - 1];
                        NetworkConnection connectionSmtp = availableConnectionInstances[selectedConnectionNumber];
                        ShellStart.StartShell(ShellType.MailShell, connectionImap, connectionSmtp);
                    }
                    else
                    {
                        var connections = MailLogin.PromptUser();
                        NetworkConnection connectionImap = connections.Item1;
                        NetworkConnection connectionSmtp = connections.Item2;
                        ShellStart.StartShell(ShellType.MailShell, connectionImap, connectionSmtp);
                    }
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
