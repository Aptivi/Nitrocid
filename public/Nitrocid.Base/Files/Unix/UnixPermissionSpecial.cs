//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

namespace Nitrocid.Base.Files.Unix
{
    /// <summary>
    /// Unix special permissions
    /// </summary>
    public enum UnixPermissionSpecial
    {
        /// <summary>
        /// No special permissions
        /// </summary>
        None,
        /// <summary>
        /// Set User ID
        /// </summary>
        SetUid,
        /// <summary>
        /// Set Group ID
        /// </summary>
        SetGid,
        /// <summary>
        /// Sticky Bit
        /// </summary>
        Sticky = 4,
    }
}
