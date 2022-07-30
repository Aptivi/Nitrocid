
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

Imports KS.Shell.ShellBase.Aliases

Namespace Shell.Commands
    Class AliasCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs?.Length > 3 Then
                If ListArgs(0) = "add" And [Enum].IsDefined(GetType(ShellType), ListArgs(1)) Then
                    ManageAlias(ListArgs(0), [Enum].Parse(GetType(ShellType), ListArgs(1)), ListArgs(2), ListArgs(3))
                Else
                    Write(DoTranslation("Invalid type {0}."), True, ColTypes.Error, ListArgs(1))
                End If
            ElseIf ListArgs?.Length = 3 Then
                If ListArgs(0) = "rem" And [Enum].IsDefined(GetType(ShellType), ListArgs(1)) Then
                    ManageAlias(ListArgs(0), [Enum].Parse(GetType(ShellType), ListArgs(1)), ListArgs(2))
                Else
                    Write(DoTranslation("Invalid type {0}."), True, ColTypes.Error, ListArgs(1))
                End If
            End If
        End Sub

    End Class
End Namespace