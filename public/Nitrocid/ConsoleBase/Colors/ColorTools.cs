
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
using ColorSeq;
using ColorSeq.Accessibility;
using Extensification.StringExts;
using KS.ConsoleBase.Themes;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.ConsoleBase.Colors
{
    /// <summary>
    /// Color tools module
    /// </summary>
    public static class ColorTools
    {

        // Variables for colors used by previous versions of the kernel.
        internal static Dictionary<KernelColorType, Color> KernelColors = PopulateColorsDefault();

        // Cache variables for background and foreground colors
        internal static string cachedForegroundColor = "";
        internal static string cachedBackgroundColor = "";

        /// <summary>
        /// Enables color blindness
        /// </summary>
        public static bool ColorBlind
        {
            get
            {
                return ColorSeq.ColorTools.EnableColorTransformation;
            }
            set
            {
                ColorSeq.ColorTools.EnableColorTransformation = value;
            }
        }

        /// <summary>
        /// Enables simple color blindness using the Vienot 1999 formula (may not be accurate for tritanopia)
        /// </summary>
        public static bool ColorBlindSimple
        {
            get
            {
                return ColorSeq.ColorTools.EnableSimpleColorTransformation;
            }
            set
            {
                ColorSeq.ColorTools.EnableSimpleColorTransformation = value;
            }
        }

        /// <summary>
        /// Color blindness deficiency
        /// </summary>
        public static Deficiency BlindnessDeficiency { get => ColorSeq.ColorTools.ColorDeficiency; set => ColorSeq.ColorTools.ColorDeficiency = value; }

        /// <summary>
        /// Color blindness severity
        /// </summary>
        public static double BlindnessSeverity { get => ColorSeq.ColorTools.ColorDeficiencySeverity; set => ColorSeq.ColorTools.ColorDeficiencySeverity = value; }

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public static Color GetColor(KernelColorType type) => new(KernelColors[type].PlainSequence);

        /// <summary>
        /// Sets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        /// <param name="color">Color to be set</param>
        public static Color SetColor(KernelColorType type, Color color) => KernelColors[type] = color;

        /// <summary>
        /// Populate the empty color dictionary
        /// </summary>
        public static Dictionary<KernelColorType, Color> PopulateColorsEmpty() => PopulateColors(0);

        /// <summary>
        /// Populate the default color dictionary
        /// </summary>
        public static Dictionary<KernelColorType, Color> PopulateColorsDefault() => PopulateColors(1);

        /// <summary>
        /// Populate the current color dictionary
        /// </summary>
        public static Dictionary<KernelColorType, Color> PopulateColorsCurrent() => PopulateColors(2);

        private static Dictionary<KernelColorType, Color> PopulateColors(int populationType)
        {
            Dictionary<KernelColorType, Color> colors = new();

            // Select population type
            switch (populationType)
            {
                case 0:
                    // Population type is empty colors
                    for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length; typeIndex++)
                    {
                        KernelColorType type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeIndex.ToString());
                        Color color = Color.Empty;
                        DebugWriter.WriteDebug(DebugLevel.I, "Adding color type {0} with color {1}...", type, color.PlainSequence);
                        colors.Add(type, color);
                    }
                    break;
                case 1:
                    // Population type is default colors
                    ThemeInfo themeInfo = new();
                    for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length; typeIndex++)
                    {
                        KernelColorType type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeIndex.ToString());
                        Color color = themeInfo.GetColor(type);
                        DebugWriter.WriteDebug(DebugLevel.I, "Adding color type {0} with color {1}...", type, color.PlainSequence);
                        colors.Add(type, color);
                    }
                    break;
                case 2:
                    // Population type is current colors
                    for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(KernelColorType)).Length; typeIndex++)
                    {
                        KernelColorType type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeIndex.ToString());
                        Color color = GetColor(type);
                        DebugWriter.WriteDebug(DebugLevel.I, "Adding color type {0} with color {1}...", type, color.PlainSequence);
                        colors.Add(type, color);
                    }
                    break;
            }

            // Return it
            DebugWriter.WriteDebug(DebugLevel.I, "Populated {0} colors.", colors.Count);
            return colors;
        }

        /// <summary>
        /// Loads the background
        /// </summary>
        public static void LoadBack() =>
            LoadBack(GetColor(KernelColorType.Background));

        /// <summary>
        /// Loads the background
        /// </summary>
        /// <param name="ColorSequence">Color sequence used to load background</param>
        /// <param name="Force">Force set background even if background setting is disabled</param>
        public static void LoadBack(Color ColorSequence, bool Force = false)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Filling background with background color {0}", ColorSequence.PlainSequence);
                SetConsoleColor(ColorSequence, true, Force);
                ConsoleWrapper.Clear();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to set background: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Gets the gray color according to the brightness of the background color
        /// </summary>
        public static Color GetGray()
        {
            if (GetColor(KernelColorType.Background).IsBright)
            {
                return GetColor(KernelColorType.NeutralText);
            }
            else
            {
                return new Color(ConsoleColors.Gray);
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        public static void SetConsoleColor(KernelColorType colorType) => SetConsoleColor(colorType, false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <param name="ForceSet">Force set color</param>
        public static void SetConsoleColor(KernelColorType colorType, bool Background, bool ForceSet = false)
        {
            SetConsoleColor(GetColor(colorType), Background, ForceSet);
            if (!Background)
                SetConsoleColor(GetColor(KernelColorType.Background), true);
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false)
        {
            if (ColorSequence is null)
                throw new KernelException(KernelExceptionType.Color, nameof(ColorSequence));

            // Define reset background sequence
            string resetSequence = CharManager.GetEsc() + $"[49m";

            // Set background
            if (Background)
            {
                if ((Flags.SetBackground | ForceSet) && cachedBackgroundColor != ColorSequence.VTSequenceBackground)
                {
                    TextWriterColor.WritePlain(ColorSequence.VTSequenceBackground, false);
                    cachedBackgroundColor = ColorSequence.VTSequenceBackground;
                }
                else if (!Flags.SetBackground && cachedBackgroundColor != resetSequence)
                {
                    TextWriterColor.WritePlain(resetSequence, false);
                    cachedBackgroundColor = resetSequence;
                }
            }
            else if (cachedForegroundColor != ColorSequence.VTSequenceForeground)
            {
                TextWriterColor.WritePlain(ColorSequence.VTSequenceForeground, false);
                cachedForegroundColor = ColorSequence.VTSequenceForeground;
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(KernelColorType colorType) => TrySetConsoleColor(colorType, false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(KernelColorType colorType, bool Background)
        {
            try
            {
                SetConsoleColor(colorType, Background);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(Color ColorSequence, bool Background)
        {
            try
            {
                SetConsoleColor(ColorSequence, Background);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, or a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(string ColorSpecifier)
        {
            try
            {
                var ColorInstance = new Color(ColorSpecifier);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int ColorNum)
        {
            try
            {
                var ColorInstance = new Color(ColorNum);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int R, int G, int B)
        {
            try
            {
                var ColorInstance = new Color(R, G, B);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed trying to parse color: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the RGB sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHexToRGB(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex.RemoveLetter(0), 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                return $"{R};{G};{B}";
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRGBToHex(string RGBSequence)
        {
            if (RGBSequence.Contains(Convert.ToString(';')))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    return $"#{R:X2}{G:X2}{B:X2}";
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RGB color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RGB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRGBToHex(int R, int G, int B)
        {
            if (R < 0 | R > 255)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid red color specifier."));
            if (G < 0 | G > 255)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid green color specifier."));
            if (B < 0 | B > 255)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid blue color specifier."));
            return $"#{R:X2}{G:X2}{B:X2}";
        }

    }
}
