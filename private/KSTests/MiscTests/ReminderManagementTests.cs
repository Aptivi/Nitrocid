
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

using KS.Misc.Calendar.Reminders;
using NUnit.Framework;
using Shouldly;
using System;

namespace KSTests.MiscTests
{

    [TestFixture]
    public class ReminderManagementTests
    {

        /// <summary>
        /// Tests adding the reminder
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddReminder()
        {
            ReminderManager.AddReminder(new DateTime(2022, 2, 22), "Kernel Simulator second-gen release");
            ReminderManager.Reminders.ShouldNotBeNull();
            ReminderManager.Reminders.ShouldNotBeEmpty();
            ReminderManager.Reminders[0].ReminderDate.Day.ShouldBe(22);
            ReminderManager.Reminders[0].ReminderDate.Month.ShouldBe(2);
            ReminderManager.Reminders[0].ReminderDate.Year.ShouldBe(2022);
        }

        /// <summary>
        /// Tests adding the reminder
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestRemoveReminder()
        {
            ReminderManager.RemoveReminder(new DateTime(2022, 2, 22), 1);
            ReminderManager.Reminders.ShouldNotBeNull();
            ReminderManager.Reminders.ShouldBeEmpty();
        }

    }
}
