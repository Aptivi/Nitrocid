
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

Namespace Misc.TextEdit.Commands
    Class TextEdit_PrintCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim LineNumber As Integer = 1
            If ListArgs?.Length > 0 Then
                If ListArgs?.Length = 1 Then
                    'We've only provided one line number
                    Wdbg(DebugLevel.I, "Line number provided: {0}", ListArgs(0))
                    Wdbg(DebugLevel.I, "Is it numeric? {0}", IsStringNumeric(ListArgs(0)))
                    If IsStringNumeric(ListArgs(0)) Then
                        LineNumber = ListArgs(0)
                        Wdbg(DebugLevel.I, "File lines: {0}", TextEdit_FileLines.Count)
                        If CInt(ListArgs(0)) <= TextEdit_FileLines.Count Then
                            Dim Line As String = TextEdit_FileLines(LineNumber - 1)
                            Wdbg(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line)
                            Write("- {0}: ", False, ColTypes.ListEntry, LineNumber)
                            Write(Line, True, ColTypes.ListValue)
                        Else
                            Write(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                        End If
                    Else
                        Write(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, ListArgs(0))
                        Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(0))
                    End If
                Else
                    'We've provided two line numbers in the range
                    Wdbg(DebugLevel.I, "Line numbers provided: {0}, {1}", ListArgs(0), ListArgs(1))
                    Wdbg(DebugLevel.I, "Is it numeric? {0}", IsStringNumeric(ListArgs(0)), IsStringNumeric(ListArgs(1)))
                    If IsStringNumeric(ListArgs(0)) And IsStringNumeric(ListArgs(1)) Then
                        Dim LineNumberStart As Integer = ListArgs(0)
                        Dim LineNumberEnd As Integer = ListArgs(1)
                        LineNumberStart.SwapIfSourceLarger(LineNumberEnd)
                        Wdbg(DebugLevel.I, "File lines: {0}", TextEdit_FileLines.Count)
                        If LineNumberStart <= TextEdit_FileLines.Count And LineNumberEnd <= TextEdit_FileLines.Count Then
                            For LineNumber = LineNumberStart To LineNumberEnd
                                Dim Line As String = TextEdit_FileLines(LineNumber - 1)
                                Wdbg(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line)
                                Write("- {0}: ", False, ColTypes.ListEntry, LineNumber)
                                Write(Line, True, ColTypes.ListValue)
                            Next
                        Else
                            Write(DoTranslation("The specified line number may not be larger than the last file line number."), True, ColTypes.Error)
                        End If
                    Else
                        Write(DoTranslation("Specified line number {0} is not a valid number."), True, ColTypes.Error, ListArgs(0))
                        Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(0))
                    End If
                End If
            Else
                For Each Line As String In TextEdit_FileLines
                    Wdbg(DebugLevel.I, "Line number: {0} ({1})", LineNumber, Line)
                    Write("- {0}: ", False, ColTypes.ListEntry, LineNumber)
                    Write(Line, True, ColTypes.ListValue)
                    LineNumber += 1
                Next
            End If
        End Sub

    End Class
End Namespace