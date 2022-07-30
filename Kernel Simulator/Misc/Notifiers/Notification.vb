
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

Namespace Misc.Notifications
    ''' <summary>
    ''' Notification holder with title, description, and priority
    ''' </summary>
    Public Class Notification

        Private _Progress As Integer
        Private _CustomBeepTimes As Integer = 1
        Private _NotificationBorderColor As Color

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
        ''' Whether the progress failed
        ''' </summary>
        Property ProgressFailed As Boolean

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

        '--> For Custom Priority Notifications

        ''' <summary>
        ''' Beep times (for custom priority notfications)
        ''' </summary>
        Property CustomBeepTimes As Integer
            Get
                Return _CustomBeepTimes
            End Get
            Set
                If Value <= 0 Then
                    _CustomBeepTimes = 0
                Else
                    _CustomBeepTimes = Value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Custom color (for custom priority notfications)
        ''' </summary>
        Property CustomColor As Color = LowPriorityBorderColor

        ''' <summary>
        ''' Custom title color (for custom priority notfications)
        ''' </summary>
        Property CustomTitleColor As Color = NotificationTitleColor

        ''' <summary>
        ''' Custom description color (for custom priority notfications)
        ''' </summary>
        Property CustomDescriptionColor As Color = NotificationDescriptionColor

        ''' <summary>
        ''' Custom progress color (for custom priority notfications)
        ''' </summary>
        Property CustomProgressColor As Color = NotificationProgressColor

        ''' <summary>
        ''' Custom progress failure color (for custom priority notfications)
        ''' </summary>
        Property CustomProgressFailureColor As Color = NotificationFailureColor

        ''' <summary>
        ''' Upper left corner character for custom priority notification
        ''' </summary>
        Property CustomUpperLeftCornerChar As String = "╔"

        ''' <summary>
        ''' Upper right corner character for custom priority notification
        ''' </summary>
        Property CustomUpperRightCornerChar As String = "╗"

        ''' <summary>
        ''' Lower left corner character for custom priority notification
        ''' </summary>
        Property CustomLowerLeftCornerChar As String = "╚"

        ''' <summary>
        ''' Lower right corner character for custom priority notification
        ''' </summary>
        Property CustomLowerRightCornerChar As String = "╝"

        ''' <summary>
        ''' Upper frame character for custom priority notification
        ''' </summary>
        Property CustomUpperFrameChar As String = "═"

        ''' <summary>
        ''' Lower frame character for custom priority notification
        ''' </summary>
        Property CustomLowerFrameChar As String = "═"

        ''' <summary>
        ''' Left frame character for custom priority notification
        ''' </summary>
        Property CustomLeftFrameChar As String = "║"

        ''' <summary>
        ''' Right frame character for custom priority notification
        ''' </summary>
        Property CustomRightFrameChar As String = "║"

        ''' <summary>
        ''' Whether the progress has been compeleted successfully or with failure
        ''' </summary>
        ReadOnly Property ProgressCompleted As Boolean
            Get
                Return _Progress >= 100 Or ProgressFailed
            End Get
        End Property

        ''' <summary>
        ''' The notification border color. Must be null for custom priority notifications.
        ''' </summary>
        Property NotificationBorderColor As Color
            Get
                Return _NotificationBorderColor
            End Get
            Set
                If Not Priority = NotifPriority.Custom Then
                    _NotificationBorderColor = Value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Creates a new notification
        ''' </summary>
        ''' <param name="Title">Title of notification</param>
        ''' <param name="Desc">Description of notification</param>
        ''' <param name="Priority">Priority of notification</param>
        ''' <param name="Type">Notification type</param>
        Public Sub New(Title As String, Desc As String, Priority As NotifPriority, Type As NotifType)
            Me.Title = Title
            Me.Desc = Desc
            Me.Priority = Priority
            Me.Type = Type
        End Sub

    End Class
End Namespace