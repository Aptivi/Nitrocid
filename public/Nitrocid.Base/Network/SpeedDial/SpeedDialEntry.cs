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

using Newtonsoft.Json;

namespace Nitrocid.Base.Network.SpeedDial
{
    /// <summary>
    /// Speed dial entry
    /// </summary>
    public class SpeedDialEntry
    {
        [JsonProperty(nameof(Address))]
        private readonly string address;
        [JsonProperty(nameof(Port))]
        private readonly int port;
        [JsonProperty(nameof(Type))]
        private readonly string type;
        [JsonProperty(nameof(Options))]
        private readonly object[] options;

        /// <summary>
        /// IP address to connect to
        /// </summary>
        [JsonIgnore]
        public string Address =>
            address;
        /// <summary>
        /// Port to connect to
        /// </summary>
        [JsonIgnore]
        public int Port =>
            port;
        /// <summary>
        /// Speed dial type
        /// </summary>
        [JsonIgnore]
        public string Type =>
            type;
        /// <summary>
        /// Speed dial options
        /// </summary>
        [JsonIgnore]
        public object[] Options =>
            options;

        [JsonConstructor]
        internal SpeedDialEntry(string address, int port, string type, object[] options)
        {
            this.address = address;
            this.port = port;
            this.type = type;
            this.options = options;
        }
    }
}
