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

namespace Nitrocid.Base.Users.Login
{
    /// <summary>
    /// Enumeration for the login error reasons
    /// </summary>
    public enum LoginErrorReasons
    {
        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Spaces not allowed on username
        /// </summary>
        Spaces = 1,
        /// <summary>
        /// Special characters not allowed on username
        /// </summary>
        SpecialCharacters = 2,
        /// <summary>
        /// User is disabled
        /// </summary>
        Disabled = 4,
        /// <summary>
        /// User is not found
        /// </summary>
        NotFound = 8,
        /// <summary>
        /// User entered the wrong password
        /// </summary>
        WrongPassword = 16
    }
}
