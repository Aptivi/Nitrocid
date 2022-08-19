
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
    ''' Deletes the bytes
    ''' </summary>
    ''' <remarks>
    ''' You can use this command to remove a extraneous bytes in a specified range. You can use the print command to review the changes.
    ''' </remarks>
    Class HexEdit_DelBytesCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If ListArgsOnly.Length = 1 Then
                If IsStringNumeric(ListArgsOnly(0)) Then
                    If CLng(ListArgsOnly(0)) <= HexEdit_FileBytes.LongLength Then
                        HexEdit_DeleteBytes(ListArgsOnly(0))
                        Write(DoTranslation("Deleted bytes."), True, ColTypes.Success)
                    Else
                        Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                    End If
                Else
                    Write(DoTranslation("Specified Byte number {0} is not a valid number."), True, ColTypes.Error, ListArgsOnly(0))
                    Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly(0))
                End If
            ElseIf ListArgsOnly.Length > 1 Then
                If IsStringNumeric(ListArgsOnly(0)) And IsStringNumeric(ListArgsOnly(1)) Then
                    If CLng(ListArgsOnly(0)) <= HexEdit_FileBytes.LongLength And CLng(ListArgsOnly(1)) <= HexEdit_FileBytes.LongLength Then
                        Dim ByteNumberStart As Long = ListArgsOnly(0)
                        Dim ByteNumberEnd As Long = ListArgsOnly(1)
                        ByteNumberStart.SwapIfSourceLarger(ByteNumberEnd)
                        HexEdit_DeleteBytes(ByteNumberStart, ByteNumberEnd)
                    Else
                        Write(DoTranslation("The specified byte number may not be larger than the file size."), True, ColTypes.Error)
                    End If
                Else
                    Write(DoTranslation("The byte number is not numeric."), True, ColTypes.Error)
                    Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgsOnly(1))
                End If
            End If
        End Sub

    End Class
End Namespace
