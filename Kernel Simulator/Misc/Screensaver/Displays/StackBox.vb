
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
Imports Extensification.IntegerExts

Module StackBoxDisplay

    Public WithEvents StackBox As New NamedBackgroundWorker("StackBox screensaver thread") With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of StackBox
    ''' </summary>
    Sub StackBox_DoWork(sender As Object, e As DoWorkEventArgs) Handles StackBox.DoWork
        'Variables
        Dim RandomDriver As New Random()
        Dim CurrentWindowWidth As Integer = Console.WindowWidth
        Dim CurrentWindowHeight As Integer = Console.WindowHeight
        Dim ResizeSyncing As Boolean

        'Preparations
        Console.BackgroundColor = ConsoleColor.Black
        Console.Clear()
        Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

        'Screensaver logic
        Do While True
            Console.CursorVisible = False
            If StackBox.CancellationPending = True Then
                HandleSaverCancel()
                Exit Do
            Else
                'Sanity checks for color levels
                If StackBoxTrueColor Or StackBox255Colors Then
                    StackBoxMinimumRedColorLevel = If(StackBoxMinimumRedColorLevel >= 0 And StackBoxMinimumRedColorLevel <= 255, StackBoxMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", StackBoxMinimumRedColorLevel)
                    StackBoxMinimumGreenColorLevel = If(StackBoxMinimumGreenColorLevel >= 0 And StackBoxMinimumGreenColorLevel <= 255, StackBoxMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", StackBoxMinimumGreenColorLevel)
                    StackBoxMinimumBlueColorLevel = If(StackBoxMinimumBlueColorLevel >= 0 And StackBoxMinimumBlueColorLevel <= 255, StackBoxMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", StackBoxMinimumBlueColorLevel)
                    StackBoxMinimumColorLevel = If(StackBoxMinimumColorLevel >= 0 And StackBoxMinimumColorLevel <= 255, StackBoxMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", StackBoxMinimumColorLevel)
                    StackBoxMaximumRedColorLevel = If(StackBoxMaximumRedColorLevel >= 0 And StackBoxMaximumRedColorLevel <= 255, StackBoxMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", StackBoxMaximumRedColorLevel)
                    StackBoxMaximumGreenColorLevel = If(StackBoxMaximumGreenColorLevel >= 0 And StackBoxMaximumGreenColorLevel <= 255, StackBoxMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", StackBoxMaximumGreenColorLevel)
                    StackBoxMaximumBlueColorLevel = If(StackBoxMaximumBlueColorLevel >= 0 And StackBoxMaximumBlueColorLevel <= 255, StackBoxMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", StackBoxMaximumBlueColorLevel)
                    StackBoxMaximumColorLevel = If(StackBoxMaximumColorLevel >= 0 And StackBoxMaximumColorLevel <= 255, StackBoxMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", StackBoxMaximumColorLevel)
                Else
                    StackBoxMinimumColorLevel = If(StackBoxMinimumColorLevel >= 0 And StackBoxMinimumColorLevel <= 15, StackBoxMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", StackBoxMinimumColorLevel)
                    StackBoxMaximumColorLevel = If(StackBoxMaximumColorLevel >= 0 And StackBoxMaximumColorLevel <= 15, StackBoxMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", StackBoxMaximumColorLevel)
                End If

                If ResizeSyncing Then
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Clear()

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                Else
                    Dim Drawable As Boolean = True
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

                    'Get the required positions for the box
                    Dim BoxStartX As Integer = RandomDriver.Next(Console.WindowWidth - IsOnUnix())
                    Dim BoxEndX As Integer = RandomDriver.Next(Console.WindowWidth - IsOnUnix())
                    Dim BoxStartY As Integer = RandomDriver.Next(Console.WindowHeight)
                    Dim BoxEndY As Integer = RandomDriver.Next(Console.WindowHeight)

                    'Check to see if start is less than or equal to end
                    BoxStartX.SwapIfSourceLarger(BoxEndX)
                    BoxStartY.SwapIfSourceLarger(BoxEndY)
                    If BoxStartX = BoxEndX Or BoxStartY = BoxEndY Then
                        'Don't draw; it won't be shown anyways
                        Drawable = False
                    End If

                    If Drawable Then
                        'Select color
                        If StackBoxTrueColor Then
                            Dim RedColorNum As Integer = RandomDriver.Next(StackBoxMinimumRedColorLevel, StackBoxMaximumRedColorLevel)
                            Dim GreenColorNum As Integer = RandomDriver.Next(StackBoxMinimumGreenColorLevel, StackBoxMaximumGreenColorLevel)
                            Dim BlueColorNum As Integer = RandomDriver.Next(StackBoxMinimumBlueColorLevel, StackBoxMaximumBlueColorLevel)
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                            SetConsoleColor(New Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), True)
                        ElseIf StackBox255Colors Then
                            Dim ColorNum As Integer = RandomDriver.Next(StackBoxMinimumColorLevel, StackBoxMaximumColorLevel)
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                            SetConsoleColor(New Color(ColorNum), True)
                        Else
                            Console.BackgroundColor = colors(RandomDriver.Next(StackBoxMinimumColorLevel, StackBoxMaximumColorLevel))
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                        End If

                        'Draw the box
                        For X As Integer = BoxStartX To BoxEndX
                            For Y As Integer = BoxStartY To BoxEndY
                                Console.SetCursorPosition(X, Y)
                                Console.Write(" ")
                            Next
                        Next
                    End If
                End If
            End If
            SleepNoBlock(StackBoxDelay, StackBox)
        Loop
    End Sub

    ''' <summary>
    ''' Checks for any screensaver error
    ''' </summary>
    Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles StackBox.RunWorkerCompleted
        HandleSaverError(e.Error)
    End Sub

End Module
