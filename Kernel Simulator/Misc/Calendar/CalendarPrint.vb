
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
Imports KS.Misc.Calendar.Reminders
Imports KS.Misc.Writers.FancyWriters.Tools

Namespace Misc.Calendar
    Public Module CalendarPrint

        ''' <summary>
        ''' Prints the table of the calendar
        ''' </summary>
        Public Sub PrintCalendar()
            PrintCalendar(Date.Today.Year, Date.Today.Month)
        End Sub

        ''' <summary>
        ''' Prints the table of the calendar
        ''' </summary>
        Public Sub PrintCalendar(Year As Integer, Month As Integer)
            Dim CalendarDays As String() = CurrentCult.DateTimeFormat.DayNames
            Dim CalendarMonths As String() = CurrentCult.DateTimeFormat.MonthNames
            Dim CalendarData(5, CalendarDays.Length - 1) As String
            Dim DateFrom As New Date(Year, Month, 1, CurrentCult.Calendar)
            Dim DateTo As New Date(Year, Month, Date.DaysInMonth(Year, Month), CurrentCult.Calendar)
            Dim CurrentWeek As Integer = 1
            Dim CalendarTitle As String = CalendarMonths(Month - 1) & " " & Year
            Dim CalendarCellOptions As New List(Of CellOptions)

            'Populate the calendar data
            WriteWhere(CalendarTitle, CInt((Console.WindowWidth - CalendarTitle.Length) / 2), Console.CursorTop, True, ColTypes.Neutral)
            Console.WriteLine()
            For CurrentDay As Integer = 1 To DateTo.Day
                Dim CurrentDate As New Date(Year, Month, CurrentDay, CurrentCult.DateTimeFormat.Calendar)
                If CurrentDate.DayOfWeek = 0 Then CurrentWeek += 1
                Dim CurrentWeekIndex As Integer = CurrentWeek - 1
                Dim CurrentDayMark As String = $" {CurrentDay} "
                Dim ReminderMarked As Boolean = False
                Dim EventMarked As Boolean = False
                Dim IsWeekend As Boolean = CurrentDate.DayOfWeek = DayOfWeek.Friday Or CurrentDate.DayOfWeek = DayOfWeek.Saturday

                'Dim out the weekends
                If IsWeekend Then
                    Dim WeekendOptions As New CellOptions(CurrentDate.DayOfWeek + 1, CurrentWeek) With {
                    .ColoredCell = True,
                    .CellColor = New Color(128, 128, 128),
                    .CellBackgroundColor = BackgroundColor
                }
                    CalendarCellOptions.Add(WeekendOptions)
                End If

                'Know where and how to put the day number
                For Each Reminder As ReminderInfo In Reminders.Reminders
                    If Reminder.ReminderDate.Date = CurrentDate And Not ReminderMarked Then
                        CurrentDayMark = $"({CurrentDay})"
                        ReminderMarked = True
                    End If
                Next
                For Each EventInstance As EventInfo In CalendarEvents
                    If EventInstance.EventDate = CurrentDate And Not EventMarked Then
                        Dim EventCell As New CellOptions(CurrentDate.DayOfWeek + 1, CurrentWeek) With {
                        .ColoredCell = True,
                        .CellColor = StageColor,
                        .CellBackgroundColor = BackgroundColor
                    }
                        CalendarCellOptions.Add(EventCell)
                        EventMarked = True
                    End If
                Next
                CalendarData(CurrentWeekIndex, CurrentDate.DayOfWeek) = CurrentDayMark
            Next
            WriteTable(CalendarDays, CalendarData, 2, True, CalendarCellOptions)
        End Sub

    End Module
End Namespace