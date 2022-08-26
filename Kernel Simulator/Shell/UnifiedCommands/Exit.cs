
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

using KS.Kernel.Debugging.RemoteDebug;
using KS.Kernel.Debugging.RemoteDebug.Interface;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using System.IO;

namespace KS.Shell.UnifiedCommands
{
    /// <summary>
    /// Exits the subshell
    /// </summary>
    /// <remarks>
    /// If the UESH shell is a subshell, you can exit it. However, you can't use this command to log out of your account, because it can't exit the mother shell. The only to exit it is to use the logout command.
    /// </remarks>
    class ExitUnifiedCommand : CommandExecutor, ICommand, IRemoteDebugCommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            ShellStart.KillShell();
        }

        void IRemoteDebugCommand.Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, StreamWriter SocketStreamWriter, string DeviceAddress)
        {
            RemoteDebugTools.DisconnectDbgDev(DeviceAddress);
        }

    }
}
