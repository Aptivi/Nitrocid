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

using Nitrocid.Base.Drivers.RNG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Drivers
{
    [TestClass]
    public class RNGActionTests
    {

        [TestMethod]
        [Description("Action")]
        public void TestRandom() =>
            RandomDriver.Random().ShouldBeInRange(0, int.MaxValue);

        [TestMethod]
        [Description("Action")]
        public void TestRandomMax() =>
            RandomDriver.Random(100).ShouldBeInRange(0, 100);

        [TestMethod]
        [Description("Action")]
        public void TestRandomMinMax() =>
            RandomDriver.Random(50, 100).ShouldBeInRange(50, 100);

        [TestMethod]
        [Description("Action")]
        public void TestRandomShort() =>
            RandomDriver.RandomShort().ShouldBeInRange((short)0, short.MaxValue);

        [TestMethod]
        [Description("Action")]
        public void TestRandomShortMax() =>
            RandomDriver.RandomShort(100).ShouldBeInRange((short)0, (short)100);

        [TestMethod]
        [Description("Action")]
        public void TestRandomShortMinMax() =>
            RandomDriver.RandomShort(50, 100).ShouldBeInRange((short)50, (short)100);

        [TestMethod]
        [Description("Action")]
        public void TestRandomIdx() =>
            RandomDriver.RandomIdx().ShouldBeInRange(0, int.MaxValue - 1);

        [TestMethod]
        [Description("Action")]
        public void TestRandomIdxMax() =>
            RandomDriver.RandomIdx(100).ShouldBeInRange(0, 100 - 1);

        [TestMethod]
        [Description("Action")]
        public void TestRandomIdxMinMax() =>
            RandomDriver.RandomIdx(50, 100).ShouldBeInRange(50, 100 - 1);

        [TestMethod]
        [Description("Action")]
        public void TestRandomDouble() =>
            RandomDriver.RandomDouble().ShouldBeInRange(0d, 1.0d);

        [TestMethod]
        [Description("Action")]
        public void TestRandomDoubleMax() =>
            RandomDriver.RandomDouble(100d).ShouldBeInRange(0d, 100d);

    }
}
