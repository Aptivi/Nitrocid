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
using Nitrocid.ConsoleBase.Themes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.Tests.ConsoleBase
{

    [TestClass]
    public class ColorQueryingTests
    {

        /// <summary>
        /// Tests getting color from the kernel type
        /// </summary>
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
        [Description("Querying")]
        public void TestPopulateColorsEmpty()
        {
            var dict = KernelColorTools.PopulateColorsEmpty();
            dict.ShouldNotBeEmpty();
            foreach (var type in dict.Keys)
            {
                var expected = type != KernelColorType.Background ? new Color(ConsoleColors.White) : Color.Empty;
                var color = dict[type];
                color.ShouldNotBeNull();
                color.ShouldBe(expected);
            }
        }

        /// <summary>
        /// Tests getting gray color
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetGrayLight()
        {
            var expected = new Color(ConsoleColors.Silver);
            Should.NotThrow(() => KernelColorTools.SetColor(KernelColorType.Background, new Color(255, 255, 255)));
            var color = ColorTools.GetGray();
            color.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting gray color
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetGrayDark()
        {
            var expected = new Color(ConsoleColors.Silver);
            Should.NotThrow(() => KernelColorTools.SetColor(KernelColorType.Background, new Color(0, 0, 0)));
            var color = ColorTools.GetGray();
            color.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting random color
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetRandomColorDefault()
        {
            Color color = Color.Empty;
            Should.NotThrow(() => color = ColorTools.GetRandomColor());
            color.ShouldNotBeNull();
            color.Type.ShouldBe(ColorType.TrueColor);
        }

        /// <summary>
        /// Tests getting random color
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetRandomColor()
        {
            var types = Enum.GetNames(typeof(ColorType));
            foreach (string typeName in types)
            {
                Color color = Color.Empty;
                var type = (ColorType)Enum.Parse(typeof(ColorType), typeName);
                Should.NotThrow(() => color = ColorTools.GetRandomColor(type));
                type = color.PlainSequence.Contains(';') ?
                       ColorType.TrueColor : color.ColorId.ColorId >= 16 ?
                       ColorType.EightBitColor : ColorType.FourBitColor;
                color.ShouldNotBeNull();
                color.Type.ShouldBe(type);
            }
        }

        /// <summary>
        /// Tests getting random color
        /// </summary>
        [TestMethod]
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
                    Should.NotThrow(() => color = ColorTools.GetRandomColor(type));
                color.ShouldNotBeNull();
                color.ShouldBe(expected);
            }
        }

        /// <summary>
        /// Tests getting random color
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetRandomColorNoBlackSelection()
        {
            var types = Enum.GetNames(typeof(ColorType)).Skip(1);
            var colors = new List<Color>();
            foreach (string typeName in types)
            {
                var type = (ColorType)Enum.Parse(typeof(ColorType), typeName);
                Color unexpected = Color.Empty;
                for (int i = 1; i <= 100; i++)
                    Should.NotThrow(() => colors.Add(ColorTools.GetRandomColor(type, false)));
                colors.ShouldNotBeEmpty();
                colors.ShouldNotContain(unexpected);
            }
        }

        /// <summary>
        /// Tests trying to parse the color from hex
        /// </summary>
        [TestMethod]
        [DataRow("#0F0F0F", true)]
        [DataRow("#0G0G0G", false)]
        [Description("Querying")]
        public void TestTryParseColorFromHex(string TargetHex, bool expected)
        {
            Console.WriteLine($"Trying {TargetHex}...");
            bool actual = ColorTools.TryParseColor(TargetHex);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from color numbers
        /// </summary>
        [TestMethod]
        [DataRow(26, true)]
        [DataRow(260, true)]
        [DataRow(-26, false)]
        [Description("Querying")]
        public void TestTryParseColorFromColorNum(int TargetColorNum, bool expected)
        {
            Console.WriteLine($"Trying colornum {TargetColorNum}...");
            bool actual = ColorTools.TryParseColor(TargetColorNum);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB
        /// </summary>
        [TestMethod]
        [DataRow(4, 4, 4, true)]
        [DataRow(400, 4, 4, false)]
        [DataRow(4, 400, 4, false)]
        [DataRow(4, 4, 400, false)]
        [DataRow(4, 400, 400, false)]
        [DataRow(400, 4, 400, false)]
        [DataRow(400, 400, 4, false)]
        [DataRow(400, 400, 400, false)]
        [DataRow(-4, 4, 4, false)]
        [DataRow(4, -4, 4, false)]
        [DataRow(4, 4, -4, false)]
        [DataRow(4, -4, -4, false)]
        [DataRow(-4, 4, -4, false)]
        [DataRow(-4, -4, 4, false)]
        [DataRow(-4, -4, -4, false)]
        [Description("Querying")]
        public void TestTryParseColorFromRGB(int R, int G, int B, bool expected)
        {
            Console.WriteLine($"Trying rgb {R}, {G}, {B}...");
            bool actual = ColorTools.TryParseColor(R, G, B);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB
        /// </summary>
        [TestMethod]
        [DataRow("4;4;4", true)]
        [DataRow("400;4;4", false)]
        [DataRow("4;400;4", false)]
        [DataRow("4;4;400", false)]
        [DataRow("4;400;400", false)]
        [DataRow("400;4;400", false)]
        [DataRow("400;400;4", false)]
        [DataRow("400;400;400", false)]
        [DataRow("-4;4;4", false)]
        [DataRow("4;-4;4", false)]
        [DataRow("4;4;-4", false)]
        [DataRow("4;-4;-4", false)]
        [DataRow("-4;4;-4", false)]
        [DataRow("-4;-4;4", false)]
        [DataRow("-4;-4;-4", false)]
        [Description("Querying")]
        public void TestTryParseColorFromSpecifier(string specifier, bool expected)
        {
            Console.WriteLine($"Trying rgb specifier {specifier}...");
            bool actual = ColorTools.TryParseColor(specifier);
            actual.ShouldBe(expected);
        }

    }
}
