
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
Imports KS.Misc.Editors.HexEdit

Namespace Shell.Shells.Hex.Commands
    Class HexEdit_ReplaceCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgs?.Count = 2 Then
                Dim ByteFrom As Byte = Convert.ToByte(ListArgs(0), 16)
                Dim ByteWith As Byte = Convert.ToByte(ListArgs(1), 16)
                HexEdit_Replace(ByteFrom, ByteWith)
                Write(DoTranslation("Byte replaced."), True, ColTypes.Success)
            ElseIf ListArgs?.Count = 3 Then
                If IsStringNumeric(ListArgs(2)) Then
                    If CLng(ListArgs(2)) <= HexEdit_FileBytes.LongLength Then
                        Dim ByteFrom As Byte = Convert.ToByte(ListArgs(0), 16)
                        Dim ByteWith As Byte = Convert.ToByte(ListArgs(1), 16)
                        HexEdit_Replace(ByteFrom, ByteWith, ListArgs(2))
                        Write(DoTranslation("Byte replaced."), True, ColTypes.Success)
                    Else
                        Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                    End If
                End If
            ElseIf ListArgs?.Count > 3 Then
                If IsStringNumeric(ListArgs(2)) And IsStringNumeric(ListArgs(3)) Then
                    If CLng(ListArgs(2)) <= HexEdit_FileBytes.LongLength And CLng(ListArgs(3)) <= HexEdit_FileBytes.LongLength Then
                        Dim ByteFrom As Byte = Convert.ToByte(ListArgs(0), 16)
                        Dim ByteWith As Byte = Convert.ToByte(ListArgs(1), 16)
                        Dim ByteNumberStart As Long = ListArgs(2)
                        Dim ByteNumberEnd As Long = ListArgs(3)
                        ByteNumberStart.SwapIfSourceLarger(ByteNumberEnd)
                        HexEdit_Replace(ByteFrom, ByteWith, ByteNumberStart, ByteNumberEnd)
                        Write(DoTranslation("Byte replaced."), True, ColTypes.Success)
                    Else
                        Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                    End If
                End If
            End If
        End Sub

    End Class
End Namespace
