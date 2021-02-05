
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
Imports System.Threading

Module BouncingTextDisplay

    Public WithEvents BouncingText As New BackgroundWorker

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
            Thread.Sleep(10)
            Console.Clear()
            If BouncingText.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Bouncing Text screensaver stopped.")
                Exit Do
            Else
                WriteWhere(BouncingTextWrite, ColumnFirstLetter, RowText, ColTypes.Neutral)

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
                ElseIf RowText = 1 Then
                    Direction = Direction.Replace("Top", "Bottom")
                End If

                If ColumnLastLetter = Console.WindowWidth - 1 Then
                    Direction = Direction.Replace("Right", "Left")
                ElseIf ColumnFirstLetter = 1 Then
                    Direction = Direction.Replace("Left", "Right")
                End If
            End If
        Loop
    End Sub

End Module
