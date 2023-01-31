
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
    /// Input element
    /// </summary>
    public class InputElement : IElement
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
            TextWriterWhereColor.WriteWhere(text, PresentationTools.PresentationUpperInnerBorderLeft, Console.CursorTop, false, PresentationTools.PresentationUpperInnerBorderLeft);

            // Get the input
            ConsoleWrapper.CursorVisible = true;
            TermReaderSettings.LeftMargin = TermReaderSettings.RightMargin = PresentationTools.PresentationUpperInnerBorderLeft;
            WrittenInput = Input.ReadLine();
            TermReaderSettings.LeftMargin = TermReaderSettings.RightMargin = 0;
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public Action<object[]> InvokeActionInput { get; set; }

        /// <inheritdoc/>
        public Action InvokeAction { get; set; }
    }
}
