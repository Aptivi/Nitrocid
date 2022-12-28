
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

using NUnit.Framework;
using Shouldly;

namespace KSTests.TimeDateTests
{

    [TestFixture]
    public class TimeInitializationTests
    {

        /// <summary>
        /// Tests initializing current times in all timezones
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestGetTimeZones()
        {
            var TimeZones = KS.TimeDate.TimeZones.GetTimeZones();
            TimeZones.ShouldNotBeNull();
            TimeZones.ShouldNotBeEmpty();
        }

    }
}
