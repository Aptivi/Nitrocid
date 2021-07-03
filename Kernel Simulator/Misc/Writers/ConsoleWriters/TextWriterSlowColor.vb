
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

Module TextWriterSlowColor

    ''' <summary>
    ''' Outputs the text into the terminal prompt slowly with no color support.
    ''' </summary>
    ''' <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="MsEachLetter">Time in milliseconds to delay writing</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteSlowly(ByVal msg As String, ByVal Line As Boolean, ByVal MsEachLetter As Double, ParamArray ByVal vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

            'Write text slowly
            Dim chars As List(Of Char) = msg.ToCharArray.ToList
            For Each ch As Char In chars
                Thread.Sleep(MsEachLetter)
                Write(ch)
            Next
            If Line Then
                WriteLine()
            End If
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
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteSlowlyC(ByVal msg As String, ByVal Line As Boolean, ByVal MsEachLetter As Double, ByVal colorType As ColTypes, ParamArray ByVal vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
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

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

            'Write text slowly
            Dim chars As List(Of Char) = msg.ToCharArray.ToList
            For Each ch As Char In chars
                Thread.Sleep(MsEachLetter)
                Write(ch)
            Next
            If Line Then
                WriteLine()
            End If
            If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then ResetColor()
            If colorType = ColTypes.Input And ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                SetInputColor()
            End If
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
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteSlowlyC16(ByVal msg As String, ByVal Line As Boolean, ByVal MsEachLetter As Double, ByVal color As ConsoleColor, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Console.BackgroundColor = If(New Color(BackgroundColor).PlainSequence.IsNumeric AndAlso BackgroundColor <= 15, [Enum].Parse(GetType(ConsoleColor), BackgroundColor), ConsoleColor.Black)
            Console.ForegroundColor = color

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

            'Write text slowly
            Dim chars As List(Of Char) = msg.ToCharArray.ToList
            For Each ch As Char In chars
                Thread.Sleep(MsEachLetter)
                Write(ch)
            Next
            If Line Then
                WriteLine()
            End If
            If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then ResetColor()
            If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                SetInputColor()
            End If
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
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteSlowlyC16(ByVal msg As String, ByVal Line As Boolean, ByVal MsEachLetter As Double, ByVal ForegroundColor As ConsoleColor, ByVal BackgroundColor As ConsoleColor, ByVal ParamArray vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Console.BackgroundColor = BackgroundColor
            Console.ForegroundColor = ForegroundColor

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

            'Write text slowly
            Dim chars As List(Of Char) = msg.ToCharArray.ToList
            For Each ch As Char In chars
                Thread.Sleep(MsEachLetter)
                Write(ch)
            Next
            If Line Then
                WriteLine()
            End If
            If BackgroundColor = ConsoleColor.Black Then ResetColor()
            If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                SetInputColor()
            End If
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
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteSlowlyC(ByVal msg As String, ByVal Line As Boolean, ByVal MsEachLetter As Double, ByVal color As Color, ParamArray ByVal vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out) Then
                SetConsoleColor(color)
                SetConsoleColor(New Color(BackgroundColor), True)
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

            'Write text slowly
            Dim chars As List(Of Char) = msg.ToCharArray.ToList
            For Each ch As Char In chars
                Thread.Sleep(MsEachLetter)
                Write(ch)
            Next
            If Line Then
                WriteLine()
            End If
            If BackgroundColor = New Color(ConsoleColors.Black).PlainSequence Or BackgroundColor = "0;0;0" Then ResetColor()
            If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                SetInputColor()
            End If
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
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    Public Sub WriteSlowlyC(ByVal msg As String, ByVal Line As Boolean, ByVal MsEachLetter As Double, ByVal ForegroundColor As Color, ByVal BackgroundColor As Color, ParamArray ByVal vars() As Object)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            If DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out) Then
                SetConsoleColor(ForegroundColor)
                SetConsoleColor(BackgroundColor, True)
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            If msg IsNot Nothing Then
                msg = msg.ToString.FormatString(vars)
            End If

            'Write text slowly
            Dim chars As List(Of Char) = msg.ToCharArray.ToList
            For Each ch As Char In chars
                Thread.Sleep(MsEachLetter)
                Write(ch)
            Next
            If Line Then
                WriteLine()
            End If
            If BackgroundColor.PlainSequence = "0" Or BackgroundColor.PlainSequence = "0;0;0" Then ResetColor()
            If ColoredShell = True And (DefConsoleOut Is Nothing Or Equals(DefConsoleOut, Out)) Then
                SetInputColor()
            End If
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

End Module
