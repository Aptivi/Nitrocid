
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

Imports System.Text
Imports System.Threading

Namespace Misc.Timers
    Public Module StopwatchScreen

        Friend Laps As New List(Of LapDisplayInfo)
        Friend StopwatchUpdate As New KernelThread("Stopwatch ETA Updater", True, AddressOf UpdateStopwatchElapsedDisplay)
        Friend LapColor As Color = NeutralTextColor
        Friend Stopwatch As New Stopwatch
        Friend LappedStopwatch As New Stopwatch
        Friend NewLapAcknowledged As Boolean

        ''' <summary>
        ''' Opens the stopwatch screen
        ''' </summary>
        Sub OpenStopwatch()
            'Clear for cleanliness
            Console.Clear()
            Console.CursorVisible = False

            'Populate the positions for time
            Dim LapsText As String = DoTranslation("Lap")
            Dim HalfWidth As Integer = Console.WindowWidth / 2
            Dim HalfHeight As Integer = Console.WindowHeight / 2
            Dim TimeLeftPosition As Integer = HalfWidth * 1.5 - Stopwatch.Elapsed.ToString("d\.hh\:mm\:ss\.fff", CurrentCult).Length / 2
            Dim TimeTopPosition As Integer = HalfHeight - 2
            Dim LapsCurrentLapLeftPosition As Integer = 4
            Dim LapsCurrentLapTopPosition As Integer = Console.WindowHeight - 6

            'Populate the keys text variable
            Dim KeysText As String = "[ENTER] " + DoTranslation("Start or stop") + " | [L] " + DoTranslation("Lap") + " | [R] " + DoTranslation("Reset") + " | [ESC] " + DoTranslation("Exit")
            Dim KeysTextLeftPosition As Integer = HalfWidth - (KeysText.Length / 2)
            Dim KeysTextTopPosition As Integer = Console.WindowHeight - 2
            Dim KeysKeypress As ConsoleKey

            'Print the keys text
            WriteWhere(KeysText, KeysTextLeftPosition, KeysTextTopPosition, True, ColTypes.Tip)

            'Print the time interval and the current lap
            WriteWhere(Stopwatch.Elapsed.ToString("d\.hh\:mm\:ss\.fff", CurrentCult), TimeLeftPosition, TimeTopPosition, True, LapColor)
            WriteWhere(LapsText + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, True, LapColor, Laps.Count + 1, LappedStopwatch.Elapsed.ToString("d\.hh\:mm\:ss\.fff", CurrentCult))

            'Print the border
            MakeBorder()

            While KeysKeypress <> ConsoleKey.Escape
                'Wait for a keypress
                KeysKeypress = Console.ReadKey(True).Key

                'Check for a keypress
                Select Case KeysKeypress
                    Case ConsoleKey.Enter
                        If Not StopwatchUpdate.IsAlive Then
                            StopwatchUpdate.Start()
                        Else
                            StopwatchUpdate.Stop()
                        End If
                        If LappedStopwatch.IsRunning Then LappedStopwatch.Stop() Else LappedStopwatch.Start()
                        If Stopwatch.IsRunning Then Stopwatch.Stop() Else Stopwatch.Start()
                    Case ConsoleKey.L
                        If LappedStopwatch.IsRunning Then
                            Dim Lap As New LapDisplayInfo(LapColor, LappedStopwatch.Elapsed)
                            Laps.Add(Lap)
                            LappedStopwatch.Restart()
                            NewLapAcknowledged = True

                            'Select random color
                            Dim Randomizer As New Random
                            Dim RedValue As Integer = Randomizer.Next(255)
                            Dim GreenValue As Integer = Randomizer.Next(255)
                            Dim BlueValue As Integer = Randomizer.Next(255)
                            LapColor = New Color(RedValue, GreenValue, BlueValue)
                        End If
                    Case ConsoleKey.R
                        If StopwatchUpdate.IsAlive Then
                            StopwatchUpdate.Stop()
                        End If
                        If LappedStopwatch.IsRunning Then LappedStopwatch.Reset()
                        If Stopwatch.IsRunning Then Stopwatch.Reset()

                        'Clear the laps and the laps list
                        Laps.Clear()
                        For Y As Integer = 1 To LapsCurrentLapTopPosition - 1
                            Console.SetCursorPosition(LapsCurrentLapLeftPosition, Y)
                            ClearLineToRight()
                        Next

                        'Reset the indicators
                        LapColor = NeutralTextColor
                        WriteWhere(DoTranslation("Lap") + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, False, LapColor, Laps.Count + 1, LappedStopwatch.Elapsed.ToString("d\.hh\:mm\:ss\.fff", CurrentCult))
                        ClearLineToRight()
                        Console.SetCursorPosition(0, TimeTopPosition)
                        ClearLineToRight()
                        WriteWhere(Stopwatch.Elapsed.ToString("d\.hh\:mm\:ss\.fff", CurrentCult), TimeLeftPosition, TimeTopPosition, False, LapColor)
                        MakeBorder()
                    Case ConsoleKey.Escape
                        If LappedStopwatch.IsRunning Then LappedStopwatch.Reset()
                        If Stopwatch.IsRunning Then Stopwatch.Reset()
                        LapColor = NeutralTextColor
                        If StopwatchUpdate.IsAlive Then StopwatchUpdate.Stop()
                End Select
            End While

            'Clear for cleanliness
            Laps.Clear()
            Console.Clear()
            Console.CursorVisible = True
        End Sub

        ''' <summary>
        ''' Updates the elapsed display for stopwatch
        ''' </summary>
        Private Sub UpdateStopwatchElapsedDisplay()
            'Populate the positions for time and for lap list
            Dim LapsText As String = DoTranslation("Lap")
            Dim HalfWidth As Integer = Console.WindowWidth / 2
            Dim HalfHeight As Integer = Console.WindowHeight / 2
            Dim TimeLeftPosition As Integer = HalfWidth * 1.5 - Stopwatch.Elapsed.ToString("d\.hh\:mm\:ss\.fff", CurrentCult).Length / 2
            Dim TimeTopPosition As Integer = HalfHeight - 2
            Dim LapsCurrentLapLeftPosition As Integer = 4
            Dim LapsCurrentLapTopPosition As Integer = Console.WindowHeight - 6
            Dim LapsLapsListLeftPosition As Integer = 4
            Dim LapsLapsListTopPosition As Integer = 3

            While StopwatchUpdate.IsAlive
                Try
                    'Update the elapsed display
                    WriteWhere(Stopwatch.Elapsed.ToString("d\.hh\:mm\:ss\.fff", CurrentCult), TimeLeftPosition, TimeTopPosition, True, LapColor)
                    WriteWhere(LapsText + " {0}: {1}", LapsCurrentLapLeftPosition, LapsCurrentLapTopPosition, True, LapColor, Laps.Count + 1, LappedStopwatch.Elapsed.ToString("d\.hh\:mm\:ss\.fff", CurrentCult))

                    'Update the laps list if new lap is acknowledged
                    If NewLapAcknowledged Then
                        Dim LapsListEndBorder As Integer = Console.WindowHeight - 10
                        Dim LapsListBuilder As New StringBuilder
                        Dim BorderDifference As Integer = Laps.Count - LapsListEndBorder
                        If BorderDifference < 0 Then BorderDifference = 0
                        For LapIndex As Integer = BorderDifference To Laps.Count - 1
                            Dim Lap As LapDisplayInfo = Laps(LapIndex)
                            LapsListBuilder.AppendLine(Lap.LapColor.VTSequenceForeground + DoTranslation("Lap") + $" {LapIndex + 1}: {Lap.LapInterval.ToString("d\.hh\:mm\:ss\.fff", CurrentCult)}")
                        Next
                        WriteWhere(LapsListBuilder.ToString, LapsLapsListLeftPosition, LapsLapsListTopPosition, True, LapColor)
                        NewLapAcknowledged = False
                    End If
                Catch ex As ThreadInterruptedException
                    Exit While
                End Try
            End While
        End Sub

        ''' <summary>
        ''' Makes the display border
        ''' </summary>
        Sub MakeBorder()
            Dim KeysTextTopPosition As Integer = Console.WindowHeight - 2
            Dim HalfWidth As Integer = Console.WindowWidth / 2
            WriteWhere("═".Repeat(Console.WindowWidth), 0, KeysTextTopPosition - 2, True, ColTypes.Gray)
            WriteWhere("═".Repeat(Console.WindowWidth), 0, 1, True, ColTypes.Gray)
            For Height = 2 To KeysTextTopPosition - 2
                WriteWhere("║", HalfWidth, Height, True, ColTypes.Gray)
            Next
            WriteWhere("╩", HalfWidth, KeysTextTopPosition - 2, True, ColTypes.Gray)
            WriteWhere("╦", HalfWidth, 1, True, ColTypes.Gray)
        End Sub

    End Module
End Namespace
