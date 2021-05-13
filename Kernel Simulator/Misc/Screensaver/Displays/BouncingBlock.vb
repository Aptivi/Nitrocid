
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

Module BouncingBlockDisplay

    Public WithEvents BouncingBlock As New BackgroundWorker

    ''' <summary>
    ''' Handles the code of Bouncing Block
    ''' </summary>
    Sub BouncingBlock_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BouncingBlock.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Dim Direction As String = "BottomRight"
        Dim RowBlock, ColumnBlock As Integer
        RowBlock = Console.WindowHeight / 2
        ColumnBlock = Console.WindowWidth / 2
        Do While True
            SleepNoBlock(BouncingBlockDelay, BouncingBlock)
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            If BouncingBlock.CancellationPending = True Then
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
                If BouncingBlockTrueColor Then
                    Dim esc As Char = GetEsc()
                    Dim RedColorNum As Integer = RandomDriver.Next(255)
                    Dim GreenColorNum As Integer = RandomDriver.Next(255)
                    Dim BlueColorNum As Integer = RandomDriver.Next(255)
                    Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                    WriteWhereC(" ", ColumnBlock, RowBlock, True, New Color(New RGB(255, 255, 255).ToString), New Color(ColorStorage.ToString))
                ElseIf BouncingBlock255Colors Then
                    Dim esc As Char = GetEsc()
                    Dim ColorNum As Integer = RandomDriver.Next(255)
                    WriteWhereC(" ", ColumnBlock, RowBlock, True, New Color(ConsoleColors.White), BackgroundColor:=New Color([Enum].Parse(GetType(ConsoleColors), ColorNum)))
                Else
                    Dim OldColumn As Integer = Console.CursorLeft
                    Dim OldRow As Integer = Console.CursorTop
                    Console.BackgroundColor = colors(RandomDriver.Next(colors.Length - 1))
                    Console.SetCursorPosition(ColumnBlock, RowBlock)
                    Console.Write(" ")
                    Console.SetCursorPosition(OldColumn, OldRow)
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Write(" ")
                End If

                If Direction = "BottomRight" Then
                    RowBlock += 1
                    ColumnBlock += 1
                ElseIf Direction = "BottomLeft" Then
                    RowBlock += 1
                    ColumnBlock -= 1
                ElseIf Direction = "TopRight" Then
                    RowBlock -= 1
                    ColumnBlock += 1
                ElseIf Direction = "TopLeft" Then
                    RowBlock -= 1
                    ColumnBlock -= 1
                End If

                If RowBlock = Console.WindowHeight - 2 Then
                    Direction = Direction.Replace("Bottom", "Top")
                ElseIf RowBlock = 1 Then
                    Direction = Direction.Replace("Top", "Bottom")
                End If

                If ColumnBlock = Console.WindowWidth - 1 Then
                    Direction = Direction.Replace("Right", "Left")
                ElseIf ColumnBlock = 1 Then
                    Direction = Direction.Replace("Left", "Right")
                End If
            End If
        Loop
    End Sub

End Module
