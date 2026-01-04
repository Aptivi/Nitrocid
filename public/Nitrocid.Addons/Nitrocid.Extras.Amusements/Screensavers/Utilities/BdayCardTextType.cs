//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

namespace Nitrocid.Extras.Amusements.Screensavers.Utilities
{
    /// <summary>
    /// Birthday card text type
    /// </summary>
    public enum BdayCardTextType
    {
        /// <summary>
        /// Simple birthday wishes to a person or implicitly
        /// <br></br>
        /// <br></br>
        /// <list type="bullet">
        /// <item><see cref="BdayCardGender.Male"/>: “Happy birthday to him!”</item>
        /// <item><see cref="BdayCardGender.Female"/>: “Happy birthday to her!”</item>
        /// <item><see cref="BdayCardGender.Unspecific"/>: “Happy birthday to the most loyal one!”</item>
        /// </list>
        /// “Happy birthday, &lt;NAME&gt;!”
        /// </summary>
        Simple,
        /// <summary>
        /// Superlative birthday wishes to a person or implicitly
        /// <br></br>
        /// <br></br>
        /// <list type="bullet">
        /// <item><see cref="BdayCardGender.Male"/>: “Happiest birthday to him!”</item>
        /// <item><see cref="BdayCardGender.Female"/>: “Happiest birthday to her!”</item>
        /// <item><see cref="BdayCardGender.Unspecific"/>: “Happiest birthday to the most loyal one!”</item>
        /// </list>
        /// “Happiest birthday, &lt;NAME&gt;!”
        /// </summary>
        Superlative,
        /// <summary>
        /// Superlative birthday wishes to a person or implicitly (alternative)
        /// <br></br>
        /// <br></br>
        /// <list type="bullet">
        /// <item><see cref="BdayCardGender.Male"/>: “Happiest one to him!”</item>
        /// <item><see cref="BdayCardGender.Female"/>: “Happiest one to her!”</item>
        /// <item><see cref="BdayCardGender.Unspecific"/>: “Happiest one to the most loyal one!”</item>
        /// </list>
        /// “Happiest one, &lt;NAME&gt;!”
        /// </summary>
        SuperAlt,
    }
}
