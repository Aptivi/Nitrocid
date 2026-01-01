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
using System.Diagnostics;

namespace Nitrocid.Kernel.Configuration.Settings
{
    /// <summary>
    /// Settings entry class
    /// </summary>
    [DebuggerDisplay("Name = {Name}, Keys = {Keys.Length}, Display = {DisplayAs}, Desc = {Desc}")]
    public class SettingsEntry
    {
        [JsonProperty(nameof(Name))]
        internal string name = "";
        [JsonProperty(nameof(Desc))]
        internal string desc = "";
        [JsonProperty(nameof(DisplayAs))]
        internal string displayAs = "";
        [JsonProperty(nameof(Keys))]
        internal SettingsKey[]? keys;

        /// <summary>
        /// Settings entry name
        /// </summary>
        [JsonIgnore]
        public string Name =>
            name;

        /// <summary>
        /// Settings entry description
        /// </summary>
        [JsonIgnore]
        public string Desc =>
            desc;

        /// <summary>
        /// Settings entry display name
        /// </summary>
        [JsonIgnore]
        public string DisplayAs =>
            displayAs;

        /// <summary>
        /// Settings entry key list
        /// </summary>
        [JsonIgnore]
        public SettingsKey[] Keys =>
            keys ?? [];

        [JsonConstructor]
        internal SettingsEntry()
        { }
    }
}
