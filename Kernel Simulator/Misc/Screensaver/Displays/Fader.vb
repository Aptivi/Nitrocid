
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
    Module FaderDisplay

        Public Fader As New KernelThread("Fader screensaver thread", True, AddressOf Fader_DoWork)

        ''' <summary>
        ''' Handles the code of Fader
        ''' </summary>
        Sub Fader_DoWork()
            Try
                'Sanity checks for color levels
                FaderMinimumRedColorLevel = If(FaderMinimumRedColorLevel >= 0 And FaderMinimumRedColorLevel <= 255, FaderMinimumRedColorLevel, 0)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", FaderMinimumRedColorLevel)
                FaderMinimumGreenColorLevel = If(FaderMinimumGreenColorLevel >= 0 And FaderMinimumGreenColorLevel <= 255, FaderMinimumGreenColorLevel, 0)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", FaderMinimumGreenColorLevel)
                FaderMinimumBlueColorLevel = If(FaderMinimumBlueColorLevel >= 0 And FaderMinimumBlueColorLevel <= 255, FaderMinimumBlueColorLevel, 0)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", FaderMinimumBlueColorLevel)
                FaderMaximumRedColorLevel = If(FaderMaximumRedColorLevel >= 0 And FaderMaximumRedColorLevel <= 255, FaderMaximumRedColorLevel, 255)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", FaderMaximumRedColorLevel)
                FaderMaximumGreenColorLevel = If(FaderMaximumGreenColorLevel >= 0 And FaderMaximumGreenColorLevel <= 255, FaderMaximumGreenColorLevel, 255)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", FaderMaximumGreenColorLevel)
                FaderMaximumBlueColorLevel = If(FaderMaximumBlueColorLevel >= 0 And FaderMaximumBlueColorLevel <= 255, FaderMaximumBlueColorLevel, 255)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", FaderMaximumBlueColorLevel)

                'Variables
                Dim RandomDriver As New Random()
                Dim RedColorNum As Integer = RandomDriver.Next(FaderMinimumRedColorLevel, FaderMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(FaderMinimumGreenColorLevel, FaderMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(FaderMinimumBlueColorLevel, FaderMaximumBlueColorLevel)
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                SetConsoleColor(New Color(FaderBackgroundColor), True)
                Console.Clear()
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
                    If FaderWrite.Length + Left >= Console.WindowWidth Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Text length of {0} exceeded window width of {1}.", FaderWrite.Length + Left, Console.WindowWidth)
                        Left -= FaderWrite.Length + 1
                    End If
                    Console.SetCursorPosition(Left, Top)
                    Console.BackgroundColor = ConsoleColor.Black
                    ClearKeepPosition()

                    'Set thresholds
                    Dim ThresholdRed As Double = RedColorNum / FaderMaxSteps
                    Dim ThresholdGreen As Double = GreenColorNum / FaderMaxSteps
                    Dim ThresholdBlue As Double = BlueColorNum / FaderMaxSteps
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue)

                    'Fade in
                    Dim CurrentColorRedIn As Integer = 0
                    Dim CurrentColorGreenIn As Integer = 0
                    Dim CurrentColorBlueIn As Integer = 0
                    For CurrentStep As Integer = FaderMaxSteps To 1 Step -1
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If ResizeSyncing Then Exit For
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, FaderMaxSteps)
                        SleepNoBlock(FaderDelay, Fader)
                        CurrentColorRedIn += ThresholdRed
                        CurrentColorGreenIn += ThresholdGreen
                        CurrentColorBlueIn += ThresholdBlue
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then WriteWhere(FaderWrite, Left, Top, True, New Color(CurrentColorRedIn & ";" & CurrentColorGreenIn & ";" & CurrentColorBlueIn), New Color(ConsoleColors.Black))
                    Next

                    'Wait until fade out
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", FaderFadeOutDelay)
                    SleepNoBlock(FaderFadeOutDelay, Fader)

                    'Fade out
                    For CurrentStep As Integer = 1 To FaderMaxSteps
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If ResizeSyncing Then Exit For
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, FaderMaxSteps)
                        SleepNoBlock(FaderDelay, Fader)
                        Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                        Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                        Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut)
                        If Not ResizeSyncing Then WriteWhere(FaderWrite, Left, Top, True, New Color(CurrentColorRedOut & ";" & CurrentColorGreenOut & ";" & CurrentColorBlueOut), New Color(ConsoleColors.Black))
                    Next

                    'Select new color
                    RedColorNum = RandomDriver.Next(FaderMinimumRedColorLevel, FaderMaximumRedColorLevel)
                    GreenColorNum = RandomDriver.Next(FaderMinimumGreenColorLevel, FaderMaximumGreenColorLevel)
                    BlueColorNum = RandomDriver.Next(FaderMinimumBlueColorLevel, FaderMaximumBlueColorLevel)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(FaderDelay, Fader)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
