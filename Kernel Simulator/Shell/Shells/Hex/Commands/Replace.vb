
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
    ''' Replaces a byte with another one
    ''' </summary>
    ''' <remarks>
    ''' You can use this command to replace a byte with another one.
    ''' </remarks>
    Class HexEdit_ReplaceCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgsOnly.Length = 2 Then
                Dim ByteFrom As Byte = Convert.ToByte(ListArgsOnly(0), 16)
                Dim ByteWith As Byte = Convert.ToByte(ListArgsOnly(1), 16)
                HexEdit_Replace(ByteFrom, ByteWith)
                Write(DoTranslation("Byte replaced."), True, ColTypes.Success)
            ElseIf ListArgsOnly.Length = 3 Then
                If IsStringNumeric(ListArgsOnly(2)) Then
                    If CLng(ListArgsOnly(2)) <= HexEdit_FileBytes.LongLength Then
                        Dim ByteFrom As Byte = Convert.ToByte(ListArgsOnly(0), 16)
                        Dim ByteWith As Byte = Convert.ToByte(ListArgsOnly(1), 16)
                        HexEdit_Replace(ByteFrom, ByteWith, ListArgsOnly(2))
                        Write(DoTranslation("Byte replaced."), True, ColTypes.Success)
                    Else
                        Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                    End If
                End If
            ElseIf ListArgsOnly.Length > 3 Then
                If IsStringNumeric(ListArgsOnly(2)) And IsStringNumeric(ListArgsOnly(3)) Then
                    If CLng(ListArgsOnly(2)) <= HexEdit_FileBytes.LongLength And CLng(ListArgsOnly(3)) <= HexEdit_FileBytes.LongLength Then
                        Dim ByteFrom As Byte = Convert.ToByte(ListArgsOnly(0), 16)
                        Dim ByteWith As Byte = Convert.ToByte(ListArgsOnly(1), 16)
                        Dim ByteNumberStart As Long = ListArgsOnly(2)
                        Dim ByteNumberEnd As Long = ListArgsOnly(3)
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
