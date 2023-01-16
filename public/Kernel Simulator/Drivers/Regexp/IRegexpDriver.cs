
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

namespace KS.Drivers.Regexp
{
    /// <summary>
    /// Regexp driver interface for drivers
    /// </summary>
    public interface IRegexpDriver : IDriver
    {
        /// <summary>
        /// Check to see if the provided pattern is a valid Regex pattern
        /// </summary>
        /// <param name="pattern">Regular expression pattern</param>
        /// <returns>True if valid. Otherwise, false</returns>
        bool IsValidRegex(string pattern);

        /// <summary>
        /// Checks to see whether the pattern matches
        /// </summary>
        /// <param name="text">The text to be matched</param>
        /// <param name="pattern">Regular expression pattern for matching</param>
        /// <returns>True if there are matches. Otherwise, false</returns>
        bool IsMatch(string text, string pattern);
    }
}
