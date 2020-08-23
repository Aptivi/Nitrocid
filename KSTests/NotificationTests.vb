
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

    <TestMethod()> Public Sub TestNotifyCreate()
        Try
            Assert.IsNotNull(NotifyCreate("This is the title.", "This is the description.", NotifPriority.Medium))
        Catch ex As Exception
            Assert.Fail("Notification creation failed.")
        End Try
    End Sub

    <TestMethod> Public Sub TestNotifySend()
        Try
            Dim Notif As Notification = NotifyCreate("Notification title", "This is a high priority notification", NotifPriority.High)
            NotifySend(Notif)
            Assert.IsTrue(NotifRecents.Count > 0)
        Catch ex As Exception
            Assert.Fail("Notification sending failed.")
        End Try
    End Sub

    <TestMethod> Public Sub TestNotifyDismiss()
        Try
            Dim Notif As Notification = NotifyCreate("Redundant title", "This is a redundant notification", NotifPriority.Low)
            NotifySend(Notif)
            Assert.IsTrue(NotifDismiss(NotifRecents.Count - 1))
        Catch ex As Exception
            Assert.Fail("Notification removal failed.")
        End Try
    End Sub

End Class