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

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nitrocid.Base.Users
{
    /// <summary>
    /// User information class
    /// </summary>
    public class UserInfo
    {

        /// <summary>
        /// The username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The full name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// The preferred language
        /// </summary>
        public string? PreferredLanguage { get; set; }
        /// <summary>
        /// The preferred culture
        /// </summary>
        public string? PreferredCulture { get; set; }
        [JsonProperty]
        internal string[] Groups { get; set; }
        [JsonProperty]
        internal string Password { get; set; }
        [JsonProperty]
        internal UserFlags Flags { get; set; }
        [JsonProperty]
        internal string[] Permissions { get; set; }
        [JsonProperty]
        internal Dictionary<string, object[]> CustomSettings { get; set; }

        /// <summary>
        /// Makes a new class instance of current user info
        /// </summary>
        [JsonConstructor]
        internal UserInfo(string username, string password, string[] permissions, string fullName, string preferredLanguage, string preferredCulture, string[] groups, UserFlags flags, Dictionary<string, object[]> customSettings)
        {
            Username = username;
            Password = password;
            Permissions = permissions;
            FullName = fullName;
            PreferredLanguage = preferredLanguage;
            PreferredCulture = preferredCulture;
            Groups = groups;
            Flags = flags;
            CustomSettings = customSettings;
        }

    }
}
