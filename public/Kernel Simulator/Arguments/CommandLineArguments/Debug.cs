
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

using KS.Arguments.ArgumentBase;
using KS.Kernel;
using KS.Kernel.Debugging;
using System;

namespace KS.Arguments.CommandLineArguments
{
    class CommandLine_DebugArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            Flags.DebugMode = true;

            // Write headers for debug
            DebugWriter.WriteDebug(DebugLevel.I, "-------------------------------------------------------------------");
            DebugWriter.WriteDebug(DebugLevel.I, "Kernel initialized, version {0}.", KernelTools.KernelVersion.ToString());
            DebugWriter.WriteDebug(DebugLevel.I, "Kernel mod API version {0}.", KernelTools.KernelApiVersion.ToString());
            DebugWriter.WriteDebug(DebugLevel.I, "OS: {0}", Environment.OSVersion.ToString());
        }
    }
}
