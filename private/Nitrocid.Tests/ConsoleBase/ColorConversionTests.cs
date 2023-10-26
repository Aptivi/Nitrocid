//
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
        public void TestConvertFromHexToRGB()
        {
            Console.WriteLine("Converting #0F0F0F...");
            KernelColorConversionTools.ConvertFromHexToRGB("#0F0F0F").ShouldBe("15;15;15");
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
        /// Tests trying to convert from RGB sequence to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRGBSequenceToHex()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRGBToHex("15;15;15").ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRGBNumbersToHex()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRGBToHex(15, 15, 15).ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from CMYK sequence to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCMYKSequenceToHex()
        {
            Console.WriteLine("Converting 0;0;0;94...");
            KernelColorConversionTools.ConvertFromCmykToHex("cmyk:0;0;0;94").ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from CMYK numbers to hex
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromCMYKNumbersToHex()
        {
            Console.WriteLine("Converting 0, 0, 0, 94...");
            KernelColorConversionTools.ConvertFromCmykToHex(0, 0, 0, 94).ShouldBe("#0F0F0F");
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
        /// Tests trying to convert from RGB sequence to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbSequenceToCmyk()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRGBToCmyk("15;15;15").ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbNumbersToCmyk()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRGBToCmyk(15, 15, 15).ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbSequenceToHsl()
        {
            Console.WriteLine("Converting 15;15;15...");
            KernelColorConversionTools.ConvertFromRGBToHsl("15;15;15").ShouldBe("hsl:0;0;5");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to HSL
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromRgbNumbersToHsl()
        {
            Console.WriteLine("Converting 15, 15, 15...");
            KernelColorConversionTools.ConvertFromRGBToHsl(15, 15, 15).ShouldBe("hsl:0;0;5");
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
        /// Tests trying to convert from HSL sequence to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslSequenceToCmyk()
        {
            Console.WriteLine("Converting 0;0;5...");
            KernelColorConversionTools.ConvertFromHslToCmyk("hsl:0;0;5").ShouldBe("cmyk:0;0;0;94");
        }

        /// <summary>
        /// Tests trying to convert from HSL numbers to CMYK
        /// </summary>
        [Test]
        [Description("Conversion")]
        public void TestConvertFromHslNumbersToCmyk()
        {
            Console.WriteLine("Converting 0, 0, 5...");
            KernelColorConversionTools.ConvertFromHslToCmyk(0, 0, 5).ShouldBe("cmyk:0;0;0;94");
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

    }
}
