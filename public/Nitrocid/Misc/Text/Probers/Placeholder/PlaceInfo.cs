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

using System;

namespace Nitrocid.Misc.Text.Probers.Placeholder
{
    /// <summary>
    /// Placeholder information class
    /// </summary>
    public class PlaceInfo
    {
        private readonly string placeholder;
        private readonly Func<string, string> placeholderAction = (_) => "null";
        private readonly string placeholderDefaultValue = "null";

        /// <summary>
        /// Placeholder specifier
        /// </summary>
        public string Placeholder =>
            placeholder;

        /// <summary>
        /// Placeholder action
        /// </summary>
        public Func<string, string> PlaceholderAction =>
            placeholderAction;

        /// <summary>
        /// Default value for the placeholder
        /// </summary>
        public string PlaceholderDefaultValue =>
            placeholderDefaultValue;

        /// <summary>
        /// Makes a new instance of the placeholder information class
        /// </summary>
        /// <param name="placeholder">Placeholder specifier</param>
        /// <param name="placeholderAction">Placeholder action</param>
        /// <param name="placeholderDefaultValue">Default value for the placeholder</param>
        public PlaceInfo(string placeholder, Func<string, string> placeholderAction, string placeholderDefaultValue = "null")
        {
            this.placeholder = placeholder;
            this.placeholderAction = placeholderAction ?? new((_) => "null");
            this.placeholderDefaultValue = placeholderDefaultValue;
        }
    }
}
