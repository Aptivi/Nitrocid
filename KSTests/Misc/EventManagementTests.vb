
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

Imports KS.Misc.Calendar.Events

<TestFixture> Public Class EventManagementTests

    ''' <summary>
    ''' Tests adding the event
    ''' </summary>
    <Test, Description("Management")> Public Sub TestAddEvent()
        AddEvent(New Date(2022, 2, 22), "Kernel Simulator second-gen release")
        CalendarEvents.ShouldNotBeNull
        CalendarEvents.ShouldNotBeEmpty
        CalendarEvents(0).EventDate.Day.ShouldBe(22)
        CalendarEvents(0).EventDate.Month.ShouldBe(2)
        CalendarEvents(0).EventDate.Year.ShouldBe(2022)
    End Sub

    ''' <summary>
    ''' Tests adding the event
    ''' </summary>
    <Test, Description("Management")> Public Sub TestRemoveEvent()
        RemoveEvent(New Date(2022, 2, 22), 1)
        CalendarEvents.ShouldNotBeNull
        CalendarEvents.ShouldBeEmpty
    End Sub

End Class