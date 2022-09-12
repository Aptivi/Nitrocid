
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

using KS.Misc.Reflection;
using System;

namespace KS.Kernel.Exceptions
{
    /// <summary>
    /// Thrown when the manpage was invalid
    /// </summary>
    public class InvalidManpageException : Exception
    {

        /// <inheritdoc/>
        public InvalidManpageException() : base()
        {
        }
        /// <inheritdoc/>
        public InvalidManpageException(string message) : base(message)
        {
        }
        /// <summary>
        /// Initializes the instance of this exception that has a message and a list of arguments
        /// </summary>
        /// <param name="vars">List of arguments</param>
        /// <param name="message">Message to be printed</param>
        public InvalidManpageException(string message, params object[] vars) : base(StringManipulate.FormatString(message, vars))
        {
        }
        /// <inheritdoc/>
        public InvalidManpageException(string message, Exception e) : base(message, e)
        {
        }
        /// <summary>
        /// Initializes the instance of this exception that has a message, an inner exception, and a list of arguments
        /// </summary>
        /// <param name="vars">List of arguments</param>
        /// <param name="e">Inner exception</param>
        /// <param name="message">Message to be printed</param>
        public InvalidManpageException(string message, Exception e, params object[] vars) : base(StringManipulate.FormatString(message, vars), e)
        {
        }

    }
}
