
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

Imports KS.Misc.Reflection
Imports KS.Misc.Editors.TextEdit

Namespace Shell.Shells.Text.Commands
    Class TextEdit_DelCharNumCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If IsStringNumeric(ListArgs(1)) And IsStringNumeric(ListArgs(0)) Then
                If CInt(ListArgs(1)) <= TextEdit_FileLines.Count Then
                    TextEdit_DeleteChar(ListArgs(0), ListArgs(1))
                    Write(DoTranslation("Character deleted."), True, ColTypes.Success)
                Else
                    Write(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                End If
            Else
                Write(DoTranslation("One or both of the numbers are not numeric."), True, ColTypes.Error)
                Wdbg(DebugLevel.E, "{0} and {1} are not numeric values.", ListArgs(0), ListArgs(1))
            End If
        End Sub

    End Class
End Namespace
