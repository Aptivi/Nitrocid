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

Namespace Shell.Commands
    Class FindCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim FileToSearch As String = ListArgsOnly(0)
            Dim DirectoryToSearch As String = CurrDir
            If ListArgsOnly.Length > 1 Then
                DirectoryToSearch = NeutralizePath(ListArgsOnly(1))
            End If

            'Print the results if found
            Dim FileEntries As String() = GetFilesystemEntries(DirectoryToSearch, FileToSearch)
            WriteList(FileEntries, True)
        End Sub

    End Class
End Namespace