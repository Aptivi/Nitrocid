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

using System.Globalization;

namespace Nitrocid.Base.Kernel.Time.Calendars
{
    /// <summary>
    /// System calendar class
    /// </summary>
    public abstract class BaseCalendar : ICalendar
    {
        /// <inheritdoc/>
        public virtual string Name =>
            "Gregorian";

        /// <inheritdoc/>
        public virtual CultureInfo Culture =>
            new("en-US");

        /// <inheritdoc/>
        public virtual Calendar Calendar =>
            Culture.DateTimeFormat.Calendar;
    }
}
