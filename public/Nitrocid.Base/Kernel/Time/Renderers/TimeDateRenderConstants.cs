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

namespace Nitrocid.Base.Kernel.Time.Renderers
{
    /// <summary>
    /// Rendering constants to help you get started with formatting date and time
    /// </summary>
    public static class TimeDateRenderConstants
    {
        /// <summary>
        /// Full time format. Days, hours, minutes, seconds, and parts of second included.
        /// </summary>
        /// <example>
        /// 1.12:00:05.482
        /// </example>
        public const string FullTimeFormat = @"d\.hh\:mm\:ss\.fff";
        /// <summary>
        /// Short time format. Hours, minutes, and seconds included.
        /// </summary>
        /// <example>
        /// 12:00:05
        /// </example>
        public const string ShortTimeFormat = @"hh\:mm\:ss";
        /// <summary>
        /// Minus sign offset for timezone and time remaining indicators
        /// </summary>
        public const string MinusSignOffset = @"\-";
        /// <summary>
        /// Plus sign offset for timezone and time remaining indicators
        /// </summary>
        public const string PlusSignOffset = @"\+";
    }
}
