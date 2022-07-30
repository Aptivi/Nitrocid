
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

Imports Extensification.IntegerExts
Imports KS.Misc.Reflection

Namespace Misc.Editors.TextEdit.Commands
    Class TextEdit_DelWordCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs?.Count = 2 Then
                If IsStringNumeric(ListArgs(1)) Then
                    If CInt(ListArgs(1)) <= TextEdit_FileLines.Count Then
                        TextEdit_DeleteWord(ListArgs(0), ListArgs(1))
                        Write(DoTranslation("Word deleted."), True, ColTypes.Success)
                    Else
                        Write(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                    End If
                Else
                    Write(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, ListArgs(1))
                    Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(1))
                End If
            ElseIf ListArgs?.Count > 2 Then
                If IsStringNumeric(ListArgs(1)) And IsStringNumeric(ListArgs(2)) Then
                    If CInt(ListArgs(1)) <= TextEdit_FileLines.Count And CInt(ListArgs(2)) <= TextEdit_FileLines.Count Then
                        Dim LineNumberStart As Integer = ListArgs(1)
                        Dim LineNumberEnd As Integer = ListArgs(2)
                        LineNumberStart.SwapIfSourceLarger(LineNumberEnd)
                        For LineNumber = LineNumberStart To LineNumberEnd
                            TextEdit_DeleteWord(ListArgs(0), LineNumber)
                            Write(DoTranslation("Word deleted in line {0}."), True, ColTypes.Success, LineNumber)
                        Next
                    Else
                        Write(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                    End If
                Else
                    Write(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, ListArgs(1))
                    Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(1))
                End If
            End If
        End Sub

    End Class
End Namespace