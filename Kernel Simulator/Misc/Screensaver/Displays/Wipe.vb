
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

Imports System.Threading

Namespace Misc.Screensaver.Displays
    Module WipeDisplay

        Public Wipe As New KernelThread("Wipe screensaver thread", True, AddressOf Wipe_DoWork)

        Sub Wipe_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim ToDirection As WipeDirections = WipeDirections.Right
                Dim TimesWiped As Integer = 0
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                SetConsoleColor(New Color(WipeBackgroundColor), True)
                Console.ForegroundColor = ConsoleColor.White
                Console.Clear()
                Console.CursorVisible = False

                'Sanity checks for color levels
                If WipeTrueColor Or Wipe255Colors Then
                    WipeMinimumRedColorLevel = If(WipeMinimumRedColorLevel >= 0 And WipeMinimumRedColorLevel <= 255, WipeMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", WipeMinimumRedColorLevel)
                    WipeMinimumGreenColorLevel = If(WipeMinimumGreenColorLevel >= 0 And WipeMinimumGreenColorLevel <= 255, WipeMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", WipeMinimumGreenColorLevel)
                    WipeMinimumBlueColorLevel = If(WipeMinimumBlueColorLevel >= 0 And WipeMinimumBlueColorLevel <= 255, WipeMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", WipeMinimumBlueColorLevel)
                    WipeMinimumColorLevel = If(WipeMinimumColorLevel >= 0 And WipeMinimumColorLevel <= 255, WipeMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", WipeMinimumColorLevel)
                    WipeMaximumRedColorLevel = If(WipeMaximumRedColorLevel >= 0 And WipeMaximumRedColorLevel <= 255, WipeMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", WipeMaximumRedColorLevel)
                    WipeMaximumGreenColorLevel = If(WipeMaximumGreenColorLevel >= 0 And WipeMaximumGreenColorLevel <= 255, WipeMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", WipeMaximumGreenColorLevel)
                    WipeMaximumBlueColorLevel = If(WipeMaximumBlueColorLevel >= 0 And WipeMaximumBlueColorLevel <= 255, WipeMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", WipeMaximumBlueColorLevel)
                    WipeMaximumColorLevel = If(WipeMaximumColorLevel >= 0 And WipeMaximumColorLevel <= 255, WipeMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", WipeMaximumColorLevel)
                Else
                    WipeMinimumColorLevel = If(WipeMinimumColorLevel >= 0 And WipeMinimumColorLevel <= 15, WipeMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", WipeMinimumColorLevel)
                    WipeMaximumColorLevel = If(WipeMaximumColorLevel >= 0 And WipeMaximumColorLevel <= 15, WipeMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", WipeMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

                    'Select a color
                    If WipeTrueColor Then
                        Dim RedColorNum As Integer = RandomDriver.Next(WipeMinimumRedColorLevel, WipeMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = RandomDriver.Next(WipeMinimumGreenColorLevel, WipeMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = RandomDriver.Next(WipeMinimumBlueColorLevel, WipeMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        If Not ResizeSyncing Then SetConsoleColor(New Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), True)
                    ElseIf Wipe255Colors Then
                        Dim ColorNum As Integer = RandomDriver.Next(WipeMinimumColorLevel, WipeMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        If Not ResizeSyncing Then SetConsoleColor(New Color(ColorNum), True)
                    Else
                        If Not ResizeSyncing Then Console.BackgroundColor = colors(RandomDriver.Next(WipeMinimumColorLevel, WipeMaximumColorLevel))
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                    End If

                    'Set max height according to platform
                    Dim MaxWindowHeight As Integer = Console.WindowHeight
                    If IsOnUnix() Then MaxWindowHeight -= 1
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Max height {0}", MaxWindowHeight)

                    'Print a space {Column} times until the entire screen is wiped.
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Wipe direction {0}", ToDirection.ToString)
                    Select Case ToDirection
                        Case WipeDirections.Right
                            For Column As Integer = 0 To Console.WindowWidth
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If ResizeSyncing Then Exit For
                                For Row As Integer = 0 To MaxWindowHeight
                                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                    If ResizeSyncing Then Exit For

                                    'Do the actual writing
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Setting Y position to {0}", Row)
                                    Console.SetCursorPosition(0, Row)
                                    Console.Write(" ".Repeat(Column))
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Column)
                                Next
                                SleepNoBlock(WipeDelay, Wipe)
                            Next
                        Case WipeDirections.Left
                            For Column As Integer = Console.WindowWidth To 1 Step -1
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If ResizeSyncing Then Exit For
                                For Row As Integer = 0 To MaxWindowHeight
                                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                    If ResizeSyncing Then Exit For

                                    'Do the actual writing
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Setting position to {0}", Column - 1, Row)
                                    Console.SetCursorPosition(Column - 1, Row)
                                    Console.Write(" ".Repeat(Console.WindowWidth - Column + 1))
                                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Console.WindowWidth - Column + 1)
                                Next
                                SleepNoBlock(WipeDelay, Wipe)
                            Next
                        Case WipeDirections.Top
                            For Row As Integer = MaxWindowHeight To 0 Step -1
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If ResizeSyncing Then Exit For

                                'Do the actual writing
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Setting Y position to {0}", Row)
                                Console.SetCursorPosition(0, Row)
                                Console.Write(" ".Repeat(Console.WindowWidth))
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Console.WindowWidth)
                                SleepNoBlock(WipeDelay, Wipe)
                            Next
                        Case WipeDirections.Bottom
                            For Row As Integer = 0 To MaxWindowHeight
                                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                                If ResizeSyncing Then Exit For

                                'Do the actual writing
                                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", Console.WindowWidth)
                                Console.Write(" ".Repeat(Console.WindowWidth))
                                SleepNoBlock(WipeDelay, Wipe)
                            Next
                            Console.SetCursorPosition(0, 0)
                    End Select

                    If Not ResizeSyncing Then
                        TimesWiped += 1
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Wiped {0} times out of {1}", TimesWiped, WipeWipesNeededToChangeDirection)

                        'Check if the number of times wiped is equal to the number of required times to change wiping direction.
                        If TimesWiped = WipeWipesNeededToChangeDirection Then
                            TimesWiped = 0
                            ToDirection = [Enum].Parse(GetType(WipeDirections), RandomDriver.Next(0, 3))
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Changed direction to {0}", ToDirection.ToString)
                        End If
                    Else
                        WdbgConditional(ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...")
                        SetConsoleColor(New Color(WipeBackgroundColor), True)
                        Console.Clear()
                    End If

                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(WipeDelay, Wipe)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Wipe directions
        ''' </summary>
        Private Enum WipeDirections
            ''' <summary>
            ''' Wipe from right to left
            ''' </summary>
            Left
            ''' <summary>
            ''' Wipe from left to right
            ''' </summary>
            Right
            ''' <summary>
            ''' Wipe from bottom to top
            ''' </summary>
            Top
            ''' <summary>
            ''' Wipe from top to bottom
            ''' </summary>
            Bottom
        End Enum

    End Module
End Namespace
