//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;
using Nitrocid.Misc.Reflection;

namespace Nitrocid.Tests.Drivers
{
    [TestClass]
    public class SortingActionTests
    {

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt8BestCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt16BestCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt32BestCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt64BestCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt128BestCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersFloatBestCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersDoubleBestCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt8MidCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt16MidCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt32MidCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt64MidCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt128MidCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersFloatMidCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersDoubleMidCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt8WorstCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt16WorstCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt32WorstCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt64WorstCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt128WorstCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersFloatWorstCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersDoubleWorstCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt8BestCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt16BestCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt32BestCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt64BestCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt128BestCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersFloatBestCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersDoubleBestCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt8MidCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt16MidCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt32MidCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt64MidCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt128MidCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersFloatMidCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersDoubleMidCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt8WorstCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt16WorstCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt32WorstCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt64WorstCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersInt128WorstCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersFloatWorstCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSortNumbersDoubleWorstCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Default").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt8BestCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt16BestCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt32BestCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt64BestCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt128BestCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersFloatBestCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersDoubleBestCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt8MidCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt16MidCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt32MidCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt64MidCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt128MidCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersFloatMidCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersDoubleMidCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt8WorstCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt16WorstCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt32WorstCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt64WorstCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt128WorstCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersFloatWorstCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersDoubleWorstCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt8BestCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt16BestCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt32BestCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt64BestCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt128BestCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersFloatBestCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersDoubleBestCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt8MidCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt16MidCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt32MidCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt64MidCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt128MidCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersFloatMidCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersDoubleMidCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt8WorstCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt16WorstCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt32WorstCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt64WorstCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersInt128WorstCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersFloatWorstCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestQuickSortNumbersDoubleWorstCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Quick").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt8BestCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt16BestCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt32BestCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt64BestCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt128BestCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersFloatBestCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersDoubleBestCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt8MidCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt16MidCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt32MidCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt64MidCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt128MidCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersFloatMidCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersDoubleMidCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt8WorstCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt16WorstCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt32WorstCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt64WorstCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt128WorstCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersFloatWorstCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersDoubleWorstCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt8BestCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt16BestCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt32BestCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt64BestCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt128BestCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersFloatBestCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersDoubleBestCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt8MidCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt16MidCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt32MidCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt64MidCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt128MidCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersFloatMidCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersDoubleMidCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt8WorstCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt16WorstCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt32WorstCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt64WorstCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersInt128WorstCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersFloatWorstCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestSelectionSortNumbersDoubleWorstCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Selection").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt8BestCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt16BestCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt32BestCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt64BestCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt128BestCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersFloatBestCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersDoubleBestCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt8MidCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt16MidCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt32MidCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt64MidCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt128MidCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersFloatMidCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersDoubleMidCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt8WorstCase()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt16WorstCase()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt32WorstCase()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt64WorstCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt128WorstCase()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersFloatWorstCase()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersDoubleWorstCase()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            ArrayTools.SortNumbers(data).SequenceEqual(expected).ShouldBeTrue();
            ArrayTools.SortNumbers(data, "Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt8BestCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt16BestCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt32BestCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt64BestCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt128BestCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [1, 4, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersFloatBestCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersDoubleBestCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt8MidCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt16MidCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt32MidCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt64MidCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt128MidCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [4, 1, 8, 64, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersFloatMidCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersDoubleMidCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [1.3f, 1.2f, 8.4f, 64.5f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt8WorstCaseUsingExtension()
        {
            byte[] expected = [1, 4, 8, 64, 255];
            byte[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt16WorstCaseUsingExtension()
        {
            short[] expected = [1, 4, 8, 64, 255];
            short[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt32WorstCaseUsingExtension()
        {
            int[] expected = [1, 4, 8, 64, 255];
            int[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt64WorstCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersInt128WorstCaseUsingExtension()
        {
            long[] expected = [1, 4, 8, 64, 255];
            long[] data = [8, 4, 64, 1, 255];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersFloatWorstCaseUsingExtension()
        {
            float[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            float[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }

        [TestMethod]
        [Description("Action")]
        public void TestMergeSortNumbersDoubleWorstCaseUsingExtension()
        {
            double[] expected = [1.2f, 1.3f, 8.4f, 64.5f, 255.6f];
            double[] data = [8.4f, 1.3f, 64.5f, 1.2f, 255.6f];
            data.SortNumbers().SequenceEqual(expected).ShouldBeTrue();
            data.SortNumbers("Merge").SequenceEqual(expected).ShouldBeTrue();
        }
    }
}
