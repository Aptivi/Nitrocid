
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.Globalization

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
        Dim CalendarDays As String() = DateTimeFormatInfo.CurrentInfo.DayNames
        Dim CalendarMonths As String() = DateTimeFormatInfo.CurrentInfo.MonthNames
        Dim CalendarData(5, CalendarDays.Length - 1) As String
        Dim DateFrom As New Date(Year, Month, 1)
        Dim DateTo As New Date(Year, Month, Date.DaysInMonth(Year, Month))
        Dim CurrentWeek As Integer = 1

        'Populate the calendar data
        WriteWhere(CalendarMonths(Month - 1) & " " & Year, (Console.WindowWidth - (DateTimeFormatInfo.CurrentInfo.MonthNames(Month - 1) & " " & Year).Length) / 2, Console.CursorTop, True, ColTypes.Neutral)
        Console.WriteLine()
        For CurrentDay As Integer = 1 To DateTo.Day
            Dim CurrentDate As New Date(Year, Month, CurrentDay, DateTimeFormatInfo.CurrentInfo.Calendar)
            If CurrentDate.DayOfWeek = DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek Then CurrentWeek += 1
            Dim CurrentWeekIndex As Integer = CurrentWeek - 1
            Dim CurrentDayMark As String = $" {CurrentDay} "
            Dim ReminderMarked As Boolean = False
            Dim EventMarked As Boolean = False

            'Know where and how to put the day number
            For Each Reminder As ReminderInfo In Reminders
                If Reminder.ReminderDate.Date = CurrentDate And Not ReminderMarked Then
                    CurrentDayMark = $"({CurrentDay})"
                    ReminderMarked = True
                End If
            Next
            For Each EventInstance As EventInfo In CalendarEvents
                If EventInstance.EventDate = CurrentDate And Not EventMarked Then
                    CurrentDayMark = New Color(StageColor).VTSequenceForeground + CurrentDayMark
                    EventMarked = True
                End If
            Next
            CalendarData(CurrentWeekIndex, CurrentDate.DayOfWeek) = CurrentDayMark
        Next
        WriteTable(CalendarDays, CalendarData, 2)
    End Sub

End Module
