
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using KS.ConsoleBase.Colors;
using NUnit.Framework;
using Shouldly;
using System.Diagnostics;

namespace KSTests.ConsoleTests
{

    [TestFixture]
    public class ColorQueryingTests
    {

        /// <summary>
        /// Tests trying to parse the color from hex
        /// </summary>
        [TestCase("#0F0F0F", ExpectedResult = true)]
        [TestCase("#0G0G0G", ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseColorFromHex(string TargetHex)
        {
            Debug.WriteLine($"Trying {TargetHex}...");
            return ColorTools.TryParseColor(TargetHex);
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
            return ColorTools.TryParseColor(TargetColorNum);
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
            return ColorTools.TryParseColor(R, G, B);
        }

        /// <summary>
        /// Tests trying to convert from hex to RGB
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestConvertFromHexToRGB()
        {
            Debug.WriteLine("Converting #0F0F0F...");
            ColorTools.ConvertFromHexToRGB("#0F0F0F").ShouldBe("15;15;15");
        }

        /// <summary>
        /// Tests trying to convert from RGB sequence to hex
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestConvertFromRGBSequenceToHex()
        {
            Debug.WriteLine("Converting 15;15;15...");
            ColorTools.ConvertFromRGBToHex("15;15;15").ShouldBe("#0F0F0F");
        }

        /// <summary>
        /// Tests trying to convert from RGB numbers to hex
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestConvertFromRGBNumbersToHex()
        {
            Debug.WriteLine("Converting 15, 15, 15...");
            ColorTools.ConvertFromRGBToHex(15, 15, 15).ShouldBe("#0F0F0F");
        }

    }
}
