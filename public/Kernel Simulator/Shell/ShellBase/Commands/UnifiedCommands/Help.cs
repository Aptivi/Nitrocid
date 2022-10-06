
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
using System.Linq;
using KS.Kernel.Debugging.RemoteDebug.Interface;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Commands.UnifiedCommands
{
    /// <summary>
    /// Opens the help page
    /// </summary>
    /// <remarks>
    /// This command allows you to get help for any specific command, including its usage. If no command is specified, all commands are listed.
    /// </remarks>
    class HelpUnifiedCommand : CommandExecutor, ICommand, IRemoteDebugCommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (string.IsNullOrWhiteSpace(StringArgs))
            {
                HelpSystem.ShowHelp("");
            }
            else
            {
                HelpSystem.ShowHelp(ListArgsOnly[0]);
            }
        }

        void IRemoteDebugCommand.Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, StreamWriter SocketStreamWriter, string DeviceAddress)
        {
            if (ListArgsOnly.Length != 0)
            {
                HelpSystem.ShowHelp(ListArgsOnly[0], ShellType.RemoteDebugShell, SocketStreamWriter);
            }
            else
            {
                HelpSystem.ShowHelp("", ShellType.RemoteDebugShell, SocketStreamWriter);
            }
        }

        public static string[] ListCmds() => GetCommand.GetCommands(Shell.CurrentShellType).Keys.ToArray();

    }
}
