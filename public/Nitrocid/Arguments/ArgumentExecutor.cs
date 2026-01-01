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

using System;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Arguments
{
    /// <summary>
    /// Argument executor base class
    /// </summary>
    public abstract class ArgumentExecutor : IArgument
    {

        /// <summary>
        /// Executes the argument
        /// </summary>
        /// <param name="parameters">Argument parameters including passed arguments and switches information</param>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual void Execute(ArgumentParameters parameters)
        {
            DebugWriter.WriteDebug(DebugLevel.F, "We shouldn't be here!!!");
            throw new KernelException(KernelExceptionType.NotImplementedYet);
        }

        /// <summary>
        /// The help helper
        /// </summary>
        public virtual void HelpHelper() =>
            DebugWriter.WriteDebug(DebugLevel.I, "No additional information found.");

    }
}
