
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

Module BarRotDisplay

    Public WithEvents BarRot As New NamedBackgroundWorker("BarRot screensaver thread") With {.WorkerSupportsCancellation = True}

    Sub BarRot_DoWork(sender As Object, e As DoWorkEventArgs) Handles BarRot.DoWork
        'Sanity checks for color levels
        BarRotMinimumRedColorLevelStart = If(BarRotMinimumRedColorLevelStart >= 0 And BarRotMinimumRedColorLevelStart <= 255, BarRotMinimumRedColorLevelStart, 0)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level (Start): {0}", BarRotMinimumRedColorLevelStart)
        BarRotMinimumGreenColorLevelStart = If(BarRotMinimumGreenColorLevelStart >= 0 And BarRotMinimumGreenColorLevelStart <= 255, BarRotMinimumGreenColorLevelStart, 0)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level (Start): {0}", BarRotMinimumGreenColorLevelStart)
        BarRotMinimumBlueColorLevelStart = If(BarRotMinimumBlueColorLevelStart >= 0 And BarRotMinimumBlueColorLevelStart <= 255, BarRotMinimumBlueColorLevelStart, 0)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level (Start): {0}", BarRotMinimumBlueColorLevelStart)
        BarRotMaximumRedColorLevelStart = If(BarRotMaximumRedColorLevelStart >= 0 And BarRotMaximumRedColorLevelStart <= 255, BarRotMaximumRedColorLevelStart, 255)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level (Start): {0}", BarRotMaximumRedColorLevelStart)
        BarRotMaximumGreenColorLevelStart = If(BarRotMaximumGreenColorLevelStart >= 0 And BarRotMaximumGreenColorLevelStart <= 255, BarRotMaximumGreenColorLevelStart, 255)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level (Start): {0}", BarRotMaximumGreenColorLevelStart)
        BarRotMaximumBlueColorLevelStart = If(BarRotMaximumBlueColorLevelStart >= 0 And BarRotMaximumBlueColorLevelStart <= 255, BarRotMaximumBlueColorLevelStart, 255)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level (Start): {0}", BarRotMaximumBlueColorLevelStart)
        BarRotMinimumRedColorLevelEnd = If(BarRotMinimumRedColorLevelEnd >= 0 And BarRotMinimumRedColorLevelEnd <= 255, BarRotMinimumRedColorLevelEnd, 0)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level (End): {0}", BarRotMinimumRedColorLevelEnd)
        BarRotMinimumGreenColorLevelEnd = If(BarRotMinimumGreenColorLevelEnd >= 0 And BarRotMinimumGreenColorLevelEnd <= 255, BarRotMinimumGreenColorLevelEnd, 0)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level (End): {0}", BarRotMinimumGreenColorLevelEnd)
        BarRotMinimumBlueColorLevelEnd = If(BarRotMinimumBlueColorLevelEnd >= 0 And BarRotMinimumBlueColorLevelEnd <= 255, BarRotMinimumBlueColorLevelEnd, 0)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level (End): {0}", BarRotMinimumBlueColorLevelEnd)
        BarRotMaximumRedColorLevelEnd = If(BarRotMaximumRedColorLevelEnd >= 0 And BarRotMaximumRedColorLevelEnd <= 255, BarRotMaximumRedColorLevelEnd, 255)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level (End): {0}", BarRotMaximumRedColorLevelEnd)
        BarRotMaximumGreenColorLevelEnd = If(BarRotMaximumGreenColorLevelEnd >= 0 And BarRotMaximumGreenColorLevelEnd <= 255, BarRotMaximumGreenColorLevelEnd, 255)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level (End): {0}", BarRotMaximumGreenColorLevelEnd)
        BarRotMaximumBlueColorLevelEnd = If(BarRotMaximumBlueColorLevelEnd >= 0 And BarRotMaximumBlueColorLevelEnd <= 255, BarRotMaximumBlueColorLevelEnd, 255)
        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level (End): {0}", BarRotMaximumBlueColorLevelEnd)

        'Variables
        Dim RandomDriver As New Random()
        Dim RedColorNumFrom As Integer = RandomDriver.Next(BarRotMinimumRedColorLevelStart, BarRotMaximumRedColorLevelStart)
        Dim GreenColorNumFrom As Integer = RandomDriver.Next(BarRotMinimumGreenColorLevelStart, BarRotMaximumGreenColorLevelStart)
        Dim BlueColorNumFrom As Integer = RandomDriver.Next(BarRotMinimumBlueColorLevelStart, BarRotMaximumBlueColorLevelStart)
        Dim RedColorNumTo As Integer = RandomDriver.Next(BarRotMinimumRedColorLevelEnd, BarRotMaximumRedColorLevelEnd)
        Dim GreenColorNumTo As Integer = RandomDriver.Next(BarRotMinimumGreenColorLevelEnd, BarRotMaximumGreenColorLevelEnd)
        Dim BlueColorNumTo As Integer = RandomDriver.Next(BarRotMinimumBlueColorLevelEnd, BarRotMaximumBlueColorLevelEnd)
        Dim CurrentWindowWidth As Integer = Console.WindowWidth
        Dim CurrentWindowHeight As Integer = Console.WindowHeight
        Dim ResizeSyncing As Boolean

        'Preparations
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()

        'Screensaver logic
        Do While True
            Console.CursorVisible = False
            If BarRot.CancellationPending = True Then
                HandleSaverCancel()
                Exit Do
            Else
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

                'Select a color range for the ramp
                RedColorNumFrom = RandomDriver.Next(BarRotMinimumRedColorLevelStart, BarRotMaximumRedColorLevelStart)
                GreenColorNumFrom = RandomDriver.Next(BarRotMinimumGreenColorLevelStart, BarRotMaximumGreenColorLevelStart)
                BlueColorNumFrom = RandomDriver.Next(BarRotMinimumBlueColorLevelStart, BarRotMaximumBlueColorLevelStart)
                RedColorNumTo = RandomDriver.Next(BarRotMinimumRedColorLevelEnd, BarRotMaximumRedColorLevelEnd)
                GreenColorNumTo = RandomDriver.Next(BarRotMinimumGreenColorLevelEnd, BarRotMaximumGreenColorLevelEnd)
                BlueColorNumTo = RandomDriver.Next(BarRotMinimumBlueColorLevelEnd, BarRotMaximumBlueColorLevelEnd)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo)

                'Set start and end widths for the ramp frame
                Dim RampFrameStartWidth As Integer = 4
                Dim RampFrameEndWidth As Integer = Console.WindowWidth - RampFrameStartWidth
                Dim RampFrameSpaces As Integer = RampFrameEndWidth - RampFrameStartWidth

                'Set thresholds for color ramp
                Dim RampColorRedThreshold As Integer = RedColorNumFrom - RedColorNumTo
                Dim RampColorGreenThreshold As Integer = GreenColorNumFrom - GreenColorNumTo
                Dim RampColorBlueThreshold As Integer = BlueColorNumFrom - BlueColorNumTo
                Dim RampColorRedSteps As Double = RampColorRedThreshold / RampFrameSpaces
                Dim RampColorGreenSteps As Double = RampColorGreenThreshold / RampFrameSpaces
                Dim RampColorBlueSteps As Double = RampColorBlueThreshold / RampFrameSpaces

                'Let the ramp be printed in the center
                Dim RampCenterPosition As Integer = Console.WindowHeight / 2

                'Set the current positions
                Dim RampCurrentPositionLeft As Integer = RampFrameStartWidth + 1

                'Draw the frame
                If Not ResizeSyncing Then
                    WriteWhereC(BarRotUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, False, If(BarRotUseBorderColors, New Color(BarRotUpperLeftCornerColor), New Color(ConsoleColors.Gray)))
                    WriteC(StrDup(RampFrameSpaces, BarRotUpperFrameChar), False, If(BarRotUseBorderColors, New Color(BarRotUpperFrameColor), New Color(ConsoleColors.Gray)))
                    WriteC(BarRotUpperRightCornerChar, False, If(BarRotUseBorderColors, New Color(BarRotUpperRightCornerColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(BarRotLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(BarRotRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, False, If(BarRotUseBorderColors, New Color(BarRotLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(BarRotLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, False, If(BarRotUseBorderColors, New Color(BarRotLowerLeftCornerColor), New Color(ConsoleColors.Gray)))
                    WriteC(StrDup(RampFrameSpaces, BarRotLowerFrameChar), False, If(BarRotUseBorderColors, New Color(BarRotLowerFrameColor), New Color(ConsoleColors.Gray)))
                    WriteC(BarRotLowerRightCornerChar, False, If(BarRotUseBorderColors, New Color(BarRotLowerRightCornerColor), New Color(ConsoleColors.Gray)))
                End If

                'Set the current colors
                Dim RampCurrentColorRed As Double = RedColorNumFrom
                Dim RampCurrentColorGreen As Double = GreenColorNumFrom
                Dim RampCurrentColorBlue As Double = BlueColorNumFrom

                'Set the console color and fill the ramp!
                Do Until Convert.ToInt32(RampCurrentColorRed) = RedColorNumTo And Convert.ToInt32(RampCurrentColorGreen) = GreenColorNumTo And Convert.ToInt32(RampCurrentColorBlue) = BlueColorNumTo
                    'Populate the variables for sub-gradients
                    Dim RampSubgradientRedColorNumFrom As Integer = RedColorNumFrom
                    Dim RampSubgradientGreenColorNumFrom As Integer = GreenColorNumFrom
                    Dim RampSubgradientBlueColorNumFrom As Integer = BlueColorNumFrom
                    Dim RampSubgradientRedColorNumTo As Integer = RampCurrentColorRed
                    Dim RampSubgradientGreenColorNumTo As Integer = RampCurrentColorGreen
                    Dim RampSubgradientBlueColorNumTo As Integer = RampCurrentColorBlue

                    'Set the sub-gradient current colors
                    Dim RampSubgradientCurrentColorRed As Double = RampSubgradientRedColorNumFrom
                    Dim RampSubgradientCurrentColorGreen As Double = RampSubgradientGreenColorNumFrom
                    Dim RampSubgradientCurrentColorBlue As Double = RampSubgradientBlueColorNumFrom

                    'Set the sub-gradient thresholds
                    Dim RampSubgradientColorRedThreshold As Integer = RampSubgradientRedColorNumFrom - RampSubgradientRedColorNumTo
                    Dim RampSubgradientColorGreenThreshold As Integer = RampSubgradientGreenColorNumFrom - RampSubgradientGreenColorNumTo
                    Dim RampSubgradientColorBlueThreshold As Integer = RampSubgradientBlueColorNumFrom - RampSubgradientBlueColorNumTo
                    Dim RampSubgradientColorRedSteps As Double = RampSubgradientColorRedThreshold / RampFrameSpaces
                    Dim RampSubgradientColorGreenSteps As Double = RampSubgradientColorGreenThreshold / RampFrameSpaces
                    Dim RampSubgradientColorBlueSteps As Double = RampSubgradientColorBlueThreshold / RampFrameSpaces

                    'Make a new instance
                    Dim RampSubgradientCurrentColorInstance As New Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}")
                    SetConsoleColor(RampSubgradientCurrentColorInstance, True)

                    'Try to fill the ramp
                    Dim RampSubgradientStepsMade As Integer = 0
                    Do Until RampSubgradientStepsMade = RampFrameSpaces
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If BarRot.CancellationPending Then Exit Do
                        If ResizeSyncing Then Exit Do
                        Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition - 1)
                        Console.Write(" "c)
                        Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition)
                        Console.Write(" "c)
                        Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition + 1)
                        Console.Write(" "c)
                        RampCurrentPositionLeft = Console.CursorLeft
                        RampSubgradientStepsMade += 1

                        'Change the colors
                        RampSubgradientCurrentColorRed -= RampSubgradientColorRedSteps
                        RampSubgradientCurrentColorGreen -= RampSubgradientColorGreenSteps
                        RampSubgradientCurrentColorBlue -= RampSubgradientColorBlueSteps
                        RampSubgradientCurrentColorInstance = New Color($"{Convert.ToInt32(RampSubgradientCurrentColorRed)};{Convert.ToInt32(RampSubgradientCurrentColorGreen)};{Convert.ToInt32(RampSubgradientCurrentColorBlue)}")
                        SetConsoleColor(RampSubgradientCurrentColorInstance, True)
                    Loop

                    'Change the colors
                    RampCurrentColorRed -= RampColorRedSteps
                    RampCurrentColorGreen -= RampColorGreenSteps
                    RampCurrentColorBlue -= RampColorBlueSteps

                    'Delay writing
                    RampCurrentPositionLeft = RampFrameStartWidth + 1
                    SleepNoBlock(BarRotDelay, BarRot)
                Loop
                SleepNoBlock(BarRotNextRampDelay, BarRot)
                Console.BackgroundColor = ConsoleColor.Black
                Console.Clear()
                ResizeSyncing = False
                CurrentWindowWidth = Console.WindowWidth
                CurrentWindowHeight = Console.WindowHeight
            End If
            SleepNoBlock(BarRotDelay, BarRot)
        Loop
    End Sub

    ''' <summary>
    ''' Checks for any screensaver error
    ''' </summary>
    Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles BarRot.RunWorkerCompleted
        HandleSaverError(e.Error)
    End Sub

End Module
