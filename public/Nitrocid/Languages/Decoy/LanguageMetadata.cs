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
    internal class LanguageMetadata
    {
        [JsonProperty(nameof(three))]
        private readonly string three;
        [JsonProperty(nameof(name))]
        private readonly string name;
        [JsonProperty(nameof(transliterable))]
        private readonly bool transliterable;
        [JsonProperty(nameof(codepage))]
        private readonly int codepage;
        [JsonProperty(nameof(country))]
        private readonly string country;

        [JsonIgnore]
        public string ThreeLetterLanguageName =>
            three;
        [JsonIgnore]
        public string Name =>
            name;
        [JsonIgnore]
        public bool Transliterable =>
            transliterable;
        [JsonIgnore]
        public int Codepage =>
            codepage;
        [JsonIgnore]
        public string Country =>
            country;

        [JsonConstructor]
        internal LanguageMetadata(string three, string name, bool transliterable, int codepage, string country)
        {
            this.three = three;
            this.name = name;
            this.transliterable = transliterable;
            this.codepage = codepage;
            this.country = country;
        }
    }
}
