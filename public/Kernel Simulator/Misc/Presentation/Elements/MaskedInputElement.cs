
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

using Extensification.StringExts;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TermRead.Reader;
using VT.NET;

namespace KS.Misc.Presentation.Elements
{
    /// <summary>
    /// Masked input element
    /// </summary>
    public class MaskedInputElement : IElement
    {
        /// <inheritdoc/>
        public bool IsInput => true;

        /// <inheritdoc/>
        public string WrittenInput { get; set; }

        /// <summary>
        /// The first argument denotes the prompt to be written, and the rest for the parameters to be formatted
        /// </summary>
        public object[] Arguments { get; set; }

        /// <summary>
        /// Renders the text
        /// </summary>
        public void Render()
        {
            // Get the text and the arguments
            object[] finalArgs = Arguments.Length > 1 ? Arguments.Skip(1).ToArray() : Array.Empty<object>();
            string text = ((string)(Arguments.Length > 0 ? Arguments[0] : "")).FormatString(finalArgs);

            // Split the text to lines that fit well to the console
            List<string> textLines = new();
            var lineBuilder = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                // Add the character to the builder
                lineBuilder.Append(text[i]);

                // Check to see if the length of the VT-filtered built string is equal or greater than the presentation width limit
                string built = lineBuilder.ToString();
                string vtFilteredBuilt = Filters.FilterVTSequences(built);
                if (vtFilteredBuilt.Length >= PresentationTools.PresentationLowerInnerBorderLeft ||
                    i == text.Length - 1)
                {
                    // Add the built string with VT sequences to the lines list
                    if (i == text.Length - 1)
                    {
                        built += ColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground;
                        built += ColorTools.GetColor(KernelColorType.Background).VTSequenceBackground;
                        textLines.Add(built);
                    }
                    else
                        textLines.Add(built + "\n");
                    lineBuilder.Clear();
                }
            }

            // Now, write the lines
            foreach (var line in textLines)
                TextWriterWhereColor.WriteWhere(line, PresentationTools.PresentationUpperInnerBorderLeft, Console.CursorTop);

            // Get the input
            ConsoleWrapper.CursorVisible = true;
            TermReaderSettings.LeftMargin = TermReaderSettings.RightMargin = PresentationTools.PresentationUpperInnerBorderLeft;
            WrittenInput = Input.ReadLineNoInput();
            TermReaderSettings.LeftMargin = TermReaderSettings.RightMargin = 0;
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public Action<object[]> InvokeActionInput { get; set; }

        /// <inheritdoc/>
        public Action InvokeAction { get; set; }
    }
}
