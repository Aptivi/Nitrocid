
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

using KS.Network.RemoteDebug;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you enable remote debugger inside the debugging core. This allows users who need to see what's going on in another computer running KS to see its debugging logs. It uses port number 3014 and can be changed.
    /// </summary>
    class Test_RDebugCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (Convert.ToBoolean(ListArgsOnly[0]) == true)
            {
                RemoteDebugger.StartRDebugThread();
            }
            else
            {
                RemoteDebugger.StopRDebugThread();
            }
        }

    }
}
