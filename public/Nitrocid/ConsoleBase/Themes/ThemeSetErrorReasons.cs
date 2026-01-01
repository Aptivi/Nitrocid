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

namespace Nitrocid.ConsoleBase.Themes
{
    /// <summary>
    /// Enumeration for the theme setting error reasons
    /// </summary>
    public enum ThemeSetErrorReasons
    {
        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Theme is not found
        /// </summary>
        NotFound = 1,
        /// <summary>
        /// Console is unsupported or terminal doesn't support true color
        /// </summary>
        ConsoleUnsupported,
        /// <summary>
        /// Trying to set a theme that is an event on a day that is before or after the event.
        /// </summary>
        EventFinished
    }
}
