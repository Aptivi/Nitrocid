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
using System.Diagnostics;

namespace Nitrocid.Base.Kernel.Configuration.Settings
{
    /// <summary>
    /// Settings function arguments
    /// </summary>
    [DebuggerDisplay("[{ArgValue}] Type = {ArgType}")]
    public class SettingsFunctionArgs
    {
        [JsonProperty(nameof(ArgType))]
        internal string argType = "";
        [JsonProperty(nameof(ArgValue))]
        internal string argValue = "";

        /// <summary>
        /// Settings function argument type
        /// </summary>
        [JsonIgnore]
        public string ArgType =>
            argType;

        /// <summary>
        /// Settings function argument value
        /// </summary>
        [JsonIgnore]
        public string ArgValue =>
            argValue;

        [JsonConstructor]
        internal SettingsFunctionArgs()
        { }
    }
}
