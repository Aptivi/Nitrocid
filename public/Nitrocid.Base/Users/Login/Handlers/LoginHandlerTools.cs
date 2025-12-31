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

using Nitrocid.Base.Users.Login.Handlers.Logins;
using System.Collections.Generic;

namespace Nitrocid.Base.Users.Login.Handlers
{
    /// <summary>
    /// Login handler tools
    /// </summary>
    public static class LoginHandlerTools
    {
        private static string currentHandler = "modern";
        private readonly static Dictionary<string, BaseLoginHandler> handlers = new()
        {
            { "classic", new ClassicLogin() },
            { "modern", new ModernLogin() },
        };
        private readonly static Dictionary<string, BaseLoginHandler> customHandlers = [];

        /// <summary>
        /// Gets and sets the current login handler name
        /// </summary>
        public static string CurrentHandlerName
        {
            get => IsHandlerRegistered(currentHandler) ? currentHandler : "modern";
            set => currentHandler = IsHandlerRegistered(value) ? value : "modern";
        }

        /// <summary>
        /// Gets the current login handler
        /// </summary>
        public static BaseLoginHandler? CurrentHandler =>
            GetHandler(CurrentHandlerName);

        /// <summary>
        /// Checks to see if the built-in or custom handler is registered
        /// </summary>
        /// <param name="name">Name of the built-in or custom handler</param>
        /// <returns>True if found. False otherwise.</returns>
        public static bool IsHandlerRegistered(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            return handlers.ContainsKey(name) || customHandlers.ContainsKey(name);
        }

        /// <summary>
        /// Checks to see if the handler is built-in
        /// </summary>
        /// <param name="name">Name of the built-in handler</param>
        /// <returns>True if found. False otherwise.</returns>
        public static bool IsHandlerBuiltin(string name)
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
        public static BaseLoginHandler? GetHandler(string name)
        {
            if (IsHandlerBuiltin(name))
                return handlers[name];
            if (IsHandlerRegistered(name))
                return customHandlers[name];
            return null;
        }

        /// <summary>
        /// Registers a custom login handler
        /// </summary>
        /// <param name="name">Custom login handler name</param>
        /// <param name="handler">Handler base to use</param>
        public static void RegisterHandler(string name, BaseLoginHandler handler)
        {
            if (!IsHandlerRegistered(name) && !IsHandlerBuiltin(name))
                customHandlers.Add(name, handler);
        }

        /// <summary>
        /// Unregisters a custom login handler
        /// </summary>
        /// <param name="name">Custom login handler name</param>
        public static void UnregisterHandler(string name)
        {
            if (IsHandlerRegistered(name) && !IsHandlerBuiltin(name))
                customHandlers.Remove(name);
        }

        /// <summary>
        /// Gets the handler names for built-in and custom handlers
        /// </summary>
        /// <returns>List of handler names</returns>
        public static string[] GetHandlerNames()
        {
            List<string> names = [.. handlers.Keys, .. customHandlers.Keys];
            return [.. names];
        }
    }
}
