//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Textify.General;

namespace Nitrocid.Base.Kernel.Exceptions
{
    /// <summary>
    /// A kernel exception occurred
    /// </summary>
    public class KernelException : Exception
    {

        /// <summary>
        /// Gets the exception type
        /// </summary>
        public KernelExceptionType ExceptionType { get; }

        /// <summary>
        /// Gets the original exception message before wrapping
        /// </summary>
        public string OriginalExceptionMessage { get; }

        /// <summary>
        /// Gets the kernel exception message
        /// </summary>
        public string KernelExceptionMessage =>
            KernelExceptionMessages.GetMessageFromType(ExceptionType);

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        public KernelException(KernelExceptionType exceptionType) :
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, "", null))
        {
            ExceptionType = exceptionType;
            OriginalExceptionMessage = "";
        }

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="e">Inner exception</param>
        public KernelException(KernelExceptionType exceptionType, Exception e) :
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, "", e), e)
        {
            ExceptionType = exceptionType;
            OriginalExceptionMessage = "";
        }

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="message">Message to be printed</param>
        public KernelException(KernelExceptionType exceptionType, string message) :
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, message, null))
        {
            ExceptionType = exceptionType;
            OriginalExceptionMessage = message;
        }

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="vars">List of arguments</param>
        /// <param name="message">Message to be printed</param>
        public KernelException(KernelExceptionType exceptionType, string message, params object[] vars) :
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, TextTools.FormatString(message, vars), null))
        {
            ExceptionType = exceptionType;
            OriginalExceptionMessage = TextTools.FormatString(message, vars);
        }

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="e">Inner exception</param>
        /// <param name="message">Message to be printed</param>
        public KernelException(KernelExceptionType exceptionType, string message, Exception e) :
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, message, e), e)
        {
            ExceptionType = exceptionType;
            OriginalExceptionMessage = message;
        }

        /// <summary>
        /// Initializes the instance of the kernel exception
        /// </summary>
        /// <param name="exceptionType">Exception type</param>
        /// <param name="vars">List of arguments</param>
        /// <param name="e">Inner exception</param>
        /// <param name="message">Message to be printed</param>
        public KernelException(KernelExceptionType exceptionType, string message, Exception e, params object[] vars) :
            base(KernelExceptionMessages.GetFinalExceptionMessage(exceptionType, TextTools.FormatString(message, vars), e), e)
        {
            ExceptionType = exceptionType;
            OriginalExceptionMessage = TextTools.FormatString(message, vars);
        }

    }
}
