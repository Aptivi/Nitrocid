
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
    Public Module BeatFaderSettings

        Private _beatFader255Colors As Boolean
        Private _beatFaderTrueColor As Boolean = True
        Private _beatFaderCycleColors As Boolean = True
        Private _beatFaderBeatColor As String = 17
        Private _beatFaderDelay As Integer = 120
        Private _beatFaderMaxSteps As Integer = 25
        Private _beatFaderMinimumRedColorLevel As Integer = 0
        Private _beatFaderMinimumGreenColorLevel As Integer = 0
        Private _beatFaderMinimumBlueColorLevel As Integer = 0
        Private _beatFaderMinimumColorLevel As Integer = 0
        Private _beatFaderMaximumRedColorLevel As Integer = 255
        Private _beatFaderMaximumGreenColorLevel As Integer = 255
        Private _beatFaderMaximumBlueColorLevel As Integer = 255
        Private _beatFaderMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [BeatFader] Enable 255 color support. Has a higher priority than 16 color support. Please note that it only works if color cycling is enabled.
        ''' </summary>
        Public Property BeatFader255Colors As Boolean
            Get
                Return _beatFader255Colors
            End Get
            Set(value As Boolean)
                _beatFader255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] Enable truecolor support. Has a higher priority than 255 color support. Please note that it only works if color cycling is enabled.
        ''' </summary>
        Public Property BeatFaderTrueColor As Boolean
            Get
                Return _beatFaderTrueColor
            End Get
            Set(value As Boolean)
                _beatFaderTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] Enable color cycling (uses RNG. If disabled, uses the <see cref="BeatFaderBeatColor"/> color.)
        ''' </summary>
        Public Property BeatFaderCycleColors As Boolean
            Get
                Return _beatFaderCycleColors
            End Get
            Set(value As Boolean)
                _beatFaderCycleColors = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The color of beats. It can be 1-16, 1-255, or "1-255;1-255;1-255".
        ''' </summary>
        Public Property BeatFaderBeatColor As String
            Get
                Return _beatFaderBeatColor
            End Get
            Set(value As String)
                _beatFaderBeatColor = New Color(value).PlainSequence
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] How many beats per minute to wait before making the next write?
        ''' </summary>
        Public Property BeatFaderDelay As Integer
            Get
                Return _beatFaderDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 120
                _beatFaderDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] How many fade steps to do?
        ''' </summary>
        Public Property BeatFaderMaxSteps As Integer
            Get
                Return _beatFaderMaxSteps
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 25
                _beatFaderMaxSteps = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The minimum red color level (true color)
        ''' </summary>
        Public Property BeatFaderMinimumRedColorLevel As Integer
            Get
                Return _beatFaderMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatFaderMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The minimum green color level (true color)
        ''' </summary>
        Public Property BeatFaderMinimumGreenColorLevel As Integer
            Get
                Return _beatFaderMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatFaderMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The minimum blue color level (true color)
        ''' </summary>
        Public Property BeatFaderMinimumBlueColorLevel As Integer
            Get
                Return _beatFaderMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _beatFaderMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BeatFaderMinimumColorLevel As Integer
            Get
                Return _beatFaderMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_beatFader255Colors Or _beatFaderTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _beatFaderMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The maximum red color level (true color)
        ''' </summary>
        Public Property BeatFaderMaximumRedColorLevel As Integer
            Get
                Return _beatFaderMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatFaderMinimumRedColorLevel Then value = _beatFaderMinimumRedColorLevel
                If value > 255 Then value = 255
                _beatFaderMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The maximum green color level (true color)
        ''' </summary>
        Public Property BeatFaderMaximumGreenColorLevel As Integer
            Get
                Return _beatFaderMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatFaderMinimumGreenColorLevel Then value = _beatFaderMinimumGreenColorLevel
                If value > 255 Then value = 255
                _beatFaderMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The maximum blue color level (true color)
        ''' </summary>
        Public Property BeatFaderMaximumBlueColorLevel As Integer
            Get
                Return _beatFaderMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _beatFaderMinimumBlueColorLevel Then value = _beatFaderMinimumBlueColorLevel
                If value > 255 Then value = 255
                _beatFaderMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [BeatFader] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property BeatFaderMaximumColorLevel As Integer
            Get
                Return _beatFaderMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_beatFader255Colors Or _beatFaderTrueColor, 255, 15)
                If value <= _beatFaderMinimumColorLevel Then value = _beatFaderMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _beatFaderMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class BeatFaderDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "BeatFader" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            Dim BeatInterval As Integer = 60000 / BeatFaderDelay
            Dim BeatIntervalStep As Integer = BeatInterval / BeatFaderMaxSteps
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1}", BeatFaderDelay, BeatInterval)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Beat steps: {0} ms", BeatFaderDelay, BeatIntervalStep)
            SleepNoBlock(BeatIntervalStep, ScreensaverDisplayerThread)

            'If we're cycling colors, set them. Else, use the user-provided color
            Dim RedColorNum, GreenColorNum, BlueColorNum As Integer
            If BeatFaderCycleColors Then
                'We're cycling. Select the color mode, starting from true color
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors...")
                If BeatFaderTrueColor Then
                    RedColorNum = RandomDriver.Next(BeatFaderMinimumRedColorLevel, BeatFaderMinimumRedColorLevel)
                    GreenColorNum = RandomDriver.Next(BeatFaderMinimumGreenColorLevel, BeatFaderMaximumGreenColorLevel)
                    BlueColorNum = RandomDriver.Next(BeatFaderMinimumBlueColorLevel, BeatFaderMaximumBlueColorLevel)
                ElseIf BeatFader255Colors Then
                    Dim ConsoleColor As New ConsoleColorsInfo(RandomDriver.Next(BeatFaderMinimumColorLevel, BeatFaderMaximumColorLevel))
                    RedColorNum = ConsoleColor.R
                    GreenColorNum = ConsoleColor.G
                    BlueColorNum = ConsoleColor.B
                Else
                    Dim ConsoleColor As New ConsoleColorsInfo(RandomDriver.Next(BeatFaderMinimumColorLevel, BeatFaderMaximumColorLevel))
                    RedColorNum = ConsoleColor.R
                    GreenColorNum = ConsoleColor.G
                    BlueColorNum = ConsoleColor.B
                End If
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
            Else
                'We're not cycling. Parse the color and then select the color mode, starting from true color
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Parsing colors... {0}", BeatFaderBeatColor)
                Dim UserColor As New Color(BeatFaderBeatColor)
                If UserColor.Type = ColorType.TrueColor Then
                    RedColorNum = UserColor.R
                    GreenColorNum = UserColor.G
                    BlueColorNum = UserColor.B
                ElseIf UserColor.Type = ColorType._255Color Then
                    Dim ConsoleColor As New ConsoleColorsInfo(UserColor.PlainSequence)
                    RedColorNum = ConsoleColor.R
                    GreenColorNum = ConsoleColor.G
                    BlueColorNum = ConsoleColor.B
                End If
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
            End If

            'Set thresholds
            Dim ThresholdRed As Double = RedColorNum / BeatFaderMaxSteps
            Dim ThresholdGreen As Double = GreenColorNum / BeatFaderMaxSteps
            Dim ThresholdBlue As Double = BlueColorNum / BeatFaderMaxSteps
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color threshold (R;G;B: {0};{1};{2})", ThresholdRed, ThresholdGreen, ThresholdBlue)

            'Fade out
            For CurrentStep As Integer = 1 To BeatFaderMaxSteps
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Step {0}/{1} each {2} ms", CurrentStep, BeatFaderMaxSteps, BeatIntervalStep)
                SleepNoBlock(BeatIntervalStep, ScreensaverDisplayerThread)
                Dim CurrentColorRedOut As Integer = RedColorNum - ThresholdRed * CurrentStep
                Dim CurrentColorGreenOut As Integer = GreenColorNum - ThresholdGreen * CurrentStep
                Dim CurrentColorBlueOut As Integer = BlueColorNum - ThresholdBlue * CurrentStep
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color out (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                SetConsoleColor(New Color($"{CurrentColorRedOut};{CurrentColorGreenOut};{CurrentColorBlueOut}"), True)
                Console.Clear()
            Next

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
        End Sub

    End Class
End Namespace
