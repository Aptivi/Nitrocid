
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Class TextEdit_DelCharNumCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String) Implements ICommand.Execute
        If IsNumeric(ListArgs(1)) And IsNumeric(ListArgs(0)) Then
            If CInt(ListArgs(1)) <= TextEdit_FileLines.Count Then
                TextEdit_DeleteChar(ListArgs(0), ListArgs(1))
                W(DoTranslation("Character deleted."), True, ColTypes.Success)
            Else
                W(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
            End If
        Else
            W(DoTranslation("One or both of the numbers are not numeric."), True, ColTypes.Error)
            Wdbg(DebugLevel.E, "{0} and {1} are not numeric values.", ListArgs(0), ListArgs(1))
        End If
    End Sub

End Class