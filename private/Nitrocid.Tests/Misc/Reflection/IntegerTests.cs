//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Base.Misc.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Misc.Reflection
{
    [TestClass]
    public class IntegerTests
    {

        /// <summary>
        /// Tests swapping integers if the source integer is larger than the target
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestSwapIfSourceLarger()
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
        [TestMethod]
        [Description("Querying")]
        public void TestSwapIfSourceLargerShouldNotSwap()
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
        [TestMethod]
        [Description("Querying")]
        public void TestGetDigits()
        {
            long number = 45000;
            int expectedDigits = 5;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetDigitsNegativeNumber()
        {
            long number = -45000;
            int expectedDigits = 5;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetDigitsOverflowingInteger()
        {
            long number = 450000000000;
            int expectedDigits = 12;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetDigitsOverflowingIntegerNegativeNumber()
        {
            long number = -450000000000;
            int expectedDigits = 12;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetDigitsInt()
        {
            int number = 45000;
            int expectedDigits = 5;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetDigitsIntNegativeNumber()
        {
            int number = -45000;
            int expectedDigits = 5;
            int digits = number.GetDigits();
            digits.ShouldBe(expectedDigits);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(-1, "0 B")]
        [DataRow(0, "0 B")]
        [DataRow(1, "1 B")]
        [DataRow(1024, "1 KB")]
        [DataRow(1024 * 1024, "1 MB")]
        [DataRow(1024 * 1024 * 1024, "1 GB")]
        [DataRow(1024L * 1024 * 1024 * 1024, "1 TB")]
        [DataRow(1024L * 1024 * 1024 * 1024 * 1024, "1 PB")]
        [DataRow(1024L * 1024 * 1024 * 1024 * 1024 * 1024, "1 EB")]
        [Description("Querying")]
        public void TestFileSizeString(long bytes, string expectedHumanized)
        {
            string humanized = bytes.SizeString();
            humanized.ShouldBe(expectedHumanized);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0L, "0")]
        [DataRow(1L, "1")]
        [DataRow(2L, "10")]
        [DataRow(4L, "100")]
        [DataRow(8L, "1000")]
        [DataRow(12L, "1100")]
        [DataRow(16L, "10000")]
        [DataRow(20L, "10100")]
        [DataRow(24L, "11000")]
        [DataRow(28L, "11100")]
        [DataRow(32L, "100000")]
        [Description("Querying")]
        public void TestToBinary(long bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0, "0")]
        [DataRow(1, "1")]
        [DataRow(2, "10")]
        [DataRow(4, "100")]
        [DataRow(8, "1000")]
        [DataRow(12, "1100")]
        [DataRow(16, "10000")]
        [DataRow(20, "10100")]
        [DataRow(24, "11000")]
        [DataRow(28, "11100")]
        [DataRow(32, "100000")]
        [Description("Querying")]
        public void TestToBinary(int bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0u, "0")]
        [DataRow(1u, "1")]
        [DataRow(2u, "10")]
        [DataRow(4u, "100")]
        [DataRow(8u, "1000")]
        [DataRow(12u, "1100")]
        [DataRow(16u, "10000")]
        [DataRow(20u, "10100")]
        [DataRow(24u, "11000")]
        [DataRow(28u, "11100")]
        [DataRow(32u, "100000")]
        [Description("Querying")]
        public void TestToBinary(uint bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((short)0, "0")]
        [DataRow((short)1, "1")]
        [DataRow((short)2, "10")]
        [DataRow((short)4, "100")]
        [DataRow((short)8, "1000")]
        [DataRow((short)12, "1100")]
        [DataRow((short)16, "10000")]
        [DataRow((short)20, "10100")]
        [DataRow((short)24, "11000")]
        [DataRow((short)28, "11100")]
        [DataRow((short)32, "100000")]
        [Description("Querying")]
        public void TestToBinary(short bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((ushort)0u, "0")]
        [DataRow((ushort)1u, "1")]
        [DataRow((ushort)2u, "10")]
        [DataRow((ushort)4u, "100")]
        [DataRow((ushort)8u, "1000")]
        [DataRow((ushort)12u, "1100")]
        [DataRow((ushort)16u, "10000")]
        [DataRow((ushort)20u, "10100")]
        [DataRow((ushort)24u, "11000")]
        [DataRow((ushort)28u, "11100")]
        [DataRow((ushort)32u, "100000")]
        [Description("Querying")]
        public void TestToBinary(ushort bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((byte)0, "0")]
        [DataRow((byte)1, "1")]
        [DataRow((byte)2, "10")]
        [DataRow((byte)4, "100")]
        [DataRow((byte)8, "1000")]
        [DataRow((byte)12, "1100")]
        [DataRow((byte)16, "10000")]
        [DataRow((byte)20, "10100")]
        [DataRow((byte)24, "11000")]
        [DataRow((byte)28, "11100")]
        [DataRow((byte)32, "100000")]
        [Description("Querying")]
        public void TestToBinary(byte bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((sbyte)0, "0")]
        [DataRow((sbyte)1, "1")]
        [DataRow((sbyte)2, "10")]
        [DataRow((sbyte)4, "100")]
        [DataRow((sbyte)8, "1000")]
        [DataRow((sbyte)12, "1100")]
        [DataRow((sbyte)16, "10000")]
        [DataRow((sbyte)20, "10100")]
        [DataRow((sbyte)24, "11000")]
        [DataRow((sbyte)28, "11100")]
        [DataRow((sbyte)32, "100000")]
        [Description("Querying")]
        public void TestToBinary(sbyte bytes, string expected)
        {
            string representation = bytes.ToBinary();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0L, "0")]
        [DataRow(1L, "1")]
        [DataRow(2L, "2")]
        [DataRow(4L, "4")]
        [DataRow(8L, "10")]
        [DataRow(12L, "14")]
        [DataRow(16L, "20")]
        [DataRow(20L, "24")]
        [DataRow(24L, "30")]
        [DataRow(28L, "34")]
        [DataRow(32L, "40")]
        [Description("Querying")]
        public void TestToOctal(long bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0, "0")]
        [DataRow(1, "1")]
        [DataRow(2, "2")]
        [DataRow(4, "4")]
        [DataRow(8, "10")]
        [DataRow(12, "14")]
        [DataRow(16, "20")]
        [DataRow(20, "24")]
        [DataRow(24, "30")]
        [DataRow(28, "34")]
        [DataRow(32, "40")]
        [Description("Querying")]
        public void TestToOctal(int bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0u, "0")]
        [DataRow(1u, "1")]
        [DataRow(2u, "2")]
        [DataRow(4u, "4")]
        [DataRow(8u, "10")]
        [DataRow(12u, "14")]
        [DataRow(16u, "20")]
        [DataRow(20u, "24")]
        [DataRow(24u, "30")]
        [DataRow(28u, "34")]
        [DataRow(32u, "40")]
        [Description("Querying")]
        public void TestToOctal(uint bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((short)0, "0")]
        [DataRow((short)1, "1")]
        [DataRow((short)2, "2")]
        [DataRow((short)4, "4")]
        [DataRow((short)8, "10")]
        [DataRow((short)12, "14")]
        [DataRow((short)16, "20")]
        [DataRow((short)20, "24")]
        [DataRow((short)24, "30")]
        [DataRow((short)28, "34")]
        [DataRow((short)32, "40")]
        [Description("Querying")]
        public void TestToOctal(short bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((ushort)0u, "0")]
        [DataRow((ushort)1u, "1")]
        [DataRow((ushort)2u, "2")]
        [DataRow((ushort)4u, "4")]
        [DataRow((ushort)8u, "10")]
        [DataRow((ushort)12u, "14")]
        [DataRow((ushort)16u, "20")]
        [DataRow((ushort)20u, "24")]
        [DataRow((ushort)24u, "30")]
        [DataRow((ushort)28u, "34")]
        [DataRow((ushort)32u, "40")]
        [Description("Querying")]
        public void TestToOctal(ushort bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((byte)0, "0")]
        [DataRow((byte)1, "1")]
        [DataRow((byte)2, "2")]
        [DataRow((byte)4, "4")]
        [DataRow((byte)8, "10")]
        [DataRow((byte)12, "14")]
        [DataRow((byte)16, "20")]
        [DataRow((byte)20, "24")]
        [DataRow((byte)24, "30")]
        [DataRow((byte)28, "34")]
        [DataRow((byte)32, "40")]
        [Description("Querying")]
        public void TestToOctal(byte bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((sbyte)0, "0")]
        [DataRow((sbyte)1, "1")]
        [DataRow((sbyte)2, "2")]
        [DataRow((sbyte)4, "4")]
        [DataRow((sbyte)8, "10")]
        [DataRow((sbyte)12, "14")]
        [DataRow((sbyte)16, "20")]
        [DataRow((sbyte)20, "24")]
        [DataRow((sbyte)24, "30")]
        [DataRow((sbyte)28, "34")]
        [DataRow((sbyte)32, "40")]
        [Description("Querying")]
        public void TestToOctal(sbyte bytes, string expected)
        {
            string representation = bytes.ToOctal();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0L, "0")]
        [DataRow(1L, "1")]
        [DataRow(2L, "2")]
        [DataRow(4L, "4")]
        [DataRow(8L, "8")]
        [DataRow(12L, "12")]
        [DataRow(16L, "16")]
        [DataRow(20L, "20")]
        [DataRow(24L, "24")]
        [DataRow(28L, "28")]
        [DataRow(32L, "32")]
        [Description("Querying")]
        public void TestToNumber(long bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0, "0")]
        [DataRow(1, "1")]
        [DataRow(2, "2")]
        [DataRow(4, "4")]
        [DataRow(8, "8")]
        [DataRow(12, "12")]
        [DataRow(16, "16")]
        [DataRow(20, "20")]
        [DataRow(24, "24")]
        [DataRow(28, "28")]
        [DataRow(32, "32")]
        [Description("Querying")]
        public void TestToNumber(int bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0u, "0")]
        [DataRow(1u, "1")]
        [DataRow(2u, "2")]
        [DataRow(4u, "4")]
        [DataRow(8u, "8")]
        [DataRow(12u, "12")]
        [DataRow(16u, "16")]
        [DataRow(20u, "20")]
        [DataRow(24u, "24")]
        [DataRow(28u, "28")]
        [DataRow(32u, "32")]
        [Description("Querying")]
        public void TestToNumber(uint bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((short)0, "0")]
        [DataRow((short)1, "1")]
        [DataRow((short)2, "2")]
        [DataRow((short)4, "4")]
        [DataRow((short)8, "8")]
        [DataRow((short)12, "12")]
        [DataRow((short)16, "16")]
        [DataRow((short)20, "20")]
        [DataRow((short)24, "24")]
        [DataRow((short)28, "28")]
        [DataRow((short)32, "32")]
        [Description("Querying")]
        public void TestToNumber(short bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((ushort)0u, "0")]
        [DataRow((ushort)1u, "1")]
        [DataRow((ushort)2u, "2")]
        [DataRow((ushort)4u, "4")]
        [DataRow((ushort)8u, "8")]
        [DataRow((ushort)12u, "12")]
        [DataRow((ushort)16u, "16")]
        [DataRow((ushort)20u, "20")]
        [DataRow((ushort)24u, "24")]
        [DataRow((ushort)28u, "28")]
        [DataRow((ushort)32u, "32")]
        [Description("Querying")]
        public void TestToNumber(ushort bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((byte)0, "0")]
        [DataRow((byte)1, "1")]
        [DataRow((byte)2, "2")]
        [DataRow((byte)4, "4")]
        [DataRow((byte)8, "8")]
        [DataRow((byte)12, "12")]
        [DataRow((byte)16, "16")]
        [DataRow((byte)20, "20")]
        [DataRow((byte)24, "24")]
        [DataRow((byte)28, "28")]
        [DataRow((byte)32, "32")]
        [Description("Querying")]
        public void TestToNumber(byte bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((sbyte)0, "0")]
        [DataRow((sbyte)1, "1")]
        [DataRow((sbyte)2, "2")]
        [DataRow((sbyte)4, "4")]
        [DataRow((sbyte)8, "8")]
        [DataRow((sbyte)12, "12")]
        [DataRow((sbyte)16, "16")]
        [DataRow((sbyte)20, "20")]
        [DataRow((sbyte)24, "24")]
        [DataRow((sbyte)28, "28")]
        [DataRow((sbyte)32, "32")]
        [Description("Querying")]
        public void TestToNumber(sbyte bytes, string expected)
        {
            string representation = bytes.ToNumber();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0L, "0")]
        [DataRow(1L, "1")]
        [DataRow(2L, "2")]
        [DataRow(4L, "4")]
        [DataRow(8L, "8")]
        [DataRow(12L, "c")]
        [DataRow(16L, "10")]
        [DataRow(20L, "14")]
        [DataRow(24L, "18")]
        [DataRow(28L, "1c")]
        [DataRow(32L, "20")]
        [Description("Querying")]
        public void TestToHex(long bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0, "0")]
        [DataRow(1, "1")]
        [DataRow(2, "2")]
        [DataRow(4, "4")]
        [DataRow(8, "8")]
        [DataRow(12, "c")]
        [DataRow(16, "10")]
        [DataRow(20, "14")]
        [DataRow(24, "18")]
        [DataRow(28, "1c")]
        [DataRow(32, "20")]
        [Description("Querying")]
        public void TestToHex(int bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow(0u, "0")]
        [DataRow(1u, "1")]
        [DataRow(2u, "2")]
        [DataRow(4u, "4")]
        [DataRow(8u, "8")]
        [DataRow(12u, "c")]
        [DataRow(16u, "10")]
        [DataRow(20u, "14")]
        [DataRow(24u, "18")]
        [DataRow(28u, "1c")]
        [DataRow(32u, "20")]
        [Description("Querying")]
        public void TestToHex(uint bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((short)0, "0")]
        [DataRow((short)1, "1")]
        [DataRow((short)2, "2")]
        [DataRow((short)4, "4")]
        [DataRow((short)8, "8")]
        [DataRow((short)12, "c")]
        [DataRow((short)16, "10")]
        [DataRow((short)20, "14")]
        [DataRow((short)24, "18")]
        [DataRow((short)28, "1c")]
        [DataRow((short)32, "20")]
        [Description("Querying")]
        public void TestToHex(short bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((ushort)0u, "0")]
        [DataRow((ushort)1u, "1")]
        [DataRow((ushort)2u, "2")]
        [DataRow((ushort)4u, "4")]
        [DataRow((ushort)8u, "8")]
        [DataRow((ushort)12u, "c")]
        [DataRow((ushort)16u, "10")]
        [DataRow((ushort)20u, "14")]
        [DataRow((ushort)24u, "18")]
        [DataRow((ushort)28u, "1c")]
        [DataRow((ushort)32u, "20")]
        [Description("Querying")]
        public void TestToHex(ushort bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((byte)0, "0")]
        [DataRow((byte)1, "1")]
        [DataRow((byte)2, "2")]
        [DataRow((byte)4, "4")]
        [DataRow((byte)8, "8")]
        [DataRow((byte)12, "c")]
        [DataRow((byte)16, "10")]
        [DataRow((byte)20, "14")]
        [DataRow((byte)24, "18")]
        [DataRow((byte)28, "1c")]
        [DataRow((byte)32, "20")]
        [Description("Querying")]
        public void TestToHex(byte bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting digits
        /// </summary>
        [TestMethod]
        [DataRow((sbyte)0, "0")]
        [DataRow((sbyte)1, "1")]
        [DataRow((sbyte)2, "2")]
        [DataRow((sbyte)4, "4")]
        [DataRow((sbyte)8, "8")]
        [DataRow((sbyte)12, "c")]
        [DataRow((sbyte)16, "10")]
        [DataRow((sbyte)20, "14")]
        [DataRow((sbyte)24, "18")]
        [DataRow((sbyte)28, "1c")]
        [DataRow((sbyte)32, "20")]
        [Description("Querying")]
        public void TestToHex(sbyte bytes, string expected)
        {
            string representation = bytes.ToHex();
            representation.ShouldBe(expected);
        }

    }
}
