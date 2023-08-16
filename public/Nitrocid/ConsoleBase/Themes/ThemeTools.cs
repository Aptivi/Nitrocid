
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

namespace KS.ConsoleBase.Themes
{
    /// <summary>
    /// Theme tools module
    /// </summary>
    public static class ThemeTools
    {

        /// <summary>
        /// All the available built-in themes
        /// </summary>
        public readonly static Dictionary<string, ThemeInfo> Themes = new()
        {
            { "Default", new ThemeInfo() },
            { "Amaya", new ThemeInfo("Amaya") },
            { "Aptivi", new ThemeInfo("Aptivi") },
            { "AyuDark", new ThemeInfo("AyuDark") },
            { "AyuLight", new ThemeInfo("AyuLight") },
            { "AyuMirage", new ThemeInfo("AyuMirage") },
            { "BlackOnWhite", new ThemeInfo("BlackOnWhite") },
            { "BlackRose", new ThemeInfo("BlackRose") },
            { "BreezeDark", new ThemeInfo("BreezeDark") },
            { "Breeze", new ThemeInfo("Breeze") },
            { "Darcula", new ThemeInfo("Darcula") },
            { "Debian", new ThemeInfo("Debian") },
            { "DefaultVivid", new ThemeInfo("DefaultVivid") },
            { "EDM", new ThemeInfo("EDM") },
            { "Fire", new ThemeInfo("Fire") },
            { "Grape", new ThemeInfo("Grape") },
            { "Grape Kiwi", new ThemeInfo("Grape_Kiwi") },
            { "Green Mix", new ThemeInfo("Green_Mix") },
            { "Grink", new ThemeInfo("Grink") },
            { "Gruvbox", new ThemeInfo("Gruvbox") },
            { "Hacker", new ThemeInfo("Hacker") },
            { "Lemon", new ThemeInfo("Lemon") },
            { "Light Planks", new ThemeInfo("Light_Planks") },
            { "LinuxColoredDef", new ThemeInfo("LinuxColoredDef") },
            { "LinuxUncolored", new ThemeInfo("LinuxUncolored") },
            { "Materialistic", new ThemeInfo("Materialistic") },
            { "Maya", new ThemeInfo("Maya") },
            { "Melange", new ThemeInfo("Melange") },
            { "MelangeDark", new ThemeInfo("MelangeDark") },
            { "Mint", new ThemeInfo("Mint") },
            { "Monokai", new ThemeInfo("Monokai") },
            { "NeonBreeze", new ThemeInfo("NeonBreeze") },
            { "Neutralized", new ThemeInfo("Neutralized") },
            { "NitricAcid", new ThemeInfo("NitricAcid") },
            { "NoFrilsAcme", new ThemeInfo("NoFrilsAcme") },
            { "NoFrilsDark", new ThemeInfo("NoFrilsDark") },
            { "NoFrilsLight", new ThemeInfo("NoFrilsLight") },
            { "NoFrilsSepia", new ThemeInfo("NoFrilsSepia") },
            { "Orange Sea", new ThemeInfo("Orange_Sea") },
            { "Papercolor", new ThemeInfo("Papercolor") },
            { "PapercolorDark", new ThemeInfo("PapercolorDark") },
            { "PhosphoricBG", new ThemeInfo("PhosphoricBG") },
            { "PhosphoricFG", new ThemeInfo("PhosphoricFG") },
            { "Red Breeze", new ThemeInfo("Red_Breeze") },
            { "RedConsole", new ThemeInfo("RedConsole") },
            { "Rigel", new ThemeInfo("Rigel") },
            { "Sakura", new ThemeInfo("Sakura") },
            { "SolarizedDark", new ThemeInfo("SolarizedDark") },
            { "SolarizedLight", new ThemeInfo("SolarizedLight") },
            { "SpaceCamp", new ThemeInfo("SpaceCamp") },
            { "SpaceDuck", new ThemeInfo("SpaceDuck") },
            { "Tealed", new ThemeInfo("Tealed") },
            { "Techno", new ThemeInfo("Techno") },
            { "Trance", new ThemeInfo("Trance") },
            { "Ubuntu", new ThemeInfo("Ubuntu") },
            { "ViceCity", new ThemeInfo("ViceCity") },
            { "VisualStudioDark", new ThemeInfo("VisualStudioDark") },
            { "VisualStudioLight", new ThemeInfo("VisualStudioLight") },
            { "Windows11", new ThemeInfo("Windows11") },
            { "Windows11Light", new ThemeInfo("Windows11Light") },
            { "Windows95", new ThemeInfo("Windows95") },
            { "Wood", new ThemeInfo("Wood") },
            { "Yasai", new ThemeInfo("Yasai") },
        };

        /// <summary>
        /// Gets the theme information
        /// </summary>
        /// <param name="theme">Theme name</param>
        public static ThemeInfo GetThemeInfo(string theme) =>
            Themes[theme];

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
            if (Themes.ContainsKey(theme))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Theme found.");

                // Populate theme info
                var ThemeInfo = GetThemeInfo(theme);

                // Check if the console supports true color
                if ((Flags.ConsoleSupportsTrueColor && ThemeInfo.TrueColorRequired) || !ThemeInfo.TrueColorRequired)
                {
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
