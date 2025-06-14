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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Calendars;
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Reflection.Internal;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Files;

namespace Nitrocid.ConsoleBase.Themes
{
    /// <summary>
    /// Theme information class
    /// </summary>
    public class ThemeInfo
    {

        internal readonly Dictionary<KernelColorType, Color> ThemeColors = KernelColorTools.PopulateColorsEmpty();
        internal readonly DateTime start = DateTime.Today;
        internal readonly DateTime end = DateTime.Today;
        private string[] useAccentTypes = [];
        private readonly ThemeMetadata metadata;
        private readonly JToken metadataToken;

        /// <summary>
        /// Theme name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Theme description
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// Is true color required?
        /// </summary>
        public bool TrueColorRequired { get; }
        /// <summary>
        /// Whether this theme celebrates a specific event
        /// </summary>
        public bool IsEvent { get; }
        /// <summary>
        /// The month in which the event starts
        /// </summary>
        public int StartMonth { get; }
        /// <summary>
        /// The day in which the event starts
        /// </summary>
        public int StartDay { get; }
        /// <summary>
        /// The start <see cref="DateTime"/> instance representing the start of the event
        /// </summary>
        public DateTime Start =>
            start;
        /// <summary>
        /// The month in which the event ends
        /// </summary>
        public int EndMonth { get; }
        /// <summary>
        /// The day in which the event ends
        /// </summary>
        public int EndDay { get; }
        /// <summary>
        /// The end <see cref="DateTime"/> instance representing the end of the event
        /// </summary>
        public DateTime End =>
            end;
        /// <summary>
        /// Whether you can set this theme or not. Always false in non-event themes. False if the theme is an event and the current
        /// time and date is between <see cref="StartMonth"/>/<see cref="StartDay"/> and <see cref="EndMonth"/>/<see cref="EndDay"/>
        /// </summary>
        public bool IsExpired =>
            IsEvent && (TimeDateTools.KernelDateTime < Start || TimeDateTools.KernelDateTime > End);
        /// <summary>
        /// The category in which the theme is categorized
        /// </summary>
        public ThemeCategory Category { get; }
        /// <summary>
        /// The calendar name in which the event is assigned to
        /// </summary>
        public string Calendar { get; }
        /// <summary>
        /// Kernel color type list to use accent color
        /// </summary>
        public string[] UseAccentTypes =>
            useAccentTypes;

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public Color GetColor(KernelColorType type) =>
            ThemeColors[type];

        internal void UpdateColors()
        {
            // Populate the colors
            DebugWriter.WriteDebug(DebugLevel.I, $"Updating color according to theme info for {Name}...");
            useAccentTypes = metadata.UseAccentTypes.Where((type) => Enum.IsDefined(typeof(KernelColorType), type[..^5])).ToArray();
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length; typeIndex++)
            {
                KernelColorType type = ThemeColors.Keys.ElementAt(typeIndex);

                // Get the color value and check to see if it's null
                string fullTypeName = $"{type}Color";
                var colorToken = metadataToken.SelectToken(fullTypeName);
                if (colorToken is null)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, $"{fullTypeName} is not defined in the theme metadata. Using defaults...");
                    ThemeColors[type] = KernelColorTools.PopulateColorsDefault()[type];
                }
                else
                    ThemeColors[type] =
                        UseAccentTypes.Contains(fullTypeName) && Config.MainConfig.UseAccentColors ?
                        fullTypeName.EndsWith("BackgroundColor") || fullTypeName.EndsWith("BackColor") ? new Color(Config.MainConfig.AccentBackgroundColor) : new Color(Config.MainConfig.AccentForegroundColor) :
                        new Color(colorToken.ToString());
            }
        }

        /// <summary>
        /// Generates a new theme info from KS resources
        /// </summary>
        public ThemeInfo() :
            this(JToken.Parse(ResourcesManager.ConvertToString(
                ResourcesManager.GetData("Default.json", ResourcesType.Themes) ??
                    throw new KernelException(KernelExceptionType.ThemeManagement, LanguageTools.GetLocalized("NKS_CONSOLEBASE_THEMES_EXCEPTION_DEFAULTTHEMEFAILED")))))
        { }

        /// <summary>
        /// Generates a new theme info from file stream
        /// </summary>
        /// <param name="themePath">Theme file path</param>
        public ThemeInfo(string themePath) :
            this(JToken.Parse(FilesystemTools.ReadContentsText(themePath)))
        { }

        /// <summary>
        /// Generates a new theme info from file stream
        /// </summary>
        /// <param name="ThemeFileStream">Theme file stream reader</param>
        public ThemeInfo(StreamReader ThemeFileStream) :
            this(JToken.Parse(ThemeFileStream.ReadToEnd()))
        { }

        /// <summary>
        /// Generates a new theme info from theme resource JSON
        /// </summary>
        /// <param name="ThemeResourceJson">Theme resource JSON</param>
        internal ThemeInfo(JToken ThemeResourceJson)
        {
            // Parse the metadata
            var metadataObj = ThemeResourceJson["Metadata"] ??
                throw new KernelException(KernelExceptionType.ThemeManagement, LanguageTools.GetLocalized("NKS_CONSOLEBASE_THEMES_EXCEPTION_NOTHEMEMETADATA"));
            metadata = JsonConvert.DeserializeObject<ThemeMetadata>(metadataObj.ToString()) ??
                throw new KernelException(KernelExceptionType.ThemeManagement, LanguageTools.GetLocalized("NKS_CONSOLEBASE_THEMES_EXCEPTION_THEMEMETADATAINVALID"));
            metadataToken = ThemeResourceJson;

            // Populate colors
            Name = metadata.Name;
            UpdateColors();

            // Install some info to the class
            Description = metadata.Description;
            TrueColorRequired = ThemeTools.MinimumTypeRequired(ThemeColors, ColorType.TrueColor);
            Category = metadata.Category;

            // Parse event-related info
            IsEvent = metadata.IsEvent;
            StartMonth = metadata.StartMonth;
            StartDay = metadata.StartDay;
            EndMonth = metadata.EndMonth;
            EndDay = metadata.EndDay;
            Calendar = metadata.Calendar;
            if (!Enum.TryParse(Calendar, out CalendarTypes calendar))
                calendar = CalendarTypes.Gregorian;

            // If the calendar is not Gregorian (for example, Hijri), convert that to Gregorian using the current date
            if (calendar != CalendarTypes.Gregorian)
            {
                var calendarInstance = CalendarTools.GetCalendar(calendar);
                int year = calendarInstance.Culture.DateTimeFormat.Calendar.GetYear(TimeDateTools.KernelDateTime);
                int yearEnd = year;
                int monthStart = StartMonth;
                int monthEnd = EndMonth;
                var dayStart = StartDay;
                var dayEnd = EndDay;
                if (monthEnd < monthStart)
                    yearEnd++;
                var dateTimeStart = new DateTime(year, monthStart, dayStart, calendarInstance.Culture.DateTimeFormat.Calendar);
                var dateTimeEnd = new DateTime(yearEnd, monthEnd, dayEnd, calendarInstance.Culture.DateTimeFormat.Calendar);
                StartMonth = dateTimeStart.Month;
                EndMonth = dateTimeEnd.Month;
                StartDay = dateTimeStart.Day;
                EndDay = dateTimeEnd.Day;
            }

            // Month sanity checks
            StartMonth =
                StartMonth < 1 ? 1 :
                StartMonth > 12 ? 12 :
                StartMonth;
            EndMonth =
                EndMonth < 1 ? 1 :
                EndMonth > 12 ? 12 :
                EndMonth;

            // Day sanity checks
            int maxDayNumStart = DateTime.DaysInMonth(TimeDateTools.KernelDateTime.Year, StartMonth);
            int maxDayNumEnd = DateTime.DaysInMonth(TimeDateTools.KernelDateTime.Year, EndMonth);
            StartDay =
                StartDay < 1 ? 1 :
                StartDay > maxDayNumStart ? maxDayNumStart :
                StartDay;
            EndDay =
                EndDay < 1 ? 1 :
                EndDay > maxDayNumEnd ? maxDayNumEnd :
                EndDay;

            // Check to see if the end is earlier than the start
            start = new(TimeDateTools.KernelDateTime.Year, StartMonth, StartDay);
            end = new(TimeDateTools.KernelDateTime.Year, EndMonth, EndDay);
            if (start > end)
            {
                // End is earlier than start! Swap the two values so that:
                //    start = end;
                //    end = start;
                (end, start) = (start, end);

                // Deal with the start and the end
                if (StartMonth > EndMonth)
                    end = end.AddYears(1);
                else if (StartDay > EndDay)
                    (EndDay, StartDay) = (StartDay, EndDay);
            }
        }

    }
}
