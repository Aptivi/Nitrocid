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

using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Kernel.Time.Timezones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Linq;

namespace Nitrocid.Tests.Kernel.Time
{

    [TestClass]
    public class TimeQueryingTests
    {

        /// <summary>
        /// Tests getting remaining time from now
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetRemainingTimeFromNow()
        {
            string RemainingTime = TimeDateMiscRenderers.RenderRemainingTimeFromNow(1000);
            RemainingTime.ShouldNotBeNullOrEmpty();
            RemainingTime.ShouldBe("0.00:00:01.000");
        }

        /// <summary>
        /// Tests getting remaining time from specified time
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetRemainingTimeFrom()
        {
            string RemainingTime = TimeDateMiscRenderers.RenderRemainingTimeFrom(DateTime.Today, 60000);
            RemainingTime.ShouldNotBeNullOrEmpty();
            RemainingTime.ShouldBe("0.00:01:00.000");
        }

        /// <summary>
        /// Tests getting remaining time from now
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetRemainingTimeFromNowFormatted()
        {
            string RemainingTime = TimeDateMiscRenderers.RenderRemainingTimeFromNow(1000, "mmss");
            RemainingTime.ShouldNotBeNullOrEmpty();
            RemainingTime.ShouldBe("0001");
        }

        /// <summary>
        /// Tests getting remaining time from specified time
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetRemainingTimeFromFormatted()
        {
            string RemainingTime = TimeDateMiscRenderers.RenderRemainingTimeFrom(DateTime.Today, 60000, "mmss");
            RemainingTime.ShouldNotBeNullOrEmpty();
            RemainingTime.ShouldBe("0100");
        }

        /// <summary>
        /// Tests showing UTC offset for time zone
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestShowTimeZoneUtcOffset()
        {
            var timeZones = TimeZones.GetTimeZoneTimes();
            int randomZone = RandomDriver.RandomIdx(timeZones.Count);
            string zone = timeZones.ElementAt(randomZone).Key;
            TimeSpan timeSpan = new();
            TimeSpan timeSpanUtc = new();
            Should.NotThrow(() => timeSpan = TimeZoneRenderers.ShowTimeZoneUtcOffset(zone));
            if (timeSpan >= timeSpanUtc)
                timeSpan.ShouldBeGreaterThanOrEqualTo(timeSpanUtc);
            else
                timeSpan.ShouldBeLessThan(timeSpanUtc);
        }

        /// <summary>
        /// Tests showing UTC offset for time zone
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestShowTimeZoneUtcOffsetString()
        {
            var timeZones = TimeZones.GetTimeZoneTimes();
            int randomZone = RandomDriver.RandomIdx(timeZones.Count);
            string zone = timeZones.ElementAt(randomZone).Key;
            TimeSpan timeSpan = new();
            TimeSpan timeSpanUtc = new();
            Should.NotThrow(() => timeSpan = TimeZoneRenderers.ShowTimeZoneUtcOffset(zone));
            if (timeSpan >= timeSpanUtc)
                TimeZoneRenderers.ShowTimeZoneUtcOffsetString(zone).ShouldNotContain("-");
            else
                TimeZoneRenderers.ShowTimeZoneUtcOffsetString(zone).ShouldContain("-");
        }

        /// <summary>
        /// Tests showing UTC offset for time zone
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestShowTimeZoneUtcOffsetLocal()
        {
            TimeSpan timeSpan = new();
            TimeSpan timeSpanUtc = new();
            Should.NotThrow(() => timeSpan = TimeZoneRenderers.ShowTimeZoneUtcOffsetLocal());
            if (timeSpan >= timeSpanUtc)
                timeSpan.ShouldBeGreaterThanOrEqualTo(timeSpanUtc);
            else
                timeSpan.ShouldBeLessThan(timeSpanUtc);
        }

        /// <summary>
        /// Tests showing UTC offset for time zone
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestShowTimeZoneUtcOffsetStringLocal()
        {
            TimeSpan timeSpan = new();
            TimeSpan timeSpanUtc = new();
            Should.NotThrow(() => timeSpan = TimeZoneRenderers.ShowTimeZoneUtcOffsetLocal());
            if (timeSpan >= timeSpanUtc)
                TimeZoneRenderers.ShowTimeZoneUtcOffsetStringLocal().ShouldNotContain("-");
            else
                TimeZoneRenderers.ShowTimeZoneUtcOffsetStringLocal().ShouldContain("-");
        }

    }
}
