
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
    Public NotifThread As New Thread(AddressOf NotifListen)

    ''' <summary>
    ''' Notification priority
    ''' </summary>
    Public Enum NotifPriority
        Low = 1
        Medium = 2
        High = 3
    End Enum

    Public Enum NotifType
        Normal = 1
        Progress = 2
    End Enum

    ''' <summary>
    ''' Notification holder with title, description, and priority
    ''' </summary>
    Public Class Notification
        Private _Progress As Integer
        Property Title As String
        Property Desc As String
        Property Priority As NotifPriority
        Property Type As NotifType
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
        Property ProgressFailed As Boolean
    End Class

    ''' <summary>
    ''' Listens for notifications and notifies the user if one has been found
    ''' </summary>
    Private Sub NotifListen()
        Dim OldNCount As Integer = NotifRecents.Count
        While Not NotifThread.ThreadState = ThreadState.AbortRequested
            Thread.Sleep(100)
            If NotifRecents.Count > OldNCount And Not InSaver Then
                Wdbg("W", "Notification received! Recents count was {0}, Old count was {1}", NotifRecents.Count, OldNCount)
                Dim Title, Desc As String
                Wdbg("I", "Title: {0}", NotifRecents(NotifRecents.Count - 1).Title)
                Wdbg("I", "Desc: {0}", NotifRecents(NotifRecents.Count - 1).Desc)
                Title = NotifRecents(NotifRecents.Count - 1).Title.Truncate(36)
                Desc = NotifRecents(NotifRecents.Count - 1).Desc.Truncate(36)
                Wdbg("I", "Truncated title: {0}", Title)
                Wdbg("I", "Truncated desc: {0}", Desc)
                Wdbg("I", "Where to store: {0}, Title: {1}, Desc: {2}", Console.WindowWidth - 40, Console.WindowTop + 1, Console.WindowTop + 2)
                WriteWhere(" ".Repeat(40), Console.WindowWidth - 40, Console.WindowTop, ColTypes.Neutral)
                WriteWhere(Title + " ".Repeat(40 - Title.Length), Console.WindowWidth - 40, Console.WindowTop + 1, ColTypes.Neutral)
                WriteWhere(Desc + " ".Repeat(40 - Desc.Length), Console.WindowWidth - 40, Console.WindowTop + 2, ColTypes.Neutral)
                Wdbg("I", "Priority: {0}", NotifRecents(NotifRecents.Count - 1).Priority)
                For i As Integer = 1 To NotifRecents(NotifRecents.Count - 1).Priority
                    Console.Beep()
                Next
                Wdbg("I", "Truncated title length: {0}", Title.Length)
                Wdbg("I", "Truncated desc length: {0}", Desc.Length)
                If NotifRecents(NotifRecents.Count - 1).Type = NotifType.Progress Then
                    Do Until NotifRecents(NotifRecents.Count - 1).Progress >= 100 Or NotifRecents(NotifRecents.Count - 1).ProgressFailed
                        Wdbg("I", "Where to store progress: {0}:{1}", Console.WindowWidth - 40, Console.WindowTop + 3)
                        Wdbg("I", "Progress: {0}", NotifRecents(NotifRecents.Count - 1).Progress)
                        WriteWhere(" ".Repeat(40), Console.WindowWidth - 40, Console.WindowTop, ColTypes.Neutral)
                        WriteWhere(Title + " ".Repeat(40 - Title.Length), Console.WindowWidth - 40, Console.WindowTop + 1, ColTypes.Neutral)
                        WriteWhere(Desc + " ".Repeat(40 - Desc.Length), Console.WindowWidth - 40, Console.WindowTop + 2, ColTypes.Neutral)
                        WriteWhere("{0}%", Console.WindowWidth - 40, Console.WindowTop + 3, ColTypes.Neutral, NotifRecents(NotifRecents.Count - 1).Progress)
                        Thread.Sleep(1)
                    Loop
                End If
                NotifClearArea(Title.Length, Desc.Length, Console.WindowWidth - 40, Console.WindowTop + 1, Console.WindowTop + 2)
            End If
            OldNCount = NotifRecents.Count
        End While
    End Sub

    ''' <summary>
    ''' Clears the area of the displayed notification
    ''' </summary>
    ''' <param name="LenTitle">String length of the title</param>
    ''' <param name="LenDesc">String length of the description</param>
    ''' <param name="Width">Console width</param>
    ''' <param name="TopTitle">Vertical location of title</param>
    ''' <param name="TopDesc">Vertical location of description</param>
    Private Sub NotifClearArea(ByVal LenTitle As Integer, ByVal LenDesc As Integer, ByVal Width As Integer, ByVal TopTitle As Integer, ByVal TopDesc As Integer)
        Thread.Sleep(5000)
        For i As Integer = 0 To LenTitle - 1
            WriteWhere(" ", Console.WindowWidth - 40 + i, Console.WindowTop + 1, ColTypes.Neutral)
            WriteWhere(" ", Width + i, TopTitle, ColTypes.Neutral)
        Next
        For i As Integer = 0 To LenDesc - 1
            WriteWhere(" ", Console.WindowWidth - 40 + i, Console.WindowTop + 2, ColTypes.Neutral)
            WriteWhere(" ", Width + i, TopDesc, ColTypes.Neutral)
        Next
        WriteWhere("     ", Console.WindowWidth - 40, Console.WindowTop + 3, ColTypes.Neutral)
        WriteWhere("     ", Width, Console.WindowTop + 3, ColTypes.Neutral)
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
        If Not NotifRecents.Contains(notif) Then NotifRecents.Add(notif)
    End Sub

    ''' <summary>
    ''' Dismisses notification
    ''' </summary>
    ''' <param name="ind">Index of notification</param>
    Public Function NotifDismiss(ByVal ind As Integer) As Boolean
        Try
            NotifRecents.RemoveAt(ind)
            Wdbg("I", "Removed index {0} from notification list", ind)
            Return True
        Catch ex As Exception
            Wdbg("E", "Error trying to dismiss notification: {0}", ex.Message)
            WStkTrc(ex)
        End Try
        Return False
    End Function

End Module
