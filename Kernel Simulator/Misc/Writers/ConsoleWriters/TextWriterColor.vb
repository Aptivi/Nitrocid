
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
    Public Module TextWriterColor

        Friend WriteLock As New Object

        ''' <summary>
        ''' Outputs the text into the terminal prompt without colors
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WritePlain(Text As String, Line As Boolean, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Get the filtered positions first.
                    Dim FilteredLeft, FilteredTop As Integer
                    If Not Line And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then GetFilteredPositions(Text, FilteredLeft, FilteredTop, vars)

                    'Actually write
                    If Line Then
                        If Not vars.Length = 0 Then
                            Console.WriteLine(Text, vars)
                        Else
                            Console.WriteLine(Text)
                        End If
                    Else
                        If Not vars.Length = 0 Then
                            Console.Write(Text, vars)
                        Else
                            Console.Write(Text)
                        End If
                    End If

                    'Return to the processed position
                    If Not Line And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then Console.SetCursorPosition(FilteredLeft, FilteredTop)
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt, and sets colors as needed.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="colorType">A type of colors that will be changed.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub Write(Text As String, Line As Boolean, colorType As ColTypes, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    SetConsoleColor(colorType)

                    'Write the text to console
                    WritePlain(Text, Line, vars)

                    'Reset the colors
                    If colorType = ColTypes.Input And ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt, and sets colors as needed.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        ''' <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub Write(Text As String, Line As Boolean, colorTypeForeground As ColTypes, colorTypeBackground As ColTypes, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    SetConsoleColor(colorTypeForeground)
                    SetConsoleColor(colorTypeBackground, True)

                    'Write the text to console
                    WritePlain(Text, Line, vars)

                    'Reset the colors
                    If colorTypeForeground = ColTypes.Input And ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="color">A color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub Write(Text As String, Line As Boolean, color As ConsoleColor, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Try to write to console
                    Console.BackgroundColor = If(IsStringNumeric(BackgroundColor.PlainSequence) AndAlso BackgroundColor.PlainSequence <= 15, [Enum].Parse(GetType(ConsoleColor), BackgroundColor.PlainSequence), ConsoleColor.Black)
                    Console.ForegroundColor = color

                    'Write the text to console
                    WritePlain(Text, Line, vars)

                    'Reset the colors
                    If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub Write(Text As String, Line As Boolean, ForegroundColor As ConsoleColor, BackgroundColor As ConsoleColor, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Try to write to console
                    Console.BackgroundColor = BackgroundColor
                    Console.ForegroundColor = ForegroundColor

                    'Write the text to console
                    WritePlain(Text, Line, vars)

                    'Reset the colors
                    If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="color">A color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub Write(Text As String, Line As Boolean, color As Color, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Try to write to console
                    If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                        SetConsoleColor(color)
                        SetConsoleColor(BackgroundColor, True)
                    End If

                    'Write the text to console
                    WritePlain(Text, Line, vars)

                    'Reset the colors
                    If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub Write(Text As String, Line As Boolean, ForegroundColor As Color, BackgroundColor As Color, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Try to write to console
                    If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                        SetConsoleColor(ForegroundColor)
                        SetConsoleColor(BackgroundColor, True)
                    End If

                    'Write the text to console
                    WritePlain(Text, Line, vars)

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
