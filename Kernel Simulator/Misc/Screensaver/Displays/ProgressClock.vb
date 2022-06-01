
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
Imports KS.TimeDate

Namespace Misc.Screensaver.Displays
    Module ProgressClockDisplay

        Public ProgressClock As New KernelThread("ProgressClock screensaver thread", True, AddressOf ProgressClock_DoWork)

        ''' <summary>
        ''' Handles the code of Progress Clock
        ''' </summary>
        Sub ProgressClock_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim CurrentTicks As Long = ProgressClockCycleColorsTicks
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean
                'Sanity checks for color levels
                If ProgressClockTrueColor Or ProgressClock255Colors Then
                    ProgressClockMinimumRedColorLevel = If(ProgressClockMinimumRedColorLevel >= 0 And ProgressClockMinimumRedColorLevel <= 255, ProgressClockMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", ProgressClockMinimumRedColorLevel)
                    ProgressClockMinimumGreenColorLevel = If(ProgressClockMinimumGreenColorLevel >= 0 And ProgressClockMinimumGreenColorLevel <= 255, ProgressClockMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", ProgressClockMinimumGreenColorLevel)
                    ProgressClockMinimumBlueColorLevel = If(ProgressClockMinimumBlueColorLevel >= 0 And ProgressClockMinimumBlueColorLevel <= 255, ProgressClockMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", ProgressClockMinimumBlueColorLevel)
                    ProgressClockMinimumColorLevel = If(ProgressClockMinimumColorLevel >= 0 And ProgressClockMinimumColorLevel <= 255, ProgressClockMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", ProgressClockMinimumColorLevel)
                    ProgressClockMaximumRedColorLevel = If(ProgressClockMaximumRedColorLevel >= 0 And ProgressClockMaximumRedColorLevel <= 255, ProgressClockMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", ProgressClockMaximumRedColorLevel)
                    ProgressClockMaximumGreenColorLevel = If(ProgressClockMaximumGreenColorLevel >= 0 And ProgressClockMaximumGreenColorLevel <= 255, ProgressClockMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", ProgressClockMaximumGreenColorLevel)
                    ProgressClockMaximumBlueColorLevel = If(ProgressClockMaximumBlueColorLevel >= 0 And ProgressClockMaximumBlueColorLevel <= 255, ProgressClockMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", ProgressClockMaximumBlueColorLevel)
                    ProgressClockMaximumColorLevel = If(ProgressClockMaximumColorLevel >= 0 And ProgressClockMaximumColorLevel <= 255, ProgressClockMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", ProgressClockMaximumColorLevel)
                    ProgressClockMinimumRedColorLevelHours = If(ProgressClockMinimumRedColorLevelHours >= 0 And ProgressClockMinimumRedColorLevelHours <= 255, ProgressClockMinimumRedColorLevelHours, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level (hours): {0}", ProgressClockMinimumRedColorLevelHours)
                    ProgressClockMinimumGreenColorLevelHours = If(ProgressClockMinimumGreenColorLevelHours >= 0 And ProgressClockMinimumGreenColorLevelHours <= 255, ProgressClockMinimumGreenColorLevelHours, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level (hours): {0}", ProgressClockMinimumGreenColorLevelHours)
                    ProgressClockMinimumBlueColorLevelHours = If(ProgressClockMinimumBlueColorLevelHours >= 0 And ProgressClockMinimumBlueColorLevelHours <= 255, ProgressClockMinimumBlueColorLevelHours, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level (hours): {0}", ProgressClockMinimumBlueColorLevelHours)
                    ProgressClockMinimumColorLevelHours = If(ProgressClockMinimumColorLevelHours >= 0 And ProgressClockMinimumColorLevelHours <= 255, ProgressClockMinimumColorLevelHours, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level (hours): {0}", ProgressClockMinimumColorLevelHours)
                    ProgressClockMaximumRedColorLevelHours = If(ProgressClockMaximumRedColorLevelHours >= 0 And ProgressClockMaximumRedColorLevelHours <= 255, ProgressClockMaximumRedColorLevelHours, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level (hours): {0}", ProgressClockMaximumRedColorLevelHours)
                    ProgressClockMaximumGreenColorLevelHours = If(ProgressClockMaximumGreenColorLevelHours >= 0 And ProgressClockMaximumGreenColorLevelHours <= 255, ProgressClockMaximumGreenColorLevelHours, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level (hours): {0}", ProgressClockMaximumGreenColorLevelHours)
                    ProgressClockMaximumBlueColorLevelHours = If(ProgressClockMaximumBlueColorLevelHours >= 0 And ProgressClockMaximumBlueColorLevelHours <= 255, ProgressClockMaximumBlueColorLevelHours, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level (hours): {0}", ProgressClockMaximumBlueColorLevelHours)
                    ProgressClockMaximumColorLevelHours = If(ProgressClockMaximumColorLevelHours >= 0 And ProgressClockMaximumColorLevelHours <= 255, ProgressClockMaximumColorLevelHours, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level (hours): {0}", ProgressClockMaximumColorLevelHours)
                    ProgressClockMinimumRedColorLevelMinutes = If(ProgressClockMinimumRedColorLevelMinutes >= 0 And ProgressClockMinimumRedColorLevelMinutes <= 255, ProgressClockMinimumRedColorLevelMinutes, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level (minutes): {0}", ProgressClockMinimumRedColorLevelMinutes)
                    ProgressClockMinimumGreenColorLevelMinutes = If(ProgressClockMinimumGreenColorLevelMinutes >= 0 And ProgressClockMinimumGreenColorLevelMinutes <= 255, ProgressClockMinimumGreenColorLevelMinutes, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level (minutes): {0}", ProgressClockMinimumGreenColorLevelMinutes)
                    ProgressClockMinimumBlueColorLevelMinutes = If(ProgressClockMinimumBlueColorLevelMinutes >= 0 And ProgressClockMinimumBlueColorLevelMinutes <= 255, ProgressClockMinimumBlueColorLevelMinutes, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level (minutes): {0}", ProgressClockMinimumBlueColorLevelMinutes)
                    ProgressClockMinimumColorLevelMinutes = If(ProgressClockMinimumColorLevelMinutes >= 0 And ProgressClockMinimumColorLevelMinutes <= 255, ProgressClockMinimumColorLevelMinutes, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level (minutes): {0}", ProgressClockMinimumColorLevelMinutes)
                    ProgressClockMaximumRedColorLevelMinutes = If(ProgressClockMaximumRedColorLevelMinutes >= 0 And ProgressClockMaximumRedColorLevelMinutes <= 255, ProgressClockMaximumRedColorLevelMinutes, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level (minutes): {0}", ProgressClockMaximumRedColorLevelMinutes)
                    ProgressClockMaximumGreenColorLevelMinutes = If(ProgressClockMaximumGreenColorLevelMinutes >= 0 And ProgressClockMaximumGreenColorLevelMinutes <= 255, ProgressClockMaximumGreenColorLevelMinutes, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level (minutes): {0}", ProgressClockMaximumGreenColorLevelMinutes)
                    ProgressClockMaximumBlueColorLevelMinutes = If(ProgressClockMaximumBlueColorLevelMinutes >= 0 And ProgressClockMaximumBlueColorLevelMinutes <= 255, ProgressClockMaximumBlueColorLevelMinutes, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level (minutes): {0}", ProgressClockMaximumBlueColorLevelMinutes)
                    ProgressClockMaximumColorLevelMinutes = If(ProgressClockMaximumColorLevelMinutes >= 0 And ProgressClockMaximumColorLevelMinutes <= 255, ProgressClockMaximumColorLevelMinutes, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level (minutes): {0}", ProgressClockMaximumColorLevelMinutes)
                    ProgressClockMinimumRedColorLevelSeconds = If(ProgressClockMinimumRedColorLevelSeconds >= 0 And ProgressClockMinimumRedColorLevelSeconds <= 255, ProgressClockMinimumRedColorLevelSeconds, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level (seconds): {0}", ProgressClockMinimumRedColorLevelSeconds)
                    ProgressClockMinimumGreenColorLevelSeconds = If(ProgressClockMinimumGreenColorLevelSeconds >= 0 And ProgressClockMinimumGreenColorLevelSeconds <= 255, ProgressClockMinimumGreenColorLevelSeconds, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level (seconds): {0}", ProgressClockMinimumGreenColorLevelSeconds)
                    ProgressClockMinimumBlueColorLevelSeconds = If(ProgressClockMinimumBlueColorLevelSeconds >= 0 And ProgressClockMinimumBlueColorLevelSeconds <= 255, ProgressClockMinimumBlueColorLevelSeconds, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level (seconds): {0}", ProgressClockMinimumBlueColorLevelSeconds)
                    ProgressClockMinimumColorLevelSeconds = If(ProgressClockMinimumColorLevelSeconds >= 0 And ProgressClockMinimumColorLevelSeconds <= 255, ProgressClockMinimumColorLevelSeconds, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level (seconds): {0}", ProgressClockMinimumColorLevelSeconds)
                    ProgressClockMaximumRedColorLevelSeconds = If(ProgressClockMaximumRedColorLevelSeconds >= 0 And ProgressClockMaximumRedColorLevelSeconds <= 255, ProgressClockMaximumRedColorLevelSeconds, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level (seconds): {0}", ProgressClockMaximumRedColorLevelSeconds)
                    ProgressClockMaximumGreenColorLevelSeconds = If(ProgressClockMaximumGreenColorLevelSeconds >= 0 And ProgressClockMaximumGreenColorLevelSeconds <= 255, ProgressClockMaximumGreenColorLevelSeconds, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level (seconds): {0}", ProgressClockMaximumGreenColorLevelSeconds)
                    ProgressClockMaximumBlueColorLevelSeconds = If(ProgressClockMaximumBlueColorLevelSeconds >= 0 And ProgressClockMaximumBlueColorLevelSeconds <= 255, ProgressClockMaximumBlueColorLevelSeconds, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level (seconds): {0}", ProgressClockMaximumBlueColorLevelSeconds)
                    ProgressClockMaximumColorLevelSeconds = If(ProgressClockMaximumColorLevelSeconds >= 0 And ProgressClockMaximumColorLevelSeconds <= 255, ProgressClockMaximumColorLevelSeconds, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level (seconds): {0}", ProgressClockMaximumColorLevelSeconds)
                Else
                    ProgressClockMinimumColorLevel = If(ProgressClockMinimumColorLevel >= 0 And ProgressClockMinimumColorLevel <= 15, ProgressClockMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", ProgressClockMinimumColorLevel)
                    ProgressClockMaximumColorLevel = If(ProgressClockMaximumColorLevel >= 0 And ProgressClockMaximumColorLevel <= 15, ProgressClockMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", ProgressClockMaximumColorLevel)
                    ProgressClockMinimumColorLevelHours = If(ProgressClockMinimumColorLevelHours >= 0 And ProgressClockMinimumColorLevelHours <= 15, ProgressClockMinimumColorLevelHours, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level (hours): {0}", ProgressClockMinimumColorLevelHours)
                    ProgressClockMaximumColorLevelHours = If(ProgressClockMaximumColorLevelHours >= 0 And ProgressClockMaximumColorLevelHours <= 15, ProgressClockMaximumColorLevelHours, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level (hours): {0}", ProgressClockMaximumColorLevelHours)
                    ProgressClockMinimumColorLevelMinutes = If(ProgressClockMinimumColorLevelMinutes >= 0 And ProgressClockMinimumColorLevelMinutes <= 15, ProgressClockMinimumColorLevelMinutes, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level (minutes): {0}", ProgressClockMinimumColorLevelMinutes)
                    ProgressClockMaximumColorLevelMinutes = If(ProgressClockMaximumColorLevelMinutes >= 0 And ProgressClockMaximumColorLevelMinutes <= 15, ProgressClockMaximumColorLevelMinutes, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level (minutes): {0}", ProgressClockMaximumColorLevelMinutes)
                    ProgressClockMinimumColorLevelSeconds = If(ProgressClockMinimumColorLevelSeconds >= 0 And ProgressClockMinimumColorLevelSeconds <= 15, ProgressClockMinimumColorLevelSeconds, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level (seconds): {0}", ProgressClockMinimumColorLevelSeconds)
                    ProgressClockMaximumColorLevelSeconds = If(ProgressClockMaximumColorLevelSeconds >= 0 And ProgressClockMaximumColorLevelSeconds <= 15, ProgressClockMaximumColorLevelSeconds, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level (seconds): {0}", ProgressClockMaximumColorLevelSeconds)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    Console.Clear()
                    'Prepare colors
                    Dim RedColorNumHours, GreenColorNumHours, BlueColorNumHours As Integer
                    Dim RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes As Integer
                    Dim RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds As Integer
                    Dim RedColorNum, GreenColorNum, BlueColorNum As Integer
                    Dim ColorNumHours, ColorNumMinutes, ColorNumSeconds, ColorNum As Integer
                    Dim ProgressFillPositionHours, ProgressFillPositionMinutes, ProgressFillPositionSeconds As Integer
                    Dim InformationPositionHours, InformationPositionMinutes, InformationPositionSeconds As Integer
                    Dim ColorStorageHours, ColorStorageMinutes, ColorStorageSeconds, ColorStorage As Color

                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current tick: {0}", CurrentTicks)
                    If ProgressClockCycleColors Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors...")
                        If CurrentTicks >= ProgressClockCycleColorsTicks Then
                            If ProgressClockTrueColor Then
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Current tick equals the maximum ticks to change color.")
                                RedColorNumHours = RandomDriver.Next(ProgressClockMinimumRedColorLevelHours, ProgressClockMaximumRedColorLevelHours)
                                GreenColorNumHours = RandomDriver.Next(ProgressClockMinimumGreenColorLevelHours, ProgressClockMaximumGreenColorLevelHours)
                                BlueColorNumHours = RandomDriver.Next(ProgressClockMinimumBlueColorLevelHours, ProgressClockMaximumBlueColorLevelHours)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Hours) (R;G;B: {0};{1};{2})", RedColorNumHours, GreenColorNumHours, BlueColorNumHours)
                                RedColorNumMinutes = RandomDriver.Next(ProgressClockMinimumRedColorLevelMinutes, ProgressClockMaximumRedColorLevelMinutes)
                                GreenColorNumMinutes = RandomDriver.Next(ProgressClockMinimumGreenColorLevelMinutes, ProgressClockMaximumGreenColorLevelMinutes)
                                BlueColorNumMinutes = RandomDriver.Next(ProgressClockMinimumBlueColorLevelMinutes, ProgressClockMaximumBlueColorLevelMinutes)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Minutes) (R;G;B: {0};{1};{2})", RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes)
                                RedColorNumSeconds = RandomDriver.Next(ProgressClockMinimumRedColorLevelSeconds, ProgressClockMaximumRedColorLevelSeconds)
                                GreenColorNumSeconds = RandomDriver.Next(ProgressClockMinimumGreenColorLevelSeconds, ProgressClockMaximumGreenColorLevelSeconds)
                                BlueColorNumSeconds = RandomDriver.Next(ProgressClockMinimumBlueColorLevelSeconds, ProgressClockMaximumBlueColorLevelSeconds)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Seconds) (R;G;B: {0};{1};{2})", RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds)
                                RedColorNum = RandomDriver.Next(ProgressClockMinimumRedColorLevel, ProgressClockMaximumRedColorLevel)
                                GreenColorNum = RandomDriver.Next(ProgressClockMinimumGreenColorLevel, ProgressClockMaximumGreenColorLevel)
                                BlueColorNum = RandomDriver.Next(ProgressClockMinimumBlueColorLevel, ProgressClockMaximumBlueColorLevel)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                                ColorStorageHours = New Color(RedColorNumHours, GreenColorNumHours, BlueColorNumHours)
                                ColorStorageMinutes = New Color(RedColorNumMinutes, GreenColorNumMinutes, BlueColorNumMinutes)
                                ColorStorageSeconds = New Color(RedColorNumSeconds, GreenColorNumSeconds, BlueColorNumSeconds)
                                ColorStorage = New Color(RedColorNum, GreenColorNum, BlueColorNum)
                            Else
                                ColorNumHours = RandomDriver.Next(ProgressClockMinimumColorLevelHours, ProgressClockMaximumColorLevelHours)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Hours) ({0})", ColorNumHours)
                                ColorNumMinutes = RandomDriver.Next(ProgressClockMinimumColorLevelMinutes, ProgressClockMaximumColorLevelMinutes)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Minutes) ({0})", ColorNumMinutes)
                                ColorNumSeconds = RandomDriver.Next(ProgressClockMinimumColorLevelSeconds, ProgressClockMaximumColorLevelSeconds)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (Seconds) ({0})", ColorNumSeconds)
                                ColorNum = RandomDriver.Next(ProgressClockMinimumColorLevel, ProgressClockMaximumColorLevel)
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                                ColorStorageHours = New Color(ColorNumHours)
                                ColorStorageMinutes = New Color(ColorNumMinutes)
                                ColorStorageSeconds = New Color(ColorNumSeconds)
                                ColorStorage = New Color(ColorNum)
                            End If
                            CurrentTicks = 0
                        End If
                    Else
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Parsing colors...")
                        ColorStorageHours = New Color(ProgressClockHoursProgressColor)
                        ColorStorageMinutes = New Color(ProgressClockMinutesProgressColor)
                        ColorStorageSeconds = New Color(ProgressClockSecondsProgressColor)
                        ColorStorage = New Color(ProgressClockProgressColor)
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
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If Not ResizeSyncing Then
                        'Hours
                        WriteWhere(ProgressClockLowerLeftCornerCharHours + ProgressClockLowerFrameCharHours.Repeat(Console.WindowWidth - 10) + ProgressClockLowerRightCornerCharHours, 4, CInt(Console.WindowHeight / 2) - 9, True, ColorStorageHours)         'Bottom of Hours
                        WriteWhere(ProgressClockLeftFrameCharHours + " ".Repeat(Console.WindowWidth - 10) + ProgressClockRightFrameCharHours, 4, ProgressFillPositionHours, True, ColorStorageHours)                                                           'Medium of Hours
                        WriteWhere(ProgressClockUpperLeftCornerCharHours + ProgressClockUpperFrameCharHours.Repeat(Console.WindowWidth - 10) + ProgressClockUpperRightCornerCharHours, 4, CInt(Console.WindowHeight / 2) - 11, True, ColorStorageHours)        'Top of Hours

                        'Minutes
                        WriteWhere(ProgressClockLowerLeftCornerCharMinutes + ProgressClockLowerFrameCharMinutes.Repeat(Console.WindowWidth - 10) + ProgressClockLowerRightCornerCharMinutes, 4, CInt(Console.WindowHeight / 2), True, ColorStorageMinutes)     'Bottom of Minutes
                        WriteWhere(ProgressClockLeftFrameCharMinutes + " ".Repeat(Console.WindowWidth - 10) + ProgressClockRightFrameCharMinutes, 4, ProgressFillPositionMinutes, True, ColorStorageMinutes)                                                   'Medium of Minutes
                        WriteWhere(ProgressClockUpperLeftCornerCharMinutes + ProgressClockUpperFrameCharMinutes.Repeat(Console.WindowWidth - 10) + ProgressClockUpperRightCornerCharMinutes, 4, CInt(Console.WindowHeight / 2) - 2, True, ColorStorageMinutes) 'Top of Minutes

                        'Seconds
                        WriteWhere(ProgressClockLowerLeftCornerCharSeconds + ProgressClockLowerFrameCharSeconds.Repeat(Console.WindowWidth - 10) + ProgressClockLowerRightCornerCharSeconds, 4, CInt(Console.WindowHeight / 2) + 9, True, ColorStorageSeconds) 'Bottom of Seconds
                        WriteWhere(ProgressClockLeftFrameCharSeconds + " ".Repeat(Console.WindowWidth - 10) + ProgressClockRightFrameCharSeconds, 4, ProgressFillPositionSeconds, True, ColorStorageSeconds)                                                   'Medium of Seconds
                        WriteWhere(ProgressClockUpperLeftCornerCharSeconds + ProgressClockUpperFrameCharSeconds.Repeat(Console.WindowWidth - 10) + ProgressClockUpperRightCornerCharSeconds, 4, CInt(Console.WindowHeight / 2) + 7, True, ColorStorageSeconds) 'Top of Seconds

                        'Fill progress for hours, minutes, and seconds
                        If Not KernelDateTime.Hour = 0 Then WriteWhere(" ".Repeat(PercentRepeat(KernelDateTime.Hour, 24, 10)), 5, ProgressFillPositionHours, True, New Color(0), ColorStorageHours)
                        If Not KernelDateTime.Minute = 0 Then WriteWhere(" ".Repeat(PercentRepeat(KernelDateTime.Minute, 60, 10)), 5, ProgressFillPositionMinutes, True, New Color(0), ColorStorageMinutes)
                        If Not KernelDateTime.Second = 0 Then WriteWhere(" ".Repeat(PercentRepeat(KernelDateTime.Second, 60, 10)), 5, ProgressFillPositionSeconds, True, New Color(0), ColorStorageSeconds)

                        'Print information
                        If Not String.IsNullOrEmpty(ProgressClockInfoTextHours) Then
                            WriteWhere(ProbePlaces(ProgressClockInfoTextHours), 4, InformationPositionHours, True, ColorStorageHours, KernelDateTime.Hour)
                        Else
                            WriteWhere("H: {0}/24", 4, InformationPositionHours, True, ColorStorageHours, KernelDateTime.Hour)
                        End If
                        If Not String.IsNullOrEmpty(ProgressClockInfoTextMinutes) Then
                            WriteWhere(ProbePlaces(ProgressClockInfoTextMinutes), 4, InformationPositionMinutes, True, ColorStorageMinutes, KernelDateTime.Minute)
                        Else
                            WriteWhere("M: {0}/60", 4, InformationPositionMinutes, True, ColorStorageMinutes, KernelDateTime.Minute)
                        End If
                        If Not String.IsNullOrEmpty(ProgressClockInfoTextHours) Then
                            WriteWhere(ProbePlaces(ProgressClockInfoTextSeconds), 4, InformationPositionSeconds, True, ColorStorageSeconds, KernelDateTime.Second)
                        Else
                            WriteWhere("S: {0}/60", 4, InformationPositionSeconds, True, ColorStorageSeconds, KernelDateTime.Second)
                        End If

                        'Print date information
                        WriteWhere(Render, Console.WindowWidth / 2 - Render.Length / 2, Console.WindowHeight - 2, True, ColorStorageSeconds)
                    End If
                    If ProgressClockCycleColors Then CurrentTicks += 1

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(ProgressClockDelay, ProgressClock)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace