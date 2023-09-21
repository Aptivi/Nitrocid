
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

using KS.Users.Login.Handlers.Logins;
using System.Collections.Generic;

namespace KS.Users.Login.Handlers
{
    /// <summary>
    /// Login handler tools
    /// </summary>
    public static class LoginHandlerTools
    {
        private readonly static Dictionary<string, BaseLoginHandler> handlers = new()
        {
            { "classic", new ClassicLogin() },
            { "modern", new ModernLogin() },
        };

        /// <summary>
        /// Checks to see if the built-in or custom handler is registered
        /// </summary>
        /// <param name="name">Name of the built-in or custom handler</param>
        /// <returns>True if found. False otherwise.</returns>
        public static bool IsHandlerRegistered(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            return handlers.ContainsKey(name);
        }

        /// <summary>
        /// Gets a handler from the name
        /// </summary>
        /// <param name="name">Name of the built-in or custom handler</param>
        /// <returns>A <see cref="BaseLoginHandler"/> instance containing handler data if found. Otherwise, null is returned.</returns>
        public static BaseLoginHandler GetHandler(string name)
        {
            if (IsHandlerRegistered(name))
                return handlers[name];
            return null;
        }

        // TODO: Add handler registration/unregistration mechanics.
    }
}
