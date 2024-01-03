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

using Nitrocid.Misc.Reflection;
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

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0L, "0")]
        [TestCase(1L, "1")]
        [TestCase(2L, "10")]
        [TestCase(4L, "100")]
        [TestCase(8L, "1000")]
        [TestCase(12L, "1100")]
        [TestCase(16L, "10000")]
        [TestCase(20L, "10100")]
        [TestCase(24L, "11000")]
        [TestCase(28L, "11100")]
        [TestCase(32L, "100000")]
        [Description("Querying")]
        public static void TestToBinary(long bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0, "0")]
        [TestCase(1, "1")]
        [TestCase(2, "10")]
        [TestCase(4, "100")]
        [TestCase(8, "1000")]
        [TestCase(12, "1100")]
        [TestCase(16, "10000")]
        [TestCase(20, "10100")]
        [TestCase(24, "11000")]
        [TestCase(28, "11100")]
        [TestCase(32, "100000")]
        [Description("Querying")]
        public static void TestToBinary(int bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0u, "0")]
        [TestCase(1u, "1")]
        [TestCase(2u, "10")]
        [TestCase(4u, "100")]
        [TestCase(8u, "1000")]
        [TestCase(12u, "1100")]
        [TestCase(16u, "10000")]
        [TestCase(20u, "10100")]
        [TestCase(24u, "11000")]
        [TestCase(28u, "11100")]
        [TestCase(32u, "100000")]
        [Description("Querying")]
        public static void TestToBinary(uint bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((short)0, "0")]
        [TestCase((short)1, "1")]
        [TestCase((short)2, "10")]
        [TestCase((short)4, "100")]
        [TestCase((short)8, "1000")]
        [TestCase((short)12, "1100")]
        [TestCase((short)16, "10000")]
        [TestCase((short)20, "10100")]
        [TestCase((short)24, "11000")]
        [TestCase((short)28, "11100")]
        [TestCase((short)32, "100000")]
        [Description("Querying")]
        public static void TestToBinary(short bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((ushort)0u, "0")]
        [TestCase((ushort)1u, "1")]
        [TestCase((ushort)2u, "10")]
        [TestCase((ushort)4u, "100")]
        [TestCase((ushort)8u, "1000")]
        [TestCase((ushort)12u, "1100")]
        [TestCase((ushort)16u, "10000")]
        [TestCase((ushort)20u, "10100")]
        [TestCase((ushort)24u, "11000")]
        [TestCase((ushort)28u, "11100")]
        [TestCase((ushort)32u, "100000")]
        [Description("Querying")]
        public static void TestToBinary(ushort bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((byte)0, "0")]
        [TestCase((byte)1, "1")]
        [TestCase((byte)2, "10")]
        [TestCase((byte)4, "100")]
        [TestCase((byte)8, "1000")]
        [TestCase((byte)12, "1100")]
        [TestCase((byte)16, "10000")]
        [TestCase((byte)20, "10100")]
        [TestCase((byte)24, "11000")]
        [TestCase((byte)28, "11100")]
        [TestCase((byte)32, "100000")]
        [Description("Querying")]
        public static void TestToBinary(byte bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((sbyte)0, "0")]
        [TestCase((sbyte)1, "1")]
        [TestCase((sbyte)2, "10")]
        [TestCase((sbyte)4, "100")]
        [TestCase((sbyte)8, "1000")]
        [TestCase((sbyte)12, "1100")]
        [TestCase((sbyte)16, "10000")]
        [TestCase((sbyte)20, "10100")]
        [TestCase((sbyte)24, "11000")]
        [TestCase((sbyte)28, "11100")]
        [TestCase((sbyte)32, "100000")]
        [Description("Querying")]
        public static void TestToBinary(sbyte bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0L, "0")]
        [TestCase(1L, "1")]
        [TestCase(2L, "2")]
        [TestCase(4L, "4")]
        [TestCase(8L, "10")]
        [TestCase(12L, "14")]
        [TestCase(16L, "20")]
        [TestCase(20L, "24")]
        [TestCase(24L, "30")]
        [TestCase(28L, "34")]
        [TestCase(32L, "40")]
        [Description("Querying")]
        public static void TestToOctal(long bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0, "0")]
        [TestCase(1, "1")]
        [TestCase(2, "2")]
        [TestCase(4, "4")]
        [TestCase(8, "10")]
        [TestCase(12, "14")]
        [TestCase(16, "20")]
        [TestCase(20, "24")]
        [TestCase(24, "30")]
        [TestCase(28, "34")]
        [TestCase(32, "40")]
        [Description("Querying")]
        public static void TestToOctal(int bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0u, "0")]
        [TestCase(1u, "1")]
        [TestCase(2u, "2")]
        [TestCase(4u, "4")]
        [TestCase(8u, "10")]
        [TestCase(12u, "14")]
        [TestCase(16u, "20")]
        [TestCase(20u, "24")]
        [TestCase(24u, "30")]
        [TestCase(28u, "34")]
        [TestCase(32u, "40")]
        [Description("Querying")]
        public static void TestToOctal(uint bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((short)0, "0")]
        [TestCase((short)1, "1")]
        [TestCase((short)2, "2")]
        [TestCase((short)4, "4")]
        [TestCase((short)8, "10")]
        [TestCase((short)12, "14")]
        [TestCase((short)16, "20")]
        [TestCase((short)20, "24")]
        [TestCase((short)24, "30")]
        [TestCase((short)28, "34")]
        [TestCase((short)32, "40")]
        [Description("Querying")]
        public static void TestToOctal(short bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((ushort)0u, "0")]
        [TestCase((ushort)1u, "1")]
        [TestCase((ushort)2u, "2")]
        [TestCase((ushort)4u, "4")]
        [TestCase((ushort)8u, "10")]
        [TestCase((ushort)12u, "14")]
        [TestCase((ushort)16u, "20")]
        [TestCase((ushort)20u, "24")]
        [TestCase((ushort)24u, "30")]
        [TestCase((ushort)28u, "34")]
        [TestCase((ushort)32u, "40")]
        [Description("Querying")]
        public static void TestToOctal(ushort bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((byte)0, "0")]
        [TestCase((byte)1, "1")]
        [TestCase((byte)2, "2")]
        [TestCase((byte)4, "4")]
        [TestCase((byte)8, "10")]
        [TestCase((byte)12, "14")]
        [TestCase((byte)16, "20")]
        [TestCase((byte)20, "24")]
        [TestCase((byte)24, "30")]
        [TestCase((byte)28, "34")]
        [TestCase((byte)32, "40")]
        [Description("Querying")]
        public static void TestToOctal(byte bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((sbyte)0, "0")]
        [TestCase((sbyte)1, "1")]
        [TestCase((sbyte)2, "2")]
        [TestCase((sbyte)4, "4")]
        [TestCase((sbyte)8, "10")]
        [TestCase((sbyte)12, "14")]
        [TestCase((sbyte)16, "20")]
        [TestCase((sbyte)20, "24")]
        [TestCase((sbyte)24, "30")]
        [TestCase((sbyte)28, "34")]
        [TestCase((sbyte)32, "40")]
        [Description("Querying")]
        public static void TestToOctal(sbyte bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0L, "0")]
        [TestCase(1L, "1")]
        [TestCase(2L, "2")]
        [TestCase(4L, "4")]
        [TestCase(8L, "8")]
        [TestCase(12L, "12")]
        [TestCase(16L, "16")]
        [TestCase(20L, "20")]
        [TestCase(24L, "24")]
        [TestCase(28L, "28")]
        [TestCase(32L, "32")]
        [Description("Querying")]
        public static void TestToNumber(long bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0, "0")]
        [TestCase(1, "1")]
        [TestCase(2, "2")]
        [TestCase(4, "4")]
        [TestCase(8, "8")]
        [TestCase(12, "12")]
        [TestCase(16, "16")]
        [TestCase(20, "20")]
        [TestCase(24, "24")]
        [TestCase(28, "28")]
        [TestCase(32, "32")]
        [Description("Querying")]
        public static void TestToNumber(int bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0u, "0")]
        [TestCase(1u, "1")]
        [TestCase(2u, "2")]
        [TestCase(4u, "4")]
        [TestCase(8u, "8")]
        [TestCase(12u, "12")]
        [TestCase(16u, "16")]
        [TestCase(20u, "20")]
        [TestCase(24u, "24")]
        [TestCase(28u, "28")]
        [TestCase(32u, "32")]
        [Description("Querying")]
        public static void TestToNumber(uint bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((short)0, "0")]
        [TestCase((short)1, "1")]
        [TestCase((short)2, "2")]
        [TestCase((short)4, "4")]
        [TestCase((short)8, "8")]
        [TestCase((short)12, "12")]
        [TestCase((short)16, "16")]
        [TestCase((short)20, "20")]
        [TestCase((short)24, "24")]
        [TestCase((short)28, "28")]
        [TestCase((short)32, "32")]
        [Description("Querying")]
        public static void TestToNumber(short bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((ushort)0u, "0")]
        [TestCase((ushort)1u, "1")]
        [TestCase((ushort)2u, "2")]
        [TestCase((ushort)4u, "4")]
        [TestCase((ushort)8u, "8")]
        [TestCase((ushort)12u, "12")]
        [TestCase((ushort)16u, "16")]
        [TestCase((ushort)20u, "20")]
        [TestCase((ushort)24u, "24")]
        [TestCase((ushort)28u, "28")]
        [TestCase((ushort)32u, "32")]
        [Description("Querying")]
        public static void TestToNumber(ushort bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((byte)0, "0")]
        [TestCase((byte)1, "1")]
        [TestCase((byte)2, "2")]
        [TestCase((byte)4, "4")]
        [TestCase((byte)8, "8")]
        [TestCase((byte)12, "12")]
        [TestCase((byte)16, "16")]
        [TestCase((byte)20, "20")]
        [TestCase((byte)24, "24")]
        [TestCase((byte)28, "28")]
        [TestCase((byte)32, "32")]
        [Description("Querying")]
        public static void TestToNumber(byte bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((sbyte)0, "0")]
        [TestCase((sbyte)1, "1")]
        [TestCase((sbyte)2, "2")]
        [TestCase((sbyte)4, "4")]
        [TestCase((sbyte)8, "8")]
        [TestCase((sbyte)12, "12")]
        [TestCase((sbyte)16, "16")]
        [TestCase((sbyte)20, "20")]
        [TestCase((sbyte)24, "24")]
        [TestCase((sbyte)28, "28")]
        [TestCase((sbyte)32, "32")]
        [Description("Querying")]
        public static void TestToNumber(sbyte bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0L, "0")]
        [TestCase(1L, "1")]
        [TestCase(2L, "2")]
        [TestCase(4L, "4")]
        [TestCase(8L, "8")]
        [TestCase(12L, "c")]
        [TestCase(16L, "10")]
        [TestCase(20L, "14")]
        [TestCase(24L, "18")]
        [TestCase(28L, "1c")]
        [TestCase(32L, "20")]
        [Description("Querying")]
        public static void TestToHex(long bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0, "0")]
        [TestCase(1, "1")]
        [TestCase(2, "2")]
        [TestCase(4, "4")]
        [TestCase(8, "8")]
        [TestCase(12, "c")]
        [TestCase(16, "10")]
        [TestCase(20, "14")]
        [TestCase(24, "18")]
        [TestCase(28, "1c")]
        [TestCase(32, "20")]
        [Description("Querying")]
        public static void TestToHex(int bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase(0u, "0")]
        [TestCase(1u, "1")]
        [TestCase(2u, "2")]
        [TestCase(4u, "4")]
        [TestCase(8u, "8")]
        [TestCase(12u, "c")]
        [TestCase(16u, "10")]
        [TestCase(20u, "14")]
        [TestCase(24u, "18")]
        [TestCase(28u, "1c")]
        [TestCase(32u, "20")]
        [Description("Querying")]
        public static void TestToHex(uint bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((short)0, "0")]
        [TestCase((short)1, "1")]
        [TestCase((short)2, "2")]
        [TestCase((short)4, "4")]
        [TestCase((short)8, "8")]
        [TestCase((short)12, "c")]
        [TestCase((short)16, "10")]
        [TestCase((short)20, "14")]
        [TestCase((short)24, "18")]
        [TestCase((short)28, "1c")]
        [TestCase((short)32, "20")]
        [Description("Querying")]
        public static void TestToHex(short bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((ushort)0u, "0")]
        [TestCase((ushort)1u, "1")]
        [TestCase((ushort)2u, "2")]
        [TestCase((ushort)4u, "4")]
        [TestCase((ushort)8u, "8")]
        [TestCase((ushort)12u, "c")]
        [TestCase((ushort)16u, "10")]
        [TestCase((ushort)20u, "14")]
        [TestCase((ushort)24u, "18")]
        [TestCase((ushort)28u, "1c")]
        [TestCase((ushort)32u, "20")]
        [Description("Querying")]
        public static void TestToHex(ushort bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((byte)0, "0")]
        [TestCase((byte)1, "1")]
        [TestCase((byte)2, "2")]
        [TestCase((byte)4, "4")]
        [TestCase((byte)8, "8")]
        [TestCase((byte)12, "c")]
        [TestCase((byte)16, "10")]
        [TestCase((byte)20, "14")]
        [TestCase((byte)24, "18")]
        [TestCase((byte)28, "1c")]
        [TestCase((byte)32, "20")]
        [Description("Querying")]
        public static void TestToHex(byte bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [Test]
        [TestCase((sbyte)0, "0")]
        [TestCase((sbyte)1, "1")]
        [TestCase((sbyte)2, "2")]
        [TestCase((sbyte)4, "4")]
        [TestCase((sbyte)8, "8")]
        [TestCase((sbyte)12, "c")]
        [TestCase((sbyte)16, "10")]
        [TestCase((sbyte)20, "14")]
        [TestCase((sbyte)24, "18")]
        [TestCase((sbyte)28, "1c")]
        [TestCase((sbyte)32, "20")]
        [Description("Querying")]
        public static void TestToHex(sbyte bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

    }
}
