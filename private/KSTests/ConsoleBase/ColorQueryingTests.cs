
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Themes;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terminaux.Colors;

namespace KSTests.ConsoleBase
{

    [TestFixture]
    public class ColorQueryingTests
    {

        /// <summary>
        /// Tests getting color from the kernel type
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetColor()
        {
            var types = Enum.GetNames(typeof(KernelColorType));
            foreach (string typeName in types)
            {
                Color color = Color.Empty;
                var type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeName);
                Should.NotThrow(() => color = KernelColorTools.GetColor(type));
                color.ShouldNotBeNull();
            }
        }

        /// <summary>
        /// Tests setting color from the kernel type
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestSetColor()
        {
            var types = Enum.GetNames(typeof(KernelColorType));
            Color expected = new(255, 255, 255);
            foreach (string typeName in types)
            {
                Color color = Color.Empty;
                var type = (KernelColorType)Enum.Parse(typeof(KernelColorType), typeName);
                Should.NotThrow(() => color = KernelColorTools.SetColor(type, expected));
                color.ShouldNotBeNull();
                color.ShouldNotBe(Color.Empty);
                color.ShouldBe(expected);
                Color kcolor = Color.Empty;
                Should.NotThrow(() => kcolor = KernelColorTools.GetColor(type));
                kcolor.ShouldNotBeNull();
                kcolor.ShouldNotBe(Color.Empty);
                kcolor.ShouldBe(expected);
            }
        }

        /// <summary>
        /// Tests populating colors
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestPopulateColorsCurrent()
        {
            var dict = KernelColorTools.PopulateColorsCurrent();
            dict.ShouldNotBeEmpty();
            foreach (var type in dict.Keys)
            {
                var expected = Color.Empty;
                var color = dict[type];
                color.ShouldNotBeNull();
                Should.NotThrow(() => expected = KernelColorTools.GetColor(type));
                expected.ShouldNotBeNull();
                color.ShouldBe(expected);
            }
        }

        /// <summary>
        /// Tests populating colors
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestPopulateColorsDefault()
        {
            var dict = KernelColorTools.PopulateColorsDefault();
            dict.ShouldNotBeEmpty();
            ThemeInfo themeInfo = new();
            foreach (var type in dict.Keys)
            {
                var expected = Color.Empty;
                var color = dict[type];
                color.ShouldNotBeNull();
                Should.NotThrow(() => expected = themeInfo.GetColor(type));
                expected.ShouldNotBeNull();
                color.ShouldBe(expected);
            }
        }

        /// <summary>
        /// Tests populating colors
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestPopulateColorsEmpty()
        {
            var dict = KernelColorTools.PopulateColorsEmpty();
            dict.ShouldNotBeEmpty();
            foreach (var type in dict.Keys)
            {
                var expected = Color.Empty;
                var color = dict[type];
                color.ShouldNotBeNull();
                color.ShouldBe(expected);
            }
        }

        /// <summary>
        /// Tests getting gray color
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetGrayLight()
        {
            var expected = KernelColorTools.GetColor(KernelColorType.NeutralText);
            Should.NotThrow(() => KernelColorTools.SetColor(KernelColorType.Background, new Color(255, 255, 255)));
            var color = KernelColorTools.GetGray();
            color.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting gray color
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetGrayDark()
        {
            var expected = new Color(ConsoleColors.Gray);
            Should.NotThrow(() => KernelColorTools.SetColor(KernelColorType.Background, new Color(0, 0, 0)));
            var color = KernelColorTools.GetGray();
            color.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting random color
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetRandomColor()
        {
            var types = Enum.GetNames(typeof(ColorType));
            foreach (string typeName in types)
            {
                Color color = Color.Empty;
                var type = (ColorType)Enum.Parse(typeof(ColorType), typeName);
                Should.NotThrow(() => color = KernelColorTools.GetRandomColor(type));
                type = color.PlainSequence.Contains(";") ? 
                       ColorType.TrueColor : color.ColorEnum255 != (ConsoleColors)(-1) ?
                       ColorType._255Color : ColorType._16Color;
                color.ShouldNotBeNull();
                color.Type.ShouldBe(type);
            }
        }

        /// <summary>
        /// Tests getting random color
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetRandomColorBlackSelection()
        {
            var types = Enum.GetNames(typeof(ColorType)).Skip(1);
            foreach (string typeName in types)
            {
                var type = (ColorType)Enum.Parse(typeof(ColorType), typeName);
                Color expected = Color.Empty;
                Color color = null;
                while (color is null || color != expected)
                    Should.NotThrow(() => color = KernelColorTools.GetRandomColor(type));
                color.ShouldNotBeNull();
                color.ShouldBe(expected);
            }
        }

        /// <summary>
        /// Tests getting random color
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetRandomColorNoBlackSelection()
        {
            var types = Enum.GetNames(typeof(ColorType)).Skip(1);
            var colors = new List<Color>();
            foreach (string typeName in types)
            {
                var type = (ColorType)Enum.Parse(typeof(ColorType), typeName);
                Color unexpected = Color.Empty;
                for (int i = 1; i <= 1000000; i++)
                    Should.NotThrow(() => colors.Add(KernelColorTools.GetRandomColor(type, false)));
                colors.ShouldNotBeEmpty();
                colors.ShouldNotContain(unexpected);
            }
        }

        /// <summary>
        /// Tests trying to parse the color from hex
        /// </summary>
        [TestCase("#0F0F0F", ExpectedResult = true)]
        [TestCase("#0G0G0G", ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseColorFromHex(string TargetHex)
        {
            Debug.WriteLine($"Trying {TargetHex}...");
            return KernelColorTools.TryParseColor(TargetHex);
        }

        /// <summary>
        /// Tests trying to parse the color from color numbers
        /// </summary>
        [TestCase(26, ExpectedResult = true)]
        [TestCase(260, ExpectedResult = false)]
        [TestCase(-26, ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseColorFromColorNum(int TargetColorNum)
        {
            Debug.WriteLine($"Trying colornum {TargetColorNum}...");
            return KernelColorTools.TryParseColor(TargetColorNum);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB
        /// </summary>
        [TestCase(4, 4, 4, ExpectedResult = true)]
        [TestCase(400, 4, 4, ExpectedResult = false)]
        [TestCase(4, 400, 4, ExpectedResult = false)]
        [TestCase(4, 4, 400, ExpectedResult = false)]
        [TestCase(4, 400, 400, ExpectedResult = false)]
        [TestCase(400, 4, 400, ExpectedResult = false)]
        [TestCase(400, 400, 4, ExpectedResult = false)]
        [TestCase(400, 400, 400, ExpectedResult = false)]
        [TestCase(-4, 4, 4, ExpectedResult = false)]
        [TestCase(4, -4, 4, ExpectedResult = false)]
        [TestCase(4, 4, -4, ExpectedResult = false)]
        [TestCase(4, -4, -4, ExpectedResult = false)]
        [TestCase(-4, 4, -4, ExpectedResult = false)]
        [TestCase(-4, -4, 4, ExpectedResult = false)]
        [TestCase(-4, -4, -4, ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseColorFromRGB(int R, int G, int B)
        {
            Debug.WriteLine($"Trying rgb {R}, {G}, {B}...");
            return KernelColorTools.TryParseColor(R, G, B);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB
        /// </summary>
        [TestCase("4;4;4", ExpectedResult = true)]
        [TestCase("400;4;4", ExpectedResult = false)]
        [TestCase("4;400;4", ExpectedResult = false)]
        [TestCase("4;4;400", ExpectedResult = false)]
        [TestCase("4;400;400", ExpectedResult = false)]
        [TestCase("400;4;400", ExpectedResult = false)]
        [TestCase("400;400;4", ExpectedResult = false)]
        [TestCase("400;400;400", ExpectedResult = false)]
        [TestCase("-4;4;4", ExpectedResult = false)]
        [TestCase("4;-4;4", ExpectedResult = false)]
        [TestCase("4;4;-4", ExpectedResult = false)]
        [TestCase("4;-4;-4", ExpectedResult = false)]
        [TestCase("-4;4;-4", ExpectedResult = false)]
        [TestCase("-4;-4;4", ExpectedResult = false)]
        [TestCase("-4;-4;-4", ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseColorFromSpecifier(string specifier)
        {
            Debug.WriteLine($"Trying rgb specifier {specifier}...");
            return KernelColorTools.TryParseColor(specifier);
        }

        /// <summary>
        /// Tests trying to convert from hex to RGB
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestConvertFromHexToRGB()
        {
            Debug.WriteLine("Converting #0F0F0F...");
            KernelColorTools.ConvertFromHexToRGB("#0F0F0F").ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to hex
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestConvertFromRGBSequenceToHex()
        {
            Debug.WriteLine("Converting 15;15;15...");
            KernelColorTools.ConvertFromRGBToHex("15;15;15").ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to hex
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestConvertFromRGBNumbersToHex()
        {
            Debug.WriteLine("Converting 15, 15, 15...");
            KernelColorTools.ConvertFromRGBToHex(15, 15, 15).ShouldBe("#0F0F0F");
        }

    }
}
