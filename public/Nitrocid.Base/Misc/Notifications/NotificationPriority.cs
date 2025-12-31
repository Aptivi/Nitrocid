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

namespace Nitrocid.Base.Misc.Notifications
{
    /// <summary>
    /// Notification priority
    /// </summary>
    public enum NotificationPriority
    {
        /// <summary>
        /// Low priority. One beep.
        /// </summary>
        Low = 1,
        /// <summary>
        /// Medium priority. Two beeps.
        /// </summary>
        Medium,
        /// <summary>
        /// High priority. Three beeps.
        /// </summary>
        High,
        /// <summary>
        /// Custom priority. Custom colors, beeps, etc.
        /// </summary>
        Custom
    }
}
