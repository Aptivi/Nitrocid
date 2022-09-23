
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

namespace KS.Drivers.Encryption
{
    /// <summary>
    /// Built-in encryption algorithms
    /// </summary>
    public enum EncryptionAlgorithms
    {
        /// <summary>
        /// The MD5 Algorithm
        /// </summary>
        MD5,
        /// <summary>
        /// The SHA1 Algorithm
        /// </summary>
        SHA1,
        /// <summary>
        /// The SHA256 Algorithm
        /// </summary>
        SHA256,
        /// <summary>
        /// The SHA384 Algorithm
        /// </summary>
        SHA384,
        /// <summary>
        /// The SHA512 Algorithm
        /// </summary>
        SHA512,
        /// <summary>
        /// The CRC32 Algorithm
        /// </summary>
        CRC32
    }
}
