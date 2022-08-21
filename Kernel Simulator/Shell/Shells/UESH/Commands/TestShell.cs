using KS.Kernel.Power;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

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

using KS.Shell.Shells.Test;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens a test shell
    /// </summary>
    /// <remarks>
    /// If you want to test functions of the kernel, you can do so using this command. Please note that it's only available in development versions of KS.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class TestShellCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            ShellStart.StartShell(ShellType.TestShell);
            if (TestShellCommon.Test_ShutdownFlag)
                PowerManager.PowerManage(PowerMode.Shutdown);
        }

    }
}