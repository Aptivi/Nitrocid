
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

Imports KS.Misc.Reflection

Namespace Misc.Writers.ConsoleWriters
    Public Module TextWriterWrappedColor

        ''' <summary>
        ''' Outputs the text into the terminal prompt, wraps the long terminal output if needed.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWrappedPlain(Text As String, Line As Boolean, ParamArray vars() As Object)
            SyncLock WriteLock
                Dim LinesMade As Integer
                Dim OldTop As Integer
                Try
                    'Format string as needed
                    If Not vars.Length = 0 Then Text = FormatString(Text, vars)

                    OldTop = Console.CursorTop
                    For Each TextChar As Char In Text.ToString.ToCharArray
                        Console.Write(TextChar)
                        LinesMade += Console.CursorTop - OldTop
                        OldTop = Console.CursorTop
                        If LinesMade = Console.WindowHeight - 1 Then
                            If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                            LinesMade = 0
                        End If
                    Next
                    If Line Then Console.WriteLine()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt, wraps the long terminal output if needed, and sets colors as needed.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="colorType">A type of colors that will be changed.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWrapped(Text As String, Line As Boolean, colorType As ColTypes, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    SetConsoleColor(colorType)

                    'Write wrapped output
                    WriteWrappedPlain(Text, Line, vars)

                    'Reset the colors
                    If colorType = ColTypes.Input And ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt, wraps the long terminal output if needed, and sets colors as needed.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        ''' <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWrapped(Text As String, Line As Boolean, colorTypeForeground As ColTypes, colorTypeBackground As ColTypes, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    SetConsoleColor(colorTypeForeground)
                    SetConsoleColor(colorTypeBackground, True)

                    'Write wrapped output
                    WriteWrappedPlain(Text, Line, vars)

                    'Reset the colors
                    If colorTypeForeground = ColTypes.Input And ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="color">A color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWrapped(Text As String, Line As Boolean, color As ConsoleColor, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Try to write to console
                    Console.BackgroundColor = If(IsStringNumeric(BackgroundColor.PlainSequence) AndAlso BackgroundColor.PlainSequence <= 15, [Enum].Parse(GetType(ConsoleColor), BackgroundColor.PlainSequence), ConsoleColor.Black)
                    Console.ForegroundColor = color

                    'Write wrapped output
                    WriteWrappedPlain(Text, Line, vars)

                    'Reset the colors
                    If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWrapped(Text As String, Line As Boolean, ForegroundColor As ConsoleColor, BackgroundColor As ConsoleColor, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Try to write to console
                    Console.BackgroundColor = BackgroundColor
                    Console.ForegroundColor = ForegroundColor

                    'Write wrapped output
                    WriteWrappedPlain(Text, Line, vars)

                    'Reset the colors
                    If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="color">A color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWrapped(Text As String, Line As Boolean, color As Color, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Try to write to console
                    If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                        SetConsoleColor(color)
                        SetConsoleColor(BackgroundColor, True)
                    End If

                    'Write wrapped output
                    WriteWrappedPlain(Text, Line, vars)

                    'Reset the colors
                    If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWrapped(Text As String, Line As Boolean, ForegroundColor As Color, BackgroundColor As Color, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Try to write to console
                    If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                        SetConsoleColor(ForegroundColor)
                        SetConsoleColor(BackgroundColor, True)
                    End If

                    'Write wrapped output
                    WriteWrappedPlain(Text, Line, vars)

                    'Reset the colors
                    If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

    End Module
End Namespace