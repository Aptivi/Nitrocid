
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
    Module FlashColorDisplay

        Public FlashColor As New KernelThread("FlashColor screensaver thread", True, AddressOf FlashColor_DoWork)

        ''' <summary>
        ''' Handles the code of Flash Colors
        ''' </summary>
        Sub FlashColor_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                SetConsoleColor(New Color(FlashColorBackgroundColor), True)
                Console.Clear()
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Sanity checks for color levels
                If FlashColorTrueColor Or FlashColor255Colors Then
                    FlashColorMinimumRedColorLevel = If(FlashColorMinimumRedColorLevel >= 0 And FlashColorMinimumRedColorLevel <= 255, FlashColorMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", FlashColorMinimumRedColorLevel)
                    FlashColorMinimumGreenColorLevel = If(FlashColorMinimumGreenColorLevel >= 0 And FlashColorMinimumGreenColorLevel <= 255, FlashColorMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", FlashColorMinimumGreenColorLevel)
                    FlashColorMinimumBlueColorLevel = If(FlashColorMinimumBlueColorLevel >= 0 And FlashColorMinimumBlueColorLevel <= 255, FlashColorMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", FlashColorMinimumBlueColorLevel)
                    FlashColorMinimumColorLevel = If(FlashColorMinimumColorLevel >= 0 And FlashColorMinimumColorLevel <= 255, FlashColorMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", FlashColorMinimumColorLevel)
                    FlashColorMaximumRedColorLevel = If(FlashColorMaximumRedColorLevel >= 0 And FlashColorMaximumRedColorLevel <= 255, FlashColorMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", FlashColorMaximumRedColorLevel)
                    FlashColorMaximumGreenColorLevel = If(FlashColorMaximumGreenColorLevel >= 0 And FlashColorMaximumGreenColorLevel <= 255, FlashColorMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", FlashColorMaximumGreenColorLevel)
                    FlashColorMaximumBlueColorLevel = If(FlashColorMaximumBlueColorLevel >= 0 And FlashColorMaximumBlueColorLevel <= 255, FlashColorMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", FlashColorMaximumBlueColorLevel)
                    FlashColorMaximumColorLevel = If(FlashColorMaximumColorLevel >= 0 And FlashColorMaximumColorLevel <= 255, FlashColorMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", FlashColorMaximumColorLevel)
                Else
                    FlashColorMinimumColorLevel = If(FlashColorMinimumColorLevel >= 0 And FlashColorMinimumColorLevel <= 15, FlashColorMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", FlashColorMinimumColorLevel)
                    FlashColorMaximumColorLevel = If(FlashColorMaximumColorLevel >= 0 And FlashColorMaximumColorLevel <= 15, FlashColorMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", FlashColorMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    'Select position
                    Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
                    Console.SetCursorPosition(Left, Top)

                    'Make a flash color
                    Dim esc As Char = GetEsc()
                    Console.BackgroundColor = ConsoleColor.Black
                    ClearKeepPosition()
                    If FlashColorTrueColor Then
                        Dim RedColorNum As Integer = RandomDriver.Next(FlashColorMinimumRedColorLevel, FlashColorMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = RandomDriver.Next(FlashColorMinimumGreenColorLevel, FlashColorMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = RandomDriver.Next(FlashColorMinimumBlueColorLevel, FlashColorMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(ColorStorage, True)
                            Console.Write(" ")
                        End If
                    ElseIf FlashColor255Colors Then
                        Dim ColorNum As Integer = RandomDriver.Next(FlashColorMinimumColorLevel, FlashColorMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(New Color(ColorNum), True)
                            Console.Write(" ")
                        End If
                    Else
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            Console.BackgroundColor = colors(RandomDriver.Next(FlashColorMinimumColorLevel, FlashColorMaximumColorLevel))
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                            Console.Write(" ")
                        End If
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(FlashColorDelay, FlashColor)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace