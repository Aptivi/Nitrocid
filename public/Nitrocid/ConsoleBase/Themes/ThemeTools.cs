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
using Terminaux.Colors;
using Newtonsoft.Json.Linq;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Events;
using Nitrocid.Misc.Reflection.Internal;

namespace Nitrocid.ConsoleBase.Themes
{
    /// <summary>
    /// Theme tools module
    /// </summary>
    public static class ThemeTools
    {

        internal readonly static Dictionary<string, ThemeInfo> themes = new()
        {
            { "Default", new ThemeInfo(JToken.Parse(ResourcesManager.GetData("Default.json", ResourcesType.Themes) ?? "[]")) },
            { "Dynamic", new ThemeInfo(JToken.Parse(ResourcesManager.GetData("Dynamic.json", ResourcesType.Themes) ?? "[]")) },
            { "NitricAcid", new ThemeInfo(JToken.Parse(ResourcesManager.GetData("NitricAcid.json", ResourcesType.Themes) ?? "[]")) },
        };

        /// <summary>
        /// Gets the installed themes
        /// </summary>
        /// <returns>List of installed themes and their <see cref="ThemeInfo"/> instances</returns>
        public static Dictionary<string, ThemeInfo> GetInstalledThemes() =>
            new(themes);

        /// <summary>
        /// Gets the installed themes by category
        /// </summary>
        /// <param name="category">Category to look for themes</param>
        /// <returns>List of installed themes and their <see cref="ThemeInfo"/> instances</returns>
        public static Dictionary<string, ThemeInfo> GetInstalledThemesByCategory(ThemeCategory category) =>
            themes
                .Where((kvp) => kvp.Value.Category == category)
                .ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);

        /// <summary>
        /// Gets the theme information
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <param name="throwNotFound">Throws an exception if the theme is not found</param>
        public static ThemeInfo GetThemeInfo(string theme, bool throwNotFound = false)
        {
            var themes = GetInstalledThemes();
            if (themes.TryGetValue(theme, out ThemeInfo? resultingTheme))
                return resultingTheme;
            if (throwNotFound)
                throw new KernelException(KernelExceptionType.NoSuchTheme, Translate.DoTranslation("Invalid color template {0}"), theme);
            return themes["Default"];
        }

        /// <summary>
        /// Checks to see if a built-in (or an addon) theme is found
        /// </summary>
        /// <param name="theme">Theme name</param>
        public static bool IsThemeFound(string theme)
        {
            var themes = GetInstalledThemes();
            return themes.ContainsKey(theme);
        }

        /// <summary>
        /// Gets the colors from the theme
        /// </summary>
        /// <param name="theme">Theme name</param>
        public static Dictionary<KernelColorType, Color> GetColorsFromTheme(string theme) =>
            GetColorsFromTheme(GetThemeInfo(theme));

        /// <summary>
        /// Gets the colors from the theme
        /// </summary>
        /// <param name="themeInfo">Theme instance</param>
        public static Dictionary<KernelColorType, Color> GetColorsFromTheme(ThemeInfo themeInfo)
        {
            if (themeInfo.UseAccentTypes.Length > 0)
                themeInfo.UpdateColors();
            return themeInfo.ThemeColors;
        }

        /// <summary>
        /// Sets system colors according to the theme information instance
        /// </summary>
        /// <param name="themeInfo">A specified theme information instance</param>
        /// <param name="dry">Whether to dryly apply the theme or not</param>
        public static void ApplyTheme(ThemeInfo themeInfo, bool dry = false)
        {
            // Check if the console supports true color
            if (Config.MainConfig.ConsoleSupportsTrueColor && themeInfo.TrueColorRequired || !themeInfo.TrueColorRequired)
            {
                // Check to see if the event is finished
                if (themeInfo.IsExpired)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Setting event theme in a day that the event finished...");
                    EventsManager.FireEvent(EventType.ThemeSetError, themeInfo.Name, ThemeSetErrorReasons.EventFinished);
                    throw new KernelException(KernelExceptionType.ThemeManagement, Translate.DoTranslation("The theme {0} celebrates an event, but you're either too early or too late to attend. Each year, this theme is accessible from {1}/{2} to {3}/{4}."), themeInfo.Name, themeInfo.StartMonth, themeInfo.StartDay, themeInfo.EndMonth, themeInfo.EndDay);
                }

                // Set colors as appropriate
                DebugWriter.WriteDebug(DebugLevel.I, "Setting colors as appropriate...");
                SetColorsTheme(themeInfo, dry);
            }
            else
            {
                // We're trying to apply true color on unsupported console
                DebugWriter.WriteDebug(DebugLevel.E, "Unsupported console or the terminal doesn't support true color.");
                EventsManager.FireEvent(EventType.ThemeSetError, themeInfo.Name, ThemeSetErrorReasons.ConsoleUnsupported);
                throw new KernelException(KernelExceptionType.UnsupportedConsole, Translate.DoTranslation("The theme {0} needs true color support, but your console doesn't support it."), themeInfo.Name);
            }

            // Raise event
            EventsManager.FireEvent(EventType.ThemeSet, themeInfo.Name);
        }

        /// <summary>
        /// Sets system colors according to the programmed templates
        /// </summary>
        /// <param name="theme">A specified theme</param>
        /// <param name="dry">Whether to dryly apply the theme or not</param>
        public static void ApplyThemeFromResources(string theme, bool dry = false)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Theme: {0}", theme);
            if (GetInstalledThemes().ContainsKey(theme))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Theme found.");

                // Populate theme info and use it
                var ThemeInfo = GetThemeInfo(theme);
                ApplyTheme(ThemeInfo, dry);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Theme not found.");
                EventsManager.FireEvent(EventType.ThemeSetError, theme, ThemeSetErrorReasons.NotFound);
                throw new KernelException(KernelExceptionType.NoSuchTheme, Translate.DoTranslation("Invalid color template {0}"), theme);
            }
        }

        /// <summary>
        /// Sets system colors according to the template file
        /// </summary>
        /// <param name="ThemeFile">Theme file</param>
        /// <param name="dry">Whether to dryly apply the theme or not</param>
        public static void ApplyThemeFromFile(string ThemeFile, bool dry = false)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Theme file name: {0}", ThemeFile);
                ThemeFile = FilesystemTools.NeutralizePath(ThemeFile, true);
                DebugWriter.WriteDebug(DebugLevel.I, "Theme file path: {0}", ThemeFile);

                // Populate theme info and use it
                var ThemeInfo = new ThemeInfo(ThemeFile);
                ApplyTheme(ThemeInfo, dry);
            }
            catch (FileNotFoundException)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Theme not found.");
                EventsManager.FireEvent(EventType.ThemeSetError, ThemeFile, ThemeSetErrorReasons.NotFound);
                throw new KernelException(KernelExceptionType.NoSuchTheme, Translate.DoTranslation("Invalid color template {0}"), ThemeFile);
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="ThemeInfo">Theme information</param>
        /// <param name="dry">Whether to dryly set the colors or not</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetColorsTheme(ThemeInfo ThemeInfo, bool dry = false)
        {
            if (ThemeInfo is null)
                throw new KernelException(KernelExceptionType.Color, nameof(ThemeInfo));

            // Check to see if we're trying to preview theme on non-true color console
            if (MinimumTypeRequired(ThemeInfo, ColorType.TrueColor) && !Config.MainConfig.ConsoleSupportsTrueColor)
                throw new KernelException(KernelExceptionType.UnsupportedConsole, Translate.DoTranslation("Your console must support true color to use this theme."));

            // Set the colors
            try
            {
                for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length; typeIndex++)
                {
                    KernelColorType type = KernelColorTools.KernelColors.Keys.ElementAt(typeIndex);
                    var themeColor = ThemeInfo.ThemeColors[type];
                    DebugWriter.WriteDebug(DebugLevel.I, "Kernel color type {0}, setting theme color {1}...", type.ToString(), themeColor.PlainSequence);
                    KernelColorTools.KernelColors[type] = themeColor;
                }
                ColorTools.LoadBack(KernelColorTools.GetColor(KernelColorType.Background));
                if (!dry)
                    Config.CreateConfig();

                // Raise event
                EventsManager.FireEvent(EventType.ColorSet);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                EventsManager.FireEvent(EventType.ColorSetError, KernelColorSetErrorReasons.InvalidColors);
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("One or more of the colors is invalid.") + " {0}", ex, ex.Message);
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="ThemeInfo">Theme information</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool TrySetColorsTheme(ThemeInfo ThemeInfo)
        {
            try
            {
                SetColorsTheme(ThemeInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="theme">Theme instance</param>
        /// <param name="type">Minimum required color type</param>
        /// <returns>If required, then true. Always false when <see cref="ColorType.FourBitColor"/> is passed.</returns>
        public static bool MinimumTypeRequired(string theme, ColorType type) =>
            MinimumTypeRequired(GetThemeInfo(theme), type);

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="theme">Theme instance</param>
        /// <param name="type">Minimum required color type</param>
        /// <returns>If required, then true. Always false when <see cref="ColorType.FourBitColor"/> is passed.</returns>
        public static bool MinimumTypeRequired(ThemeInfo theme, ColorType type) =>
            MinimumTypeRequired(GetColorsFromTheme(theme), type);

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="colors">Dictionary of colors</param>
        /// <param name="type">Minimum required color type</param>
        /// <returns>If required, then true. Always false when <see cref="ColorType.FourBitColor"/> is passed.</returns>
        public static bool MinimumTypeRequired(Dictionary<KernelColorType, Color> colors, ColorType type)
        {
            if (type == ColorType.FourBitColor)
                return false;

            // Now, check for 255-color requirement
            for (int key = 0; key < colors.Count; key++)
                if (colors.Values.ElementAt(key).Type <= type)
                    return true;

            // Else, 255 color support is not required
            return false;
        }

    }
}
