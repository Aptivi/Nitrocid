
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
Imports KS.TimeDate
Imports System.Threading
Imports System.Timers
Imports Figgle
Imports Timer = System.Timers.Timer

Namespace Misc.Timers
    Public Module TimerScreen

        Public TimerFigletFont As String = "Small"
        Friend TimerUpdate As New KernelThread("Timer Remaining Time Updater", True, AddressOf UpdateTimerElapsedDisplay)
        Friend TimerStarted As Date
        Friend FigletTimeOldWidth As Integer
        Friend FigletTimeOldWidthEnd As Integer
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

            'Update the old positions
            FigletTimeOldWidth = HalfWidth - (GetFigletWidth(CurrentRemainingString, FigletFont) / 2)
            FigletTimeOldWidthEnd = HalfWidth + (GetFigletWidth(CurrentRemainingString, FigletFont) / 2)

            'Populate the keys text variable
            Dim KeysText As String = "[ENTER] " + DoTranslation("Start (re)counting down") + " | [T] " + DoTranslation("Set interval") + " | [ESC] " + DoTranslation("Exit")
            Dim KeysTextLeftPosition As Integer = HalfWidth - (KeysText.Length / 2)
            Dim KeysTextTopPosition As Integer = Console.WindowHeight - 2
            Dim KeysKeypress As ConsoleKey

            'Print the keys text
            WriteWhere(KeysText, KeysTextLeftPosition, KeysTextTopPosition, True, ColTypes.Tip)

            'Print the time interval
            If EnableFigletTimer Then
                WriteFigletWhere(CurrentRemainingString, TimeLeftPosition, TimeTopPosition, True, FigletFont, ColTypes.Neutral)
            Else
                WriteWhere(CurrentRemainingString, TimeLeftPosition, TimeTopPosition, True, ColTypes.Neutral)
            End If

            'Print the border
            WriteWhere("═".Repeat(Console.WindowWidth), 0, KeysTextTopPosition - 2, True, ColTypes.Gray)

            'Wait for a keypress
            While KeysKeypress <> ConsoleKey.Escape
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
                        'User requested to specify the timeout in milliseconds
                        If Not Timer.Enabled Then
                            WriteWhere(DoTranslation("Specify the timeout in milliseconds") + " [{0}] ", 2, KeysTextTopPosition - 4, False, ColTypes.Question, TimerInterval)
                            SetInputColor()

                            'Try to parse the interval
                            Dim UnparsedInterval As String = Console.ReadLine()
                            If Not Double.TryParse(UnparsedInterval, TimerInterval) Then
                                'Not numeric.
                                WriteWhere(DoTranslation("Indicated timeout is not numeric."), 2, KeysTextTopPosition - 4, False, ColTypes.Error)
                                ClearLineToRight()
                                Console.ReadKey()
                            Else
                                'Update the remaining time
                                Dim RemainingString As String = GetRemainingTimeFromNow(TimerInterval)
                                UpdateRemainingPositions(RemainingString, TimeLeftPosition, TimeTopPosition)
                                ClearRemainingTimeDisplay(RemainingString, FigletTimeOldWidth, FigletTimeOldWidthEnd)
                                If EnableFigletTimer Then
                                    WriteFigletWhere(RemainingString, TimeLeftPosition, TimeTopPosition, True, FigletFont, ColTypes.Neutral)
                                Else
                                    WriteWhere(RemainingString, TimeLeftPosition, TimeTopPosition, True, ColTypes.Neutral)
                                End If
                            End If

                            'Clean up
                            Console.SetCursorPosition(0, KeysTextTopPosition - 4)
                            ClearLineToRight()
                        End If
                    Case ConsoleKey.Escape
                        'Stop the timer
                        Timer.Stop()
                        Timer.Dispose()
                        If TimerUpdate.IsAlive Then TimerUpdate.Stop()
                End Select
            End While

            'Clear for cleanliness
            Timer = New Timer
            Console.Clear()
            Console.CursorVisible = True
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

            'Prepare the display
            UpdateRemainingPositions(ElapsedText, TimeLeftPosition, TimeTopPosition)
            ClearRemainingTimeDisplay(ElapsedText, FigletTimeOldWidth, FigletTimeOldWidthEnd)

            'Actually display it
            If TimerUpdate.IsAlive Then TimerUpdate.Stop()
            If EnableFigletTimer Then
                WriteFigletWhere(ElapsedText, TimeLeftPosition, TimeTopPosition, True, FigletFont, ColTypes.Success)
            Else
                WriteWhere(ElapsedText, TimeLeftPosition, TimeTopPosition, True, ColTypes.Success)
            End If
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

                    'Prepare the display
                    UpdateRemainingPositions(UntilText, TimeLeftPosition, TimeTopPosition)
                    ClearRemainingTimeDisplay(UntilText, FigletTimeOldWidth, FigletTimeOldWidthEnd)

                    'Actually display the remaining time
                    If EnableFigletTimer Then
                        WriteFigletWhere(UntilText, TimeLeftPosition, TimeTopPosition, True, FigletFont, ColTypes.Neutral)
                    Else
                        WriteWhere(UntilText, TimeLeftPosition, TimeTopPosition, True, ColTypes.Neutral)
                    End If
                Catch ex As ThreadInterruptedException
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
            TimeTopPosition = HalfHeight - 3
            If EnableFigletTimer Then
                TimeLeftPosition = FigletTimeLeftPosition
                TimeTopPosition = FigletTimeTopPosition
            End If
        End Sub

        Private Sub ClearRemainingTimeDisplay(RemainingTimeText As String, FigletOldWidth As Integer, FigletOldWidthEnd As Integer)
            'Some initial variables
            Dim FigletFont As FiggleFont = GetFigletFont(TimerFigletFont)
            Dim HalfWidth As Integer = Console.WindowWidth / 2
            Dim HalfHeight As Integer = Console.WindowHeight / 2

            'Get the Figlet time left and top position
            Dim FigletTimeLeftPosition As Integer = HalfWidth - (GetFigletWidth(RemainingTimeText, FigletFont) / 2)
            Dim FigletTimeLeftEndPosition As Integer = HalfWidth + (GetFigletWidth(RemainingTimeText, FigletFont) / 2)
            Dim FigletTimeTopPosition As Integer = HalfHeight - (GetFigletHeight(RemainingTimeText, FigletFont) / 2)
            Dim FigletTimeBottomPosition As Integer = HalfHeight + (GetFigletHeight(RemainingTimeText, FigletFont) / 2)

            'If figlet is enabled, clear the display
            If EnableFigletTimer Then
                For FigletTimePosition As Integer = FigletTimeTopPosition To FigletTimeBottomPosition
                    Console.CursorTop = FigletTimePosition
                    For Position As Integer = FigletOldWidth - 1 To FigletTimeLeftPosition - 1
                        Console.CursorLeft = Position
                        Write(" ", False, NeutralTextColor, BackgroundColor)
                    Next
                    For Position As Integer = FigletOldWidthEnd To FigletTimeLeftEndPosition + 1
                        Console.CursorLeft = Position
                        Write(" ", False, NeutralTextColor, BackgroundColor)
                    Next
                Next
            End If

            'Update the old positions
            FigletTimeOldWidth = HalfWidth - (GetFigletWidth(RemainingTimeText, FigletFont) / 2)
            FigletTimeOldWidthEnd = HalfWidth + (GetFigletWidth(RemainingTimeText, FigletFont) / 2)
        End Sub

    End Module
End Namespace