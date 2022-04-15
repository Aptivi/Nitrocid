
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

Imports Extensification.LongExts
Imports KS.Misc.Reflection

Namespace Misc.HexEdit.Commands
    Class HexEdit_DelBytesCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs?.Count = 1 Then
                If IsStringNumeric(ListArgs(0)) Then
                    If CLng(ListArgs(0)) <= HexEdit_FileBytes.LongLength Then
                        HexEdit_DeleteBytes(ListArgs(0))
                        Write(DoTranslation("Deleted bytes."), True, ColTypes.Success)
                    Else
                        Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                    End If
                Else
                    Write(DoTranslation("Specified Byte number {0} is not a valid number."), True, ColTypes.Error, ListArgs(0))
                    Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(0))
                End If
            ElseIf ListArgs?.Count > 1 Then
                If IsStringNumeric(ListArgs(0)) And IsStringNumeric(ListArgs(1)) Then
                    If CLng(ListArgs(0)) <= HexEdit_FileBytes.LongLength And CLng(ListArgs(1)) <= HexEdit_FileBytes.LongLength Then
                        Dim ByteNumberStart As Long = ListArgs(0)
                        Dim ByteNumberEnd As Long = ListArgs(1)
                        ByteNumberStart.SwapIfSourceLarger(ByteNumberEnd)
                        HexEdit_DeleteBytes(ByteNumberStart, ByteNumberEnd)
                    Else
                        Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                    End If
                Else
                    Write(DoTranslation("The byte number is not numeric."), True, ColTypes.Error)
                    Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(1))
                End If
            End If
        End Sub

    End Class
End Namespace