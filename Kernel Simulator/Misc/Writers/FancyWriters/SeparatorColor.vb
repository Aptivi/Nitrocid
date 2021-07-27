
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

Public Module SeparatorColor

    ''' <summary>
    ''' Draw a separator with text
    ''' </summary>
    ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
    ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
    ''' <param name="ColTypes">A type of colors that will be changed.</param>
    ''' <param name="Vars">Variables to format the message before it's written.</param>
    Public Sub WriteSeparator(ByVal Text As String, ByVal PrintSuffix As Boolean, ByVal ColTypes As ColTypes, ParamArray Vars() As Object)
        'Print the suffix and the text
        If Not String.IsNullOrWhiteSpace(Text) Then
            If PrintSuffix Then Text = "- " + Text
            W(Text.Truncate(Console.WindowWidth - 6) + " ", False, ColTypes, Vars)
        End If

        'See how many times to repeat the closing minus sign. We could be running this in the wrap command.
        Dim RepeatTimes As Integer
        If Not Console.CursorLeft = 0 Then
            RepeatTimes = Console.WindowWidth - Console.CursorLeft
        Else
            RepeatTimes = Console.WindowWidth - (Text.Truncate(Console.WindowWidth - 6) + " ").Length
        End If

        'Write the closing minus sign.
        W("-".Repeat(RepeatTimes), True, ColTypes)

        'Fix CursorTop value on Unix systems. Mono...
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.BufferHeight - 1 Then Console.CursorTop -= 1
        End If
    End Sub

    ''' <summary>
    ''' Draw a separator with text
    ''' </summary>
    ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
    ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
    ''' <param name="Color">A color that will be changed to.</param>
    ''' <param name="Vars">Variables to format the message before it's written.</param>
    Public Sub WriteSeparatorC16(ByVal Text As String, ByVal PrintSuffix As Boolean, ByVal Color As ConsoleColor, ParamArray Vars() As Object)
        'Print the suffix and the text
        If Not String.IsNullOrWhiteSpace(Text) Then
            If PrintSuffix Then Text = "- " + Text
            WriteC16(Text.Truncate(Console.WindowWidth - 6) + " ", False, Color, Vars)
        End If

        'See how many times to repeat the closing minus sign. We could be running this in the wrap command.
        Dim RepeatTimes As Integer
        If Not Console.CursorLeft = 0 Then
            RepeatTimes = Console.WindowWidth - Console.CursorLeft
        Else
            RepeatTimes = Console.WindowWidth - (Text.Truncate(Console.WindowWidth - 6) + " ").Length
        End If

        'Write the closing minus sign.
        WriteC16("-".Repeat(RepeatTimes), True, Color)

        'Fix CursorTop value on Unix systems. Mono...
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.BufferHeight - 1 Then Console.CursorTop -= 1
        End If
    End Sub

    ''' <summary>
    ''' Draw a separator with text
    ''' </summary>
    ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
    ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="Vars">Variables to format the message before it's written.</param>
    Public Sub WriteSeparatorC16(ByVal Text As String, ByVal PrintSuffix As Boolean, ByVal ForegroundColor As ConsoleColor, ByVal BackgroundColor As ConsoleColor, ParamArray Vars() As Object)
        'Print the suffix and the text
        If Not String.IsNullOrWhiteSpace(Text) Then
            If PrintSuffix Then Text = "- " + Text
            WriteC16(Text.Truncate(Console.WindowWidth - 6) + " ", False, ForegroundColor, BackgroundColor, Vars)
        End If

        'See how many times to repeat the closing minus sign. We could be running this in the wrap command.
        Dim RepeatTimes As Integer
        If Not Console.CursorLeft = 0 Then
            RepeatTimes = Console.WindowWidth - Console.CursorLeft
        Else
            RepeatTimes = Console.WindowWidth - (Text.Truncate(Console.WindowWidth - 6) + " ").Length
        End If

        'Write the closing minus sign.
        WriteC16("-".Repeat(RepeatTimes), True, ForegroundColor, BackgroundColor)

        'Fix CursorTop value on Unix systems. Mono...
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.BufferHeight - 1 Then Console.CursorTop -= 1
        End If
    End Sub

    ''' <summary>
    ''' Draw a separator with text
    ''' </summary>
    ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
    ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
    ''' <param name="Color">A color that will be changed to.</param>
    ''' <param name="Vars">Variables to format the message before it's written.</param>
    Public Sub WriteSeparatorC(ByVal Text As String, ByVal PrintSuffix As Boolean, ByVal Color As Color, ParamArray Vars() As Object)
        'Print the suffix and the text
        If Not String.IsNullOrWhiteSpace(Text) Then
            If PrintSuffix Then Text = "- " + Text
            WriteC(Text.Truncate(Console.WindowWidth - 6) + " ", False, Color, Vars)
        End If

        'See how many times to repeat the closing minus sign. We could be running this in the wrap command.
        Dim RepeatTimes As Integer
        If Not Console.CursorLeft = 0 Then
            RepeatTimes = Console.WindowWidth - Console.CursorLeft
        Else
            RepeatTimes = Console.WindowWidth - (Text.Truncate(Console.WindowWidth - 6) + " ").Length
        End If

        'Write the closing minus sign.
        WriteC("-".Repeat(RepeatTimes), True, Color)

        'Fix CursorTop value on Unix systems. Mono...
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.BufferHeight - 1 Then Console.CursorTop -= 1
        End If
    End Sub

    ''' <summary>
    ''' Draw a separator with text
    ''' </summary>
    ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
    ''' <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
    ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
    ''' <param name="BackgroundColor">A background color that will be changed to.</param>
    ''' <param name="Vars">Variables to format the message before it's written.</param>
    Public Sub WriteSeparatorC(ByVal Text As String, ByVal PrintSuffix As Boolean, ByVal ForegroundColor As Color, ByVal BackgroundColor As Color, ParamArray Vars() As Object)
        'Print the suffix and the text
        If Not String.IsNullOrWhiteSpace(Text) Then
            If PrintSuffix Then Text = "- " + Text
            WriteC(Text.Truncate(Console.WindowWidth - 6) + " ", False, ForegroundColor, BackgroundColor, Vars)
        End If

        'See how many times to repeat the closing minus sign. We could be running this in the wrap command.
        Dim RepeatTimes As Integer
        If Not Console.CursorLeft = 0 Then
            RepeatTimes = Console.WindowWidth - Console.CursorLeft
        Else
            RepeatTimes = Console.WindowWidth - (Text.Truncate(Console.WindowWidth - 6) + " ").Length
        End If

        'Write the closing minus sign.
        WriteC("-".Repeat(RepeatTimes), True, ForegroundColor, BackgroundColor)

        'Fix CursorTop value on Unix systems. Mono...
        If IsOnUnix() Then
            If Not Console.CursorTop = Console.BufferHeight - 1 Then Console.CursorTop -= 1
        End If
    End Sub

End Module
