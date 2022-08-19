
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

Imports KS.Files.Operations

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' You can copy the file to a destination.
    ''' </summary>
    ''' <remarks>
    ''' This command allows you to create copies of files or folders to a different name, or different path. This is useful for many purposes.
    ''' </remarks>
    Class CopyCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            CopyFileOrDir(ListArgsOnly(0), ListArgsOnly(1))
        End Sub

    End Class
End Namespace
