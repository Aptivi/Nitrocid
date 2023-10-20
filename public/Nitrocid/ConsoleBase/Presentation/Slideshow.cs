
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
using System.Collections.Generic;

namespace KS.ConsoleBase.Presentation
{
    /// <summary>
    /// The presentation containing all the pages
    /// </summary>
    public class Slideshow
    {
        /// <summary>
        /// Presentation name
        /// </summary>
        public string Name { get; } = Translate.DoTranslation("Untitled presentation");

        /// <summary>
        /// Presentation pages
        /// </summary>
        public List<PresentationPage> Pages { get; }

        /// <summary>
        /// Makes a new presentation
        /// </summary>
        /// <param name="name">Presentation name</param>
        /// <param name="pages">Presentation pages</param>
        public Slideshow(string name, List<PresentationPage> pages)
        {
            Name = name;
            Pages = pages;
        }
    }
}
