
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

Module LighterDisplay

    Public WithEvents Lighter As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Lighter
    ''' </summary>
    Sub Lighter_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Lighter.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Dim CoveredPositions As New ArrayList
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            If Lighter.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(New Color(InputColor).VTSequenceForeground)
                Console.Write(New Color(BackgroundColor).VTSequenceBackground)
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Lighter screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                SleepNoBlock(LighterDelay, Lighter)
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                Console.SetCursorPosition(Left, Top)
                If LighterTrueColor Then
                    Dim esc As Char = GetEsc()
                    Dim RedColorNum As Integer = RandomDriver.Next(255)
                    Dim GreenColorNum As Integer = RandomDriver.Next(255)
                    Dim BlueColorNum As Integer = RandomDriver.Next(255)
                    Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                    If Not CoveredPositions.Contains(Left & ";" & Top) Then CoveredPositions.Add(Left & ";" & Top)
                    Console.Write(esc + "[48;2;" + ColorStorage.ToString + "m ")
                ElseIf Lighter255Colors Then
                    Dim esc As Char = GetEsc()
                    Dim ColorNum As Integer = RandomDriver.Next(255)
                    If Not CoveredPositions.Contains(Left & ";" & Top) Then CoveredPositions.Add(Left & ";" & Top)
                    Console.Write(esc + "[48;5;" + CStr(ColorNum) + "m ")
                Else
                    If Not CoveredPositions.Contains(Left & ";" & Top) Then CoveredPositions.Add(Left & ";" & Top)
                    Console.BackgroundColor = colors(RandomDriver.Next(colors.Length - 1))
                    Console.Write(" ")
                End If
                If CoveredPositions.Count = LighterMaxPositions Then
                    Dim WipeLeft As Integer = CoveredPositions(0).ToString.Substring(0, CoveredPositions(0).ToString.IndexOf(";"))
                    Dim WipeTop As Integer = CoveredPositions(0).ToString.Substring(CoveredPositions(0).ToString.IndexOf(";") + 1)
                    Console.SetCursorPosition(WipeLeft, WipeTop)
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Write(" ")
                    CoveredPositions.RemoveAt(0)
                End If
            End If
        Loop
    End Sub

End Module
