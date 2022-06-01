
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
    Module GlitterColorDisplay

        Public GlitterColor As New KernelThread("GlitterColor screensaver thread", True, AddressOf GlitterColor_DoWork)

        ''' <summary>
        ''' Handles the code of Glitter Colors
        ''' </summary>
        Sub GlitterColor_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                Console.BackgroundColor = ConsoleColor.Black
                Console.Clear()
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Sanity checks for color levels
                If GlitterColorTrueColor Or GlitterColor255Colors Then
                    GlitterColorMinimumRedColorLevel = If(GlitterColorMinimumRedColorLevel >= 0 And GlitterColorMinimumRedColorLevel <= 255, GlitterColorMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", GlitterColorMinimumRedColorLevel)
                    GlitterColorMinimumGreenColorLevel = If(GlitterColorMinimumGreenColorLevel >= 0 And GlitterColorMinimumGreenColorLevel <= 255, GlitterColorMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", GlitterColorMinimumGreenColorLevel)
                    GlitterColorMinimumBlueColorLevel = If(GlitterColorMinimumBlueColorLevel >= 0 And GlitterColorMinimumBlueColorLevel <= 255, GlitterColorMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", GlitterColorMinimumBlueColorLevel)
                    GlitterColorMinimumColorLevel = If(GlitterColorMinimumColorLevel >= 0 And GlitterColorMinimumColorLevel <= 255, GlitterColorMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", GlitterColorMinimumColorLevel)
                    GlitterColorMaximumRedColorLevel = If(GlitterColorMaximumRedColorLevel >= 0 And GlitterColorMaximumRedColorLevel <= 255, GlitterColorMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", GlitterColorMaximumRedColorLevel)
                    GlitterColorMaximumGreenColorLevel = If(GlitterColorMaximumGreenColorLevel >= 0 And GlitterColorMaximumGreenColorLevel <= 255, GlitterColorMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", GlitterColorMaximumGreenColorLevel)
                    GlitterColorMaximumBlueColorLevel = If(GlitterColorMaximumBlueColorLevel >= 0 And GlitterColorMaximumBlueColorLevel <= 255, GlitterColorMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", GlitterColorMaximumBlueColorLevel)
                    GlitterColorMaximumColorLevel = If(GlitterColorMaximumColorLevel >= 0 And GlitterColorMaximumColorLevel <= 255, GlitterColorMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", GlitterColorMaximumColorLevel)
                Else
                    GlitterColorMinimumColorLevel = If(GlitterColorMinimumColorLevel >= 0 And GlitterColorMinimumColorLevel <= 15, GlitterColorMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", GlitterColorMinimumColorLevel)
                    GlitterColorMaximumColorLevel = If(GlitterColorMaximumColorLevel >= 0 And GlitterColorMaximumColorLevel <= 15, GlitterColorMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", GlitterColorMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    'Select position
                    Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                    Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
                    Console.SetCursorPosition(Left, Top)

                    'Make a glitter color
                    Dim esc As Char = GetEsc()
                    If GlitterColorTrueColor Then
                        Dim RedColorNum As Integer = RandomDriver.Next(GlitterColorMinimumRedColorLevel, GlitterColorMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = RandomDriver.Next(GlitterColorMinimumGreenColorLevel, GlitterColorMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = RandomDriver.Next(GlitterColorMinimumBlueColorLevel, GlitterColorMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(ColorStorage, True)
                            Console.Write(" ")
                        End If
                    ElseIf GlitterColor255Colors Then
                        Dim ColorNum As Integer = RandomDriver.Next(GlitterColorMinimumColorLevel, GlitterColorMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(New Color(ColorNum), True)
                            Console.Write(" ")
                        End If
                    Else
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            Console.BackgroundColor = colors(RandomDriver.Next(GlitterColorMinimumColorLevel, GlitterColorMaximumColorLevel))
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                            Console.Write(" ")
                        End If
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(GlitterColorDelay, GlitterColor)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
