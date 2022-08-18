
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

Namespace Misc.Animations.BeatFader
    Public Module BeatFader

        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        ''' <summary>
        ''' Simulates the beat fading animation
        ''' </summary>
        Public Sub Simulate(Settings As BeatFaderSettings)
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Dim RandomDriver As Random = Settings.RandomDriver
            Console.CursorVisible = False
            Dim BeatInterval As Integer = 60000 / Settings.BeatFaderDelay
            Dim BeatIntervalStep As Integer = BeatInterval / Settings.BeatFaderMaxSteps
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", Settings.BeatFaderDelay, BeatInterval)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", Settings.BeatFaderDelay, BeatIntervalStep)
            SleepNoBlock(BeatIntervalStep, ScreensaverDisplayerThread)

            'If we're cycling colors, set them. Else, use the user-provided color
            Dim RedColorNum, GreenColorNum, BlueColorNum As Integer
            If Settings.BeatFaderCycleColors Then
                'We're cycling. Select the color mode, starting from true color
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors...")
                If Settings.BeatFaderTrueColor Then
                    RedColorNum = RandomDriver.Next(Settings.BeatFaderMinimumRedColorLevel, Settings.BeatFaderMinimumRedColorLevel)
                    GreenColorNum = RandomDriver.Next(Settings.BeatFaderMinimumGreenColorLevel, Settings.BeatFaderMaximumGreenColorLevel)
                    BlueColorNum = RandomDriver.Next(Settings.BeatFaderMinimumBlueColorLevel, Settings.BeatFaderMaximumBlueColorLevel)
                ElseIf Settings.BeatFader255Colors Then
                    Dim ConsoleColor As New ConsoleColorsInfo(RandomDriver.Next(Settings.BeatFaderMinimumColorLevel, Settings.BeatFaderMaximumColorLevel))
                    RedColorNum = ConsoleColor.R
                    GreenColorNum = ConsoleColor.G
                    BlueColorNum = ConsoleColor.B
                Else
                    Dim ConsoleColor As New ConsoleColorsInfo(RandomDriver.Next(Settings.BeatFaderMinimumColorLevel, Settings.BeatFaderMaximumColorLevel))
                    RedColorNum = ConsoleColor.R
                    GreenColorNum = ConsoleColor.G
                    BlueColorNum = ConsoleColor.B
                End If
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
            Else
                'We're not cycling. Parse the color and then select the color mode, starting from true color
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", Settings.BeatFaderBeatColor)
                Dim UserColor As New Color(Settings.BeatFaderBeatColor)
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
            Dim ThresholdRed As Double = RedColorNum / Settings.BeatFaderMaxSteps
            Dim ThresholdGreen As Double = GreenColorNum / Settings.BeatFaderMaxSteps
            Dim ThresholdBlue As Double = BlueColorNum / Settings.BeatFaderMaxSteps
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", ThresholdRed, ThresholdGreen, ThresholdBlue)

            'Fade out
            For CurrentStep As Integer = 1 To Settings.BeatFaderMaxSteps
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", CurrentStep, Settings.BeatFaderMaxSteps, BeatIntervalStep)
                SleepNoBlock(BeatIntervalStep, System.Threading.Thread.CurrentThread)
                Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    SetConsoleColor(New Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), True, True)
                    Console.Clear()
                End If
            Next

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(Settings.BeatFaderDelay, System.Threading.Thread.CurrentThread)
        End Sub

    End Module
End Namespace
