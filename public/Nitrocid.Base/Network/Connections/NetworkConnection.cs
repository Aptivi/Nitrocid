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

using System;

namespace Nitrocid.Base.Network.Connections
{
    /// <summary>
    /// A class for network connection information
    /// </summary>
    public abstract class NetworkConnection
    {
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

        internal NetworkConnection(string connectionName, Uri connectionUri, string connectionType, string connectionOriginalUrl)
        {
            ConnectionName = connectionName;
            ConnectionUri = connectionUri;
            ConnectionType = connectionType;
            ConnectionOriginalUrl = connectionOriginalUrl;
        }
    }
}
