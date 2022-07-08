
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
    Public Module FaderSettings

        Private _faderDelay As Integer = 50
        Private _faderFadeOutDelay As Integer = 3000
        Private _faderWrite As String = "Kernel Simulator"
        Private _faderMaxSteps As Integer = 25
        Private _faderBackgroundColor As String = New Color(ConsoleColor.Black).PlainSequence
        Private _faderMinimumRedColorLevel As Integer = 0
        Private _faderMinimumGreenColorLevel As Integer = 0
        Private _faderMinimumBlueColorLevel As Integer = 0
        Private _faderMaximumRedColorLevel As Integer = 255
        Private _faderMaximumGreenColorLevel As Integer = 255
        Private _faderMaximumBlueColorLevel As Integer = 255

        ''' <summary>
        ''' [Fader] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property FaderDelay As Integer
            Get
                Return _faderDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _faderDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] How many milliseconds to wait before fading the text out?
        ''' </summary>
        Public Property FaderFadeOutDelay As Integer
            Get
                Return _faderFadeOutDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3000
                _faderFadeOutDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] Text for Fader. Shorter is better.
        ''' </summary>
        Public Property FaderWrite As String
            Get
                Return _faderWrite
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _faderWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] How many fade steps to do?
        ''' </summary>
        Public Property FaderMaxSteps As Integer
            Get
                Return _faderMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _faderMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] Screensaver background color
        ''' </summary>
        Public Property FaderBackgroundColor As String
            Get
                Return _faderBackgroundColor
            End Get
            Set(value As String)
                _faderBackgroundColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The minimum red color level (true color)
        ''' </summary>
        Public Property FaderMinimumRedColorLevel As Integer
            Get
                Return _faderMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The minimum green color level (true color)
        ''' </summary>
        Public Property FaderMinimumGreenColorLevel As Integer
            Get
                Return _faderMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The minimum blue color level (true color)
        ''' </summary>
        Public Property FaderMinimumBlueColorLevel As Integer
            Get
                Return _faderMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _faderMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The maximum red color level (true color)
        ''' </summary>
        Public Property FaderMaximumRedColorLevel As Integer
            Get
                Return _faderMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderMinimumRedColorLevel Then value = _faderMinimumRedColorLevel
                If value > 255 Then value = 255
                _faderMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The maximum green color level (true color)
        ''' </summary>
        Public Property FaderMaximumGreenColorLevel As Integer
            Get
                Return _faderMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderMinimumGreenColorLevel Then value = _faderMinimumGreenColorLevel
                If value > 255 Then value = 255
                _faderMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Fader] The maximum blue color level (true color)
        ''' </summary>
        Public Property FaderMaximumBlueColorLevel As Integer
            Get
                Return _faderMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _faderMinimumBlueColorLevel Then value = _faderMinimumBlueColorLevel
                If value > 255 Then value = 255
                _faderMaximumBlueColorLevel = value
            End Set
        End Property

    End Module

    Public Class FaderDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "Fader" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SetConsoleColor(New Color(FaderBackgroundColor), True)
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Dim RedColorNum As Integer = RandomDriver.Next(FaderMinimumRedColorLevel, FaderMaximumRedColorLevel)
            Dim GreenColorNum As Integer = RandomDriver.Next(FaderMinimumGreenColorLevel, FaderMaximumGreenColorLevel)
            Dim BlueColorNum As Integer = RandomDriver.Next(FaderMinimumBlueColorLevel, FaderMaximumBlueColorLevel)

            Console.CursorVisible = False
            Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
            Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
            If FaderWrite.Length + Left >= Console.WindowWidth Then
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Text length of {0} exceeded window width of {1}.", FaderWrite.Length + Left, Console.WindowWidth)
                Left -= FaderWrite.Length + 1
            End If
            Console.SetCursorPosition(Left, Top)
            Console.BackgroundColor = ConsoleColor.Black
            ClearKeepPosition()

            'Set thresholds
            Dim ThresholdRed As Double = RedColorNum / FaderMaxSteps
            Dim ThresholdGreen As Double = GreenColorNum / FaderMaxSteps
            Dim ThresholdBlue As Double = BlueColorNum / FaderMaxSteps
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0})", ThresholdRed, ThresholdGreen, ThresholdBlue)

            'Fade in
            Dim CurrentColorRedIn As Integer = 0
            Dim CurrentColorGreenIn As Integer = 0
            Dim CurrentColorBlueIn As Integer = 0
            For CurrentStep As Integer = FaderMaxSteps To 1 Step -1
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, FaderMaxSteps)
                SleepNoBlock(FaderDelay, ScreensaverDisplayerThread)
                CurrentColorRedIn += ThresholdRed
                CurrentColorGreenIn += ThresholdGreen
                CurrentColorBlueIn += ThresholdBlue
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color in (R;G;B: {0};{1};{2})", CurrentColorRedIn, CurrentColorGreenIn, CurrentColorBlueIn)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then WriteWhere(FaderWrite, Left, Top, True, New Color(CurrentColorRedIn & ";" & CurrentColorGreenIn & ";" & CurrentColorBlueIn), New Color(ConsoleColors.Black))
            Next

            'Wait until fade out
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Waiting {0} ms...", FaderFadeOutDelay)
            SleepNoBlock(FaderFadeOutDelay, ScreensaverDisplayerThread)

            'Fade out
            For CurrentStep As Integer = 1 To FaderMaxSteps
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1}", CurrentStep, FaderMaxSteps)
                SleepNoBlock(FaderDelay, ScreensaverDisplayerThread)
                Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", CurrentColorRedOut, CurrentColorGreenOut, CurrentColorBlueOut)
                If Not ResizeSyncing Then WriteWhere(FaderWrite, Left, Top, True, New Color(CurrentColorRedOut & ";" & CurrentColorGreenOut & ";" & CurrentColorBlueOut), New Color(ConsoleColors.Black))
            Next

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(FaderDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
