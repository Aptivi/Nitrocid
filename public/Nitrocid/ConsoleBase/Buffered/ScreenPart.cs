//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Text;

namespace KS.ConsoleBase.Buffered
{
    /// <summary>
    /// Buffered screen part
    /// </summary>
    public class ScreenPart
    {
        private readonly StringBuilder bufferBuilder = new();

        /// <summary>
        /// Adds a text to the buffer
        /// </summary>
        /// <param name="text">Text to write to the buffer builder</param>
        public void AddText(string text) =>
            bufferBuilder.Append(text);

        /// <summary>
        /// Adds a text to the buffer with a new line
        /// </summary>
        /// <param name="text">Text to write to the buffer builder</param>
        public void AddTextLine(string text) =>
            bufferBuilder.AppendLine(text);

        /// <summary>
        /// Clears the buffer
        /// </summary>
        public void Clear() =>
            bufferBuilder.Clear();

        /// <summary>
        /// Gets the resulting buffer
        /// </summary>
        /// <returns>The resulting buffer</returns>
        public string GetBuffer() =>
            bufferBuilder.ToString();

        /// <summary>
        /// Makes a new instance of the screen part
        /// </summary>
        public ScreenPart()
        { }
    }
}
