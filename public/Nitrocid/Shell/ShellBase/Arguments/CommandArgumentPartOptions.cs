
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

using System;

namespace KS.Shell.ShellBase.Arguments
{
    /// <summary>
    /// Command argument part options class
    /// </summary>
    public class CommandArgumentPartOptions
    {

        /// <summary>
        /// Auto completion function delegate
        /// </summary>
        public Func<string[], string[]> AutoCompleter { get; set; }
        /// <summary>
        /// Command argument expression
        /// </summary>
        public bool IsNumeric { get; set; }
        /// <summary>
        /// User is required to provide this exact wording
        /// </summary>
        public string ExactWording { get; set; }

    }
}
