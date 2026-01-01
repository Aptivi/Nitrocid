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

using Nitrocid.Kernel.Time.Calendars;
using Nitrocid.Kernel.Time.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace Nitrocid.Tests.Kernel.Time
{

    [TestClass]
    public class TimeConversionTests
    {

        /// <summary>
        /// Tests converting the date to Unix time (seconds since 1970/1/1)
        /// </summary>
        [TestMethod]
        [Description("Conversion")]
        public void TestDateToUnix()
        {
            // Convert the target date (for example: September 20, 2014, 8:04:34 AM) to Unix
            var TargetDate = new DateTime(2014, 9, 20, 5, 4, 34, DateTimeKind.Utc);
            double ExpectedUnixTime = 1411189474d;
            double UnixTime = TimeDateConverters.DateToUnix(TargetDate);
            UnixTime.ShouldBe(ExpectedUnixTime);
        }

        /// <summary>
        /// Tests converting the Unix time (seconds since 1970/1/1) to date
        /// </summary>
        [TestMethod]
        [Description("Conversion")]
        public void TestUnixToDate()
        {
            // Convert the target date (for example: September 20, 2014, 8:04:34 AM) to Unix
            double TargetUnixTime = 1411189474d;
            var ExpectedDate = new DateTime(2014, 9, 20, 5, 4, 34, DateTimeKind.Utc);
            var ActualDate = TimeDateConverters.UnixToDate(TargetUnixTime);
            ActualDate.ShouldBe(ExpectedDate);
        }

        /// <summary>
        /// Tests getting date from calendar
        /// </summary>
        [TestMethod]
        [DataRow(CalendarTypes.Chinese, 2014, 9, 20)]
        [DataRow(CalendarTypes.Gregorian, 2014, 9, 20)]
        [DataRow(CalendarTypes.Hijri, 1435, 11, 26)]
        [DataRow(CalendarTypes.Japanese, 26, 9, 20)]
        [DataRow(CalendarTypes.Persian, 1393, 6, 29)]
        [DataRow(CalendarTypes.SaudiHijri, 1435, 11, 25)]
        [DataRow(CalendarTypes.Taiwanese, 103, 9, 20)]
        [DataRow(CalendarTypes.ThaiBuddhist, 2557, 9, 20)]
        [Description("Conversion")]
        public void GetDateFromCalendar(CalendarTypes calendarType, int year, int month, int day)
        {
            // Convert the target date (for example: September 20, 2014, 8:04:34 AM) to all calendar types
            var date = new DateTime(2014, 9, 20, 5, 4, 34, DateTimeKind.Utc);
            var (actualYear, actualMonth, actualDay, _) = TimeDateConverters.GetDateFromCalendar(date, calendarType);
            actualDay.ShouldBe(day);
            actualMonth.ShouldBe(month);
            actualYear.ShouldBe(year);
        }

        /// <summary>
        /// Tests getting date from calendar without accessing the culture information
        /// </summary>
        [TestMethod]
        [DataRow(CalendarTypes.Chinese, 2014, 9, 20)]
        [DataRow(CalendarTypes.Gregorian, 2014, 9, 20)]
        [DataRow(CalendarTypes.Hijri, 1435, 11, 26)]
        [DataRow(CalendarTypes.Japanese, 26, 9, 20)]
        [DataRow(CalendarTypes.Persian, 1393, 6, 29)]
        [DataRow(CalendarTypes.SaudiHijri, 1435, 11, 25)]
        [DataRow(CalendarTypes.Taiwanese, 103, 9, 20)]
        [DataRow(CalendarTypes.ThaiBuddhist, 2557, 9, 20)]
        [Description("Conversion")]
        public void GetDateFromCalendarNoCulture(CalendarTypes calendarType, int year, int month, int day)
        {
            // Convert the target date (for example: September 20, 2014, 8:04:34 AM) to all calendar types
            var date = new DateTime(2014, 9, 20, 5, 4, 34, DateTimeKind.Utc);
            var (actualYear, actualMonth, actualDay, _) = TimeDateConverters.GetDateFromCalendarNoCulture(date, calendarType);
            actualDay.ShouldBe(day);
            actualMonth.ShouldBe(month);
            actualYear.ShouldBe(year);
        }

    }
}
