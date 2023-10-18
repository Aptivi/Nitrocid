using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS.ConsoleBase.Themes
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
        [JsonProperty(nameof(Localizable))]
        private readonly bool localizable;

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
        public bool Localizable =>
            localizable;

        [JsonConstructor]
        internal ThemeMetadata(string name, string description, bool isEvent, int startMonth, int startDay, int endMonth, int endDay, string calendar, bool localizable)
        {
            this.name = name;
            this.description = description;
            this.isEvent = isEvent;
            this.startMonth = startMonth;
            this.startDay = startDay;
            this.endMonth = endMonth;
            this.endDay = endDay;
            this.calendar = calendar;
            this.localizable = localizable;
        }
    }
}
