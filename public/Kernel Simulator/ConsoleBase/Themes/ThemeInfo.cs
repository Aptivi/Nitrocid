
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
using Newtonsoft.Json.Linq;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.ConsoleBase.Themes
{
    /// <summary>
    /// Theme information class
    /// </summary>
    public class ThemeInfo
    {

        internal readonly Dictionary<ColorTools.ColTypes, Color> ThemeColors = ColorTools.PopulateColorsEmpty();

        /// <summary>
        /// Theme name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Is true color required?
        /// </summary>
        public bool TrueColorRequired { get; }

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public Color GetColor(ColorTools.ColTypes type) => ThemeColors[type];

        /// <summary>
        /// Generates a new theme info from KS resources
        /// </summary>
        public ThemeInfo() : this("_Default") {}

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
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ColorTools.ColTypes)).Length - 1; typeIndex++)
            {
                ColorTools.ColTypes type = ThemeColors.Keys.ElementAt(typeIndex);
                ThemeColors[type] = new Color(ThemeResourceJson.SelectToken($"{type}Color").ToString());
            }
            Name = ThemeResourceJson["Metadata"]["Name"].ToString();
            TrueColorRequired = ThemeTools.IsTrueColorRequired(ThemeColors);
        }

    }
}
