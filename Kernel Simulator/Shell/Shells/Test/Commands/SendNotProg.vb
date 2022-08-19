
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

Imports KS.Misc.Notifications

Namespace Shell.Shells.Test.Commands
    ''' <summary>
    ''' It lets you test the notification system by sending the notification with the specified title and description on a specific priority with progress support to test the incrementation. It can be set to fail at a specific percentage (0-100).
    ''' </summary>
    Class Test_SendNotProgCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Notif As New Notification(ListArgsOnly(1), ListArgsOnly(2), ListArgsOnly(0), NotifType.Progress)
            NotifySend(Notif)
            Do While Not Notif.ProgressCompleted
                Threading.Thread.Sleep(100)
                If ListArgsOnly(3) >= 0 And Notif.Progress >= ListArgsOnly(3) Then
                    Notif.ProgressFailed = True
                End If
                Notif.Progress += 1
            Loop
        End Sub

    End Class
End Namespace
