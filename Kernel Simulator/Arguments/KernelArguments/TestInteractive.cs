using System;
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

using KS.Arguments.ArgumentBase;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.Test;

namespace KS.Arguments.KernelArguments
{
    class TestInteractiveArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            Kernel.Kernel.StageTimer.Stop();
            ShellStart.StartShellForced(ShellType.TestShell);
            ShellStart.PurgeShells();
            Kernel.Kernel.StageTimer.Start();
            if (TestShellCommon.Test_ShutdownFlag)
            {
                // Clear the console and reset the colors
                Console.ResetColor();
                Console.Clear();
                Environment.Exit(0);
            }
        }

    }
}