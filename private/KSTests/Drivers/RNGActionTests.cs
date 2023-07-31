
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

using NUnit.Framework;
using Shouldly;
using KS.Drivers.RNG;

namespace KSTests.Drivers
{
    [TestFixture]
    public class RNGActionTests
    {

        [Test]
        [Description("Action")]
        public void TestRandom() => RandomDriver.Random().ShouldBeInRange(0, int.MaxValue);

        [Test]
        [Description("Action")]
        public void TestRandomMax() => RandomDriver.Random(100).ShouldBeInRange(0, 100);

        [Test]
        [Description("Action")]
        public void TestRandomMinMax() => RandomDriver.Random(50, 100).ShouldBeInRange(50, 100);

        [Test]
        [Description("Action")]
        public void TestRandomShort() => RandomDriver.RandomShort().ShouldBeInRange((short)0, short.MaxValue);

        [Test]
        [Description("Action")]
        public void TestRandomShortMax() => RandomDriver.RandomShort(100).ShouldBeInRange((short)0, (short)100);

        [Test]
        [Description("Action")]
        public void TestRandomShortMinMax() => RandomDriver.RandomShort(50, 100).ShouldBeInRange((short)50, (short)100);

        [Test]
        [Description("Action")]
        public void TestRandomIdx() => RandomDriver.RandomIdx().ShouldBeInRange(0, int.MaxValue - 1);

        [Test]
        [Description("Action")]
        public void TestRandomIdxMax() => RandomDriver.RandomIdx(100).ShouldBeInRange(0, 100 - 1);

        [Test]
        [Description("Action")]
        public void TestRandomIdxMinMax() => RandomDriver.RandomIdx(50, 100).ShouldBeInRange(50, 100 - 1);

        [Test]
        [Description("Action")]
        public void TestRandomDouble() => RandomDriver.RandomDouble().ShouldBeInRange(0d, 1.0d);

        [Test]
        [Description("Action")]
        public void TestRandomDoubleMax() => RandomDriver.RandomDouble(100d).ShouldBeInRange(0d, 100d);

    }
}
