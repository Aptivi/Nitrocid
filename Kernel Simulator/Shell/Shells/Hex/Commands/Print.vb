
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
    ''' Prints the contents of the file
    ''' </summary>
    ''' <remarks>
    ''' Prints the contents of the file with bytes to the console. This is useful if you need to view the contents before and after editing.
    ''' </remarks>
    Class HexEdit_PrintCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ByteNumber As Long
            If ListArgs?.Length > 0 Then
                If ListArgs?.Length = 1 Then
                    'We've only provided one range
                    Wdbg(DebugLevel.I, "Byte number provided: {0}", ListArgs(0))
                    Wdbg(DebugLevel.I, "Is it numeric? {0}", IsStringNumeric(ListArgs(0)))
                    If IsStringNumeric(ListArgs(0)) Then
                        ByteNumber = ListArgs(0)
                        HexEdit_DisplayHex(ByteNumber)
                    Else
                        Write(DoTranslation("The byte number is not numeric."), True, ColTypes.Error)
                        Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(0))
                    End If
                Else
                    'We've provided two Byte numbers in the range
                    Wdbg(DebugLevel.I, "Byte numbers provided: {0}, {1}", ListArgs(0), ListArgs(1))
                    Wdbg(DebugLevel.I, "Is it numeric? {0}", IsStringNumeric(ListArgs(0)), IsStringNumeric(ListArgs(1)))
                    If IsStringNumeric(ListArgs(0)) And IsStringNumeric(ListArgs(1)) Then
                        Dim ByteNumberStart As Long = ListArgs(0)
                        Dim ByteNumberEnd As Long = ListArgs(1)
                        ByteNumberStart.SwapIfSourceLarger(ByteNumberEnd)
                        HexEdit_DisplayHex(ByteNumberStart, ByteNumberEnd)
                    Else
                        Write(DoTranslation("The byte number is not numeric."), True, ColTypes.Error)
                        Wdbg(DebugLevel.E, "{0} is not a numeric value.", ListArgs(0))
                    End If
                End If
            Else
                HexEdit_DisplayHex()
            End If
        End Sub

    End Class
End Namespace
