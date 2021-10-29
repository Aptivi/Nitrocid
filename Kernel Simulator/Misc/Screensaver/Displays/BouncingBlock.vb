
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

Module BouncingBlockDisplay

    Public WithEvents BouncingBlock As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Bouncing Block
    ''' </summary>
    Sub BouncingBlock_DoWork(sender As Object, e As DoWorkEventArgs) Handles BouncingBlock.DoWork
        'Variables
        Dim RandomDriver As New Random()
        Dim Direction As String = "BottomRight"
        Dim RowBlock, ColumnBlock As Integer
        Dim CurrentWindowWidth As Integer = Console.WindowWidth
        Dim CurrentWindowHeight As Integer = Console.WindowHeight
        Dim ResizeSyncing As Boolean

        'Preparations
        SetConsoleColor(New Color(BouncingBlockBackgroundColor), True)
        SetConsoleColor(New Color(BouncingBlockForegroundColor))
        Console.Clear()
        RowBlock = Console.WindowHeight / 2
        ColumnBlock = Console.WindowWidth / 2

        'Screensaver logic
        Do While True
            Console.CursorVisible = False
            SetConsoleColor(New Color(BouncingBlockBackgroundColor), True)
            SetConsoleColor(New Color(BouncingBlockForegroundColor))
            Console.Clear()
            If BouncingBlock.CancellationPending = True Then
                HandleSaverCancel()
                Exit Do
            Else
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Row block: {0} | Column block: {1}", RowBlock, ColumnBlock)

                'Sanity checks for color levels
                If BouncingBlockTrueColor Or BouncingBlock255Colors Then
                    BouncingBlockMinimumRedColorLevel = If(BouncingBlockMinimumRedColorLevel >= 0 And BouncingBlockMinimumRedColorLevel <= 255, BouncingBlockMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", BouncingBlockMinimumRedColorLevel)
                    BouncingBlockMinimumGreenColorLevel = If(BouncingBlockMinimumGreenColorLevel >= 0 And BouncingBlockMinimumGreenColorLevel <= 255, BouncingBlockMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", BouncingBlockMinimumGreenColorLevel)
                    BouncingBlockMinimumBlueColorLevel = If(BouncingBlockMinimumBlueColorLevel >= 0 And BouncingBlockMinimumBlueColorLevel <= 255, BouncingBlockMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", BouncingBlockMinimumBlueColorLevel)
                    BouncingBlockMinimumColorLevel = If(BouncingBlockMinimumColorLevel >= 0 And BouncingBlockMinimumColorLevel <= 255, BouncingBlockMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", BouncingBlockMinimumColorLevel)
                    BouncingBlockMaximumRedColorLevel = If(BouncingBlockMaximumRedColorLevel >= 0 And BouncingBlockMaximumRedColorLevel <= 255, BouncingBlockMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", BouncingBlockMaximumRedColorLevel)
                    BouncingBlockMaximumGreenColorLevel = If(BouncingBlockMaximumGreenColorLevel >= 0 And BouncingBlockMaximumGreenColorLevel <= 255, BouncingBlockMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", BouncingBlockMaximumGreenColorLevel)
                    BouncingBlockMaximumBlueColorLevel = If(BouncingBlockMaximumBlueColorLevel >= 0 And BouncingBlockMaximumBlueColorLevel <= 255, BouncingBlockMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", BouncingBlockMaximumBlueColorLevel)
                    BouncingBlockMaximumColorLevel = If(BouncingBlockMaximumColorLevel >= 0 And BouncingBlockMaximumColorLevel <= 255, BouncingBlockMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", BouncingBlockMaximumColorLevel)
                Else
                    BouncingBlockMinimumColorLevel = If(BouncingBlockMinimumColorLevel >= 0 And BouncingBlockMinimumColorLevel <= 15, BouncingBlockMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", BouncingBlockMinimumColorLevel)
                    BouncingBlockMaximumColorLevel = If(BouncingBlockMaximumColorLevel >= 0 And BouncingBlockMaximumColorLevel <= 15, BouncingBlockMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", BouncingBlockMaximumColorLevel)
                End If

                'Change the color
                If BouncingBlockTrueColor Then
                    Dim RedColorNum As Integer = RandomDriver.Next(BouncingBlockMinimumRedColorLevel, BouncingBlockMaximumRedColorLevel)
                    Dim GreenColorNum As Integer = RandomDriver.Next(BouncingBlockMinimumGreenColorLevel, BouncingBlockMaximumGreenColorLevel)
                    Dim BlueColorNum As Integer = RandomDriver.Next(BouncingBlockMinimumBlueColorLevel, BouncingBlockMaximumBlueColorLevel)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If Not ResizeSyncing Then
                        WriteWhereC(" ", ColumnBlock, RowBlock, True, New Color(255, 255, 255), New Color(RedColorNum, GreenColorNum, BlueColorNum))
                    Else
                        RowBlock = Console.WindowHeight / 2
                        ColumnBlock = Console.WindowWidth / 2
                    End If
                ElseIf BouncingBlock255Colors Then
                    Dim ColorNum As Integer = RandomDriver.Next(BouncingBlockMinimumColorLevel, BouncingBlockMaximumColorLevel)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If Not ResizeSyncing Then
                        WriteWhereC(" ", ColumnBlock, RowBlock, True, New Color(ConsoleColors.White), New Color(ColorNum))
                    Else
                        RowBlock = Console.WindowHeight / 2
                        ColumnBlock = Console.WindowWidth / 2
                    End If
                Else
                    Dim OldColumn As Integer = Console.CursorLeft
                    Dim OldRow As Integer = Console.CursorTop
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If Not ResizeSyncing Then
                        Console.BackgroundColor = colors(RandomDriver.Next(BouncingBlockMinimumColorLevel, BouncingBlockMaximumColorLevel))
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                        Console.SetCursorPosition(ColumnBlock, RowBlock)
                        Console.Write(" ")
                        Console.SetCursorPosition(OldColumn, OldRow)
                        Console.BackgroundColor = ConsoleColor.Black
                        Console.Write(" ")
                    Else
                        RowBlock = Console.WindowHeight / 2
                        ColumnBlock = Console.WindowWidth / 2
                    End If
                End If

                If RowBlock = Console.WindowHeight - 2 Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the bottom.")
                    Direction = Direction.Replace("Bottom", "Top")
                ElseIf RowBlock = 1 Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the top.")
                    Direction = Direction.Replace("Top", "Bottom")
                End If

                If ColumnBlock = Console.WindowWidth - 1 Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the right.")
                    Direction = Direction.Replace("Right", "Left")
                ElseIf ColumnBlock = 1 Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the left.")
                    Direction = Direction.Replace("Left", "Right")
                End If

                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Block is facing {0}.", Direction)
                If Direction = "BottomRight" Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Increasing row and column block position")
                    RowBlock += 1
                    ColumnBlock += 1
                ElseIf Direction = "BottomLeft" Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Increasing row and decreasing column block position")
                    RowBlock += 1
                    ColumnBlock -= 1
                ElseIf Direction = "TopRight" Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Decreasing row and increasing column block position")
                    RowBlock -= 1
                    ColumnBlock += 1
                ElseIf Direction = "TopLeft" Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Decreasing row and column block position")
                    RowBlock -= 1
                    ColumnBlock -= 1
                End If

                'Reset resize sync
                ResizeSyncing = False
                CurrentWindowWidth = Console.WindowWidth
                CurrentWindowHeight = Console.WindowHeight
            End If
            SleepNoBlock(BouncingBlockDelay, BouncingBlock)
        Loop
    End Sub

    ''' <summary>
    ''' Checks for any screensaver error
    ''' </summary>
    Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles BouncingBlock.RunWorkerCompleted
        HandleSaverError(e.Error)
    End Sub

End Module
