
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

using KS.Kernel.Exceptions;
using System;

namespace KS.Kernel.Debugging.RemoteDebug.Command
{
    /// <summary>
    /// The command executor class
    /// </summary>
    public abstract class RemoteDebugBaseCommand : IRemoteDebugCommand
    {

        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="StringArgs">String of arguments</param>
        /// <param name="ListArgsOnly">List of all arguments</param>
        /// <param name="ListSwitchesOnly">List of all switches</param>
        /// <param name="device">Device that executed the command</param>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, RemoteDebugDevice device)
        {
            DebugWriter.WriteDebug(DebugLevel.F, "We shouldn't be here!!!");
            throw new KernelException(KernelExceptionType.NotImplementedYet);
        }

        /// <summary>
        /// The help helper
        /// </summary>
        public virtual void HelpHelper() => DebugWriter.WriteDebug(DebugLevel.I, "No additional information found.");

    }
}
