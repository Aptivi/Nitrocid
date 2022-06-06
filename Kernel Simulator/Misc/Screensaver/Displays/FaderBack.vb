
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
    Public Module FaderBackDisplay

        Friend FaderBack As New KernelThread("FaderBack screensaver thread", True, AddressOf FaderBack_DoWork)
        Private _faderBackDelay As Integer = 10
        Private _faderBackFadeOutDelay As Integer = 3000
        Private _faderBackMaxSteps As Integer = 25
        Private _faderBackMinimumRedColorLevel As Integer = 0
        Private _faderBackMinimumGreenColorLevel As Integer = 0
        Private _faderBackMinimumBlueColorLevel As Integer = 0
        Private _faderBackMaximumRedColorLevel As Integer = 255
        Private _faderBackMaximumGreenColorLevel As Integer = 255
        Private _faderBackMaximumBlueColorLevel As Integer = 255

        ''' <summary>
        ''' [FaderBack] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FaderBackDelay As Integer
            Get
                Return _faderBackDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _faderBackDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] How many milliseconds to wait before fading the text out?
        ''' </summary>
        Public Property FaderBackFadeOutDelay As Integer
            Get
                Return _faderBackFadeOutDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3000
                _faderBackFadeOutDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] How many fade steps to do?
        ''' </summary>
        Public Property FaderBackMaxSteps As Integer
            Get
                Return _faderBackMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _faderBackMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The minimum red color level (true color)
        ''' </summary>
        Public Property FaderBackMinimumRedColorLevel As Integer
            Get
                Return _faderBackMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderBackMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The minimum green color level (true color)
        ''' </summary>
        Public Property FaderBackMinimumGreenColorLevel As Integer
            Get
                Return _faderBackMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderBackMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The minimum blue color level (true color)
        ''' </summary>
        Public Property FaderBackMinimumBlueColorLevel As Integer
            Get
                Return _faderBackMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderBackMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The maximum red color level (true color)
        ''' </summary>
        Public Property FaderBackMaximumRedColorLevel As Integer
            Get
                Return _faderBackMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderBackMinimumRedColorLevel Then value = _faderBackMinimumRedColorLevel
                If value > 255 Then value = 255
                _faderBackMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The maximum green color level (true color)
        ''' </summary>
        Public Property FaderBackMaximumGreenColorLevel As Integer
            Get
                Return _faderBackMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderBackMinimumGreenColorLevel Then value = _faderBackMinimumGreenColorLevel
                If value > 255 Then value = 255
                _faderBackMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [FaderBack] The maximum blue color level (true color)
        ''' </summary>
        Public Property FaderBackMaximumBlueColorLevel As Integer
            Get
                Return _faderBackMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderBackMinimumBlueColorLevel Then value = _faderBackMinimumBlueColorLevel
                If value > 255 Then value = 255
                _faderBackMaximumBlueColorLevel = value
            End Set
        End Property

        ''' <summary>
        ''' Handles the code of FaderBack
        ''' </summary>
        Sub FaderBack_DoWork()
            Try
                'Sanity checks for color levels
                FaderBackMinimumRedColorLevel = If(FaderBackMinimumRedColorLevel >= 0 And FaderBackMinimumRedColorLevel <= 255, FaderBackMinimumRedColorLevel, 0)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", FaderBackMinimumRedColorLevel)
                FaderBackMinimumGreenColorLevel = If(FaderBackMinimumGreenColorLevel >= 0 And FaderBackMinimumGreenColorLevel <= 255, FaderBackMinimumGreenColorLevel, 0)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", FaderBackMinimumGreenColorLevel)
                FaderBackMinimumBlueColorLevel = If(FaderBackMinimumBlueColorLevel >= 0 And FaderBackMinimumBlueColorLevel <= 255, FaderBackMinimumBlueColorLevel, 0)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", FaderBackMinimumBlueColorLevel)
                FaderBackMaximumRedColorLevel = If(FaderBackMaximumRedColorLevel >= 0 And FaderBackMaximumRedColorLevel <= 255, FaderBackMaximumRedColorLevel, 255)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", FaderBackMaximumRedColorLevel)
                FaderBackMaximumGreenColorLevel = If(FaderBackMaximumGreenColorLevel >= 0 And FaderBackMaximumGreenColorLevel <= 255, FaderBackMaximumGreenColorLevel, 255)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", FaderBackMaximumGreenColorLevel)
                FaderBackMaximumBlueColorLevel = If(FaderBackMaximumBlueColorLevel >= 0 And FaderBackMaximumBlueColorLevel <= 255, FaderBackMaximumBlueColorLevel, 255)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", FaderBackMaximumBlueColorLevel)

                'Variables
                Dim RandomDriver As New Random()
                Dim RedColorNum As Integer = RandomDriver.Next(FaderBackMinimumRedColorLevel, FaderBackMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(FaderBackMinimumGreenColorLevel, FaderBackMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(FaderBackMinimumBlueColorLevel, FaderBackMaximumBlueColorLevel)
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
                    'Set thresholds
                    Dim ThresholdRed As Double = RedColorNum / FaderBackMaxSteps
                    Dim ThresholdGreen As Double = GreenColorNum / FaderBackMaxSteps
                    Dim ThresholdBlue As Double = BlueColorNum / FaderBackMaxSteps
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2}) using {3} steps", ThresholdRed, ThresholdGreen, ThresholdBlue, FaderBackMaxSteps)

                    'Fade in
                    Dim CurrentColorRedIn As Integer = 0
                    Dim CurrentColorGreenIn As Integer = 0
                    Dim CurrentColorBlueIn As Integer = 0
                    For CurrentStep As Integer = 1 To FaderBackMaxSteps
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If ResizeSyncing Then Exit For
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
                    If Not ResizeSyncing Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", FaderBackFadeOutDelay)
                        SleepNoBlock(FaderBackFadeOutDelay, FaderBack)
                    End If

                    'Fade out
                    For CurrentStep As Integer = 1 To FaderBackMaxSteps
                        If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                        If ResizeSyncing Then Exit For
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
                    RedColorNum = RandomDriver.Next(FaderBackMinimumRedColorLevel, FaderBackMaximumRedColorLevel)
                    GreenColorNum = RandomDriver.Next(FaderBackMinimumGreenColorLevel, FaderBackMaximumGreenColorLevel)
                    BlueColorNum = RandomDriver.Next(FaderBackMinimumBlueColorLevel, FaderBackMaximumBlueColorLevel)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(FaderBackDelay, FaderBack)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace
