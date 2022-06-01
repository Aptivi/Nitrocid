
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

Namespace Misc.Screensaver.Displays
    Module BeatFaderDisplay

        Public BeatFader As New KernelThread("BeatFader screensaver thread", True, AddressOf BeatFader_DoWork)

        ''' <summary>
        ''' Handles the code of FaderBack
        ''' </summary>
        Sub BeatFader_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                Console.BackgroundColor = ConsoleColor.Black
                Console.Clear()
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Sanity checks for color levels
                If BeatFaderTrueColor Or BeatFader255Colors Then
                    BeatFaderMinimumRedColorLevel = If(BeatFaderMinimumRedColorLevel >= 0 And BeatFaderMinimumRedColorLevel <= 255, BeatFaderMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", BeatFaderMinimumRedColorLevel)
                    BeatFaderMinimumGreenColorLevel = If(BeatFaderMinimumGreenColorLevel >= 0 And BeatFaderMinimumGreenColorLevel <= 255, BeatFaderMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", BeatFaderMinimumGreenColorLevel)
                    BeatFaderMinimumBlueColorLevel = If(BeatFaderMinimumBlueColorLevel >= 0 And BeatFaderMinimumBlueColorLevel <= 255, BeatFaderMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", BeatFaderMinimumBlueColorLevel)
                    BeatFaderMinimumColorLevel = If(BeatFaderMinimumColorLevel >= 0 And BeatFaderMinimumColorLevel <= 255, BeatFaderMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", BeatFaderMinimumColorLevel)
                    BeatFaderMaximumRedColorLevel = If(BeatFaderMaximumRedColorLevel >= 0 And BeatFaderMaximumRedColorLevel <= 255, BeatFaderMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", BeatFaderMaximumRedColorLevel)
                    BeatFaderMaximumGreenColorLevel = If(BeatFaderMaximumGreenColorLevel >= 0 And BeatFaderMaximumGreenColorLevel <= 255, BeatFaderMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", BeatFaderMaximumGreenColorLevel)
                    BeatFaderMaximumBlueColorLevel = If(BeatFaderMaximumBlueColorLevel >= 0 And BeatFaderMaximumBlueColorLevel <= 255, BeatFaderMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", BeatFaderMaximumBlueColorLevel)
                    BeatFaderMaximumColorLevel = If(BeatFaderMaximumColorLevel >= 0 And BeatFaderMaximumColorLevel <= 255, BeatFaderMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", BeatFaderMaximumColorLevel)
                Else
                    BeatFaderMinimumColorLevel = If(BeatFaderMinimumColorLevel >= 0 And BeatFaderMinimumColorLevel <= 15, BeatFaderMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", BeatFaderMinimumColorLevel)
                    BeatFaderMaximumColorLevel = If(BeatFaderMaximumColorLevel >= 0 And BeatFaderMaximumColorLevel <= 15, BeatFaderMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", BeatFaderMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    Dim BeatInterval As Integer = 60000 / BeatFaderDelay
                    Dim BeatIntervalStep As Integer = BeatInterval / BeatFaderMaxSteps
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", BeatFaderDelay, BeatInterval)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", BeatFaderDelay, BeatIntervalStep)
                    SleepNoBlock(BeatIntervalStep, BeatFader)

                    'If we're cycling colors, set them. Else, use the user-provided color
                    Dim RedColorNum, GreenColorNum, BlueColorNum As Integer
                    If BeatFaderCycleColors Then
                        'We're cycling. Select the color mode, starting from true color
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors...")
                        If BeatFaderTrueColor Then
                            RedColorNum = RandomDriver.Next(BeatFaderMinimumRedColorLevel, BeatFaderMinimumRedColorLevel)
                            GreenColorNum = RandomDriver.Next(BeatFaderMinimumGreenColorLevel, BeatFaderMaximumGreenColorLevel)
                            BlueColorNum = RandomDriver.Next(BeatFaderMinimumBlueColorLevel, BeatFaderMaximumBlueColorLevel)
                        ElseIf BeatFader255Colors Then
                            Dim ConsoleColor As New ConsoleColorsInfo(RandomDriver.Next(BeatFaderMinimumColorLevel, BeatFaderMaximumColorLevel))
                            RedColorNum = ConsoleColor.R
                            GreenColorNum = ConsoleColor.G
                            BlueColorNum = ConsoleColor.B
                        Else
                            Dim ConsoleColor As New ConsoleColorsInfo(RandomDriver.Next(BeatFaderMinimumColorLevel, BeatFaderMaximumColorLevel))
                            RedColorNum = ConsoleColor.R
                            GreenColorNum = ConsoleColor.G
                            BlueColorNum = ConsoleColor.B
                        End If
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                    Else
                        'We're not cycling. Parse the color and then select the color mode, starting from true color
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", BeatFaderBeatColor)
                        Dim UserColor As New Color(BeatFaderBeatColor)
                        If UserColor.Type = ColorType.TrueColor Then
                            RedColorNum = UserColor.R
                            GreenColorNum = UserColor.G
                            BlueColorNum = UserColor.B
                        ElseIf UserColor.Type = ColorType._255Color Then
                            Dim ConsoleColor As New ConsoleColorsInfo(UserColor.PlainSequence)
                            RedColorNum = ConsoleColor.R
                            GreenColorNum = ConsoleColor.G
                            BlueColorNum = ConsoleColor.B
                        End If
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                    End If

                    'Set thresholds
                    Dim ThresholdRed As Double = RedColorNum / BeatFaderMaxSteps
                    Dim ThresholdGreen As Double = GreenColorNum / BeatFaderMaxSteps
                    Dim ThresholdBlue As Double = BlueColorNum / BeatFaderMaxSteps
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", ThresholdRed, ThresholdGreen, ThresholdBlue)

                    'Fade out
                    For CurrentStep As Integer = 1 To BeatFaderMaxSteps
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If ResizeSyncing Then Exit For
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", CurrentStep, BeatFaderMaxSteps, BeatIntervalStep)
                        SleepNoBlock(BeatIntervalStep, FaderBack)
                        Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                        Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                        Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        SetConsoleColor(New Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), True)
                        Console.Clear()
                    Next

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
