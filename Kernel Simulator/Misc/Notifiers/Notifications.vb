
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

    'TODO: Implement showing notification on receive (corner). The position should not overlap the time/date corner.
    'Variables
    Public NotifRecents As New List(Of Notification)
    Public NotifThread As New Thread(AddressOf NotifListen)

    'Priority enumeration
    Public Enum NotifPriority
        Low = 1
        Medium = 2
        High = 3
    End Enum

    'Notification holder class
    Public Class Notification
        Property Title As String
        Property Desc As String
        Property Priority As NotifPriority
    End Class

    'Subs
    Private Sub NotifListen()
        Dim OldNCount As Integer = NotifRecents.Count
        While Not NotifThread.ThreadState = ThreadState.AbortRequested
            If NotifRecents.Count <> OldNCount Then
                OldNCount = NotifRecents.Count
                For i As Integer = 1 To NotifRecents(OldNCount - 1).Priority
                    Console.Beep()
                    Thread.Sleep(100)
                Next
            End If
        End While
    End Sub
    Public Sub NotifySend(ByVal notif As Notification)
        If Not NotifRecents.Contains(notif) Then NotifRecents.Add(notif)
    End Sub
    Public Sub NotifDismiss(ByVal ind As Integer)
        Try
            NotifRecents.RemoveAt(ind)
        Catch ex As Exception
            W(DoTranslation("Error trying to dismiss notification: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

End Module
