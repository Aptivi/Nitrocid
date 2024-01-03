//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System;
using Terminaux.Colors;
using Terminaux.Colors.Models.Conversion;

namespace Nitrocid.ConsoleBase.Colors
{
    /// <summary>
    /// Kernel color conversion tools
    /// </summary>
    public static class KernelColorConversionTools
    {
        #region From hex to...
        /// <summary>
        /// Converts from the hexadecimal representation of a color to the RGB sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHexToRgb(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex[1..], 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                return $"{R};{G};{B}";
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the RYB sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHexToRyb(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex[1..], 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                var ryb = RybConversionTools.ConvertFrom(new Color(R, G, B).RGB);
                string rybSeq = $"ryb:{ryb.R};{ryb.Y};{ryb.B}";
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", rybSeq);
                return rybSeq;
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the CMYK sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromHexToCmyk(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex[1..], 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                var cmyk = CmykConversionTools.ConvertFrom(new Color(R, G, B).RGB);
                var cmy = cmyk.CMY;
                string cmykSeq = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMYK: {0})", cmykSeq);
                return cmykSeq;
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the CMY sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromHexToCmy(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex[1..], 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                var cmy = CmyConversionTools.ConvertFrom(new Color(R, G, B).RGB);
                string cmySeq = $"cmy:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole}";
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMY: {0})", cmySeq);
                return cmySeq;
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the HSL sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromHexToHsl(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex[1..], 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                var hsl = HslConversionTools.ConvertFrom(new Color(R, G, B).RGB);
                string hslSeq = $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSL: {0})", hslSeq);
                return hslSeq;
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the HSV sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromHexToHsv(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex[1..], 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                var hsv = HsvConversionTools.ConvertFrom(new Color(R, G, B).RGB);
                string hsvSeq = $"hsv:{hsv.HueWhole};{hsv.SaturationWhole};{hsv.ValueWhole}";
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSV: {0})", hsvSeq);
                return hsvSeq;
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }
        #endregion

        #region From RGB to...
        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRgbToHex(int R, int G, int B)
        {
            if (R < 0 | R > 255)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid red color specifier."));
            if (G < 0 | G > 255)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid green color specifier."));
            if (B < 0 | B > 255)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid blue color specifier."));
            string hex = $"#{R:X2}{G:X2}{B:X2}";
            DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
            return hex;
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRgbToHex(string RGBSequence)
        {
            if (RGBSequence.Contains(';'))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    string hex = $"#{R:X2}{G:X2}{B:X2}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                    return hex;
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
        /// Converts from the RGB sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</returns>
        public static string ConvertFromRgbToRyb(int R, int G, int B) =>
            ConvertFromRgbToRyb($"{R};{G};{B}");

        /// <summary>
        /// Converts from the RGB sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</returns>
        public static string ConvertFromRgbToRyb(string RGBSequence)
        {
            if (RGBSequence.Contains(';'))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                    var ryb = RybConversionTools.ConvertFrom(new Color(R, G, B).RGB);
                    string rybSeq = $"ryb:{ryb.R};{ryb.Y};{ryb.B}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", rybSeq);
                    return rybSeq;
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
        /// Converts from the RGB sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromRgbToCmyk(int R, int G, int B) =>
            ConvertFromRgbToCmyk($"{R};{G};{B}");

        /// <summary>
        /// Converts from the RGB sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromRgbToCmyk(string RGBSequence)
        {
            if (RGBSequence.Contains(';'))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                    var cmyk = CmykConversionTools.ConvertFrom(new Color(R, G, B).RGB);
                    var cmy = cmyk.CMY;
                    string cmykSeq = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMYK: {0})", cmykSeq);
                    return cmykSeq;
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
        /// Converts from the RGB sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromRgbToCmy(int R, int G, int B) =>
            ConvertFromRgbToCmy($"{R};{G};{B}");

        /// <summary>
        /// Converts from the RGB sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromRgbToCmy(string RGBSequence)
        {
            if (RGBSequence.Contains(';'))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                    var cmy = CmyConversionTools.ConvertFrom(new Color(R, G, B).RGB);
                    string cmySeq = $"cmy:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMY: {0})", cmySeq);
                    return cmySeq;
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
        /// Converts from the RGB sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromRgbToHsl(int R, int G, int B) =>
            ConvertFromRgbToHsl($"{R};{G};{B}");

        /// <summary>
        /// Converts from the RGB sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromRgbToHsl(string RGBSequence)
        {
            if (RGBSequence.Contains(';'))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                    var hsl = HslConversionTools.ConvertFrom(new Color(R, G, B).RGB);
                    string hslSeq = $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSL: {0})", hslSeq);
                    return hslSeq;
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
        /// Converts from the RGB sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromRgbToHsv(int R, int G, int B) =>
            ConvertFromRgbToHsv($"{R};{G};{B}");

        /// <summary>
        /// Converts from the RGB sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromRgbToHsv(string RGBSequence)
        {
            if (RGBSequence.Contains(';'))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", R, G, B);
                    var hsv = HsvConversionTools.ConvertFrom(new Color(R, G, B).RGB);
                    string hsvSeq = $"hsv:{hsv.HueWhole};{hsv.SaturationWhole};{hsv.ValueWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSV: {0})", hsvSeq);
                    return hsvSeq;
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
        #endregion

        #region From RYB to...
        /// <summary>
        /// Converts from the RYB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="B">The blue level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRybToHex(int R, int Y, int B) =>
            ConvertFromRybToHex($"ryb:{R};{Y};{B}");

        /// <summary>
        /// Converts from the RYB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="RYBSequence">ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRybToHex(string RYBSequence)
        {
            if (RYBSequence.Contains(';') && RYBSequence.StartsWith("ryb:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RYBSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"ryb:{R};{Y};{B}");
                    string hex = rgb.Hex;
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                    return hex;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RYB sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="B">The blue level</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromRybToRgb(int R, int Y, int B) =>
            ConvertFromRybToRgb($"ryb:{R};{Y};{B}");

        /// <summary>
        /// Converts from the RYB sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="RYBSequence">ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromRybToRgb(string RYBSequence)
        {
            if (RYBSequence.Contains(';') && RYBSequence.StartsWith("ryb:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RYBSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", $"ryb:{R};{Y};{B}");
                    var rgb = new Color($"ryb:{R};{Y};{B}");
                    string rgbSeq = rgb.ToString();
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RGB: {0})", rgbSeq);
                    return rgbSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RYB sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="B">The blue level</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromRybToCmyk(int R, int Y, int B) =>
            ConvertFromRybToCmyk($"ryb:{R};{Y};{B}");

        /// <summary>
        /// Converts from the RYB sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="RYBSequence">ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromRybToCmyk(string RYBSequence)
        {
            if (RYBSequence.Contains(';') && RYBSequence.StartsWith("ryb:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RYBSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", $"ryb:{R};{Y};{B}");
                    var cmyk = CmykConversionTools.ConvertFrom(new Color($"ryb:{R};{Y};{B}").RGB);
                    var cmy = cmyk.CMY;
                    string cmykSeq = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMYK: {0})", cmykSeq);
                    return cmykSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RYB sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="B">The blue level</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromRybToCmy(int R, int Y, int B) =>
            ConvertFromRybToCmy($"ryb:{R};{Y};{B}");

        /// <summary>
        /// Converts from the RYB sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="RYBSequence">ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromRybToCmy(string RYBSequence)
        {
            if (RYBSequence.Contains(';') && RYBSequence.StartsWith("ryb:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RYBSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", $"ryb:{R};{Y};{B}");
                    var cmy = CmyConversionTools.ConvertFrom(new Color($"ryb:{R};{Y};{B}").RGB);
                    string cmySeq = $"cmy:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMY: {0})", cmySeq);
                    return cmySeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RYB sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="B">The blue level</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromRybToHsl(int R, int Y, int B) =>
            ConvertFromRybToHsl($"ryb:{R};{Y};{B}");

        /// <summary>
        /// Converts from the RYB sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="RYBSequence">ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromRybToHsl(string RYBSequence)
        {
            if (RYBSequence.Contains(';') && RYBSequence.StartsWith("ryb:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RYBSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", $"ryb:{R};{Y};{B}");
                    var hsl = HslConversionTools.ConvertFrom(new Color($"ryb:{R};{Y};{B}").RGB);
                    string hslSeq = $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSL: {0})", hslSeq);
                    return hslSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RYB sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="B">The blue level</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromRybToHsv(int R, int Y, int B) =>
            ConvertFromRybToHsv($"ryb:{R};{Y};{B}");

        /// <summary>
        /// Converts from the RYB sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="RYBSequence">ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromRybToHsv(string RYBSequence)
        {
            if (RYBSequence.Contains(';') && RYBSequence.StartsWith("ryb:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RYBSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", $"ryb:{R};{Y};{B}");
                    var hsv = HsvConversionTools.ConvertFrom(new Color($"ryb:{R};{Y};{B}").RGB);
                    string hsvSeq = $"hsv:{hsv.HueWhole};{hsv.SaturationWhole};{hsv.ValueWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSV: {0})", hsvSeq);
                    return hsvSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
            }
        }
        #endregion

        #region From CMY to...
        /// <summary>
        /// Converts from the CMY sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromCmyToHex(int C, int M, int Y)
        {
            if (C < 0 | C > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid cyan color specifier."));
            if (M < 0 | M > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid magenta color specifier."));
            if (Y < 0 | Y > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid yellow color specifier."));
            var rgb = new Color($"cmy:{C};{M};{Y}");
            string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
            DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
            return hex;
        }

        /// <summary>
        /// Converts from the CMY sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="CMYSequence">cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromCmyToHex(string CMYSequence)
        {
            if (CMYSequence.Contains(';') && CMYSequence.StartsWith("cmy:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"cmy:{C};{M};{Y}");
                    string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                    return hex;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMY sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromCmyToRgb(int C, int M, int Y) =>
            ConvertFromCmyToRgb($"cmy:{C};{M};{Y}");

        /// <summary>
        /// Converts from the CMY sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="CMYSequence">cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromCmyToRgb(string CMYSequence)
        {
            if (CMYSequence.Contains(';') && CMYSequence.StartsWith("cmy:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"cmy:{C};{M};{Y}");
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RGB: {0})", rgb.PlainSequenceTrueColor);
                    return rgb.PlainSequenceTrueColor;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMY sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <returns>ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</returns>
        public static string ConvertFromCmyToRyb(int C, int M, int Y) =>
            ConvertFromCmyToRyb($"cmy:{C};{M};{Y}");

        /// <summary>
        /// Converts from the CMY sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="CMYSequence">cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</param>
        /// <returns>ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</returns>
        public static string ConvertFromCmyToRyb(string CMYSequence)
        {
            if (CMYSequence.Contains(';') && CMYSequence.StartsWith("cmy:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    var ryb = RybConversionTools.ConvertFrom(new Color($"cmy:{C};{M};{Y}").RGB);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", ryb.ToString());
                    return ryb.ToString();
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMY sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromCmyToHsl(int C, int M, int Y) =>
            ConvertFromCmyToHsl($"cmy:{C};{M};{Y}");

        /// <summary>
        /// Converts from the CMY sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="CMYSequence">cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromCmyToHsl(string CMYSequence)
        {
            if (CMYSequence.Contains(';') && CMYSequence.StartsWith("cmy:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"cmy:{C};{M};{Y}");
                    var hsl = HslConversionTools.ConvertFrom(rgb.RGB);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSL: hsl:{0};{1};{2})", hsl.HueWhole, hsl.SaturationWhole, hsl.LightnessWhole);
                    return $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMY sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromCmyToHsv(int C, int M, int Y) =>
            ConvertFromCmyToHsv($"cmy:{C};{M};{Y}");

        /// <summary>
        /// Converts from the CMY sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="CMYSequence">cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromCmyToHsv(string CMYSequence)
        {
            if (CMYSequence.Contains(';') && CMYSequence.StartsWith("cmy:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"cmy:{C};{M};{Y}");
                    var hsv = HsvConversionTools.ConvertFrom(rgb.RGB);
                    string hsvSequence = $"hsv:{hsv.HueWhole};{hsv.SaturationWhole};{hsv.ValueWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSV: {0})", hsvSequence);
                    return hsvSequence;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMY sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromCmyToCmyk(int C, int M, int Y) =>
            ConvertFromCmyToCmyk($"cmy:{C};{M};{Y}");

        /// <summary>
        /// Converts from the CMY sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="CMYSequence">cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromCmyToCmyk(string CMYSequence)
        {
            if (CMYSequence.Contains(';') && CMYSequence.StartsWith("cmy:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"cmy:{C};{M};{Y}");
                    var cmyk = CmykConversionTools.ConvertFrom(rgb.RGB);
                    var cmy = cmyk.CMY;
                    string cmySequence = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMYK: {0})", cmySequence);
                    return cmySequence;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
            }
        }
        #endregion

        #region From CMYK to...
        /// <summary>
        /// Converts from the CMYK sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromCmykToHex(int C, int M, int Y, int K)
        {
            if (C < 0 | C > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid cyan color specifier."));
            if (M < 0 | M > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid magenta color specifier."));
            if (Y < 0 | Y > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid yellow color specifier."));
            if (K < 0 | K > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid black key specifier."));
            var rgb = new Color($"cmyk:{C};{M};{Y};{K}");
            string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
            DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
            return hex;
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromCmykToHex(string CMYKSequence)
        {
            if (CMYKSequence.Contains(';') && CMYKSequence.StartsWith("cmyk:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYKSequence[5..].Split(';');
                if (ColorSpecifierArray.Length == 4)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    int K = Convert.ToInt32(ColorSpecifierArray[3]);
                    var rgb = new Color($"cmyk:{C};{M};{Y};{K}");
                    string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                    return hex;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromCmykToRgb(int C, int M, int Y, int K) =>
            ConvertFromCmykToRgb($"cmyk:{C};{M};{Y};{K}");

        /// <summary>
        /// Converts from the CMYK sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromCmykToRgb(string CMYKSequence)
        {
            if (CMYKSequence.Contains(';') && CMYKSequence.StartsWith("cmyk:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYKSequence[5..].Split(';');
                if (ColorSpecifierArray.Length == 4)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    int K = Convert.ToInt32(ColorSpecifierArray[3]);
                    var rgb = new Color($"cmyk:{C};{M};{Y};{K}");
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RGB: {0})", rgb.PlainSequenceTrueColor);
                    return rgb.PlainSequenceTrueColor;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</returns>
        public static string ConvertFromCmykToRyb(int C, int M, int Y, int K) =>
            ConvertFromCmykToRyb($"cmyk:{C};{M};{Y};{K}");

        /// <summary>
        /// Converts from the CMYK sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</returns>
        public static string ConvertFromCmykToRyb(string CMYKSequence)
        {
            if (CMYKSequence.Contains(';') && CMYKSequence.StartsWith("cmyk:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYKSequence[5..].Split(';');
                if (ColorSpecifierArray.Length == 4)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    int K = Convert.ToInt32(ColorSpecifierArray[3]);
                    var ryb = RybConversionTools.ConvertFrom(new Color($"cmyk:{C};{M};{Y};{K}").RGB);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", ryb.ToString());
                    return ryb.ToString();
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromCmykToCmy(int C, int M, int Y, int K) =>
            ConvertFromCmykToCmy($"cmyk:{C};{M};{Y};{K}");

        /// <summary>
        /// Converts from the CMYK sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromCmykToCmy(string CMYKSequence)
        {
            if (CMYKSequence.Contains(';') && CMYKSequence.StartsWith("cmyk:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYKSequence[5..].Split(';');
                if (ColorSpecifierArray.Length == 4)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    int K = Convert.ToInt32(ColorSpecifierArray[3]);
                    var cmy = CmyConversionTools.ConvertFrom(new Color($"cmyk:{C};{M};{Y};{K}").RGB);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMY: cmy:{0};{1};{2})", cmy.CWhole, cmy.MWhole, cmy.YWhole);
                    return $"cmy:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole}";
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromCmykToHsv(int C, int M, int Y, int K) =>
            ConvertFromCmykToHsv($"cmyk:{C};{M};{Y};{K}");

        /// <summary>
        /// Converts from the CMYK sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromCmykToHsv(string CMYKSequence)
        {
            if (CMYKSequence.Contains(';') && CMYKSequence.StartsWith("cmyk:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYKSequence[5..].Split(';');
                if (ColorSpecifierArray.Length == 4)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    int K = Convert.ToInt32(ColorSpecifierArray[3]);
                    var hsv = HsvConversionTools.ConvertFrom(new Color($"cmyk:{C};{M};{Y};{K}").RGB);
                    string hsvSequence = $"hsv:{hsv.HueWhole};{hsv.SaturationWhole};{hsv.ValueWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSV: {0})", hsvSequence);
                    return hsvSequence;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromCmykToHsl(int C, int M, int Y, int K) =>
            ConvertFromCmykToHsl($"cmyk:{C};{M};{Y};{K}");

        /// <summary>
        /// Converts from the CMYK sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromCmykToHsl(string CMYKSequence)
        {
            if (CMYKSequence.Contains(';') && CMYKSequence.StartsWith("cmyk:"))
            {
                // Split the specifier into four parts
                var ColorSpecifierArray = CMYKSequence[5..].Split(';');
                if (ColorSpecifierArray.Length == 4)
                {
                    int C = Convert.ToInt32(ColorSpecifierArray[0]);
                    int M = Convert.ToInt32(ColorSpecifierArray[1]);
                    int Y = Convert.ToInt32(ColorSpecifierArray[2]);
                    int K = Convert.ToInt32(ColorSpecifierArray[3]);
                    var rgb = new Color($"cmyk:{C};{M};{Y};{K}");
                    var hsl = HslConversionTools.ConvertFrom(rgb.RGB);
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSL: hsl:{0};{1};{2})", hsl.HueWhole, hsl.SaturationWhole, hsl.LightnessWhole);
                    return $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }
        #endregion

        #region From HSL to...
        /// <summary>
        /// Converts from the HSL sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromHslToHex(int H, int S, int L)
        {
            if (H < 0 | H > 360)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid color hue specifier. Make sure that you specify the hue as degrees, not as radians."));
            if (S < 0 | S > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid saturation color specifier."));
            if (L < 0 | L > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid lightness color specifier."));
            var rgb = new Color($"hsl:{H};{S};{L}");
            string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
            DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
            return hex;
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromHslToHex(string HSLSequence)
        {
            if (HSLSequence.Contains(';') && HSLSequence.StartsWith("hsl:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSLSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int L = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsl:{H};{S};{L}");
                    string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                    return hex;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromHslToCmyk(int H, int S, int L) =>
            ConvertFromHslToCmyk($"hsl:{H};{S};{L}");

        /// <summary>
        /// Converts from the HSL sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromHslToCmyk(string HSLSequence)
        {
            if (HSLSequence.Contains(';') && HSLSequence.StartsWith("hsl:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSLSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int L = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsl:{H};{S};{L}");
                    var cmyk = CmykConversionTools.ConvertFrom(rgb.RGB);
                    var cmy = cmyk.CMY;
                    string cmykSeq = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMYK: {0})", cmykSeq);
                    return cmykSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHslToRyb(int H, int S, int L) =>
            ConvertFromHslToRyb($"hsl:{H};{S};{L}");

        /// <summary>
        /// Converts from the HSL sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHslToRyb(string HSLSequence)
        {
            if (HSLSequence.Contains(';') && HSLSequence.StartsWith("hsl:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSLSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int L = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsl:{H};{S};{L}");
                    var ryb = RybConversionTools.ConvertFrom(rgb.RGB);
                    string rybSeq = ryb.ToString();
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", rybSeq);
                    return rybSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromHslToCmy(int H, int S, int L) =>
            ConvertFromHslToCmy($"hsl:{H};{S};{L}");

        /// <summary>
        /// Converts from the HSL sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromHslToCmy(string HSLSequence)
        {
            if (HSLSequence.Contains(';') && HSLSequence.StartsWith("hsl:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSLSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int L = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsl:{H};{S};{L}");
                    var cmy = CmyConversionTools.ConvertFrom(rgb.RGB);
                    string cmySeq = $"cmy:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMY: {0})", cmySeq);
                    return cmySeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromHslToHsv(int H, int S, int L) =>
            ConvertFromHslToHsv($"hsl:{H};{S};{L}");

        /// <summary>
        /// Converts from the HSL sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromHslToHsv(string HSLSequence)
        {
            if (HSLSequence.Contains(';') && HSLSequence.StartsWith("hsl:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSLSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int L = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsl:{H};{S};{L}");
                    var hsv = HsvConversionTools.ConvertFrom(rgb.RGB);
                    string hsvSeq = $"hsv:{hsv.HueWhole};{hsv.SaturationWhole};{hsv.ValueWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSV: {0})", hsvSeq);
                    return hsvSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHslToRgb(int H, int S, int L) =>
            ConvertFromHslToRgb($"hsl:{H};{S};{L}");

        /// <summary>
        /// Converts from the HSL sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHslToRgb(string HSLSequence)
        {
            if (HSLSequence.Contains(';') && HSLSequence.StartsWith("hsl:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSLSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int L = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsl:{H};{S};{L}");
                    var hsl = rgb;
                    string hslSeq = $"{hsl.R};{hsl.G};{hsl.B}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RGB: {0})", hslSeq);
                    return hslSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }
        #endregion

        #region From HSV to...
        /// <summary>
        /// Converts from the HSV sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="V">The value level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromHsvToHex(int H, int S, int V)
        {
            if (H < 0 | H > 360)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid color hue specifier. Make sure that you specify the hue as degrees, not as radians."));
            if (S < 0 | S > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid saturation color specifier."));
            if (V < 0 | V > 100)
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid value color specifier."));
            var rgb = new Color($"hsv:{H};{S};{V}");
            string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
            DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
            return hex;
        }

        /// <summary>
        /// Converts from the HSV sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="HSVSequence">hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromHsvToHex(string HSVSequence)
        {
            if (HSVSequence.Contains(';') && HSVSequence.StartsWith("hsv:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSVSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int V = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsv:{H};{S};{V}");
                    string hex = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                    return hex;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSV sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="V">The value level</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromHsvToCmyk(int H, int S, int V) =>
            ConvertFromHsvToCmyk($"hsv:{H};{S};{V}");

        /// <summary>
        /// Converts from the HSV sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="HSVSequence">hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromHsvToCmyk(string HSVSequence)
        {
            if (HSVSequence.Contains(';') && HSVSequence.StartsWith("hsv:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSVSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int V = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsv:{H};{S};{V}");
                    var cmyk = CmykConversionTools.ConvertFrom(rgb.RGB);
                    var cmy = cmyk.CMY;
                    string cmykSeq = $"cmyk:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole};{cmyk.KWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMYK: {0})", cmykSeq);
                    return cmykSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSV sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="V">The value level</param>
        /// <returns>ryb:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromHsvToRyb(int H, int S, int V) =>
            ConvertFromHsvToRyb($"hsv:{H};{S};{V}");

        /// <summary>
        /// Converts from the HSV sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="HSVSequence">hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</param>
        /// <returns>ryb:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromHsvToRyb(string HSVSequence)
        {
            if (HSVSequence.Contains(';') && HSVSequence.StartsWith("hsv:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSVSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int V = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsv:{H};{S};{V}");
                    var ryb = RybConversionTools.ConvertFrom(rgb.RGB);
                    string rybSeq = ryb.ToString();
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RYB: {0})", rybSeq);
                    return rybSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSV sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="V">The value level</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromHsvToCmy(int H, int S, int V) =>
            ConvertFromHsvToCmy($"hsv:{H};{S};{V}");

        /// <summary>
        /// Converts from the HSV sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="HSVSequence">hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromHsvToCmy(string HSVSequence)
        {
            if (HSVSequence.Contains(';') && HSVSequence.StartsWith("hsv:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSVSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int V = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsv:{H};{S};{V}");
                    var cmy = CmyConversionTools.ConvertFrom(rgb.RGB);
                    string cmySeq = $"cmy:{cmy.CWhole};{cmy.MWhole};{cmy.YWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (CMY: {0})", cmySeq);
                    return cmySeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSV sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="V">The value level</param>
        /// <returns>hsl:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromHsvToHsl(int H, int S, int V) =>
            ConvertFromHsvToHsl($"hsv:{H};{S};{V}");

        /// <summary>
        /// Converts from the HSV sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="HSVSequence">hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromHsvToHsl(string HSVSequence)
        {
            if (HSVSequence.Contains(';') && HSVSequence.StartsWith("hsv:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSVSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int V = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsv:{H};{S};{V}");
                    var hsl = HslConversionTools.ConvertFrom(rgb.RGB);
                    string hslSeq = $"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{hsl.LightnessWhole}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSL: {0})", hslSeq);
                    return hslSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSV sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="V">The value level</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHsvToRgb(int H, int S, int V) =>
            ConvertFromHsvToRgb($"hsv:{H};{S};{V}");

        /// <summary>
        /// Converts from the HSV sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="HSVSequence">hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHsvToRgb(string HSVSequence)
        {
            if (HSVSequence.Contains(';') && HSVSequence.StartsWith("hsv:"))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = HSVSequence[4..].Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int H = Convert.ToInt32(ColorSpecifierArray[0]);
                    int S = Convert.ToInt32(ColorSpecifierArray[1]);
                    int V = Convert.ToInt32(ColorSpecifierArray[2]);
                    var rgb = new Color($"hsv:{H};{S};{V}");
                    var hsl = rgb;
                    string hslSeq = $"{hsl.R};{hsl.G};{hsl.B}";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got color (RGB: {0})", hslSeq);
                    return hslSeq;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
            }
        }
        #endregion
    }
}
