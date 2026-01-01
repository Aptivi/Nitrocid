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
using System.Collections.Generic;
using System.Linq;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Kernel.Threading;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Inputs.Styles.Selection;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Network.SpeedDial;
using Terminaux.Inputs.Styles;

namespace Nitrocid.Network.Connections
{
    /// <summary>
    /// Network connection tools to manipulate with connections
    /// </summary>
    public static class NetworkConnectionTools
    {
        private static readonly List<NetworkConnection> networkConnections = [];
        internal static readonly List<string> networkTypes =
        [
            NetworkConnectionType.FTP.ToString(),
            NetworkConnectionType.HTTP.ToString(),
            NetworkConnectionType.Mail.ToString(),
            NetworkConnectionType.RSS.ToString(),
            NetworkConnectionType.SFTP.ToString(),
            NetworkConnectionType.SSH.ToString(),
        ];

        /// <summary>
        /// Gets the network connections according to the given type
        /// </summary>
        /// <param name="connectionType">Type of connection</param>
        /// <returns>Array of <see cref="NetworkConnection"/>s that is of the same type specified</returns>
        public static NetworkConnection[] GetNetworkConnections(NetworkConnectionType connectionType) =>
            GetNetworkConnections(connectionType.ToString());

        /// <summary>
        /// Gets the network connections according to the given type name
        /// </summary>
        /// <param name="connectionType">Type of connection</param>
        /// <returns>Array of <see cref="NetworkConnection"/>s that is of the same type specified. If the connection type is not found, it throws an exception.</returns>
        public static NetworkConnection[] GetNetworkConnections(string connectionType)
        {
            var connections = networkConnections.Where((connection) => connection.ConnectionType == connectionType);
            if (!networkTypes.Contains(connectionType))
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("Connection type is not found"));
            return connections.ToArray();
        }

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
                networkConnections[connectionIndex].ConnectionThread?.Stop();
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
        public static NetworkConnection EstablishConnection(string name, string url, NetworkConnectionType connectionType, KernelThread connectionThread) =>
            EstablishConnection(name, url, connectionType.ToString(), connectionThread);

        /// <summary>
        /// Establishes the connection to the given URI
        /// </summary>
        /// <param name="name">Connection name</param>
        /// <param name="uri">Connection URI</param>
        /// <param name="connectionType">Connection type</param>
        /// <param name="connectionThread">Thread which holds a loop for connection (usually send/receive)</param>
        /// <returns>An instance of NetworkConnection</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection EstablishConnection(string name, Uri uri, NetworkConnectionType connectionType, KernelThread connectionThread) =>
            EstablishConnection(name, uri, connectionType.ToString(), connectionThread);

        /// <summary>
        /// Establishes the connection to the given URL
        /// </summary>
        /// <param name="name">Connection name</param>
        /// <param name="url">Connection URL</param>
        /// <param name="connectionType">Connection type</param>
        /// <param name="connectionThread">Thread which holds a loop for connection (usually send/receive)</param>
        /// <returns>An instance of NetworkConnection</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection EstablishConnection(string name, string url, string connectionType, KernelThread connectionThread)
        {
            if (!networkTypes.Contains(connectionType))
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("Connection type is not found"));

            // First, parse the URL
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri? uri))
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
        public static NetworkConnection EstablishConnection(string name, Uri uri, string connectionType, KernelThread connectionThread)
        {
            if (!networkTypes.Contains(connectionType))
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("Connection type is not found"));

            // Now, make a connection and start the connection thread
            try
            {
                NetworkConnection connection = new(name, uri, connectionType, connectionThread, null, uri.OriginalString);
                connection.ConnectionThread?.Start();
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
        public static NetworkConnection EstablishConnection(string name, string url, NetworkConnectionType connectionType, object connectionInstance) =>
            EstablishConnection(name, url, connectionType.ToString(), connectionInstance);

        /// <summary>
        /// Establishes the connection to the given URI
        /// </summary>
        /// <param name="name">Connection name</param>
        /// <param name="uri">Connection URI</param>
        /// <param name="connectionType">Connection type</param>
        /// <param name="connectionInstance">Instance which holds this connection (if threads are unsuitable)</param>
        /// <returns>An instance of NetworkConnection</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection EstablishConnection(string name, Uri uri, NetworkConnectionType connectionType, object connectionInstance) =>
            EstablishConnection(name, uri, connectionType.ToString(), connectionInstance);

        /// <summary>
        /// Establishes the connection to the given URL
        /// </summary>
        /// <param name="name">Connection name</param>
        /// <param name="url">Connection URL</param>
        /// <param name="connectionType">Connection type</param>
        /// <param name="connectionInstance">Instance which holds this connection (if threads are unsuitable)</param>
        /// <returns>An instance of NetworkConnection</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection EstablishConnection(string name, string url, string connectionType, object connectionInstance)
        {
            if (!networkTypes.Contains(connectionType))
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("Connection type is not found"));

            // First, parse the URL
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri? uri))
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
        public static NetworkConnection EstablishConnection(string name, Uri uri, string connectionType, object connectionInstance)
        {
            if (!networkTypes.Contains(connectionType))
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("Connection type is not found"));

            // Now, make a connection and start the connection thread
            try
            {
                NetworkConnection connection = new(name, uri, connectionType, null, connectionInstance, uri.OriginalString);

                // Just return the connection. This instance is an object and could be anything that represents a network connection.
                networkConnections.Add(connection);
                if (connectionInstance is not null)
                    DebugWriter.WriteDebug(DebugLevel.I, "Added connection {0} for URI {1} to {2} list with instance type {3}", name, uri.ToString(), connectionType.ToString(), connectionInstance.GetType().Name);
                else
                    DebugWriter.WriteDebug(DebugLevel.I, "Added connection {0} for URI {1} to {2} list...", name, uri.ToString(), connectionType.ToString());
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
        public static int GetConnectionIndex(NetworkConnection? connection)
        {
            // Check to see if we have this connection
            if (connection is null)
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("Connection is not established yet."));
            if (!networkConnections.Contains(connection))
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("Connection is not established yet."));
            return networkConnections.IndexOf(connection);
        }

        /// <summary>
        /// Gets the connection index from the specific type
        /// </summary>
        /// <param name="connection">Network connection to get its index from</param>
        /// <param name="connectionType">Connection type</param>
        /// <returns>Network connection index starting from zero (0)</returns>
        /// <exception cref="KernelException"></exception>
        public static int GetConnectionIndexSpecific(NetworkConnection connection, NetworkConnectionType connectionType) =>
            GetConnectionIndexSpecific(connection, connectionType.ToString());

        /// <summary>
        /// Gets the connection index from the specific type
        /// </summary>
        /// <param name="connection">Network connection to get its index from</param>
        /// <param name="connectionType">Connection type</param>
        /// <returns>Network connection index starting from zero (0)</returns>
        /// <exception cref="KernelException"></exception>
        public static int GetConnectionIndexSpecific(NetworkConnection connection, string connectionType)
        {
            if (!networkTypes.Contains(connectionType))
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("Connection type is not found"));

            // Check to see if we have this connection
            var availableConnections = GetNetworkConnections(connectionType);
            if (!availableConnections.Contains(connection))
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("Connection is not established yet."));
            return Array.IndexOf(availableConnections, connection);
        }

        /// <summary>
        /// Gets the connection from the index
        /// </summary>
        /// <param name="index">Network connection index</param>
        /// <returns>Network connection from the index</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection GetConnectionFromIndex(int index)
        {
            // Check to see if we have this connection
            if (index >= networkConnections.Count || index < 0)
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("The connection index is out of range."));
            return networkConnections[index];
        }

        /// <summary>
        /// Gets the connection from the index from the specific type
        /// </summary>
        /// <param name="index">Network connection index</param>
        /// <param name="connectionType">Connection type</param>
        /// <returns>Network connection from the index</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection GetConnectionFromIndexSpecific(int index, NetworkConnectionType connectionType) =>
            GetConnectionFromIndexSpecific(index, connectionType.ToString());

        /// <summary>
        /// Gets the connection from the index from the specific type
        /// </summary>
        /// <param name="index">Network connection index</param>
        /// <param name="connectionType">Connection type</param>
        /// <returns>Network connection from the index</returns>
        /// <exception cref="KernelException"></exception>
        public static NetworkConnection GetConnectionFromIndexSpecific(int index, string connectionType)
        {
            if (!networkTypes.Contains(connectionType))
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("Connection type is not found"));

            // Check to see if we have this connection
            var availableConnections = GetNetworkConnections(connectionType);
            if (index >= availableConnections.Length || index < 0)
                throw new KernelException(KernelExceptionType.NetworkConnection,
                    Translate.DoTranslation("The connection index is out of range."));
            return availableConnections[index];
        }

        /// <summary>
        /// Registers a custom connection type
        /// </summary>
        /// <param name="connectionType">Connection type to be registered</param>
        /// <exception cref="KernelException"></exception>
        public static void RegisterCustomConnectionType(string connectionType)
        {
            if (networkTypes.Contains(connectionType))
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("Connection type already exists"));

            // Now, add the connection type
            networkTypes.Add(connectionType);
        }

        /// <summary>
        /// Unregisters a custom connection type
        /// </summary>
        /// <param name="connectionType">Connection type to be unregistered</param>
        /// <exception cref="KernelException"></exception>
        public static void UnregisterCustomConnectionType(string connectionType)
        {
            if (!networkTypes.Contains(connectionType))
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("Connection type is not found"));

            // Now, add the connection type
            networkTypes.Remove(connectionType);
        }

        /// <summary>
        /// Checks to see if the connection type exists
        /// </summary>
        /// <param name="connectionType">Connection type to be queried</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool ConnectionTypeExists(string connectionType) =>
            networkTypes.Contains(connectionType);

        /// <summary>
        /// Opens a connection for the selected shell
        /// </summary>
        /// <param name="shellType">Any shell type that has its <see cref="BaseShellInfo.AcceptsNetworkConnection"/> flag set to true.</param>
        /// <param name="establisher">The function responsible for establishing the network connection</param>
        /// <param name="speedEstablisher">The function responsible for establishing the network connection with speed dial options</param>
        /// <param name="address">Target address to connect to</param>
        public static void OpenConnectionForShell(ShellType shellType, Func<string, NetworkConnection?> establisher, Func<string, SpeedDialEntry, NetworkConnection?> speedEstablisher, string address = "") =>
            OpenConnectionForShell(ShellManager.GetShellTypeName(shellType), establisher, speedEstablisher, address);

        /// <summary>
        /// Opens a connection for the selected shell
        /// </summary>
        /// <param name="shellType">Any shell type that has its <see cref="BaseShellInfo.AcceptsNetworkConnection"/> flag set to true.</param>
        /// <param name="establisher">The function responsible for establishing the network connection</param>
        /// <param name="speedEstablisher">The function responsible for establishing the network connection with speed dial options</param>
        /// <param name="address">Target address to connect to</param>
        public static void OpenConnectionForShell(string shellType, Func<string, NetworkConnection?> establisher, Func<string, SpeedDialEntry, NetworkConnection?> speedEstablisher, string address = "")
        {
            // Get shell info to check to see if the shell accepts network connections
            var shellInfo = ShellManager.GetShellInfo(shellType);
            if (!shellInfo.AcceptsNetworkConnection)
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("The shell {0} doesn't accept network connections."), shellType);

            // Determine the network connection type
            string connectionType = shellInfo.NetworkConnectionType;
            if (!networkTypes.Contains(connectionType))
                throw new KernelException(KernelExceptionType.NetworkConnection, Translate.DoTranslation("Connection type is not found"));

            // Now, do the job!
            try
            {
                NetworkConnection? connection;
                if (string.IsNullOrEmpty(address))
                {
                    // Select a connection according to user input
                    int selectedConnection = NetworkConnectionSelector.ConnectionSelector(connectionType);
                    var availableConnectionInstances = GetNetworkConnections(connectionType);
                    int availableConnections = availableConnectionInstances.Length;
                    DebugWriter.WriteDebug(DebugLevel.I, "Selected connection {0} out of {1} connections", selectedConnection, availableConnections);
                    if (selectedConnection == -1)
                        return;

                    // Now, check to see if the user selected "Create a new connection"
                    if (selectedConnection == availableConnections + 1)
                    {
                        // Prompt the user to provide connection information
                        DebugWriter.WriteDebug(DebugLevel.I, "Letting user provide connection info...");
                        address = InputTools.ReadLine(Translate.DoTranslation("Enter the server address:") + " ");
                        connection = establisher(address);
                    }
                    else if (selectedConnection == availableConnections + 2)
                    {
                        // Prompt the user to select a server to connect to from the speed dial
                        var speedDials = SpeedDialTools.ListSpeedDialEntriesByType(connectionType);
                        var connectionsChoiceList = new List<InputChoiceInfo>();
                        for (int i = 0; i < speedDials.Length; i++)
                        {
                            string connectionUrl = speedDials[i].Address;
                            DebugWriter.WriteDebug(DebugLevel.I, "Speed dial info: {0}.", connectionUrl);
                            connectionsChoiceList.Add(new InputChoiceInfo($"{i + 1}", connectionUrl));
                        }
                        int selectedSpeedDial = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a connection from the speed dial list."), [.. connectionsChoiceList], [
                            new InputChoiceInfo($"{speedDials.Length + 1}", Translate.DoTranslation("Create a new connection")),
                        ]);
                        DebugWriter.WriteDebug(DebugLevel.I, "Selected speed dial {0} out of {1} servers", selectedSpeedDial, speedDials.Length);
                        if (selectedSpeedDial == -1)
                            return;

                        // Now, check to see if we're going to connect
                        if (selectedSpeedDial == speedDials.Length + 1)
                        {
                            // User selected to create a new connection
                            DebugWriter.WriteDebug(DebugLevel.I, "Letting user provide connection info...");
                            address = InputTools.ReadLine(Translate.DoTranslation("Enter the server address:") + " ");
                            connection = establisher(address);
                        }
                        else
                        {
                            // Get the address from the speed dial and connect to it
                            var speedDialKvp = speedDials.ElementAt(selectedSpeedDial - 1);
                            address = speedDialKvp.Address;
                            DebugWriter.WriteDebug(DebugLevel.I, "Establishing connection to {0}...", address);
                            connection = speedEstablisher(address, speedDialKvp);
                        }
                    }
                    else
                    {
                        // User selected connection
                        DebugWriter.WriteDebug(DebugLevel.I, "Establishing connection to {0}...", selectedConnection);
                        connection = availableConnectionInstances[selectedConnection - 1];
                    }
                }
                else
                {
                    // Check to see if the provided address has an already existing connection
                    var availableConnectionInstances = GetNetworkConnections(connectionType).Where((connection) => connection.ConnectionOriginalUrl.Contains(address)).ToArray();
                    if (availableConnectionInstances.Length != 0)
                    {
                        var connectionNames = availableConnectionInstances.Select((connection) => connection.ConnectionUri.ToString()).ToArray();
                        var connectionsChoiceList = new List<InputChoiceInfo>();
                        for (int i = 0; i < connectionNames.Length; i++)
                        {
                            string connectionUrl = connectionNames[i];
                            connectionsChoiceList.Add(new InputChoiceInfo($"{i + 1}", connectionUrl));
                        }

                        // Get connection from user selection
                        int selectedConnectionNumber = SelectionStyle.PromptSelection(Translate.DoTranslation("Select a connection."), [.. connectionsChoiceList]);
                        DebugWriter.WriteDebug(DebugLevel.I, "Selected connection {0} out of {1} connections", selectedConnectionNumber, availableConnectionInstances.Length);
                        if (selectedConnectionNumber == -1)
                            return;
                        DebugWriter.WriteDebug(DebugLevel.I, "Opening shell to selected connection number {0}...", selectedConnectionNumber);
                        connection = availableConnectionInstances[selectedConnectionNumber - 1];
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Opening shell to selected connection created by the invoker for address {0}...", address);
                        connection = establisher(address);
                    }
                }

                // Check the connection for validity
                if (connection is null)
                    throw new KernelException(KernelExceptionType.NetworkConnection);

                // Use that information to start the shell
                DebugWriter.WriteDebug(DebugLevel.I, "Finalizing the shell...");
                ShellManager.StartShell(shellType, connection);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to establish a connection [type: {0}] to a network [address: {1}] for shell: {2}", shellType, address, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(Translate.DoTranslation("Unknown networked shell error:") + " {0}", true, KernelColorType.Error, ex.Message);
            }
        }
    }
}
