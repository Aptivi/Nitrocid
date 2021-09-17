
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

Module FaderBackDisplay

    Public WithEvents FaderBack As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of FaderBack
    ''' </summary>
    Sub FaderBack_DoWork(sender As Object, e As DoWorkEventArgs) Handles FaderBack.DoWork
        Try
            'Variables
            Dim RandomDriver As New Random()
            Dim RedColorNum As Integer = RandomDriver.Next(255)
            Dim GreenColorNum As Integer = RandomDriver.Next(255)
            Dim BlueColorNum As Integer = RandomDriver.Next(255)

            'Preparations
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            Console.CursorVisible = False
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

            'Screensaver logic
            Do While True
                If FaderBack.CancellationPending = True Then
                    Wdbg(DebugLevel.W, "Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    SetInputColor()
                    LoadBack()
                    Console.CursorVisible = True
                    Wdbg(DebugLevel.I, "All clean. Fader Background screensaver stopped.")
                    SaverAutoReset.Set()
                    Exit Do
                Else
                    SleepNoBlock(FaderBackDelay, FaderBack)

                    'Set thresholds
                    Dim ThresholdRed As Double = RedColorNum / FaderBackMaxSteps
                    Dim ThresholdGreen As Double = GreenColorNum / FaderBackMaxSteps
                    Dim ThresholdBlue As Double = BlueColorNum / FaderBackMaxSteps
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue)

                    'Fade in
                    Dim CurrentColorRedIn As Integer = 0
                    Dim CurrentColorGreenIn As Integer = 0
                    Dim CurrentColorBlueIn As Integer = 0
                    For CurrentStep As Integer = 1 To FaderBackMaxSteps
                        If FaderBack.CancellationPending Then Exit For
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, FaderBackMaxSteps)
                        SleepNoBlock(FaderBackDelay, FaderBack)
                        CurrentColorRedIn += ThresholdRed
                        CurrentColorGreenIn += ThresholdGreen
                        CurrentColorBlueIn += ThresholdBlue
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn)
                        SetConsoleColor(New Color($"{CurrentColorRedIn};{CurrentColorGreenIn};{CurrentColorBlueIn}"), True)
                        Console.Clear()
                    Next

                    'Wait until fade out
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", FaderBackFadeOutDelay)
                    SleepNoBlock(FaderBackFadeOutDelay, FaderBack)

                    'Fade out
                    For CurrentStep As Integer = 1 To FaderBackMaxSteps
                        If FaderBack.CancellationPending Then Exit For
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, FaderBackMaxSteps)
                        SleepNoBlock(FaderBackDelay, FaderBack)
                        Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                        Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                        Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut)
                        SetConsoleColor(New Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), True)
                        Console.Clear()
                    Next

                    'Select new color
                    RedColorNum = RandomDriver.Next(255)
                    GreenColorNum = RandomDriver.Next(255)
                    BlueColorNum = RandomDriver.Next(255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                End If
            Loop
        Catch ex As Exception
            Wdbg(DebugLevel.W, "Screensaver experienced an error: {0}. Cleaning everything up...", ex.Message)
            WStkTrc(ex)
            e.Cancel = True
            SetInputColor()
            LoadBack()
            Console.CursorVisible = True
            Wdbg(DebugLevel.I, "All clean. Fader Background screensaver stopped.")
            W(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, ex.Message)
            SaverAutoReset.Set()
        End Try
    End Sub

End Module
