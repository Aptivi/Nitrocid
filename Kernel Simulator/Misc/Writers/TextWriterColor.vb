
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Imports System.Console

'This module is very important to reduce line numbers when there is color.
Module TextWriterColor

    ''' <summary>
    ''' Outputs the text into the terminal prompt, and sets colors as needed.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="colorType">A type of colors that will be changed. Any of neutralText, input, contError, uncontError, hostName, userName, def, or license.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    ''' <remarks>This is used to reduce number of lines containing "System.Console.ForegroundColor = " and "System.Console.ResetColor()" text.</remarks>
    Sub W(ByVal text As Object, ByVal colorType As String, ByVal ParamArray vars() As Object)

        On Error GoTo bug
        If (colorType = "neutralText") Then
            ForegroundColor = neutralTextColor
        ElseIf (colorType = "input") Then
            ForegroundColor = neutralTextColor
        ElseIf (colorType = "contError") Then
            ForegroundColor = contKernelErrorColor
        ElseIf (colorType = "uncontError") Then
            ForegroundColor = uncontKernelErrorColor
        ElseIf (colorType = "hostName") Then
            ForegroundColor = hostNameShellColor
        ElseIf (colorType = "userName") Then
            ForegroundColor = userNameShellColor
        ElseIf (colorType = "license") Then
            ForegroundColor = licenseColor
        ElseIf (colorType = "def") Then
            ForegroundColor = ConsoleColor.Gray
        Else
            Exit Sub
        End If
        Write(text, vars)
        If (Console.BackgroundColor = ConsoleColor.Black) Then
            ResetColor()
        End If
        If (colorType = "input") Then
            ForegroundColor = inputColor
        End If
        Exit Sub
bug:
        KernelError(CChar("C"), False, 0, "There is a serious error when printing text.")

    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt, sets colors as needed, and returns a new line.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="colorType">A type of colors that will be changed.  Any of neutralText, input, contError, uncontError, hostName, userName, def, or license.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    ''' <remarks>This is used to reduce number of lines containing "System.Console.ForegroundColor = " and "System.Console.ResetColor()" text.</remarks>
    Sub Wln(ByVal text As Object, ByVal colorType As String, ByVal ParamArray vars() As Object)

        On Error GoTo bug
        If (colorType = "neutralText") Then
            ForegroundColor = neutralTextColor
        ElseIf (colorType = "input") Then
            ForegroundColor = neutralTextColor
        ElseIf (colorType = "contError") Then
            ForegroundColor = contKernelErrorColor
        ElseIf (colorType = "uncontError") Then
            ForegroundColor = uncontKernelErrorColor
        ElseIf (colorType = "hostName") Then
            ForegroundColor = hostNameShellColor
        ElseIf (colorType = "userName") Then
            ForegroundColor = userNameShellColor
        ElseIf (colorType = "license") Then
            ForegroundColor = licenseColor
        ElseIf (colorType = "def") Then
            ForegroundColor = ConsoleColor.Gray
        Else
            Exit Sub
        End If
        WriteLine(text, vars)
        If (Console.BackgroundColor = ConsoleColor.Black) Then
            ResetColor()
        End If
        If (colorType = "input") Then
            ForegroundColor = inputColor
        End If
        Exit Sub
bug:
        KernelError(CChar("C"), False, 0, "There is a serious error when printing text.")

    End Sub

End Module
