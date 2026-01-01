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

namespace Nitrocid.Kernel.Journaling
{
    /// <summary>
    /// A kernel journal entry class
    /// </summary>
    public class JournalEntry
    {
        [JsonProperty(PropertyName = "date")]
        internal string date = string.Empty;
        [JsonProperty(PropertyName = "time")]
        internal string time = string.Empty;
        [JsonProperty(PropertyName = "status")]
        internal string status = string.Empty;
        [JsonProperty(PropertyName = "message")]
        internal string message = string.Empty;

        /// <summary>
        /// Specifies the date of the journal
        /// </summary>
        [JsonIgnore]
        public string Date =>
            date ?? string.Empty;
        /// <summary>
        /// Specifies the time of the journal
        /// </summary>
        [JsonIgnore]
        public string Time =>
            time ?? string.Empty;
        /// <summary>
        /// Specifies the status of the journal
        /// </summary>
        [JsonIgnore]
        public string Status =>
            status ?? string.Empty;
        /// <summary>
        /// Specifies the message of the journal
        /// </summary>
        [JsonIgnore]
        public string Message =>
            message ?? string.Empty;

        [JsonConstructor]
        internal JournalEntry()
        { }
    }
}
