
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
Imports KS.Misc.Games

Namespace Misc.Screensaver.Displays
    Module SnakerDisplay

        Public Snaker As New KernelThread("Snaker screensaver thread", True, AddressOf Snaker_DoWork)

        ''' <summary>
        ''' Handles the code of Snaker
        ''' </summary>
        Sub Snaker_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean

                'Preparations
                Console.BackgroundColor = ConsoleColor.Black
                Console.ForegroundColor = ConsoleColor.White
                Console.Clear()
                Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

                'Sanity checks for color levels
                If SnakerTrueColor Or Snaker255Colors Then
                    SnakerMinimumRedColorLevel = If(SnakerMinimumRedColorLevel >= 0 And SnakerMinimumRedColorLevel <= 255, SnakerMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", SnakerMinimumRedColorLevel)
                    SnakerMinimumGreenColorLevel = If(SnakerMinimumGreenColorLevel >= 0 And SnakerMinimumGreenColorLevel <= 255, SnakerMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", SnakerMinimumGreenColorLevel)
                    SnakerMinimumBlueColorLevel = If(SnakerMinimumBlueColorLevel >= 0 And SnakerMinimumBlueColorLevel <= 255, SnakerMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", SnakerMinimumBlueColorLevel)
                    SnakerMinimumColorLevel = If(SnakerMinimumColorLevel >= 0 And SnakerMinimumColorLevel <= 255, SnakerMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", SnakerMinimumColorLevel)
                    SnakerMaximumRedColorLevel = If(SnakerMaximumRedColorLevel >= 0 And SnakerMaximumRedColorLevel <= 255, SnakerMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", SnakerMaximumRedColorLevel)
                    SnakerMaximumGreenColorLevel = If(SnakerMaximumGreenColorLevel >= 0 And SnakerMaximumGreenColorLevel <= 255, SnakerMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", SnakerMaximumGreenColorLevel)
                    SnakerMaximumBlueColorLevel = If(SnakerMaximumBlueColorLevel >= 0 And SnakerMaximumBlueColorLevel <= 255, SnakerMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", SnakerMaximumBlueColorLevel)
                    SnakerMaximumColorLevel = If(SnakerMaximumColorLevel >= 0 And SnakerMaximumColorLevel <= 255, SnakerMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", SnakerMaximumColorLevel)
                Else
                    SnakerMinimumColorLevel = If(SnakerMinimumColorLevel >= 0 And SnakerMinimumColorLevel <= 15, SnakerMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", SnakerMinimumColorLevel)
                    SnakerMaximumColorLevel = If(SnakerMaximumColorLevel >= 0 And SnakerMaximumColorLevel <= 15, SnakerMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", SnakerMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    InitializeSnaker(True)

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(SnakerDelay, Snaker)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Changes the snake color
        ''' </summary>
        Function ChangeSnakeColor() As Color
            Dim RandomDriver As New Random()
            Dim esc As Char = GetEsc()
            If SnakerTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(SnakerMinimumRedColorLevel, SnakerMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(SnakerMinimumGreenColorLevel, SnakerMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(SnakerMinimumBlueColorLevel, SnakerMaximumBlueColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                Return New Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}")
            ElseIf Snaker255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(SnakerMinimumColorLevel, SnakerMaximumColorLevel)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                Return New Color(ColorNum)
            Else
                Console.BackgroundColor = colors(RandomDriver.Next(SnakerMinimumColorLevel, SnakerMaximumColorLevel))
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
            End If
        End Function

        ''' <summary>
        ''' Where would the snake go?
        ''' </summary>
        Enum SnakeDirection
            Top
            Bottom
            Left
            Right
        End Enum

    End Module
End Namespace
