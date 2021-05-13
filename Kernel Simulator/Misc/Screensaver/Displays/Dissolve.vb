
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

Module DissolveDisplay

    Public WithEvents Dissolve As New BackgroundWorker

    ''' <summary>
    ''' Handles the code of Dissolve
    ''' </summary>
    Sub Dissolve_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Dissolve.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Dim ColorFilled As Boolean
        Dim CoveredPositions As New ArrayList
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Do While True
            If Dissolve.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Dissolve screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                If ColorFilled Then Thread.Sleep(1)
                Dim EndLeft As Integer = Console.WindowWidth - 1
                Dim EndTop As Integer = Console.WindowHeight - 1
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                If Not ColorFilled Then
                    'NOTICE: Mono seems to have a bug in Console.CursorLeft and Console.CursorTop when printing with VT escape sequences.
                    If Not (Console.CursorLeft = EndLeft And Console.CursorTop = EndTop) Then
                        If DissolveTrueColor Then
                            Dim esc As Char = GetEsc()
                            Dim RedColorNum As Integer = RandomDriver.Next(255)
                            Dim GreenColorNum As Integer = RandomDriver.Next(255)
                            Dim BlueColorNum As Integer = RandomDriver.Next(255)
                            Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                            Console.Write(esc + "[48;2;" + ColorStorage.ToString + "m ")
                        ElseIf Dissolve255Colors Then
                            Dim esc As Char = GetEsc()
                            Dim ColorNum As Integer = RandomDriver.Next(255)
                            Console.Write(esc + "[48;5;" + CStr(ColorNum) + "m ")
                        Else
                            Console.BackgroundColor = colors(RandomDriver.Next(colors.Length - 1))
                            Console.Write(" ")
                        End If
                    Else
                        ColorFilled = True
                    End If
                Else
                    If Not CoveredPositions.Contains(Left & " - " & Top) Then CoveredPositions.Add(Left & " - " & Top)
                    Console.SetCursorPosition(Left, Top)
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Write(" ")
                    If CoveredPositions.Count = (EndLeft + 1) * (EndTop + 1) Then
                        ColorFilled = False
                        Console.BackgroundColor = ConsoleColor.Black
                        Console.Clear()
                        CoveredPositions.Clear()
                    End If
                End If
            End If
        Loop
    End Sub

End Module
