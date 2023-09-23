
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

using KS.Drivers;

namespace KS.Misc.Text.Probers.Regexp
{
    /// <summary>
    /// All tools for the regular expressions
    /// </summary>
    public static class RegexpTools
    {
        /// <summary>
        /// Check to see if the provided pattern is a valid Regex pattern
        /// </summary>
        /// <param name="pattern">Regular expression pattern</param>
        /// <returns>True if valid. Otherwise, false</returns>
        public static bool IsValidRegex(string pattern) =>
            DriverHandler.CurrentRegexpDriverLocal.IsValidRegex(pattern);

        /// <summary>
        /// Checks to see whether the pattern matches
        /// </summary>
        /// <param name="text">The text to be matched</param>
        /// <param name="pattern">Regular expression pattern for matching</param>
        /// <returns>True if there are matches. Otherwise, false</returns>
        public static bool IsMatch(string text, string pattern) =>
            DriverHandler.CurrentRegexpDriverLocal.IsMatch(text, pattern);
    }
}
