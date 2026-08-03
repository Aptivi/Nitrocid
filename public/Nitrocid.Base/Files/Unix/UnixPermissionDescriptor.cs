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
    /// Unix permission descriptor class
    /// </summary>
    public class UnixPermissionDescriptor
    {
        /// <summary>
        /// Unix permission types
        /// </summary>
        public UnixPermissionType Types { get; set; }

        /// <summary>
        /// Unix permission scope
        /// </summary>
        public UnixPermissionScope Scope { get; set; }

        /// <summary>
        /// Calculates the permission number according to specified permission types
        /// </summary>
        /// <returns>Permission number</returns>
        public int Calculate() =>
            UnixPermissionManager.Calculate(Types);

        /// <summary>
        /// Unix permission descriptor
        /// </summary>
        /// <param name="permNum">Unix permission number (0 to 7)</param>
        /// <param name="scope">Unix permission scope</param>
        public UnixPermissionDescriptor(int permNum, UnixPermissionScope scope)
        {
            Types = UnixPermissionManager.GetTypeFrom(permNum);
            Scope = scope;
        }

        /// <summary>
        /// Unix permission descriptor
        /// </summary>
        /// <param name="types">Unix permission types</param>
        /// <param name="scope">Unix permission scope</param>
        public UnixPermissionDescriptor(UnixPermissionType types, UnixPermissionScope scope)
        {
            Types = types;
            Scope = scope;
        }
    }
}
