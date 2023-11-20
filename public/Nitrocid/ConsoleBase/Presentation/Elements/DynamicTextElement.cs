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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Misc.Text;
using System;
using System.Linq;
using System.Text;

namespace KS.ConsoleBase.Presentation.Elements
{
    /// <summary>
    /// Dynamic text element
    /// </summary>
    public class DynamicTextElement : IElement
    {
        /// <inheritdoc/>
        public bool IsInput => false;

        /// <inheritdoc/>
        public string WrittenInput { get; set; }

        /// <summary>
        /// The first argument denotes the action which invokes the text, and the rest for the parameters to be formatted
        /// </summary>
        public object[] Arguments { get; set; }

        /// <summary>
        /// Renders the text
        /// </summary>
        public void Render()
        {
            // Get the text and the arguments
            object[] finalArgs = Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : [];
            string text = TextTools.FormatString(Arguments.Length > 0 ? ((Func<string>)Arguments[0])() : "", finalArgs);

            // Check the bounds
            string[] splitText = TextTools.GetWrappedSentences(text, PresentationTools.PresentationLowerInnerBorderLeft - PresentationTools.PresentationUpperBorderLeft + 2);
            int top = ConsoleWrapper.CursorTop;
            int seekTop = ConsoleWrapper.CursorTop;
            var buffer = new StringBuilder();
            foreach (string split in splitText)
            {
                int maxHeight = PresentationTools.PresentationLowerInnerBorderTop - top + 1;
                if (maxHeight < 0)
                {
                    // If the text is going to overflow the presentation view, clear the presentation and finish writing the parts
                    TextWriterWhereColor.WriteWhereKernelColor(buffer.ToString(), PresentationTools.PresentationUpperInnerBorderLeft, seekTop, false, KernelColorType.NeutralText);
                    Input.DetectKeypress();
                    PresentationTools.ClearPresentation();
                    seekTop = top = PresentationTools.PresentationUpperInnerBorderTop;
                    buffer.Clear();
                }

                // Write the part
                buffer.Append(split + "\n");
                top++;
            }
            TextWriterWhereColor.WriteWhereKernelColor(buffer.ToString(), PresentationTools.PresentationUpperInnerBorderLeft, seekTop, false, KernelColorType.NeutralText);
        }

        /// <summary>
        /// Checks to see if the text is possibly overflowing the slideshow display
        /// </summary>
        public bool IsPossibleOutOfBounds()
        {
            // Get the text and the arguments
            object[] finalArgs = Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : [];
            string text = TextTools.FormatString(Arguments.Length > 0 ? ((Func<string>)Arguments[0])() : "", finalArgs);

            // Check the bounds
            string[] splitText = TextTools.GetWrappedSentences(text, PresentationTools.PresentationLowerInnerBorderLeft - PresentationTools.PresentationUpperInnerBorderLeft);
            int maxHeight = PresentationTools.PresentationLowerInnerBorderTop - ConsoleWrapper.CursorTop + 3;
            return splitText.Length > maxHeight;
        }

        /// <inheritdoc/>
        public Action<object[]> InvokeActionInput { get; }

        /// <inheritdoc/>
        public Action InvokeAction { get; set; }
    }
}
