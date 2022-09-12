
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using ColorSeq;
using System.Collections.Generic;

namespace KS.Misc.Settings
{
    /// <summary>
    /// Key type for settings entry
    /// </summary>
    enum SettingsKeyType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        SUnknown,
        /// <summary>
        /// The value is of <see cref="bool"/>
        /// </summary>
        SBoolean,
        /// <summary>
        /// The value is of <see cref="int"/>
        /// </summary>
        SInt,
        /// <summary>
        /// The value is of <see cref="string"/>
        /// </summary>
        SString,
        /// <summary>
        /// The value is of the selection, which can either come from enums, or from <see cref="IEnumerable{T}"/>, like <see cref="List{T}"/>
        /// </summary>
        SSelection,
        /// <summary>
        /// The value is of <see cref="IEnumerable{T}"/>, like <see cref="List{T}"/>
        /// </summary>
        SList,
        /// <summary>
        /// The value is variant and comes from a function
        /// </summary>
        SVariant,
        /// <summary>
        /// The value is of <see cref="Color"/> and comes from the color wheel
        /// </summary>
        SColor,
        /// <summary>
        /// The value is of <see cref="string"/>, but masked. Useful for passwords.
        /// </summary>
        SMaskedString,
        /// <summary>
        /// The value is of <see cref="char"/> and only accepts one character.
        /// </summary>
        SChar,
        /// <summary>
        /// The value is of <see cref="int"/>, but has a slider which has a minimum and maximum value. Useful for numbers which are limited.
        /// </summary>
        SIntSlider
    }
}
