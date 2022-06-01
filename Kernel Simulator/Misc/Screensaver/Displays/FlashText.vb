
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
    Module FlashTextDisplay

        Public FlashText As New KernelThread("FlashText screensaver thread", True, AddressOf FlashText_DoWork)

        ''' <summary>
        ''' Handles the code of Flash Text
        ''' </summary>
        Sub FlashText_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                SetConsoleColor(New Color(FlashTextBackgroundColor), True)
                Console.Clear()
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Sanity checks for color levels
                If FlashTextTrueColor Or FlashText255Colors Then
                    FlashTextMinimumRedColorLevel = If(FlashTextMinimumRedColorLevel >= 0 And FlashTextMinimumRedColorLevel <= 255, FlashTextMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", FlashTextMinimumRedColorLevel)
                    FlashTextMinimumGreenColorLevel = If(FlashTextMinimumGreenColorLevel >= 0 And FlashTextMinimumGreenColorLevel <= 255, FlashTextMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", FlashTextMinimumGreenColorLevel)
                    FlashTextMinimumBlueColorLevel = If(FlashTextMinimumBlueColorLevel >= 0 And FlashTextMinimumBlueColorLevel <= 255, FlashTextMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", FlashTextMinimumBlueColorLevel)
                    FlashTextMinimumColorLevel = If(FlashTextMinimumColorLevel >= 0 And FlashTextMinimumColorLevel <= 255, FlashTextMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", FlashTextMinimumColorLevel)
                    FlashTextMaximumRedColorLevel = If(FlashTextMaximumRedColorLevel >= 0 And FlashTextMaximumRedColorLevel <= 255, FlashTextMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", FlashTextMaximumRedColorLevel)
                    FlashTextMaximumGreenColorLevel = If(FlashTextMaximumGreenColorLevel >= 0 And FlashTextMaximumGreenColorLevel <= 255, FlashTextMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", FlashTextMaximumGreenColorLevel)
                    FlashTextMaximumBlueColorLevel = If(FlashTextMaximumBlueColorLevel >= 0 And FlashTextMaximumBlueColorLevel <= 255, FlashTextMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", FlashTextMaximumBlueColorLevel)
                    FlashTextMaximumColorLevel = If(FlashTextMaximumColorLevel >= 0 And FlashTextMaximumColorLevel <= 255, FlashTextMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", FlashTextMaximumColorLevel)
                Else
                    FlashTextMinimumColorLevel = If(FlashTextMinimumColorLevel >= 0 And FlashTextMinimumColorLevel <= 15, FlashTextMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", FlashTextMinimumColorLevel)
                    FlashTextMaximumColorLevel = If(FlashTextMaximumColorLevel >= 0 And FlashTextMaximumColorLevel <= 15, FlashTextMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", FlashTextMaximumColorLevel)
                End If

                'Select position
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
                Console.SetCursorPosition(Left, Top)

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False

                    'Make two delay halves to make up one half for screen with text and one half for screen with no text to make a flashing effect
                    Dim HalfDelay As Integer = FlashTextDelay / 2

                    'Make a flashing text
                    Dim esc As Char = GetEsc()
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Clear()
                    If FlashTextTrueColor Then
                        Dim RedColorNum As Integer = RandomDriver.Next(FlashTextMinimumRedColorLevel, FlashTextMaximumRedColorLevel)
                        Dim GreenColorNum As Integer = RandomDriver.Next(FlashTextMinimumGreenColorLevel, FlashTextMaximumGreenColorLevel)
                        Dim BlueColorNum As Integer = RandomDriver.Next(FlashTextMinimumBlueColorLevel, FlashTextMaximumBlueColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            WriteWhere(FlashTextWrite, Left, Top, True, ColorStorage)
                        End If
                    ElseIf FlashText255Colors Then
                        Dim ColorNum As Integer = RandomDriver.Next(FlashTextMinimumColorLevel, FlashTextMaximumColorLevel)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            WriteWhere(FlashTextWrite, Left, Top, True, New Color(ColorNum))
                        End If
                    Else
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If Not ResizeSyncing Then
                            WriteWhere(FlashTextWrite, Left, Top, True, colors(RandomDriver.Next(FlashTextMinimumColorLevel, FlashTextMaximumColorLevel)))
                        End If
                    End If
                    SleepNoBlock(HalfDelay, FlashText)
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Clear()
                    SleepNoBlock(HalfDelay, FlashText)

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace