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

namespace Nitrocid.Base.Misc.Splash
{
    /// <summary>
    /// Context of the splash
    /// </summary>
    public enum SplashContext
    {
        /// <summary>
        /// The kernel is showcasing a splash screen
        /// </summary>
        Showcase,
        /// <summary>
        /// The kernel is starting up
        /// </summary>
        StartingUp,
        /// <summary>
        /// The kernel is shutting down
        /// </summary>
        ShuttingDown,
        /// <summary>
        /// The kernel is rebooting
        /// </summary>
        Rebooting,
        /// <summary>
        /// The kernel is on the preboot stage
        /// </summary>
        Preboot,
    }
}
