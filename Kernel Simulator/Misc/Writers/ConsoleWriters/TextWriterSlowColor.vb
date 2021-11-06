
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

Imports System.Threading

Module TextWriterSlowColor

    ''' <summary>
    ''' Outputs the text into the terminal prompt slowly with no color support.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
    ''' <param name="vars">Variables to format the message before it's written.</param>
    Public Sub WriteSlowly(msg As String, Line As Boolean, MsEachLetter As Double, ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text slowly
                Dim chars As List(Of Char) = msg.ToCharArray.ToList
                For Each ch As Char In chars
                    Thread.Sleep(MsEachLetter)
                    Console.Write(ch)
                Next
                If Line Then
                    Console.WriteLine()
                End If
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt slowly with color support.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <param name="vars">Variables to format the message before it's written.</param>
    Public Sub WriteSlowly(msg As String, Line As Boolean, MsEachLetter As Double, colorType As ColTypes, ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                SetConsoleColor(colorType)

                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text slowly
                Dim chars As List(Of Char) = msg.ToCharArray.ToList
                For Each ch As Char In chars
                    Thread.Sleep(MsEachLetter)
                    Console.Write(ch)
                Next
                If Line Then
                    Console.WriteLine()
                End If
                If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then Console.ResetColor()
                If colorType = ColTypes.Input And ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt slowly with color support.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
    ''' <param name="color">A color that will be changed to.</param>
    ''' <param name="vars">Variables to format the message before it's written.</param>
    Public Sub WriteSlowly(msg As String, Line As Boolean, MsEachLetter As Double, color As ConsoleColor, ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                Console.BackgroundColor = If(New Color(BackgroundColor).PlainSequence.IsNumeric AndAlso BackgroundColor <= 15, [Enum].Parse(GetType(ConsoleColor), BackgroundColor), ConsoleColor.Black)
                Console.ForegroundColor = color

                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text slowly
                Dim chars As List(Of Char) = msg.ToCharArray.ToList
                For Each ch As Char In chars
                    Thread.Sleep(MsEachLetter)
                    Console.Write(ch)
                Next
                If Line Then
                    Console.WriteLine()
                End If
                If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then Console.ResetColor()
                If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt slowly with color support.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="vars">Variables to format the message before it's written.</param>
    Public Sub WriteSlowly(msg As String, Line As Boolean, MsEachLetter As Double, ForegroundColor As ConsoleColor, BackgroundColor As ConsoleColor, ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                Console.BackgroundColor = BackgroundColor
                Console.ForegroundColor = ForegroundColor

                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text slowly
                Dim chars As List(Of Char) = msg.ToCharArray.ToList
                For Each ch As Char In chars
                    Thread.Sleep(MsEachLetter)
                    Console.Write(ch)
                Next
                If Line Then
                    Console.WriteLine()
                End If
                If BackgroundColor = ConsoleColor.Black Then Console.ResetColor()
                If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt slowly with color support.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
    ''' <param name="color">A color that will be changed to.</param>
    ''' <param name="vars">Variables to format the message before it's written.</param>
    Public Sub WriteSlowly(msg As String, Line As Boolean, MsEachLetter As Double, color As Color, ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                    SetConsoleColor(color)
                    SetConsoleColor(New Color(BackgroundColor), True)
                End If

                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text slowly
                Dim chars As List(Of Char) = msg.ToCharArray.ToList
                For Each ch As Char In chars
                    Thread.Sleep(MsEachLetter)
                    Console.Write(ch)
                Next
                If Line Then
                    Console.WriteLine()
                End If
                If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then Console.ResetColor()
                If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt slowly with color support.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="vars">Variables to format the message before it's written.</param>
    Public Sub WriteSlowly(msg As String, Line As Boolean, MsEachLetter As Double, ForegroundColor As Color, BackgroundColor As Color, ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out) Then
                    SetConsoleColor(ForegroundColor)
                    SetConsoleColor(BackgroundColor, True)
                End If

                'Format string as needed
                If Not vars.Length = 0 Then msg = String.Format(msg, vars)

                'Write text slowly
                Dim chars As List(Of Char) = msg.ToCharArray.ToList
                For Each ch As Char In chars
                    Thread.Sleep(MsEachLetter)
                    Console.Write(ch)
                Next
                If Line Then
                    Console.WriteLine()
                End If
                If BackgroundColor.PlainSequence = "0" Or BackgroundColor.PlainSequence = "0;0;0" Then Console.ResetColor()
                If ColoredShell And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Console.Out)) Then SetInputColor()
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

End Module
