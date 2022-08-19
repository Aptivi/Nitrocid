
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
    ''' <summary>
    ''' Queries a byte in a specified byte, a range of bytes, or entirely
    ''' </summary>
    ''' <remarks>
    ''' You can use this command to query a byte and get its number from the specified byte, a range of bytes, or entirely.
    ''' </remarks>
    Class HexEdit_QueryByteCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgsOnly.Length = 1 Then
                Dim ByteContent As Byte = Convert.ToByte(ListArgsOnly(0), 16)
                HexEdit_QueryByteAndDisplay(ByteContent)
            ElseIf ListArgsOnly.Length = 2 Then
                If IsStringNumeric(ListArgsOnly(1)) Then
                    If CLng(ListArgsOnly(1)) <= HexEdit_FileBytes.LongLength Then
                        Dim ByteContent As Byte = Convert.ToByte(ListArgsOnly(0), 16)
                        HexEdit_QueryByteAndDisplay(ByteContent, ListArgsOnly(1))
                    Else
                        Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                    End If
                End If
            ElseIf ListArgsOnly.Length > 2 Then
                If IsStringNumeric(ListArgsOnly(1)) And IsStringNumeric(ListArgsOnly(2)) Then
                    If CLng(ListArgsOnly(1)) <= HexEdit_FileBytes.LongLength And CLng(ListArgsOnly(2)) <= HexEdit_FileBytes.LongLength Then
                        Dim ByteContent As Byte = Convert.ToByte(ListArgsOnly(0), 16)
                        Dim ByteNumberStart As Long = ListArgsOnly(1)
                        Dim ByteNumberEnd As Long = ListArgsOnly(2)
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
