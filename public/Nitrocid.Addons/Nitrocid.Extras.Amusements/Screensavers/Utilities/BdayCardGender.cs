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
    /// Birthday card gender
    /// </summary>
    public enum BdayCardGender
    {
        /// <summary>
        /// The birthday person is a male
        /// <br></br>
        /// <br></br>
        /// <list type="bullet">
        /// <item><see cref="BdayCardTextType.Simple"/>: “Happy birthday to him!”</item>
        /// <item><see cref="BdayCardTextType.Superlative"/>: “Happiest birthday to him!”</item>
        /// <item><see cref="BdayCardTextType.SuperAlt"/>: “Happiest one to him!”</item>
        /// </list>
        /// </summary>
        Male,
        /// <summary>
        /// The birthday person is a female
        /// <br></br>
        /// <br></br>
        /// <list type="bullet">
        /// <item><see cref="BdayCardTextType.Simple"/>: “Happy birthday to her!”</item>
        /// <item><see cref="BdayCardTextType.Superlative"/>: “Happiest birthday to her!”</item>
        /// <item><see cref="BdayCardTextType.SuperAlt"/>: “Happiest one to her!”</item>
        /// </list>
        /// </summary>
        Female,
        /// <summary>
        /// Other gender, or prefer not to reveal
        /// <br></br>
        /// <br></br>
        /// <list type="bullet">
        /// <item><see cref="BdayCardTextType.Simple"/>: “Happy birthday to the most loyal one!”</item>
        /// <item><see cref="BdayCardTextType.Superlative"/>: “Happiest birthday to the most loyal one!”</item>
        /// <item><see cref="BdayCardTextType.SuperAlt"/>: “Happiest one to the most loyal one!”</item>
        /// </list>
        /// </summary>
        Unspecific,
    }
}
