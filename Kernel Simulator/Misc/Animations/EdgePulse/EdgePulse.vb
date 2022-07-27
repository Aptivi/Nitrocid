
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

Namespace Misc.Animations.EdgePulse
    Public Module EdgePulse

        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        ''' <summary>
        ''' Simulates the edge pulsing animation
        ''' </summary>
        Public Sub Simulate(Settings As EdgePulseSettings)
            'Now, do the rest
            Dim RandomDriver As Random = Settings.RandomDriver
            Dim RedColorNum As Integer = RandomDriver.Next(Settings.EdgePulseMinimumRedColorLevel, Settings.EdgePulseMaximumRedColorLevel)
            Dim GreenColorNum As Integer = RandomDriver.Next(Settings.EdgePulseMinimumGreenColorLevel, Settings.EdgePulseMaximumGreenColorLevel)
            Dim BlueColorNum As Integer = RandomDriver.Next(Settings.EdgePulseMinimumBlueColorLevel, Settings.EdgePulseMaximumBlueColorLevel)
            Console.CursorVisible = False

            'Set thresholds
            Dim ThresholdRed As Double = RedColorNum / Settings.EdgePulseMaxSteps
            Dim ThresholdGreen As Double = GreenColorNum / Settings.EdgePulseMaxSteps
            Dim ThresholdBlue As Double = BlueColorNum / Settings.EdgePulseMaxSteps
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue)

            'Fade in
            Dim CurrentColorRedIn As Integer = 0
            Dim CurrentColorGreenIn As Integer = 0
            Dim CurrentColorBlueIn As Integer = 0
            For CurrentStep As Integer = Settings.EdgePulseMaxSteps To 1 Step -1
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.EdgePulseMaxSteps)
                SleepNoBlock(Settings.EdgePulseDelay, System.Threading.Thread.CurrentThread)
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
            For CurrentStep As Integer = 1 To Settings.EdgePulseMaxSteps
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, Settings.EdgePulseMaxSteps)
                SleepNoBlock(Settings.EdgePulseDelay, System.Threading.Thread.CurrentThread)
                Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut)
                If Not ResizeSyncing Then
                    SetConsoleColor(New Color(CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut), True)
                    FillIn()
                End If
            Next

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(Settings.EdgePulseDelay, System.Threading.Thread.CurrentThread)
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
