
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

Imports Microsoft.VisualBasic

Namespace Shell.Shells.Test.Commands
    ''' <summary>
    ''' It lets you raise any event. If you have loaded mods, you can use this command for testing event raises.
    ''' </summary>
    Class Test_TestEventCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
                Dim SubName As String = "Raise" + ListArgs(0)
                CallByName(New Events.Events, SubName, CallType.Method)
            Catch ex As Exception
                Write(DoTranslation("Failure to raise event {0}: {1}"), True, ColTypes.Error, ListArgs(0))
            End Try
        End Sub

    End Class
End Namespace
