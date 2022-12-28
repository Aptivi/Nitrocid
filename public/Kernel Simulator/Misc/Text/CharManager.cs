
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

using System;
using System.Data;
using System.Linq;

namespace KS.Misc.Text
{
    /// <summary>
    /// Character querying and management module
    /// </summary>
    public static class CharManager
    {

        /// <summary>
        /// New line constant
        /// </summary>
        public static string NewLine { get => Environment.NewLine; }

        /// <summary>
        /// Gets all the letters and the numbers.
        /// </summary>
        public static char[] GetAllLettersAndNumbers() =>
            Enumerable.Range(0, Convert.ToInt32(char.MaxValue) + 1).Select(CharNum => Convert.ToChar(CharNum)).Where(c => char.IsLetterOrDigit(c)).ToArray();

        /// <summary>
        /// Gets all the letters.
        /// </summary>
        public static char[] GetAllLetters() => 
            Enumerable.Range(0, Convert.ToInt32(char.MaxValue) + 1).Select(CharNum => Convert.ToChar(CharNum)).Where(c => char.IsLetter(c)).ToArray();

        /// <summary>
        /// Gets all the numbers.
        /// </summary>
        public static char[] GetAllNumbers() => 
            Enumerable.Range(0, Convert.ToInt32(char.MaxValue) + 1).Select(CharNum => Convert.ToChar(CharNum)).Where(c => char.IsNumber(c)).ToArray();

        /// <summary>
        /// A simplification for <see cref="Convert.ToChar(int)"/> function to return the ESC character
        /// </summary>
        /// <returns>ESC</returns>
        public static char GetEsc() => 
            Convert.ToChar(0x1B);

        /// <summary>
        /// Is the character a real control character
        /// </summary>
        /// <param name="ch">Character to query</param>
        public static bool IsControlChar(char ch) =>
            // If the character is greater than the NULL character and less than the BACKSPACE character, or
            // if the character is greater than the CARRIAGE RETURN character and less than the SUBSTITUTE character,
            // it's a real control character.
            (ch > (char)0 && ch < (char)8) || (ch > (char)13 && ch < (char)26);

    }
}
