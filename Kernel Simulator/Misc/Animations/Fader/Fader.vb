
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

Imports KS.Misc.Screensaver

Namespace Misc.Animations.Fader
    Public Module Fader

        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        ''' <summary>
        ''' Simulates the fading animation
        ''' </summary>
        Public Sub Simulate(Settings As FaderSettings)
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Dim RandomDriver As Random = Settings.RandomDriver
            Dim RedColorNum As Integer = RandomDriver.Next(Settings.FaderMinimumRedColorLevel, Settings.FaderMaximumRedColorLevel)
            Dim GreenColorNum As Integer = RandomDriver.Next(Settings.FaderMinimumGreenColorLevel, Settings.FaderMaximumGreenColorLevel)
            Dim BlueColorNum As Integer = RandomDriver.Next(Settings.FaderMinimumBlueColorLevel, Settings.FaderMaximumBlueColorLevel)
            Console.CursorVisible = False

            'Check the text
            Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
            Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
            If Settings.FaderWrite.Length + Left >= Console.WindowWidth Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Text length of {0} exceeded window width of {1}.", Settings.FaderWrite.Length + Left, Console.WindowWidth)
                Left -= Settings.FaderWrite.Length + 1
            End If
            Console.SetCursorPosition(Left, Top)
            Console.BackgroundColor = ConsoleColor.Black
            ClearKeepPosition()

            'Set thresholds
            Dim ThresholdRed As Double = RedColorNum / Settings.FaderMaxSteps
            Dim ThresholdGreen As Double = GreenColorNum / Settings.FaderMaxSteps
            Dim ThresholdBlue As Double = BlueColorNum / Settings.FaderMaxSteps
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue)

            'Fade in
            Dim CurrentColorRedIn As Integer = 0
            Dim CurrentColorGreenIn As Integer = 0
            Dim CurrentColorBlueIn As Integer = 0
            For CurrentStep As Integer = Settings.FaderMaxSteps To 1 Step -1
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.FaderMaxSteps)
                SleepNoBlock(Settings.FaderDelay, System.Threading.Thread.CurrentThread)
                CurrentColorRedIn += ThresholdRed
                CurrentColorGreenIn += ThresholdGreen
                CurrentColorBlueIn += ThresholdBlue
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then WriteWhere(Settings.FaderWrite, Left, Top, True, New Color(CurrentColorRedIn & ";" & CurrentColorGreenIn & ";" & CurrentColorBlueIn), New Color(ConsoleColors.Black))
            Next

            'Wait until fade out
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", Settings.FaderFadeOutDelay)
            SleepNoBlock(Settings.FaderFadeOutDelay, ScreensaverDisplayerThread)

            'Fade out
            For CurrentStep As Integer = 1 To Settings.FaderMaxSteps
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.FaderMaxSteps)
                SleepNoBlock(Settings.FaderDelay, System.Threading.Thread.CurrentThread)
                Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut)
                If Not ResizeSyncing Then WriteWhere(Settings.FaderWrite, Left, Top, True, New Color(CurrentColorRedOut & ";" & CurrentColorGreenOut & ";" & CurrentColorBlueOut), New Color(ConsoleColors.Black))
            Next

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(Settings.FaderDelay, System.Threading.Thread.CurrentThread)
        End Sub

    End Module
End Namespace
