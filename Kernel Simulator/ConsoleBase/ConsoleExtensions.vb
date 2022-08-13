
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
Imports VT.NET

Namespace ConsoleBase
    Public Module ConsoleExtensions

        ''' <summary>
        ''' Clears the console buffer, but keeps the current cursor position
        ''' </summary>
        Public Sub ClearKeepPosition()
            Dim Left As Integer = Console.CursorLeft
            Dim Top As Integer = Console.CursorTop
            Console.Clear()
            Console.SetCursorPosition(Left, Top)
        End Sub

        ''' <summary>
        ''' Clears the line to the right
        ''' </summary>
        Public Sub ClearLineToRight()
            Console.Write(GetEsc() + "[0K")
        End Sub

        ''' <summary>
        ''' Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        ''' </summary>
        ''' <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        ''' <param name="MaximumNumber">The maximum number.</param>
        ''' <param name="WidthOffset">The console window width offset. It's usually a multiple of 2.</param>
        ''' <returns>How many times to repeat the character</returns>
        Public Function PercentRepeat(CurrentNumber As Integer, MaximumNumber As Integer, WidthOffset As Integer) As Integer
            Return CurrentNumber * 100 / MaximumNumber * ((Console.WindowWidth - WidthOffset) * 0.01)
        End Function

        ''' <summary>
        ''' Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        ''' </summary>
        ''' <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        ''' <param name="MaximumNumber">The maximum number.</param>
        ''' <param name="TargetWidth">The target width</param>
        ''' <returns>How many times to repeat the character</returns>
        Public Function PercentRepeatTargeted(CurrentNumber As Integer, MaximumNumber As Integer, TargetWidth As Integer) As Integer
            Return CurrentNumber * 100 / MaximumNumber * (TargetWidth * 0.01)
        End Function

        ''' <summary>
        ''' Filters the VT sequences that matches the regex
        ''' </summary>
        ''' <param name="Text">The text that contains the VT sequences</param>
        ''' <returns>The text that doesn't contain the VT sequences</returns>
        Public Function FilterVTSequences(Text As String) As String
            'Filter all sequences
            Text = Filters.FilterVTSequences(Text)
            Return Text
        End Function

        ''' <summary>
        ''' Get the filtered cursor positions (by filtered means filtered from the VT escape sequences that matches the regex in the routine)
        ''' </summary>
        ''' <param name="Text">The text that contains the VT sequences</param>
        ''' <param name="Left">The filtered left position</param>
        ''' <param name="Top">The filtered top position</param>
        Public Sub GetFilteredPositions(Text As String, ByRef Left As Integer, ByRef Top As Integer, ParamArray Vars() As Object)
            'First, get the old cursor positions
            Dim OldLeft As Integer = Console.CursorLeft
            Dim OldTop As Integer = Console.CursorTop

            'Second, filter all text from the VT escape sequences
            Text = FilterVTSequences(Text)

            'Third, seek through filtered text (make it seem like it came from Linux by removing CR (\r)), return to the old position, and return the filtered positions
            Text = FormatString(Text, Vars)
            Text = Text.Replace(Convert.ToChar(13), "")
            Dim LeftSeekPosition As Integer = OldLeft
            Dim TopSeekPosition As Integer = OldTop
            For i As Integer = 1 To Text.Length
                'If we spotted a new line character, get down by one line.
                If Text(i - 1) = Convert.ToChar(10) And TopSeekPosition < Console.BufferHeight - 1 Then
                    TopSeekPosition += 1
                ElseIf Text(i - 1) <> Convert.ToChar(10) Then
                    'Simulate seeking through text
                    LeftSeekPosition += 1
                    If LeftSeekPosition >= Console.WindowWidth Then
                        'We've reached end of line
                        LeftSeekPosition = 0

                        'Get down by one line
                        TopSeekPosition += 1
                        If TopSeekPosition > Console.BufferHeight - 1 Then
                            'We're at the end of buffer! Decrement by one.
                            TopSeekPosition -= 1
                        End If
                    End If
                End If
            Next
            Left = LeftSeekPosition
            Top = TopSeekPosition

            'Finally, set the correct old position
            Console.SetCursorPosition(OldLeft, OldTop)
        End Sub

        ''' <summary>
        ''' Polls $TERM_PROGRAM to get terminal emulator
        ''' </summary>
        Public Function GetTerminalEmulator() As String
            Return If(Environment.GetEnvironmentVariable("TERM_PROGRAM"), "")
        End Function

        ''' <summary>
        ''' Polls $TERM to get terminal type (vt100, dumb, ...)
        ''' </summary>
        Public Function GetTerminalType() As String
            Return If(Environment.GetEnvironmentVariable("TERM"), "")
        End Function

        Public Sub SetTitle(Text As String)
            Dim BellChar As Char = Convert.ToChar(7)
            Dim EscapeChar As Char = Convert.ToChar(27)
            Dim Sequence As String = $"{EscapeChar}]0;{Text}{BellChar}"
            Console.Title = Text
            WritePlain(Sequence, False)
        End Sub

    End Module
End Namespace
