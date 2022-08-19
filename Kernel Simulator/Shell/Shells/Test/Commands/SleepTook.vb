
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

Namespace Shell.Shells.Test.Commands
    ''' <summary>
    ''' Checks how many milliseconds (or ticks if started with the -t switch) did it really take to sleep.
    ''' </summary>
    Class Test_SleepTookCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim SleepMs As Integer = ListArgsOnly(0)
            Dim Ticks As Boolean = ListSwitchesOnly.Contains("-t")
            If Ticks Then
                Write("{0} ms => {1} ticks", True, ColTypes.Neutral, SleepMs, GetActualTicks(SleepMs))
            Else
                Write("{0} ms => {1} ms", True, ColTypes.Neutral, SleepMs, GetActualMilliseconds(SleepMs))
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -t: ", False, ColTypes.ListEntry) : Write(DoTranslation("Unit in ticks"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace
