
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
    ''' It lets you send any message to the debugger, using <see cref="Wdbg(DebugLevel, String, Object())"/> call. It doesn't provide support for variables unlike printdf. It only works if you have enabled the debugger which you can enable by debug 1.
    ''' </summary>
    Class Test_PrintDCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Wdbg(DebugLevel.I, String.Join(" ", ListArgsOnly))
        End Sub

    End Class
End Namespace
