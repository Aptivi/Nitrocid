
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
    Class Test_TestDictWriterCharCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim NormalCharDict As New Dictionary(Of String, Char) From {{"One", "1"c}, {"Two", "2"c}, {"Three", "3"c}}
            Dim ArrayCharDict As New Dictionary(Of String, Char()) From {{"One", {"1"c, "2"c, "3"c}}, {"Two", {"1"c, "2"c, "3"c}}, {"Three", {"1"c, "2"c, "3"c}}}
            Write(DoTranslation("Normal char dictionary:"), True, ColTypes.Neutral)
            WriteList(NormalCharDict)
            Write(DoTranslation("Array char dictionary:"), True, ColTypes.Neutral)
            WriteList(ArrayCharDict)
        End Sub

    End Class
End Namespace