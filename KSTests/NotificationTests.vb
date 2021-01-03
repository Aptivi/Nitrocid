
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

Imports KS

<TestClass()> Public Class NotificationTests

    ''' <summary>
    ''' Tests notification creation
    ''' </summary>
    <TestMethod()> Public Sub TestNotifyCreate()
        Assert.IsNotNull(NotifyCreate("This is the title.", "This is the description.", NotifPriority.Medium), "Notification creation failed. Got null.")
    End Sub

    ''' <summary>
    ''' Tests notifications sending
    ''' </summary>
    <TestMethod> Public Sub TestNotifySend()
        Dim Notif As Notification = NotifyCreate("Notification title", "This is a high priority notification", NotifPriority.High)
        NotifySend(Notif)
        Assert.IsTrue(NotifRecents.Count > 0, "Notification sending failed. Got {0}", NotifRecents.Count)
    End Sub

    ''' <summary>
    ''' Tests notification dismiss
    ''' </summary>
    <TestMethod> Public Sub TestNotifyDismiss()
        Dim Notif As Notification = NotifyCreate("Redundant title", "This is a redundant notification", NotifPriority.Low)
        NotifySend(Notif)
        Assert.IsTrue(NotifDismiss(NotifRecents.Count - 1), "Notification removal failed. Expected True, got False.")
    End Sub

End Class