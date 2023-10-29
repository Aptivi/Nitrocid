﻿
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

Imports KS.Misc.Notifications

Namespace Shell.Commands
    Class ShowNotifsCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Count As Integer = 1
            If Not NotifRecents.Count = 0 Then
                For Each Notif As Notification In NotifRecents
                    TextWriterColor.Write($"[{Count}/{NotifRecents.Count}] {Notif.Title}: ", False, ColTypes.ListEntry)
                    TextWriterColor.Write(Notif.Desc, False, ColTypes.ListValue)
                    If Notif.Type = NotifType.Progress Then
                        TextWriterColor.Write($" ({Notif.Progress}%)", False, If(Notif.ProgressFailed, ColTypes.Error, ColTypes.Success))
                    End If
                    TextWriterColor.Write("", True, ColTypes.Neutral)
                    Count += 1
                Next
            Else
                TextWriterColor.Write(DoTranslation("No recent notifications"), True, ColTypes.Neutral)
            End If
        End Sub

    End Class
End Namespace