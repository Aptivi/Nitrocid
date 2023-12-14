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

using KS.ConsoleBase.Colors;
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
        /// Tests trying to convert from CMY sequence to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmySequenceToCmyk()
        {
            Console.WriteLine("Converting 94;94;94...");
            KernelColorConversionTools.ConvertFromCmyToCmyk("cmy:94;94;94").ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from CMY numbers to CMY
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCmyNumbersToCmyk()
        {
            Console.WriteLine("Converting 94, 94, 94...");
            KernelColorConversionTools.ConvertFromCmyToCmyk(94, 94, 94).ShouldBe("cmyk:0;0;0;94");
        }

    }
}
