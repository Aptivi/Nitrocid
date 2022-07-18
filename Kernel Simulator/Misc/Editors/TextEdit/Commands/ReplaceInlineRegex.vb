
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

Imports Extensification.IntegerExts
Imports KS.Misc.Reflection

Namespace Misc.Editors.TextEdit.Commands
    Class TextEdit_ReplaceInlineRegexCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs?.Count = 3 Then
                If IsStringNumeric(ListArgs(2)) Then
                    If CInt(ListArgs(2)) <= TextEdit_FileLines.Count Then
                        TextEdit_ReplaceRegex(ListArgs(0), ListArgs(1), ListArgs(2))
                        Write(DoTranslation("String replaced."), True, ColTypes.Success)
                    Else
                        Write(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                    End If
                Else
                    Write(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, ListArgs(2))
                    Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(2))
                End If
            ElseIf ListArgs?.Count > 3 Then
                If IsStringNumeric(ListArgs(2)) And IsStringNumeric(ListArgs(3)) Then
                    If CInt(ListArgs(2)) <= TextEdit_FileLines.Count And CInt(ListArgs(3)) <= TextEdit_FileLines.Count Then
                        Dim LineNumberStart As Integer = ListArgs(2)
                        Dim LineNumberEnd As Integer = ListArgs(3)
                        LineNumberStart.SwapIfSourceLarger(LineNumberEnd)
                        For LineNumber = LineNumberStart To LineNumberEnd
                            TextEdit_ReplaceRegex(ListArgs(0), ListArgs(1), LineNumber)
                            Write(DoTranslation("String replaced in line {0}."), True, ColTypes.Success, LineNumber)
                        Next
                    Else
                        Write(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                    End If
                End If
            End If
        End Sub

    End Class
End Namespace