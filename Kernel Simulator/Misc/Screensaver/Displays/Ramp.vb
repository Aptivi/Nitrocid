
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
        Try
            'Variables
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()

            'Preparations
            Dim RandomDriver As New Random()
            Dim RedColorNumFrom As Integer = RandomDriver.Next(255)
            Dim GreenColorNumFrom As Integer = RandomDriver.Next(255)
            Dim BlueColorNumFrom As Integer = RandomDriver.Next(255)
            Dim ColorNumFrom As Integer = RandomDriver.Next(255)
            Dim RedColorNumTo As Integer = RandomDriver.Next(255)
            Dim GreenColorNumTo As Integer = RandomDriver.Next(255)
            Dim BlueColorNumTo As Integer = RandomDriver.Next(255)
            Dim ColorNumTo As Integer = RandomDriver.Next(255)
            Dim CurrentWindowWidth As Integer = Console.WindowWidth
            Dim CurrentWindowHeight As Integer = Console.WindowHeight
            Dim ResizeSyncing As Boolean

            'Screensaver logic
            Do While True
                'Console resizing can sometimes cause the cursor to remain visible. This happens on Windows 10's terminal.
                Console.CursorVisible = False
                If Ramp.CancellationPending = True Then
                    Wdbg(DebugLevel.W, "Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    SetInputColor()
                    LoadBack()
                    Console.CursorVisible = True
                    Wdbg(DebugLevel.I, "All clean. Ramp screensaver stopped.")
                    SaverAutoReset.Set()
                    Exit Do
                Else
                    SleepNoBlock(RampDelay, Ramp)
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

                    'Select a color range for the ramp
                    If RampTrueColor Then
                        RedColorNumFrom = RandomDriver.Next(255)
                        GreenColorNumFrom = RandomDriver.Next(255)
                        BlueColorNumFrom = RandomDriver.Next(255)
                        RedColorNumTo = RandomDriver.Next(255)
                        GreenColorNumTo = RandomDriver.Next(255)
                        BlueColorNumTo = RandomDriver.Next(255)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color from (R;G;B: {0};{1};{2}) to (R;G;B: {3};{4};{5})", RedColorNumFrom, GreenColorNumFrom, BlueColorNumFrom, RedColorNumTo, GreenColorNumTo, BlueColorNumTo)
                    ElseIf Ramp255Colors Then
                        ColorNumFrom = RandomDriver.Next(255)
                        ColorNumTo = RandomDriver.Next(255)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color from ({0}) to ({1})", ColorNumFrom, ColorNumTo)
                    Else
                        ColorNumFrom = RandomDriver.Next(15)
                        ColorNumTo = RandomDriver.Next(15)
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
                        Console.ForegroundColor = ConsoleColor.Gray
                        Console.SetCursorPosition(RampFrameStartWidth, RampCenterPosition - 2)
                        Console.Write("╔"c)
                        Console.Write(StrDup(RampFrameSpaces, "═"c))
                        Console.Write("╗"c)
                        Console.SetCursorPosition(RampFrameStartWidth, RampCenterPosition - 1)
                        Console.Write("║"c)
                        Console.SetCursorPosition(RampFrameStartWidth, RampCenterPosition)
                        Console.Write("║"c)
                        Console.SetCursorPosition(RampFrameStartWidth, RampCenterPosition + 1)
                        Console.Write("║"c)
                        Console.SetCursorPosition(RampFrameEndWidth + 1, RampCenterPosition - 1)
                        Console.Write("║"c)
                        Console.SetCursorPosition(RampFrameEndWidth + 1, RampCenterPosition)
                        Console.Write("║"c)
                        Console.SetCursorPosition(RampFrameEndWidth + 1, RampCenterPosition + 1)
                        Console.Write("║"c)
                        Console.SetCursorPosition(RampFrameStartWidth, RampCenterPosition + 2)
                        Console.Write("╚"c)
                        Console.Write(StrDup(RampFrameSpaces, "═"c))
                        Console.Write("╝"c)
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
            Loop
        Catch ex As Exception
            Wdbg(DebugLevel.W, "Screensaver experienced an error: {0}. Cleaning everything up...", ex.Message)
            WStkTrc(ex)
            e.Cancel = True
            SetInputColor()
            LoadBack()
            Console.CursorVisible = True
            Wdbg(DebugLevel.I, "All clean. Ramp screensaver stopped.")
            W(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, ex.Message)
            SaverAutoReset.Set()
        End Try
    End Sub

End Module
