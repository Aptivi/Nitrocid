
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

Class CalendarCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
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
                        W(DoTranslation("Failed to add or remove an event.") + " {0}", True, ColTypes.Error, ex.Message)
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
                        Case "add", "remove"
                            'Parse the arguments to check to see if enough arguments are passed to those parameters
                            ActionMinimumArguments = 3
                            If ActionArguments.Length >= ActionMinimumArguments Then
                                'Enough arguments provided.
                                Try
                                    Dim StringDate As String = ActionArguments(1)
                                    Dim EventTitle As String = ActionArguments(2)
                                    Dim ParsedDate As Date = Date.Parse(StringDate)

                                    'Choose whether to add or remove
                                    Select Case ActionType
                                        Case "add"
                                            AddEvent(ParsedDate, EventTitle)
                                        Case "remove"
                                            Dim EventId As Integer = EventTitle
                                            RemoveEvent(ParsedDate, EventId)
                                    End Select
                                Catch ex As Exception
                                    WStkTrc(ex)
                                    W(DoTranslation("Failed to add or remove an event.") + " {0}", True, ColTypes.Error, ex.Message)
                                End Try
                            Else
                                W(DoTranslation("Not enough arguments provided to add or remove an event."), True, ColTypes.Error)
                            End If
                        Case "list"
                            'User chose to list. No parse needed as we're only listing.
                            ListEvents()
                        Case "saveall"
                            'User chose to save all.
                            SaveEvents()
                        Case Else
                            'Invalid action.
                            W(DoTranslation("Invalid action."), True, ColTypes.Error)
                    End Select
                Else
                    W(DoTranslation("Not enough arguments provided for event manipulation."), True, ColTypes.Error)
                End If
            Case "reminder"
                'User chose to manipulate with the day reminders
                If ActionArguments.Length >= ActionMinimumArguments Then
                    'User provided any of add, remove, and list. However, the first two arguments need minimum arguments of three parameters, so check.
                    Dim ActionType As String = ActionArguments(0)
                    Select Case ActionType
                        Case "add", "remove"
                            'Parse the arguments to check to see if enough arguments are passed to those parameters
                            ActionMinimumArguments = 3
                            If ActionArguments.Length >= ActionMinimumArguments Then
                                'Enough arguments provided.
                                Try
                                    Dim StringDate As String = ActionArguments(1)
                                    Dim ReminderTitle As String = ActionArguments(2)
                                    Dim ParsedDate As Date = Date.Parse(StringDate)

                                    'Choose whether to add or remove
                                    Select Case ActionType
                                        Case "add"
                                            AddReminder(ParsedDate, ReminderTitle)
                                        Case "remove"
                                            Dim ReminderId As Integer = ReminderTitle
                                            RemoveReminder(ParsedDate, ReminderId)
                                    End Select
                                Catch ex As Exception
                                    WStkTrc(ex)
                                    W(DoTranslation("Failed to add or remove a reminder.") + " {0}", True, ColTypes.Error, ex.Message)
                                End Try
                            Else
                                W(DoTranslation("Not enough arguments provided to add or remove a reminder."), True, ColTypes.Error)
                            End If
                        Case "list"
                            'User chose to list. No parse needed as we're only listing.
                            ListReminders()
                        Case "saveall"
                            'User chose to save all.
                            SaveReminders()
                        Case Else
                            'Invalid action.
                            W(DoTranslation("Invalid action."), True, ColTypes.Error)
                    End Select
                Else
                    W(DoTranslation("Not enough arguments provided for reminder manipulation."), True, ColTypes.Error)
                End If
            Case Else
                'Invalid action.
                W(DoTranslation("Invalid action."), True, ColTypes.Error)
        End Select
    End Sub

End Class