
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
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VT.NET;

namespace KS.Misc.Presentation.Elements
{
    /// <summary>
    /// Text element
    /// </summary>
    public class TextElement : IElement
    {
        /// <inheritdoc/>
        public bool IsInput => false;

        /// <inheritdoc/>
        public string WrittenInput { get; set; }

        /// <summary>
        /// The first argument denotes the text to be written, and the rest for the parameters to be formatted
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
            TextWriterWhereColor.WriteWhere(text + "\n", PresentationTools.PresentationUpperInnerBorderLeft, Console.CursorTop, false, PresentationTools.PresentationUpperInnerBorderLeft);
        }

        /// <inheritdoc/>
        public Action<object[]> InvokeActionInput { get; }

        /// <inheritdoc/>
        public Action InvokeAction { get; set; }
    }
}
