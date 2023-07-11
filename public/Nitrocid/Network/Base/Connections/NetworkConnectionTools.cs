
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

using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (!networkConnections[connectionIndex].ConnectionIsInstance)
                networkConnections[connectionIndex].ConnectionThread.Stop();
            networkConnections.RemoveAt(connectionIndex);
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
                NetworkConnection connection = new(name, uri, connectionType, connectionThread, null);
                connection.ConnectionThread.Start();
                networkConnections.Add(connection);
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
                NetworkConnection connection = new(name, uri, connectionType, null, connectionInstance);

                // Just return the connection. This instance is an object and could be anything that represents a network connection.
                networkConnections.Add(connection);
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
    }
}
