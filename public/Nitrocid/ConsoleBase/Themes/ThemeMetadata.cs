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

namespace Nitrocid.ConsoleBase.Themes
{
    internal class ThemeMetadata
    {
        [JsonProperty(nameof(Name))]
        private readonly string name;
        [JsonProperty(nameof(Description))]
        private readonly string description;
        [JsonProperty(nameof(IsEvent))]
        private readonly bool isEvent;
        [JsonProperty(nameof(StartMonth))]
        private readonly int startMonth;
        [JsonProperty(nameof(StartDay))]
        private readonly int startDay;
        [JsonProperty(nameof(EndMonth))]
        private readonly int endMonth;
        [JsonProperty(nameof(EndDay))]
        private readonly int endDay;
        [JsonProperty(nameof(Calendar))]
        private readonly string calendar;
        [JsonProperty(nameof(Category))]
        private readonly ThemeCategory category;
        [JsonProperty(nameof(Localizable))]
        private readonly bool localizable;
        [JsonProperty(nameof(UseAccentTypes))]
        private readonly string[] useAccentTypes;

        [JsonIgnore]
        public string Name =>
            name ?? "";
        [JsonIgnore]
        public string Description =>
            description ?? "";
        [JsonIgnore]
        public bool IsEvent =>
            isEvent;
        [JsonIgnore]
        public int StartMonth =>
            startMonth;
        [JsonIgnore]
        public int StartDay =>
            startDay;
        [JsonIgnore]
        public int EndMonth =>
            endMonth;
        [JsonIgnore]
        public int EndDay =>
            endDay;
        [JsonIgnore]
        public string Calendar =>
            calendar ?? "Gregorian";
        [JsonIgnore]
        public ThemeCategory Category =>
            category;
        [JsonIgnore]
        public bool Localizable =>
            localizable;
        [JsonIgnore]
        public string[] UseAccentTypes =>
            useAccentTypes;

        [JsonConstructor]
        internal ThemeMetadata(string name, string description, bool isEvent, int startMonth, int startDay, int endMonth, int endDay, string calendar, ThemeCategory category, bool localizable, string[] useAccentTypes)
        {
            this.name = name;
            this.description = description;
            this.isEvent = isEvent;
            this.startMonth = startMonth;
            this.startDay = startDay;
            this.endMonth = endMonth;
            this.endDay = endDay;
            this.calendar = calendar;
            this.category = category;
            this.localizable = localizable;
            this.useAccentTypes = useAccentTypes ?? [];
        }
    }
}
