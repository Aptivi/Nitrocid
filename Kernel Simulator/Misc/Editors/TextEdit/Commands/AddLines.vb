
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

Namespace Misc.TextEdit.Commands
    Class TextEdit_AddLinesCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim FinalLines As New List(Of String)
            Dim FinalLine As String = ""

            'Keep prompting for lines until the user finishes
            Write(DoTranslation("Enter the text that you want to append to the end of the file. When you're done, write ""EOF"" on its own line."), True, ColTypes.Neutral)
            Do Until FinalLine = "EOF"
                Write(">> ", False, ColTypes.Input)
                FinalLine = ReadLine()
                If Not FinalLine = "EOF" Then
                    FinalLines.Add(FinalLine)
                End If
            Loop

            'Add the new lines
            TextEdit_AddNewLines(FinalLines.ToArray)
        End Sub

    End Class
End Namespace