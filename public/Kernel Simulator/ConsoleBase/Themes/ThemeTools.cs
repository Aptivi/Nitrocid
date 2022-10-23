
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
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using static KS.ConsoleBase.Colors.ColorTools;

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
            { "3Y-Diamond", new ThemeInfo("_3Y_Diamond") },
            { "Amaya", new ThemeInfo("Amaya") },
            { "Aptivi", new ThemeInfo("Aptivi") },
            { "Aquatic", new ThemeInfo("Aquatic") },
            { "AyuDark", new ThemeInfo("AyuDark") },
            { "AyuLight", new ThemeInfo("AyuLight") },
            { "AyuMirage", new ThemeInfo("AyuMirage") },
            { "BlackOnWhite", new ThemeInfo("BlackOnWhite") },
            { "BedOS", new ThemeInfo("BedOS") },
            { "Bloody", new ThemeInfo("Bloody") },
            { "Blue Power", new ThemeInfo("Blue_Power") },
            { "Bluesome", new ThemeInfo("Bluesome") },
            { "Bluespire", new ThemeInfo("Bluespire") },
            { "BreezeDark", new ThemeInfo("BreezeDark") },
            { "Breeze", new ThemeInfo("Breeze") },
            { "Dawn Aurora", new ThemeInfo("Dawn_Aurora") },
            { "Darcula", new ThemeInfo("Darcula") },
            { "Debian", new ThemeInfo("Debian") },
            { "EDM", new ThemeInfo("EDM") },
            { "Fire", new ThemeInfo("Fire") },
            { "Grape", new ThemeInfo("Grape") },
            { "Grape Kiwi", new ThemeInfo("Grape_Kiwi") },
            { "GTASA", new ThemeInfo("GTASA") },
            { "Grays", new ThemeInfo("Grays") },
            { "GrayOnYellow", new ThemeInfo("GrayOnYellow") },
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
            { "Metallic", new ThemeInfo("Metallic") },
            { "Mint", new ThemeInfo("Mint") },
            { "Mint Gum", new ThemeInfo("Mint_Gum") },
            { "Mintback", new ThemeInfo("Mintback") },
            { "Mintish", new ThemeInfo("Mintish") },
            { "Monokai", new ThemeInfo("Monokai") },
            { "NeonBreeze", new ThemeInfo("NeonBreeze") },
            { "Neutralized", new ThemeInfo("Neutralized") },
            { "NFSHP-Cop", new ThemeInfo("NFSHP_Cop") },
            { "NFSHP-Racer", new ThemeInfo("NFSHP_Racer") },
            { "NoFrilsAcme", new ThemeInfo("NoFrilsAcme") },
            { "NoFrilsDark", new ThemeInfo("NoFrilsDark") },
            { "NoFrilsLight", new ThemeInfo("NoFrilsLight") },
            { "NoFrilsSepia", new ThemeInfo("NoFrilsSepia") },
            { "Orange Sea", new ThemeInfo("Orange_Sea") },
            { "Pastel 1", new ThemeInfo("Pastel_1") },
            { "Pastel 2", new ThemeInfo("Pastel_2") },
            { "Pastel 3", new ThemeInfo("Pastel_3") },
            { "Papercolor", new ThemeInfo("Papercolor") },
            { "PapercolorDark", new ThemeInfo("PapercolorDark") },
            { "PhosphoricBG", new ThemeInfo("PhosphoricBG") },
            { "PhosphoricFG", new ThemeInfo("PhosphoricFG") },
            { "Planted Wood", new ThemeInfo("Planted_Wood") },
            { "Purlow", new ThemeInfo("Purlow") },
            { "Red Breeze", new ThemeInfo("Red_Breeze") },
            { "RedConsole", new ThemeInfo("RedConsole") },
            { "Reddish", new ThemeInfo("Reddish") },
            { "Rigel", new ThemeInfo("Rigel") },
            { "Sakura", new ThemeInfo("Sakura") },
            { "SolarizedDark", new ThemeInfo("SolarizedDark") },
            { "SolarizedLight", new ThemeInfo("SolarizedLight") },
            { "SpaceCamp", new ThemeInfo("SpaceCamp") },
            { "SpaceDuck", new ThemeInfo("SpaceDuck") },
            { "Tealed", new ThemeInfo("Tealed") },
            { "TealerOS", new ThemeInfo("TealerOS") },
            { "Techno", new ThemeInfo("Techno") },
            { "TrafficLight", new ThemeInfo("TrafficLight") },
            { "Ubuntu", new ThemeInfo("Ubuntu") },
            { "ViceCity", new ThemeInfo("ViceCity") },
            { "VisualStudioDark", new ThemeInfo("VisualStudioDark") },
            { "VisualStudioLight", new ThemeInfo("VisualStudioLight") },
            { "Windows11", new ThemeInfo("Windows11") },
            { "Windows11Light", new ThemeInfo("Windows11Light") },
            { "Windows95", new ThemeInfo("Windows95") },
            { "Wood", new ThemeInfo("Wood") },
            { "Yasai", new ThemeInfo("Yasai") },
            { "YellowBG", new ThemeInfo("YellowBG") },
            { "YellowFG", new ThemeInfo("YellowFG") }
        };

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
                var ThemeInfo = default(ThemeInfo);
                theme = theme.ReplaceAll(new[] { "-", " " }, "_");
                if (theme == "Default")
                {
                    ResetColors();
                }
                else if (theme == "3Y-Diamond")
                {
                    ThemeInfo = new ThemeInfo("_3Y_Diamond");
                }
                else
                {
                    ThemeInfo = new ThemeInfo(theme);
                }

                if (!(theme == "Default"))
                {
                    // Set colors as appropriate
                    SetColorsTheme(ThemeInfo);
                }

                // Raise event
                Kernel.Events.EventsManager.FireEvent("ThemeSet", theme);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Theme not found.");
                Kernel.Events.EventsManager.FireEvent("ThemeSetError", theme, ThemeSetErrorReasons.NotFound);
                throw new Kernel.Exceptions.NoSuchThemeException(Translate.DoTranslation("Invalid color template {0}"), theme);
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
                    SetColorsTheme(ThemeInfo);
                }

                // Raise event
                Kernel.Events.EventsManager.FireEvent("ThemeSet", ThemeFile);
            }
            catch (FileNotFoundException)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Theme not found.");
                Kernel.Events.EventsManager.FireEvent("ThemeSetError", ThemeFile, ThemeSetErrorReasons.NotFound);
                throw new Kernel.Exceptions.NoSuchThemeException(Translate.DoTranslation("Invalid color template {0}"), ThemeFile);
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="ThemeInfo">Theme information</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Kernel.Exceptions.ColorException"></exception>
        public static void SetColorsTheme(ThemeInfo ThemeInfo)
        {
            if (ThemeInfo is null)
                throw new ArgumentNullException(nameof(ThemeInfo));

            // Set the colors
            try
            {
                for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ColTypes)).Length - 2; typeIndex++)
                {
                    ColTypes type = KernelColors.Keys.ElementAt(typeIndex);
                    KernelColors[type] = ThemeInfo.ThemeColors[type];
                }
                LoadBack();
                Config.CreateConfig();

                // Raise event
                Kernel.Events.EventsManager.FireEvent("ColorSet");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                Kernel.Events.EventsManager.FireEvent("ColorSetError", ColorSetErrorReasons.InvalidColors);
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("One or more of the colors is invalid.") + " {0}", ex, ex.Message);
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="ThemeInfo">Theme information</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Kernel.Exceptions.ColorException"></exception>
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

    }
}
