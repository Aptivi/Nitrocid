
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

Imports System.Xml.Serialization
Imports KS.Misc.Notifications

Namespace Misc.Calendar.Events
    <XmlRoot("ReminderInfo", IsNullable:=False)>
    Public Class EventInfo

        Private EventNotified As Boolean
        ''' <summary>
        ''' Event date
        ''' </summary>
        Public ReadOnly Property EventDate As Date
        ''' <summary>
        ''' Event title
        ''' </summary>
        Public ReadOnly Property EventTitle As String

        Public Sub New()
        End Sub

        Public Sub New(EventDate As Date, EventTitle As String)
            Me.EventDate = EventDate
            Me.EventTitle = EventTitle
        End Sub

        ''' <summary>
        ''' Notifies the user about the event
        ''' </summary>
        Protected Friend Sub NotifyEvent()
            If Not EventNotified Then
                Dim EventNotification As New Notification(EventTitle, DoTranslation("Now it's an event day!"), NotifPriority.Medium, NotifType.Normal)
                NotifySend(EventNotification)
                EventNotified = True
            End If
        End Sub

    End Class
End Namespace