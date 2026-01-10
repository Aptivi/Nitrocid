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

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terminaux.Themes.Colors;
using Terminaux.Themes;
using Nitrocid.Base.Files;
using Terminaux.Colors;

namespace Nitrocid.Extras.ThemeStudio.Studio
{
    static class ThemeStudioTools
    {

        /// <summary>
        /// Selected theme name
        /// </summary>
        internal static string SelectedThemeName = "";
        internal static readonly Dictionary<string, Color> SelectedColors = ThemeColorsTools.PopulateColorsCurrent();

        /// <summary>
        /// Saves theme to current directory under "<paramref name="Theme"/>.json."
        /// </summary>
        /// <param name="Theme">Theme name</param>
        public static void SaveThemeToCurrentDirectory(string Theme)
        {
            var ThemeJson = GetThemeJson();
            FilesystemTools.WriteContentsText(FilesystemTools.NeutralizePath(Theme + ".json"), JsonConvert.SerializeObject(ThemeJson, Formatting.Indented));
        }

        /// <summary>
        /// Saves theme to another directory under "<paramref name="Theme"/>.json."
        /// </summary>
        /// <param name="Theme">Theme name</param>
        /// <param name="Path">Path name. Neutralized by <see cref="FilesystemTools.NeutralizePath(string, bool)"/></param>
        public static void SaveThemeToAnotherDirectory(string Theme, string Path)
        {
            var ThemeJson = GetThemeJson();
            FilesystemTools.WriteContentsText(FilesystemTools.NeutralizePath(Path + "/" + Theme + ".json"), JsonConvert.SerializeObject(ThemeJson, Formatting.Indented));
        }

        /// <summary>
        /// Loads theme from resource and places it to the studio
        /// </summary>
        /// <param name="Theme">A theme name</param>
        public static void LoadThemeFromResource(string Theme) =>
            LoadThemeFromThemeInfo(ThemeTools.GetThemeInfo(Theme));

        /// <summary>
        /// Loads theme from resource and places it to the studio
        /// </summary>
        /// <param name="Theme">A theme name</param>
        public static void LoadThemeFromFile(string Theme)
        {
            // Populate theme info
            var ThemeInfo = new ThemeInfo(FilesystemTools.NeutralizePath(Theme));
            LoadThemeFromThemeInfo(ThemeInfo);
        }

        /// <summary>
        /// Loads theme from theme info and places it to the studio
        /// </summary>
        /// <param name="themeInfo">A theme info instance</param>
        public static void LoadThemeFromThemeInfo(ThemeInfo themeInfo)
        {
            // Place information to the studio
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length; typeIndex++)
            {
                string type = SelectedColors.Keys.ElementAt(typeIndex);
                SelectedColors[type] = ThemeTools.GetColorsFromTheme(themeInfo)[type];
            }
        }

        /// <summary>
        /// Loads theme from current colors and places it to the studio
        /// </summary>
        public static void LoadThemeFromCurrentColors()
        {
            // Place information to the studio
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length; typeIndex++)
            {
                string type = SelectedColors.Keys.ElementAt(typeIndex);
                SelectedColors[type] = ThemeColorsTools.GetColor(type);
            }
        }

        /// <summary>
        /// Gets the full theme JSON object
        /// </summary>
        /// <returns>A JSON object</returns>
        public static JObject GetThemeJson()
        {
            JObject themeJson = [];

            // Populate the metadata
            JProperty metadata =
                /*
                 * Metadata instance with the format of:
                 * 
                 *     "Metadata": {
                 *         "Name": "ThemeName"
                 *     },
                 */
                new("Metadata",
                    new JObject(
                        new JProperty("Name", SelectedThemeName)
                    )
                );
            themeJson.Add(metadata);

            // Populate the colors
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length; typeIndex++)
            {
                // Add the color to the final object
                string type = SelectedColors.Keys.ElementAt(typeIndex);
                themeJson.Add(new JProperty($"{type}Color", SelectedColors[type].PlainSequence));
            }

            // Return the final object with the metadata
            return themeJson;
        }

    }
}
