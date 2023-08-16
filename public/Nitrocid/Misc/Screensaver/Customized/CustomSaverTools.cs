
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

using System.Collections.Generic;
using KS.Kernel.Exceptions;
using KS.Languages;

namespace KS.Misc.Screensaver.Customized
{
    /// <summary>
    /// Custom screensaver tools
    /// </summary>
    public static class CustomSaverTools
    {

        internal static Dictionary<string, BaseScreensaver> CustomSavers = new();

        /// <summary>
        /// Registers a custom screensaver
        /// </summary>
        /// <param name="name">Screensaver name to register</param>
        /// <param name="screensaver">Base screensaver containing custom screensaver</param>
        public static void RegisterCustomScreensaver(string name, BaseScreensaver screensaver)
        {
            if (Screensaver.IsScreensaverRegistered(name))
                throw new KernelException(KernelExceptionType.ScreensaverManagement, Translate.DoTranslation("Custom screensaver already exists."));

            // Add a custom screensaver to the list of available screensavers.
            CustomSavers.Add(name.ToLower(), screensaver);
        }

        /// <summary>
        /// Unregisters a custom screensaver
        /// </summary>
        /// <param name="name">Screensaver name to unregister</param>
        public static void UnregisterCustomScreensaver(string name)
        {
            if (!Screensaver.IsScreensaverRegistered(name))
                throw new KernelException(KernelExceptionType.ScreensaverManagement, Translate.DoTranslation("Custom screensaver not found."));

            // Remove a custom screensaver from the list of available screensavers.
            CustomSavers.Remove(name.ToLower());
        }

    }
}
