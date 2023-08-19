
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Power;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Restarts the kernel
    /// </summary>
    /// <remarks>
    /// This command restarts your simulated kernel and reloads all the config that are not loaded using reloadconfig. This is especially useful if you want to change colors of text, set arguments into the kernel, inject arguments, and so on.
    /// <br></br>
    /// > [!WARNING]
    /// > There is no file system syncing because the current kernel version doesn't have the real file system to sync, and the kernel is not final.
    /// </remarks>
    class RebootCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            if (!(ListArgsOnly.Length == 0))
            {
                if (ListArgsOnly[0] == "safe")
                {
                    PowerManager.PowerManage(PowerMode.RebootSafe);
                }
                else if (!string.IsNullOrEmpty(ListArgsOnly[0]))
                {
                    if (ListArgsOnly.Length > 1)
                    {
                        PowerManager.PowerManage(PowerMode.RemoteRestart, ListArgsOnly[0], Convert.ToInt32(ListArgsOnly[1]));
                    }
                    else
                    {
                        PowerManager.PowerManage(PowerMode.RemoteRestart, ListArgsOnly[0]);
                    }
                }
                else
                {
                    PowerManager.PowerManage(PowerMode.Reboot);
                }
            }
            else
            {
                PowerManager.PowerManage(PowerMode.Reboot);
            }
            return 0;
        }

    }
}
