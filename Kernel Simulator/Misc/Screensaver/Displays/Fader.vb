
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

Module FaderDisplay

    Public WithEvents Fader As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Fader
    ''' </summary>
    Sub Fader_DoWork(sender As Object, e As DoWorkEventArgs) Handles Fader.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Console.CursorVisible = False
        Dim RandomDriver As New Random()
        Dim RedColorNum As Integer = RandomDriver.Next(255)
        Dim GreenColorNum As Integer = RandomDriver.Next(255)
        Dim BlueColorNum As Integer = RandomDriver.Next(255)
        Wdbg("I", "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        Try
            Do While True
                If Fader.CancellationPending = True Then
                    Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    SetInputColor()
                    LoadBack()
                    Console.CursorVisible = True
                    Wdbg("I", "All clean. Fader screensaver stopped.")
                    SaverAutoReset.Set()
                    Exit Do
                Else
                    SleepNoBlock(FaderDelay, Fader)
                    Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                    WdbgConditional(ScreensaverDebug, "I", "Selected left and top: {0}, {1}", Left, Top)
                    If FaderWrite.Length + Left >= Console.WindowWidth Then
                        WdbgConditional(ScreensaverDebug, "I", "Text length of {0} exceeded window width of {1}.", FaderWrite.Length + Left, Console.WindowWidth)
                        Left -= FaderWrite.Length + 1
                    End If
                    Console.SetCursorPosition(Left, Top)
                    Console.BackgroundColor = ConsoleColor.Black
                    ClearKeepPosition()

                    'Set thresholds
                    Dim ThresholdRed As Double = RedColorNum / FaderMaxSteps
                    Dim ThresholdGreen As Double = GreenColorNum / FaderMaxSteps
                    Dim ThresholdBlue As Double = BlueColorNum / FaderMaxSteps
                    WdbgConditional(ScreensaverDebug, "I", "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue)

                    'Fade in
                    Dim CurrentColorRedIn As Integer = 0
                    Dim CurrentColorGreenIn As Integer = 0
                    Dim CurrentColorBlueIn As Integer = 0
                    For CurrentStep As Integer = FaderMaxSteps To 1 Step -1
                        If Fader.CancellationPending Then Exit For
                        WdbgConditional(ScreensaverDebug, "I", "Step {0}/{1}", CurrentStep, FaderMaxSteps)
                        SleepNoBlock(FaderDelay, Fader)
                        CurrentColorRedIn += ThresholdRed
                        CurrentColorGreenIn += ThresholdGreen
                        CurrentColorBlueIn += ThresholdBlue
                        WdbgConditional(ScreensaverDebug, "I", "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn)
                        WriteWhereC(FaderWrite, Left, Top, True, New Color(CurrentColorRedIn & ";" & CurrentColorGreenIn & ";" & CurrentColorBlueIn), New Color(ConsoleColors.Black))
                    Next

                    'Wait until fade out
                    WdbgConditional(ScreensaverDebug, "I", "Waiting {0} ms...", FaderFadeOutDelay)
                    SleepNoBlock(FaderFadeOutDelay, Fader)

                    'Fade out
                    For CurrentStep As Integer = 1 To FaderMaxSteps
                        If Fader.CancellationPending Then Exit For
                        WdbgConditional(ScreensaverDebug, "I", "Step {0}/{1}", CurrentStep, FaderMaxSteps)
                        SleepNoBlock(FaderDelay, Fader)
                        Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                        Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                        Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                        WdbgConditional(ScreensaverDebug, "I", "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut)
                        WriteWhereC(FaderWrite, Left, Top, True, New Color(CurrentColorRedOut & ";" & CurrentColorGreenOut & ";" & CurrentColorBlueOut), New Color(ConsoleColors.Black))
                    Next

                    'Select new color
                    RedColorNum = RandomDriver.Next(255)
                    GreenColorNum = RandomDriver.Next(255)
                    BlueColorNum = RandomDriver.Next(255)
                    WdbgConditional(ScreensaverDebug, "I", "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                End If
            Loop
        Catch ex As Exception
            Wdbg("W", "Screensaver experienced an error: {0}. Cleaning everything up...", ex.Message)
            WStkTrc(ex)
            e.Cancel = True
            SetInputColor()
            LoadBack()
            Console.CursorVisible = True
            Wdbg("I", "All clean. Fader screensaver stopped.")
            W(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, ex.Message)
            SaverAutoReset.Set()
        End Try
    End Sub

End Module
