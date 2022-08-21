
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using System.IO;

namespace KS.Network.RemoteDebug.Interface
{
    public interface IRemoteDebugCommand
    {

        /// <summary>
        /// Executes the command with the given argument
        /// </summary>
        /// <param name="StringArgs">Arguments in a string</param>
        /// <param name="ListArgsOnly">List of provided arguments</param>
        /// <param name="ListSwitchesOnly">List of provided switches</param>
        /// <param name="SocketStreamWriter">Target debug devide to write on</param>
        /// <param name="DeviceAddress">Target device address</param>
        void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, StreamWriter SocketStreamWriter, string DeviceAddress);

    }
}