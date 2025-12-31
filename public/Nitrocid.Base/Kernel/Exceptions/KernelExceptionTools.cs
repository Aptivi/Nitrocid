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

namespace Nitrocid.Base.Kernel.Exceptions
{
    /// <summary>
    /// Kernel exception tools
    /// </summary>
    public static class KernelExceptionTools
    {
        /// <summary>
        /// Gets an error code for kernel exceptions
        /// </summary>
        /// <param name="kex">Kernel exception instance</param>
        /// <returns>Kernel exception's error code by type</returns>
        public static int GetErrorCode(KernelException kex) =>
            GetErrorCode(kex.ExceptionType);

        /// <summary>
        /// Gets an error code for a kernel exception type
        /// </summary>
        /// <param name="type">Kernel exception type</param>
        /// <returns>Kernel exception's error code by type</returns>
        public static int GetErrorCode(KernelExceptionType type) =>
            10000 + (int)type;
    }
}
