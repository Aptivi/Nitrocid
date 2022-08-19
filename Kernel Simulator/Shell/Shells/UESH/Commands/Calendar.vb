
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
Imports KS.Misc.Calendar

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Manages your calendar
    ''' </summary>
    ''' <remarks>
    ''' This is a master application for the calendar that not only it shows you the calendar, but also shows and manages the events and reminders.
    ''' </remarks>
    Class CalendarCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Action As String = ListArgsOnly(0)

            'Enumerate based on action
            Dim ActionMinimumArguments As Integer = 1
            Dim ActionArguments() As String = ListArgsOnly.Skip(1).ToArray
            Select Case Action
                Case "show"
                    'User chose to show the calendar
                    If ActionArguments.Length <> 0 Then
                        Try
                            Dim StringYear As String = ActionArguments(0)
                            Dim StringMonth As String = Date.Today.Month
                            If ActionArguments.Length >= 2 Then StringMonth = ActionArguments(1)

                            'Show the calendar using the provided year and month
                            PrintCalendar(StringYear, StringMonth)
                        Catch ex As Exception
                            WStkTrc(ex)
                            Write(DoTranslation("Failed to add or remove an event.") + " {0}", True, ColTypes.Error, ex.Message)
                        End Try
                    Else
                        PrintCalendar()
                    End If
                Case "event"
                    'User chose to manipulate with the day events
                    If ActionArguments.Length >= ActionMinimumArguments Then
                        'User provided any of add, remove, and list. However, the first two arguments need minimum arguments of three parameters, so check.
                        Dim ActionType As String = ActionArguments(0)
                        Select Case ActionType
                            Case "add"
                                'Parse the arguments to check to see if enough arguments are passed to those parameters
                                ActionMinimumArguments = 3
                                If ActionArguments.Length >= ActionMinimumArguments Then
                                    'Enough arguments provided.
                                    Try
                                        Dim StringDate As String = ActionArguments(1)
                                        Dim EventTitle As String = ActionArguments(2)
                                        Dim ParsedDate As Date = Date.Parse(StringDate)
                                        AddEvent(ParsedDate, EventTitle)
                                    Catch ex As Exception
                                        WStkTrc(ex)
                                        Write(DoTranslation("Failed to add an event.") + " {0}", True, ColTypes.Error, ex.Message)
                                    End Try
                                Else
                                    Write(DoTranslation("Not enough arguments provided to add an event."), True, ColTypes.Error)
                                End If
                            Case "remove"
                                'Parse the arguments to check to see if enough arguments are passed to those parameters
                                ActionMinimumArguments = 2
                                If ActionArguments.Length >= ActionMinimumArguments Then
                                    'Enough arguments provided.
                                    Try
                                        Dim EventId As Integer = ActionArguments(1)
                                        Dim EventInstance As EventInfo = CalendarEvents(EventId - 1)
                                        RemoveEvent(EventInstance.EventDate, EventId)
                                    Catch ex As Exception
                                        WStkTrc(ex)
                                        Write(DoTranslation("Failed to remove an event.") + " {0}", True, ColTypes.Error, ex.Message)
                                    End Try
                                Else
                                    Write(DoTranslation("Not enough arguments provided to remove an event."), True, ColTypes.Error)
                                End If
                            Case "list"
                                'User chose to list. No parse needed as we're only listing.
                                ListEvents()
                            Case "saveall"
                                'User chose to save all.
                                SaveEvents()
                            Case Else
                                'Invalid action.
                                Write(DoTranslation("Invalid action."), True, ColTypes.Error)
                        End Select
                    Else
                        Write(DoTranslation("Not enough arguments provided for event manipulation."), True, ColTypes.Error)
                    End If
                Case "reminder"
                    'User chose to manipulate with the day reminders
                    If ActionArguments.Length >= ActionMinimumArguments Then
                        'User provided any of add, remove, and list. However, the first two arguments need minimum arguments of three parameters, so check.
                        Dim ActionType As String = ActionArguments(0)
                        Select Case ActionType
                            Case "add"
                                'Parse the arguments to check to see if enough arguments are passed to those parameters
                                ActionMinimumArguments = 3
                                If ActionArguments.Length >= ActionMinimumArguments Then
                                    'Enough arguments provided.
                                    Try
                                        Dim StringDate As String = ActionArguments(1)
                                        Dim ReminderTitle As String = ActionArguments(2)
                                        Dim ParsedDate As Date = Date.Parse(StringDate)
                                        AddReminder(ParsedDate, ReminderTitle)
                                    Catch ex As Exception
                                        WStkTrc(ex)
                                        Write(DoTranslation("Failed to add a reminder.") + " {0}", True, ColTypes.Error, ex.Message)
                                    End Try
                                Else
                                    Write(DoTranslation("Not enough arguments provided to add a reminder."), True, ColTypes.Error)
                                End If
                            Case "remove"
                                'Parse the arguments to check to see if enough arguments are passed to those parameters
                                ActionMinimumArguments = 2
                                If ActionArguments.Length >= ActionMinimumArguments Then
                                    'Enough arguments provided.
                                    Try
                                        Dim ReminderId As Integer = ActionArguments(1)
                                        Dim ReminderInstance As ReminderInfo = Reminders.Reminders(ReminderId - 1)
                                        RemoveReminder(ReminderInstance.ReminderDate, ReminderId)
                                    Catch ex As Exception
                                        WStkTrc(ex)
                                        Write(DoTranslation("Failed to remove a reminder.") + " {0}", True, ColTypes.Error, ex.Message)
                                    End Try
                                Else
                                    Write(DoTranslation("Not enough arguments provided to remove a reminder."), True, ColTypes.Error)
                                End If
                            Case "list"
                                'User chose to list. No parse needed as we're only listing.
                                ListReminders()
                            Case "saveall"
                                'User chose to save all.
                                SaveReminders()
                            Case Else
                                'Invalid action.
                                Write(DoTranslation("Invalid action."), True, ColTypes.Error)
                        End Select
                    Else
                        Write(DoTranslation("Not enough arguments provided for reminder manipulation."), True, ColTypes.Error)
                    End If
                Case Else
                    'Invalid action.
                    Write(DoTranslation("Invalid action."), True, ColTypes.Error)
            End Select
        End Sub

    End Class
End Namespace
