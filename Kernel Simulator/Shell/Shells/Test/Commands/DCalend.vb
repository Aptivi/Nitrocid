
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

Imports System.Globalization
Imports KS.TimeDate

Namespace Shell.Shells.Test.Commands
    ''' <summary>
    ''' It lets you render date using different calendar types (one of Gregorian, Hijri, Persian, Saudi-Hijri, Thai-Buddhist)
    ''' </summary>
    Class Test_DCalendCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs(0) = "Gregorian" Then
                Write(RenderDate(New CultureInfo("en-US")), True, ColTypes.Neutral)
            ElseIf ListArgs(0) = "Hijri" Then
                Dim Cult As New CultureInfo("ar") : Cult.DateTimeFormat.Calendar = New HijriCalendar
                Write(RenderDate(Cult), True, ColTypes.Neutral)
            ElseIf ListArgs(0) = "Persian" Then
                Write(RenderDate(New CultureInfo("fa")), True, ColTypes.Neutral)
            ElseIf ListArgs(0) = "Saudi-Hijri" Then
                Write(RenderDate(New CultureInfo("ar-SA")), True, ColTypes.Neutral)
            ElseIf ListArgs(0) = "Thai-Buddhist" Then
                Write(RenderDate(New CultureInfo("th-TH")), True, ColTypes.Neutral)
            End If
        End Sub

    End Class
End Namespace
