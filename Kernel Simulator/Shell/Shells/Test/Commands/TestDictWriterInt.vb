
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
    ''' It lets you test the dictionary writer using the Integer type.
    ''' </summary>
    Class Test_TestDictWriterIntCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim NormalIntegerDict As New Dictionary(Of String, Integer) From {{"One", 1}, {"Two", 2}, {"Three", 3}}
            Dim ArrayIntegerDict As New Dictionary(Of String, Integer()) From {{"One", {1, 2, 3}}, {"Two", {1, 2, 3}}, {"Three", {1, 2, 3}}}
            Write(DoTranslation("Normal integer dictionary:"), True, ColTypes.Neutral)
            WriteList(NormalIntegerDict)
            Write(DoTranslation("Array integer dictionary:"), True, ColTypes.Neutral)
            WriteList(ArrayIntegerDict)
        End Sub

    End Class
End Namespace
