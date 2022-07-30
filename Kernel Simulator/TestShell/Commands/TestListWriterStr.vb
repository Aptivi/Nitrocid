
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

Namespace TestShell.Commands
    Class Test_TestListWriterStrCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim NormalStringList As New List(Of String) From {"String 1", "String 2", "String 3"}
            Dim ArrayStringList As New List(Of String()) From {{{"String 1", "String 2", "String 3"}}, {{"String 1", "String 2", "String 3"}}, {{"String 1", "String 2", "String 3"}}}
            Write(DoTranslation("Normal string list:"), True, ColTypes.Neutral)
            WriteList(NormalStringList)
            Write(DoTranslation("Array string list:"), True, ColTypes.Neutral)
            WriteList(ArrayStringList)
        End Sub

    End Class
End Namespace