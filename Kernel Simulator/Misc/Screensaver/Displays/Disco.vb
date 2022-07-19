
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

Namespace Misc.Screensaver.Displays
    Public Module DiscoSettings

        Private _disco255Colors As Boolean
        Private _discoTrueColor As Boolean = True
        Private _discoCycleColors As Boolean
        Private _discoDelay As Integer = 100
        Private _discoUseBeatsPerMinute As Boolean
        Private _discoEnableFedMode As Boolean
        Private _discoMinimumRedColorLevel As Integer = 0
        Private _discoMinimumGreenColorLevel As Integer = 0
        Private _discoMinimumBlueColorLevel As Integer = 0
        Private _discoMinimumColorLevel As Integer = 0
        Private _discoMaximumRedColorLevel As Integer = 255
        Private _discoMaximumGreenColorLevel As Integer = 255
        Private _discoMaximumBlueColorLevel As Integer = 255
        Private _discoMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Disco] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Disco255Colors As Boolean
            Get
                Return _disco255Colors
            End Get
            Set(value As Boolean)
                _disco255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property DiscoTrueColor As Boolean
            Get
                Return _discoTrueColor
            End Get
            Set(value As Boolean)
                _discoTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] Enable color cycling
        ''' </summary>
        Public Property DiscoCycleColors As Boolean
            Get
                Return _discoCycleColors
            End Get
            Set(value As Boolean)
                _discoCycleColors = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] How many milliseconds, or beats per minute, to wait before making the next write?
        ''' </summary>
        Public Property DiscoDelay As Integer
            Get
                Return _discoDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 100
                _discoDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] Whether to use the Beats Per Minute (1/4) to change the writing delay. If False, will use the standard milliseconds delay instead.
        ''' </summary>
        Public Property DiscoUseBeatsPerMinute As Boolean
            Get
                Return _discoUseBeatsPerMinute
            End Get
            Set(value As Boolean)
                _discoUseBeatsPerMinute = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] Uses the black and white cycle to produce the same effect as the legacy "fed" screensaver introduced back in v0.0.1
        ''' </summary>
        Public Property DiscoEnableFedMode As Boolean
            Get
                Return _discoEnableFedMode
            End Get
            Set(value As Boolean)
                _discoEnableFedMode = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The minimum red color level (true color)
        ''' </summary>
        Public Property DiscoMinimumRedColorLevel As Integer
            Get
                Return _discoMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _discoMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The minimum green color level (true color)
        ''' </summary>
        Public Property DiscoMinimumGreenColorLevel As Integer
            Get
                Return _discoMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _discoMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The minimum blue color level (true color)
        ''' </summary>
        Public Property DiscoMinimumBlueColorLevel As Integer
            Get
                Return _discoMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _discoMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property DiscoMinimumColorLevel As Integer
            Get
                Return _discoMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_disco255Colors Or _discoTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _discoMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The maximum red color level (true color)
        ''' </summary>
        Public Property DiscoMaximumRedColorLevel As Integer
            Get
                Return _discoMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _discoMinimumRedColorLevel Then value = _discoMinimumRedColorLevel
                If value > 255 Then value = 255
                _discoMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The maximum green color level (true color)
        ''' </summary>
        Public Property DiscoMaximumGreenColorLevel As Integer
            Get
                Return _discoMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _discoMinimumGreenColorLevel Then value = _discoMinimumGreenColorLevel
                If value > 255 Then value = 255
                _discoMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The maximum blue color level (true color)
        ''' </summary>
        Public Property DiscoMaximumBlueColorLevel As Integer
            Get
                Return _discoMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _discoMinimumBlueColorLevel Then value = _discoMinimumBlueColorLevel
                If value > 255 Then value = 255
                _discoMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Disco] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property DiscoMaximumColorLevel As Integer
            Get
                Return _discoMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_disco255Colors Or _discoTrueColor, 255, 15)
                If value <= _discoMinimumColorLevel Then value = _discoMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _discoMaximumColorLevel = value
            End Set
        End Property

    End Module
    Public Class DiscoDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentColor As Integer = 0
        Private CurrentColorR, CurrentColorG, CurrentColorB As Integer

        Public Overrides Property ScreensaverName As String = "Disco" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Dim MaximumColors As Integer = DiscoMaximumColorLevel
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", MaximumColors)
            Dim MaximumColorsR As Integer = DiscoMaximumRedColorLevel
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", MaximumColorsR)
            Dim MaximumColorsG As Integer = DiscoMaximumGreenColorLevel
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", MaximumColorsG)
            Dim MaximumColorsB As Integer = DiscoMaximumBlueColorLevel
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", MaximumColorsB)
            Dim FedColors As ConsoleColors() = {ConsoleColors.Black, ConsoleColors.White}

            Console.CursorVisible = False

            'Select the background color
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Cycling colors: {0}", DiscoCycleColors)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "fed (future-eyes-destroyer) mode: {0}", DiscoEnableFedMode)
            If Not DiscoEnableFedMode Then
                If DiscoTrueColor Then
                    If Not DiscoCycleColors Then
                        Dim RedColorNum As Integer = RandomDriver.Next(255)
                        Dim GreenColorNum As Integer = RandomDriver.Next(255)
                        Dim BlueColorNum As Integer = RandomDriver.Next(255)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                        Dim ColorStorage As New Color(RedColorNum, GreenColorNum, BlueColorNum)
                        SetConsoleColor(ColorStorage, True)
                    Else
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", CurrentColorR, CurrentColorG, CurrentColorB)
                        Dim ColorStorage As New Color(CurrentColorR, CurrentColorG, CurrentColorB)
                        SetConsoleColor(ColorStorage, True)
                    End If
                ElseIf Disco255Colors Then
                    If Not DiscoCycleColors Then
                        Dim color As Integer = RandomDriver.Next(255)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", color)
                        SetConsoleColor(New Color(color), True)
                    Else
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", CurrentColor)
                        SetConsoleColor(New Color(CurrentColor), True)
                    End If
                Else
                    If Not DiscoCycleColors Then
                        Console.BackgroundColor = colors(RandomDriver.Next(colors.Length - 1))
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                    Else
                        MaximumColors = If(DiscoMaximumColorLevel >= 0 And DiscoMaximumColorLevel <= 15, DiscoMaximumColorLevel, 15)
                        Console.BackgroundColor = colors(CurrentColor)
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                    End If
                End If
            Else
                If CurrentColor = ConsoleColors.Black Then
                    CurrentColor = ConsoleColors.White
                Else
                    CurrentColor = ConsoleColors.Black
                End If
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", CurrentColor)
                SetConsoleColor(New Color(CurrentColor), True)
            End If

            'Make the disco effect!
            Console.Clear()

            'Switch to the next color
            If DiscoTrueColor Then
                If CurrentColorR >= MaximumColorsR Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Red level exceeded maximum color. {0} >= {1}", CurrentColorR, MaximumColorsR)
                    CurrentColorR = 0
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Stepping one (R)...")
                    CurrentColorR += 1
                End If
                If CurrentColorG >= MaximumColorsG Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Green level exceeded maximum color. {0} >= {1}", CurrentColorG, MaximumColorsG)
                    CurrentColorG = 0
                ElseIf CurrentColorR = 0 Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Stepping one (G)...")
                    CurrentColorG += 1
                End If
                If CurrentColorB >= MaximumColorsB Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Blue level exceeded maximum color. {0} >= {1}", CurrentColorB, MaximumColorsB)
                    CurrentColorB = 0
                ElseIf CurrentColorG = 0 And CurrentColorR = 0 Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Stepping one (B)...")
                    CurrentColorB += 1
                End If
                If CurrentColorB = 0 And CurrentColorG = 0 And CurrentColorR = 0 Then
                    CurrentColorB = 0
                    CurrentColorG = 0
                    CurrentColorR = 0
                End If
            Else
                If CurrentColor >= MaximumColors Then
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Color level exceeded maximum color. {0} >= {1}", CurrentColor, MaximumColors)
                    CurrentColor = 0
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Stepping one...")
                    CurrentColor += 1
                End If
            End If

            'Check to see if we're dealing with beats per minute
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Using BPM: {0}", DiscoUseBeatsPerMinute)
            If DiscoUseBeatsPerMinute Then
                Dim BeatInterval As Integer = 60000 / DiscoDelay
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1} ms", DiscoDelay, BeatInterval)
                SleepNoBlock(BeatInterval, ScreensaverDisplayerThread)
            Else
                SleepNoBlock(DiscoDelay, ScreensaverDisplayerThread)
            End If
        End Sub

    End Class
End Namespace
