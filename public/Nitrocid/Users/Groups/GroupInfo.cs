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

using Newtonsoft.Json;

namespace Nitrocid.Users.Groups
{
    /// <summary>
    /// Group information class
    /// </summary>
    public class GroupInfo
    {

        /// <summary>
        /// The group name
        /// </summary>
        public string GroupName { get; set; }
        [JsonProperty]
        internal string[] Permissions { get; set; }

        /// <summary>
        /// Makes a new class instance of current group info
        /// </summary>
        [JsonConstructor]
        internal GroupInfo(string name, string[] permissions)
        {
            GroupName = name;
            Permissions = permissions;
        }

    }
}
