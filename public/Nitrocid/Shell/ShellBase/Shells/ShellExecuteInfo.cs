//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using System.Collections.Generic;
using Nitrocid.Kernel.Threading;

namespace Nitrocid.Shell.ShellBase.Shells
{
    /// <summary>
    /// Shell execution information
    /// </summary>
    public class ShellExecuteInfo
    {

        internal int LastErrorCode = 0;
        internal readonly List<KernelThread> AltCommandThreads = [];
        private readonly string shellType;
        private readonly BaseShell? shellBase;
        private readonly KernelThread shellCommandThread;

        /// <summary>
        /// Shell type
        /// </summary>
        public string ShellType => shellType;

        /// <summary>
        /// Shell base class
        /// </summary>
        public BaseShell? ShellBase => shellBase;

        /// <summary>
        /// Shell command thread
        /// </summary>
        public KernelThread ShellCommandThread => shellCommandThread;

        /// <summary>
        /// Installs the values to a new instance of ShellInfo
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellBase">Shell base class</param>
        /// <param name="ShellCommandThread">Shell command thread</param>
        public ShellExecuteInfo(ShellType ShellType, BaseShell? ShellBase, KernelThread ShellCommandThread) :
            this(ShellManager.GetShellTypeName(ShellType), ShellBase, ShellCommandThread)
        { }

        /// <summary>
        /// Installs the values to a new instance of ShellInfo
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellBase">Shell base class</param>
        /// <param name="ShellCommandThread">Shell command thread</param>
        public ShellExecuteInfo(string ShellType, BaseShell? ShellBase, KernelThread ShellCommandThread)
        {
            shellType = ShellType;
            shellBase = ShellBase;
            shellCommandThread = ShellCommandThread;
        }

    }
}
