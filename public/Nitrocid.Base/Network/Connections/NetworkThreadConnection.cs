//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Threadify.Manager;
using System;

namespace Nitrocid.Base.Network.Connections
{
    /// <summary>
    /// A class for thread-based network connection information
    /// </summary>
    public class NetworkThreadConnection : NetworkConnection
    {
        /// <summary>
        /// Is the connection alive?
        /// </summary>
        public bool ConnectionAlive =>
            ConnectionThread.IsAlive;

        /// <summary>
        /// Connection thread
        /// </summary>
        public ThreadInstance ConnectionThread { get; }

        internal NetworkThreadConnection(string connectionName, Uri connectionUri, string connectionType, string connectionOriginalUrl, ThreadInstance connectionThread) :
            base(connectionName, connectionUri, connectionType, connectionOriginalUrl)
        {
            ConnectionThread = connectionThread;
        }
    }
}
