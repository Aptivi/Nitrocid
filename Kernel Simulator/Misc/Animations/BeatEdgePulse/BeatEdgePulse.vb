
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

Namespace Misc.Animations.BeatEdgePulse
    Public Module BeatEdgePulse

        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        ''' <summary>
        ''' Simulates the beat pulsing animation
        ''' </summary>
        Public Sub Simulate(Settings As BeatEdgePulseSettings)
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Dim RandomDriver As Random = Settings.RandomDriver
            Console.CursorVisible = False
            Dim BeatInterval As Integer = 60000 / Settings.BeatEdgePulseDelay
            Dim BeatIntervalStep As Integer = BeatInterval / Settings.BeatEdgePulseMaxSteps
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", Settings.BeatEdgePulseDelay, BeatInterval)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", Settings.BeatEdgePulseDelay, BeatIntervalStep)
            SleepNoBlock(BeatIntervalStep, ScreensaverDisplayerThread)

            'If we're cycling colors, set them. Else, use the user-provided color
            Dim RedColorNum, GreenColorNum, BlueColorNum As Integer
            If Settings.BeatEdgePulseCycleColors Then
                'We're cycling. Select the color mode, starting from true color
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors...")
                If Settings.BeatEdgePulseTrueColor Then
                    RedColorNum = RandomDriver.Next(Settings.BeatEdgePulseMinimumRedColorLevel, Settings.BeatEdgePulseMinimumRedColorLevel)
                    GreenColorNum = RandomDriver.Next(Settings.BeatEdgePulseMinimumGreenColorLevel, Settings.BeatEdgePulseMaximumGreenColorLevel)
                    BlueColorNum = RandomDriver.Next(Settings.BeatEdgePulseMinimumBlueColorLevel, Settings.BeatEdgePulseMaximumBlueColorLevel)
                ElseIf Settings.BeatEdgePulse255Colors Then
                    Dim ConsoleColor As New ConsoleColorsInfo(RandomDriver.Next(Settings.BeatEdgePulseMinimumColorLevel, Settings.BeatEdgePulseMaximumColorLevel))
                    RedColorNum = ConsoleColor.R
                    GreenColorNum = ConsoleColor.G
                    BlueColorNum = ConsoleColor.B
                Else
                    Dim ConsoleColor As New ConsoleColorsInfo(RandomDriver.Next(Settings.BeatEdgePulseMinimumColorLevel, Settings.BeatEdgePulseMaximumColorLevel))
                    RedColorNum = ConsoleColor.R
                    GreenColorNum = ConsoleColor.G
                    BlueColorNum = ConsoleColor.B
                End If
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
            Else
                'We're not cycling. Parse the color and then select the color mode, starting from true color
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", Settings.BeatEdgePulseBeatColor)
                Dim UserColor As New Color(Settings.BeatEdgePulseBeatColor)
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
            Dim ThresholdRed As Double = RedColorNum / Settings.BeatEdgePulseMaxSteps
            Dim ThresholdGreen As Double = GreenColorNum / Settings.BeatEdgePulseMaxSteps
            Dim ThresholdBlue As Double = BlueColorNum / Settings.BeatEdgePulseMaxSteps
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", ThresholdRed, ThresholdGreen, ThresholdBlue)

            'Fade in
            Dim CurrentColorRedIn As Integer = 0
            Dim CurrentColorGreenIn As Integer = 0
            Dim CurrentColorBlueIn As Integer = 0
            For CurrentStep As Integer = Settings.BeatEdgePulseMaxSteps To 1 Step -1
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, BeatIntervalStep)
                SleepNoBlock(BeatIntervalStep, System.Threading.Thread.CurrentThread)
                CurrentColorRedIn += ThresholdRed
                CurrentColorGreenIn += ThresholdGreen
                CurrentColorBlueIn += ThresholdBlue
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    SetConsoleColor(New Color(CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn), True)
                    FillIn()
                End If
            Next

            'Fade out
            For CurrentStep As Integer = 1 To Settings.BeatEdgePulseMaxSteps
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", CurrentStep, Settings.BeatEdgePulseMaxSteps, BeatIntervalStep)
                SleepNoBlock(BeatIntervalStep, System.Threading.Thread.CurrentThread)
                Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    SetConsoleColor(New Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), True)
                    FillIn()
                End If
            Next

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(Settings.BeatEdgePulseDelay, System.Threading.Thread.CurrentThread)
        End Sub

        Private Sub FillIn()
            Dim FloorTopLeftEdge As Integer = 0
            Dim FloorBottomLeftEdge As Integer = 0
            Wdbg(DebugLevel.I, "Top left edge: {0}, Bottom left edge: {1}", FloorTopLeftEdge, FloorBottomLeftEdge)

            Dim FloorTopRightEdge As Integer = Console.WindowWidth - 1
            Dim FloorBottomRightEdge As Integer = Console.WindowWidth - 1
            Wdbg(DebugLevel.I, "Top right edge: {0}, Bottom right edge: {1}", FloorTopRightEdge, FloorBottomRightEdge)

            Dim FloorTopEdge As Integer = 0
            Dim FloorBottomEdge As Integer = Console.WindowHeight - 1
            Wdbg(DebugLevel.I, "Top edge: {0}, Bottom edge: {1}", FloorTopEdge, FloorBottomEdge)

            Dim FloorLeftEdge As Integer = 0
            Dim FloorRightEdge As Integer = Console.WindowWidth - 2
            Wdbg(DebugLevel.I, "Left edge: {0}, Right edge: {1}", FloorLeftEdge, FloorRightEdge)

            'First, draw the floor top edge
            For x As Integer = FloorTopLeftEdge To FloorTopRightEdge
                Console.SetCursorPosition(x, 0)
                Wdbg(DebugLevel.I, "Drawing floor top edge ({0}, {1})", x, 1)
                Console.Write(" ")
            Next

            'Second, draw the floor bottom edge
            For x As Integer = FloorBottomLeftEdge To FloorBottomRightEdge
                Console.SetCursorPosition(x, FloorBottomEdge)
                Wdbg(DebugLevel.I, "Drawing floor bottom edge ({0}, {1})", x, FloorBottomEdge)
                Console.Write(" ")
            Next

            'Third, draw the floor left edge
            For y As Integer = FloorTopEdge To FloorBottomEdge
                Console.SetCursorPosition(FloorLeftEdge, y)
                Wdbg(DebugLevel.I, "Drawing floor left edge ({0}, {1})", FloorLeftEdge, y)
                Console.Write("  ")
            Next

            'Finally, draw the floor right edge
            For y As Integer = FloorTopEdge To FloorBottomEdge
                Console.SetCursorPosition(FloorRightEdge, y)
                Wdbg(DebugLevel.I, "Drawing floor right edge ({0}, {1})", FloorRightEdge, y)
                Console.Write("  ")
            Next
        End Sub

    End Module
End Namespace
