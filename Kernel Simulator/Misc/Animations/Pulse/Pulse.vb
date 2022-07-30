
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports KS.Misc.Screensaver

Namespace Misc.Animations.Pulse
    Public Module Pulse

        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        ''' <summary>
        ''' Simulates the pulsing animation
        ''' </summary>
        Public Sub Simulate(Settings As PulseSettings)
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Dim RandomDriver As Random = Settings.RandomDriver
            Dim RedColorNum As Integer = RandomDriver.Next(Settings.PulseMinimumRedColorLevel, Settings.PulseMaximumRedColorLevel)
            Dim GreenColorNum As Integer = RandomDriver.Next(Settings.PulseMinimumGreenColorLevel, Settings.PulseMaximumGreenColorLevel)
            Dim BlueColorNum As Integer = RandomDriver.Next(Settings.PulseMinimumBlueColorLevel, Settings.PulseMaximumBlueColorLevel)
            Console.CursorVisible = False

            'Set thresholds
            Dim ThresholdRed As Double = RedColorNum / Settings.PulseMaxSteps
            Dim ThresholdGreen As Double = GreenColorNum / Settings.PulseMaxSteps
            Dim ThresholdBlue As Double = BlueColorNum / Settings.PulseMaxSteps
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue)

            'Fade in
            Dim CurrentColorRedIn As Integer = 0
            Dim CurrentColorGreenIn As Integer = 0
            Dim CurrentColorBlueIn As Integer = 0
            For CurrentStep As Integer = Settings.PulseMaxSteps To 1 Step -1
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.PulseMaxSteps)
                SleepNoBlock(Settings.PulseDelay, System.Threading.Thread.CurrentThread)
                CurrentColorRedIn += ThresholdRed
                CurrentColorGreenIn += ThresholdGreen
                CurrentColorBlueIn += ThresholdBlue
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    SetConsoleColor(New Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn), True)
                    Console.Clear()
                End If
            Next

            'Fade out
            For CurrentStep As Integer = 1 To Settings.PulseMaxSteps
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.PulseMaxSteps)
                SleepNoBlock(Settings.PulseDelay, System.Threading.Thread.CurrentThread)
                Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut)
                If Not ResizeSyncing Then
                    SetConsoleColor(New Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut), True)
                    Console.Clear()
                End If
            Next

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(Settings.PulseDelay, System.Threading.Thread.CurrentThread)
        End Sub

    End Module
End Namespace
