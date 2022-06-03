
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

Namespace Misc.HexEdit.Commands
    Class HexEdit_AddBytesCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim FinalBytes As New List(Of Byte)
            Dim FinalByte As String = ""

            'Keep prompting for bytes until the user finishes
            Write(DoTranslation("Enter a byte on its own line that you want to append to the end of the file. When you're done, write ""EOF"" on its own line."), True, ColTypes.Neutral)
            Do Until FinalByte = "EOF"
                Write(">> ", False, ColTypes.Input)
                FinalByte = ReadLine(False)
                If Not FinalByte = "EOF" Then
                    Dim ByteContent As Byte
                    If Byte.TryParse(FinalByte, Globalization.NumberStyles.HexNumber, Nothing, ByteContent) Then
                        FinalBytes.Add(ByteContent)
                    Else
                        Write(DoTranslation("Not a valid byte."), True, ColTypes.Error)
                    End If
                End If
            Loop

            'Add the new bytes
            HexEdit_AddNewBytes(FinalBytes.ToArray)
        End Sub

    End Class
End Namespace