
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
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
using ColorSeq;
using KS.ConsoleBase.Colors;
using Newtonsoft.Json.Linq;

namespace KS.ConsoleBase.Themes
{
    /// <summary>
    /// Theme information class
    /// </summary>
    public class ThemeInfo
    {

        internal readonly Dictionary<ColorTools.ColTypes, Color> ThemeColors = new()
        {
            { ColorTools.ColTypes.Input, Color.Empty },
            { ColorTools.ColTypes.License, Color.Empty },
            { ColorTools.ColTypes.Continuable, Color.Empty },
            { ColorTools.ColTypes.Uncontinuable, Color.Empty },
            { ColorTools.ColTypes.HostName, Color.Empty },
            { ColorTools.ColTypes.UserName, Color.Empty },
            { ColorTools.ColTypes.Background, Color.Empty },
            { ColorTools.ColTypes.Neutral, Color.Empty },
            { ColorTools.ColTypes.ListEntry, Color.Empty },
            { ColorTools.ColTypes.ListValue, Color.Empty },
            { ColorTools.ColTypes.Stage, Color.Empty },
            { ColorTools.ColTypes.Error, Color.Empty },
            { ColorTools.ColTypes.Warning, Color.Empty },
            { ColorTools.ColTypes.Option, Color.Empty },
            { ColorTools.ColTypes.Banner, Color.Empty },
            { ColorTools.ColTypes.NotificationTitle, Color.Empty },
            { ColorTools.ColTypes.NotificationDescription, Color.Empty },
            { ColorTools.ColTypes.NotificationProgress, Color.Empty },
            { ColorTools.ColTypes.NotificationFailure, Color.Empty },
            { ColorTools.ColTypes.Question, Color.Empty },
            { ColorTools.ColTypes.Success, Color.Empty },
            { ColorTools.ColTypes.UserDollar, Color.Empty },
            { ColorTools.ColTypes.Tip, Color.Empty },
            { ColorTools.ColTypes.SeparatorText, Color.Empty },
            { ColorTools.ColTypes.Separator, Color.Empty },
            { ColorTools.ColTypes.ListTitle, Color.Empty },
            { ColorTools.ColTypes.DevelopmentWarning, Color.Empty },
            { ColorTools.ColTypes.StageTime, Color.Empty },
            { ColorTools.ColTypes.Progress, Color.Empty },
            { ColorTools.ColTypes.BackOption, Color.Empty },
            { ColorTools.ColTypes.LowPriorityBorder, Color.Empty },
            { ColorTools.ColTypes.MediumPriorityBorder, Color.Empty },
            { ColorTools.ColTypes.HighPriorityBorder, Color.Empty },
            { ColorTools.ColTypes.TableSeparator, Color.Empty },
            { ColorTools.ColTypes.TableHeader, Color.Empty },
            { ColorTools.ColTypes.TableValue, Color.Empty },
            { ColorTools.ColTypes.SelectedOption, Color.Empty },
            { ColorTools.ColTypes.AlternativeOption, Color.Empty },
        };

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public Color GetColor(ColorTools.ColTypes type) => ThemeColors[type];

        /// <summary>
        /// Generates a new theme info from KS resources
        /// </summary>
        /// <param name="ThemeResourceName">Theme name (must match resource name)</param>
        public ThemeInfo(string ThemeResourceName) : this(JToken.Parse(Properties.Resources.Resources.ResourceManager.GetString(ThemeResourceName))) {}

        /// <summary>
        /// Generates a new theme info from file stream
        /// </summary>
        /// <param name="ThemeFileStream">Theme file stream reader</param>
        public ThemeInfo(StreamReader ThemeFileStream) : this(JToken.Parse(ThemeFileStream.ReadToEnd())) {}

        /// <summary>
        /// Generates a new theme info from theme resource JSON
        /// </summary>
        /// <param name="ThemeResourceJson">Theme resource JSON</param>
        protected ThemeInfo(JToken ThemeResourceJson)
        {
            // Place information to the class
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ColorTools.ColTypes)).Length - 2; typeIndex++)
            {
                ColorTools.ColTypes type = ThemeColors.Keys.ElementAt(typeIndex);
                ThemeColors[type] = new Color(ThemeResourceJson.SelectToken($"{type}Color").ToString());
            }
        }

    }
}
