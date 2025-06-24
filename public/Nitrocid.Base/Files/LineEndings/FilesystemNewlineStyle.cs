//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

namespace Nitrocid.Base.Files.LineEndings
{
    /// <summary>
    /// Line ending styles
    /// </summary>
    public enum FilesystemNewlineStyle
    {
        /// <summary>
        /// Carriage return character. Used in Mac OS 9 or earlier.
        /// </summary>
        CR,
        /// <summary>
        /// Line feed character. Used in Linux, Unix, macOS, ...
        /// </summary>
        LF,
        /// <summary>
        /// Carriage return + line feed character. Used in Windows.
        /// </summary>
        CRLF
    }
}