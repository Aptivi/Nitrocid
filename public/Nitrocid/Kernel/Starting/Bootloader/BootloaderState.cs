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

namespace Nitrocid.Kernel.Starting.Bootloader
{
    /// <summary>
    /// Stores the bootloader state
    /// </summary>
    public static class BootloaderState
    {
        internal static bool waitingForBootKey = true;
        internal static bool waitingForFirstBootKey = true;

        /// <summary>
        /// Specifies whether the bootloader is waiting for any key to be pressed
        /// </summary>
        public static bool WaitingForBootKey =>
            waitingForBootKey;

        /// <summary>
        /// Specifies whether the bootloader is waiting for any key to be pressed for the first time
        /// </summary>
        public static bool WaitingForFirstBootKey =>
            waitingForFirstBootKey;
    }
}
