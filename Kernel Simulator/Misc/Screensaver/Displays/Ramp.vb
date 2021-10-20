
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

Module RampDisplay

    Public WithEvents Ramp As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    Sub Ramp_DoWork(sender As Object, e As DoWorkEventArgs) Handles Ramp.DoWork
        'Sanity checks for color levels
        If RampTrueColor Or Ramp255Colors Then
            RampMinimumRedColorLevelStart = If(RampMinimumRedColorLevelStart >= 0 And RampMinimumRedColorLevelStart <= 255, RampMinimumRedColorLevelStart, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level (Start): {0}", RampMinimumRedColorLevelStart)
            RampMinimumGreenColorLevelStart = If(RampMinimumGreenColorLevelStart >= 0 And RampMinimumGreenColorLevelStart <= 255, RampMinimumGreenColorLevelStart, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level (Start): {0}", RampMinimumGreenColorLevelStart)
            RampMinimumBlueColorLevelStart = If(RampMinimumBlueColorLevelStart >= 0 And RampMinimumBlueColorLevelStart <= 255, RampMinimumBlueColorLevelStart, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level (Start): {0}", RampMinimumBlueColorLevelStart)
            RampMinimumColorLevelStart = If(RampMinimumColorLevelStart >= 0 And RampMinimumColorLevelStart <= 255, RampMinimumColorLevelStart, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level (Start): {0}", RampMinimumColorLevelStart)
            RampMaximumRedColorLevelStart = If(RampMaximumRedColorLevelStart >= 0 And RampMaximumRedColorLevelStart <= 255, RampMaximumRedColorLevelStart, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level (Start): {0}", RampMaximumRedColorLevelStart)
            RampMaximumGreenColorLevelStart = If(RampMaximumGreenColorLevelStart >= 0 And RampMaximumGreenColorLevelStart <= 255, RampMaximumGreenColorLevelStart, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level (Start): {0}", RampMaximumGreenColorLevelStart)
            RampMaximumBlueColorLevelStart = If(RampMaximumBlueColorLevelStart >= 0 And RampMaximumBlueColorLevelStart <= 255, RampMaximumBlueColorLevelStart, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level (Start): {0}", RampMaximumBlueColorLevelStart)
            RampMaximumColorLevelStart = If(RampMaximumColorLevelStart >= 0 And RampMaximumColorLevelStart <= 255, RampMaximumColorLevelStart, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level (Start): {0}", RampMaximumColorLevelStart)
            RampMinimumRedColorLevelEnd = If(RampMinimumRedColorLevelEnd >= 0 And RampMinimumRedColorLevelEnd <= 255, RampMinimumRedColorLevelEnd, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level (End): {0}", RampMinimumRedColorLevelEnd)
            RampMinimumGreenColorLevelEnd = If(RampMinimumGreenColorLevelEnd >= 0 And RampMinimumGreenColorLevelEnd <= 255, RampMinimumGreenColorLevelEnd, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level (End): {0}", RampMinimumGreenColorLevelEnd)
            RampMinimumBlueColorLevelEnd = If(RampMinimumBlueColorLevelEnd >= 0 And RampMinimumBlueColorLevelEnd <= 255, RampMinimumBlueColorLevelEnd, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level (End): {0}", RampMinimumBlueColorLevelEnd)
            RampMinimumColorLevelEnd = If(RampMinimumColorLevelEnd >= 0 And RampMinimumColorLevelEnd <= 255, RampMinimumColorLevelEnd, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level (End): {0}", RampMinimumColorLevelEnd)
            RampMaximumRedColorLevelEnd = If(RampMaximumRedColorLevelEnd >= 0 And RampMaximumRedColorLevelEnd <= 255, RampMaximumRedColorLevelEnd, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level (End): {0}", RampMaximumRedColorLevelEnd)
            RampMaximumGreenColorLevelEnd = If(RampMaximumGreenColorLevelEnd >= 0 And RampMaximumGreenColorLevelEnd <= 255, RampMaximumGreenColorLevelEnd, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level (End): {0}", RampMaximumGreenColorLevelEnd)
            RampMaximumBlueColorLevelEnd = If(RampMaximumBlueColorLevelEnd >= 0 And RampMaximumBlueColorLevelEnd <= 255, RampMaximumBlueColorLevelEnd, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level (End): {0}", RampMaximumBlueColorLevelEnd)
            RampMaximumColorLevelEnd = If(RampMaximumColorLevelEnd >= 0 And RampMaximumColorLevelEnd <= 255, RampMaximumColorLevelEnd, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level (End): {0}", RampMaximumColorLevelEnd)
        Else
            RampMinimumColorLevelStart = If(RampMinimumColorLevelStart >= 0 And RampMinimumColorLevelStart <= 15, RampMinimumColorLevelStart, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level (Start): {0}", RampMinimumColorLevelStart)
            RampMaximumColorLevelStart = If(RampMaximumColorLevelStart >= 0 And RampMaximumColorLevelStart <= 15, RampMaximumColorLevelStart, 15)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level (Start): {0}", RampMaximumColorLevelStart)
            RampMinimumColorLevelEnd = If(RampMinimumColorLevelEnd >= 0 And RampMinimumColorLevelEnd <= 15, RampMinimumColorLevelEnd, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level (End): {0}", RampMinimumColorLevelEnd)
            RampMaximumColorLevelEnd = If(RampMaximumColorLevelEnd >= 0 And RampMaximumColorLevelEnd <= 15, RampMaximumColorLevelEnd, 15)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level (End): {0}", RampMaximumColorLevelEnd)
        End If

        'Variables
        Dim RandomDriver As New Random()
        Dim RedColorNumFrom As Integer = RandomDriver.Next(RampMinimumRedColorLevelStart, RampMaximumRedColorLevelStart)
        Dim GreenColorNumFrom As Integer = RandomDriver.Next(RampMinimumGreenColorLevelStart, RampMaximumGreenColorLevelStart)
        Dim BlueColorNumFrom As Integer = RandomDriver.Next(RampMinimumBlueColorLevelStart, RampMaximumBlueColorLevelStart)
        Dim ColorNumFrom As Integer = RandomDriver.Next(RampMinimumColorLevelStart, RampMaximumColorLevelStart)
        Dim RedColorNumTo As Integer = RandomDriver.Next(RampMinimumRedColorLevelEnd, RampMaximumRedColorLevelEnd)
        Dim GreenColorNumTo As Integer = RandomDriver.Next(RampMinimumGreenColorLevelEnd, RampMaximumGreenColorLevelEnd)
        Dim BlueColorNumTo As Integer = RandomDriver.Next(RampMinimumBlueColorLevelEnd, RampMaximumBlueColorLevelEnd)
        Dim ColorNumTo As Integer = RandomDriver.Next(RampMinimumColorLevelEnd, RampMaximumColorLevelEnd)
        Dim CurrentWindowWidth As Integer = Console.WindowWidth
        Dim CurrentWindowHeight As Integer = Console.WindowHeight
        Dim ResizeSyncing As Boolean

        'Preparations
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()

        'Screensaver logic
        Do While True
            'Console resizing can sometimes cause the cursor to remain visible. This happens on Windows 10's terminal.
            Console.CursorVisible = False
            If Ramp.CancellationPending = True Then
                HandleSaverCancel()
                Exit Do
            Else
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

                'Select a color range for the ramp
                If RampTrueColor Then
                    RedColorNumFrom = RandomDriver.Next(RampMinimumRedColorLevelStart, RampMaximumRedColorLevelStart)
                    GreenColorNumFrom = RandomDriver.Next(RampMinimumGreenColorLevelStart, RampMaximumGreenColorLevelStart)
                    BlueColorNumFrom = RandomDriver.Next(RampMinimumBlueColorLevelStart, RampMaximumBlueColorLevelStart)
                    RedColorNumTo = RandomDriver.Next(RampMinimumRedColorLevelEnd, RampMaximumRedColorLevelEnd)
                    GreenColorNumTo = RandomDriver.Next(RampMinimumGreenColorLevelEnd, RampMaximumGreenColorLevelEnd)
                    BlueColorNumTo = RandomDriver.Next(RampMinimumBlueColorLevelEnd, RampMaximumBlueColorLevelEnd)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo)
                ElseIf Ramp255Colors Then
                    ColorNumFrom = RandomDriver.Next(RampMinimumColorLevelStart, RampMaximumColorLevelStart)
                    ColorNumTo = RandomDriver.Next(RampMinimumColorLevelEnd, RampMaximumColorLevelEnd)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color from ({0}) to ({1})", ColorNumFrom, ColorNumTo)
                Else
                    ColorNumFrom = RandomDriver.Next(RampMinimumColorLevelStart, RampMaximumColorLevelStart)
                    ColorNumTo = RandomDriver.Next(RampMinimumColorLevelEnd, RampMaximumColorLevelEnd)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                End If

                'Set start and end widths for the ramp frame
                Dim RampFrameStartWidth As Integer = 4
                Dim RampFrameEndWidth As Integer = Console.WindowWidth - RampFrameStartWidth
                Dim RampFrameSpaces As Integer = RampFrameEndWidth - RampFrameStartWidth

                'Set thresholds for color ramps
                Dim RampColorRedThreshold As Integer = RedColorNumFrom - RedColorNumTo
                Dim RampColorGreenThreshold As Integer = GreenColorNumFrom - GreenColorNumTo
                Dim RampColorBlueThreshold As Integer = BlueColorNumFrom - BlueColorNumTo
                Dim RampColorThreshold As Integer = ColorNumFrom - ColorNumTo
                Dim RampColorRedSteps As Double = RampColorRedThreshold / RampFrameSpaces
                Dim RampColorGreenSteps As Double = RampColorGreenThreshold / RampFrameSpaces
                Dim RampColorBlueSteps As Double = RampColorBlueThreshold / RampFrameSpaces
                Dim RampColorSteps As Double = RampColorThreshold / RampFrameSpaces

                'Let the ramp be printed in the center
                Dim RampCenterPosition As Integer = Console.WindowHeight / 2

                'Set the current positions
                Dim RampCurrentPositionLeft As Integer = RampFrameStartWidth + 1

                'Draw the frame
                If Not ResizeSyncing Then
                    WriteWhereC(RampUpperLeftCornerChar, RampFrameStartWidth, RampCenterPosition - 2, False, If(RampUseBorderColors, New Color(RampUpperLeftCornerColor), New Color(ConsoleColors.Gray)))
                    WriteC(StrDup(RampFrameSpaces, RampUpperFrameChar), False, If(RampUseBorderColors, New Color(RampUpperFrameColor), New Color(ConsoleColors.Gray)))
                    WriteC(RampUpperRightCornerChar, False, If(RampUseBorderColors, New Color(RampUpperRightCornerColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(RampLeftFrameChar, RampFrameStartWidth, RampCenterPosition - 1, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(RampLeftFrameChar, RampFrameStartWidth, RampCenterPosition, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(RampLeftFrameChar, RampFrameStartWidth, RampCenterPosition + 1, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(RampRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition - 1, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(RampRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(RampRightFrameChar, RampFrameEndWidth + 1, RampCenterPosition + 1, False, If(RampUseBorderColors, New Color(RampLeftFrameColor), New Color(ConsoleColors.Gray)))
                    WriteWhereC(RampLowerLeftCornerChar, RampFrameStartWidth, RampCenterPosition + 2, False, If(RampUseBorderColors, New Color(RampLowerLeftCornerColor), New Color(ConsoleColors.Gray)))
                    WriteC(StrDup(RampFrameSpaces, RampLowerFrameChar), False, If(RampUseBorderColors, New Color(RampLowerFrameColor), New Color(ConsoleColors.Gray)))
                    WriteC(RampLowerRightCornerChar, False, If(RampUseBorderColors, New Color(RampLowerRightCornerColor), New Color(ConsoleColors.Gray)))
                End If

                'Draw the ramp
                If RampTrueColor Then
                    'Set the current colors
                    Dim RampCurrentColorRed As Double = RedColorNumFrom
                    Dim RampCurrentColorGreen As Double = GreenColorNumFrom
                    Dim RampCurrentColorBlue As Double = BlueColorNumFrom
                    Dim RampCurrentColorInstance As New Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}")

                    'Set the console color and fill the ramp!
                    SetConsoleColor(RampCurrentColorInstance, True)
                    Do Until Convert.ToInt32(RampCurrentColorRed) = RedColorNumTo And Convert.ToInt32(RampCurrentColorGreen) = GreenColorNumTo And Convert.ToInt32(RampCurrentColorBlue) = BlueColorNumTo
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Ramp.CancellationPending Then Exit Do
                        If ResizeSyncing Then Exit Do
                        Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition - 1)
                        Console.Write(" "c)
                        Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition)
                        Console.Write(" "c)
                        Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition + 1)
                        Console.Write(" "c)
                        RampCurrentPositionLeft = Console.CursorLeft

                        'Change the colors
                        RampCurrentColorRed -= RampColorRedSteps
                        RampCurrentColorGreen -= RampColorGreenSteps
                        RampCurrentColorBlue -= RampColorBlueSteps
                        RampCurrentColorInstance = New Color($"{Convert.ToInt32(RampCurrentColorRed)};{Convert.ToInt32(RampCurrentColorGreen)};{Convert.ToInt32(RampCurrentColorBlue)}")
                        SetConsoleColor(RampCurrentColorInstance, True)

                        'Delay writing
                        SleepNoBlock(RampDelay, Ramp)
                    Loop
                Else
                    'Set the current colors
                    Dim RampCurrentColor As Double = ColorNumFrom
                    Dim RampCurrentColorInstance As New Color(Convert.ToInt32(RampCurrentColor))

                    'Set the console color and fill the ramp!
                    SetConsoleColor(RampCurrentColorInstance, True)
                    Do Until Convert.ToInt32(RampCurrentColor) = ColorNumTo
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Ramp.CancellationPending Then Exit Do
                        If ResizeSyncing Then Exit Do
                        Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition - 1)
                        Console.Write(" "c)
                        Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition)
                        Console.Write(" "c)
                        Console.SetCursorPosition(RampCurrentPositionLeft, RampCenterPosition + 1)
                        Console.Write(" "c)
                        RampCurrentPositionLeft = Console.CursorLeft

                        'Change the colors
                        RampCurrentColor -= RampColorSteps
                        RampCurrentColorInstance = New Color(Convert.ToInt32(RampCurrentColor))
                        SetConsoleColor(RampCurrentColorInstance, True)

                        'Delay writing
                        SleepNoBlock(RampDelay, Ramp)
                    Loop
                End If
                SleepNoBlock(RampNextRampDelay, Ramp)
                Console.BackgroundColor = ConsoleColor.Black
                Console.Clear()
                ResizeSyncing = False
                CurrentWindowWidth = Console.WindowWidth
                CurrentWindowHeight = Console.WindowHeight
            End If
            SleepNoBlock(RampDelay, Ramp)
        Loop
    End Sub

    ''' <summary>
    ''' Checks for any screensaver error
    ''' </summary>
    Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles Ramp.RunWorkerCompleted
        HandleSaverError(e.Error)
    End Sub

End Module
