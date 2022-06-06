
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

Imports System.Threading
Imports KS.Misc.Reflection

Namespace Misc.Writers.ConsoleWriters
    Public Module TextWriterWhereSlowColor

        ''' <summary>
        ''' Outputs the text into the terminal prompt with location support.
        ''' </summary>
        ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="Left">Column number in console</param>
        ''' <param name="Top">Row number in console</param>
        ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        ''' <param name="Return">Whether or not to return to old position</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWhereSlowlyPlain(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Format string as needed
                    If Not vars.Length = 0 Then msg = FormatString(msg, vars)

                    'Write text in another place slowly
                    Dim OldLeft As Integer = Console.CursorLeft
                    Dim OldTop As Integer = Console.CursorTop
                    Dim Paragraphs() As String = msg.SplitNewLines
                    Console.SetCursorPosition(Left, Top)
                    For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                        'We can now check to see if we're writing a letter past the console window width
                        Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                        For Each ParagraphChar As Char In MessageParagraph
                            Thread.Sleep(MsEachLetter)
                            If Console.CursorLeft = Console.WindowWidth Then
                                Console.CursorTop += 1
                                Console.CursorLeft = Left
                            End If
                            Console.Write(ParagraphChar)
                            If Line Then Console.WriteLine()
                        Next

                        'We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                            Console.CursorTop += 1
                            Console.CursorLeft = Left
                        End If
                    Next
                    If [Return] Then Console.SetCursorPosition(OldLeft, OldTop)
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
        ''' </summary>
        ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="Left">Column number in console</param>
        ''' <param name="Top">Row number in console</param>
        ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        ''' <param name="Return">Whether or not to return to old position</param>
        ''' <param name="colorType">A type of colors that will be changed.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWhereSlowly(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, colorType As ColTypes, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    SetConsoleColor(colorType)

                    'Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, [Return], vars)

                    'Reset the colors
                    If BackgroundColor.PlainSequence = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor.PlainSequence = "0;0;0" Then Console.ResetColor()
                    If colorType = ColTypes.Input And ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
        ''' </summary>
        ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="Left">Column number in console</param>
        ''' <param name="Top">Row number in console</param>
        ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        ''' <param name="Return">Whether or not to return to old position</param>
        ''' <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        ''' <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWhereSlowly(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, colorTypeForeground As ColTypes, colorTypeBackground As ColTypes, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    SetConsoleColor(colorTypeForeground)
                    SetConsoleColor(colorTypeBackground, True)

                    'Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, [Return], vars)

                    'Reset the colors
                    If BackgroundColor.PlainSequence = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor.PlainSequence = "0;0;0" Then Console.ResetColor()
                    If colorTypeForeground = ColTypes.Input And ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
        ''' </summary>
        ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="Left">Column number in console</param>
        ''' <param name="Top">Row number in console</param>
        ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        ''' <param name="Return">Whether or not to return to old position</param>
        ''' <param name="color">A color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWhereSlowly(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, color As ConsoleColor, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    Console.BackgroundColor = If(IsStringNumeric(BackgroundColor.PlainSequence) AndAlso BackgroundColor.PlainSequence <= 15, [Enum].Parse(GetType(ConsoleColor), BackgroundColor.PlainSequence), ConsoleColor.Black)
                    Console.ForegroundColor = color

                    'Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, [Return], vars)

                    'Reset the colors
                    If BackgroundColor.PlainSequence = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor.PlainSequence = "0;0;0" Then Console.ResetColor()
                    If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
        ''' </summary>
        ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="Left">Column number in console</param>
        ''' <param name="Top">Row number in console</param>
        ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        ''' <param name="Return">Whether or not to return to old position</param>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWhereSlowly(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, ForegroundColor As ConsoleColor, BackgroundColor As ConsoleColor, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    Console.BackgroundColor = BackgroundColor
                    Console.ForegroundColor = ForegroundColor

                    'Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, [Return], vars)

                    'Reset the colors
                    If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Then Console.ResetColor()
                    If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
        ''' </summary>
        ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="Left">Column number in console</param>
        ''' <param name="Top">Row number in console</param>
        ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        ''' <param name="Return">Whether or not to return to old position</param>
        ''' <param name="color">A color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWhereSlowly(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, color As Color, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                        SetConsoleColor(color)
                        SetConsoleColor(BackgroundColor, True)
                    End If

                    'Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, [Return], vars)

                    'Reset the colors
                    If BackgroundColor.PlainSequence = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor.PlainSequence = "0;0;0" Then Console.ResetColor()
                    If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with location support, and sets colors as needed.
        ''' </summary>
        ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        ''' <param name="Line">Whether to print a new line or not</param>
        ''' <param name="Left">Column number in console</param>
        ''' <param name="Top">Row number in console</param>
        ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        ''' <param name="Return">Whether or not to return to old position</param>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        ''' <param name="vars">Variables to format the message before it's written.</param>
        Public Sub WriteWhereSlowly(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, ForegroundColor As Color, BackgroundColor As Color, ParamArray vars() As Object)
            SyncLock WriteLock
                Try
                    If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                        SetConsoleColor(ForegroundColor)
                        SetConsoleColor(BackgroundColor, True)
                    End If

                    'Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, [Return], vars)

                    'Reset the colors
                    If BackgroundColor.PlainSequence = "0" Or BackgroundColor.PlainSequence = "0;0;0" Then Console.ResetColor()
                    If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

    End Module
End Namespace
