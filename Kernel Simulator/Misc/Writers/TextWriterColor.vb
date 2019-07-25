
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

Imports System.Console
Imports System.IO

'This module is very important to reduce line numbers when there is color.
Public Module TextWriterColor

    Public dbgWriter As StreamWriter = New StreamWriter(paths("Debugging"), True) With {.AutoFlush = True}

    ''' <summary>
    ''' Outputs the text into the debugger file, and sets the time stamp.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    ''' <remarks></remarks>
    Public Sub Wdbg(ByVal text As String, ByVal ParamArray vars() As Object)
        If DebugMode = True Then
            Dim STrace As New StackTrace(True)
            Dim Source As String = Path.GetFileName(STrace.GetFrame(1).GetFileName)
            Dim LineNum As String = STrace.GetFrame(1).GetFileLineNumber
            If Not Source Is Nothing And Not LineNum = 0 Then
                dbgWriter.WriteLine($"{FormatDateTime(KernelDateTime, DateFormat.ShortDate)} {FormatDateTime(KernelDateTime, DateFormat.ShortTime)} ({Source}:{LineNum}): {text}", vars)
                Debug.WriteLine($"{FormatDateTime(KernelDateTime, DateFormat.ShortDate)} {FormatDateTime(KernelDateTime, DateFormat.ShortTime)} ({Source}:{LineNum}): {text}", vars)
            Else
                dbgWriter.WriteLine($"{FormatDateTime(KernelDateTime, DateFormat.ShortDate)} {FormatDateTime(KernelDateTime, DateFormat.ShortTime)}: {text}", vars)
                Debug.WriteLine($"{FormatDateTime(KernelDateTime, DateFormat.ShortDate)} {FormatDateTime(KernelDateTime, DateFormat.ShortTime)}: {text}", vars)
            End If
        End If
    End Sub

    'TODO: Merge line and without line writes into unified "W" sub
    ''' <summary>
    ''' Outputs the text into the terminal prompt, and sets colors as needed.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="colorType">A type of colors that will be changed. Any of neutralText, input, contError, uncontError, hostName, userName, def, helpCmd, helpDef, or license.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    ''' <remarks>This is used to reduce number of lines containing "System.Console.ForegroundColor = " and "System.Console.ResetColor()" text.</remarks>
    Public Sub W(ByVal text As Object, ByVal colorType As String, ByVal ParamArray vars() As Object)

        Try
            If colorType = "neutralText" Or colorType = "input" Then
                ForegroundColor = neutralTextColor
            ElseIf colorType = "contError" Then
                ForegroundColor = contKernelErrorColor
            ElseIf colorType = "uncontError" Then
                ForegroundColor = uncontKernelErrorColor
            ElseIf colorType = "hostName" Then
                ForegroundColor = hostNameShellColor
            ElseIf colorType = "userName" Then
                ForegroundColor = userNameShellColor
            ElseIf colorType = "license" Then
                ForegroundColor = licenseColor
            ElseIf colorType = "def" Then
                ForegroundColor = ConsoleColor.Gray
            ElseIf colorType = "helpDef" Then
                ForegroundColor = cmdDefColor
            ElseIf colorType = "helpCmd" Then
                ForegroundColor = cmdListColor
            Else
                Exit Sub
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            For v As Integer = 0 To vars.Length - 1
                text = text.Replace("{" + CStr(v) + "}", vars(v).ToString)
            Next

            Write(text)
            If Console.BackgroundColor = ConsoleColor.Black Then ResetColor()
            If colorType = "input" And ColoredShell = True Then ForegroundColor = inputColor
        Catch ex As Exception
            KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
        End Try

    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt, sets colors as needed, and returns a new line.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="colorType">A type of colors that will be changed.  Any of neutralText, input, contError, uncontError, hostName, userName, def, helpCmd, helpDef, or license.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    ''' <remarks>This is used to reduce number of lines containing "System.Console.ForegroundColor = " and "System.Console.ResetColor()" text.</remarks>
    Public Sub Wln(ByVal text As Object, ByVal colorType As String, ByVal ParamArray vars() As Object)

        Try
            If colorType = "neutralText" Or colorType = "input" Then
                ForegroundColor = neutralTextColor
            ElseIf colorType = "contError" Then
                ForegroundColor = contKernelErrorColor
            ElseIf colorType = "uncontError" Then
                ForegroundColor = uncontKernelErrorColor
            ElseIf colorType = "hostName" Then
                ForegroundColor = hostNameShellColor
            ElseIf colorType = "userName" Then
                ForegroundColor = userNameShellColor
            ElseIf colorType = "license" Then
                ForegroundColor = licenseColor
            ElseIf colorType = "def" Then
                ForegroundColor = ConsoleColor.Gray
            ElseIf colorType = "helpDef" Then
                ForegroundColor = cmdDefColor
            ElseIf colorType = "helpCmd" Then
                ForegroundColor = cmdListColor
            Else
                Exit Sub
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            For v As Integer = 0 To vars.Length - 1
                text = text.Replace("{" + CStr(v) + "}", vars(v).ToString)
            Next

            WriteLine(text)
            If Console.BackgroundColor = ConsoleColor.Black Then ResetColor()
            If colorType = "input" Then ForegroundColor = inputColor
        Catch ex As Exception
            KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
        End Try

    End Sub

End Module
