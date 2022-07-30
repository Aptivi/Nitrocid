
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
    Class Test_TestDictWriterStrCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim NormalStringDict As New Dictionary(Of String, String) From {{"One", "String 1"}, {"Two", "String 2"}, {"Three", "String 3"}}
            Dim ArrayStringDict As New Dictionary(Of String, String()) From {{"One", {"String 1", "String 2", "String 3"}}, {"Two", {"String 1", "String 2", "String 3"}}, {"Three", {"String 1", "String 2", "String 3"}}}
            Write(DoTranslation("Normal string dictionary:"), True, ColTypes.Neutral)
            WriteList(NormalStringDict)
            Write(DoTranslation("Array string dictionary:"), True, ColTypes.Neutral)
            WriteList(ArrayStringDict)
        End Sub

    End Class
End Namespace