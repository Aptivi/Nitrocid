
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports KS.Misc.Calendar.Reminders

<TestClass()> Public Class ReminderManagementTests

    ''' <summary>
    ''' Tests adding the reminder
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestAddReminder()
        AddReminder(New Date(2022, 2, 22), "Kernel Simulator second-gen release")
        Reminders.ShouldNotBeNull
        Reminders.ShouldNotBeEmpty
        Reminders(0).ReminderDate.Day.ShouldBe(22)
        Reminders(0).ReminderDate.Month.ShouldBe(2)
        Reminders(0).ReminderDate.Year.ShouldBe(2022)
    End Sub

    ''' <summary>
    ''' Tests adding the reminder
    ''' </summary>
    <TestMethod()> <TestCategory("Management")> Public Sub TestRemoveReminder()
        AddReminder(New Date(2022, 2, 22), 1)
        Reminders.ShouldNotBeNull
        Reminders.ShouldBeEmpty
    End Sub

End Class