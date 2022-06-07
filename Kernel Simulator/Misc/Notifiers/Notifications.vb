
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

Imports System.Threading
Imports KS.Misc.Screensaver
Imports KS.Misc.Splash

Namespace Misc.Notifications
    Public Module Notifications

        'Variables
        Public NotifRecents As New List(Of Notification)
        Public NotifThread As New KernelThread("Notification Thread", False, AddressOf NotifListen)
        Public NotifyUpperLeftCornerChar As String = "╔"
        Public NotifyUpperRightCornerChar As String = "╗"
        Public NotifyLowerLeftCornerChar As String = "╚"
        Public NotifyLowerRightCornerChar As String = "╝"
        Public NotifyUpperFrameChar As String = "═"
        Public NotifyLowerFrameChar As String = "═"
        Public NotifyLeftFrameChar As String = "║"
        Public NotifyRightFrameChar As String = "║"

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
            ''' <summary>
            ''' Custom priority. Custom colors, beeps, etc.
            ''' </summary>
            Custom = 4
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
        ''' Listens for notifications and notifies the user if one has been found
        ''' </summary>
        Private Sub NotifListen()
            Dim OldNotificationsList As New List(Of Notification)(NotifRecents)
            Dim NewNotificationsList As List(Of Notification)
            While Not KernelShutdown
                Thread.Sleep(100)
                NewNotificationsList = NotifRecents.Except(OldNotificationsList).ToList
                If NewNotificationsList.Count > 0 And Not InSaver Then
                    'Update the old notifications list
                    Wdbg(DebugLevel.W, "Notifications received! Recents count was {0}, Old count was {1}", NotifRecents.Count, OldNotificationsList.Count)
                    OldNotificationsList = New List(Of Notification)(NotifRecents)
                    KernelEventManager.RaiseNotificationsReceived(NewNotificationsList)

                    'Iterate through new notifications. If we're on the booting stage, ensure that the notifications are only queued until the
                    'kernel has finished booting.
                    While Not KernelBooted
                        Thread.Sleep(100)
                    End While
                    For Each NewNotification As Notification In NewNotificationsList
                        KernelEventManager.RaiseNotificationReceived(NewNotification)

                        'Populate title and description
                        Dim Title, Desc As String
                        Wdbg(DebugLevel.I, "Title: {0}", NewNotification.Title)
                        Wdbg(DebugLevel.I, "Desc: {0}", NewNotification.Desc)
                        Title = NewNotification.Title.Truncate(36)
                        Desc = NewNotification.Desc.Truncate(36)
                        Wdbg(DebugLevel.I, "Truncated title: {0}", Title)
                        Wdbg(DebugLevel.I, "Truncated desc: {0}", Desc)
                        Wdbg(DebugLevel.I, "Truncated title length: {0}", Title.Length)
                        Wdbg(DebugLevel.I, "Truncated desc length: {0}", Desc.Length)

                        'Set the border color
                        Wdbg(DebugLevel.I, "Priority: {0}", NewNotification.Priority)
                        Dim NotifyBorderColor As Color = LowPriorityBorderColor
                        Dim NotifyTitleColor As Color = NotificationTitleColor
                        Dim NotifyDescColor As Color = NotificationDescriptionColor
                        Dim NotifyProgressColor As Color = NotificationProgressColor
                        Dim NotifyProgressFailureColor As Color = NotificationFailureColor
                        Select Case NewNotification.Priority
                            Case NotifPriority.Medium
                                NotifyBorderColor = MediumPriorityBorderColor
                            Case NotifPriority.High
                                NotifyBorderColor = HighPriorityBorderColor
                            Case NotifPriority.Custom
                                NotifyBorderColor = NewNotification.CustomColor
                                NotifyTitleColor = NewNotification.CustomTitleColor
                                NotifyDescColor = NewNotification.CustomDescriptionColor
                                NotifyProgressColor = NewNotification.CustomProgressColor
                                NotifyProgressFailureColor = NewNotification.CustomProgressFailureColor
                        End Select
                        If NewNotification.NotificationBorderColor IsNot Nothing Then
                            NotifyBorderColor = NewNotification.NotificationBorderColor
                        End If

                        'Write notification to console
                        Wdbg(DebugLevel.I, "Where to store: ({0}, {1}), Title top: {2}, Desc top: {3}", Console.WindowWidth - 40, Console.WindowTop, Console.WindowTop + 1, Console.WindowTop + 2)
                        WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop, True, ColTypes.Neutral)
                        WriteWhere(Title + GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, True, NotifyTitleColor)
                        WriteWhere(Desc + GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 2, True, NotifyDescColor)
                        WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 3, True, ColTypes.Neutral)

                        'Optionally, draw a border
                        If DrawBorderNotification Then
                            'Prepare the variables
                            Dim CurrentNotifyUpperLeftCornerChar As String = NotifyUpperLeftCornerChar
                            Dim CurrentNotifyUpperRightCornerChar As String = NotifyUpperRightCornerChar
                            Dim CurrentNotifyLowerLeftCornerChar As String = NotifyLowerLeftCornerChar
                            Dim CurrentNotifyLowerRightCornerChar As String = NotifyLowerRightCornerChar
                            Dim CurrentNotifyUpperFrameChar As String = NotifyUpperFrameChar
                            Dim CurrentNotifyLowerFrameChar As String = NotifyLowerFrameChar
                            Dim CurrentNotifyLeftFrameChar As String = NotifyLeftFrameChar
                            Dim CurrentNotifyRightFrameChar As String = NotifyRightFrameChar

                            'Get custom corner characters
                            If NewNotification.Priority = NotifPriority.Custom Then
                                CurrentNotifyUpperLeftCornerChar = NewNotification.CustomUpperLeftCornerChar
                                CurrentNotifyUpperRightCornerChar = NewNotification.CustomUpperRightCornerChar
                                CurrentNotifyLowerLeftCornerChar = NewNotification.CustomLowerLeftCornerChar
                                CurrentNotifyLowerRightCornerChar = NewNotification.CustomLowerRightCornerChar
                                CurrentNotifyUpperFrameChar = NewNotification.CustomUpperFrameChar
                                CurrentNotifyLowerFrameChar = NewNotification.CustomLowerFrameChar
                                CurrentNotifyLeftFrameChar = NewNotification.CustomLeftFrameChar
                                CurrentNotifyRightFrameChar = NewNotification.CustomRightFrameChar
                            End If

                            'Just draw the border!
                            WriteWhere(CurrentNotifyUpperLeftCornerChar + CurrentNotifyUpperFrameChar.Repeat(38) + CurrentNotifyUpperRightCornerChar, Console.WindowWidth - 41, Console.WindowTop, True, NotifyBorderColor)
                            WriteWhere(CurrentNotifyLeftFrameChar, Console.WindowWidth - 41, Console.WindowTop + 1, True, NotifyBorderColor)
                            WriteWhere(CurrentNotifyLeftFrameChar, Console.WindowWidth - 41, Console.WindowTop + 2, True, NotifyBorderColor)
                            WriteWhere(CurrentNotifyLeftFrameChar, Console.WindowWidth - 41, Console.WindowTop + 3, True, NotifyBorderColor)
                            WriteWhere(CurrentNotifyRightFrameChar, Console.WindowWidth - 2, Console.WindowTop + 1, True, NotifyBorderColor)
                            WriteWhere(CurrentNotifyRightFrameChar, Console.WindowWidth - 2, Console.WindowTop + 2, True, NotifyBorderColor)
                            WriteWhere(CurrentNotifyRightFrameChar, Console.WindowWidth - 2, Console.WindowTop + 3, True, NotifyBorderColor)
                            WriteWhere(CurrentNotifyLowerLeftCornerChar + CurrentNotifyLowerFrameChar.Repeat(38) + CurrentNotifyLowerRightCornerChar, Console.WindowWidth - 41, Console.WindowTop + 4, True, NotifyBorderColor)
                        End If

                        'Beep according to priority
                        Dim BeepTimes As Integer = NewNotification.Priority
                        If NewNotification.Priority = NotifPriority.Custom Then BeepTimes = NewNotification.CustomBeepTimes
                        For i As Integer = 1 To BeepTimes
                            Console.Beep()
                        Next

                        'Show progress
                        If NewNotification.Type = NotifType.Progress Then
                            Do Until NewNotification.Progress >= 100 Or NewNotification.ProgressFailed
                                Dim ProgressTitle As String = Title + " (" + CStr(NewNotification.Progress) + "%)"
                                Wdbg(DebugLevel.I, "Where to store progress: {0},{1}", Console.WindowWidth - 40, Console.WindowTop + 3)
                                Wdbg(DebugLevel.I, "Progress: {0}", NewNotification.Progress)
                                WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop, True, ColTypes.Neutral)
                                WriteWhere(ProgressTitle + GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, True, NotifyTitleColor, NewNotification.Progress)
                                WriteWhere(Desc + GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 2, True, NotifyDescColor)
                                WriteWhere("*".Repeat(NewNotification.Progress * 100 / 100 * (38 / 100)), Console.WindowWidth - 40, Console.WindowTop + 3, True, NotifyProgressColor)
                                Thread.Sleep(1)
                                If NewNotification.ProgressFailed Then WriteWhere(ProgressTitle + GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, True, NotifyProgressFailureColor, NewNotification.Progress)
                            Loop
                        End If

                        'Clear the area
                        Dim TopTitleClear As Integer = Console.WindowTop + 1
                        Dim TopDescClear As Integer = Console.WindowTop + 2
                        Dim TopProgClear As Integer = Console.WindowTop + 3
                        Dim TopOpenBorderClear As Integer = Console.WindowTop
                        Dim TopCloseBorderClear As Integer = Console.WindowTop + 4
                        Thread.Sleep(5000)
                        NotifClearArea(Console.WindowWidth - If(DrawBorderNotification, 41, 40), TopTitleClear, TopDescClear, TopProgClear, TopOpenBorderClear, TopCloseBorderClear)
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
        Private Sub NotifClearArea(Width As Integer, TopTitle As Integer, TopDesc As Integer, TopProg As Integer, TopOpenBorder As Integer, TopCloseBorder As Integer)
            If DrawBorderNotification Then
                WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 41, Console.WindowTop, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Width, TopOpenBorder, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 41, Console.WindowTop + 1, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Width, TopTitle, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 41, Console.WindowTop + 2, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Width, TopDesc, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 41, Console.WindowTop + 3, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Width, TopProg, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 41, Console.WindowTop + 4, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Width, TopCloseBorder, True, ColTypes.Neutral)
            Else
                WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 1, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Width, TopTitle, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 2, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Width, TopDesc, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Console.WindowWidth - 40, Console.WindowTop + 3, True, ColTypes.Neutral)
                WriteWhere(GetEsc() + "[0K", Width, TopProg, True, ColTypes.Neutral)
            End If
        End Sub

        ''' <summary>
        ''' Sends notification
        ''' </summary>
        ''' <param name="notif">Instance of notification holder</param>
        Public Sub NotifySend(notif As Notification)
            Wdbg(DebugLevel.I, "List contains this notification? {0}", NotifRecents.Contains(notif))
            If Not NotifRecents.Contains(notif) Then
                NotifRecents.Add(notif)
                KernelEventManager.RaiseNotificationSent(notif)
            End If
        End Sub

        ''' <summary>
        ''' Sends notifications
        ''' </summary>
        ''' <param name="notifs">Instances of notification holder</param>
        Public Sub NotifySendRange(notifs As List(Of Notification))
            For Each notif As Notification In notifs
                NotifySend(notif)
            Next
            KernelEventManager.RaiseNotificationsSent(notifs)
        End Sub

        ''' <summary>
        ''' Dismisses notification
        ''' </summary>
        ''' <param name="ind">Index of notification</param>
        Public Function NotifDismiss(ind As Integer) As Boolean
            Try
                NotifRecents.RemoveAt(ind)
                Wdbg(DebugLevel.I, "Removed index {0} from notification list", ind)
                KernelEventManager.RaiseNotificationDismissed()
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Error trying to dismiss notification: {0}", ex.Message)
                WStkTrc(ex)
            End Try
            Return False
        End Function

    End Module
End Namespace