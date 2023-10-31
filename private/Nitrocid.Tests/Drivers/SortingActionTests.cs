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

using NUnit.Framework;
using Shouldly;
using KS.Drivers.Sorting;
using System.Linq;

namespace Nitrocid.Tests.Drivers
{
    [TestFixture]
    public class SortingActionTests
    {

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt8BestCase()
        {
            byte[] expected = { 1, 4, 8, 64, 255 };
            byte[] data = { 1, 4, 8, 64, 255 };
            SortingDriver.SortNumbersInt8(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt16BestCase()
        {
            short[] expected = { 1, 4, 8, 64, 255 };
            short[] data = { 1, 4, 8, 64, 255 };
            SortingDriver.SortNumbersInt16(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt32BestCase()
        {
            int[] expected = { 1, 4, 8, 64, 255 };
            int[] data = { 1, 4, 8, 64, 255 };
            SortingDriver.SortNumbersInt32(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt64BestCase()
        {
            long[] expected = { 1, 4, 8, 64, 255 };
            long[] data = { 1, 4, 8, 64, 255 };
            SortingDriver.SortNumbersInt64(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt128BestCase()
        {
            long[] expected = { 1, 4, 8, 64, 255 };
            long[] data = { 1, 4, 8, 64, 255 };
            SortingDriver.SortNumbersInt128(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt8MidCase()
        {
            byte[] expected = { 1, 4, 8, 64, 255 };
            byte[] data = { 4, 1, 8, 64, 255 };
            SortingDriver.SortNumbersInt8(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt16MidCase()
        {
            short[] expected = { 1, 4, 8, 64, 255 };
            short[] data = { 4, 1, 8, 64, 255 };
            SortingDriver.SortNumbersInt16(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt32MidCase()
        {
            int[] expected = { 1, 4, 8, 64, 255 };
            int[] data = { 4, 1, 8, 64, 255 };
            SortingDriver.SortNumbersInt32(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt64MidCase()
        {
            long[] expected = { 1, 4, 8, 64, 255 };
            long[] data = { 4, 1, 8, 64, 255 };
            SortingDriver.SortNumbersInt64(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt128MidCase()
        {
            long[] expected = { 1, 4, 8, 64, 255 };
            long[] data = { 4, 1, 8, 64, 255 };
            SortingDriver.SortNumbersInt128(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt8WorstCase()
        {
            byte[] expected = { 1, 4, 8, 64, 255 };
            byte[] data = { 8, 4, 64, 1, 255 };
            SortingDriver.SortNumbersInt8(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt16WorstCase()
        {
            short[] expected = { 1, 4, 8, 64, 255 };
            short[] data = { 8, 4, 64, 1, 255 };
            SortingDriver.SortNumbersInt16(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt32WorstCase()
        {
            int[] expected = { 1, 4, 8, 64, 255 };
            int[] data = { 8, 4, 64, 1, 255 };
            SortingDriver.SortNumbersInt32(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt64WorstCase()
        {
            long[] expected = { 1, 4, 8, 64, 255 };
            long[] data = { 8, 4, 64, 1, 255 };
            SortingDriver.SortNumbersInt64(data).SequenceEqual(expected).ShouldBeTrue();
        }

        [Test]
        [Description("Action")]
        public void TestSortNumbersInt128WorstCase()
        {
            long[] expected = { 1, 4, 8, 64, 255 };
            long[] data = { 8, 4, 64, 1, 255 };
            SortingDriver.SortNumbersInt128(data).SequenceEqual(expected).ShouldBeTrue();
        }
    }
}
