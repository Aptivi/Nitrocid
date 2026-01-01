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

using System.Globalization;

namespace Nitrocid.Kernel.Time.Calendars
{
    /// <summary>
    /// Calendar interface
    /// </summary>
    public interface ICalendar
    {
        /// <summary>
        /// Calendar name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Culture information class associated with the calendar
        /// </summary>
        CultureInfo Culture { get; }
        /// <summary>
        /// Calendar information class associated with this calendar instance (for direct access)
        /// </summary>
        Calendar Calendar { get; }
    }
}
