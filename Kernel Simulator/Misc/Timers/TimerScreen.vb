
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

Imports KS.Misc.Writers.FancyWriters.Tools
Imports System.Threading
Imports System.Timers
Imports Figgle
Imports Timer = System.Timers.Timer

Namespace Misc.Timers
    Public Module TimerScreen

        Public TimerFigletFont As String = "Small"
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

            'Populate the figlet font (if any)
            Dim FigletFont As FiggleFont = GetFigletFont(TimerFigletFont)

            'Populate the time
            Dim TimerInterval As Double = 60000

            'Populate the positions for time
            Dim HalfWidth As Integer = Console.WindowWidth / 2
            Dim HalfHeight As Integer = Console.WindowHeight / 2
            Dim CurrentRemainingString As String = GetRemainingTimeFromNow(TimerInterval)
            Dim TimeLeftPosition As Integer = 0
            Dim TimeTopPosition As Integer = 0
            UpdateRemainingPositions(CurrentRemainingString, TimeLeftPosition, TimeTopPosition)

            'Populate the keys text variable
            Dim KeysText As String = "[ENTER] " + DoTranslation("Start (re)counting down") + " | [T] " + DoTranslation("Set interval") + " | [ESC] " + DoTranslation("Exit")
            Dim KeysTextLeftPosition As Integer = HalfWidth - (KeysText.Length / 2)
            Dim KeysTextTopPosition As Integer = Console.WindowHeight - 2
            Dim KeysKeypress As ConsoleKey

            'Print the keys text
            WriteWhere(KeysText, KeysTextLeftPosition, KeysTextTopPosition, True, ColTypes.Tip)

            'Print the time interval
            WriteFigletWhere(CurrentRemainingString, TimeLeftPosition, TimeTopPosition, True, FigletFont, ColTypes.Neutral)

            While KeysKeypress <> ConsoleKey.Escape
                'Wait for a keypress
                KeysKeypress = Console.ReadKey(True).Key

                'Check for a keypress
                Select Case KeysKeypress
                    Case ConsoleKey.Enter
                        'User requested to start up the timer
                        Timer.Interval = TimerInterval
                        Timer.Start()
                        TimerStarted = Date.Now
                        If Not TimerUpdate.IsAlive Then TimerUpdate.Start()
                    Case ConsoleKey.T
                        'User requested to specif the timeout in milliseconds
                        If Not Timer.Enabled Then
                            WriteWhere(DoTranslation("Specify the timeout in milliseconds") + " [{0}] ", 2, KeysTextTopPosition - 2, False, ColTypes.Question, TimerInterval)
                            SetInputColor()

                            'Try to parse the interval
                            Dim UnparsedInterval As String = Console.ReadLine()
                            If Not Double.TryParse(UnparsedInterval, TimerInterval) Then
                                'Not numeric.
                                WriteWhere(DoTranslation("Indicated timeout is not numeric."), 2, KeysTextTopPosition - 2, False, ColTypes.Error)
                                ClearLineToRight()
                                Console.ReadKey()
                            Else
                                'Update the remaining time
                                Dim RemainingString As String = GetRemainingTimeFromNow(TimerInterval)
                                UpdateRemainingPositions(RemainingString, TimeLeftPosition, TimeTopPosition)
                                WriteFigletWhere(RemainingString, TimeLeftPosition, TimeTopPosition, True, FigletFont, ColTypes.Neutral)
                            End If

                            'Clean up
                            Console.SetCursorPosition(0, KeysTextTopPosition - 2)
                            ClearLineToRight()
                        End If
                    Case ConsoleKey.Escape
                        'Stop the timer
                        Timer.Stop()
                        Timer.Dispose()
                        If TimerUpdate.IsAlive Then TimerUpdate.Abort()
                        TimerUpdate = New Thread(AddressOf UpdateTimerElapsedDisplay) With {.IsBackground = True}
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
            Dim FigletFont As FiggleFont = GetFigletFont(TimerFigletFont)
            Dim HalfWidth As Integer = Console.WindowWidth / 2
            Dim HalfHeight As Integer = Console.WindowHeight / 2
            Dim ElapsedText As String = New TimeSpan().ToString("d\.hh\:mm\:ss\.fff", CurrentCult)
            Dim TimeLeftPosition As Integer = 0
            Dim TimeTopPosition As Integer = 0
            UpdateRemainingPositions(ElapsedText, TimeLeftPosition, TimeTopPosition)
            If TimerUpdate.IsAlive Then TimerUpdate.Abort()
            TimerUpdate = New Thread(AddressOf UpdateTimerElapsedDisplay) With {.IsBackground = True}
            WriteFigletWhere(ElapsedText, TimeLeftPosition, TimeTopPosition, True, FigletFont, ColTypes.Success)
            Timer.Stop()
        End Sub

        ''' <summary>
        ''' Updates the timer elapsed display
        ''' </summary>
        Private Sub UpdateTimerElapsedDisplay()
            Dim FigletFont As FiggleFont = GetFigletFont(TimerFigletFont)
            While TimerUpdate.IsAlive
                Try
                    Dim Until As TimeSpan = TimerStarted.AddMilliseconds(Timer.Interval) - Date.Now
                    Dim HalfWidth As Integer = Console.WindowWidth / 2
                    Dim HalfHeight As Integer = Console.WindowHeight / 2
                    Dim UntilText As String = Until.ToString("d\.hh\:mm\:ss\.fff", CurrentCult)
                    Dim TimeLeftPosition As Integer = 0
                    Dim TimeTopPosition As Integer = 0
                    UpdateRemainingPositions(UntilText, TimeLeftPosition, TimeTopPosition)
                    WriteFigletWhere(UntilText, TimeLeftPosition, TimeTopPosition, True, FigletFont, ColTypes.Neutral)
                Catch ex As ThreadAbortException
                    Exit While
                End Try
            End While
        End Sub

        ''' <summary>
        ''' Updates the remaining positions for time, adapting to Figlet if possible
        ''' </summary>
        Private Sub UpdateRemainingPositions(RemainingTimeText As String, ByRef TimeLeftPosition As Integer, ByRef TimeTopPosition As Integer)
            'Some initial variables
            Dim FigletFont As FiggleFont = GetFigletFont(TimerFigletFont)
            Dim HalfWidth As Integer = Console.WindowWidth / 2
            Dim HalfHeight As Integer = Console.WindowHeight / 2

            'Get the Figlet time left and top position
            Dim FigletTimeLeftPosition As Integer = HalfWidth - (GetFigletWidth(RemainingTimeText, FigletFont) / 2)
            Dim FigletTimeTopPosition As Integer = HalfHeight - (GetFigletHeight(RemainingTimeText, FigletFont) / 2)

            'Now, get the normal time left and top position and update the values according to timer type
            TimeLeftPosition = HalfWidth - (RemainingTimeText.Length / 2)
            TimeTopPosition = HalfHeight - 2
            If EnableFigletTimer Then
                TimeLeftPosition = FigletTimeLeftPosition
                TimeTopPosition = FigletTimeTopPosition
            End If
        End Sub

    End Module
End Namespace