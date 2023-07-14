
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

using System;
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Base.Connections;
using KS.Network.FTP;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can interact with the File Transfer Protocol (FTP) shell to connect to a server and transfer files
    /// </summary>
    /// <remarks>
    /// You can use the FTP shell to connect to your FTP server or the public FTP servers to interact with the files found in the server.
    /// <br></br>
    /// You can download files to your computer, upload files to the server, manage files by renaming, deleting, etc., and so on.
    /// </remarks>
    class FtpCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                if (ListArgsOnly.Length == 0)
                {
                    // Select a connection according to user input
                    int selectedConnection = NetworkConnectionSelector.ConnectionSelector(NetworkConnectionType.FTP);
                    var availableConnectionInstances = NetworkConnectionTools.GetNetworkConnections(NetworkConnectionType.FTP);
                    int availableConnections = NetworkConnectionTools.GetNetworkConnections(NetworkConnectionType.FTP).Length;

                    // Now, check to see if the user selected "Create a new connection"
                    NetworkConnection connection;
                    if (selectedConnection == availableConnections + 1)
                    {
                        // Prompt the user to provide connection information
                        string address = Input.ReadLine(Translate.DoTranslation("Enter the server address:") + " ");
                        connection = FTPTools.TryToConnect(address);
                    }
                    else
                    {
                        // User selected connection
                        connection = availableConnectionInstances[selectedConnection - 1];
                    }

                    // Use that information to start the shell
                    ShellStart.StartShell(ShellType.FTPShell, connection);
                }
                else
                {
                    // Check to see if the provided address has an already existing connection
                    string address = ListArgsOnly[0];
                    int indexOfPort = address.LastIndexOf(":");
                    address = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "");
                    address = indexOfPort < 0 ? address : address.Replace(address[address.LastIndexOf(":")..], "");
                    string port = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "").Replace(address + ":", "");
                    var availableConnectionInstances = NetworkConnectionTools.GetNetworkConnections(NetworkConnectionType.FTP).Where((connection) => connection.ConnectionUri.Host == address || $"{connection.ConnectionUri.Host}:{connection.ConnectionUri.Port}" == $"{address}:{port}").ToArray();
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
                        ShellStart.StartShell(ShellType.FTPShell, availableConnectionInstances[selectedConnectionNumber - 1]);
                    }
                    else
                        ShellStart.StartShell(ShellType.FTPShell, FTPTools.TryToConnect(address));
                }
            }
            catch (KernelException ftpex) when (ftpex.ExceptionType == KernelExceptionType.FTPShell)
            {
                TextWriterColor.Write(ftpex.Message, true, KernelColorType.Error);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Unknown FTP shell error:") + " {0}", true, KernelColorType.Error, ex.Message);
            }
        }

    }
}
