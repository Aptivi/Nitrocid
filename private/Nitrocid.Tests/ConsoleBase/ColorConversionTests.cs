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

using Nitrocid.ConsoleBase.Colors;
using NUnit.Framework;
using Shouldly;
using System;

namespace Nitrocid.Tests.ConsoleBase
{

    [TestFixture]
    public class ColorConversionTests
    {

        /// <summary>
        /// Tests trying to convert from hex to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHexToRgb()
        {
            Console.WriteLine("Converting #0F0F0F...");
            KernelColorConversionTools.ConvertFromHexToRgb("#0F0F0F").ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from hex to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHexToRyb()
        {
            Console.WriteLine("Converting #0F0F0F...");
            KernelColorConversionTools.ConvertFromHexToRyb("#0F0F0F").ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from hex to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHexToCmyk()
        {
            Console.WriteLine("Converting #0F0F0F...");
            KernelColorConversionTools.ConvertFromHexToCmyk("#0F0F0F").ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from hex to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHexToCmy()
        {
            Console.WriteLine("Converting #0F0F0F...");
            KernelColorConversionTools.ConvertFromHexToCmy("#0F0F0F").ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from hex to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHexToHsl()
        {
            Console.WriteLine("Converting #0F0F0F...");
            KernelColorConversionTools.ConvertFromHexToHsl("#0F0F0F").ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from hex to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHexToHsv()
        {
            Console.WriteLine("Converting #0F0F0F...");
            KernelColorConversionTools.ConvertFromHexToHsv("#0F0F0F").ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from hex to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHexToYiq()
        {
            Console.WriteLine("Converting #0F0F0F...");
            KernelColorConversionTools.ConvertFromHexToYiq("#0F0F0F").ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from hex to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHexToYuv()
        {
            Console.WriteLine("Converting #0F0F0F...");
            KernelColorConversionTools.ConvertFromHexToYuv("#0F0F0F").ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbSequenceToHex()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRgbToHex("15;15;15").ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbNumbersToHex()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRgbToHex(15, 15, 15).ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from RYB sequence to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybSequenceToHex()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRybToHex("ryb:15;15;15").ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from RYB numbers to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybNumbersToHex()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRybToHex(15, 15, 15).ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from CMYK sequence to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykSequenceToHex()
        {
            Console.WriteLine("Converting 0;0;0;94...");
            KernelColorConversionTools.ConvertFromCmykToHex("cmyk:0;0;0;94").ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from CMYK numbers to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykNumbersToHex()
        {
            Console.WriteLine("Converting 0, 0, 0, 94...");
            KernelColorConversionTools.ConvertFromCmykToHex(0, 0, 0, 94).ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from CMY sequence to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmySequenceToHex()
        {
            Console.WriteLine("Converting 94;94;94...");
            KernelColorConversionTools.ConvertFromCmyToHex("cmy:94;94;94").ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from CMY numbers to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmyNumbersToHex()
        {
            Console.WriteLine("Converting 94, 94, 94...");
            KernelColorConversionTools.ConvertFromCmyToHex(94, 94, 94).ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from HSL sequence to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHSLSequenceToHex()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHslToHex("hsl:0;0;5").ShouldBe("#0D0D0D");
        }

        /// <summary>
        /// Tests trying to convert from HSL numbers to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHSLNumbersToHex()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHslToHex(0, 0, 5).ShouldBe("#0D0D0D");
        }

        /// <summary>
        /// Tests trying to convert from HSV sequence to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHSVSequenceToHex()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHsvToHex("hsv:0;0;5").ShouldBe("#0D0D0D");
        }

        /// <summary>
        /// Tests trying to convert from HSV numbers to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHSVNumbersToHex()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHsvToHex(0, 0, 5).ShouldBe("#0D0D0D");
        }

        /// <summary>
        /// Tests trying to convert from YIQ sequence to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqSequenceToHex()
        {
            Console.WriteLine("Converting 0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToHex("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18").ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from YIQ numbers to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqNumbersToHex()
        {
            Console.WriteLine("Converting 0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToHex(0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18).ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from YUV sequence to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        // TODO: Deal with the below ignore attribute on Terminaux 2.5.0.
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvSequenceToHex()
        {
            Console.WriteLine("Converting 15;128;128...");
            KernelColorConversionTools.ConvertFromYuvToHex("yuv:15;128;128").ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from YUV numbers to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvNumbersToHex()
        {
            Console.WriteLine("Converting 15, 128, 128...");
            KernelColorConversionTools.ConvertFromYuvToHex(15, 128, 128).ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbSequenceToCmyk()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRgbToCmyk("15;15;15").ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbNumbersToCmyk()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRgbToCmyk(15, 15, 15).ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbSequenceToRyb()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRgbToRyb("15;15;15").ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbNumbersToRyb()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRgbToRyb(15, 15, 15).ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbSequenceToCmy()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRgbToCmy("15;15;15").ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbNumbersToCmy()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRgbToCmy(15, 15, 15).ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbSequenceToHsl()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRgbToHsl("15;15;15").ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbNumbersToHsl()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRgbToHsl(15, 15, 15).ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbSequenceToHsv()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRgbToHsv("15;15;15").ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbNumbersToHsv()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRgbToHsv(15, 15, 15).ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbSequenceToYiq()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRgbToYiq("15;15;15").ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbNumbersToYiq()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRgbToYiq(15, 15, 15).ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbSequenceToYuv()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRgbToYuv("15;15;15").ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbNumbersToYuv()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRgbToYuv(15, 15, 15).ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from RYB sequence to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybSequenceToCmyk()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRybToCmyk("ryb:15;15;15").ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from RYB numbers to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybNumbersToCmyk()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRybToCmyk(15, 15, 15).ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from RYB sequence to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybSequenceToRgb()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRybToRgb("ryb:15;15;15").ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from RYB numbers to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybNumbersToRgb()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRybToRgb(15, 15, 15).ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from RYB sequence to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybSequenceToCmy()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRybToCmy("ryb:15;15;15").ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from RYB numbers to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybNumbersToCmy()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRybToCmy(15, 15, 15).ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from RYB sequence to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybSequenceToHsl()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRybToHsl("ryb:15;15;15").ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from RYB numbers to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybNumbersToHsl()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRybToHsl(15, 15, 15).ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from RYB sequence to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybSequenceToHsv()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRybToHsv("ryb:15;15;15").ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from RYB numbers to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybNumbersToHsv()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRybToHsv(15, 15, 15).ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from RYB sequence to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybSequenceToYiq()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRybToYiq("ryb:15;15;15").ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from RYB numbers to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybNumbersToYiq()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRybToYiq(15, 15, 15).ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from RYB sequence to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybSequenceToYuv()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRybToYuv("ryb:15;15;15").ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from RYB numbers to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRybNumbersToYuv()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRybToYuv(15, 15, 15).ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from HSL sequence to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslSequenceToRgb()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHslToRgb("hsl:0;0;5").ShouldBe("13;13;13");
        }

        /// <summary>
        /// Tests trying to convert from HSL numbers to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslNumbersToRgb()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHslToRgb(0, 0, 5).ShouldBe("13;13;13");
        }

        /// <summary>
        /// Tests trying to convert from HSL sequence to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslSequenceToRyb()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHslToRyb("hsl:0;0;5").ShouldBe("ryb:13;13;13");
        }

        /// <summary>
        /// Tests trying to convert from HSL numbers to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslNumbersToRyb()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHslToRyb(0, 0, 5).ShouldBe("ryb:13;13;13");
        }

        /// <summary>
        /// Tests trying to convert from HSL sequence to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslSequenceToCmyk()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHslToCmyk("hsl:0;0;5").ShouldBe("cmyk:0;0;0;95");
        }

        /// <summary>
        /// Tests trying to convert from HSL numbers to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslNumbersToCmyk()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHslToCmyk(0, 0, 5).ShouldBe("cmyk:0;0;0;95");
        }

        /// <summary>
        /// Tests trying to convert from HSL sequence to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslSequenceToCmy()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHslToCmy("hsl:0;0;5").ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from HSL numbers to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslNumbersToCmy()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHslToCmy(0, 0, 5).ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from HSL sequence to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslSequenceToHsv()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHslToHsv("hsl:0;0;5").ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from HSL numbers to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslNumbersToHsv()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHslToHsv(0, 0, 5).ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from HSL sequence to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslSequenceToYiq()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHslToYiq("hsl:0;0;5").ShouldBe("yiq:0.05098039215686274;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from HSL numbers to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslNumbersToYiq()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHslToYiq(0, 0, 5).ShouldBe("yiq:0.05098039215686274;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from HSL sequence to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslSequenceToYuv()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHslToYuv("hsl:0;0;5").ShouldBe("yuv:12.999999999999998;128;128");
        }

        /// <summary>
        /// Tests trying to convert from HSL numbers to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslNumbersToYuv()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHslToYuv(0, 0, 5).ShouldBe("yuv:12.999999999999998;128;128");
        }

        /// <summary>
        /// Tests trying to convert from HSV sequence to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvSequenceToRgb()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHsvToRgb("hsv:0;0;5").ShouldBe("13;13;13");
        }

        /// <summary>
        /// Tests trying to convert from HSV numbers to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvNumbersToRgb()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHsvToRgb(0, 0, 5).ShouldBe("13;13;13");
        }

        /// <summary>
        /// Tests trying to convert from HSV sequence to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvSequenceToRyb()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHsvToRyb("hsv:0;0;5").ShouldBe("ryb:13;13;13");
        }

        /// <summary>
        /// Tests trying to convert from HSV numbers to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvNumbersToRyb()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHsvToRyb(0, 0, 5).ShouldBe("ryb:13;13;13");
        }

        /// <summary>
        /// Tests trying to convert from HSV sequence to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvSequenceToCmyk()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHsvToCmyk("hsv:0;0;5").ShouldBe("cmyk:0;0;0;95");
        }

        /// <summary>
        /// Tests trying to convert from HSV numbers to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvNumbersToCmyk()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHsvToCmyk(0, 0, 5).ShouldBe("cmyk:0;0;0;95");
        }

        /// <summary>
        /// Tests trying to convert from HSV sequence to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvSequenceToCmy()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHsvToCmy("hsv:0;0;5").ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from HSV numbers to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvNumbersToCmy()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHsvToCmy(0, 0, 5).ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from HSV sequence to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvSequenceToHsl()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHsvToHsl("hsv:0;0;5").ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from HSV numbers to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvNumbersToHsl()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHsvToHsl(0, 0, 5).ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from HSV sequence to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvSequenceToYiq()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHsvToYiq("hsv:0;0;5").ShouldBe("yiq:0.05098039215686274;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from HSV numbers to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvNumbersToYiq()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHsvToYiq(0, 0, 5).ShouldBe("yiq:0.05098039215686274;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from HSV sequence to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvSequenceToYuv()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHsvToYuv("hsv:0;0;5").ShouldBe("yuv:12.999999999999998;128;128");
        }

        /// <summary>
        /// Tests trying to convert from HSV numbers to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHsvNumbersToYuv()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHsvToYuv(0, 0, 5).ShouldBe("yuv:12.999999999999998;128;128");
        }

        /// <summary>
        /// Tests trying to convert from CMYK sequence to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykSequenceToRgb()
        {
            Console.WriteLine("Converting 0;0;0;94...");
            KernelColorConversionTools.ConvertFromCmykToRgb("cmyk:0;0;0;94").ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from CMYK numbers to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykNumbersToRgb()
        {
            Console.WriteLine("Converting 0, 0, 0, 94...");
            KernelColorConversionTools.ConvertFromCmykToRgb(0, 0, 0, 94).ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from CMYK sequence to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykSequenceToRyb()
        {
            Console.WriteLine("Converting 0;0;0;94...");
            KernelColorConversionTools.ConvertFromCmykToRyb("cmyk:0;0;0;94").ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from CMYK numbers to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykNumbersToRyb()
        {
            Console.WriteLine("Converting 0, 0, 0, 94...");
            KernelColorConversionTools.ConvertFromCmykToRyb(0, 0, 0, 94).ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from CMYK sequence to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykSequenceToHsl()
        {
            Console.WriteLine("Converting 0;0;0;94...");
            KernelColorConversionTools.ConvertFromCmykToHsl("cmyk:0;0;0;94").ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from CMYK numbers to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykNumbersToHsl()
        {
            Console.WriteLine("Converting 0, 0, 0, 94...");
            KernelColorConversionTools.ConvertFromCmykToHsl(0, 0, 0, 94).ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from CMYK sequence to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykSequenceToHsv()
        {
            Console.WriteLine("Converting 0;0;0;94...");
            KernelColorConversionTools.ConvertFromCmykToHsv("cmyk:0;0;0;94").ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from CMYK numbers to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykNumbersToHsv()
        {
            Console.WriteLine("Converting 0, 0, 0, 94...");
            KernelColorConversionTools.ConvertFromCmykToHsv(0, 0, 0, 94).ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from CMYK sequence to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykSequenceToCmy()
        {
            Console.WriteLine("Converting 0;0;0;94...");
            KernelColorConversionTools.ConvertFromCmykToCmy("cmyk:0;0;0;94").ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from CMYK numbers to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykNumbersToCmy()
        {
            Console.WriteLine("Converting 0, 0, 0, 94...");
            KernelColorConversionTools.ConvertFromCmykToCmy(0, 0, 0, 94).ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from CMYK sequence to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykSequenceToYiq()
        {
            Console.WriteLine("Converting 0;0;0;94...");
            KernelColorConversionTools.ConvertFromCmykToYiq("cmyk:0;0;0;94").ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from CMYK numbers to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykNumbersToYiq()
        {
            Console.WriteLine("Converting 0, 0, 0, 94...");
            KernelColorConversionTools.ConvertFromCmykToYiq(0, 0, 0, 94).ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from CMYK sequence to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykSequenceToYuv()
        {
            Console.WriteLine("Converting 0;0;0;94...");
            KernelColorConversionTools.ConvertFromCmykToYuv("cmyk:0;0;0;94").ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from CMYK numbers to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmykNumbersToYuv()
        {
            Console.WriteLine("Converting 0, 0, 0, 94...");
            KernelColorConversionTools.ConvertFromCmykToYuv(0, 0, 0, 94).ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from CMY sequence to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmySequenceToRgb()
        {
            Console.WriteLine("Converting 94;94;94...");
            KernelColorConversionTools.ConvertFromCmyToRgb("cmy:94;94;94").ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from CMY numbers to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmyNumbersToRgb()
        {
            Console.WriteLine("Converting 94, 94, 94...");
            KernelColorConversionTools.ConvertFromCmyToRgb(94, 94, 94).ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from CMY sequence to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmySequenceToRyb()
        {
            Console.WriteLine("Converting 94;94;94...");
            KernelColorConversionTools.ConvertFromCmyToRyb("cmy:94;94;94").ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from CMY numbers to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmyNumbersToRyb()
        {
            Console.WriteLine("Converting 94, 94, 94...");
            KernelColorConversionTools.ConvertFromCmyToRyb(94, 94, 94).ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from CMY sequence to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmySequenceToHsl()
        {
            Console.WriteLine("Converting 94;94;94...");
            KernelColorConversionTools.ConvertFromCmyToHsl("cmy:94;94;94").ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from CMY numbers to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmyNumbersToHsl()
        {
            Console.WriteLine("Converting 94, 94, 94...");
            KernelColorConversionTools.ConvertFromCmyToHsl(94, 94, 94).ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from CMY sequence to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmySequenceToHsv()
        {
            Console.WriteLine("Converting 94;94;94...");
            KernelColorConversionTools.ConvertFromCmyToHsv("cmy:94;94;94").ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from CMY numbers to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmyNumbersToHsv()
        {
            Console.WriteLine("Converting 94, 94, 94...");
            KernelColorConversionTools.ConvertFromCmyToHsv(94, 94, 94).ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from CMY sequence to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmySequenceToCmyk()
        {
            Console.WriteLine("Converting 94;94;94...");
            KernelColorConversionTools.ConvertFromCmyToCmyk("cmy:94;94;94").ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from CMY numbers to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmyNumbersToCmyk()
        {
            Console.WriteLine("Converting 94, 94, 94...");
            KernelColorConversionTools.ConvertFromCmyToCmyk(94, 94, 94).ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from CMY sequence to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmySequenceToYiq()
        {
            Console.WriteLine("Converting 94;94;94...");
            KernelColorConversionTools.ConvertFromCmyToYiq("cmy:94;94;94").ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from CMY numbers to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmyNumbersToYiq()
        {
            Console.WriteLine("Converting 94, 94, 94...");
            KernelColorConversionTools.ConvertFromCmyToYiq(94, 94, 94).ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from CMY sequence to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmySequenceToYuv()
        {
            Console.WriteLine("Converting 94;94;94...");
            KernelColorConversionTools.ConvertFromCmyToYuv("cmy:94;94;94").ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from CMY numbers to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmyNumbersToYuv()
        {
            Console.WriteLine("Converting 94, 94, 94...");
            KernelColorConversionTools.ConvertFromCmyToYuv(94, 94, 94).ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from YIQ sequence to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqSequenceToRgb()
        {
            Console.WriteLine("Converting 0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToRgb("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18").ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from YIQ numbers to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqNumbersToRgb()
        {
            Console.WriteLine("Converting 0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToRgb(0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18).ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from YIQ sequence to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqSequenceToRyb()
        {
            Console.WriteLine("Converting 0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToRyb("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18").ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from YIQ numbers to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqNumbersToRyb()
        {
            Console.WriteLine("Converting 0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToRyb(0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18).ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from YIQ sequence to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqSequenceToHsl()
        {
            Console.WriteLine("Converting 0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToHsl("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18").ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from YIQ numbers to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqNumbersToHsl()
        {
            Console.WriteLine("Converting 0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToHsl(0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18).ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from YIQ sequence to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqSequenceToHsv()
        {
            Console.WriteLine("Converting 0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToHsv("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18").ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from YIQ numbers to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqNumbersToHsv()
        {
            Console.WriteLine("Converting 0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToHsv(0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18).ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from YIQ sequence to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqSequenceToCmyk()
        {
            Console.WriteLine("Converting 0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToCmyk("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18").ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from YIQ numbers to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqNumbersToCmyk()
        {
            Console.WriteLine("Converting 0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToCmyk(0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18).ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from YIQ sequence to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqSequenceToCmy()
        {
            Console.WriteLine("Converting 0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToCmy("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18").ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from YIQ numbers to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqNumbersToCmy()
        {
            Console.WriteLine("Converting 0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToCmy(0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18).ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from YIQ sequence to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqSequenceToYuv()
        {
            Console.WriteLine("Converting 0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToYuv("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18").ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from YIQ numbers to YUV
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromYiqNumbersToYuv()
        {
            Console.WriteLine("Converting 0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18...");
            KernelColorConversionTools.ConvertFromYiqToYuv(0.058823529411764705, -3.483052626275001E-18, -3.483052626275001E-18).ShouldBe("yuv:15;128;128");
        }

        /// <summary>
        /// Tests trying to convert from YUV sequence to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvSequenceToRgb()
        {
            Console.WriteLine("Converting 15;128;128...");
            KernelColorConversionTools.ConvertFromYuvToRgb("yuv:15;128;128").ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from YUV numbers to RGB
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvNumbersToRgb()
        {
            Console.WriteLine("Converting 15, 128, 128...");
            KernelColorConversionTools.ConvertFromYuvToRgb(15, 128, 128).ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from YUV sequence to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvSequenceToRyb()
        {
            Console.WriteLine("Converting 15;128;128...");
            KernelColorConversionTools.ConvertFromYuvToRyb("yuv:15;128;128").ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from YUV numbers to RYB
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvNumbersToRyb()
        {
            Console.WriteLine("Converting 15, 128, 128...");
            KernelColorConversionTools.ConvertFromYuvToRyb(15, 128, 128).ShouldBe("ryb:15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from YUV sequence to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvSequenceToHsl()
        {
            Console.WriteLine("Converting 15;128;128...");
            KernelColorConversionTools.ConvertFromYuvToHsl("yuv:15;128;128").ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from YUV numbers to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvNumbersToHsl()
        {
            Console.WriteLine("Converting 15, 128, 128...");
            KernelColorConversionTools.ConvertFromYuvToHsl(15, 128, 128).ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from YUV sequence to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvSequenceToHsv()
        {
            Console.WriteLine("Converting 15;128;128...");
            KernelColorConversionTools.ConvertFromYuvToHsv("yuv:15;128;128").ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from YUV numbers to HSV
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvNumbersToHsv()
        {
            Console.WriteLine("Converting 15, 128, 128...");
            KernelColorConversionTools.ConvertFromYuvToHsv(15, 128, 128).ShouldBe("hsv:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from YUV sequence to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvSequenceToCmyk()
        {
            Console.WriteLine("Converting 15;128;128...");
            KernelColorConversionTools.ConvertFromYuvToCmyk("yuv:15;128;128").ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from YUV numbers to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvNumbersToCmyk()
        {
            Console.WriteLine("Converting 15, 128, 128...");
            KernelColorConversionTools.ConvertFromYuvToCmyk(15, 128, 128).ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from YUV sequence to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvSequenceToCmy()
        {
            Console.WriteLine("Converting 15;128;128...");
            KernelColorConversionTools.ConvertFromYuvToCmy("yuv:15;128;128").ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from YUV numbers to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvNumbersToCmy()
        {
            Console.WriteLine("Converting 15, 128, 128...");
            KernelColorConversionTools.ConvertFromYuvToCmy(15, 128, 128).ShouldBe("cmy:94;94;94");
        }

        /// <summary>
        /// Tests trying to convert from YUV sequence to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvSequenceToYiq()
        {
            Console.WriteLine("Converting 15;128;128...");
            KernelColorConversionTools.ConvertFromYuvToYiq("yuv:15;128;128").ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

        /// <summary>
        /// Tests trying to convert from YUV numbers to YIQ
        /// </summary>
        [Test]
        [Description("Conversion")]
        [Ignore("Terminaux has a bug where the Y component of the YUV specifier is not accepting values larger than 1.0.")]
        public void TestConvertFromYuvNumbersToYiq()
        {
            Console.WriteLine("Converting 15, 128, 128...");
            KernelColorConversionTools.ConvertFromYuvToYiq(15, 128, 128).ShouldBe("yiq:0.058823529411764705;-3.483052626275001E-18;-3.483052626275001E-18");
        }

    }
}
