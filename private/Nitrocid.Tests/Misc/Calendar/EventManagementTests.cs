
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

using KS.Kernel.Exceptions;
using KS.Misc.Calendar.Events;
using NUnit.Framework;
using Shouldly;
using System;

namespace Nitrocid.Tests.Misc.Calendar
{

    [TestFixture]
    public class EventManagementTests
    {

        /// <summary>
        /// Tests adding the event
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddEvent()
        {
            EventManager.AddEvent(new DateTime(2022, 2, 22), "Nitrocid KS second-gen release");
            EventManager.CalendarEvents.ShouldNotBeNull();
            EventManager.CalendarEvents.ShouldNotBeEmpty();
            EventManager.CalendarEvents[0].EventDate.Day.ShouldBe(22);
            EventManager.CalendarEvents[0].EventDate.Month.ShouldBe(2);
            EventManager.CalendarEvents[0].EventDate.Year.ShouldBe(2022);
        }

        /// <summary>
        /// Tests removing an event
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestRemoveEvent()
        {
            Should.NotThrow(() => EventManager.RemoveEvent(new DateTime(2022, 2, 22), 1));
            EventManager.CalendarEvents.ShouldNotBeNull();
            EventManager.CalendarEvents.ShouldBeEmpty();
        }

        /// <summary>
        /// Tests trying to remove a non-existent event
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestRemoveNonexistentEvent()
        {
            Should.Throw(() => EventManager.RemoveEvent(new DateTime(2022, 2, 22), 5), typeof(KernelException));
            EventManager.CalendarEvents.ShouldNotBeNull();
            EventManager.CalendarEvents.ShouldBeEmpty();
        }

    }
}
