
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
using KS.Files;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.ConsoleBase.Themes.Studio
{
    static class ThemeStudioTools
    {

        /// <summary>
        /// Selected theme name
        /// </summary>
        internal static string SelectedThemeName = "";
        internal static readonly Dictionary<ColorTools.ColTypes, Color> SelectedColors = new()
        {
            { ColorTools.ColTypes.Input, ColorTools.GetColor(ColorTools.ColTypes.Input) },
            { ColorTools.ColTypes.License, ColorTools.GetColor(ColorTools.ColTypes.License) },
            { ColorTools.ColTypes.ContKernelError, ColorTools.GetColor(ColorTools.ColTypes.ContKernelError) },
            { ColorTools.ColTypes.UncontKernelError, ColorTools.GetColor(ColorTools.ColTypes.UncontKernelError) },
            { ColorTools.ColTypes.HostNameShell, ColorTools.GetColor(ColorTools.ColTypes.HostNameShell) },
            { ColorTools.ColTypes.UserNameShell, ColorTools.GetColor(ColorTools.ColTypes.UserNameShell) },
            { ColorTools.ColTypes.Background, ColorTools.GetColor(ColorTools.ColTypes.Background) },
            { ColorTools.ColTypes.NeutralText, ColorTools.GetColor(ColorTools.ColTypes.NeutralText) },
            { ColorTools.ColTypes.ListEntry, ColorTools.GetColor(ColorTools.ColTypes.ListEntry) },
            { ColorTools.ColTypes.ListValue, ColorTools.GetColor(ColorTools.ColTypes.ListValue) },
            { ColorTools.ColTypes.Stage, ColorTools.GetColor(ColorTools.ColTypes.Stage) },
            { ColorTools.ColTypes.Error, ColorTools.GetColor(ColorTools.ColTypes.Error) },
            { ColorTools.ColTypes.Warning, ColorTools.GetColor(ColorTools.ColTypes.Warning) },
            { ColorTools.ColTypes.Option, ColorTools.GetColor(ColorTools.ColTypes.Option) },
            { ColorTools.ColTypes.Banner, ColorTools.GetColor(ColorTools.ColTypes.Banner) },
            { ColorTools.ColTypes.NotificationTitle, ColorTools.GetColor(ColorTools.ColTypes.NotificationTitle) },
            { ColorTools.ColTypes.NotificationDescription, ColorTools.GetColor(ColorTools.ColTypes.NotificationDescription) },
            { ColorTools.ColTypes.NotificationProgress, ColorTools.GetColor(ColorTools.ColTypes.NotificationProgress) },
            { ColorTools.ColTypes.NotificationFailure, ColorTools.GetColor(ColorTools.ColTypes.NotificationFailure) },
            { ColorTools.ColTypes.Question, ColorTools.GetColor(ColorTools.ColTypes.Question) },
            { ColorTools.ColTypes.Success, ColorTools.GetColor(ColorTools.ColTypes.Success) },
            { ColorTools.ColTypes.UserDollar, ColorTools.GetColor(ColorTools.ColTypes.UserDollar) },
            { ColorTools.ColTypes.Tip, ColorTools.GetColor(ColorTools.ColTypes.Tip) },
            { ColorTools.ColTypes.SeparatorText, ColorTools.GetColor(ColorTools.ColTypes.SeparatorText) },
            { ColorTools.ColTypes.Separator, ColorTools.GetColor(ColorTools.ColTypes.Separator) },
            { ColorTools.ColTypes.ListTitle, ColorTools.GetColor(ColorTools.ColTypes.ListTitle) },
            { ColorTools.ColTypes.DevelopmentWarning, ColorTools.GetColor(ColorTools.ColTypes.DevelopmentWarning) },
            { ColorTools.ColTypes.StageTime, ColorTools.GetColor(ColorTools.ColTypes.StageTime) },
            { ColorTools.ColTypes.Progress, ColorTools.GetColor(ColorTools.ColTypes.Progress) },
            { ColorTools.ColTypes.BackOption, ColorTools.GetColor(ColorTools.ColTypes.BackOption) },
            { ColorTools.ColTypes.LowPriorityBorder, ColorTools.GetColor(ColorTools.ColTypes.LowPriorityBorder) },
            { ColorTools.ColTypes.MediumPriorityBorder, ColorTools.GetColor(ColorTools.ColTypes.MediumPriorityBorder) },
            { ColorTools.ColTypes.HighPriorityBorder, ColorTools.GetColor(ColorTools.ColTypes.HighPriorityBorder) },
            { ColorTools.ColTypes.TableSeparator, ColorTools.GetColor(ColorTools.ColTypes.TableSeparator) },
            { ColorTools.ColTypes.TableHeader, ColorTools.GetColor(ColorTools.ColTypes.TableHeader) },
            { ColorTools.ColTypes.TableValue, ColorTools.GetColor(ColorTools.ColTypes.TableValue) },
            { ColorTools.ColTypes.SelectedOption, ColorTools.GetColor(ColorTools.ColTypes.SelectedOption) },
            { ColorTools.ColTypes.AlternativeOption, ColorTools.GetColor(ColorTools.ColTypes.AlternativeOption) },
        };

        /// <summary>
        /// Saves theme to current directory under "<paramref name="Theme"/>.json."
        /// </summary>
        /// <param name="Theme">Theme name</param>
        public static void SaveThemeToCurrentDirectory(string Theme)
        {
            var ThemeJson = GetThemeJson();
            File.WriteAllText(Filesystem.NeutralizePath(Theme + ".json"), JsonConvert.SerializeObject(ThemeJson, Formatting.Indented));
        }

        /// <summary>
        /// Saves theme to another directory under "<paramref name="Theme"/>.json."
        /// </summary>
        /// <param name="Theme">Theme name</param>
        /// <param name="Path">Path name. Neutralized by <see cref="Filesystem.NeutralizePath(string, bool)"/></param>
        public static void SaveThemeToAnotherDirectory(string Theme, string Path)
        {
            var ThemeJson = GetThemeJson();
            File.WriteAllText(Filesystem.NeutralizePath(Path + "/" + Theme + ".json"), JsonConvert.SerializeObject(ThemeJson, Formatting.Indented));
        }

        /// <summary>
        /// Loads theme from resource and places it to the studio
        /// </summary>
        /// <param name="Theme">A theme name</param>
        public static void LoadThemeFromResource(string Theme)
        {
            // Populate theme info
            ThemeInfo ThemeInfo;
            if (Theme == "Default")
            {
                ThemeInfo = new ThemeInfo();
            }
            else if (Theme == "3Y-Diamond")
            {
                ThemeInfo = new ThemeInfo("_3Y_Diamond");
            }
            else
            {
                ThemeInfo = new ThemeInfo(Theme.Replace("-", "_"));
            }
            LoadThemeFromThemeInfo(ThemeInfo);
        }

        /// <summary>
        /// Loads theme from resource and places it to the studio
        /// </summary>
        /// <param name="Theme">A theme name</param>
        public static void LoadThemeFromFile(string Theme)
        {
            // Populate theme info
            var ThemeStream = new StreamReader(Filesystem.NeutralizePath(Theme));
            var ThemeInfo = new ThemeInfo(ThemeStream);
            ThemeStream.Close();
            LoadThemeFromThemeInfo(ThemeInfo);
        }

        /// <summary>
        /// Loads theme from theme info and places it to the studio
        /// </summary>
        /// <param name="themeInfo">A theme info instance</param>
        public static void LoadThemeFromThemeInfo(ThemeInfo themeInfo)
        {
            // Place information to the studio
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ColorTools.ColTypes)).Length - 2; typeIndex++)
            {
                ColorTools.ColTypes type = SelectedColors.Keys.ElementAt(typeIndex);
                SelectedColors[type] = themeInfo.ThemeColors[type];
            }
        }

        /// <summary>
        /// Loads theme from current colors and places it to the studio
        /// </summary>
        public static void LoadThemeFromCurrentColors()
        {
            // Place information to the studio
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ColorTools.ColTypes)).Length - 2; typeIndex++)
            {
                ColorTools.ColTypes type = SelectedColors.Keys.ElementAt(typeIndex);
                SelectedColors[type] = ColorTools.KernelColors[type];
            }
        }

        /// <summary>
        /// Gets the full theme JSON object
        /// </summary>
        /// <returns>A JSON object</returns>
        public static JObject GetThemeJson()
        {
            JObject themeJson = new();

            // Populate the metadata
            JProperty metadata =
                /*
                 * Metadata instance with the format of:
                 * 
                 *     "Metadata": {
                 *         "Name": "ThemeName",
                 *         "255ColorsRequired": true,
                 *         "TrueColorRequired": true
                 *     },
                 */
                new("Metadata",
                    new JObject(
                        new JProperty("Name", SelectedThemeName),
                        new JProperty("255ColorsRequired", Is255ColorsRequired()),
                        new JProperty("TrueColorRequired", IsTrueColorRequired())
                    )
                );
            themeJson.Add(metadata);

            // Populate the colors
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ColorTools.ColTypes)).Length - 2; typeIndex++)
            {
                // Add the color to the final object
                ColorTools.ColTypes type = SelectedColors.Keys.ElementAt(typeIndex);
                themeJson.Add(new JProperty($"{type}Color", SelectedColors[type].PlainSequence));
            }

            // Return the final object with the metadata
            return themeJson;
        }

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <returns>If required, then true.</returns>
        public static bool Is255ColorsRequired()
        {
            // Check for true color requirement
            if (IsTrueColorRequired())
                return true;

            // Now, check for 255-color requirement
            for (int key = 0; key < SelectedColors.Count; key++)
                if (SelectedColors.Values.ElementAt(key).Type == ColorType._255Color)
                    return true;

            // Else, 255 color support is not required
            return false;
        }

        /// <summary>
        /// Is the true color support required?
        /// </summary>
        /// <returns>If required, then true.</returns>
        public static bool IsTrueColorRequired()
        {
            // Check for true color requirement according to color type
            for (int key = 0; key < SelectedColors.Count; key++)
                if (SelectedColors.Values.ElementAt(key).Type == ColorType.TrueColor)
                    return true;

            // Else, no requirement
            return false;
        }

        /// <summary>
        /// Prepares the preview
        /// </summary>
        public static void PreparePreview()
        {
            ConsoleWrapper.Clear();
            TextWriterColor.Write(Translate.DoTranslation("Here's how your theme will look like:") + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.NeutralText);

            // Print every possibility of color types
            for (int key = 0; key < SelectedColors.Count; key++)
            {
                TextWriterColor.Write($"*) {SelectedColors.Keys.ElementAt(key)}: ", false, ColorTools.ColTypes.Option);
                TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, SelectedColors.Values.ElementAt(key));
            }
        }

    }
}
