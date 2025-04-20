//
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

using Nitrocid.Kernel.Power;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using System;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Restarts the remote kernel
    /// </summary>
    /// <remarks>
    /// This command restarts your simulated kernel in the remote instance and reloads all the config that are not loaded using reloadconfig.
    /// <br></br>
    /// > [!WARNING]
    /// > There is no file system syncing because the current kernel version doesn't have the real file system to sync, and the kernel is not final.
    /// </remarks>
    class RRebootCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool debug = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-debug");
            bool safe = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-safe");
            bool maintenance = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-maintenance");
            PowerMode mode =
                debug ? PowerMode.RemoteRestartDebug :
                safe ? PowerMode.RemoteRestartSafe :
                maintenance ? PowerMode.RemoteRestartMaintenance :
                PowerMode.RemoteRestart;
            if (!string.IsNullOrEmpty(parameters.ArgumentsList[0]))
            {
                if (parameters.ArgumentsList.Length > 1)
                    PowerManager.PowerManage(mode, parameters.ArgumentsList[0], Convert.ToInt32(parameters.ArgumentsList[1]));
                else
                    PowerManager.PowerManage(mode, parameters.ArgumentsList[0]);
            }
            return 0;
        }

    }
}
