
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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
Imports System.Timers
Imports Timer = System.Timers.Timer

Public Module TimerScreen

    Friend TimerUpdate As New Thread(AddressOf UpdateTimerElapsedDisplay) With {.IsBackground = True}
    Friend TimerStarted As Date
    Friend WithEvents Timer As New Timer

    ''' <summary>
    ''' Opens the timer screen
    ''' </summary>
    Sub OpenTimer()
        'Clear for cleanliness
        Console.Clear()
        Console.CursorVisible = False

        'Populate the time
        Dim TimerInterval As Double = 60000

        'Populate the positions for time
        Dim HalfWidth As Integer = Console.WindowWidth / 2
        Dim HalfHeight As Integer = Console.WindowHeight / 2
        Dim TimeLeftPosition As Integer = HalfWidth - ((Date.Now.AddMilliseconds(TimerInterval) - Date.Now).ToString("d\.hh\:mm\:ss\.fff", CurrentCult).Length / 2)
        Dim TimeTopPosition As Integer = HalfHeight - 2

        'Populate the keys text variable
        Dim KeysText As String = "[ENTER] " + DoTranslation("Start counting down or recount") + " | [T] " + DoTranslation("Indicate milliseconds") + " | [ESC] " + DoTranslation("Exit")
        Dim KeysTextLeftPosition As Integer = HalfWidth - (KeysText.Length / 2)
        Dim KeysTextTopPosition As Integer = Console.WindowHeight - 2
        Dim KeysKeypress As ConsoleKey

        'Print the keys text
        WriteWhere(KeysText, KeysTextLeftPosition, KeysTextTopPosition, True, ColTypes.Tip)

        'Print the time interval
        WriteWhere((Date.Now.AddMilliseconds(TimerInterval) - Date.Now).ToString("d\.hh\:mm\:ss\.fff", CurrentCult), TimeLeftPosition, TimeTopPosition, True, ColTypes.Neutral)

        While KeysKeypress <> ConsoleKey.Escape
            'Wait for a keypress
            KeysKeypress = Console.ReadKey(True).Key

            'Check for a keypress
            Select Case KeysKeypress
                Case ConsoleKey.Enter
                    Timer.Interval = TimerInterval
                    Timer.Start()
                    TimerStarted = Date.Now
                    If Not TimerUpdate.IsAlive Then TimerUpdate.Start()
                Case ConsoleKey.T
                    If Not Timer.Enabled Then
                        WriteWhere(DoTranslation("Specify the timeout in milliseconds") + " [{0}] ", 2, KeysTextTopPosition - 2, False, ColTypes.Question, TimerInterval)
                        SetInputColor()
                        Dim UnparsedInterval As String = Console.ReadLine()
                        If Not Double.TryParse(UnparsedInterval, TimerInterval) Then
                            WriteWhere(DoTranslation("Indicated timeout is not numeric."), 2, KeysTextTopPosition - 2, False, ColTypes.Error)
                            ClearLineToRight()
                            Console.ReadKey()
                        Else
                            TimeLeftPosition = HalfWidth - ((Date.Now.AddMilliseconds(TimerInterval) - Date.Now).ToString("d\.hh\:mm\:ss\.fff", CurrentCult).Length / 2)
                            WriteWhere((Date.Now.AddMilliseconds(TimerInterval) - Date.Now).ToString("d\.hh\:mm\:ss\.fff", CurrentCult), TimeLeftPosition, TimeTopPosition, True, ColTypes.Neutral)
                        End If
                        Console.SetCursorPosition(0, KeysTextTopPosition - 2)
                        ClearLineToRight()
                    End If
                Case ConsoleKey.Escape
                    Timer.Stop()
                    Timer.Dispose()
                    If TimerUpdate.IsAlive Then TimerUpdate.Abort()
            End Select
        End While

        'Clear for cleanliness
        Timer = New Timer
        Console.Clear()
        Console.CursorVisible = True
        TimerUpdate = New Thread(AddressOf UpdateTimerElapsedDisplay) With {.IsBackground = True}
    End Sub

    ''' <summary>
    ''' Indicates that the timer has elapsed
    ''' </summary>
    Private Sub TimerElapsed(sender As Object, e As ElapsedEventArgs) Handles Timer.Elapsed
        Dim HalfWidth As Integer = Console.WindowWidth / 2
        Dim HalfHeight As Integer = Console.WindowHeight / 2
        Dim TimeLeftPosition As Integer = HalfWidth - (New TimeSpan().ToString("d\.hh\:mm\:ss\.fff", CurrentCult).Length / 2)
        Dim TimeTopPosition As Integer = HalfHeight - 2
        If TimerUpdate.IsAlive Then TimerUpdate.Abort()
        WriteWhere(New TimeSpan().ToString("d\.hh\:mm\:ss\.fff", CurrentCult), TimeLeftPosition, TimeTopPosition, True, ColTypes.Success)
        Timer.Stop()
    End Sub

    ''' <summary>
    ''' Updates the timer elapsed display
    ''' </summary>
    Private Sub UpdateTimerElapsedDisplay()
        While TimerUpdate.IsAlive
            Try
                Dim Until As TimeSpan = TimerStarted.AddMilliseconds(Timer.Interval) - Date.Now
                Dim HalfWidth As Integer = Console.WindowWidth / 2
                Dim HalfHeight As Integer = Console.WindowHeight / 2
                Dim TimeLeftPosition As Integer = HalfWidth - (Until.ToString("d\.hh\:mm\:ss\.fff", CurrentCult).Length / 2)
                Dim TimeTopPosition As Integer = HalfHeight - 2
                WriteWhere(Until.ToString("d\.hh\:mm\:ss\.fff", CurrentCult), TimeLeftPosition, TimeTopPosition, True, ColTypes.Neutral)
            Catch ex As ThreadAbortException
                Exit While
            End Try
        End While
    End Sub

End Module
