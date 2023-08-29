
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Kernel.Time;
using KS.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terminaux.Colors;

namespace KS.ConsoleBase.Themes
{
    /// <summary>
    /// Theme information class
    /// </summary>
    public class ThemeInfo
    {

        internal readonly Dictionary<KernelColorType, Color> ThemeColors = KernelColorTools.PopulateColorsEmpty();
        internal readonly DateTime start = DateTime.Today;
        internal readonly DateTime end = DateTime.Today;

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
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public Color GetColor(KernelColorType type) =>
            ThemeColors[type];

        /// <summary>
        /// Generates a new theme info from KS resources
        /// </summary>
        public ThemeInfo() :
            this("_Default")
        { }

        /// <summary>
        /// Generates a new theme info from KS resources
        /// </summary>
        /// <param name="ThemeResourceName">Theme name (must match resource name)</param>
        public ThemeInfo(string ThemeResourceName) :
            this(JToken.Parse(ThemesResources.ResourceManager.GetString(ThemeResourceName)))
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
        protected ThemeInfo(JToken ThemeResourceJson)
        {
            // Place information to the class
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length; typeIndex++)
            {
                KernelColorType type = ThemeColors.Keys.ElementAt(typeIndex);
                ThemeColors[type] = new Color(ThemeResourceJson.SelectToken($"{type}Color").ToString());
            }
            Name = ThemeResourceJson["Metadata"]["Name"].ToString();
            Description = ThemeResourceJson["Metadata"]["Description"]?.ToString();
            TrueColorRequired = ThemeTools.IsTrueColorRequired(ThemeColors);

            // Parse event-related info
            IsEvent = (bool)(ThemeResourceJson["Metadata"]["IsEvent"] ?? false);
            StartMonth = (int)(ThemeResourceJson["Metadata"]["StartMonth"] ?? 1);
            StartDay = (int)(ThemeResourceJson["Metadata"]["StartDay"] ?? 1);
            EndMonth = (int)(ThemeResourceJson["Metadata"]["EndMonth"] ?? 1);
            EndDay = (int)(ThemeResourceJson["Metadata"]["EndDay"] ?? 1);

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
                    (EndMonth, StartMonth) = (StartMonth, EndMonth);
                else if (StartDay > EndDay)
                    (EndDay, StartDay) = (StartDay, EndDay);
            }
        }

    }
}
