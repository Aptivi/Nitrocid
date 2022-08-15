
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

Imports KS.Scripting

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Makes a UESH array and sets their values
    ''' </summary>
    ''' <remarks>
    ''' If you want to store a group of values in one variable, you can use this command to create arrays of values. Such variables will have the [n] suffix, for example, $values[1].
    ''' </remarks>
    Class SetRangeCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            SetVariables(ListArgs(0), ListArgsOnly.Skip(1).ToArray)
        End Sub

    End Class
End Namespace
