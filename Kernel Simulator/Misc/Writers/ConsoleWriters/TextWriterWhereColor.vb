
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
    ''' <param name="Return">Whether or not to return to old position</param>
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhere(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal [Return] As Boolean, ByVal colorType As ColTypes, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out) Then
                If colorType = ColTypes.Neutral Or colorType = ColTypes.Input Then
                    Write(New Color(NeutralTextColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.Continuable Then
                    Write(New Color(ContKernelErrorColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.Uncontinuable Then
                    Write(New Color(UncontKernelErrorColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.HostName Then
                    Write(New Color(HostNameShellColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.UserName Then
                    Write(New Color(UserNameShellColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.License Then
                    Write(New Color(LicenseColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.Gray Then
                    If New Color(BackgroundColor).IsBright Then
                        Write(New Color(NeutralTextColor).VTSequenceForeground)
                    Else
                        Write(New Color(ConsoleColors.Gray).VTSequenceForeground)
                    End If
                ElseIf colorType = ColTypes.ListValue Then
                    Write(New Color(ListValueColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.ListEntry Then
                    Write(New Color(ListEntryColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.Stage Then
                    Write(New Color(StageColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.Error Then
                    Write(New Color(ErrorColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.Warning Then
                    Write(New Color(WarningColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.Option Then
                    Write(New Color(OptionColor).VTSequenceForeground)
                ElseIf colorType = ColTypes.Banner Then
                    Write(New Color(BannerColor).VTSequenceForeground)
                Else
                    Exit Sub
                End If
                Write(New Color(BackgroundColor).VTSequenceBackground)
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
            If [Return] Then SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then ResetColor()
            If colorType = ColTypes.Input And ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                SetInputColor()
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
    ''' <param name="Return">Whether or not to return to old position</param>
    ''' <param name="color">A color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereC16(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal [Return] As Boolean, ByVal color As ConsoleColor, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Console.BackgroundColor = IIf(IsNumeric(New Color(BackgroundColor).PlainSequence), If(BackgroundColor <= 15, [Enum].Parse(GetType(ConsoleColor), BackgroundColor), ConsoleColor.Black), ConsoleColor.Black)
            Console.ForegroundColor = color

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

            'Write text in another place
            Dim OldLeft As Integer = CursorLeft
            Dim OldTop As Integer = CursorTop
            SetCursorPosition(Left, Top)
            Write(msg)
            If [Return] Then SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then ResetColor()
            If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                SetInputColor()
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
    ''' <param name="Return">Whether or not to return to old position</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereC16(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal [Return] As Boolean, ByVal ForegroundColor As ConsoleColor, ByVal BackgroundColor As ConsoleColor, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Console.BackgroundColor = BackgroundColor
            Console.ForegroundColor = ForegroundColor

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

            'Write text in another place
            Dim OldLeft As Integer = CursorLeft
            Dim OldTop As Integer = CursorTop
            SetCursorPosition(Left, Top)
            Write(msg)
            If [Return] Then SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor = ConsoleColor.Black Then ResetColor()
            If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                SetInputColor()
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
    ''' <param name="Return">Whether or not to return to old position</param>
    ''' <param name="color">A color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereC(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal [Return] As Boolean, ByVal color As Color, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out) Then
                Write(color.VTSequenceForeground)
                Write(New Color(BackgroundColor).VTSequenceBackground)
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
            If [Return] Then SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then ResetColor()
            If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                SetInputColor()
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
    ''' <param name="Return">Whether or not to return to old position</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteWhereC(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal [Return] As Boolean, ByVal ForegroundColor As Color, ByVal BackgroundColor As Color, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out) Then
                Write(ForegroundColor.VTSequenceForeground)
                Write(BackgroundColor.VTSequenceBackground)
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
            If [Return] Then SetCursorPosition(OldLeft, OldTop)
            If BackgroundColor.PlainSequence = "0" Or BackgroundColor.PlainSequence = "0;0;0" Then ResetColor()
            If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                SetInputColor()
            End If
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

End Module
