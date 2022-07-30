
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

Imports Extensification.LongExts
Imports KS.Misc.Reflection

Namespace Misc.Editors.HexEdit.Commands
    Class HexEdit_QueryByteCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs?.Count = 1 Then
                Dim ByteContent As Byte = Convert.ToByte(ListArgs(0), 16)
                HexEdit_QueryByteAndDisplay(ByteContent)
            ElseIf ListArgs?.Count = 2 Then
                If IsStringNumeric(ListArgs(1)) Then
                    If CLng(ListArgs(1)) <= HexEdit_FileBytes.LongLength Then
                        Dim ByteContent As Byte = Convert.ToByte(ListArgs(0), 16)
                        HexEdit_QueryByteAndDisplay(ByteContent, ListArgs(1))
                    Else
                        Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                    End If
                End If
            ElseIf ListArgs?.Count > 2 Then
                If IsStringNumeric(ListArgs(1)) And IsStringNumeric(ListArgs(2)) Then
                    If CLng(ListArgs(1)) <= HexEdit_FileBytes.LongLength And CLng(ListArgs(2)) <= HexEdit_FileBytes.LongLength Then
                        Dim ByteContent As Byte = Convert.ToByte(ListArgs(0), 16)
                        Dim ByteNumberStart As Long = ListArgs(1)
                        Dim ByteNumberEnd As Long = ListArgs(2)
                        ByteNumberStart.SwapIfSourceLarger(ByteNumberEnd)
                        HexEdit_QueryByteAndDisplay(ByteContent, ByteNumberStart, ByteNumberEnd)
                    Else
                        Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                    End If
                End If
            End If
        End Sub

    End Class
End Namespace