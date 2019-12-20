
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

    'TODO: Notification strings should be wrapped with the "..." in the temporary notification display (corner)
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
                Dim Title, Desc As String
                Title = NotifRecents(OldNCount - 1).Title.Truncate(27)
                Desc = NotifRecents(OldNCount - 1).Desc.Truncate(27)
                WriteWhere(Title, Console.WindowWidth - 30, Console.WindowTop + 2, ColTypes.Neutral)
                WriteWhere(Desc, Console.WindowWidth - 30, Console.WindowTop + 3, ColTypes.Neutral)
                For i As Integer = 1 To NotifRecents(OldNCount - 1).Priority
                    Console.Beep()
                Next
                NotifClearArea(Title.Length, Desc.Length, Console.WindowWidth - 30, Console.WindowTop + 2, Console.WindowTop + 3)
            End If
        End While
    End Sub
    Private Sub NotifClearArea(ByVal LenTitle As Integer, ByVal LenDesc As Integer, ByVal Width As Integer, ByVal TopTitle As Integer, ByVal TopDesc As Integer)
        Thread.Sleep(5000)
        For i As Integer = 0 To LenTitle - 1
            WriteWhere(" ", Console.WindowWidth - 30 + i, Console.WindowTop + 2, ColTypes.Neutral)
            WriteWhere(" ", Width + i, TopTitle, ColTypes.Neutral)
        Next
        For i As Integer = 0 To LenDesc - 1
            WriteWhere(" ", Console.WindowWidth - 30 + i, Console.WindowTop + 3, ColTypes.Neutral)
            WriteWhere(" ", Width + i, TopDesc, ColTypes.Neutral)
        Next
    End Sub
    Public Sub NotifySend(ByVal notif As Notification)
        If Not NotifRecents.Contains(notif) Then NotifRecents.Add(notif)
    End Sub
    Public Sub NotifDismiss(ByVal ind As Integer)
        Try
            NotifRecents.RemoveAt(ind)
            W(DoTranslation("Notification dismissed successfully.", currentLang), True, ColTypes.Neutral)
        Catch ex As Exception
            W(DoTranslation("Error trying to dismiss notification: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

End Module
