
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

using KS.Misc.Text;
using System;

namespace KS.Kernel.Exceptions
{
    /// <summary>
    /// A kernel exception occurred
    /// </summary>
    public class KernelException : Exception
    {

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        public KernelException(KernelExceptionType exceptionType) : 
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, "", null))
        { ExceptionType = exceptionType; }

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="e">Inner exception</param>
        public KernelException(KernelExceptionType exceptionType, Exception e) : 
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, "", e), e)
        { ExceptionType = exceptionType; }

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="message">Message to be printed</param>
        public KernelException(KernelExceptionType exceptionType, string message) : 
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, message, null))
        { ExceptionType = exceptionType; }

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="vars">List of arguments</param>
        /// <param name="message">Message to be printed</param>
        public KernelException(KernelExceptionType exceptionType, string message, params object[] vars) : 
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, TextTools.FormatString(message, vars), null))
        { ExceptionType = exceptionType; }

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="e">Inner exception</param>
        /// <param name="message">Message to be printed</param>
        public KernelException(KernelExceptionType exceptionType, string message, Exception e) : 
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, message, e), e)
        { ExceptionType = exceptionType; }

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="vars">List of arguments</param>
        /// <param name="e">Inner exception</param>
        /// <param name="message">Message to be printed</param>
        public KernelException(KernelExceptionType exceptionType, string message, Exception e, params object[] vars) : 
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, TextTools.FormatString(message, vars), e), e)
        { ExceptionType = exceptionType; }

        /// <summary>
        /// Gets the exception type
        /// </summary>
        public KernelExceptionType ExceptionType { get; }

    }
}
