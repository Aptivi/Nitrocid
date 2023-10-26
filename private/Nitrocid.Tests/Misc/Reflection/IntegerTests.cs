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

using KS.Misc.Reflection;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Misc.Reflection
{
    [TestFixture]
    public class IntegerTests
    {

        /// <summary>
        /// Tests swapping integers if the source integer is larger than the target
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestSwapIfSourceLarger()
        {
            int source = 6;
            int target = 4;
            int expectedSource = 4;
            int expectedTarget = 6;
            source.SwapIfSourceLarger(ref target);
            source.ShouldBe(expectedSource);
            target.ShouldBe(expectedTarget);
        }

        /// <summary>
        /// [Counterexample] Tests swapping integers if the source integer is larger than the target
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestSwapIfSourceLargerShouldNotSwap()
        {
            int source = 4;
            int target = 6;
            int expectedSource = 4;
            int expectedTarget = 6;
            source.SwapIfSourceLarger(ref target);
            source.ShouldBe(expectedSource);
            target.ShouldBe(expectedTarget);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestGetDigits()
        {
            long number = 45000;
            int expectedDigits = 5;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestGetDigitsNegativeNumber()
        {
            long number = -45000;
            int expectedDigits = 5;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestGetDigitsOverflowingInteger()
        {
            long number = 450000000000;
            int expectedDigits = 12;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestGetDigitsOverflowingIntegerNegativeNumber()
        {
            long number = -450000000000;
            int expectedDigits = 12;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestGetDigitsInt()
        {
            int number = 45000;
            int expectedDigits = 5;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestGetDigitsIntNegativeNumber()
        {
            int number = -45000;
            int expectedDigits = 5;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(-1, "0 B")]
        [TestCase(0, "0 B")]
        [TestCase(1, "1 B")]
        [TestCase(1024, "1 KB")]
        [TestCase(1024 * 1024, "1 MB")]
        [TestCase(1024 * 1024 * 1024, "1 GB")]
        [TestCase(1024L * 1024 * 1024 * 1024, "1 TB")]
        [TestCase(1024L * 1024 * 1024 * 1024 * 1024, "1 PB")]
        [TestCase(1024L * 1024 * 1024 * 1024 * 1024 * 1024, "1 EB")]
        [Description("Querying")]
        public static void TestFileSizeString(long bytes, string expectedHumanized)
        {
            string humanized = bytes.SizeString();
            humanized.ShouldBe(expectedHumanized);
        }

    }
}
