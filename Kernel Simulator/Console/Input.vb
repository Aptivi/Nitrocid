
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Imports System.Runtime.CompilerServices

Public Module Input

    ''' <summary>
    ''' Reads the next line of characters from the standard input stream without showing input being written by user.
    ''' </summary>
    Public Function ReadLineNoInput() As String
        Dim Final As String = ""
        While True
            Dim character As Char = Console.ReadKey(True).KeyChar
            If character = vbCr Or character = vbLf Then
                Exit While
            ElseIf character = vbBack Then
                If Not Final.Length = 0 Then Final = Final.Remove(Final.Length - 1)
            Else
                Final += character
            End If
        End While
        Return Final
    End Function

End Module
