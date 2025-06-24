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

namespace Nitrocid.Base.Kernel.Journaling
{
    /// <summary>
    /// Journaling status to write to the kernel journal
    /// </summary>
    public enum JournalStatus
    {
        /// <summary>
        /// A fatal error occurred
        /// </summary>
        Fatal,
        /// <summary>
        /// An error occurred
        /// </summary>
        Error,
        /// <summary>
        /// Warning message
        /// </summary>
        Warning,
        /// <summary>
        /// Informational message
        /// </summary>
        Info
    }
}
