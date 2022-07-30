
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

Imports System.Runtime.InteropServices
Imports Newtonsoft.Json.Linq

Namespace ConsoleBase.Colors
    Public Module Color255

        ''' <summary>
        ''' The 255 console colors data JSON token to get information about these colors
        ''' </summary>
        Public ReadOnly ColorDataJson As JToken = JToken.Parse(My.Resources.ConsoleColorsData)

        ''' <summary>
        ''' [Windows] Sets console mode
        ''' </summary>
        ''' <param name="hConsoleHandle">Console Handle</param>
        ''' <param name="mode">Mode</param>
        ''' <returns>True if succeeded, false if failed</returns>
        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Function SetConsoleMode(hConsoleHandle As IntPtr, mode As Integer) As Boolean
        End Function

        ''' <summary>
        ''' [Windows] Gets console mode
        ''' </summary>
        ''' <param name="handle">Console handle</param>
        ''' <param name="mode">Mode</param>
        ''' <returns>True if succeeded, false if failed</returns>
        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Function GetConsoleMode(handle As IntPtr, <Out()> ByRef mode As Integer) As Boolean
        End Function

        ''' <summary>
        ''' [Windows] Gets console handle
        ''' </summary>
        ''' <param name="handle">Handle number</param>
        ''' <returns>True if succeeded, false if failed</returns>
        <DllImport("kernel32.dll", SetLastError:=True)>
        Private Function GetStdHandle(handle As Integer) As IntPtr
        End Function

        ''' <summary>
        ''' [Windows] Initializes 255 color support
        ''' </summary>
        Sub Initialize255()
            Dim handle = GetStdHandle(-11)
            Wdbg(DebugLevel.I, "Integer pointer {0}", handle)
            Dim mode As Integer
            GetConsoleMode(handle, mode)
            Wdbg(DebugLevel.I, "Mode: {0}", mode)
            If Not mode = 7 Then
                SetConsoleMode(handle, mode Or &H4)
                Wdbg(DebugLevel.I, "Added support for VT escapes.")
            End If
        End Sub

        ''' <summary>
        ''' A simplification for <see cref="Convert.ToChar(Integer)"/> function to return the ESC character
        ''' </summary>
        ''' <returns>ESC</returns>
        Public Function GetEsc() As Char
            Return Convert.ToChar(&H1B)
        End Function

    End Module
End Namespace
