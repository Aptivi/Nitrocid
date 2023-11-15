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

using KS.Kernel.Exceptions;
using KS.Languages;
using System.Collections.Generic;
using System.Text;

namespace KS.ConsoleBase.Buffered
{
    /// <summary>
    /// A screen instance to store your buffered screen parts
    /// </summary>
    public class Screen
    {
        private readonly List<ScreenPart> screenParts = [];

        /// <summary>
        /// Buffered screen parts list to render one by one while buffering the console
        /// </summary>
        public ScreenPart[] ScreenParts =>
            screenParts.ToArray();

        /// <summary>
        /// Adds the buffered part to the list of screen parts
        /// </summary>
        /// <param name="part">Buffered screen part to add to the screen part list for buffering</param>
        /// <exception cref="KernelException"></exception>
        public void AddBufferedPart(ScreenPart part)
        {
            if (part is null)
                throw new KernelException(KernelExceptionType.Console, Translate.DoTranslation("You must specify the screen part."));

            // Now, add the buffered part
            screenParts.Add(part);
        }

        /// <summary>
        /// Removes the buffered part from the list of screen parts
        /// </summary>
        /// <param name="part">Buffered screen part to add to the screen part list for buffering</param>
        /// <exception cref="KernelException"></exception>
        public void RemoveBufferedPart(ScreenPart part)
        {
            if (part is null)
                throw new KernelException(KernelExceptionType.Console, Translate.DoTranslation("You must specify the screen part."));

            // Now, remove the buffered part
            screenParts.Remove(part);
        }

        /// <summary>
        /// Removes all the buffered parts from the list of screen parts
        /// </summary>
        public void RemoveBufferedParts() =>
            screenParts.Clear();

        /// <summary>
        /// Gets a buffer from all the buffered screen parts
        /// </summary>
        /// <returns>A buffer that is to be written to the console</returns>
        public string GetBuffer()
        {
            var builder = new StringBuilder();
            foreach (var part in ScreenParts)
                builder.Append(part.GetBuffer());
            return builder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the screen
        /// </summary>
        public Screen()
        { }
    }
}
