
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

Module TextWriterWhereColor

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhere(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal colorType As ColTypes, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
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
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

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
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="color">A color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereC(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal color As ConsoleColors, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                Write(esc + "[38;5;" + CStr(color) + "m")
                Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

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
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Left">Column number in console</param>
    ''' <param name="Top">Row number in console</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereC(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal ForegroundColor As ConsoleColors, ByVal BackgroundColor As ConsoleColors, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                Write(esc + "[38;5;" + CStr(ForegroundColor) + "m")
                Write(esc + "[48;5;" + CStr(BackgroundColor) + "m")
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

            'Write text in another place
            Dim OldLeft As Integer = CursorLeft
            Dim OldTop As Integer = CursorTop
            SetCursorPosition(Left, Top)
            Write(msg)
            SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor = ConsoleColors.Black Then ResetColor()
            If ColoredShell = True And (IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out)) Then
                Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Write(esc + "[48;5;" + CStr(Color.backgroundColor) + "m")
            End If
#If Not NOWRITELOCK Then
        End SyncLock
#End If
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
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                Write(esc + "[38;2;" + ColorRGBFG.ToString + "m")
                Write(esc + "[48;2;" + ColorRGBBG.ToString + "m")
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

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
#If Not NOWRITELOCK Then
        End SyncLock
#End If
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
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            If IsNothing(DefConsoleOut) Or Equals(DefConsoleOut, Out) Then
                Write(esc + "[38;2;" + ColorRGB.ToString + "m")
                Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

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
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

End Module
