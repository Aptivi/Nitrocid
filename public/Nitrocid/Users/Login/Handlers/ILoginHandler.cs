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

namespace Nitrocid.Users.Login.Handlers
{
    /// <summary>
    /// Login handler interface
    /// </summary>
    public interface ILoginHandler
    {
        /// <summary>
        /// Login screen
        /// </summary>
        /// <returns>True if we need to proceed to the user selector. Otherwise, false.</returns>
        bool LoginScreen();

        /// <summary>
        /// Username selector
        /// </summary>
        /// <returns>The proposed username to log into</returns>
        string UserSelector();

        /// <summary>
        /// Password handler
        /// </summary>
        /// <returns>True if the password if valid. Otherwise, false. Please be honest here, because we don't want failed logins to appear as successful.</returns>
        bool PasswordHandler(string user, ref string pass);
    }
}
