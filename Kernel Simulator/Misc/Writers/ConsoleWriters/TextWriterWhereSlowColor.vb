
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
Imports System.Threading

Module TextWriterWhereSlowColor

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
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out) Then
                    If colorType = ColTypes.Neutral Or colorType = ColTypes.Input Then
                        SetConsoleColor(New Color(NeutralTextColor))
                    ElseIf colorType = ColTypes.Continuable Then
                        SetConsoleColor(New Color(ContKernelErrorColor))
                    ElseIf colorType = ColTypes.Uncontinuable Then
                        SetConsoleColor(New Color(UncontKernelErrorColor))
                    ElseIf colorType = ColTypes.HostName Then
                        SetConsoleColor(New Color(HostNameShellColor))
                    ElseIf colorType = ColTypes.UserName Then
                        SetConsoleColor(New Color(UserNameShellColor))
                    ElseIf colorType = ColTypes.License Then
                        SetConsoleColor(New Color(LicenseColor))
                    ElseIf colorType = ColTypes.Gray Then
                        If New Color(BackgroundColor).IsBright Then
                            SetConsoleColor(New Color(NeutralTextColor))
                        Else
                            SetConsoleColor(New Color(ConsoleColors.Gray))
                        End If
                    ElseIf colorType = ColTypes.ListValue Then
                        SetConsoleColor(New Color(ListValueColor))
                    ElseIf colorType = ColTypes.ListEntry Then
                        SetConsoleColor(New Color(ListEntryColor))
                    ElseIf colorType = ColTypes.Stage Then
                        SetConsoleColor(New Color(StageColor))
                    ElseIf colorType = ColTypes.Error Then
                        SetConsoleColor(New Color(ErrorColor))
                    ElseIf colorType = ColTypes.Warning Then
                        SetConsoleColor(New Color(WarningColor))
                    ElseIf colorType = ColTypes.Option Then
                        SetConsoleColor(New Color(OptionColor))
                    ElseIf colorType = ColTypes.Banner Then
                        SetConsoleColor(New Color(BannerColor))
                    Else
                        Exit Sub
                    End If
                    SetConsoleColor(New Color(BackgroundColor), True)
                End If

                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text in another place slowly
                Dim OldLeft As Integer = CursorLeft
                Dim OldTop As Integer = CursorTop
                Dim Paragraphs() As String = msg.SplitNewLines
                SetCursorPosition(Left, Top)
                For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                    'We can now check to see if we're writing a letter past the console window width
                    Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                    For Each ParagraphChar As Char In MessageParagraph
                        Thread.Sleep(MsEachLetter)
                        If CursorLeft = WindowWidth - 1 Then
                            CursorTop += 1
                            CursorLeft = Left
                        End If
                        Write(ParagraphChar)
                        If Line Then WriteLine()
                    Next

                    'We're starting with the new paragraph, so we increase the CursorTop value by 1.
                    If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                        CursorTop += 1
                        CursorLeft = Left
                    End If
                Next
                If [Return] Then SetCursorPosition(OldLeft, OldTop)
                If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then ResetColor()
                If colorType = ColTypes.Input And ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                    SetInputColor()
                End If
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
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
    Public Sub WriteWhereSlowlyC16(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, color As ConsoleColor, ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                Console.BackgroundColor = If(New Color(BackgroundColor).PlainSequence.IsNumeric AndAlso BackgroundColor <= 15, [Enum].Parse(GetType(ConsoleColor), BackgroundColor), ConsoleColor.Black)
                Console.ForegroundColor = color

                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text in another place slowly
                Dim OldLeft As Integer = CursorLeft
                Dim OldTop As Integer = CursorTop
                Dim Paragraphs() As String = msg.SplitNewLines
                SetCursorPosition(Left, Top)
                For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                    'We can now check to see if we're writing a letter past the console window width
                    Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                    For Each ParagraphChar As Char In MessageParagraph
                        Thread.Sleep(MsEachLetter)
                        If CursorLeft = WindowWidth - 1 Then
                            CursorTop += 1
                            CursorLeft = Left
                        End If
                        Write(ParagraphChar)
                        If Line Then WriteLine()
                    Next

                    'We're starting with the new paragraph, so we increase the CursorTop value by 1.
                    If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                        CursorTop += 1
                        CursorLeft = Left
                    End If
                Next
                If [Return] Then SetCursorPosition(OldLeft, OldTop)
                If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then ResetColor()
                If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                    SetInputColor()
                End If
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
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
    Public Sub WriteWhereSlowlyC16(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, ForegroundColor As ConsoleColor, BackgroundColor As ConsoleColor, ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                Console.BackgroundColor = BackgroundColor
                Console.ForegroundColor = ForegroundColor

                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text in another place slowly
                Dim OldLeft As Integer = CursorLeft
                Dim OldTop As Integer = CursorTop
                Dim Paragraphs() As String = msg.SplitNewLines
                SetCursorPosition(Left, Top)
                For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                    'We can now check to see if we're writing a letter past the console window width
                    Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                    For Each ParagraphChar As Char In MessageParagraph
                        Thread.Sleep(MsEachLetter)
                        If CursorLeft = WindowWidth - 1 Then
                            CursorTop += 1
                            CursorLeft = Left
                        End If
                        Write(ParagraphChar)
                        If Line Then WriteLine()
                    Next

                    'We're starting with the new paragraph, so we increase the CursorTop value by 1.
                    If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                        CursorTop += 1
                        CursorLeft = Left
                    End If
                Next
                If [Return] Then SetCursorPosition(OldLeft, OldTop)
                If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Then ResetColor()
                If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                    SetInputColor()
                End If
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
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
    Public Sub WriteWhereSlowlyC(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, color As Color, ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out) Then
                    SetConsoleColor(color)
                    SetConsoleColor(New Color(BackgroundColor), True)
                End If

                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text in another place slowly
                Dim OldLeft As Integer = CursorLeft
                Dim OldTop As Integer = CursorTop
                Dim Paragraphs() As String = msg.SplitNewLines
                SetCursorPosition(Left, Top)
                For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                    'We can now check to see if we're writing a letter past the console window width
                    Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                    For Each ParagraphChar As Char In MessageParagraph
                        Thread.Sleep(MsEachLetter)
                        If CursorLeft = WindowWidth - 1 Then
                            CursorTop += 1
                            CursorLeft = Left
                        End If
                        Write(ParagraphChar)
                        If Line Then WriteLine()
                    Next

                    'We're starting with the new paragraph, so we increase the CursorTop value by 1.
                    If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                        CursorTop += 1
                        CursorLeft = Left
                    End If
                Next
                If [Return] Then SetCursorPosition(OldLeft, OldTop)
                If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then ResetColor()
                If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                    SetInputColor()
                End If
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
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
    Public Sub WriteWhereSlowlyC(msg As String, Line As Boolean, Left As Integer, Top As Integer, MsEachLetter As Double, [Return] As Boolean, ForegroundColor As Color, BackgroundColor As Color, ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out) Then
                    SetConsoleColor(ForegroundColor)
                    SetConsoleColor(BackgroundColor, True)
                End If

                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text in another place slowly
                Dim OldLeft As Integer = CursorLeft
                Dim OldTop As Integer = CursorTop
                Dim Paragraphs() As String = msg.SplitNewLines
                SetCursorPosition(Left, Top)
                For MessageParagraphIndex As Integer = 0 To Paragraphs.Length - 1
                    'We can now check to see if we're writing a letter past the console window width
                    Dim MessageParagraph As String = Paragraphs(MessageParagraphIndex)
                    For Each ParagraphChar As Char In MessageParagraph
                        Thread.Sleep(MsEachLetter)
                        If CursorLeft = WindowWidth - 1 Then
                            CursorTop += 1
                            CursorLeft = Left
                        End If
                        Write(ParagraphChar)
                        If Line Then WriteLine()
                    Next

                    'We're starting with the new paragraph, so we increase the CursorTop value by 1.
                    If Not MessageParagraphIndex = Paragraphs.Length - 1 Then
                        CursorTop += 1
                        CursorLeft = Left
                    End If
                Next
                If [Return] Then SetCursorPosition(OldLeft, OldTop)
                If BackgroundColor.PlainSequence = "0" Or BackgroundColor.PlainSequence = "0;0;0" Then ResetColor()
                If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                    SetInputColor()
                End If
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

End Module
