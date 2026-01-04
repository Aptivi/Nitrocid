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
    /// Birthday card naming type
    /// </summary>
    public enum BdayCardNameType
    {
        /// <summary>
        /// Random first name, combined with the text type, will appear as:
        /// <br></br>
        /// <br></br>
        /// <list type="bullet">
        /// <item><see cref="BdayCardTextType.Simple"/>: “Happy birthday, &lt;NAME&gt;!”</item>
        /// <item><see cref="BdayCardTextType.Superlative"/>: “Happiest birthday, &lt;NAME&gt;!”</item>
        /// <item><see cref="BdayCardTextType.SuperAlt"/>: “Happiest one, &lt;NAME&gt;!”</item>
        /// </list>
        /// </summary>
        Random,
        /// <summary>
        /// First name specified by user, combined with the text type, will appear as:
        /// <br></br>
        /// <br></br>
        /// <list type="bullet">
        /// <item><see cref="BdayCardTextType.Simple"/>: “Happy birthday, &lt;NAME&gt;!”</item>
        /// <item><see cref="BdayCardTextType.Superlative"/>: “Happiest birthday, &lt;NAME&gt;!”</item>
        /// <item><see cref="BdayCardTextType.SuperAlt"/>: “Happiest one, &lt;NAME&gt;!”</item>
        /// </list>
        /// </summary>
        User,
        /// <summary>
        /// Without mentioning a name (only the pronoun), birthday wishes, combined with the text type, will appear as:
        /// <br></br>
        /// <br></br>
        /// <list type="bullet">
        /// <item><see cref="BdayCardTextType.Simple"/> with <see cref="BdayCardGender.Male"/>: “Happy birthday to him!”</item>
        /// <item><see cref="BdayCardTextType.Superlative"/> with <see cref="BdayCardGender.Male"/>: “Happiest birthday to him!”</item>
        /// <item><see cref="BdayCardTextType.SuperAlt"/> with <see cref="BdayCardGender.Male"/>: “Happiest one to him!”</item>
        /// <item><see cref="BdayCardTextType.Simple"/> with <see cref="BdayCardGender.Female"/>: “Happy birthday to her!”</item>
        /// <item><see cref="BdayCardTextType.Superlative"/> with <see cref="BdayCardGender.Female"/>: “Happiest birthday to her!”</item>
        /// <item><see cref="BdayCardTextType.SuperAlt"/> with <see cref="BdayCardGender.Female"/>: “Happiest one to her!”</item>
        /// <item><see cref="BdayCardTextType.Simple"/> with <see cref="BdayCardGender.Unspecific"/>: “Happy birthday to the most loyal one!”</item>
        /// <item><see cref="BdayCardTextType.Superlative"/> with <see cref="BdayCardGender.Unspecific"/>: “Happiest birthday to the most loyal one!”</item>
        /// <item><see cref="BdayCardTextType.SuperAlt"/> with <see cref="BdayCardGender.Unspecific"/>: “Happiest one to the most loyal one!”</item>
        /// </list>
        /// </summary>
        Implicit,
    }
}
