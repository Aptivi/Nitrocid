
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

Imports System.Console

Public Module TextWriterColor

#If Not NOWRITELOCK Then
    Friend WriteLock As New Object
#End If

    ''' <summary>
    ''' Outputs the text into the terminal prompt, and sets colors as needed.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub W(ByVal text As Object, ByVal Line As Boolean, ByVal colorType As ColTypes, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
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
                    ElseIf colorType = ColTypes.Warning Then
                        Write(esc + "[38;5;" + CStr(WarningColor) + "m")
                    ElseIf colorType = ColTypes.Option Then
                        Write(esc + "[38;5;" + CStr(OptionColor) + "m")
                    Else
                        Exit Sub
                    End If
                    Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                End If

                'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
                If text IsNot Nothing Then
                    text = text.ToString.FormatString(vars)
                End If

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
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with custom color support.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="color">A color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteC16(ByVal text As Object, ByVal Line As Boolean, ByVal color As ConsoleColor, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            Try
                'Try to write to console
                Console.BackgroundColor = If(backgroundColor <= 15, [Enum].Parse(GetType(ConsoleColor), backgroundColor), ConsoleColor.Black)
                Console.ForegroundColor = color

                'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
                If text IsNot Nothing Then
                    text = text.ToString.FormatString(vars)
                End If

                If Line Then WriteLine(text) Else Write(text)
                If backgroundColor = ConsoleColors.Black Then ResetColor()
                If ColoredShell = True And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
                    Console.BackgroundColor = If(backgroundColor <= 15, [Enum].Parse(GetType(ConsoleColor), backgroundColor), ConsoleColor.Black)
                    Console.ForegroundColor = If(inputColor <= 15, [Enum].Parse(GetType(ConsoleColor), inputColor), ConsoleColor.White)
                End If
            Catch ex As Exception
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with custom color support.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteC16(ByVal text As Object, ByVal Line As Boolean, ByVal ForegroundColor As ConsoleColors, ByVal BackgroundColor As ConsoleColors, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            Try
                'Try to write to console
                Console.BackgroundColor = BackgroundColor
                Console.ForegroundColor = ForegroundColor

                'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
                If text IsNot Nothing Then
                    text = text.ToString.FormatString(vars)
                End If

                If Line Then WriteLine(text) Else Write(text)
                If BackgroundColor = ConsoleColors.Black Then ResetColor()
                If ColoredShell = True And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
                    Console.BackgroundColor = If(BackgroundColor <= 15, [Enum].Parse(GetType(ConsoleColor), BackgroundColor), ConsoleColor.Black)
                    Console.ForegroundColor = If(inputColor <= 15, [Enum].Parse(GetType(ConsoleColor), inputColor), ConsoleColor.White)
                End If
            Catch ex As Exception
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with custom color support.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="color">A color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteC(ByVal text As Object, ByVal Line As Boolean, ByVal color As ConsoleColors, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            Try
                'Try to write to console
                If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                    Write(esc + "[38;5;" + CStr(color) + "m")
                    Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                End If

                'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
                If text IsNot Nothing Then
                    text = text.ToString.FormatString(vars)
                End If

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
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with custom color support.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteC(ByVal text As Object, ByVal Line As Boolean, ByVal ForegroundColor As ConsoleColors, ByVal BackgroundColor As ConsoleColors, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            Try
                'Try to write to console
                If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                    Write(esc + "[38;5;" + CStr(ForegroundColor) + "m")
                    Write(esc + "[48;5;" + CStr(BackgroundColor) + "m")
                End If

                'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
                If text IsNot Nothing Then
                    text = text.ToString.FormatString(vars)
                End If

                If Line Then WriteLine(text) Else Write(text)
                If BackgroundColor = ConsoleColors.Black Then ResetColor()
                If ColoredShell And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
                    Write(esc + "[38;5;" + CStr(inputColor) + "m")
                    Write(esc + "[48;5;" + CStr(Color.backgroundColor) + "m")
                End If
            Catch ex As Exception
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
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
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            Try
                'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                    Write(esc + "[38;2;" + ColorRGBFG.ToString + "m")
                    Write(esc + "[48;2;" + ColorRGBBG.ToString + "m")
                End If

                'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
                If text IsNot Nothing Then
                    text = text.ToString.FormatString(vars)
                End If

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
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt, and sets the true colors as needed.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="ColorRGB">Color RGB storage</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteTrueColor(ByVal text As Object, ByVal Line As Boolean, ByVal ColorRGB As RGB, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            Try
                'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                    Write(esc + "[38;2;" + ColorRGB.ToString + "m")
                    Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                End If

                'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
                If text IsNot Nothing Then
                    text = text.ToString.FormatString(vars)
                End If

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
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

End Module
