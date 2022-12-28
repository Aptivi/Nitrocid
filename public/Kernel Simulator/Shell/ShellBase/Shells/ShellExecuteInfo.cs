
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

using System.Collections.Generic;
using KS.Misc.Threading;

namespace KS.Shell.ShellBase.Shells
{
    /// <summary>
    /// Shell information
    /// </summary>
    public class ShellExecuteInfo
    {

        /// <summary>
        /// Shell type
        /// </summary>
        public readonly string ShellType;
        /// <summary>
        /// Shell base class
        /// </summary>
        public readonly BaseShell ShellBase;
        /// <summary>
        /// Shell command thread
        /// </summary>
        public readonly KernelThread ShellCommandThread;
        /// <summary>
        /// Alternative shell command threads
        /// </summary>
        protected internal readonly List<KernelThread> AltCommandThreads = new();

        /// <summary>
        /// Installs the values to a new instance of ShellInfo
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellBase">Shell base class</param>
        /// <param name="ShellCommandThread">Shell command thread</param>
        public ShellExecuteInfo(ShellType ShellType, BaseShell ShellBase, KernelThread ShellCommandThread) :
            this(Shell.GetShellTypeName(ShellType), ShellBase, ShellCommandThread)
        { }

        /// <summary>
        /// Installs the values to a new instance of ShellInfo
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellBase">Shell base class</param>
        /// <param name="ShellCommandThread">Shell command thread</param>
        public ShellExecuteInfo(string ShellType, BaseShell ShellBase, KernelThread ShellCommandThread)
        {
            this.ShellType = ShellType;
            this.ShellBase = ShellBase;
            this.ShellCommandThread = ShellCommandThread;
        }

    }
}
