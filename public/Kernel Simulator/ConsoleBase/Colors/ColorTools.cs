
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
using System.Linq;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase.Themes;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.WriterBase;

namespace KS.ConsoleBase.Colors
{
    /// <summary>
    /// Color tools module
    /// </summary>
    public static class ColorTools
    {

        /// <summary>
        /// Enumeration for color types
        /// </summary>
        public enum ColTypes : int
        {
            /// <summary>
            /// Input text
            /// </summary>
            Input,
            /// <summary>
            /// License color
            /// </summary>
            License,
            /// <summary>
            /// Continuable kernel panic text (usually sync'd with Warning)
            /// </summary>
            ContKernelError,
            /// <summary>
            /// Uncontinuable kernel panic text (usually sync'd with Error)
            /// </summary>
            UncontKernelError,
            /// <summary>
            /// Host name color
            /// </summary>
            HostNameShell,
            /// <summary>
            /// User name color
            /// </summary>
            UserNameShell,
            /// <summary>
            /// Background color
            /// </summary>
            Background,
            /// <summary>
            /// Neutral text (for general purposes)
            /// </summary>
            NeutralText,
            /// <summary>
            /// List entry text
            /// </summary>
            ListEntry,
            /// <summary>
            /// List value text
            /// </summary>
            ListValue,
            /// <summary>
            /// Stage text
            /// </summary>
            Stage,
            /// <summary>
            /// Error text
            /// </summary>
            Error,
            /// <summary>
            /// Warning text
            /// </summary>
            Warning,
            /// <summary>
            /// Option text
            /// </summary>
            Option,
            /// <summary>
            /// Banner text
            /// </summary>
            Banner,
            /// <summary>
            /// Notification title text
            /// </summary>
            NotificationTitle,
            /// <summary>
            /// Notification description text
            /// </summary>
            NotificationDescription,
            /// <summary>
            /// Notification progress text
            /// </summary>
            NotificationProgress,
            /// <summary>
            /// Notification failure text
            /// </summary>
            NotificationFailure,
            /// <summary>
            /// Question text
            /// </summary>
            Question,
            /// <summary>
            /// Success text
            /// </summary>
            Success,
            /// <summary>
            /// User dollar sign on shell text
            /// </summary>
            UserDollar,
            /// <summary>
            /// Tip text
            /// </summary>
            Tip,
            /// <summary>
            /// Separator text
            /// </summary>
            SeparatorText,
            /// <summary>
            /// Separator color
            /// </summary>
            Separator,
            /// <summary>
            /// List title text
            /// </summary>
            ListTitle,
            /// <summary>
            /// Development warning text
            /// </summary>
            DevelopmentWarning,
            /// <summary>
            /// Stage time text
            /// </summary>
            StageTime,
            /// <summary>
            /// General progress text
            /// </summary>
            Progress,
            /// <summary>
            /// Back option text
            /// </summary>
            BackOption,
            /// <summary>
            /// Low priority notification border color
            /// </summary>
            LowPriorityBorder,
            /// <summary>
            /// Medium priority notification border color
            /// </summary>
            MediumPriorityBorder,
            /// <summary>
            /// High priority notification border color
            /// </summary>
            HighPriorityBorder,
            /// <summary>
            /// Table separator
            /// </summary>
            TableSeparator,
            /// <summary>
            /// Table header
            /// </summary>
            TableHeader,
            /// <summary>
            /// Table value
            /// </summary>
            TableValue,
            /// <summary>
            /// Selected option
            /// </summary>
            SelectedOption,
            /// <summary>
            /// Alternative option
            /// </summary>
            AlternativeOption,
            /// <summary>
            /// Gray color (for special purposes)
            /// </summary>
            Gray = -1,
        }

        // Variables for colors used by previous versions of the kernel.
        internal static readonly Dictionary<ColTypes, Color> KernelColors = new()
        {
            { ColTypes.Input, new((int)ConsoleColors.White) },
            { ColTypes.License, new((int)ConsoleColors.White) },
            { ColTypes.ContKernelError, new((int)ConsoleColors.Yellow) },
            { ColTypes.UncontKernelError, new((int)ConsoleColors.Red) },
            { ColTypes.HostNameShell, new((int)ConsoleColors.DarkGreen) },
            { ColTypes.UserNameShell, new((int)ConsoleColors.Green) },
            { ColTypes.Background, new((int)ConsoleColors.Black) },
            { ColTypes.NeutralText, new((int)ConsoleColors.Gray) },
            { ColTypes.ListEntry, new((int)ConsoleColors.DarkYellow) },
            { ColTypes.ListValue, new((int)ConsoleColors.DarkGray) },
            { ColTypes.Stage, new((int)ConsoleColors.Green) },
            { ColTypes.Error, new((int)ConsoleColors.Red) },
            { ColTypes.Warning, new((int)ConsoleColors.Yellow) },
            { ColTypes.Option, new((int)ConsoleColors.DarkYellow) },
            { ColTypes.Banner, new((int)ConsoleColors.Green) },
            { ColTypes.NotificationTitle, new((int)ConsoleColors.White) },
            { ColTypes.NotificationDescription, new((int)ConsoleColors.Gray) },
            { ColTypes.NotificationProgress, new((int)ConsoleColors.DarkYellow) },
            { ColTypes.NotificationFailure, new((int)ConsoleColors.Red) },
            { ColTypes.Question, new((int)ConsoleColors.Yellow) },
            { ColTypes.Success, new((int)ConsoleColors.Green) },
            { ColTypes.UserDollar, new((int)ConsoleColors.Gray) },
            { ColTypes.Tip, new((int)ConsoleColors.Gray) },
            { ColTypes.SeparatorText, new((int)ConsoleColors.White) },
            { ColTypes.Separator, new((int)ConsoleColors.Gray) },
            { ColTypes.ListTitle, new((int)ConsoleColors.White) },
            { ColTypes.DevelopmentWarning, new((int)ConsoleColors.Yellow) },
            { ColTypes.StageTime, new((int)ConsoleColors.Gray) },
            { ColTypes.Progress, new((int)ConsoleColors.DarkYellow) },
            { ColTypes.BackOption, new((int)ConsoleColors.DarkRed) },
            { ColTypes.LowPriorityBorder, new((int)ConsoleColors.White) },
            { ColTypes.MediumPriorityBorder, new((int)ConsoleColors.Yellow) },
            { ColTypes.HighPriorityBorder, new((int)ConsoleColors.Red) },
            { ColTypes.TableSeparator, new((int)ConsoleColors.DarkGray) },
            { ColTypes.TableHeader, new((int)ConsoleColors.White) },
            { ColTypes.TableValue, new((int)ConsoleColors.Gray) },
            { ColTypes.SelectedOption, new((int)ConsoleColors.Yellow) },
            { ColTypes.AlternativeOption, new((int)ConsoleColors.DarkGreen) },
        };

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public static Color GetColor(ColTypes type) => KernelColors[type];

        /// <summary>
        /// Sets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        /// <param name="color">Color to be set</param>
        public static Color SetColor(ColTypes type, Color color) => KernelColors[type] = color;

        /// <summary>
        /// Resets all colors to default
        /// </summary>
        public static void ResetColors()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Resetting colors");
            var DefInfo = new ThemeInfo();

            // Set colors
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ColTypes)).Length - 2; typeIndex++)
            {
                ColTypes type = KernelColors.Keys.ElementAt(typeIndex);
                KernelColors[type] = DefInfo.ThemeColors[type];
            }
            LoadBack();

            // Raise event
            Kernel.Kernel.KernelEventManager.RaiseColorReset();
        }

        /// <summary>
        /// Loads the background
        /// </summary>
        public static void LoadBack()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Filling background with background color");
                SetConsoleColor(GetColor(ColTypes.Background), true);
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
            if (GetColor(ColTypes.Background).IsBright)
            {
                return GetColor(ColTypes.NeutralText);
            }
            else
            {
                return new Color((int)ConsoleColors.Gray);
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        public static void SetConsoleColor(ColTypes colorType) => SetConsoleColor(colorType, false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <param name="ForceSet">Force set color</param>
        public static void SetConsoleColor(ColTypes colorType, bool Background, bool ForceSet = false)
        {
            switch (colorType)
            {
                case ColTypes.Gray:
                    {
                        SetConsoleColor(GetGray(), Background, ForceSet);
                        break;
                    }
                default:
                    {
                        SetConsoleColor(GetColor(colorType), Background, ForceSet);
                        break;
                    }
            }
            if (!Background)
                SetConsoleColor(GetColor(ColTypes.Background), true);
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false)
        {
            if (Shell.Shell.ColoredShell)
            {
                if (ColorSequence is null)
                    throw new ArgumentNullException(nameof(ColorSequence));

                // Define reset background sequence
                string resetSequence = CharManager.GetEsc() + $"[49m";

                // Set background
                if (Background)
                {
                    if (Flags.SetBackground | ForceSet)
                        WriterPlainManager.CurrentPlain.WritePlain(ColorSequence.VTSequenceBackground, false);
                    else
                        WriterPlainManager.CurrentPlain.WritePlain(resetSequence, false);
                }
                else
                {
                    WriterPlainManager.CurrentPlain.WritePlain(ColorSequence.VTSequenceForeground, false);
                }
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(ColTypes colorType) => TrySetConsoleColor(colorType, false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(ColTypes colorType, bool Background)
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
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid hex color specifier."));
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
                    throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid RGB color specifier."));
                }
            }
            else
            {
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid RGB color specifier."));
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
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid red color specifier."));
            if (G < 0 | G > 255)
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid green color specifier."));
            if (B < 0 | B > 255)
                throw new Kernel.Exceptions.ColorException(Translate.DoTranslation("Invalid blue color specifier."));
            return $"#{R:X2}{G:X2}{B:X2}";
        }

    }
}
