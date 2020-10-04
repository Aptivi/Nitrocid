
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

Imports System.Console
Imports System.IO
Imports System.Threading

'This module is very important to reduce line numbers when there is color.
Public Module TextWriterColor

    ''' <summary>
    ''' Enumeration for color types
    ''' </summary>
    Public Enum ColTypes As Integer
        Neutral = 1
        Input = 2
        Continuable = 3
        Uncontinuable = 4
        HostName = 5
        UserName = 6
        License = 7
        Gray = 8
        HelpDef = 9
        HelpCmd = 10
        Stage = 11
        Err = 12
    End Enum

    ''' <summary>
    ''' Outputs the text into the terminal prompt, and sets colors as needed.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub W(ByVal text As Object, ByVal Line As Boolean, ByVal colorType As ColTypes, ByVal ParamArray vars() As Object)

        Dim esc As Char = GetEsc()
        Try
            'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
            If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                If colorType = ColTypes.Neutral Or colorType = ColTypes.Input Then
                    Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
                ElseIf colorType = ColTypes.Continuable Then
                    Write(esc + "[38;5;" + CStr(contKernelErrorColor) + "m")
                ElseIf colorType = ColTypes.Uncontinuable Then
                    Write(esc + "[38;5;" + CStr(uncontKernelErrorColor) + "m")
                ElseIf colorType = ColTypes.HostName Then
                    Write(esc + "[38;5;" + CStr(hostNameShellColor) + "m")
                ElseIf colorType = ColTypes.UserName Then
                    Write(esc + "[38;5;" + CStr(userNameShellColor) + "m")
                ElseIf colorType = ColTypes.License Then
                    Write(esc + "[38;5;" + CStr(licenseColor) + "m")
                ElseIf colorType = ColTypes.Gray Then
                    If backgroundColor = ConsoleColors.DarkYellow Or backgroundColor = ConsoleColors.Yellow Or backgroundColor = ConsoleColors.White Then
                        Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
                    Else
                        Write(esc + "[38;5;" + CStr(ConsoleColors.Gray) + "m")
                    End If
                ElseIf colorType = ColTypes.HelpDef Then
                    Write(esc + "[38;5;" + CStr(cmdDefColor) + "m")
                ElseIf colorType = ColTypes.HelpCmd Then
                    Write(esc + "[38;5;" + CStr(cmdListColor) + "m")
                ElseIf colorType = ColTypes.Stage Then
                    Write(esc + "[38;5;" + CStr(stageColor) + "m")
                ElseIf colorType = ColTypes.Err Then
                    Write(esc + "[38;5;" + CStr(errorColor) + "m")
                Else
                    Exit Sub
                End If
                Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            text = text.ToString.FormatString(vars)

            If Line Then WriteLine(text) Else Write(text)
            If backgroundColor = ConsoleColors.Black Then ResetColor()
            If colorType = ColTypes.Input And ColoredShell = True And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
                Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            End If
        Catch ex As Exception
            WStkTrc(ex)
            KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
        End Try

    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt slowly with no color support.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteSlowly(ByVal msg As String, ByVal Line As Boolean, ByVal MsEachLetter As Double, ParamArray ByVal vars() As Object)
        'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
        msg = msg.FormatString(vars)

        'Write text slowly
        Dim chars As List(Of Char) = msg.ToCharArray.ToList
        For Each ch As Char In chars
            Thread.Sleep(MsEachLetter)
            Write(ch)
        Next
        If Line Then
            WriteLine()
        End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt slowly with color support.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteSlowlyC(ByVal msg As String, ByVal Line As Boolean, ByVal MsEachLetter As Double, ByVal colorType As ColTypes, ParamArray ByVal vars() As Object)
        Dim esc As Char = GetEsc()
        If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
            If colorType = ColTypes.Neutral Or colorType = ColTypes.Input Then
                Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
            ElseIf colorType = ColTypes.Continuable Then
                Write(esc + "[38;5;" + CStr(contKernelErrorColor) + "m")
            ElseIf colorType = ColTypes.Uncontinuable Then
                Write(esc + "[38;5;" + CStr(uncontKernelErrorColor) + "m")
            ElseIf colorType = ColTypes.HostName Then
                Write(esc + "[38;5;" + CStr(hostNameShellColor) + "m")
            ElseIf colorType = ColTypes.UserName Then
                Write(esc + "[38;5;" + CStr(userNameShellColor) + "m")
            ElseIf colorType = ColTypes.License Then
                Write(esc + "[38;5;" + CStr(licenseColor) + "m")
            ElseIf colorType = ColTypes.Gray Then
                If backgroundColor = ConsoleColors.DarkYellow Or backgroundColor = ConsoleColors.Yellow Or backgroundColor = ConsoleColors.White Then
                    Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
                Else
                    Write(esc + "[38;5;" + CStr(ConsoleColors.Gray) + "m")
                End If
            ElseIf colorType = ColTypes.HelpDef Then
                Write(esc + "[38;5;" + CStr(cmdDefColor) + "m")
            ElseIf colorType = ColTypes.HelpCmd Then
                Write(esc + "[38;5;" + CStr(cmdListColor) + "m")
            ElseIf colorType = ColTypes.Stage Then
                Write(esc + "[38;5;" + CStr(stageColor) + "m")
            ElseIf colorType = ColTypes.Err Then
                Write(esc + "[38;5;" + CStr(errorColor) + "m")
            Else
                Exit Sub
            End If
            Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
        End If

        'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
        msg = msg.FormatString(vars)

        'Write text slowly
        Dim chars As List(Of Char) = msg.ToCharArray.ToList
        For Each ch As Char In chars
            Thread.Sleep(MsEachLetter)
            Write(ch)
        Next
        If Line Then
            WriteLine()
        End If
        If backgroundColor = ConsoleColors.Black Then ResetColor()
        If colorType = ColTypes.Input And ColoredShell = True And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
            Write(esc + "[38;5;" + CStr(inputColor) + "m")
            Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
        End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhere(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal colorType As ColTypes, ByVal ParamArray vars() As Object)
        Dim esc As Char = GetEsc()
        If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
            If colorType = ColTypes.Neutral Or colorType = ColTypes.Input Then
                Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
            ElseIf colorType = ColTypes.Continuable Then
                Write(esc + "[38;5;" + CStr(contKernelErrorColor) + "m")
            ElseIf colorType = ColTypes.Uncontinuable Then
                Write(esc + "[38;5;" + CStr(uncontKernelErrorColor) + "m")
            ElseIf colorType = ColTypes.HostName Then
                Write(esc + "[38;5;" + CStr(hostNameShellColor) + "m")
            ElseIf colorType = ColTypes.UserName Then
                Write(esc + "[38;5;" + CStr(userNameShellColor) + "m")
            ElseIf colorType = ColTypes.License Then
                Write(esc + "[38;5;" + CStr(licenseColor) + "m")
            ElseIf colorType = ColTypes.Gray Then
                If backgroundColor = ConsoleColors.DarkYellow Or backgroundColor = ConsoleColors.Yellow Or backgroundColor = ConsoleColors.White Then
                    Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
                Else
                    Write(esc + "[38;5;" + CStr(ConsoleColors.Gray) + "m")
                End If
            ElseIf colorType = ColTypes.HelpDef Then
                Write(esc + "[38;5;" + CStr(cmdDefColor) + "m")
            ElseIf colorType = ColTypes.HelpCmd Then
                Write(esc + "[38;5;" + CStr(cmdListColor) + "m")
            ElseIf colorType = ColTypes.Stage Then
                Write(esc + "[38;5;" + CStr(stageColor) + "m")
            ElseIf colorType = ColTypes.Err Then
                Write(esc + "[38;5;" + CStr(errorColor) + "m")
            Else
                Exit Sub
            End If
            Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
        End If

        'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
        msg = msg.FormatString(vars)

        'Write text in another place
        Dim OldLeft As Integer = CursorLeft
        Dim OldTop As Integer = CursorTop
        SetCursorPosition(Left, Top)
        Write(msg)
        SetCursorPosition(OldLeft, OldTop)
        If backgroundColor = ConsoleColors.Black Then ResetColor()
        If colorType = ColTypes.Input And ColoredShell = True And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
            Write(esc + "[38;5;" + CStr(inputColor) + "m")
            Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
        End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with custom color support.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="color">A color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteC(ByVal text As Object, ByVal Line As Boolean, ByVal color As ConsoleColors, ByVal ParamArray vars() As Object)

        Dim esc As Char = GetEsc()
        Try
            'Try to write to console
            If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                Write(esc + "[38;5;" + CStr(color) + "m")
                Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            text = text.ToString.FormatString(vars)

            If Line Then WriteLine(text) Else Write(text)
            If backgroundColor = ConsoleColors.Black Then ResetColor()
            If ColoredShell And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
                Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            End If
        Catch ex As Exception
            WStkTrc(ex)
            KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
        End Try

    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt, and sets the true colors as needed.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="ColorRGBFG">Foreground color RGB storage</param>
    ''' <param name="ColorRGBBG">Background color RGB storage</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteTrueColor(ByVal text As Object, ByVal Line As Boolean, ByVal ColorRGBFG As RGB, ByVal ColorRGBBG As RGB, ByVal ParamArray vars() As Object)

        Dim esc As Char = GetEsc()
        Try
            'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
            If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                Write(esc + "[38;2;" + ColorRGBFG.ToString + "m")
                Write(esc + "[48;2;" + ColorRGBBG.ToString + "m")
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            text = text.ToString.FormatString(vars)

            If Line Then WriteLine(text) Else Write(text)
            If backgroundColor = ConsoleColors.Black Then ResetColor()
            If ColoredShell And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
                Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            End If
        Catch ex As Exception
            WStkTrc(ex)
            KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
        End Try

    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt, and sets the true colors as needed.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="ColorRGB">Color RGB storage</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteTrueColor(ByVal text As Object, ByVal Line As Boolean, ByVal ColorRGB As RGB, ByVal ParamArray vars() As Object)

        Dim esc As Char = GetEsc()
        Try
            'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
            If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                Write(esc + "[38;2;" + ColorRGB.ToString + "m")
                Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            text = text.ToString.FormatString(vars)

            If Line Then WriteLine(text) Else Write(text)
            If backgroundColor = ConsoleColors.Black Then ResetColor()
            If ColoredShell And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
                Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            End If
        Catch ex As Exception
            WStkTrc(ex)
            KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
        End Try

    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets true colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="ColorRGBFG">Foreground color RGB storage</param>
    ''' <param name="ColorRGBBG">Background color RGB storage</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereTrueColor(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal ColorRGBFG As RGB, ByVal ColorRGBBG As RGB, ByVal ParamArray vars() As Object)
        Dim esc As Char = GetEsc()
        If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
            Write(esc + "[38;2;" + ColorRGBFG.ToString + "m")
            Write(esc + "[48;2;" + ColorRGBBG.ToString + "m")
        End If

        'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
        msg = msg.FormatString(vars)

        'Write text in another place
        Dim OldLeft As Integer = CursorLeft
        Dim OldTop As Integer = CursorTop
        SetCursorPosition(Left, Top)
        Write(msg)
        SetCursorPosition(OldLeft, OldTop)
        If backgroundColor = ConsoleColors.Black Then ResetColor()
        If ColoredShell = True And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
            Write(esc + "[38;5;" + CStr(inputColor) + "m")
            Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
        End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets true colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="ColorRGB">Color RGB storage</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereTrueColor(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal ColorRGB As RGB, ByVal ParamArray vars() As Object)
        Dim esc As Char = GetEsc()
        If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
            Write(esc + "[38;2;" + ColorRGB.ToString + "m")
            Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
        End If

        'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
        msg = msg.FormatString(vars)

        'Write text in another place
        Dim OldLeft As Integer = CursorLeft
        Dim OldTop As Integer = CursorTop
        SetCursorPosition(Left, Top)
        Write(msg)
        SetCursorPosition(OldLeft, OldTop)
        If backgroundColor = ConsoleColors.Black Then ResetColor()
        If ColoredShell = True And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
            Write(esc + "[38;5;" + CStr(inputColor) + "m")
            Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
        End If
    End Sub

End Module
