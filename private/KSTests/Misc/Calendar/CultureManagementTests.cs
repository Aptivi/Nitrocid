
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

using KS.Misc.Calendar;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Misc.Calendar
{

    [TestFixture]
    public class CultureManagementTests
    {

        /// <summary>
        /// Tests getting the culture
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetCultureFromCalendar()
        {
            var cult = CalendarTools.GetCultureFromCalendar(CalendarTypes.Gregorian);
            cult.ShouldNotBeNull();
            cult.Name.ShouldBe("en-US");
        }

        /// <summary>
        /// Tests getting the culture
        /// </summary>
        [Test]
        [TestCase(CalendarTypes.Gregorian, ExpectedResult = "en-US")]
        [TestCase(CalendarTypes.Hijri, ExpectedResult = "ar")]
        [TestCase(CalendarTypes.SaudiHijri, ExpectedResult = "ar-SA")]
        [TestCase(CalendarTypes.Persian, ExpectedResult = "fa")]
        [TestCase(CalendarTypes.ThaiBuddhist, ExpectedResult = "th-TH")]
        [TestCase(CalendarTypes.Chinese, ExpectedResult = "zh-CN")]
        [TestCase(CalendarTypes.Japanese, ExpectedResult = "ja-JP")]
        [Description("Management")]
        public string TestGetCultureFromCalendars(CalendarTypes type)
        {
            var cult = CalendarTools.GetCultureFromCalendar(type);
            cult.ShouldNotBeNull();
            return cult.Name;
        }

    }
}
