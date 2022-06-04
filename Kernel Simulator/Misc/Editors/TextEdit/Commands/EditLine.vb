
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

Imports KS.Misc.Reflection

Namespace Misc.TextEdit.Commands
    Class TextEdit_EditLineCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If IsStringNumeric(ListArgsOnly(0)) Then
                If CInt(ListArgsOnly(0)) <= TextEdit_FileLines.Count Then
                    Dim OriginalLine As String = TextEdit_FileLines(ListArgsOnly(0) - 1)
                    Write(">> [{0}]", False, ColTypes.Input, OriginalLine)
                    Dim EditedLine As String = ReadLine("", OriginalLine, False)
                    TextEdit_FileLines(ListArgsOnly(0) - 1) = EditedLine
                Else
                    Write(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                End If
            Else
                Write(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error)
                Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(0))
            End If
        End Sub

    End Class
End Namespace