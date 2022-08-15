
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
    ''' Lists variables
    ''' </summary>
    ''' <remarks>
    ''' This command lists all the defined UESH variables by either the set or the setrange commands, UESH commands that define and set a variable to a value (choice, ...), a UESH script, a mod, or your system's environment variables.
    ''' </remarks>
    Class LsVarsCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            For Each VarName As String In GetVariables().Keys
                Write($"- {VarName}: ", False, ColTypes.ListEntry)
                Write(GetVariables()(VarName), True, ColTypes.ListValue)
            Next
        End Sub

    End Class
End Namespace
