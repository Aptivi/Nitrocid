
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

Imports System.Threading

Public Module Notifications

    'Variables
    Public NotifRecents As New List(Of Notification)
    Public NotifThread As New Thread(AddressOf NotifListen) With {.Name = "Notification Thread"}

    ''' <summary>
    ''' Notification priority
    ''' </summary>
    Public Enum NotifPriority
        ''' <summary>
        ''' Low priority. One beep.
        ''' </summary>
        Low = 1
        ''' <summary>
        ''' Medium priority. Two beeps.
        ''' </summary>
        Medium = 2
        ''' <summary>
        ''' High priority. Three beeps.
        ''' </summary>
        High = 3
    End Enum

    ''' <summary>
    ''' Notification type
    ''' </summary>
    Public Enum NotifType
        ''' <summary>
        ''' Normal notification.
        ''' </summary>
        Normal = 1
        ''' <summary>
        ''' A notification with the progress bar. Use if you're going to notify the user while your mod is doing something.
        ''' </summary>
        Progress = 2
    End Enum

    ''' <summary>
    ''' Notification holder with title, description, and priority
    ''' </summary>
    Public Class Notification
        Private _Progress As Integer
        ''' <summary>
        ''' Notification title
        ''' </summary>
        Property Title As String
        ''' <summary>
        ''' Notification description
        ''' </summary>
        Property Desc As String
        ''' <summary>
        ''' Notification priority
        ''' </summary>
        Property Priority As NotifPriority
        ''' <summary>
        ''' Notification type
        ''' </summary>
        Property Type As NotifType
        ''' <summary>
        ''' Notification progress
        ''' </summary>
        Property Progress As Integer
            Get
                Return _Progress
            End Get
            Set
                If Value >= 100 Then
                    _Progress = 100
                ElseIf Value <= 0 Then
                    _Progress = 0
                Else
                    _Progress = Value
                End If
            End Set
        End Property
        ''' <summary>
        ''' Whether the progress failed
        ''' </summary>
        ''' <returns></returns>
        Property ProgressFailed As Boolean
    End Class

    ''' <summary>
    ''' Listens for notifications and notifies the user if one has been found
    ''' </summary>
    Private Sub NotifListen()
        Dim OldNotificationsList As New List(Of Notification)(NotifRecents)
        Dim NewNotificationsList As List(Of Notification)
        While Not NotifThread.ThreadState = ThreadState.AbortRequested
            Thread.Sleep(100)
            NewNotificationsList = NotifRecents.Except(OldNotificationsList).ToList
            If NewNotificationsList.Count > 0 And Not InSaver Then
                'Update the old notifications list
                Wdbg("W", "Notifications received! Recents count was {0}, Old count was {1}", NotifRecents.Count, OldNotificationsList.Count)
                OldNotificationsList = New List(Of Notification)(NotifRecents)
                EventManager.RaiseNotificationsReceived(NewNotificationsList)

                'Iterate through new notifications
                For Each NewNotification As Notification In NewNotificationsList
                    EventManager.RaiseNotificationReceived(NewNotification)

                    'Populate title and description
                    Dim Title, Desc As String
                    Wdbg("I", "Title: {0}", NewNotification.Title)
                    Wdbg("I", "Desc: {0}", NewNotification.Desc)
                    Title = NewNotification.Title.Truncate(36)
                    Desc = NewNotification.Desc.Truncate(36)
                    Wdbg("I", "Truncated title: {0}", Title)
                    Wdbg("I", "Truncated desc: {0}", Desc)
                    Wdbg("I", "Truncated title length: {0}", Title.Length)
                    Wdbg("I", "Truncated desc length: {0}", Desc.Length)

                    'Write notification to console
                    Wdbg("I", "Where to store: ({0}, {1}), Title top: {2}, Desc top: {3}", Console.WindowWidth - 40, Console.WindowTop, Console.WindowTop + 1, Console.WindowTop + 2)
                    WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop, True, ColTypes.Neutral)
                    WriteWhere(Title + GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, True, ColTypes.Neutral)
                    WriteWhere(Desc + GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 2, True, ColTypes.Neutral)

                    'Beep according to priority
                    Wdbg("I", "Priority: {0}", NewNotification.Priority)
                    For i As Integer = 1 To NewNotification.Priority
                        Console.Beep()
                    Next

                    'Show progress
                    If NewNotification.Type = NotifType.Progress Then
                        Do Until NewNotification.Progress >= 100 Or NewNotification.ProgressFailed
                            Dim ProgressTitle As String = Title + " (" + CStr(NewNotification.Progress) + "%)"
                            Wdbg("I", "Where to store progress: {0},{1}", Console.WindowWidth - 40, Console.WindowTop + 3)
                            Wdbg("I", "Progress: {0}", NewNotification.Progress)
                            WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop, True, ColTypes.Neutral)
                            WriteWhere(ProgressTitle + GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, True, ColTypes.Neutral, NewNotification.Progress)
                            WriteWhere(Desc + GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 2, True, ColTypes.Neutral)
                            WriteWhere("*".Repeat(NewNotification.Progress * 100 / 100 * (38 / 100)), Console.WindowWidth - 40, Console.WindowTop + 3, True, ColTypes.Neutral)
                            Thread.Sleep(1)
                        Loop
                    End If

                    'Clear the area
                    Thread.Sleep(5000)
                    NotifClearArea(Console.WindowWidth - 40, Console.WindowTop + 1, Console.WindowTop + 2, Console.WindowTop + 3)
                Next
            End If
        End While
    End Sub

    ''' <summary>
    ''' Clears the area of the displayed notification
    ''' </summary>
    ''' <param name="Width">Console width</param>
    ''' <param name="TopTitle">Vertical location of title</param>
    ''' <param name="TopDesc">Vertical location of description</param>
    Private Sub NotifClearArea(ByVal Width As Integer, ByVal TopTitle As Integer, ByVal TopDesc As Integer, ByVal TopProg As Integer)
        WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, True, ColTypes.Neutral)
        WriteWhere(GetEsc() + "[0K", Width, TopTitle, True, ColTypes.Neutral)
        WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 2, True, ColTypes.Neutral)
        WriteWhere(GetEsc() + "[0K", Width, TopDesc, True, ColTypes.Neutral)
        WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 3, True, ColTypes.Neutral)
        WriteWhere(GetEsc() + "[0K", Width, TopProg, True, ColTypes.Neutral)
    End Sub

    ''' <summary>
    ''' Creates a notification
    ''' </summary>
    ''' <param name="Title">Title of notification</param>
    ''' <param name="Desc">Description of notification</param>
    ''' <param name="Priority">Priority of notification</param>
    ''' <returns></returns>
    Public Function NotifyCreate(ByVal Title As String, ByVal Desc As String, ByVal Priority As NotifPriority, ByVal Type As NotifType) As Notification
        Return New Notification With {.Title = Title, .Desc = Desc, .Priority = Priority, .Type = Type, .Progress = 0}
    End Function

    ''' <summary>
    ''' Sends notification
    ''' </summary>
    ''' <param name="notif">Instance of notification holder</param>
    Public Sub NotifySend(ByVal notif As Notification)
        Wdbg("I", "List contains this notification? {0}", NotifRecents.Contains(notif))
        If Not NotifRecents.Contains(notif) Then
            NotifRecents.Add(notif)
            EventManager.RaiseNotificationSent(notif)
        End If
    End Sub

    ''' <summary>
    ''' Sends notifications
    ''' </summary>
    ''' <param name="notifs">Instances of notification holder</param>
    Public Sub NotifySendRange(ByVal notifs As List(Of Notification))
        For Each notif As Notification In notifs
            NotifySend(notif)
        Next
        EventManager.RaiseNotificationsSent(notifs)
    End Sub

    ''' <summary>
    ''' Dismisses notification
    ''' </summary>
    ''' <param name="ind">Index of notification</param>
    Public Function NotifDismiss(ByVal ind As Integer) As Boolean
        Try
            NotifRecents.RemoveAt(ind)
            Wdbg("I", "Removed index {0} from notification list", ind)
            EventManager.RaiseNotificationDismissed()
            Return True
        Catch ex As Exception
            Wdbg("E", "Error trying to dismiss notification: {0}", ex.Message)
            WStkTrc(ex)
        End Try
        Return False
    End Function

End Module
