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

extern alias TextifyDep;

using TextifyDep::System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Nitrocid.Base.Drivers.Regexp
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
        bool IsValidRegex([StringSyntax(StringSyntaxAttribute.Regex)] string pattern);

        /// <summary>
        /// Checks to see whether the pattern matches
        /// </summary>
        /// <param name="text">The text to be matched</param>
        /// <param name="pattern">Regular expression pattern for matching</param>
        /// <returns>True if there are matches. Otherwise, false</returns>
        bool IsMatch(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern);

        /// <summary>
        /// Matches the pattern with the text given
        /// </summary>
        /// <param name="text">The text to be matched</param>
        /// <param name="pattern">Regular expression pattern for matching</param>
        /// <returns>A <see cref="System.Text.RegularExpressions.Match"/> that contains information about the current match</returns>
        Match Match(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern);

        /// <summary>
        /// Matches the pattern with the text given
        /// </summary>
        /// <param name="text">The text to be matched</param>
        /// <param name="pattern">Regular expression pattern for matching</param>
        /// <returns>Collection of <see cref="System.Text.RegularExpressions.Match"/>es that contains information about the current match</returns>
        MatchCollection Matches(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern);

        /// <summary>
        /// Filters the string from the substrings matched by the given pattern
        /// </summary>
        /// <param name="text">The text to be processed</param>
        /// <param name="pattern">Regular expression pattern for replacing</param>
        /// <returns>Filtered text</returns>
        string Filter(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern);

        /// <summary>
        /// Filters the string from the substrings matched by the given pattern
        /// </summary>
        /// <param name="text">The text to be processed</param>
        /// <param name="pattern">Regular expression pattern for replacing</param>
        /// <param name="replaceWith">Replaces the matched substrings with the specified text</param>
        /// <returns>Filtered text</returns>
        string Filter(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern, string replaceWith);

        /// <summary>
        /// Splits the string using the matched substrings as the delimiters
        /// </summary>
        /// <param name="text">The text to be split</param>
        /// <param name="pattern">Regular expression pattern for splitting</param>
        /// <returns>Array of strings</returns>
        string[] Split(string text, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern);

        /// <summary>
        /// Escapes the invalid characters from the string
        /// </summary>
        /// <param name="text">The text containing invalid characters to escape</param>
        /// <returns>Escaped string</returns>
        string Escape(string text);

        /// <summary>
        /// Unescapes the escaped characters from the string
        /// </summary>
        /// <param name="text">The text containing escaped characters to unescape</param>
        /// <returns>Unescaped string</returns>
        string Unescape(string text);
    }
}
