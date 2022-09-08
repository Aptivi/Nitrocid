
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

using System;
using System.IO;
using KS.Kernel.Debugging;
using KS.Kernel.Debugging.RemoteDebug.Interface;

namespace KS.Shell.ShellBase.Commands
{
    public abstract class CommandExecutor : ICommand, IRemoteDebugCommand
    {

        public virtual void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            DebugWriter.WriteDebug(DebugLevel.F, "We shouldn't be here!!!");
            throw new InvalidOperationException();
        }

        void IRemoteDebugCommand.Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, StreamWriter SocketStreamWriter, string DeviceAddress) { }

        public virtual void HelpHelper() => DebugWriter.WriteDebug(DebugLevel.I, "No additional information found.");

    }
}
