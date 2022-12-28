
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Misc.Threading;
using System.IO;
using System.Net.Sockets;

namespace KS.Kernel.Debugging.RemoteDebug
{
    /// <summary>
    /// Remote debug device instance
    /// </summary>
    public class RemoteDebugDevice
    {

        /// <summary>
        /// The client socket
        /// </summary>
        public Socket ClientSocket { get; private set; }
        /// <summary>
        /// The client stream
        /// </summary>
        public NetworkStream ClientStream { get; private set; }
        /// <summary>
        /// The client stream writer
        /// </summary>
        public StreamWriter ClientStreamWriter { get; private set; }
        /// <summary>
        /// The client IP address
        /// </summary>
        public string ClientIP { get; private set; }
        /// <summary>
        /// The client name
        /// </summary>
        public string ClientName { get; set; }
        internal KernelThread ClientThread { get; private set; }

        /// <summary>
        /// Makes a new instance of a remote debug device
        /// </summary>
        internal RemoteDebugDevice(Socket ClientSocket, NetworkStream ClientStream, string ClientIP, string ClientName, KernelThread ClientThread)
        {
            this.ClientSocket = ClientSocket;
            this.ClientStream = ClientStream;
            ClientStreamWriter = new StreamWriter(ClientStream) { AutoFlush = true };
            this.ClientIP = ClientIP;
            this.ClientName = ClientName;
            this.ClientThread = ClientThread;
        }

    }
}
