
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

Imports System.IO

Namespace Misc.Writers.MiscWriters
    Public Module Decisive

        ''' <summary>
        ''' Decides where to write the text
        ''' </summary>
        ''' <param name="CommandType">A specified command type</param>
        ''' <param name="DebugDeviceSocket">Only for remote debug shell. Specifies the debug device socket.</param>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="colorType">A type of colors that will be changed.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub DecisiveWrite(CommandType As ShellType, DebugDeviceSocket As StreamWriter, Text As String, Line As Boolean, colorType As ColTypes, ParamArray vars() As Object)
            If Not CommandType = ShellType.RemoteDebugShell Then
                Write(Text, Line, colorType, vars)
            ElseIf DebugDeviceSocket IsNot Nothing Then
                If Line Then
                    DebugDeviceSocket.WriteLine(Text, vars)
                Else
                    DebugDeviceSocket.Write(Text, vars)
                End If
            End If
        End Sub

    End Module
End Namespace
