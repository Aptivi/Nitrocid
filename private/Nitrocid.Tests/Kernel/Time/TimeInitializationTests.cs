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
using Nitrocid.Base.Kernel.Time.Converters;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Kernel.Time.Timezones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;

namespace Nitrocid.Tests.Kernel.Time
{

    [TestClass]
    public class TimeInitializationTests
    {

        /// <summary>
        /// Tests initializing current times in all timezones
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGetTimeZones()
        {
            var timeZones = TimeZones.GetTimeZoneTimes();
            timeZones.ShouldNotBeNull();
            timeZones.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests getting time from timezone
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGetZoneTime()
        {
            var timeZones = TimeZones.GetTimeZoneTimes();
            int randomZone = RandomDriver.RandomIdx(timeZones.Count);
            string zone = timeZones.ElementAt(randomZone).Key;
            DateTime unexpected = new(1970, 1, 1);
            DateTime zoneTime = unexpected;
            Should.NotThrow(() => zoneTime = TimeZoneRenderers.GetZoneTime(zone));
            zoneTime.ShouldNotBe(unexpected);
        }

        /// <summary>
        /// Tests getting time from timezone
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGetDateTimeFromZone()
        {
            var timeZones = TimeZones.GetTimeZoneTimes();
            int randomZone = RandomDriver.RandomIdx(timeZones.Count);
            string zone = timeZones.ElementAt(randomZone).Key;
            DateTime unexpected = new(1970, 1, 1);
            DateTime zoneTime = unexpected;
            Should.NotThrow(() => zoneTime = TimeDateConverters.GetDateTimeFromZone(zone));
            zoneTime.ShouldNotBe(unexpected);
        }

        /// <summary>
        /// Tests getting time from timezone
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestGetZoneTimeString()
        {
            var timeZones = TimeZones.GetTimeZoneTimes();
            int randomZone = RandomDriver.RandomIdx(timeZones.Count);
            string zone = timeZones.ElementAt(randomZone).Key;
            string unexpected = TimeDateRenderers.Render(new DateTime(1970, 1, 1));
            string zoneTime = unexpected;
            Should.NotThrow(() => zoneTime = TimeZoneRenderers.GetZoneTimeString(zone));
            zoneTime.ShouldNotBe(unexpected);
        }

        /// <summary>
        /// Tests checking to see if a specified time zone exists
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestTimeZoneExists()
        {
            var timeZones = TimeZones.GetTimeZoneTimes();
            int randomZone = RandomDriver.RandomIdx(timeZones.Count);
            string zone = timeZones.ElementAt(randomZone).Key;
            TimeZones.TimeZoneExists(zone).ShouldBeTrue();
        }

        /// <summary>
        /// Tests checking to see if a nonexistent time zone exists
        /// </summary>
        [TestMethod]
        [Description("Initialization")]
        public void TestTimeZoneExistsNonexistent() =>
            TimeZones.TimeZoneExists("GMT Version 2.0").ShouldBeFalse();

    }
}
