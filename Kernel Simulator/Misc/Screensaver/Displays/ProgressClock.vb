
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

    Public WithEvents ProgressClock As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Progress Clock
    ''' </summary>
    Sub ProgressClock_DoWork(sender As Object, e As DoWorkEventArgs) Handles ProgressClock.DoWork
        Try
            'Variables
            Dim RandomDriver As New Random()
            Dim CurrentTicks As Long = ProgressClockCycleColorsTicks

            'Preparations
            Console.Clear()
            Console.CursorVisible = False

            'Screensaver logic
            Do While True
                SleepNoBlock(500, ProgressClock)
                Console.Clear()
                If ProgressClock.CancellationPending = True Then
                    Wdbg(DebugLevel.W, "Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    SetInputColor()
                    LoadBack()
                    Console.CursorVisible = True
                    Wdbg(DebugLevel.I, "All clean. Progress Clock screensaver stopped.")
                    SaverAutoReset.Set()
                    Exit Do
                Else
                    If ProgressClockTrueColor Then
                        'Prepare colors
                        Dim RedColorNumHours, GreenColorNumHours, BlueColorNumHours As Integer
                        Dim RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes As Integer
                        Dim RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds As Integer
                        Dim RedColorNum, GreenColorNum, BlueColorNum As Integer
                        Dim ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds As Integer
                        Dim InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds As Integer
                        Dim ColorStorageHours, ColorStorageMinutes, ColorStorageSeconds, ColorStorage As RGB

                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current tick: {0}", CurrentTicks)
                        If ProgressClockCycleColors Then
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors...")
                            If CurrentTicks >= ProgressClockCycleColorsTicks Then
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current tick equals the maximum ticks to change color.")
                                RedColorNumHours = RandomDriver.Next(255)
                                GreenColorNumHours = RandomDriver.Next(255)
                                BlueColorNumHours = RandomDriver.Next(255)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Hours) (R;G;B: {0};{1};{2})", RedColorNumHours, GreenColorNumHours, BlueColorNumHours)
                                RedColorNumMinutes = RandomDriver.Next(255)
                                GreenColorNumMinutes = RandomDriver.Next(255)
                                BlueColorNumMinutes = RandomDriver.Next(255)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Minutes) (R;G;B: {0};{1};{2})", RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes)
                                RedColorNumSeconds = RandomDriver.Next(255)
                                GreenColorNumSeconds = RandomDriver.Next(255)
                                BlueColorNumSeconds = RandomDriver.Next(255)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Seconds) (R;G;B: {0};{1};{2})", RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds)
                                RedColorNum = RandomDriver.Next(255)
                                GreenColorNum = RandomDriver.Next(255)
                                BlueColorNum = RandomDriver.Next(255)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                                ColorStorageHours = New RGB(RedColorNumHours, GreenColorNumHours, BlueColorNumHours)
                                ColorStorageMinutes = New RGB(RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes)
                                ColorStorageSeconds = New RGB(RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds)
                                ColorStorage = New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                                CurrentTicks = 0
                            End If
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Parsing colors...")
                            ColorStorageHours = New RGB(ProgressClockHoursProgressColor)
                            ColorStorageMinutes = New RGB(ProgressClockMinutesProgressColor)
                            ColorStorageSeconds = New RGB(ProgressClockSecondsProgressColor)
                            ColorStorage = New RGB(ProgressClockProgressColor)
                        End If
                        ProgressFillPositionHours = CInt(Console.WindowHeight / 2) - 10
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Hours) {0}", ProgressFillPositionHours)
                        ProgressFillPositionMinutes = CInt(Console.WindowHeight / 2) - 1
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Minutes) {0}", ProgressFillPositionMinutes)
                        ProgressFillPositionSeconds = CInt(Console.WindowHeight / 2) + 8
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Seconds) {0}", ProgressFillPositionSeconds)
                        InformationPositionHours = CInt(Console.WindowHeight / 2) - 12
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Hours) {0}", InformationPositionHours)
                        InformationPositionMinutes = CInt(Console.WindowHeight / 2) - 3
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Minutes) {0}", InformationPositionMinutes)
                        InformationPositionSeconds = CInt(Console.WindowHeight / 2) + 6
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Seconds) {0}", InformationPositionSeconds)

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
                        Dim ColorNumHours, ColorNumMinutes, ColorNumSeconds, ColorNum As Integer
                        Dim ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds As Integer
                        Dim InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds As Integer
                        If ProgressClockCycleColors Then
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors...")
                            If CurrentTicks >= ProgressClockCycleColorsTicks Then
                                ColorNumHours = [Enum].Parse(GetType(ConsoleColors), RandomDriver.Next(1, 255))
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Hours) ({0})", ColorNumHours)
                                ColorNumMinutes = [Enum].Parse(GetType(ConsoleColors), RandomDriver.Next(1, 255))
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Minutes) ({0})", ColorNumMinutes)
                                ColorNumSeconds = [Enum].Parse(GetType(ConsoleColors), RandomDriver.Next(1, 255))
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Seconds) ({0})", ColorNumSeconds)
                                ColorNum = [Enum].Parse(GetType(ConsoleColors), RandomDriver.Next(1, 255))
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                                CurrentTicks = 0
                            End If
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Parsing colors...")
                            ColorNumHours = [Enum].Parse(GetType(ConsoleColors), ProgressClockHoursProgressColor)
                            ColorNumMinutes = [Enum].Parse(GetType(ConsoleColors), ProgressClockMinutesProgressColor)
                            ColorNumSeconds = [Enum].Parse(GetType(ConsoleColors), ProgressClockSecondsProgressColor)
                            ColorNum = [Enum].Parse(GetType(ConsoleColors), ProgressClockProgressColor)
                        End If
                        ProgressFillPositionHours = CInt(Console.WindowHeight / 2) - 10
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Hours) {0}", ProgressFillPositionHours)
                        ProgressFillPositionMinutes = CInt(Console.WindowHeight / 2) - 1
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Minutes) {0}", ProgressFillPositionMinutes)
                        ProgressFillPositionSeconds = CInt(Console.WindowHeight / 2) + 8
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Seconds) {0}", ProgressFillPositionSeconds)
                        InformationPositionHours = CInt(Console.WindowHeight / 2) - 12
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Hours) {0}", InformationPositionHours)
                        InformationPositionMinutes = CInt(Console.WindowHeight / 2) - 3
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Minutes) {0}", InformationPositionMinutes)
                        InformationPositionSeconds = CInt(Console.WindowHeight / 2) + 6
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Seconds) {0}", InformationPositionSeconds)

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
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors...")
                            If CurrentTicks >= ProgressClockCycleColorsTicks Then
                                ColorNumHours = [Enum].Parse(GetType(ConsoleColor), RandomDriver.Next(1, 15))
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Hours) ({0})", ColorNumHours)
                                ColorNumMinutes = [Enum].Parse(GetType(ConsoleColor), RandomDriver.Next(1, 15))
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Minutes) ({0})", ColorNumMinutes)
                                ColorNumSeconds = [Enum].Parse(GetType(ConsoleColor), RandomDriver.Next(1, 15))
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Seconds) ({0})", ColorNumSeconds)
                                ColorNum = [Enum].Parse(GetType(ConsoleColor), RandomDriver.Next(1, 15))
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                                CurrentTicks = 0
                            End If
                        Else
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Parsing colors...")
                            ColorNumHours = [Enum].Parse(GetType(ConsoleColor), ProgressClockHoursProgressColor)
                            ColorNumMinutes = [Enum].Parse(GetType(ConsoleColor), ProgressClockMinutesProgressColor)
                            ColorNumSeconds = [Enum].Parse(GetType(ConsoleColor), ProgressClockSecondsProgressColor)
                            ColorNum = [Enum].Parse(GetType(ConsoleColor), ProgressClockProgressColor)
                        End If
                        ProgressFillPositionHours = CInt(Console.WindowHeight / 2) - 10
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Hours) {0}", ProgressFillPositionHours)
                        ProgressFillPositionMinutes = CInt(Console.WindowHeight / 2) - 1
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Minutes) {0}", ProgressFillPositionMinutes)
                        ProgressFillPositionSeconds = CInt(Console.WindowHeight / 2) + 8
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for progress (Seconds) {0}", ProgressFillPositionSeconds)
                        InformationPositionHours = CInt(Console.WindowHeight / 2) - 12
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Hours) {0}", InformationPositionHours)
                        InformationPositionMinutes = CInt(Console.WindowHeight / 2) - 3
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Minutes) {0}", InformationPositionMinutes)
                        InformationPositionSeconds = CInt(Console.WindowHeight / 2) + 6
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Fill position for info (Seconds) {0}", InformationPositionSeconds)

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
        Catch ex As Exception
            Wdbg(DebugLevel.W, "Screensaver experienced an error: {0}. Cleaning everything up...", ex.Message)
            WStkTrc(ex)
            e.Cancel = True
            SetInputColor()
            LoadBack()
            Console.CursorVisible = True
            Wdbg(DebugLevel.I, "All clean. Progress Clock screensaver stopped.")
            W(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, ex.Message)
            SaverAutoReset.Set()
        End Try
    End Sub

End Module
