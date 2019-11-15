
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Imports System.Runtime.InteropServices

Public Module Color255

    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Function SetConsoleMode(ByVal hConsoleHandle As IntPtr, ByVal mode As Integer) As Boolean
    End Function
    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Function GetConsoleMode(ByVal handle As IntPtr, <Out()> ByRef mode As Integer) As Boolean
    End Function
    <DllImport("kernel32.dll", SetLastError:=True)>
    Public Function GetStdHandle(ByVal handle As Integer) As IntPtr
    End Function
    Public Used255 As Boolean
    Sub Initialize255()
        Dim handle = GetStdHandle(-11)
        Wdbg("Integer pointer {0}", handle)
        Dim mode As Integer
        GetConsoleMode(handle, mode)
        Wdbg("Mode: {0}", mode)
        If Not mode = 7 Then
            SetConsoleMode(handle, mode Or &H4)
            Wdbg("Added support for VT escapes.")
        End If
        Used255 = True
    End Sub
    Function GetEsc() As Char
        If Used255 Then
            Return ChrW(&H1B) 'ESC
        Else
            Return ChrW(0) 'vbNullChar
        End If
    End Function

End Module
