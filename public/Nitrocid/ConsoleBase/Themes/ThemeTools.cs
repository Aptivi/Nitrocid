
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
using KS.Files;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Kernel.Events;
using static KS.ConsoleBase.Colors.KernelColorTools;
using KS.ConsoleBase.Writers.ConsoleWriters;
using Terminaux.Colors;
using KS.Misc.Reflection;
using KS.Resources;

namespace KS.ConsoleBase.Themes
{
    /// <summary>
    /// Theme tools module
    /// </summary>
    public static class ThemeTools
    {

        private readonly static Dictionary<string, ThemeInfo> themes = new();

        /// <summary>
        /// Gets the installed themes
        /// </summary>
        /// <returns>List of installed themes and their <see cref="ThemeInfo"/> instances</returns>
        public static Dictionary<string, ThemeInfo> GetInstalledThemes()
        {
            // Return cached version
            if (themes.Count > 0)
                return themes;

            // Now, get all theme names and populate them using ThemeInfo
            string[] nonThemes = new[]
            {
                nameof(ThemesResources.Culture),
                nameof(ThemesResources.ResourceManager)
            };
            string[] themeResNames = PropertyManager.GetPropertiesNoEvaluation(typeof(ThemesResources)).Keys.Except(nonThemes).ToArray();
            foreach (string key in themeResNames)
                themes.Add(key, new ThemeInfo(key));
            return themes;
        }

        /// <summary>
        /// Gets the theme information
        /// </summary>
        /// <param name="theme">Theme name</param>
        public static ThemeInfo GetThemeInfo(string theme) =>
            GetInstalledThemes()[theme];

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
        public static Dictionary<KernelColorType, Color> GetColorsFromTheme(ThemeInfo themeInfo) => 
            themeInfo.ThemeColors;

        /// <summary>
        /// Sets system colors according to the programmed templates
        /// </summary>
        /// <param name="theme">A specified theme</param>
        public static void ApplyThemeFromResources(string theme)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Theme: {0}", theme);
            if (GetInstalledThemes().ContainsKey(theme))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Theme found.");

                // Populate theme info
                var ThemeInfo = GetThemeInfo(theme);

                // Check if the console supports true color
                if ((Flags.ConsoleSupportsTrueColor && ThemeInfo.TrueColorRequired) || !ThemeInfo.TrueColorRequired)
                {
                    // Check to see if the event is finished
                    if (ThemeInfo.IsExpired)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Setting event theme in a day that the event finished...");
                        EventsManager.FireEvent(EventType.ThemeSetError, theme, ThemeSetErrorReasons.EventFinished);
                        throw new KernelException(KernelExceptionType.ThemeManagement, Translate.DoTranslation("The theme {0} celebrates an event, but you're either too early or too late to attend. Each year, this theme is accessible from {1}/{2} to {3}/{4}."), theme, ThemeInfo.StartMonth, ThemeInfo.StartDay, ThemeInfo.EndMonth, ThemeInfo.EndDay);
                    }

                    // Set colors as appropriate
                    DebugWriter.WriteDebug(DebugLevel.I, "Setting colors as appropriate...");
                    SetColorsTheme(ThemeInfo);
                }
                else
                {
                    // We're trying to apply true color on unsupported console
                    DebugWriter.WriteDebug(DebugLevel.E, "Unsupported console or the terminal doesn't support true color.");
                    EventsManager.FireEvent(EventType.ThemeSetError, theme, ThemeSetErrorReasons.ConsoleUnsupported);
                    throw new KernelException(KernelExceptionType.UnsupportedConsole, Translate.DoTranslation("The theme {0} needs true color support, but your console doesn't support it."), theme);
                }

                // Raise event
                EventsManager.FireEvent(EventType.ThemeSet, theme);
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
        public static void ApplyThemeFromFile(string ThemeFile)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Theme file name: {0}", ThemeFile);
                ThemeFile = Filesystem.NeutralizePath(ThemeFile, true);
                DebugWriter.WriteDebug(DebugLevel.I, "Theme file path: {0}", ThemeFile);

                // Populate theme info
                var ThemeStream = new StreamReader(ThemeFile);
                var ThemeInfo = new ThemeInfo(ThemeStream);
                ThemeStream.Close();

                if (!(ThemeFile == "Default"))
                {
                    // Set colors as appropriate
                    DebugWriter.WriteDebug(DebugLevel.I, "Setting colors as appropriate...");
                    SetColorsTheme(ThemeInfo);
                }

                // Raise event
                EventsManager.FireEvent(EventType.ThemeSet, ThemeFile);
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
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetColorsTheme(ThemeInfo ThemeInfo)
        {
            if (ThemeInfo is null)
                throw new KernelException(KernelExceptionType.Color, nameof(ThemeInfo));

            // Check to see if we're trying to preview theme on non-true color console
            if (IsTrueColorRequired(ThemeInfo) && !Flags.ConsoleSupportsTrueColor)
                throw new KernelException(KernelExceptionType.UnsupportedConsole, Translate.DoTranslation("Your console must support true color to use this theme."));

            // Set the colors
            try
            {
                for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length - 2; typeIndex++)
                {
                    KernelColorType type = KernelColors.Keys.ElementAt(typeIndex);
                    var themeColor = ThemeInfo.ThemeColors[type];
                    DebugWriter.WriteDebug(DebugLevel.I, "Kernel color type {0}, setting theme color {1}...", type.ToString(), themeColor.PlainSequence);
                    KernelColors[type] = themeColor;
                }
                LoadBack();
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
        /// Prepares the preview of the theme
        /// </summary>
        /// <param name="theme">Theme name</param>
        public static void PreviewTheme(string theme) =>
            PreviewTheme(GetThemeInfo(theme));

        /// <summary>
        /// Prepares the preview of the theme
        /// </summary>
        /// <param name="theme">Theme instance</param>
        public static void PreviewTheme(ThemeInfo theme) =>
            PreviewTheme(GetColorsFromTheme(theme));

        /// <summary>
        /// Prepares the preview of the theme
        /// </summary>
        /// <param name="colors">Dictionary of colors</param>
        public static void PreviewTheme(Dictionary<KernelColorType, Color> colors)
        {
            // Check to see if we're trying to preview theme on non-true color console
            if (IsTrueColorRequired(colors) && !Flags.ConsoleSupportsTrueColor)
                throw new KernelException(KernelExceptionType.UnsupportedConsole, Translate.DoTranslation("Your console must support true color to use this theme."));

            ConsoleWrapper.Clear();
            TextWriterColor.Write(Translate.DoTranslation("Here's how your theme will look like:") + CharManager.NewLine);

            // Print every possibility of color types
            for (int key = 0; key < colors.Count; key++)
            {
                TextWriterColor.Write($"*) {colors.Keys.ElementAt(key)}: ", false, KernelColorType.Option);
                TextWriterColor.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit.", true, colors.Values.ElementAt(key));
            }
        }

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <returns>If required, then true.</returns>
        public static bool Is255ColorsRequired(string theme) =>
            Is255ColorsRequired(GetThemeInfo(theme));

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="theme">Theme instance</param>
        /// <returns>If required, then true.</returns>
        public static bool Is255ColorsRequired(ThemeInfo theme) =>
            Is255ColorsRequired(GetColorsFromTheme(theme));

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="colors">Dictionary of colors</param>
        /// <returns>If required, then true.</returns>
        public static bool Is255ColorsRequired(Dictionary<KernelColorType, Color> colors)
        {
            // Check for true color requirement
            if (IsTrueColorRequired(colors))
                return true;

            // Now, check for 255-color requirement
            for (int key = 0; key < colors.Count; key++)
                if (colors.Values.ElementAt(key).Type == ColorType._255Color)
                    return true;

            // Else, 255 color support is not required
            return false;
        }

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <returns>If required, then true.</returns>
        public static bool IsTrueColorRequired(string theme) =>
            IsTrueColorRequired(GetThemeInfo(theme));

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="theme">Theme instance</param>
        /// <returns>If required, then true.</returns>
        public static bool IsTrueColorRequired(ThemeInfo theme) =>
            IsTrueColorRequired(GetColorsFromTheme(theme));

        /// <summary>
        /// Is the true color support required?
        /// </summary>
        /// <param name="colors">Dictionary of colors</param>
        /// <returns>If required, then true.</returns>
        public static bool IsTrueColorRequired(Dictionary<KernelColorType, Color> colors)
        {
            // Check for true color requirement according to color type
            for (int key = 0; key < colors.Count; key++)
                if (colors.Values.ElementAt(key).Type == ColorType.TrueColor)
                    return true;

            // Else, no requirement
            return false;
        }

    }
}
