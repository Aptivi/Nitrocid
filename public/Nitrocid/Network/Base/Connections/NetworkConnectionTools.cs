
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
using KS.Shell;
using KS.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using System.Linq;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Network.SpeedDial;

namespace KS.Network.Base.Connections
{
    /// <summary>
    /// Network connection tools to manipulate with connections
    /// </summary>
    public static class NetworkConnectionTools
    {
        private static readonly List<NetworkConnection> networkConnections = new();

        /// <summary>
        /// Gets the network connections according to the given type
        /// </summary>
        /// <param name="connectionType">Type of connection</param>
        /// <returns>Array of <see cref="NetworkConnection"/>s that is of the same type specified</returns>
        public static NetworkConnection[] GetNetworkConnections(NetworkConnectionType connectionType) =>
            networkConnections.Where((connection) => connection.ConnectionType == connectionType).ToArray();

        /// <summary>
        /// Closes all the connections
        /// </summary>
        public static void CloseAllConnections()
        {
            for (int connection = networkConnections.Count - 1; connection >= 0; connection--)
                CloseConnection(connection);
        }

        /// <summary>
        /// Closes the selected connection
        /// </summary>
        /// <param name="connectionIndex">Index of connection to close</param>
        /// <exception cref="KernelException"></exception>
        public static void CloseConnection(int connectionIndex)
        {
            // Check to see if we have this connection
            if (connectionIndex >= networkConnections.Count)
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("Connection is not found."));

            // Now, try to close this connection
            DebugWriter.WriteDebug(DebugLevel.I, "Closing connection {0}...", connectionIndex);
            if (!networkConnections[connectionIndex].ConnectionIsInstance)
                networkConnections[connectionIndex].ConnectionThread.Stop();
            networkConnections.RemoveAt(connectionIndex);
            DebugWriter.WriteDebug(DebugLevel.I, "Connection {0} closed...", connectionIndex);
        }

        /// <summary>
        /// Establishes the connection to the given URL
        /// </summary>
        /// <param name="name">Connection name</param>
        /// <param name="url">Connection URL</param>
        /// <param name="connectionType">Connection type</param>
        /// <param name="connectionThread">Thread which holds a loop for connection (usually send/receive)</param>
        /// <returns>An instance of NetworkConnection</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection EstablishConnection(string name, string url, NetworkConnectionType connectionType, KernelThread connectionThread)
        {
            // First, parse the URL
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri))
                return EstablishConnection(name, uri, connectionType, connectionThread);
            else
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("Invalid link for connection."));
        }

        /// <summary>
        /// Establishes the connection to the given URI
        /// </summary>
        /// <param name="name">Connection name</param>
        /// <param name="uri">Connection URI</param>
        /// <param name="connectionType">Connection type</param>
        /// <param name="connectionThread">Thread which holds a loop for connection (usually send/receive)</param>
        /// <returns>An instance of NetworkConnection</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection EstablishConnection(string name, Uri uri, NetworkConnectionType connectionType, KernelThread connectionThread)
        {
            // Now, make a connection and start the connection thread
            try
            {
                NetworkConnection connection = new(name, uri, connectionType, connectionThread, null, uri.OriginalString);
                connection.ConnectionThread.Start();
                networkConnections.Add(connection);
                DebugWriter.WriteDebug(DebugLevel.I, "Added connection {0} for URI {1} to {2} list with thread name {3}", name, uri.ToString(), connectionType.ToString(), connectionThread.Name);
                return connection;
            }
            catch (Exception e)
            {
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("Failed to establish connection with {0}.") + " {1}", e, uri.ToString(), e.Message);
            }
        }

        /// <summary>
        /// Establishes the connection to the given URL
        /// </summary>
        /// <param name="name">Connection name</param>
        /// <param name="url">Connection URL</param>
        /// <param name="connectionType">Connection type</param>
        /// <param name="connectionInstance">Instance which holds this connection (if threads are unsuitable)</param>
        /// <returns>An instance of NetworkConnection</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection EstablishConnection(string name, string url, NetworkConnectionType connectionType, object connectionInstance)
        {
            // First, parse the URL
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri))
                return EstablishConnection(name, uri, connectionType, connectionInstance);
            else
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("Invalid link for connection."));
        }

        /// <summary>
        /// Establishes the connection to the given URI
        /// </summary>
        /// <param name="name">Connection name</param>
        /// <param name="uri">Connection URI</param>
        /// <param name="connectionType">Connection type</param>
        /// <param name="connectionInstance">Instance which holds this connection (if threads are unsuitable)</param>
        /// <returns>An instance of NetworkConnection</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection EstablishConnection(string name, Uri uri, NetworkConnectionType connectionType, object connectionInstance)
        {
            // Now, make a connection and start the connection thread
            try
            {
                NetworkConnection connection = new(name, uri, connectionType, null, connectionInstance, uri.OriginalString);

                // Just return the connection. This instance is an object and could be anything that represents a network connection.
                networkConnections.Add(connection);
                DebugWriter.WriteDebug(DebugLevel.I, "Added connection {0} for URI {1} to {2} list with instance type {3}", name, uri.ToString(), connectionType.ToString(), connectionInstance.GetType().Name);
                return connection;
            }
            catch (Exception e)
            {
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("Failed to establish connection with {0}.") + " {1}", e, uri.ToString(), e.Message);
            }
        }

        /// <summary>
        /// Gets the connection index
        /// </summary>
        /// <param name="connection">Network connection to get its index from</param>
        /// <returns>Network connection index starting from zero (0)</returns>
        /// <exception cref="KernelException"></exception>
        public static int GetConnectionIndex(NetworkConnection connection)
        {
            // Check to see if we have this connection
            if (!networkConnections.Contains(connection))
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("Connection is not established yet."));
            return networkConnections.IndexOf(connection);
        }

        /// <summary>
        /// Opens a connection for the selected shell
        /// </summary>
        /// <param name="shellType">Any shell type that has its <see cref="BaseShellInfo.AcceptsNetworkConnection"/> flag set to true.</param>
        /// <param name="establisher">The function responsible for establishing the network connection</param>
        /// <param name="address">Target address to connect to</param>
        public static void OpenConnectionForShell(ShellType shellType, Func<string, NetworkConnection> establisher, string address = "") =>
            OpenConnectionForShell(ShellManager.GetShellTypeName(shellType), establisher, address);

        /// <summary>
        /// Opens a connection for the selected shell
        /// </summary>
        /// <param name="shellType">Any shell type that has its <see cref="BaseShellInfo.AcceptsNetworkConnection"/> flag set to true.</param>
        /// <param name="establisher">The function responsible for establishing the network connection</param>
        /// <param name="address">Target address to connect to</param>
        public static void OpenConnectionForShell(string shellType, Func<string, NetworkConnection> establisher, string address = "")
        {
            // Get shell info to check to see if the shell accepts network connections
            var shellInfo = ShellManager.GetShellInfo(shellType);
            if (!shellInfo.AcceptsNetworkConnection)
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("The shell {0} doesn't accept network connections."), shellType);

            // Determine the network connection type
            // TODO: Deal with the custom network connection type as soon as we finish Beta 2.
            NetworkConnectionType connectionType = NetworkConnectionType.FTP;
            switch (shellType)
            {
                case "SFTPShell":
                    connectionType = NetworkConnectionType.SFTP;
                    break;
                case "MailShell":
                    connectionType = NetworkConnectionType.Mail;
                    break;
                case "RSSShell":
                    connectionType = NetworkConnectionType.RSS;
                    break;
                case "HTTPShell":
                    connectionType = NetworkConnectionType.HTTP;
                    break;
            }

            // Now, do the job!
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    // Select a connection according to user input
                    int selectedConnection = NetworkConnectionSelector.ConnectionSelector(connectionType);
                    var availableConnectionInstances = GetNetworkConnections(connectionType);
                    int availableConnections = GetNetworkConnections(connectionType).Length;
                    if (selectedConnection == -1)
                        return;

                    // Now, check to see if the user selected "Create a new connection"
                    NetworkConnection connection;
                    if (selectedConnection == availableConnections + 1)
                    {
                        // Prompt the user to provide connection information
                        address = Input.ReadLine(Translate.DoTranslation("Enter the server address:") + " ");
                        connection = establisher(address);
                    }
                    else if (selectedConnection == availableConnections + 2)
                    {
                        // Prompt the user to select a server to connect to from the speed dial
                        var speedDials = SpeedDialTools.ListSpeedDialEntries();
                        var connectionsChoiceList = new List<InputChoiceInfo>();
                        for (int i = 0; i < speedDials.Keys.Count; i++)
                        {
                            string connectionUrl = speedDials.ElementAt(i).Key;
                            connectionsChoiceList.Add(new InputChoiceInfo($"{i + 1}", connectionUrl));
                        }
                        int selectedSpeedDial = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a connection from the speed dial list."), connectionsChoiceList, new List<InputChoiceInfo>() {
                            new InputChoiceInfo($"{speedDials.Count + 1}", Translate.DoTranslation("Create a new connection")),
                        });
                        if (selectedSpeedDial == -1)
                            return;

                        // Now, check to see if we're going to connect
                        if (selectedSpeedDial == speedDials.Count + 1)
                        {
                            // User selected to create a new connection
                            address = Input.ReadLine(Translate.DoTranslation("Enter the server address:") + " ");
                            connection = establisher(address);
                        }
                        else
                        {
                            // Get the address from the speed dial and connect to it
                            var speedDialKvp = speedDials.ElementAt(selectedSpeedDial - 1);
                            address = speedDialKvp.Key;
                            connection = establisher(address);
                        }
                    }
                    else
                    {
                        // User selected connection
                        connection = availableConnectionInstances[selectedConnection - 1];
                    }

                    // Use that information to start the shell
                    ShellStart.StartShell(shellType, connection);
                }
                else
                {
                    // Check to see if the provided address has an already existing connection
                    var availableConnectionInstances = GetNetworkConnections(connectionType).Where((connection) => connection.ConnectionOriginalUrl.Contains(address)).ToArray();
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
                        if (selectedConnectionNumber == -1)
                            return;
                        ShellStart.StartShell(shellType, availableConnectionInstances[selectedConnectionNumber - 1]);
                    }
                    else
                        ShellStart.StartShell(shellType, establisher(address));
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to establish a connection [type: {0}] to a network [address: {1}] for shell: {2}", shellType, address, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Unknown networked shell error:") + " {0}", true, KernelColorType.Error, ex.Message);
            }
        }
    }
}
