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
using Terminaux.Colors;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Models.Parsing;

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
            if (ParsingTools.IsSpecifierAndValueValidRgbHash(Hex))
            {
                var color = new Color(Hex);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", color.R, color.G, color.B);
                return color.RGB.ToString();
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
            if (ParsingTools.IsSpecifierAndValueValidRgbHash(Hex))
            {
                var ryb = RybConversionTools.ConvertFrom(new Color(ConvertFromHexToRgb(Hex)).RGB);
                string rybSeq = ryb.ToString();
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
            if (ParsingTools.IsSpecifierAndValueValidRgbHash(Hex))
            {
                var cmyk = CmykConversionTools.ConvertFrom(new Color(ConvertFromHexToRgb(Hex)).RGB);
                string cmykSeq = cmyk.ToString();
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
            if (ParsingTools.IsSpecifierAndValueValidRgbHash(Hex))
            {
                var cmy = CmyConversionTools.ConvertFrom(new Color(ConvertFromHexToRgb(Hex)).RGB);
                string cmySeq = cmy.ToString();
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
            if (ParsingTools.IsSpecifierAndValueValidRgbHash(Hex))
            {
                var hsl = HslConversionTools.ConvertFrom(new Color(ConvertFromHexToRgb(Hex)).RGB);
                string hslSeq = hsl.ToString();
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
            if (ParsingTools.IsSpecifierAndValueValidRgbHash(Hex))
            {
                var hsv = HsvConversionTools.ConvertFrom(new Color(ConvertFromHexToRgb(Hex)).RGB);
                string hsvSeq = hsv.ToString();
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (HSV: {0})", hsvSeq);
                return hsvSeq;
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the YIQ sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromHexToYiq(string Hex)
        {
            if (ParsingTools.IsSpecifierAndValueValidRgbHash(Hex))
            {
                var yiq = YiqConversionTools.ConvertFrom(new Color(ConvertFromHexToRgb(Hex)).RGB);
                string yiqSeq = yiq.ToString();
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (YIQ: {0})", yiqSeq);
                return yiqSeq;
            }
            else
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid hex color specifier."));
        }

        /// <summary>
        /// Converts from the hexadecimal representation of a color to the YUV sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromHexToYuv(string Hex)
        {
            if (ParsingTools.IsSpecifierAndValueValidRgbHash(Hex))
            {
                var yuv = YuvConversionTools.ConvertFrom(new Color(ConvertFromHexToRgb(Hex)).RGB);
                string yuvSeq = yuv.ToString();
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (YUV: {0})", yuvSeq);
                return yuvSeq;
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
            string hex = new Color(R, G, B).Hex;
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
            if (ParsingTools.IsSpecifierAndValueValid(RGBSequence))
            {
                string hex = new Color(RGBSequence).Hex;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                return hex;
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
            if (ParsingTools.IsSpecifierAndValueValid(RGBSequence))
            {
                var rgb = new Color(RGBSequence).RGB;
                var ryb = RybConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", ryb.ToString());
                return ryb.ToString();
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
            if (ParsingTools.IsSpecifierAndValueValid(RGBSequence))
            {
                var rgb = new Color(RGBSequence).RGB;
                var cmyk = CmykConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmyk.ToString());
                return cmyk.ToString();
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
            if (ParsingTools.IsSpecifierAndValueValid(RGBSequence))
            {
                var rgb = new Color(RGBSequence).RGB;
                var cmy = CmyConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmy.ToString());
                return cmy.ToString();
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
            if (ParsingTools.IsSpecifierAndValueValid(RGBSequence))
            {
                var rgb = new Color(RGBSequence).RGB;
                var hsl = HslConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsl.ToString());
                return hsl.ToString();
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
            if (ParsingTools.IsSpecifierAndValueValid(RGBSequence))
            {
                var rgb = new Color(RGBSequence).RGB;
                var hsv = HsvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsv.ToString());
                return hsv.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RGB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromRgbToYiq(int R, int G, int B) =>
            ConvertFromRgbToYiq($"{R};{G};{B}");

        /// <summary>
        /// Converts from the RGB sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromRgbToYiq(string RGBSequence)
        {
            if (ParsingTools.IsSpecifierAndValueValid(RGBSequence))
            {
                var rgb = new Color(RGBSequence).RGB;
                var yiq = YiqConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yiq.ToString());
                return yiq.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RGB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromRgbToYuv(int R, int G, int B) =>
            ConvertFromRgbToYuv($"{R};{G};{B}");

        /// <summary>
        /// Converts from the RGB sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromRgbToYuv(string RGBSequence)
        {
            if (ParsingTools.IsSpecifierAndValueValid(RGBSequence))
            {
                var rgb = new Color(RGBSequence).RGB;
                var yuv = YuvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yuv.ToString());
                return yuv.ToString();
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
            if (RybParsingTools.IsSpecifierAndValueValid(RYBSequence))
            {
                string hex = new Color(RYBSequence).Hex;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                return hex;
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
            if (RybParsingTools.IsSpecifierAndValueValid(RYBSequence))
            {
                var rgb = new Color(RYBSequence).RGB;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", rgb.ToString());
                return rgb.ToString();
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
            if (RybParsingTools.IsSpecifierAndValueValid(RYBSequence))
            {
                var rgb = new Color(RYBSequence).RGB;
                var cmyk = CmykConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmyk.ToString());
                return cmyk.ToString();
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
        /// <returns>cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromRybToCmy(int R, int Y, int B) =>
            ConvertFromRybToCmy($"ryb:{R};{Y};{B}");

        /// <summary>
        /// Converts from the RYB sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="RYBSequence">ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</param>
        /// <returns>cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromRybToCmy(string RYBSequence)
        {
            if (RybParsingTools.IsSpecifierAndValueValid(RYBSequence))
            {
                var rgb = new Color(RYBSequence).RGB;
                var cmy = CmyConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmy.ToString());
                return cmy.ToString();
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
            if (RybParsingTools.IsSpecifierAndValueValid(RYBSequence))
            {
                var rgb = new Color(RYBSequence).RGB;
                var hsl = HslConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsl.ToString());
                return hsl.ToString();
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
            if (RybParsingTools.IsSpecifierAndValueValid(RYBSequence))
            {
                var rgb = new Color(RYBSequence).RGB;
                var hsv = HsvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsv.ToString());
                return hsv.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RYB sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="B">The blue level</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromRybToYiq(int R, int Y, int B) =>
            ConvertFromRybToYiq($"ryb:{R};{Y};{B}");

        /// <summary>
        /// Converts from the RYB sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="RYBSequence">ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromRybToYiq(string RYBSequence)
        {
            if (RybParsingTools.IsSpecifierAndValueValid(RYBSequence))
            {
                var rgb = new Color(RYBSequence).RGB;
                var yiq = YiqConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yiq.ToString());
                return yiq.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid RYB color specifier."));
            }
        }

        /// <summary>
        /// Converts from the RYB sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="B">The blue level</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromRybToYuv(int R, int Y, int B) =>
            ConvertFromRybToYuv($"ryb:{R};{Y};{B}");

        /// <summary>
        /// Converts from the RYB sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="RYBSequence">ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromRybToYuv(string RYBSequence)
        {
            if (RybParsingTools.IsSpecifierAndValueValid(RYBSequence))
            {
                var rgb = new Color(RYBSequence).RGB;
                var yuv = YuvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yuv.ToString());
                return yuv.ToString();
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
        public static string ConvertFromCmyToHex(int C, int M, int Y) =>
            ConvertFromCmyToHex($"cmy:{C};{M};{Y}");

        /// <summary>
        /// Converts from the CMY sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="CMYSequence">cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromCmyToHex(string CMYSequence)
        {
            if (CmyParsingTools.IsSpecifierAndValueValid(CMYSequence))
            {
                string hex = new Color(CMYSequence).Hex;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                return hex;
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
            if (CmyParsingTools.IsSpecifierAndValueValid(CMYSequence))
            {
                var rgb = new Color(CMYSequence).RGB;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", rgb.ToString());
                return rgb.ToString();
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
            if (CmyParsingTools.IsSpecifierAndValueValid(CMYSequence))
            {
                var rgb = new Color(CMYSequence).RGB;
                var ryb = RybConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", ryb.ToString());
                return ryb.ToString();
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
            if (CmyParsingTools.IsSpecifierAndValueValid(CMYSequence))
            {
                var rgb = new Color(CMYSequence).RGB;
                var hsl = HslConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsl.ToString());
                return hsl.ToString();
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
            if (CmyParsingTools.IsSpecifierAndValueValid(CMYSequence))
            {
                var rgb = new Color(CMYSequence).RGB;
                var hsv = HsvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsv.ToString());
                return hsv.ToString();
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
            if (CmyParsingTools.IsSpecifierAndValueValid(CMYSequence))
            {
                var rgb = new Color(CMYSequence).RGB;
                var cmyk = CmykConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmyk.ToString());
                return cmyk.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMY sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromCmyToYiq(int C, int M, int Y) =>
            ConvertFromCmyToYiq($"cmy:{C};{M};{Y}");

        /// <summary>
        /// Converts from the CMY sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="CMYSequence">cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromCmyToYiq(string CMYSequence)
        {
            if (CmyParsingTools.IsSpecifierAndValueValid(CMYSequence))
            {
                var rgb = new Color(CMYSequence).RGB;
                var yiq = YiqConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yiq.ToString());
                return yiq.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMY color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMY sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromCmyToYuv(int C, int M, int Y) =>
            ConvertFromCmyToYuv($"cmy:{C};{M};{Y}");

        /// <summary>
        /// Converts from the CMY sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="CMYSequence">cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromCmyToYuv(string CMYSequence)
        {
            if (CmyParsingTools.IsSpecifierAndValueValid(CMYSequence))
            {
                var rgb = new Color(CMYSequence).RGB;
                var yuv = YuvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yuv.ToString());
                return yuv.ToString();
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
        public static string ConvertFromCmykToHex(int C, int M, int Y, int K) =>
            ConvertFromCmykToHex($"cmyk:{C};{M};{Y};{K}");

        /// <summary>
        /// Converts from the CMYK sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromCmykToHex(string CMYKSequence)
        {
            if (CmykParsingTools.IsSpecifierAndValueValid(CMYKSequence))
            {
                string hex = new Color(CMYKSequence).Hex;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color (#RRGGBB: {0})", hex);
                return hex;
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
            if (CmykParsingTools.IsSpecifierAndValueValid(CMYKSequence))
            {
                var rgb = new Color(CMYKSequence).RGB;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", rgb.ToString());
                return rgb.ToString();
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
            if (CmykParsingTools.IsSpecifierAndValueValid(CMYKSequence))
            {
                var rgb = new Color(CMYKSequence).RGB;
                var ryb = RybConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", ryb.ToString());
                return ryb.ToString();
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
            if (CmykParsingTools.IsSpecifierAndValueValid(CMYKSequence))
            {
                var rgb = new Color(CMYKSequence).RGB;
                var cmy = CmyConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmy.ToString());
                return cmy.ToString();
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
            if (CmykParsingTools.IsSpecifierAndValueValid(CMYKSequence))
            {
                var rgb = new Color(CMYKSequence).RGB;
                var hsv = HsvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsv.ToString());
                return hsv.ToString();
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
            if (CmykParsingTools.IsSpecifierAndValueValid(CMYKSequence))
            {
                var rgb = new Color(CMYKSequence).RGB;
                var hsl = HslConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsl.ToString());
                return hsl.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromCmykToYiq(int C, int M, int Y, int K) =>
            ConvertFromCmykToYiq($"cmyk:{C};{M};{Y};{K}");

        /// <summary>
        /// Converts from the CMYK sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromCmykToYiq(string CMYKSequence)
        {
            if (CmykParsingTools.IsSpecifierAndValueValid(CMYKSequence))
            {
                var rgb = new Color(CMYKSequence).RGB;
                var yiq = YiqConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yiq.ToString());
                return yiq.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid CMYK color specifier."));
            }
        }

        /// <summary>
        /// Converts from the CMYK sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="C">The cyan level</param>
        /// <param name="M">The magenta level</param>
        /// <param name="Y">The yellow level</param>
        /// <param name="K">The black key level</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromCmykToYuv(int C, int M, int Y, int K) =>
            ConvertFromCmykToYuv($"cmyk:{C};{M};{Y};{K}");

        /// <summary>
        /// Converts from the CMYK sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="CMYKSequence">cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromCmykToYuv(string CMYKSequence)
        {
            if (CmykParsingTools.IsSpecifierAndValueValid(CMYKSequence))
            {
                var rgb = new Color(CMYKSequence).RGB;
                var yuv = YuvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yuv.ToString());
                return yuv.ToString();
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
        public static string ConvertFromHslToHex(int H, int S, int L) =>
            ConvertFromHslToHex($"hsl:{H};{S};{L}");

        /// <summary>
        /// Converts from the HSL sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromHslToHex(string HSLSequence)
        {
            if (HslParsingTools.IsSpecifierAndValueValid(HSLSequence))
            {
                string hex = new Color(HSLSequence).Hex;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hex);
                return hex;
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
            if (HslParsingTools.IsSpecifierAndValueValid(HSLSequence))
            {
                var rgb = new Color(HSLSequence).RGB;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", rgb.ToString());
                return rgb.ToString();
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
            if (HslParsingTools.IsSpecifierAndValueValid(HSLSequence))
            {
                var rgb = new Color(HSLSequence).RGB;
                var ryb = RybConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", ryb.ToString());
                return ryb.ToString();
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
            if (HslParsingTools.IsSpecifierAndValueValid(HSLSequence))
            {
                var rgb = new Color(HSLSequence).RGB;
                var cmyk = CmykConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmyk.ToString());
                return cmyk.ToString();
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
            if (HslParsingTools.IsSpecifierAndValueValid(HSLSequence))
            {
                var rgb = new Color(HSLSequence).RGB;
                var cmy = CmyConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmy.ToString());
                return cmy.ToString();
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
            if (HslParsingTools.IsSpecifierAndValueValid(HSLSequence))
            {
                var rgb = new Color(HSLSequence).RGB;
                var hsv = HsvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsv.ToString());
                return hsv.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromHslToYiq(int H, int S, int L) =>
            ConvertFromHslToYiq($"hsl:{H};{S};{L}");

        /// <summary>
        /// Converts from the HSL sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromHslToYiq(string HSLSequence)
        {
            if (HslParsingTools.IsSpecifierAndValueValid(HSLSequence))
            {
                var rgb = new Color(HSLSequence).RGB;
                var yiq = YiqConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yiq.ToString());
                return yiq.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSL color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSL sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="L">The luminance (lightness) level</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromHslToYuv(int H, int S, int L) =>
            ConvertFromHslToYuv($"hsl:{H};{S};{L}");

        /// <summary>
        /// Converts from the HSL sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="HSLSequence">hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromHslToYuv(string HSLSequence)
        {
            if (HslParsingTools.IsSpecifierAndValueValid(HSLSequence))
            {
                var rgb = new Color(HSLSequence).RGB;
                var yuv = YuvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yuv.ToString());
                return yuv.ToString();
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
        public static string ConvertFromHsvToHex(int H, int S, int V) =>
            ConvertFromHsvToHex($"hsv:{H};{S};{V}");

        /// <summary>
        /// Converts from the HSV sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="HSVSequence">hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromHsvToHex(string HSVSequence)
        {
            if (HsvParsingTools.IsSpecifierAndValueValid(HSVSequence))
            {
                string hex = new Color(HSVSequence).Hex;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hex);
                return hex;
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
            if (HsvParsingTools.IsSpecifierAndValueValid(HSVSequence))
            {
                var rgb = new Color(HSVSequence).RGB;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", rgb.ToString());
                return rgb.ToString();
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
            if (HsvParsingTools.IsSpecifierAndValueValid(HSVSequence))
            {
                var rgb = new Color(HSVSequence).RGB;
                var ryb = RybConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", ryb.ToString());
                return ryb.ToString();
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
            if (HsvParsingTools.IsSpecifierAndValueValid(HSVSequence))
            {
                var rgb = new Color(HSVSequence).RGB;
                var cmyk = CmykConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmyk.ToString());
                return cmyk.ToString();
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
            if (HsvParsingTools.IsSpecifierAndValueValid(HSVSequence))
            {
                var rgb = new Color(HSVSequence).RGB;
                var cmy = CmyConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmy.ToString());
                return cmy.ToString();
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
            if (HsvParsingTools.IsSpecifierAndValueValid(HSVSequence))
            {
                var rgb = new Color(HSVSequence).RGB;
                var hsl = HslConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsl.ToString());
                return hsl.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSV sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="V">The value level</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromHsvToYiq(int H, int S, int V) =>
            ConvertFromHsvToYiq($"hsv:{H};{S};{V}");

        /// <summary>
        /// Converts from the HSV sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="HSVSequence">hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</param>
        /// <returns>yiq:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromHsvToYiq(string HSVSequence)
        {
            if (HsvParsingTools.IsSpecifierAndValueValid(HSVSequence))
            {
                var rgb = new Color(HSVSequence).RGB;
                var yiq = YiqConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yiq.ToString());
                return yiq.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the HSV sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="H">The hue level</param>
        /// <param name="S">The saturation level</param>
        /// <param name="V">The value level</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromHsvToYuv(int H, int S, int V) =>
            ConvertFromHsvToYuv($"hsv:{H};{S};{V}");

        /// <summary>
        /// Converts from the HSV sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="HSVSequence">hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</param>
        /// <returns>yuv:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromHsvToYuv(string HSVSequence)
        {
            if (HsvParsingTools.IsSpecifierAndValueValid(HSVSequence))
            {
                var rgb = new Color(HSVSequence).RGB;
                var yuv = YuvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yuv.ToString());
                return yuv.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid HSV color specifier."));
            }
        }
        #endregion

        #region From YIQ to...
        /// <summary>
        /// Converts from the YIQ sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="I">The I component (in-phase)</param>
        /// <param name="Q">The Q component (quadrant)</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromYiqToHex(int Y, int I, int Q) =>
            ConvertFromYiqToHex($"yiq:{Y};{I};{Q}");

        /// <summary>
        /// Converts from the YIQ sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="YIQSequence">yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromYiqToHex(string YIQSequence)
        {
            if (YiqParsingTools.IsSpecifierAndValueValid(YIQSequence))
            {
                string hex = new Color(YIQSequence).Hex;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hex);
                return hex;
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YIQ color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YIQ sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="I">The I component (in-phase)</param>
        /// <param name="Q">The Q component (quadrant)</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromYiqToRgb(int Y, int I, int Q) =>
            ConvertFromYiqToRgb($"yiq:{Y};{I};{Q}");

        /// <summary>
        /// Converts from the YIQ sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="YIQSequence">yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromYiqToRgb(string YIQSequence)
        {
            if (YiqParsingTools.IsSpecifierAndValueValid(YIQSequence))
            {
                var rgb = new Color(YIQSequence).RGB;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", rgb.ToString());
                return rgb.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YIQ color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YIQ sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="I">The I component (in-phase)</param>
        /// <param name="Q">The Q component (quadrant)</param>
        /// <returns>ryb:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromYiqToRyb(int Y, int I, int Q) =>
            ConvertFromYiqToRyb($"yiq:{Y};{I};{Q}");

        /// <summary>
        /// Converts from the YIQ sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="YIQSequence">yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>ryb:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromYiqToRyb(string YIQSequence)
        {
            if (YiqParsingTools.IsSpecifierAndValueValid(YIQSequence))
            {
                var rgb = new Color(YIQSequence).RGB;
                var ryb = RybConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", ryb.ToString());
                return ryb.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YIQ color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YIQ sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="I">The I component (in-phase)</param>
        /// <param name="Q">The Q component (quadrant)</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromYiqToCmyk(int Y, int I, int Q) =>
            ConvertFromYiqToCmyk($"yiq:{Y};{I};{Q}");

        /// <summary>
        /// Converts from the YIQ sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="YIQSequence">yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromYiqToCmyk(string YIQSequence)
        {
            if (YiqParsingTools.IsSpecifierAndValueValid(YIQSequence))
            {
                var rgb = new Color(YIQSequence).RGB;
                var cmyk = CmykConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmyk.ToString());
                return cmyk.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YIQ color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YIQ sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="I">The I component (in-phase)</param>
        /// <param name="Q">The Q component (quadrant)</param>
        /// <returns>cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromYiqToCmy(int Y, int I, int Q) =>
            ConvertFromYiqToCmy($"yiq:{Y};{I};{Q}");

        /// <summary>
        /// Converts from the YIQ sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="YIQSequence">yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromYiqToCmy(string YIQSequence)
        {
            if (YiqParsingTools.IsSpecifierAndValueValid(YIQSequence))
            {
                var rgb = new Color(YIQSequence).RGB;
                var cmy = CmyConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmy.ToString());
                return cmy.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YIQ color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YIQ sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="I">The I component (in-phase)</param>
        /// <param name="Q">The Q component (quadrant)</param>
        /// <returns>hsl:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromYiqToHsl(int Y, int I, int Q) =>
            ConvertFromYiqToHsl($"yiq:{Y};{I};{Q}");

        /// <summary>
        /// Converts from the YIQ sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="YIQSequence">yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromYiqToHsl(string YIQSequence)
        {
            if (YiqParsingTools.IsSpecifierAndValueValid(YIQSequence))
            {
                var rgb = new Color(YIQSequence).RGB;
                var hsl = HslConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsl.ToString());
                return hsl.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YIQ color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YIQ sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="I">The I component (in-phase)</param>
        /// <param name="Q">The Q component (quadrant)</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromYiqToHsv(int Y, int I, int Q) =>
            ConvertFromYiqToHsv($"yiq:{Y};{I};{Q}");

        /// <summary>
        /// Converts from the YIQ sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="YIQSequence">yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromYiqToHsv(string YIQSequence)
        {
            if (YiqParsingTools.IsSpecifierAndValueValid(YIQSequence))
            {
                var rgb = new Color(YIQSequence).RGB;
                var hsv = HsvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsv.ToString());
                return hsv.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YIQ color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YIQ sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="I">The I component (in-phase)</param>
        /// <param name="Q">The Q component (quadrant)</param>
        /// <returns>yuv:&lt;Y&gt;;&lt;U&gt;;&lt;V&gt;</returns>
        public static string ConvertFromYiqToYuv(int Y, int I, int Q) =>
            ConvertFromYiqToYuv($"yiq:{Y};{I};{Q}");

        /// <summary>
        /// Converts from the YIQ sequence of a color to the YUV sequence
        /// </summary>
        /// <param name="YIQSequence">yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>yuv:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromYiqToYuv(string YIQSequence)
        {
            if (YiqParsingTools.IsSpecifierAndValueValid(YIQSequence))
            {
                var rgb = new Color(YIQSequence).RGB;
                var yuv = YuvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yuv.ToString());
                return yuv.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YIQ color specifier."));
            }
        }
        #endregion

        #region From YUV to...
        /// <summary>
        /// Converts from the YUV sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="U">The U component (chroma)</param>
        /// <param name="V">The V component (chroma)</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromYuvToHex(int Y, int U, int V) =>
            ConvertFromYuvToHex($"yuv:{Y};{U};{V}");

        /// <summary>
        /// Converts from the YUV sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="YUVSequence">yuv:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromYuvToHex(string YUVSequence)
        {
            if (YuvParsingTools.IsSpecifierAndValueValid(YUVSequence))
            {
                string hex = new Color(YUVSequence).Hex;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hex);
                return hex;
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YUV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YUV sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="U">The U component (chroma)</param>
        /// <param name="V">The V component (chroma)</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromYuvToRgb(int Y, int U, int V) =>
            ConvertFromYuvToRgb($"yuv:{Y};{U};{V}");

        /// <summary>
        /// Converts from the YUV sequence of a color to the RGB sequence
        /// </summary>
        /// <param name="YUVSequence">yuv:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromYuvToRgb(string YUVSequence)
        {
            if (YuvParsingTools.IsSpecifierAndValueValid(YUVSequence))
            {
                var rgb = new Color(YUVSequence).RGB;
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", rgb.ToString());
                return rgb.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YUV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YUV sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="U">The U component (chroma)</param>
        /// <param name="V">The V component (chroma)</param>
        /// <returns>ryb:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromYuvToRyb(int Y, int U, int V) =>
            ConvertFromYuvToRyb($"yuv:{Y};{U};{V}");

        /// <summary>
        /// Converts from the YUV sequence of a color to the RYB sequence
        /// </summary>
        /// <param name="YUVSequence">yuv:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>ryb:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromYuvToRyb(string YUVSequence)
        {
            if (YuvParsingTools.IsSpecifierAndValueValid(YUVSequence))
            {
                var rgb = new Color(YUVSequence).RGB;
                var ryb = RybConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", ryb.ToString());
                return ryb.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YUV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YUV sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="U">The U component (chroma)</param>
        /// <param name="V">The V component (chroma)</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromYuvToCmyk(int Y, int U, int V) =>
            ConvertFromYuvToCmyk($"yuv:{Y};{U};{V}");

        /// <summary>
        /// Converts from the YUV sequence of a color to the CMYK sequence
        /// </summary>
        /// <param name="YUVSequence">yuv:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;</returns>
        public static string ConvertFromYuvToCmyk(string YUVSequence)
        {
            if (YuvParsingTools.IsSpecifierAndValueValid(YUVSequence))
            {
                var rgb = new Color(YUVSequence).RGB;
                var cmyk = CmykConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmyk.ToString());
                return cmyk.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YUV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YUV sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="U">The U component (chroma)</param>
        /// <param name="V">The V component (chroma)</param>
        /// <returns>cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromYuvToCmy(int Y, int U, int V) =>
            ConvertFromYuvToCmy($"yuv:{Y};{U};{V}");

        /// <summary>
        /// Converts from the YUV sequence of a color to the CMY sequence
        /// </summary>
        /// <param name="YUVSequence">yuv:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromYuvToCmy(string YUVSequence)
        {
            if (YuvParsingTools.IsSpecifierAndValueValid(YUVSequence))
            {
                var rgb = new Color(YUVSequence).RGB;
                var cmy = CmyConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", cmy.ToString());
                return cmy.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YUV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YUV sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="U">The U component (chroma)</param>
        /// <param name="V">The V component (chroma)</param>
        /// <returns>hsl:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;</returns>
        public static string ConvertFromYuvToHsl(int Y, int U, int V) =>
            ConvertFromYuvToHsl($"yuv:{Y};{U};{V}");

        /// <summary>
        /// Converts from the YUV sequence of a color to the HSL sequence
        /// </summary>
        /// <param name="YUVSequence">yuv:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;</returns>
        public static string ConvertFromYuvToHsl(string YUVSequence)
        {
            if (YuvParsingTools.IsSpecifierAndValueValid(YUVSequence))
            {
                var rgb = new Color(YUVSequence).RGB;
                var hsl = HslConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsl.ToString());
                return hsl.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YUV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YUV sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="U">The U component (chroma)</param>
        /// <param name="V">The V component (chroma)</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromYuvToHsv(int Y, int U, int V) =>
            ConvertFromYuvToHsv($"yuv:{Y};{U};{V}");

        /// <summary>
        /// Converts from the YUV sequence of a color to the HSV sequence
        /// </summary>
        /// <param name="YUVSequence">yuv:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;</returns>
        public static string ConvertFromYuvToHsv(string YUVSequence)
        {
            if (YuvParsingTools.IsSpecifierAndValueValid(YUVSequence))
            {
                var rgb = new Color(YUVSequence).RGB;
                var hsv = HsvConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", hsv.ToString());
                return hsv.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YUV color specifier."));
            }
        }

        /// <summary>
        /// Converts from the YUV sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="Y">The Y component (luma)</param>
        /// <param name="U">The U component (chroma)</param>
        /// <param name="V">The V component (chroma)</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromYuvToYiq(int Y, int U, int V) =>
            ConvertFromYuvToYiq($"yuv:{Y};{U};{V}");

        /// <summary>
        /// Converts from the YUV sequence of a color to the YIQ sequence
        /// </summary>
        /// <param name="YUVSequence">yuv:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</param>
        /// <returns>yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;</returns>
        public static string ConvertFromYuvToYiq(string YUVSequence)
        {
            if (YuvParsingTools.IsSpecifierAndValueValid(YUVSequence))
            {
                var rgb = new Color(YUVSequence).RGB;
                var yiq = YiqConversionTools.ConvertFrom(rgb);
                DebugWriter.WriteDebug(DebugLevel.I, "Got color {0}", yiq.ToString());
                return yiq.ToString();
            }
            else
            {
                throw new KernelException(KernelExceptionType.Color, Translate.DoTranslation("Invalid YUV color specifier."));
            }
        }
        #endregion
    }
}
