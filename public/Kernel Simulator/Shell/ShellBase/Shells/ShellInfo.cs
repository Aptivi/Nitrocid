
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

using System.Collections.Generic;
using KS.Misc.Threading;

namespace KS.Shell.ShellBase.Shells
{
    /// <summary>
    /// Shell information
    /// </summary>
    public class ShellInfo
    {

        /// <summary>
        /// Shell type
        /// </summary>
        public readonly ShellType ShellType;
        /// <summary>
        /// Shell executor
        /// </summary>
        public readonly ShellExecutor ShellExecutor;
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
        /// <param name="ShellExecutor">Shell executor</param>
        /// <param name="ShellCommandThread">Shell command thread</param>
        public ShellInfo(ShellType ShellType, ShellExecutor ShellExecutor, KernelThread ShellCommandThread)
        {
            this.ShellType = ShellType;
            this.ShellExecutor = ShellExecutor;
            this.ShellCommandThread = ShellCommandThread;
        }

    }
}
