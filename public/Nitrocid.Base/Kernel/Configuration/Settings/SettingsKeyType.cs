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

using Terminaux.Shell.Prompts;
using System.Collections.Generic;
using Terminaux.Colors;

namespace Nitrocid.Base.Kernel.Configuration.Settings
{
    /// <summary>
    /// Key type for settings entry
    /// </summary>
    public enum SettingsKeyType
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
        /// The value is of <see cref="Color"/> and comes from the color wheel
        /// </summary>
        SColor,
        /// <summary>
        /// The value is of <see cref="char"/> and only accepts one character.
        /// </summary>
        SChar,
        /// <summary>
        /// The value is of <see cref="int"/>, but has a slider which has a minimum and maximum value. Useful for numbers which are limited.
        /// </summary>
        SIntSlider,
        /// <summary>
        /// The value is of <see cref="double"/>
        /// </summary>
        SDouble,
        /// <summary>
        /// The value is a shell preset defined using <see cref="IPromptPreset"/>
        /// </summary>
        SPreset,
        /// <summary>
        /// The value is a Figlet font name that allows Settings to use the Figlet font selector.
        /// </summary>
        SFiglet,
        /// <summary>
        /// The value is an icon font name that allows Settings to use the icon selector.
        /// </summary>
        SIcon,
        /// <summary>
        /// The value consists of multiple variables.
        /// </summary>
        SMultivar,
    }
}
