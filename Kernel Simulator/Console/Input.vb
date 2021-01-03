'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Public Module Input

    ''' <summary>
    ''' Reads the next line of characters from the standard input stream without showing input being written by user.
    ''' </summary>
    ''' <param name="MaskChar">Specifies the password mask character</param>
    Public Function ReadLineNoInput(ByVal MaskChar As Char) As String
        Dim Final As String = ""
        While True
            Dim KeyInfo As ConsoleKeyInfo = Console.ReadKey(True)
            Dim KeyCharacter As Char = KeyInfo.KeyChar
            If KeyCharacter = vbCr Or KeyCharacter = vbLf Then
                Exit While
            ElseIf KeyInfo.Key = ConsoleKey.Backspace Then
                If Not Final.Length = 0 Then
                    Final = Final.Remove(Final.Length - 1)
                    If Not MaskChar = vbNullChar Then
                        Console.Write(GetEsc() + "D") 'Cursor backwards by one character
                        Console.Write(GetEsc() + "[1X") 'Remove a character
                    End If
                End If
            Else
                Final += KeyCharacter
                If Not MaskChar = vbNullChar Then Console.Write(MaskChar)
            End If
        End While
        Return Final
    End Function

End Module
