
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

Imports System.ComponentModel

Module BouncingTextDisplay

    Public WithEvents BouncingText As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Bouncing Text
    ''' </summary>
    Sub BouncingText_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BouncingText.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim Direction As String = "BottomRight"
        Dim RowText, ColumnFirstLetter, ColumnLastLetter As Integer
        RowText = Console.WindowHeight / 2
        ColumnFirstLetter = (Console.WindowWidth / 2) - BouncingTextWrite.Length / 2
        ColumnLastLetter = (Console.WindowWidth / 2) + BouncingTextWrite.Length / 2
        Do While True
            SleepNoBlock(BouncingTextDelay, BouncingText)
            Console.Clear()
            If BouncingText.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(New Color(InputColor).VTSequenceForeground)
                Console.Write(New Color(BackgroundColor).VTSequenceBackground)
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Bouncing Text screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                Console.SetCursorPosition(ColumnFirstLetter, RowText)
                Console.Write(BouncingTextWrite)

                If Direction = "BottomRight" Then
                    RowText += 1
                    ColumnFirstLetter += 1
                    ColumnLastLetter += 1
                ElseIf Direction = "BottomLeft" Then
                    RowText += 1
                    ColumnFirstLetter -= 1
                    ColumnLastLetter -= 1
                ElseIf Direction = "TopRight" Then
                    RowText -= 1
                    ColumnFirstLetter += 1
                    ColumnLastLetter += 1
                ElseIf Direction = "TopLeft" Then
                    RowText -= 1
                    ColumnFirstLetter -= 1
                    ColumnLastLetter -= 1
                End If

                If RowText = Console.WindowHeight - 2 Then
                    Direction = Direction.Replace("Bottom", "Top")
                    ChangeBouncingTextColor()
                ElseIf RowText = 1 Then
                    Direction = Direction.Replace("Top", "Bottom")
                    ChangeBouncingTextColor()
                End If

                If ColumnLastLetter = Console.WindowWidth - 1 Then
                    Direction = Direction.Replace("Right", "Left")
                    ChangeBouncingTextColor()
                ElseIf ColumnFirstLetter = 1 Then
                    Direction = Direction.Replace("Left", "Right")
                    ChangeBouncingTextColor()
                End If
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Changes the color of bouncing text
    ''' </summary>
    Sub ChangeBouncingTextColor()
        Dim RandomDriver As New Random
        If BouncingTextTrueColor Then
            Dim RedColorNum As Integer = RandomDriver.Next(1, 255)
            Dim GreenColorNum As Integer = RandomDriver.Next(1, 255)
            Dim BlueColorNum As Integer = RandomDriver.Next(1, 255)
            Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
            Console.Write(ProbePlaces($"<f:{ColorStorage}><b:0;0;0>"))
        ElseIf BouncingText255Colors Then
            Dim ColorNum As Integer = RandomDriver.Next(1, 255)
            Console.Write(ProbePlaces($"<f:{ColorNum}><b:0>"))
        Else
            Console.ForegroundColor = colors(RandomDriver.Next(1, colors.Length - 1))
            Console.BackgroundColor = ConsoleColor.Black
        End If
    End Sub

End Module
