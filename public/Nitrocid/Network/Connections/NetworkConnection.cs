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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using System;

namespace Nitrocid.Network.Connections
{
    /// <summary>
    /// A class for network connection information
    /// </summary>
    public class NetworkConnection
    {
        private readonly bool connectionIsInstance;

        /// <summary>
        /// Connection name
        /// </summary>
        public string ConnectionName { get; }
        /// <summary>
        /// Connection URL
        /// </summary>
        public Uri ConnectionUri { get; }
        /// <summary>
        /// Connection original URL, in case the URI method didn't work
        /// </summary>
        public string ConnectionOriginalUrl { get; }
        /// <summary>
        /// Connection type
        /// </summary>
        public string ConnectionType { get; }

        /// <summary>
        /// Is this connection using an instance instead of a thread?
        /// </summary>
        public bool ConnectionIsInstance =>
            connectionIsInstance;
        /// <summary>
        /// Is the connection alive? [Only use this if this connection uses threads.]
        /// </summary>
        public bool ConnectionAlive =>
            connectionIsInstance ?
            throw new KernelException(KernelExceptionType.NetworkConnection,
                Translate.DoTranslation("This property is only valid for connections with threads. For instance connections, call the appropriate property on the instance itself instead of calling this property.")) :
            ConnectionThread?.IsAlive ?? false;

        internal KernelThread? ConnectionThread { get; }
        internal object? ConnectionInstance { get; }

        internal NetworkConnection(string connectionName, Uri connectionUri, string connectionType, KernelThread? connectionThread, object? connectionInstance, string connectionOriginalUrl)
        {
            ConnectionName = connectionName;
            ConnectionUri = connectionUri;
            ConnectionType = connectionType;
            ConnectionThread = connectionThread;
            ConnectionInstance = connectionInstance;
            connectionIsInstance = connectionThread is null;
            ConnectionOriginalUrl = connectionOriginalUrl;
        }
    }
}
