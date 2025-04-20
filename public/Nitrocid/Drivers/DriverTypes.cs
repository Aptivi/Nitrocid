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

namespace Nitrocid.Drivers
{
    /// <summary>
    /// Driver types
    /// </summary>
    public enum DriverTypes
    {
        /// <summary>
        /// Random number generator drivers
        /// </summary>
        RNG,
        /// <summary>
        /// Console drivers
        /// </summary>
        Console,
        /// <summary>
        /// Network drivers
        /// </summary>
        Network,
        /// <summary>
        /// Filesystem drivers
        /// </summary>
        Filesystem,
        /// <summary>
        /// Encryption drivers
        /// </summary>
        Encryption,
        /// <summary>
        /// Regular expression drivers
        /// </summary>
        Regexp,
        /// <summary>
        /// Debug logging drivers
        /// </summary>
        DebugLogger,
        /// <summary>
        /// Symmetric encoding drivers
        /// </summary>
        Encoding,
        /// <summary>
        /// Hardware prober drivers
        /// </summary>
        HardwareProber,
        /// <summary>
        /// Array sorting drivers
        /// </summary>
        Sorting,
        /// <summary>
        /// Console input drivers
        /// </summary>
        Input,
        /// <summary>
        /// Asymmetric encoding drivers
        /// </summary>
        EncodingAsymmetric,
    }
}
