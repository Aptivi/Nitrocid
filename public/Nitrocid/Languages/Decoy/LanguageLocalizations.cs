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

namespace Nitrocid.Languages.Decoy
{
    internal class LanguageLocalizations
    {
        [JsonProperty(nameof(Name))]
        private readonly string name;
        [JsonProperty(nameof(Transliterable))]
        private readonly bool transliterable;
        [JsonProperty(nameof(Localizations))]
        private readonly string[] localizations;

        [JsonIgnore]
        public string Name =>
            name;
        [JsonIgnore]
        public bool Transliterable =>
            transliterable;
        [JsonIgnore]
        public string[] Localizations =>
            localizations;

        [JsonConstructor]
        internal LanguageLocalizations(string name, bool transliterable, string[] localizations)
        {
            this.name = name;
            this.transliterable = transliterable;
            this.localizations = localizations;
        }
    }
}
