
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
    Module ColorMixDisplay

        Public ColorMix As New KernelThread("ColorMix screensaver thread", True, AddressOf ColorMix_DoWork)

        ''' <summary>
        ''' Handles the code of ColorMix
        ''' </summary>
        Sub ColorMix_DoWork()
            Try
                'Variables
                Dim colorrand As New Random()
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                SetConsoleColor(New Color(ColorMixBackgroundColor), True)
                Console.ForegroundColor = ConsoleColor.White
                Console.Clear()

                'Sanity checks for color levels
                If ColorMixTrueColor Or ColorMix255Colors Then
                    ColorMixMinimumRedColorLevel = If(ColorMixMinimumRedColorLevel >= 0 And ColorMixMinimumRedColorLevel <= 255, ColorMixMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", ColorMixMinimumRedColorLevel)
                    ColorMixMinimumGreenColorLevel = If(ColorMixMinimumGreenColorLevel >= 0 And ColorMixMinimumGreenColorLevel <= 255, ColorMixMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", ColorMixMinimumGreenColorLevel)
                    ColorMixMinimumBlueColorLevel = If(ColorMixMinimumBlueColorLevel >= 0 And ColorMixMinimumBlueColorLevel <= 255, ColorMixMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", ColorMixMinimumBlueColorLevel)
                    ColorMixMinimumColorLevel = If(ColorMixMinimumColorLevel >= 0 And ColorMixMinimumColorLevel <= 255, ColorMixMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", ColorMixMinimumColorLevel)
                    ColorMixMaximumRedColorLevel = If(ColorMixMaximumRedColorLevel >= 0 And ColorMixMaximumRedColorLevel <= 255, ColorMixMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", ColorMixMaximumRedColorLevel)
                    ColorMixMaximumGreenColorLevel = If(ColorMixMaximumGreenColorLevel >= 0 And ColorMixMaximumGreenColorLevel <= 255, ColorMixMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", ColorMixMaximumGreenColorLevel)
                    ColorMixMaximumBlueColorLevel = If(ColorMixMaximumBlueColorLevel >= 0 And ColorMixMaximumBlueColorLevel <= 255, ColorMixMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", ColorMixMaximumBlueColorLevel)
                    ColorMixMaximumColorLevel = If(ColorMixMaximumColorLevel >= 0 And ColorMixMaximumColorLevel <= 255, ColorMixMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", ColorMixMaximumColorLevel)
                Else
                    ColorMixMinimumColorLevel = If(ColorMixMinimumColorLevel >= 0 And ColorMixMinimumColorLevel <= 16, ColorMixMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", ColorMixMinimumColorLevel)
                    ColorMixMaximumColorLevel = If(ColorMixMaximumColorLevel >= 0 And ColorMixMaximumColorLevel <= 16, ColorMixMaximumColorLevel, 16)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", ColorMixMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    'Set colors
                    If ColorMixTrueColor Then
                        Dim RedColorNum As Integer = colorrand.Next(ColorMixMinimumRedColorLevel, ColorMixMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = colorrand.Next(ColorMixMinimumGreenColorLevel, ColorMixMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = colorrand.Next(ColorMixMinimumBlueColorLevel, ColorMixMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(ColorStorage, True)
                            Console.Write(" ")
                        End If
                    ElseIf ColorMix255Colors Then
                        Dim ColorNum As Integer = colorrand.Next(ColorMixMinimumColorLevel, ColorMixMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            SetConsoleColor(New Color(ColorNum), True)
                            Console.Write(" ")
                        End If
                    Else
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            Console.BackgroundColor = CType(colorrand.Next(ColorMixMinimumColorLevel, ColorMixMaximumColorLevel), ConsoleColor)
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                            Console.Write(" ")
                        End If
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(ColorMixDelay, ColorMix)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
