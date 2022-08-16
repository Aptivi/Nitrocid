
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

Imports Figgle
Imports KS.Misc.Writers.FancyWriters.Tools

Namespace Shell.Shells.Test.Commands
    ''' <summary>
    ''' It lets you test the figlet print to print every text, using the font and colors that you need. It has an additional feature of variables.
    ''' </summary>
    Class Test_PrintFigletFCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Parts As New List(Of String)(ListArgs)
            Dim Color As ColTypes = ListArgs(0)
            Dim FigletFont As FiggleFont = GetFigletFont(ListArgs(1))
            Dim Vars As Object() = ListArgs(2).Split(";")
            Dim Text As String = ListArgs(3)
            WriteFiglet(Text, FigletFont, Color, Vars)
        End Sub

    End Class
End Namespace
