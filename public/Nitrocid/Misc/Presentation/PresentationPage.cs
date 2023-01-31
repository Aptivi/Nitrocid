
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

using KS.Languages;
using KS.Misc.Presentation.Elements;
using System.Collections.Generic;

namespace KS.Misc.Presentation
{
    /// <summary>
    /// Presentation page
    /// </summary>
    public class PresentationPage
    {
        /// <summary>
        /// Presentation page name
        /// </summary>
        public string Name { get; } = Translate.DoTranslation("Untitled presentation page");

        /// <summary>
        /// Presentation page elements
        /// </summary>
        public List<IElement> Elements { get; }

        /// <summary>
        /// Makes a new presentation page
        /// </summary>
        /// <param name="name">Page name</param>
        /// <param name="elements">List of elements</param>
        public PresentationPage(string name, List<IElement> elements)
        {
            Name = name;
            Elements = elements;
        }
    }
}
