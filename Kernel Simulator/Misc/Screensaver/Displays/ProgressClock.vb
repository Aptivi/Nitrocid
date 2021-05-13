
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

Module ProgressClockDisplay

    Public WithEvents ProgressClock As New BackgroundWorker

    ''' <summary>
    ''' Handles the code of Progress Clock
    ''' </summary>
    Sub ProgressClock_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles ProgressClock.DoWork
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Dim CurrentTicks As Long = ProgressClockCycleColorsTicks
        Do While True
            SleepNoBlock(500, ProgressClock)
            Console.Clear()
            If ProgressClock.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(New Color(InputColor).VTSequenceForeground)
                Console.Write(New Color(BackgroundColor).VTSequenceBackground)
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Progress Clock screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                If ProgressClockTrueColor Then
                    'Prepare colors
                    Dim esc As Char = GetEsc()
                    Dim RedColorNumHours, GreenColorNumHours, BlueColorNumHours As Integer
                    Dim RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes As Integer
                    Dim RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds As Integer
                    Dim RedColorNum, GreenColorNum, BlueColorNum As Integer
                    Dim ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds As Integer
                    Dim InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds As Integer
                    Dim ColorStorageHours, ColorStorageMinutes, ColorStorageSeconds, ColorStorage As RGB
                    If ProgressClockCycleColors Then
                        If CurrentTicks >= ProgressClockCycleColorsTicks Then
                            RedColorNumHours = RandomDriver.Next(255)
                            GreenColorNumHours = RandomDriver.Next(255)
                            BlueColorNumHours = RandomDriver.Next(255)
                            RedColorNumMinutes = RandomDriver.Next(255)
                            GreenColorNumMinutes = RandomDriver.Next(255)
                            BlueColorNumMinutes = RandomDriver.Next(255)
                            RedColorNumSeconds = RandomDriver.Next(255)
                            GreenColorNumSeconds = RandomDriver.Next(255)
                            BlueColorNumSeconds = RandomDriver.Next(255)
                            RedColorNum = RandomDriver.Next(255)
                            GreenColorNum = RandomDriver.Next(255)
                            BlueColorNum = RandomDriver.Next(255)
                            ColorStorageHours = New RGB(RedColorNumHours, GreenColorNumHours, BlueColorNumHours)
                            ColorStorageMinutes = New RGB(RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes)
                            ColorStorageSeconds = New RGB(RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds)
                            ColorStorage = New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                            CurrentTicks = 0
                        End If
                    Else
                        ColorStorageHours = New RGB(ProgressClockHoursProgressColor)
                        ColorStorageMinutes = New RGB(ProgressClockMinutesProgressColor)
                        ColorStorageSeconds = New RGB(ProgressClockSecondsProgressColor)
                        ColorStorage = New RGB(ProgressClockProgressColor)
                    End If
                    ProgressFillPositionHours = CInt(Console.WindowHeight / 2) - 10
                    ProgressFillPositionMinutes = CInt(Console.WindowHeight / 2) - 1
                    ProgressFillPositionSeconds = CInt(Console.WindowHeight / 2) + 8
                    InformationPositionHours = CInt(Console.WindowHeight / 2) - 12
                    InformationPositionMinutes = CInt(Console.WindowHeight / 2) - 3
                    InformationPositionSeconds = CInt(Console.WindowHeight / 2) + 6

#Disable Warning BC42104
                    'Hours
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) - 9, True, New Color(ColorStorageHours.ToString))   'Top of Hours
                    WriteWhereC("|" + " ".Repeat(Console.WindowWidth - 10) + "|", 4, ProgressFillPositionHours, True, New Color(ColorStorageHours.ToString))            'Medium of Hours
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) - 11, True, New Color(ColorStorageHours.ToString))  'Bottom of Hours

                    'Minutes
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2), True, New Color(ColorStorageMinutes.ToString))     'Top of Minutes
                    WriteWhereC("|" + " ".Repeat(Console.WindowWidth - 10) + "|", 4, ProgressFillPositionMinutes, True, New Color(ColorStorageMinutes.ToString))        'Medium of Minutes
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) - 2, True, New Color(ColorStorageMinutes.ToString)) 'Bottom of Minutes

                    'Seconds
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) + 9, True, New Color(ColorStorageSeconds.ToString)) 'Top of Seconds
                    WriteWhereC("|" + " ".Repeat(Console.WindowWidth - 10) + "|", 4, ProgressFillPositionSeconds, True, New Color(ColorStorageSeconds.ToString))        'Medium of Seconds
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) + 7, True, New Color(ColorStorageSeconds.ToString)) 'Bottom of Seconds
#Enable Warning BC42104
                    'Fill progress for hours, minutes, and seconds
                    If Not KernelDateTime.Hour = 0 Then WriteWhereC(" ".Repeat(KernelDateTime.Hour * 100 / 24 * ((Console.WindowWidth - 10) * 0.01)), 5, ProgressFillPositionHours, True, New Color(New RGB(0, 0, 0).ToString), New Color(ColorStorageHours.ToString))
                    If Not KernelDateTime.Minute = 0 Then WriteWhereC(" ".Repeat(KernelDateTime.Minute * 100 / 60 * ((Console.WindowWidth - 10) * 0.01)), 5, ProgressFillPositionMinutes, True, New Color(New RGB(0, 0, 0).ToString), New Color(ColorStorageMinutes.ToString))
                    If Not KernelDateTime.Second = 0 Then WriteWhereC(" ".Repeat(KernelDateTime.Second * 100 / 60 * ((Console.WindowWidth - 10) * 0.01)), 5, ProgressFillPositionSeconds, True, New Color(New RGB(0, 0, 0).ToString), New Color(ColorStorageSeconds.ToString))

                    'Print information
                    WriteWhereC("H: {0}/24", 4, InformationPositionHours, True, New Color(ColorStorageHours.ToString), KernelDateTime.Hour)
                    WriteWhereC("M: {0}/60", 4, InformationPositionMinutes, True, New Color(ColorStorageMinutes.ToString), KernelDateTime.Minute)
                    WriteWhereC("S: {0}/60", 4, InformationPositionSeconds, True, New Color(ColorStorageSeconds.ToString), KernelDateTime.Second)

                    'Print date information
                    WriteWhereC(Render, Console.WindowWidth / 2 - Render.Length / 2, Console.WindowHeight - 2, True, New Color(ColorStorageSeconds.ToString))
                ElseIf ProgressClock255Colors Then
                    Dim esc As Char = GetEsc()
                    Dim ColorNumHours, ColorNumMinutes, ColorNumSeconds, ColorNum As Integer
                    Dim ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds As Integer
                    Dim InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds As Integer
                    If ProgressClockCycleColors Then
                        If CurrentTicks >= ProgressClockCycleColorsTicks Then
                            ColorNumHours = [Enum].Parse(GetType(ConsoleColors), RandomDriver.Next(1, 255))
                            ColorNumMinutes = [Enum].Parse(GetType(ConsoleColors), RandomDriver.Next(1, 255))
                            ColorNumSeconds = [Enum].Parse(GetType(ConsoleColors), RandomDriver.Next(1, 255))
                            ColorNum = [Enum].Parse(GetType(ConsoleColors), RandomDriver.Next(1, 255))
                            CurrentTicks = 0
                        End If
                    Else
                        ColorNumHours = [Enum].Parse(GetType(ConsoleColors), ProgressClockHoursProgressColor)
                        ColorNumMinutes = [Enum].Parse(GetType(ConsoleColors), ProgressClockMinutesProgressColor)
                        ColorNumSeconds = [Enum].Parse(GetType(ConsoleColors), ProgressClockSecondsProgressColor)
                        ColorNum = [Enum].Parse(GetType(ConsoleColors), ProgressClockProgressColor)
                    End If
                    ProgressFillPositionHours = CInt(Console.WindowHeight / 2) - 10
                    ProgressFillPositionMinutes = CInt(Console.WindowHeight / 2) - 1
                    ProgressFillPositionSeconds = CInt(Console.WindowHeight / 2) + 8
                    InformationPositionHours = CInt(Console.WindowHeight / 2) - 12
                    InformationPositionMinutes = CInt(Console.WindowHeight / 2) - 3
                    InformationPositionSeconds = CInt(Console.WindowHeight / 2) + 6

                    'Hours
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) - 9, True, New Color(ColorNumHours))   'Top of Hours
                    WriteWhereC("|" + " ".Repeat(Console.WindowWidth - 10) + "|", 4, ProgressFillPositionHours, True, New Color(ColorNumHours))            'Medium of Hours
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) - 11, True, New Color(ColorNumHours))  'Bottom of Hours

                    'Minutes
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2), True, New Color(ColorNumMinutes))     'Top of Minutes
                    WriteWhereC("|" + " ".Repeat(Console.WindowWidth - 10) + "|", 4, ProgressFillPositionMinutes, True, New Color(ColorNumMinutes))        'Medium of Minutes
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) - 2, True, New Color(ColorNumMinutes)) 'Bottom of Minutes

                    'Seconds
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) + 9, True, New Color(ColorNumSeconds)) 'Top of Seconds
                    WriteWhereC("|" + " ".Repeat(Console.WindowWidth - 10) + "|", 4, ProgressFillPositionSeconds, True, New Color(ColorNumSeconds))        'Medium of Seconds
                    WriteWhereC("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) + 7, True, New Color(ColorNumSeconds)) 'Bottom of Seconds

                    'Fill progress for hours, minutes, and seconds
                    If Not KernelDateTime.Hour = 0 Then WriteWhereC(" ".Repeat(KernelDateTime.Hour * 100 / 24 * ((Console.WindowWidth - 10) * 0.01)), 5, ProgressFillPositionHours, True, New Color(ConsoleColors.Black), BackgroundColor:=New Color(ColorNumHours))
                    If Not KernelDateTime.Minute = 0 Then WriteWhereC(" ".Repeat(KernelDateTime.Minute * 100 / 60 * ((Console.WindowWidth - 10) * 0.01)), 5, ProgressFillPositionMinutes, True, New Color(ConsoleColors.Black), BackgroundColor:=New Color(ColorNumMinutes))
                    If Not KernelDateTime.Second = 0 Then WriteWhereC(" ".Repeat(KernelDateTime.Second * 100 / 60 * ((Console.WindowWidth - 10) * 0.01)), 5, ProgressFillPositionSeconds, True, New Color(ConsoleColors.Black), BackgroundColor:=New Color(ColorNumSeconds))

                    'Print information
                    WriteWhereC("H: {0}/24", 4, InformationPositionHours, True, New Color(ColorNumHours), KernelDateTime.Hour)
                    WriteWhereC("M: {0}/60", 4, InformationPositionMinutes, True, New Color(ColorNumMinutes), KernelDateTime.Minute)
                    WriteWhereC("S: {0}/60", 4, InformationPositionSeconds, True, New Color(ColorNumSeconds), KernelDateTime.Second)

                    'Print date information
                    WriteWhereC(Render, Console.WindowWidth / 2 - Render.Length / 2, Console.WindowHeight - 2, True, New Color(ColorNum))
                Else
                    Dim ColorNumHours, ColorNumMinutes, ColorNumSeconds, ColorNum As ConsoleColor
                    Dim ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds As Integer
                    Dim InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds As Integer
                    If ProgressClockCycleColors Then
                        If CurrentTicks >= ProgressClockCycleColorsTicks Then
                            ColorNumHours = [Enum].Parse(GetType(ConsoleColor), RandomDriver.Next(1, 15))
                            ColorNumMinutes = [Enum].Parse(GetType(ConsoleColor), RandomDriver.Next(1, 15))
                            ColorNumSeconds = [Enum].Parse(GetType(ConsoleColor), RandomDriver.Next(1, 15))
                            ColorNum = [Enum].Parse(GetType(ConsoleColor), RandomDriver.Next(1, 15))
                            CurrentTicks = 0
                        End If
                    Else
                        ColorNumHours = [Enum].Parse(GetType(ConsoleColor), ProgressClockHoursProgressColor)
                        ColorNumMinutes = [Enum].Parse(GetType(ConsoleColor), ProgressClockMinutesProgressColor)
                        ColorNumSeconds = [Enum].Parse(GetType(ConsoleColor), ProgressClockSecondsProgressColor)
                        ColorNum = [Enum].Parse(GetType(ConsoleColor), ProgressClockProgressColor)
                    End If
                    ProgressFillPositionHours = CInt(Console.WindowHeight / 2) - 10
                    ProgressFillPositionMinutes = CInt(Console.WindowHeight / 2) - 1
                    ProgressFillPositionSeconds = CInt(Console.WindowHeight / 2) + 8
                    InformationPositionHours = CInt(Console.WindowHeight / 2) - 12
                    InformationPositionMinutes = CInt(Console.WindowHeight / 2) - 3
                    InformationPositionSeconds = CInt(Console.WindowHeight / 2) + 6

                    'Hours
                    WriteWhereC16("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) - 9, True, ColorNumHours)   'Top of Hours
                    WriteWhereC16("|" + " ".Repeat(Console.WindowWidth - 10) + "|", 4, ProgressFillPositionHours, True, ColorNumHours)            'Medium of Hours
                    WriteWhereC16("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) - 11, True, ColorNumHours)  'Bottom of Hours

                    'Minutes
                    WriteWhereC16("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2), True, ColorNumMinutes)     'Top of Minutes
                    WriteWhereC16("|" + " ".Repeat(Console.WindowWidth - 10) + "|", 4, ProgressFillPositionMinutes, True, ColorNumMinutes)        'Medium of Minutes
                    WriteWhereC16("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) - 2, True, ColorNumMinutes) 'Bottom of Minutes

                    'Seconds
                    WriteWhereC16("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) + 9, True, ColorNumSeconds) 'Top of Seconds
                    WriteWhereC16("|" + " ".Repeat(Console.WindowWidth - 10) + "|", 4, ProgressFillPositionSeconds, True, ColorNumSeconds)        'Medium of Seconds
                    WriteWhereC16("+" + "-".Repeat(Console.WindowWidth - 10) + "+", 4, CInt(Console.WindowHeight / 2) + 7, True, ColorNumSeconds) 'Bottom of Seconds

                    'Fill progress for hours, minutes, and seconds
                    If Not KernelDateTime.Hour = 0 Then WriteWhereC16(" ".Repeat(KernelDateTime.Hour * 100 / 24 * ((Console.WindowWidth - 10) * 0.01)), 5, ProgressFillPositionHours, True, ConsoleColor.Black, BackgroundColor:=ColorNumHours)
                    If Not KernelDateTime.Minute = 0 Then WriteWhereC16(" ".Repeat(KernelDateTime.Minute * 100 / 60 * ((Console.WindowWidth - 10) * 0.01)), 5, ProgressFillPositionMinutes, True, ConsoleColor.Black, BackgroundColor:=ColorNumMinutes)
                    If Not KernelDateTime.Second = 0 Then WriteWhereC16(" ".Repeat(KernelDateTime.Second * 100 / 60 * ((Console.WindowWidth - 10) * 0.01)), 5, ProgressFillPositionSeconds, True, ConsoleColor.Black, BackgroundColor:=ColorNumSeconds)

                    'Print information
                    WriteWhereC16("H: {0}/24", 4, InformationPositionHours, True, ColorNumHours, KernelDateTime.Hour)
                    WriteWhereC16("M: {0}/60", 4, InformationPositionMinutes, True, ColorNumMinutes, KernelDateTime.Minute)
                    WriteWhereC16("S: {0}/60", 4, InformationPositionSeconds, True, ColorNumSeconds, KernelDateTime.Second)

                    'Print date information
                    WriteWhereC16(Render, Console.WindowWidth / 2 - Render.Length / 2, Console.WindowHeight - 2, True, ColorNum)
                End If
                If ProgressClockCycleColors Then CurrentTicks += 1
            End If
        Loop
    End Sub

End Module
